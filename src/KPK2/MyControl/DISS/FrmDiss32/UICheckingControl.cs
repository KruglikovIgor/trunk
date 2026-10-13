using Sparc.KPK2.NFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using KPK2.Properties;
using Sparc.KPK2.NFunctions.Enums;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.MyClasseUSB;
using System.Threading;
using KPK2;
using KPK2.Common.DissFunctions;
using KPK2.DB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.LTR;
using Globals = Sparc.KPK2.NFunctions.Globals;
using UserControlsKPK2;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.Imitator;
using KPK2.Common;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel;
using KPK2.Common.TokKFunction.OneChannel.Events;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.MyClasse;
using LCards;
using NLog;
using KPK2.Interfaces;
using CommonGlobals = KPK2.Common.Globals;

namespace KPK2.MyControl.DISS.FrmDiss32
{
    public partial class UICheckingControl : UserControl, IUICheckingControl
    {
		private const double PowerSupply27 = 27.5;
		private const double MaxCurrent115 = 5;
		private readonly double _maxCurrent27 = 15;
		private const int PowerSupply115Phase = 0;

		private int countButton;
		private MeasurementPerformance _state;
		private List<RadioButton> chButtons = new List<RadioButton>();
		private ITypeChecker _checker;
		private int curTok_K = 0;
		private DataBaseKPK2 _dbKPK2;
		private Mode _mode;
		private MeasuringValueInfo _measValueInfo;
		private Button[] _arButton = new Button[7];
		private Modification _modification;
		private DevicesID _devicesId;
		private Measure _measure;
		private List<USBBaseNew> _measList;
		private Imitator.Device _device;
		private ushort _currentDirection = 0;
		private string[] specSignal = new[] { "Delta_15V_A", "Minus_15V", "Plus_15V" };
		private Control _activeControl;
		private bool _dissStopped = true;
		private bool _isOneChannel;
		public event EventHandler EventOnHighFlat;
		public event EventHandler EventOnHighStandard;
		public event EventHandler EventHighEnable;
		public event EventHandler EventTurnOnHighIfOff;
		public event Action<object, double> EventIndicatorPowerU;
		public event Action<object, double> EventIndicatorPowerI;
		public event Action<object, string> EventErrorDetected;

		public ICurTok CurTok { get; private set; }

		public bool IsOneChannel 
		{ get 
			{
				return _isOneChannel;
			}
			set 
			{
				_isOneChannel = value;
				measurementsControl1.IsOneChannel = value;
			} 
		}

		public bool IsButtonElecResistSelect { get; set; } = false;

		public MeasurementPerformance MeasurementPerformance 
		{ 
			get => _state;
			set
            {
                if (value == null)
                    return;
                value.Initialize(gbAll);
                _state = value;
			}
		}

		public UICheckingControl() //todo передавать состояние проверок или передовать какие проверку будут проходить
		{
			InitializeComponent();
            if (this.DesignMode)
            {
                return;
            }
            Load += UICheckingControl_Load;
		}

		public void TurnOffButtons()
		{
			foreach (var item in _arButton)
				if (item.FlatStyle == FlatStyle.Flat)
				{
					item.PerformClick();
					return;
				}

			if(measurementsControl1.IsLoad)
				measurementsControl1.TurnOffButtonsTask();
		}

		private void ParentForm_EventShowStrip(object arg1, double[] arg2)
		{
			if (bFirstPitan.FlatStyle == FlatStyle.Flat)
			{
				SetIndicatorValue(4, arg2[2]);
				SetIndicatorValue(8, arg2[1]);
				SetIndicatorValue(12, arg2[3]);
			}
		}
		
