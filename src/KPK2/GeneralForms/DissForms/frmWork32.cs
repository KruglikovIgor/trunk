using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.DB;
using KPK2.Interfaces;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.MyControl;
using KPK2.Properties;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.NFunctions;
using UserControlsKPK2;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using Timer = System.Windows.Forms.Timer;
using Globals = Sparc.KPK2.NFunctions.Globals;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.NFunctions.Enums;
using KPK2.MyClasse;
using Hardware.LfGenerator.Akip._3410.Fm;

namespace KPK2.GeneralForms.DissForms
{
    /// <summary>
    /// Проведение проверки для ДИСС-32-90А 
    /// </summary>
    public partial class frmWork32 : Form,IFrmWork32
    {
		private static Logger _logger = LogManager.GetLogger("KPK2Error");
		#region Определения
        
		private PowerStrip Strip;

	    /// <summary>
	    /// Интегратор для сглаживания всплесков
	    /// </summary>
	    // Integrator ltrIntegrator = new Integrator();
	    public Measure _measure;

	    /// <summary>
	    /// Минимум для БП 27В = 24.8В(default)
	    /// </summary>
	    private double power27_Min = 24.8;

	    /// <summary>
	    /// Максимум для БП 27В = 30.2В(default) 
	    /// </summary>
	    private double power27_Max = 30.2;

	    /// <summary>
	    /// Норма для БП 27В = 27.5В(default) 
	    /// </summary>
	    private double power27_Norma = 27.5;

	    /// <summary>
	    /// Минимум для БП 115В = 109.25В(default)
	    /// </summary>
	    private double power115_Min = 109.25;

	    /// <summary>
	    /// Максимум для БП 115В = 121.75В(default) 
	    /// </summary>
	    private double power115_Max = 120.75;

	    /// <summary>
	    /// Норма для БП 115В = 115В(default) 
	    /// </summary>
	    private double power115_Norma = 115;

        /// <summary>
        /// [2]Разрешение на +/- 0,1В для БП27В; [0]-true, можно отнимать ещё; 
        /// [1]-true, можно прибавлять ещё 
        /// </summary>
        private bool[] arPower_27 = { true, true };
        /// <summary>
        /// [2]Разрешение на +/- 0,1В для БП115В; [0]-true, можно отнимать ещё; 
        /// [1]-true, можно прибавлять ещё 
        /// </summary>
        private bool[] arPower_115 = { true, true };

        private bool _oneChannelDiss; 
		private bool errorPS27VAppear = false;
		private Modification _modification;
	    private MeasurementPerformance _measurementPerform;
	    private DevicesID _deviceId;
	    private string _deviceNumber;

	    private System.Timers.Timer timerStrip = new System.Timers.Timer(1000);

		private List<USBBaseNew> _measList = new List<USBBaseNew>();

	    public bool IsOutputBlock8
	    {
	        get
	        {
	            return frmMain.IsFormClose ||
	                   rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
	                   rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
			}
	    }

	    public bool saveResults
	    {
	        get { return SettingsObj.GetSettings().TestId != -1; }
	    }
		
		public bool CheckerMinus
		{
			get => rbtn_Minus.Checked;
		}

		public bool CheckerNorma
		{
			get => rbtn_Norma.Checked;
		}

		public bool CheckerPlus
		{
			get => rbtn_Plus.Checked;
		}


        #endregion Определения

        public bool stateMapCorner
        {
            get { return tiMapCorner.State; }
        }

            public bool IsOneChannel
	    {
		    get => cbOneChannelDiss.Checked;
	    }

		public BitArray InputsStates
		{
			get; private set;
		}