		private void UICheckingControl_Load(object sender, EventArgs e)
		{
            if (this.DesignMode)
                return;

			CurTok = new CurTok0();
			CurTok.EventExecuting += CurTok_EventExecuting;

			var settings = SettingsObj.GetSettings();

			InitialiseChannelButtons();

			_mode = settings.Mode;
			_measure = settings.Measure;
			_dbKPK2 = settings.DBKPK2;
			_devicesId = settings.DevicesId;
			_modification = settings.Modification;
			_measList = settings.MeasList;
			_measValueInfo = settings.MeasValueInfo;
			measurementsControl1.Measure = _measure;

            if (_devicesId == DevicesID.Diss32_90 && _modification != Modification.Diss32_90A)
                ReconfigureVisualDiss32_90(@"""ОТКЛ Бл 8 О"", ""ОТКЛ Бл 8 Г""");
            if (_devicesId == DevicesID.Diss32 || _devicesId == DevicesID.Diss32_WorkTest)
            {
                ReconfigureVisualDiss32("");
            }

			if (_mode.ModeApplication == ModeApplication.DefaultCheck)
			{
				_checker = new StandardChecker();//todo изменить 
				btnInsulationResist.Visible = false;
				btnFreq.Visible = false;
			}
			else if(_mode.ModeApplication == ModeApplication.RepairCheck)
			{
				_checker = new RepairChecker();
			}
			_checker.Initialize(SettingsObj.GetSettings().TestId, _modification, _devicesId, IsOneChannel);

			_measure.ResultsReady += ShowResult;
			_measure.ErrorOnLTR += ShowLTRError;

			if (Globals.Emulation)
				_device = measurementsControl1.Device = new Imitator.Diss32();

			if (_devicesId == DevicesID.Diss32_90 && _modification != Modification.Diss32_90A)
			{
				SegmentIndicatorControl[] indicators = new SegmentIndicatorControl[1];
				SegmentIndicatorControl indicator;
				for (int i = 0; i < indicators.Length; i++)
				{
					indicator = indicators[i] = new SegmentIndicatorControl();
					indicator.SetValue(_checker.LoadModificationDiss3290.Viewers[i]);
					_measList.Add(_checker.LoadModificationDiss3290.Viewers[i].UsbBaseNew);
				}
				_measure.AddMeasureList(_measList);
				_measure.HoldCurrentSignals();
			}

			if (Power115V.Instance.IsAktor)
			{
				SegmentIndicatorControl[] indicators = new SegmentIndicatorControl[_checker.LoadModificationDiss3290.Viewers.Count];
				SegmentIndicatorControl indicator;
				for (int i = 0; i < indicators.Length; i++)
				{
					indicator = indicators[i] = new SegmentIndicatorControl();
					indicator.SetValue(_checker.LoadPower115VActor.Viewers[i]);
					_measList.Add(_checker.LoadModificationDiss3290.Viewers[i].UsbBaseNew);
				}
				_measure.AddMeasureList(_measList);
				_measure.HoldCurrentSignals();
			}

			if (IsOneChannel)
				DataManager.Instance.SetAddParam(SettingsObj.GetSettings().TestId, Globals.TypeDiss32Param, "1");

			((frmWork32)ParentForm).EventShowStrip += ParentForm_EventShowStrip;
			_currentDirection = (ushort)CommonSettings.Default.AddFlyDirectionDiss32;

			_arButton[0] = bFirstPitan;
			_arButton[1] = bSecPitan;
			_arButton[2] = btn_GLPD_MOD;
			_arButton[3] = btn_Tok_K;
			_arButton[4] = btnSD;
			_arButton[5] = btnNoise;
			_arButton[6] = btnFreq;

			measurementsControl1.DevicesId = SettingsObj.GetSettings().DevicesId;
			measurementsControl1.MeasuringValueInfo = SettingsObj.GetSettings().MeasValueInfo;
			measurementsControl1.Modification = SettingsObj.GetSettings().Modification;
			measurementsControl1.TestTasks = new TestTasks(
				SettingsObj.GetSettings().TestId,
				SettingsObj.GetSettings().IsOneChannel,
				SettingsObj.GetSettings().DevicesId,
				SettingsObj.GetSettings().Modification);
		}

		public void DisableButton()
		{
			if (countButton > 0) // есть нажатые кнопки
			{
				if (tabControl.SelectedTab.Name == "pageWork")
					for (int i = 0; i < _arButton.Length; i++)
						if (_arButton[i].FlatStyle == FlatStyle.Flat)
						{
							if (_arButton[i] == btn_Tok_K)
								curTok_K = 3; // откл. измерение "Ток К" 
							_arButton[i].PerformClick();
						}
				if (tabControl.SelectedTab.Name == "pageTask")//todo вставить кнопку из формы MeasuringControl
					measurementsControl1.DisableButton();
			}
		}

		public void ChangeTab(string namePage)
		{
			tabControl.SelectTab(namePage);
		}
		
		private void CurTok_EventExecuting(object sender, EventImageTok e)
		{
			if (sender is CurTok3)
			{
				ImageTok_K();
			}

			if (sender is CurTok0)
			{
				EventOnHighFlat?.Invoke(this, EventArgs.Empty);
				ImageTok_K_Off();
			}

			if (sender is CurTok1)
			{
				EventOnHighStandard?.Invoke(this, EventArgs.Empty);
			}
			gbAll.Text = e.Name;
			_state.SetIndicatorsVisibility(e.CheckType, IsOneChannel);
		}

		public void Start(MeasurementPerformance state, bool isOneChannel)
		{
			_state = state;
			_checker.IsOneChannel = isOneChannel;
			_state.EventIndicatorPowerI += EventIndicatorPowerI;
			_state.EventIndicatorPowerU += EventIndicatorPowerU;
			_state.Initialize(gbAll);
			IsOneChannel = isOneChannel;
			_measure.StartMeasuring();
		}

		public void Stop()
		{
			ImageStop();
		}

		public void ImageOpenForm()
		{
			pDefaultValues.Visible = true;
			gbAll.Text = "";
			VisibleFrequency(false);
		}

		#region Проведение измерений

		#region блок питания 27в и 115в
		private void btn_FirstPitan_Click(object sender, EventArgs e)
		{
			bFirstPitan.Enabled = false;
			if (bFirstPitan.FlatStyle == FlatStyle.Standard)
			{
				PressButton(bFirstPitan);
				ImageFirstPitanie();
				_state.ViewIndicators(_checker.FirstPitenIndicatorViewer);
				_state.StartWorker();
			}
			else
			{
				_state.StopWorker();
				ImageStop();
				string nameWC27VI = _modification == Modification.Diss32_90A? "Plus_27V_32(90A)_I" : "Plus_27V_32_I";
				string nameWC115VI = _modification == Modification.Diss32_90A ? "Plus_115V_32(90A)_I" : "Plus_115V_32_I";
				if (!DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, 
					    "115V_Read_U", _state.GetIndicatorValue(4)))
					EventErrorDetected?.Invoke(this, Resources.CantSaveDbParam); //todo добавить события 
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, nameWC27VI, _state.GetIndicatorValue(8));
					DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, nameWC115VI, _state.GetIndicatorValue(12));
				}
                _measList.Clear();
				WrungButton(bFirstPitan);
			}
			bFirstPitan.Enabled = true;
		}

		private void ImageFirstPitanie()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.PSParams;
			_state.SetIndicatorsVisibility(CheckType.FirstPitanie, IsOneChannel);
			// регулировка новых БП на 27В и 115В 
			grpBox_27V.Visible = grpBox_115V.Visible = true;
		}
		#endregion

		#region Вторичные источники питания

		private void btn_SecPitan_Click(object sender, EventArgs e)
		{
			bSecPitan.Enabled = false;
			if (bSecPitan.FlatStyle == FlatStyle.Standard)
			{
				PressButton(bSecPitan);
				ImageSecPitanie();
				_checker.InitializeSecPitanIndicatorViewerDiss32(SettingsObj.GetSettings().TestId);
				_state.ViewIndicators(_checker.SecPitanIndicatorViewer);
				_state.StartWorker();
			}
			else
			{
				_state.StopWorker();
                _measList.Clear();
				ImageStop();
				WrungButton(bSecPitan);
			}
			bSecPitan.Enabled = true;
		}

		private void ImageSecPitanie()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.SecondaryPSVoltage;
			_state.SetIndicatorsVisibility(CheckType.SecPower, IsOneChannel);
		}
		#endregion

		#region ГЛПД и МОД

		private void btn_GLPD_MOD_Click(object sender, EventArgs e)
		{
			btn_GLPD_MOD.Enabled = false;
			if (btn_GLPD_MOD.FlatStyle == FlatStyle.Standard)
			{
				PressButton(btn_GLPD_MOD);
				ImageGLPD();
				_state.ViewIndicators(_checker.GLPDIndicatorViewer);
				_state.StartWorker();
				if (!DiscreteOutputs.Instance().SetOutput("TK_32", true))
					EventErrorDetected?.Invoke(this,Resources.TKOn); //CommandError
			}
			else
			{
				string parameterNameFrequency = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck 
					? "Mnl_F_32_WorkTest" : "Mnl_F_32";
				VisibleFrequency(false);
				_state.StopWorker();
				ImageStop();
				if (!DiscreteOutputs.Instance().SetOutput("TK_32", false))
					EventErrorDetected?.Invoke(this, Resources.TKOff);

				if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
					if (!ManualRecordDouble(_checker.GLPDIndicatorViewer.CheckId, parameterNameFrequency, ivFrequency.Value))
						EventErrorDetected?.Invoke(this, Resources.CantSaveDbParam);
				WrungButton(btn_GLPD_MOD);
			}
			btn_GLPD_MOD.Enabled = true;
		}

		private void ImageGLPD()
		{
			string parameterNameFrequency = "Mnl_F_32";
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.GLPDParams;
			USBBaseNew signal = new USBBaseNew((int)_devicesId, parameterNameFrequency, 0);
			if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck) { 
			ivFrequency.MinGreenValue = signal.Min;
			ivFrequency.MaxGreenValue = signal.Max;
			VisibleFrequency(true);
			ivFrequency.Value = double.NaN;}
			_state.SetIndicatorsVisibility(CheckType.GLPD, IsOneChannel);
		}

		#endregion

		#region ТОК К

		private void btn_Tok_K_Click(object sender, EventArgs e)
		{
			btn_Tok_K.Enabled = false;
			if (btn_Tok_K.FlatStyle == FlatStyle.Standard)
			{
				PressButton(btn_Tok_K);
				ImageTok_K_Off();
				// выключаем "Высокое"
				EventOnHighFlat?.Invoke(this, EventArgs.Empty);
				Thread.Sleep(50);
				if(!IsOneChannel)
					_state.ViewIndicators(_checker.OffTokKIndicatorViewer);
				if (!DiscreteOutputs.Instance().SetOutput("TK_32", true))
					EventErrorDetected?.Invoke(this, Resources.TKOn);//CommandError
				if (IsOneChannel)
				{
					curTok_K = 3;
					ShowChannelButtons(true);
					CurTok.EventExecuting += CurTok_EventExecuting;
				}
				else
				{
					curTok_K = 1;
					_state.StartWorker();
				}
			}
			else
			{//todo исправить конструкцию
				if (curTok_K == 1) // измеряем при включенном "Высокое"
					TokKStep2();
				else if (curTok_K == 2) // расчет дельта U и абс.дельта U
					TokKStep3();
				else if (curTok_K == 3) // заканчиваем измерения
				{
					_state.StopWorker(false);
					ImageStop();
					EventHighEnable?.Invoke(this, EventArgs.Empty);//TODO не забыть про кнопку вкл высокого
					if (!DiscreteOutputs.Instance().SetOutput("TK_32", false))
						EventErrorDetected?.Invoke(this, Resources.TKOff);//commandError
					if (IsOneChannel)
						ShowChannelButtons(false);
					WrungButton(btn_Tok_K);
					SettingsObj.GetSettings().MeasList.Clear();
				}
				else // на всякий случай
					ImageStop();
			}
			btn_Tok_K.Enabled = true;
		}
		
		private void btnSD_Click(object sender, EventArgs e)
		{
			btnSD.Enabled = false;
			if (btnSD.FlatStyle == FlatStyle.Standard)
			{
				PressButton(btnSD);
				ImageSD();
				if (!DiscreteOutputs.Instance().SetOutput("TK_32", true)) // "Токи кристаллов"
					EventErrorDetected?.Invoke(this, Resources.TKOn);//commandError
                if (IsOneChannel)
                {
                    ShowChannelButtons(true);
                }
				_state.ViewIndicators(_checker.CdIndicatorViewer);
				_state.StartWorker();
				_state.CurrentParam = "SD_{0}_{1}";
			}
			else
			{
				if (IsOneChannel)
				{
					_state.StopWorker(false);
					SaveOneChannelParams(_state.CurrentParam, _state.ChannelState);
				}
				else
					_state.StopWorker();
				SettingsObj.GetSettings().MeasList.Clear();
				if (!DiscreteOutputs.Instance().SetOutput("TK_32", false)) // "Токи кристаллов"
					EventErrorDetected?.Invoke(this, Resources.TKOff);
				ShowChannelButtons(false);
				ImageStop();//commandError
				WrungButton(btnSD);
			}
			btnSD.Enabled = true;
		}

		private void btnNoise_Click(object sender, EventArgs e)
		{
			btnNoise.Enabled = false;
			if (btnNoise.FlatStyle == FlatStyle.Standard)
			{
				PressButton(btnNoise);
				ImageNoise();
				DiscreteOutputs.Instance().SetOutput("TK_32", false);
				EventTurnOnHighIfOff?.Invoke(this, EventArgs.Empty);
                if (IsOneChannel)
                {
                    //_checker.InitializeNoise32(TestId);
                    ShowChannelButtons(true);
                }
                _state.ViewIndicators(_checker.NoiseIndicatorViewer);
				_state.StartWorker();
				_state.CurrentParam = "UNCh_{0}_{1}_Noise32";
			}
			else
			{
				if (IsOneChannel)
				{
					_state.StopWorker(false);
					SaveOneChannelParams(_state.CurrentParam, _state.ChannelState);
				}
				else
					_state.StopWorker();
                _measList.Clear();
				ShowChannelButtons(false);
				ImageStop();
				WrungButton(btnNoise);
			}
			btnNoise.Enabled = true;
		}

		private void ImageNoise()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.NoiseVoltages;
			// индикаторы и подписи - видимы/не видимы

			_state.SetIndicatorsVisibility(CheckType.Noise, IsOneChannel);
		}

		private void ImageSD()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.VoltageShiftOnSD;
			_state.SetIndicatorsVisibility(CheckType.SD, IsOneChannel);
		}

		private void ImageTok_K_Off()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.VoltagesKCurrentsHighOff;
			_state.SetIndicatorsVisibility(CheckType.Tok_K_Off, IsOneChannel);
		}

		private void ImageTok_K_On()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.VoltagesKCurrentHighOn;
			_state.SetIndicatorsVisibility(CheckType.Tok_K_On, IsOneChannel);
		}

		private void ImageTok_K()
		{
			pDefaultValues.Visible = false;
			gbAll.Text = Resources.MeasVoltageAfterSVChOn;
			_state.SetIndicatorsVisibility(CheckType.Tok_K, IsOneChannel);
		}

		private void TokKStep2()
		{
			_state.SaveAll(SettingsObj.GetSettings().MeasList);
			pDefaultValues.Visible = false;
			_state.StopWorker(!IsOneChannel);
			EventOnHighStandard?.Invoke(this, EventArgs.Empty);
			SettingsObj.GetSettings().MeasList.Clear();
			_state.ViewIndicators(_checker.OnTokKIndicatorViewer);
			_state.StartWorker();
			ImageStop();
			ImageTok_K_On();
			curTok_K = 2;
		}

		private void TokKStep3()
		{
			pDefaultValues.Visible = false;
			int group1Count = 3;
			const int group2Count = 2;
			int idInd;
			double result;
			string parameterDelta = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck
				? "I_K_{0}_{1}_Delta_WorkTest"
				: "I_K_{0}_{1}_Delta";
			_state.CalcDelta(SettingsObj.GetSettings().MeasList);
			_state.StopWorker(false);
			ImageTok_K();
			_checker.InitializeDeltaAbsTok();
			_state.ViewIndicators(_checker.DeltaAbsTokIndicatorViewer);
			SettingsObj.GetSettings().MeasList.Clear();
			for (int group1 = 1; group1 <= group1Count; group1++)
				for (int group2 = 1; group2 <= group2Count; group2++)
				{
					idInd = (group1 - 1) * group1Count + group2 - 1;
					result = _state.DeltaResults[(group1 - 1) * (group1Count - 1) + group2 - 1];
					_state.SetIndicatorValue(idInd,  result);
					DataManager.Instance.
						SaveParameter
							(SettingsObj.GetSettings().TestId,
							string.Format(parameterDelta, group1, group2),
							result);
				}

			for (int i = 0; i < 3; i++) // определяем Abs 
			{
				idInd = i * 3 + 9 - 1;
				result = Math.Abs(_state.DeltaResults[i * 2] - _state.DeltaResults[i * 2 + 1]);
				_state.SetIndicatorValue(idInd, result);
				DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, string.Format("I_K_{0}_Abs", i + 1), result);
			}

			curTok_K = 3;
		}

		#endregion
		
		private void btnInsulationResist_Click(object sender, EventArgs e)
		{
			btnInsulationResist.Enabled = false;
			if (btnInsulationResist.FlatStyle == FlatStyle.Standard)
			{
				IsButtonElecResistSelect = true;
                btnInsulationResist.FlatStyle = FlatStyle.Flat;
                btnInsulationResist.BackColor = Color.FromArgb(153, 249, 164);
                ImageInsulationResist();
			}
			else
			{
				IsButtonElecResistSelect = false;
				SaveElecInsulationResistanceControl();
                btnInsulationResist.FlatStyle = FlatStyle.Standard;
                btnInsulationResist.BackColor = Color.Transparent;
                gbAll.Controls.Remove(_activeControl);
			}
			btnInsulationResist.Enabled = true;
        }

        private void ImageInsulationResist()
        {
            pDefaultValues.Visible = false;
            _activeControl = new ElecInsulationResistanceControl(_checker, _modification);
            gbAll.Controls.Add(_activeControl);
        }

        private void btnFreq_Click(object sender, EventArgs e)
        {
			if(btnFreq.FlatStyle == FlatStyle.Standard)
			{
				PressButton(btnFreq);
				ImageFreq();
			}
			else
            {
                gbAll.Size = new Size(gbAll.Size.Width, gbAll.Size.Height - 50);
				if(_activeControl is SVChFreqAndPowerDiss32)
				{
					((SVChFreqAndPowerDiss32)_activeControl).Save();
					((SVChFreqAndPowerDiss32)_activeControl).Stop();
					((SVChFreqAndPowerDiss32)_activeControl).Close();
                }
                gbAll.Controls.Remove(_activeControl);
				_activeControl = null;
				ImageStop();
				WrungButton(btnFreq);
			}
        }

		private void ImageFreq()
		{

			pDefaultValues.Visible = false;
			_activeControl = new SVChFreqAndPowerDiss32(_checker.TestId, this);
			gbAll.Controls.Add(_activeControl);
			gbAll.Size = new Size(gbAll.Size.Width, gbAll.Size.Height + 50);
		}

		private void VisibleFrequency(bool visible)
		{

			ivFrequency.Visible = visible;
		}

		private void SaveElecInsulationResistanceControl()
		{
			if (SettingsObj.GetSettings().TestId == -1)
				return;
			ElecInsulationResistanceControl contr = (ElecInsulationResistanceControl)_activeControl;
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VCH_H34_38", contr.VCH3438.Value);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VCH_H34_39", contr.VCH3439.Value);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VCH_H35_1", contr.VCH351.Value);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VCH_H35_4", contr.VCH354.Value);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VSS_H4_38", contr.VSSH438.Value);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_VSS_H4_39", contr.VSSH439.Value);

			var valid = contr.VCH3438.Validate &&
				contr.VCH3439.Validate &&
				contr.VCH351.Validate &&
				contr.VCH354.Validate &&
				contr.VSSH438.Validate &&
				contr.VSSH439.Validate;


			if (_modification == Modification.Diss32_90A && _mode.ModeApplication == ModeApplication.RepairCheck)
			{
				DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_BL_H2_49", contr.BL8H249.Value);
				DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_BL_H2_50", contr.BL8H250.Value);
				DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_BVK_H2_41", contr.BVKH241.Value);
				DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_BVK_H2_40", contr.BVKH240.Value);

				valid = valid &&
					contr.BL8H249.Validate &&
					contr.BL8H250.Validate &&
					contr.BVKH241.Validate &&
					contr.BVKH240.Validate;
			}

			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ElecResist_WorkTest", valid ? 1 : 0);
		}

		private void ImageStop()
		{
			pDefaultValues.Visible = true;
			_state.ImageStop();
			_state.HideIndicators();
			grpBox_115V.Visible = grpBox_27V.Visible = false;
			_activeControl = null;
            gbAll.Text = "";
		}

		private void InitialiseChannelButtons()
		{
			RadioButton b;
			for (int i = 0; i < 3; i++)
			{
				b = new RadioButton();
				b.Name = "bChannel" + (i + 1);
				pageWork.Controls.Add(b);
				b.TextAlign = ContentAlignment.MiddleCenter;
				b.Text = (i + 1).ToString();
				b.Appearance = Appearance.Button;
				b.Size = new Size(219, 40);
				b.Visible = false;
				b.CheckedChanged += BOnCheckedChanged;
				b.Click += BOnClick;
				chButtons.Add(b);
			}
		}

		private void BOnClick(object sender, EventArgs eventArgs)
		{
			if (btn_Tok_K.FlatStyle != FlatStyle.Flat)
				return;
			//Только для проверки тока
			_state.CurrentParam = "I_K_{0}_{1}_32";
			
			CurTok = CurTok.Execude(_state, _checker);
			CurTok.EventExecuting += CurTok_EventExecuting;
		}

		private void BOnCheckedChanged(object sender, EventArgs eventArgs)
		{
			RadioButton rb = (RadioButton)sender;
			rb.BackColor = rb.Checked ? Globals.SuccessColor : Color.Transparent;
			if (rb.Checked)
			{
				int numButton = Convert.ToInt32(rb.Name.Substring(rb.Name.Length - 1, 1));
				SaveOneChannelParams(_state.CurrentParam, _state.ChannelState);
				_state.SetChannelIndicatorsVisible(_state.ChannelState - 1, false);
				_state.ChannelState = numButton;
				_checker.ChannelState = numButton;
				Diss32Functions.SetMeasChannel(_state.ChannelState - 1);
				_state.SetChannelIndicatorsVisible(_state.ChannelState - 1, true);
				if (btn_Tok_K.FlatStyle == FlatStyle.Flat)
				{
					CurTok = new CurTok0();
					CurTok.EventExecuting += CurTok_EventExecuting;
				}
				else if (btnSD.FlatStyle == FlatStyle.Flat)
				{
					_checker.InitializeSD(SettingsObj.GetSettings().TestId);
                    _state.ViewIndicators(_checker.CdIndicatorViewer);

                }
				else if (btnNoise.FlatStyle == FlatStyle.Flat)
				{
					_checker.InitializeNoise32(SettingsObj.GetSettings().TestId);
                    _state.ViewIndicators(_checker.NoiseIndicatorViewer);
                }
			}
		}

		public void ShowChannelButtons(bool state)
		{
			int baseY = 5;
			int baseWidth = 104;
			int space = 11;
			int stepX = baseWidth + space * 2;
			//baseY += stepY;
			baseWidth *= 2;
			baseWidth += space;
			stepX += baseWidth;
			int i = 0;
			if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
			{
				chButtons[0].Visible = state;
				chButtons[0].Visible = state;
				chButtons[0].Location = new Point(gbAll.Location.X + i * stepX, gbAll.Location.Y + gbAll.Height + baseY);
				if (!state)
				{
					chButtons[0].FlatStyle = FlatStyle.Standard;
					chButtons[0].Checked = false;
				}
			}
			else
			{
				foreach (RadioButton rb in chButtons)
				{
					rb.Visible = state;
					rb.Location = new Point(gbAll.Location.X + i * stepX, gbAll.Location.Y + gbAll.Height + baseY);
					if (!state)
					{
						rb.FlatStyle = FlatStyle.Standard;
						rb.Checked = false;
					}

					i++;
				}
			}
			if (state)
				chButtons[0].PerformClick();
			else
				_state.ChannelState = 0;
		}

		public void EnableForm(bool enable)
		{
			_dissStopped = !enable;
			btnNoise.Enabled = enable;
			btnSD.Enabled = enable;
			bFirstPitan.Enabled = enable;
			bSecPitan.Enabled = enable;
			btn_Tok_K.Enabled = enable;
			btn_GLPD_MOD.Enabled = enable;
			btnFreq.Enabled = enable;
			btnInsulationResist.Enabled = !enable;
		}

		#region отключение и включение кнопок
		private void PressButton(Button btn)
		{
			countButton = 1;
			btn.FlatStyle = FlatStyle.Flat;
			btn.BackColor = Color.FromArgb(153, 249, 164);
			Control.ControlCollection collButton = btn.Parent.Controls;
			for (int i = 0; i < collButton.Count; i++)
				if (collButton[i].GetType() == typeof(Button) &&
						collButton[i].Name != btn.Name)
                    if (collButton[i].Name != "btnInsulationResist")
                        collButton[i].Enabled = false;
		}

		private void WrungButton(Button btn)
		{
			countButton = 0;
			btn.FlatStyle = FlatStyle.Standard;
			btn.BackColor = Color.Transparent;
			Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button))
                {
                    if(collButton[i].Name != "btnInsulationResist")
                        collButton[i].Enabled = true;
                }

			if (btn == btn_GLPD_MOD)
			{
				VisibleFrequency(false);
			}
		}

		private void ShowResult(List<USBBaseNew> chList)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<List<USBBaseNew>>(ShowResult), new object[] { chList });
				return;
			}
			try
			{
				if (frmMain.IsFormClose)
					return;
				double voltage1 = 0, voltage2 = 0;
				foreach (USBBaseNew signal in chList)
				{
					if (signal.ParamName == specSignal[1])
					{
						voltage1 = signal.Result;
					}
					if (signal.ParamName == specSignal[2])
					{
						voltage2 = signal.Result;
					}

					if (signal.ParamName == specSignal[0])
						SetIndicatorValue(signal.IndicatorNumber, _measValueInfo.CalculateDelta15(voltage1, voltage2));
					else
						SetIndicatorValue(signal.IndicatorNumber, signal.Result);
				}
			}
			catch
			{
				//для отладки
			}
		}

		private void SetIndicatorValue(int number, double val)
		{
			if (number < 0)
				return;

			_state.SetIndicatorValue(number, val);
			measurementsControl1.SetIndicatorValue(number, val);
		}

		private void ShowLTRError()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(ShowLTRError));
				return;
			}
			MessageBox.Show(
				string.Format(Resources.ErrorOnDevice, Resources.LTRBoards, Globals.Operation.GetValue) + " " +
				Resources.ApplicationMustBeClosed, Resources.ErrorCaption);
			//Close();
		}
		#endregion

		#region Подстройка БП

		// -0,1В для 27В
		private void btn_27_Minus_Click(object sender, EventArgs e)
		{
			var frm = (frmWork32)ParentForm;
            lbl_Edit_27V.Text = frm.Minus27V(btn_27_Minus, btn_27_Plus);
		}

		// +0,1В для 27В
		private void btn_27_Plus_Click(object sender, EventArgs e)
		{
			var frm = (frmWork32)ParentForm;
			lbl_Edit_27V.Text = frm.Plus27V(btn_27_Minus, btn_27_Plus);

        }

		// -0,1В для 115В
		private void btn_115_Minus_Click(object sender, EventArgs e)
		{
			var frm = (frmWork32)ParentForm;
			lbl_Edit_115V.Text = frm.Minus115V(btn_115_Minus, btn_115_Plus);
        }

		// +0,1В для 115В 
		private void btn_115_Plus_Click(object sender, EventArgs e)
		{
			var frm = (frmWork32)ParentForm;
			lbl_Edit_115V.Text = frm.Plus115V(btn_115_Minus, btn_115_Plus);
		}

		#endregion Подстройка БП

		public bool ManualRecordDouble(int testId, string str, double value)
		{
			if (double.IsNaN(value))
				return true;
			return DataManager.Instance.SaveParameter(testId, str, value);
		}

		public bool ManualRecordDouble(string str, double value)
		{
			return ManualRecordDouble(_checker.TestId, str, value);
		}

		#endregion
		public void ImageTaskStop()
		{
			_measure.StopMeasuring(false);
			_measure.StartMeasuring();
			if (_activeControl != null)
				_activeControl.Enabled = false;
			if (Visible) // для всех КЗ
			{
				measurementsControl1.ImageStop();
			}
		}

		private void ReconfigureVisualDiss32_90(string nameRep)
		{
			rtbIndicatorsBaseState.Text = rtbIndicatorsBaseState.Text.Replace("ОТКЛ. БЛ. 8", nameRep);
			rtbIndicatorsBaseState.Text = rtbIndicatorsBaseState.Text.Replace("Unit 8 OFF", nameRep);
		}

		private void ReconfigureVisualDiss32(string nameRep)
		{
            rtbIndicatorsBaseState.Text = rtbIndicatorsBaseState.Text.Replace("“ОТКЛ. БЛ. 8”,", nameRep);
        }

		private void SaveOneChannelParams(string paramName, int channelState)
		{
			var val1 = _state.GetValueChannel(channelState, 1);
			if (val1 == null)
				return;
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, string.Format(paramName, channelState, 1), val1[0]);
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, string.Format(paramName, channelState, 2), val1[1]);
		}

		private void tabControl_Selected(object sender, TabControlEventArgs e)
		{
			switch (e.TabPage.Name)
			{
				case "pageWork": // проводим все измерения
					ImageOpenForm();
					break;
				case "pageTask": // проводим Контрольные задачи
					measurementsControl1.ImageTaskOpen();
					break;
				default:
					MessageBox.Show(Resources.IncorrectTab, Resources.ApplicationName, MessageBoxButtons.OK,
						MessageBoxIcon.Information);
					break;
			}
		}

		private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{      
			if(_dissStopped)
			{
				e.Cancel = true;
				return;
			}
			// При нажатых кнопках на вкладке запрещаем переход на другую вкладку
			if (countButton != 0 || measurementsControl1.CountButtons != 0)
			{
				e.Cancel = true;
				MessageBox.Show(Resources.CantChangeTab, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
		}

        private void tabControl_EnabledChanged(object sender, EventArgs e)
        {
			//btnInsulationResist.Enabled = !Enabled;
        }

        public void ConfigurePower()
        {
            if (!CommonGlobals.InitPower27V(PowerSupply27, _maxCurrent27))
                MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.Power27V, Globals.Operation.SetValue));
            if (!Power27V.Instance.SetOnOff(false))
                MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.Power27V, Globals.Operation.SetValue));
            if (!CommonGlobals.InitPower115(PowerSupply115Phase, MaxCurrent115))
                MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.Power115V, Globals.Operation.SetValue));
            if (!Power115V.Instance.SetOnOff(false))
                MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.Power115V, Globals.Operation.SetValue));

            if (!Globals.Emulation)
                if (!LTRManager.CheckCrate())
                    MessageBox.Show(Resources.LTRCrateFailed);
        }
    }
}