		public bool TiMemory1
		{ get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiMemory1.State; }));
				}
				return tiMemory1.State;
			} 
		}
		public bool TiMemory2
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiMemory2.State; }));
				}
				return tiMemory2.State;
			}
		}
		public bool TiNavigation
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiNavigation.State; }));
				}
				return tiNavigation.State;
			}
		}
		public bool TiVCH
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiRefuseVCh.State; }));
				}
				return tiRefuseVCh.State;
			}
		}
		public bool TiVSS
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiRefuseVss.State; }));
				}
				return tiRefuseVss.State;
			}
		}
		public bool TiGood
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiGood.State; }));
				}
				return tiGood.State;
			}
		}
		public bool TiBPKOn
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiBPKOn.State; }));
				}
				return tiBPKOn.State;
			}
		}
		public bool TiFlight
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiFlight.State; }));
				}
				return tiFlight.State;
			}
		}
		public bool TiCapture
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiCapture.State; }));
				}
				return tiCapture.State;
			}
		}
		public bool TiCapture1
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiCapture1.State; }));
				}
				return tiCapture1.State;
			}
		}
		public bool TiCapture2
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiCapture2.State; }));
				}
				return tiCapture2.State;
			}
		}
		public bool TiCapture3
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(new Func<bool>(() => { return tiCapture3.State; }));
				}
				return tiCapture3.State;
			}
		}

		public bool TiDpsm
		{
			get
            {
                if (InvokeRequired)
                {
                    return (bool)Invoke(new Func<bool>(() => { return tiDPSM.State; }));
                }
                return tiDPSM.State;
            }
		}

		public bool TiDiss
		{
			get
			{
                if (InvokeRequired)
                {
                    return (bool)Invoke(new Func<bool>(() => { return tiDiss.State; }));
                }
                return tiDiss.State;
            }
		}

		public bool TiBVK
        {
            get
            {
                if (InvokeRequired)
                {
                    return (bool)Invoke(new Func<bool>(() => { return tiBVK.State; }));
                }
                return tiBVK.State;
            }
        }

		public bool TiPositionOff
        {
            get
            {
                if (InvokeRequired)
                {
                    return (bool)Invoke(new Func<bool>(() => { return tiPositionOff.State; }));
                }
                return tiPositionOff.State;
            }
        }

        public event Action<object, double[]> EventShowStrip;

	    public MeasuringValueInfo MeasuringValue { get; private set; }
        //bool IFrmWork32.stateMapCorner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public frmWork32(MeasurementPerformance state, Mode mode)
	    {
            try
            {
                MeasuringValue = new MeasuringValueInfo();
                _measurementPerform = state;
                _deviceNumber = _measurementPerform.DeviceNumb;
                _deviceId = _measurementPerform.DevicesId;
                _modification = _measurementPerform.Modification;
                _mode = mode;

                var settings = SettingsObj.GetSettings();
                settings.Modification = _modification;
                settings.DevicesId = _deviceId;
                settings.MeasValueInfo = MeasuringValue;
                _measure = settings.Measure;
                settings.MeasList = new List<USBBaseNew>();
                _measure.AddMeasureList(settings.MeasList);

                InitializeComponent();

                if (this.DesignMode)
                {
                    return;
                }

                if (_modification == Modification.Diss32_90A || _deviceId == DevicesID.Diss32 ||
                    _deviceId == DevicesID.Diss32_WorkTest)
                {
                    tiBlock8GeoOff.Visible = tiMapCorner.Visible = false;
                    tiBlock8OrtOff.VisualText = Res.Diss32IndBlock8;
                    if (_measurementPerform.DevicesId == DevicesID.Diss32 || _deviceId == DevicesID.Diss32_WorkTest)
                        tiBlock8OrtOff.Visible = tiPositionOff.Visible =
                            tiRefuseBVK.Visible = tiDPSM.Visible = tiDiss.Visible = tiBVK.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error);
            }
        }
	    
	    private void frmWork_Load(object sender, EventArgs e)
	    {
			try
			{
                if (String.IsNullOrEmpty(_deviceNumber))
                {
                    Text = string.Format(Res.CheckDeviceN, Globals.GetDissCaption(_deviceId, _modification));
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }
                else
                {
                    Text = string.Format(Res.CheckDeviceNN, Globals.GetDissCaption(_deviceId, _modification),
                        _deviceNumber);
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }

                if (!DiscreteOutputs.Instance().SetChannelsOff())
					MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.DiscreteOutputs, Globals.Operation.SetValue));
				if (!ControlTask.Instance().ControlTaskOff())
					MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				if (!GenSinCos.Instance().GeneratorOff())
					MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.GenSinCos, Globals.Operation.SetValue));


				DiscreteInputs.InversionList = new[] { DataManager.Instance.GetChannelByName(DataManager.DiscretInputs, "Refuse_BVK") };
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Resources.Error);
				gbMode.Enabled = gbPower.Enabled = false;
				tabControl.EnableForm(false);
				StartCloseTimer();
				return;
			}

            try
            {
                tabControl.ImageOpenForm();
                tabControl.EventOnHighFlat += TabControl_EventOnHighFlat;
                tabControl.EventOnHighStandard += TabControl_EventOnHighStandard;
                tabControl.EventHighEnable += (o, args) => { btn_High.Enabled = true; };
                tabControl.EventIndicatorPowerI += TabControl_EventIndicatorPowerI;
                tabControl.EventIndicatorPowerU += TabControlOnEventIndicatorPowerU;

                btn_High.Enabled = false;

                tabControl.MeasurementPerformance = _measurementPerform;

                if (_deviceId == DevicesID.Diss32)
                {
                    tabControl.measurementsControl1._deviceId = _deviceId;
                    IndicatorStatesReady += tabControl.measurementsControl1.IndicatorStatesReadyFunc;
                }

                tabControl.EnableForm(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error);
            }
        }

	    private void TabControlOnEventIndicatorPowerU(object arg1, double arg2)
		{
			piStrip.Bars[2].Value = (float)arg2;
		}

	    private void TabControl_EventIndicatorPowerI(object arg1, double arg2)
		{
			piStrip.Bars[3].Value = (float)arg2;
		}

		private void TabControl_EventOnHighFlat(object sender, EventArgs e)
		{
			btn_High.Enabled = true;
			if (btn_High.FlatStyle == FlatStyle.Flat)
				btn_High.PerformClick();
			btn_High.Enabled = false;
		}

	    private void TabControl_EventOnHighStandard(object sender, EventArgs e)
		{
			btn_High.Enabled = true;
			if (btn_High.FlatStyle == FlatStyle.Standard)
			    btn_High.PerformClick();
		    btn_High.Enabled = false;
		}

	    private void StartCloseTimer()
		{
		    Timer tmrClose = new Timer { Interval = 400 };
		    tmrClose.Tick += (sender, args) =>
		    {
			    tmrClose.Stop();
				tmrClose.Dispose();
			    Close();
		    };
		    tmrClose.Start();
		}
		
		private void ReconfigureVisualDiss32()
	    {
			tiNavigation.Location = tiPositionOff.Location;
			tiRefuseVCh.Location = tiRefuseBVK.Location;
	    }

	// закрываем форму
	    private void frmWork_FormClosing(object sender, FormClosingEventArgs e)
	    {
	        try
	        {
	            //isPower = false;
	            Power115V.Instance.SetOnOff(false);
	            Power27V.Instance.SetOnOff(false);
	            //_lambda27V.OnOffState(false);

	            if (!rbtn_Off.Checked)
	                rbtn_Off.PerformClick();

	            DiscreteInputs.Instance().StopMeasuring();
	            _measure.StopMeasuring(false);
	            timerStrip.Stop();
				timerStrip.Dispose();
	            GenSinCos.Instance().GeneratorOff();

	        }
	        catch (Exception ex)
	        {
	            _logger.Error(ex);
	            MessageBox.Show(Resources.CloseFormError);
	        }
	        finally
	        {
	            // разблокировать меню 
	            if (ParentForm != null)
	                ((frmMain) ParentForm).EnableMenu(true);
	        }
	    }


	    #region Режимы работы

	    // ДИСС выключен 
	    private void rbtn_Off_Click(object sender, EventArgs e)
	    {
			if (tabControl.IsButtonElecResistSelect)
			{
				MessageBox.Show("Закончите с вычеслениями электрическим сопротивлением изоляции", "Информация!", MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				return;
			}
			if (rbtn_Off.FlatStyle == FlatStyle.Standard)
			{
				cbOneChannelDiss.Enabled = true;
				tabControl.TurnOffButtons();
				StopDiss();
				rbtn_Off.Text = Resources.Diss + ' ' + Resources.Disabled;
				rbtn_Off.FlatStyle = FlatStyle.Flat;
				rbtn_On.FlatStyle = FlatStyle.Standard;
			}
	    }
		
	    // ДИСС выключить
	    private void rbtn_Off_CheckedChanged(object sender, EventArgs e)
	    {
	      if (rbtn_Off.FlatStyle == FlatStyle.Flat)
	        rbtn_Off.Text = Resources.Diss + ' ' + Resources.EnableOff;
	    }

	    // ДИСС включен 
	    private void rbtn_On_Click(object sender, EventArgs e)
	    {
	      if (rbtn_On.FlatStyle == FlatStyle.Standard)
	      {
	        cbOneChannelDiss.Enabled = false;
	        StartDiss();
	        rbtn_On.Text = Resources.Diss + ' ' + Resources.Enabled;
	        rbtn_On.FlatStyle = FlatStyle.Flat;
	        rbtn_Off.FlatStyle = FlatStyle.Standard;
			tabControl.Start(_measurementPerform, cbOneChannelDiss.Checked);
	      }
        }

	    // ДИСС включить 
	    private void rbtn_On_CheckedChanged(object sender, EventArgs e)
	    {
	      if (rbtn_On.FlatStyle == FlatStyle.Flat)
	        rbtn_On.Text = Resources.Diss + ' ' + Resources.EnableOn;
        }

	    // ВЫСОКОЕ включить/выключить 
	    private void btn_High_Click(object sender, EventArgs e)
	    {
	      if (btn_High.FlatStyle == FlatStyle.Standard)
	      {
	        bool bl = false;
	        try
	        {
	          DiscreteOutputs.Instance().SetOutput("High_32", false);
	        }
	        catch
	        {
	          bl = true;
	          MessageBox.Show(string.Format(Resources.ErrorCommand, Resources.HighVoltageOn) + Environment.NewLine +
	                          Resources.ApplicationMustBeClosed, Resources.ErrorCaption);

	        }
	        if (bl) return;
	        btn_High.Text = Resources.High + ' ' + Resources.EnableOff;
	        btn_High.FlatStyle = FlatStyle.Flat;
	        btn_High.BackColor = Color.FromArgb(153, 249, 164);
	      }
	      else
	      {
	        try
	        {
	          DiscreteOutputs.Instance().SetOutput("High_32", true);
	        }
	        catch
	        {
	        }
	        btn_High.Text = Resources.High + ' ' + Resources.EnableOn;
	        btn_High.FlatStyle = FlatStyle.Standard;
	        btn_High.BackColor = Color.Transparent;
	      }
        }

	    #endregion
	        
	    /// <summary>
	    /// Обработчик кнопки "24,4В ~109В" - сигналы "27V_Minus" и "115V_Minus"
	    /// </summary>
	    private void rbtn_Minus_Click(object sender, EventArgs e)
        {
            if (rbtn_Minus.FlatStyle == FlatStyle.Standard)
	      {
	        try
	        {
	           Power27V.Instance.SetVoltage(power27_Min);
	          Power115V.Instance.SetVoltage(power115_Min);           
	        }
	        catch
	        {
	          return;
	        }
	        rbtn_Minus.FlatStyle = FlatStyle.Flat;
	        rbtn_Norma.FlatStyle = rbtn_Plus.FlatStyle = FlatStyle.Standard;
	      }
        }

	    /// <summary>
	    /// Обработчик кнопки "27B~115B" - сигналы "27V_On" и "115V_Norma"
	    /// </summary>
	    private void rbtn_Norma_Click(object sender, EventArgs e)
	    {
	      if (rbtn_Norma.FlatStyle == FlatStyle.Standard)
	      {
	        try
	        {
	           Power27V.Instance.SetVoltage(power27_Norma);
	          Power115V.Instance.SetVoltage(power115_Norma);
	        }
	        catch
	        {
	          return;
	        }
	        rbtn_Norma.FlatStyle = FlatStyle.Flat;
	        rbtn_Minus.FlatStyle = rbtn_Plus.FlatStyle = FlatStyle.Standard;
	      }
	    }

	    /// <summary>
	    /// Обработчик кнопки "24,4В ~109В" - сигналы "27V_Plus" и "115V_ Plus"
	    /// </summary>
	    private void rbtn_Plus_Click(object sender, EventArgs e)
	    {
	      if (rbtn_Plus.FlatStyle == FlatStyle.Standard)
	      {
	        try
	        {
	           Power27V.Instance.SetVoltage(power27_Max);
	          Power115V.Instance.SetVoltage(power115_Max);
	        }
	        catch
	        {
	          return;
	        }
	        rbtn_Plus.FlatStyle = FlatStyle.Flat;
	        rbtn_Minus.FlatStyle = rbtn_Norma.FlatStyle = FlatStyle.Standard;
	      }
	    }
	    

	    public void ErrorDetected(string errorString)
	    {
	      if (InvokeRequired)
	      {
	        Invoke(new Action<string>(ErrorDetected), errorString);
	        return;
	      }
	      MessageBox.Show(errorString, Resources.ErrorCaption);
	    }

	    // Контрольная задача 3        
	    private const string Task3_PC_32_Const = "Task3_PC_32";
	    private const string Task3_Y0_32_Const = "Task3_Y0_32";


	    // Контрольная задача 4   
	    private const string Task4_PC_32_Const = "Task4_PC_32";
	    private const string Task4_Y9_32_Const = "Task4_Y9_32";

	    /// <summary>
	    /// Включаем ДИСС 
	    /// </summary>
	    private void StartDiss()
	    {
		      bool bl = false;
		      try
		      {
					errorPS27VAppear = false;
					_measurementPerform.StartWorker();
			        if (!DiscreteInputs.Instance().StartMeasuring(DiscreteInputsReceived))
						throw new Exception(Globals.GetDeviceErrorString(Globals.Device.DiscreteInputs, Globals.Operation.GetValue));
			        DiscreteOutputs.Instance().SetOutput("High_32", false);
			        Power27V.Instance.SetOnOff(true);
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП постоянного тока 27В включен!"));
                Power115V.Instance.SetVoltage(MeasuringValue.Voltage115);
			        Power115V.Instance.SetOnOff(true);
                    _logger.Info(string.Format("ИП 115В включен!"));
			        DO176X.Instance.SetBVKRefuse(false); //специально так делается т.к. для плат 1762 другая логика

			        if (_modification == Modification.Diss32_90A)
			            DiscreteOutputs.Instance().SetOutput("Course", true); //Отключение географического блока
		      }
		      catch
		      {
			        bl = true;
			        gbMode.Enabled = gbPower.Enabled = false;
					tabControl.EnableForm(false);
					rbtn_On.Checked = false;
			        rbtn_On.FlatStyle = FlatStyle.Standard;
			        MessageBox.Show(Resources.CommandError + ' ' + Resources.ApplicationMustBeClosed, Resources.ErrorCaption);
		      }
		      if (bl)
					return;
		      // включаем "Высокое" 
		      btn_High.Enabled = true;
		      btn_High.Text = Resources.High + ' ' + Resources.EnableOff.ToUpper();
		      btn_High.FlatStyle = FlatStyle.Flat;
		      btn_High.BackColor = Color.FromArgb(153, 249, 164);
		      btn_High.Enabled = true;
			  tabControl.EnableForm(true);            
		      gbMode.Enabled = gbPower.Enabled = true;
	    }

	    private bool _oneTimeError;

	    private void DiscreteInputsReceived(BitArray ba)
	    {
	      if (ba == null)
	        return;
		  //InputsStates = ba;
	      if (InvokeRequired)
	      {
	        try
	        {
	          Invoke(new Action<BitArray>(DiscreteInputsReceived), ba);
	        }
	        catch
	        {
	        }
	        return;
	      }
	      Dictionary<int, string> chList = DataManager.Instance.GetDiscreteInputsChannels();
	      foreach (KeyValuePair<int, string> rec in chList)
	      {
	        SetTextindicatorState(rec.Value, ba[rec.Key]);
	      }
	      try
	      {
	        ba = DO176X.Instance.ReadDI32(true);
	        chList = DataManager.Instance.GetAdv1760Channels();
	      }
	      catch
	      {
	        if (!_oneTimeError)
	        {
	          _oneTimeError = true;
	          ErrorDetected(Globals.GetDeviceErrorString(Globals.Device.Advantech176X, Globals.Operation.GetValue));
	        }
	      }
	      foreach (KeyValuePair<int, string> rec in chList)
	      {
	        SetTextindicatorState(rec.Value, ba[rec.Key]);
	      }
	      IndicatorStatesReadyFunc(_currentIndicatorsState);
	    }

	    private void SetTextindicatorState(string chName, bool state)
	    {
	      TextIndicator current;
	      switch (chName)
	      {
	        case "Capture":
	          current = tiCapture;
	          _currentIndicatorsState.Capture = state;
	          break;
	        case "Capture_1":
	          current = tiCapture1;
	          _currentIndicatorsState.Capture1 = state;
	          break;
	        case "Capture_2":
	          current = tiCapture2;
	          _currentIndicatorsState.Capture2 = state;
	          break;
	        case "Capture_3":
	          current = tiCapture3;
	          _currentIndicatorsState.Capture3 = state;
	          break;
	        case "GoodCondition":
	          current = tiGood;
	          _currentIndicatorsState.Serviceability = state;
	          break;
	        case "Memory_1":
	          current = tiMemory1;
	          _currentIndicatorsState.Memory1 = state;
	          break;
	        case "Memory_2":
	          current = tiMemory2;
	          _currentIndicatorsState.Memory2 = state;
	          break;
	        case "BPK_On":
	          current = tiBPKOn;
	          break;
	        case "Flight":
	          current = tiFlight;
	          _currentIndicatorsState.Flight = state;
	          break;
	        case "Refuse_VCh":
	          current = tiRefuseVCh;
	          break;
	        case "Refuse_VSS":
	          current = tiRefuseVss;
	          break;
	        case "Refuse_BVK":
	          current = tiRefuseBVK;
	          break;
	        case "DissOn":
	          current = tiDissOn;
	          _currentIndicatorsState.DissOn = state;
	          break;
	        case "High":
	          current = tiHigh;
	          _currentIndicatorsState.High = state;
	          break;
	        //case "Block8_32":
	        case "BVK_Refuse":
	          current = tiBlock8OrtOff;
	          //LogOpen.WriteLogOpen("Set: " + state+"\n");
	          break;
	        case "DPSM_32":
	          tiDPSM.State = tiBVK.State && tiMemory2.State;
	         // current = tiDPSM;
	          return;
	        case "Calculus":
	          current = tiDiss;
	          break;
	        case "Disconnect":
	          current = tiPositionOff;
	          break;
	        case "BVK_Good":
	          current = tiBVK;
	          break;
	        case "Navigation":
	          current = tiNavigation;
	          break;
	        case "DPSM":
	          current = tiBlock8GeoOff;
	          break;
	        default:
	          return;
	      }
	      current.State = state;
	    }

	    private ucIndicatorsState.IndicatorsState _currentIndicatorsState = new ucIndicatorsState.IndicatorsState();

	    private void IndicatorStatesReadyFunc(ucIndicatorsState.IndicatorsState states)
	    {
	      Action<ucIndicatorsState.IndicatorsState> handler = IndicatorStatesReady;
	      if (handler != null)
	        handler(states);
	    }

	    private event Action<ucIndicatorsState.IndicatorsState> IndicatorStatesReady;

	    /// <summary>
	    /// Выключаем ДИСС 
	    /// </summary>
	    private void StopDiss()
	    {
			rbtn_Norma.PerformClick();
			gbMode.Enabled = gbPower.Enabled = false;
			_measurementPerform.HideIndicators();
			tabControl.ImageOpenForm();
			tabControl.ImageTaskStop();
			GenSinCos.Instance().GeneratorOff();
			Power115V.Instance.SetOnOff(false);
            //var logger = LogManager.GetCurrentClassLogger();
            _logger.Info(string.Format("ИП 115В выключен!"));
            Power27V.Instance.SetOnOff(false);
            _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
            if (btn_High.FlatStyle == FlatStyle.Flat) // выключаем "Высокое" 
	        btn_High.PerformClick();
	      btn_High.Enabled = false;

	      try
	      {
	          DO176X.Instance.SetChannelsOff();
	      } // отключили PCI 
	      catch
	      {
	      }

	      // Выключить все кнопки на активной вкладке
	      tabControl.DisableButton();
	      tabControl.ChangeTab("pageWork"); // переход на первую вкладку
		  tabControl.EnableForm(false);
	      gbMode.Enabled = true;
	      StopWorker();
	      DiscreteInputs.Instance().StopMeasuring();
	      Application.DoEvents();
	      TurnOffIndicators();
	    }

	    private void TurnOffIndicators()
	    {
	      tiHigh.State =
	        tiCapture2.State =
	          tiBPKOn.State =
	            tiBVK.State =
	              tiBlock8GeoOff.State =
	                tiBlock8OrtOff.State =
	                  tiCapture.State =
	                    tiCapture1.State =
	                      tiCapture3.State =
	                          tiDPSM.State =
	                            tiBPKOn.State =
	                              tiDiss.State =
	                                tiFlight.State =
	                                  tiGood.State =
	                                    tiMapCorner.State =
	                                      tiMemory1.State =
	                                        tiMemory2.State =
	                                          tiNavigation.State =
	                                            tiPositionOff.State =
	                                              tiRefuseBVK.State = tiRefuseVCh.State = tiRefuseVss.State = tiDissOn.State = false;
	    }

		#region timerStrip

		private void timerStrip_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			timerStrip.Stop();
			try
			{
				Strip.Execute(new ManualResetEvent(false));
				if (!errorPS27VAppear)
					if (Common.Globals.CheckPS27Protection(Power27V.Instance, ShowError))
						errorPS27VAppear = true;
			}
			catch (Exception ex)
			{
				if (!rbtn_Off.Checked)
					_logger.Error(ex);
			}
			if(timerStrip != null)
				timerStrip.Start();
		}

		private void ShowError(string text)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<string>(ShowError), text);
				return;
			}

			MessageBox.Show(text, Resources.ErrorCaption);
		}

		#endregion

		/// <summary>
		/// Инициализация для измерения Шума  
		/// </summary>
		
	    private void StopWorker()
	    {
	      _measure.StopMeasuring(saveResults);
	      if (Power115V.Instance.IsAktor)
	        _measure.StartMeasuring();
	    }

	    public void ShowStrip(double[] arResult)
		{
			  for (int i = 0; i< 2; i++)
				  piStrip.Bars[i].Value = (float) Math.Round(arResult[i], 1);
			  if (!Power115V.Instance.IsAktor) //если ИП другой, то плат-нормализаторов нет, значит измеряем с источника
			  {
				  for (int i = 2; i< 4; i++)
					  piStrip.Bars[i].Value = (float) Math.Round(arResult[i], 1);
				  EventShowStrip?.Invoke(this, arResult);
			  }
		}

	    private void GetFocusedControl(Control.ControlCollection Controls)
	    {
	      foreach (Control ctrl in Controls)
	      {
	        //  if (ctrl.Focused)
	        //    _fcName = ctrl.Name;
	        if (ctrl.Controls.Count != 0)
	          GetFocusedControl(ctrl.Controls);
	      }
	    }

	    private void frmWork32_KeyUp(object sender, KeyEventArgs e)
	    {
			GetFocusedControl(Controls);
	    }

	    private void cbOneChannelDiss_CheckedChanged(object sender, EventArgs e)
	    {
	      _oneChannelDiss = cbOneChannelDiss.Checked;
	      tiCapture1.Visible = tiCapture2.Visible = tiCapture3.Visible = !_oneChannelDiss;
	      tabControl.IsOneChannel = cbOneChannelDiss.Checked;
	    }
	    
	  
	    private void frmWork32_Shown(object sender, EventArgs e)
	    {
            try
            {
                if (_deviceId == DevicesID.Diss32 || _deviceId == DevicesID.Diss32_WorkTest)
                    ReconfigureVisualDiss32();

                Application.DoEvents();

                tabControl.ConfigurePower();

                try
                {
                    Strip = new PowerStrip(false, SettingsObj.GetSettings().TestId, ShowStrip, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.PowerSupplyMeasuringError + Environment.NewLine + ex.Message);
                    gbMode.Enabled = gbPower.Enabled = false;
                    tabControl.EnableForm(false);
                    return;
                }

                timerStrip.Elapsed += timerStrip_Elapsed;
                timerStrip.Start();
                rbtn_On.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error);
            }
        }

        private void tabControl_Load(object sender, EventArgs e)
        {

        }

		internal string Minus27V(Button btn_27_Minus, Button btn_27_Plus)
		{
            btn_27_Minus.Enabled = false;
            // можем прибавлять напряжение 
            btn_27_Plus.Enabled = arPower_27[1] = true;
            if (!arPower_27[0]) // достигли min 
                return Resources.MinValue;
            if (CheckerMinus)
                power27_Min -= 0.1;
            if (CheckerNorma)
                power27_Norma -= 0.1;
            if (CheckerPlus)
                power27_Max -= 0.1;
            double ddd = CheckerMinus
            ? power27_Min
                           : CheckerNorma ? power27_Norma : power27_Max;
            // выставили min на БП 
            arPower_27[0] = !(ddd <= 23.5);
            try
            {
                Power27V.Instance.SetVoltage(ddd); // _lambda27V.SetVoltage(ddd)
                                                   //ControlSignal_27V(ddd);
            }
            catch
            {
            }
            btn_27_Minus.Enabled = arPower_27[0];
			return arPower_27[0] ? "" : Resources.MinValue;
        }

		internal string Plus27V(Button btn_27_Minus, Button btn_27_Plus)
		{
            btn_27_Plus.Enabled = false;
            // можем отнимать напряжение 
            btn_27_Minus.Enabled = arPower_27[0] = true;
            if (!arPower_27[1]) // достигли max  
                return Resources.MaxValue;
            if (CheckerMinus)
                power27_Min += 0.1;
            if (CheckerNorma)
                power27_Norma += 0.1;
            if (CheckerPlus)
                power27_Max += 0.1;
            double limitValue = CheckerMinus
                           ? power27_Min
                           : CheckerNorma ? power27_Norma : power27_Max;
            // выставили max на БП 
            arPower_27[1] = !(limitValue >= 31.0);
            try
            {
                Power27V.Instance.SetVoltage(limitValue); //_lambda27V.SetVoltage(limitValue)
                                                          //ControlSignal_27V(ddd);
            }
            catch
            {
            } 
            btn_27_Plus.Enabled = arPower_27[1];
			return arPower_27[1] ? "" : Resources.MaxValue;
        }

		internal string Minus115V(Button btn_115_Minus, Button btn_115_Plus) 
		{
            btn_115_Minus.Enabled = false;
            // можем прибавлять напряжение 
            btn_115_Plus.Enabled = arPower_115[1] = true;
            if (!arPower_115[0]) // достигли min 
                return Resources.MinValue;
            if (CheckerMinus)
                power115_Min -= 0.25;
            if (CheckerNorma)
                power115_Norma -= 0.25;
            if (CheckerPlus)
                power115_Max -= 0.25;
            double ddd = CheckerMinus
                           ? power115_Min
                           : CheckerNorma ? power115_Norma : power115_Max;
            // выставили min на БП 
            arPower_115[0] = !(ddd <= 108.5);
            try
            {
                Power115V.Instance.SetVoltage(ddd);
            }
            catch
            {
            }
            btn_115_Minus.Enabled = arPower_115[0];
			return arPower_115[0] ? String.Empty : Resources.MinValue;
        }

		internal string Plus115V(Button btn_115_Minus, Button btn_115_Plus)
        {
            btn_115_Plus.Enabled = false;
            // можем прибавлять напряжение 
            btn_115_Minus.Enabled = arPower_115[0] = true;
            if (!arPower_115[1]) // достигли max 
                return Resources.MaxValue;
            if (CheckerMinus)
                power115_Min += 0.25;
            if (CheckerNorma)
                power115_Norma += 0.25;
            if (CheckerPlus)
                power115_Max += 0.25;
            double ddd = CheckerMinus
                           ? power115_Min
                           : CheckerNorma ? power115_Norma : power115_Max;
            // выставили min на БП 
            arPower_115[1] = ddd >= 122.5 ? false : true;
            try
            {
                Power115V.Instance.SetVoltage(ddd);
            }
            catch { }
            btn_115_Plus.Enabled = arPower_115[1];
			return arPower_115[1] ? String.Empty : Resources.MaxValue;
        }
    }
}
