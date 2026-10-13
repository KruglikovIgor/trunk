using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using UserControlsKPK2;
using Resources = KPK2.Properties.Resources;
using Globals = Sparc.KPK2.NFunctions.Globals;
using Functions = Sparc.KPK2.NFunctions.Functions;
using Sparc.KPK2.NFunctions.Enums;
using System.Threading;
using Sparc.KPK2.Hardware.LTR;
using UserControlsKPK2.Enums;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Imitator;
using KPK2.GeneralForms.AddForms;
using KPK2.Common;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.MyClasse;
using Sparc.KPK2.Hardware.MyClasseUSB;
using NLog;
using KPK2.Interfaces;

namespace KPK2.MyControl.DISS.FrmDiss32
{
	public partial class MeasurementsControl : UserControl, IMeasurementsControl
    {
		private const string MapCornerParam = "MapCorner";
		private const string ModeMemoryPC = "ModeMemory_PC";
		private const string ModeMemoryYC = "ModeMemory_YC";
		private const string ModeMemoryPC2 = "ModeMemory_PC2";
		private const string ModeMemoryYC2 = "ModeMemory_YC2";
		public string ModeMSStr = "Mode_M_C";
		public string ModeMBStr = "Mode_M_B";
        public string ModeMS2Str = "Mode_M_C_2";

		private string[] _arComboBoxCorner = new[] { Resources.NotChanged, Resources.GreatThan, Resources.LessThan };
		private List<MethodInvoker> _renewDelegateList = new List<MethodInvoker>();
		private Button[] arButtonTask = new Button[8];
		private Button[] btnCT3 = new Button[2];
		private Control _activeControl;
		private Control _activeControl2;
		private Measure _measure;
		private Imitator.Device _device;
		private bool[] _arBlock8 = new bool[4];
		private double[] _arBlock8Value = new[] { -111.0, -111.0, -111.0, -111.0 };
		private bool[] arTxtBox_Speed = new bool[2];
		private bool _directionReversed = false;
        private ushort _currentDirection = 0;
		private int _lastTaskNum = 1;
		private int countBlock8;
		private TextIndicator _tiMapCorner; //todo сделать запись значения через Событие.
		private DateTime _workDT;

		#region Из старой ревизии код

		/// <summary>
		/// Переменная для (возможно) записи скорости в БД и м.б.
		/// расчета разности в КЗ-3 (Режим ПАМЯТЬ)
		/// </summary>
		private double task3_PC1;

		private double task3_PC1_2;

		/// <summary>
		/// Переменная для расчета разности скоростей 
		/// в КЗ-3 (Режим ПАМЯТЬ)
		/// </summary>
		private double task3_PC1_differ;

		private double task3_PC1_differ_2;

		/// <summary>
		/// Переменная для (возможно) записи угла в БД и м.б.
		/// расчета разности в КЗ-3 (Режим ПАМЯТЬ)
		/// </summary>
		private double task3_YC0;

		private double task3_YC0_2;

		/// <summary>
		/// Переменная для расчета разности углов 
		/// в КЗ-3 (Режим ПАМЯТЬ)
		/// </summary>
		private double task3_YC0_differ;

		private double task3_YC0_differ_2;

		/// <summary>
		/// Переменная для (возможно) записи скорости в БД и м.б.
		/// расчета разности в КЗ-4 (Режим М-С - М-Б)
		/// </summary>
		private double task4_PC2;

		private double task4_PC2_2;

		/// <summary>
		/// Переменная для расчета % превышения скорости
		/// в КЗ-4 (Режим М-С - М-Б) 
		/// </summary>
		private double task4_PC2_differ;

		private double task4_PC2_differ_2;

		/// <summary>
		/// Переменная для (возможно) записи угла в БД и м.б.
		/// расчета разности в КЗ-4 (Режим ПАМЯТЬ)
		/// </summary>
		private double task4_YC9;

		private double task4_YC9_2;

		/// <summary>
		/// Переменная для расчета разности углов 
		/// в КЗ-3 (Режим ПАМЯТЬ)
		/// </summary>
		private double task4_YC9_differ;

		private double task4_YC9_differ_2;

		/// <summary>
		/// true - нажат "Крен", иначе - "Тангаж" КЗ-1 
		/// </summary>
		private bool isHeel;

		/// <summary>
		/// true - "+12°", иначе - "-12°" КЗ-1 
		/// </summary>
		private bool isPlusCorner;

		/// <summary>
		/// Содержимое ComboBox для КЗ-1 и КЗ-3 в grpBox_Corner
		/// (Ввдение углов крена и тангажа)
		/// </summary>
		private string[] arComboBoxCorner = new[] { Resources.NotChanged, Resources.GreatThan, Resources.LessThan };

		/// <summary>
		/// [4]отсутствие сигналов П1,П2,П,ПОИСК; [0]-true выбрали +11град. КЗ-3; 
		/// [1]-true выбрали -11 КЗ-3; [2]-true "ДА" для +11 КЗ-3; 
		/// [3]-true "ДА" для -11 КЗ-3; 
		/// </summary>
		private bool[] arPoint32_1 = new bool[4];

		/// <summary>
		/// [4]отсутствие запоминания при +/-5град; [0]-true выбрали +5 КЗ-3; 
		/// [1]-true выбрали -5 КЗ-3; [2]-true "ДА" для +5 КЗ-3; 
		/// [3]-true "ДА" для -5 КЗ-3; 
		/// </summary>
		private bool[] arPoint32_2 = new bool[4];

		/// <summary>
		/// [4]отсутствие запоминания при +/-15град(М-Б); [0]-true выбрали +15 КЗ-3; 
		/// [1]-true выбрали -15 КЗ-3; [2]-true "ДА" для +15 КЗ-3; 
		/// [3]-true "ДА" для -15 КЗ-3; 
		/// </summary>
		private bool[] arPoint32_3 = new bool[4];

		/// <summary>
		/// [4]отсутствие запоминания при +/-15град(ВПЕРЕД); [0]-true выбрали +15 КЗ-1; 
		/// [1]-true выбрали -15 КЗ-1; [2]-true "ДА" для +15 КЗ-1; 
		/// [3]-true "ДА" для -15 КЗ-1; 
		/// </summary>
		private bool[] arPoint32_4 = new bool[4];

		/// <summary>
		/// Символ плюс/минус "±" 
		/// </summary>
		private string strPlusMinus = ((Char)0x00B1).ToString();

		/// <summary>
		/// Символ градуса "°" 
		/// </summary>
		private string strGrad = ((Char)0x00B0).ToString();

		/// <summary>
		/// Для контроля выбранных значений в ComboBox-х
		/// при проведении КЗ-1 и КЗ-3
		/// </summary>
		private bool[] arComboBox = new bool[3];

		#endregion

		private string Plus15Grad = string.Format(Resources.AngleGrad, "+15");
		private string Minus15Grad = string.Format(Resources.AngleGrad, -15);
		private string Plus12Grad = string.Format(Resources.AngleGrad, "+12");
		private string Minus12Grad = string.Format(Resources.AngleGrad, -12);
		private string Plus11Grad = string.Format(Resources.AngleGrad, "+11");
		private string Minus11Grad = string.Format(Resources.AngleGrad, -11);
		private string Plus5Grad = string.Format(Resources.AngleGrad, "+5");
		private string Minus5Grad = string.Format(Resources.AngleGrad, -5);
		private string ZeroGrad = string.Format(Resources.AngleGrad, 0);

		private const string Task3_PC_32_Const = "Task3_PC_32"; //todo вынести в отдельный класс
		private const string Task3_Y0_32_Const = "Task3_Y0_32";
		private string Task3_PC_32 = Task3_PC_32_Const;
		private string Task3_Y0_32 = Task3_Y0_32_Const;
		private const string Task3_PC2_32 = "Task3_PC2_32";
		private const string Task3_Y20_32 = "Task3_Y20_32";

		private const string Task4_PC_32_Const = "Task4_PC_32"; //todo вынести в отдельный класс
		private const string Task4_Y9_32_Const = "Task4_Y9_32";
		private string Task4_PC_32 = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck ? Task4_PC_32_Const + "_RepairTest": Task4_PC_32_Const;
		private string Task4_PC2_32 = "Task4_PC2_32";
		private string Task4_Y9_32 = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck ? Task4_Y9_32_Const + "_RepairTest" : Task4_Y9_32_Const;
		private string Task4_Y29_32 = "Task4_Y29_32";

		private int? _typeVSS = null;

		private bool IsFlightVSS
		{
			get { return _typeVSS == 1; } //todo зачем это?
		}

		private bool UseSecondUSPSChannel ///Смена каналов на 2-ой жгут там работает М-С и М-Б
		{
			get
			{
				//return CommonSettings.Default.IsNSK && !IsDiss32_90;
				return false;
			}
		}

		private bool IsDiss32_90
		{
			get { return (DevicesId == DevicesID.Diss32_90 || DevicesId == DevicesID.Diss32_90_WorkTest) && (Modification == Modification.None); }
		}

		public event Action<object, string> EventErrorMessage;
        public event EventHandler EventHigh;

		public bool IsLoad { get; private set; } = false;
		
		public bool SaveResults
		{
			get { return SettingsObj.GetSettings().TestId != -1; }
		}

		public bool IsOneChannel { get; set; }

		public int CountButtons { get; private set; }
		
		public TestTasks TestTasks { get; set; }

		public Measure Measure
		{
			get => _measure;
			set => _measure = value;
		}

		public MeasuringValueInfo MeasuringValueInfo { get; set; }

		public DevicesID DevicesId { get; set; }

		public Modification Modification { get; set; }

		public Imitator.Device Device
		{
			get => _device;
			set => _device = value;
		}

		public MeasurementsControl()
        {
            InitializeComponent();
            if (this.DesignMode)
            {
                return;
            }

            string processName = Process.GetCurrentProcess().ProcessName;
			ModeApplication mode = ModeApplication.RepairCheck;
			if (processName == "KPK2" && Mode.GetMode().ModeApplication == mode)
			{
				ModeMSStr += "_WorkTest";
				ModeMS2Str += "_WorkTest";
			}
		}

		public void DisableButton()
		{
			for (int i = 0; i < arButtonTask.Length; i++)
				if (arButtonTask[i].FlatStyle == FlatStyle.Flat)
					arButtonTask[i].PerformClick();
		}

		public void ImageTaskOpen()
		{
			panel_Task.Visible = false;
		}

		public void TurnOffButtonsTask()
		{
			foreach (var item in arButtonTask)
				if (item.FlatStyle == FlatStyle.Flat)
				{
					item.PerformClick();
					return;
				}
		}

		private void MeasurementsControl_Load(object sender, EventArgs e)
		{
			if (UseSecondUSPSChannel)
			{
				Task3_PC_32 = Task3_PC2_32;
				Task3_Y0_32 = Task3_Y20_32;
				Task4_PC_32 = Task4_PC2_32;
				Task4_Y9_32 = Task4_Y29_32;
			}

			btnCT3[0] = btn_Memory;

			bBortSystems.Visible = DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest || DevicesId == DevicesID.Diss32_90_WorkTest;
			if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
			{
				arButtonTask = new Button[10];
				btn_Task_1.Size = new Size(90, 40);
				btn_Task_2.Size = new Size(90, 40);
				btn_Task_3.Size = new Size(90, 40);
				btn_Task_4.Size = new Size(90, 40);

				btn_Task_2.Location = new Point(135, 313);
				btn_Task_3.Location = new Point(253, 313);
				btn_Task_4.Location = new Point(359, 313);

				btn_Light.Location = new Point(718, 313);
				bBortSystems.Location = new Point(482, 313);


				Button btnReadyWorkCheck = new Button();
				btnReadyWorkCheck.Name = "btnReadyWorkCheck";
                btnReadyWorkCheck.Size = new Size(196, 40);
				btnReadyWorkCheck.Font = new Font("Microsoft Sans Serif", 9.0f);
				btnReadyWorkCheck.Text = "Время готовности к работе";
				btnReadyWorkCheck.TextAlign = ContentAlignment.MiddleCenter;
                btnReadyWorkCheck.Click += ReadyWorkCheck_Click;
				btnReadyWorkCheck.BackColor = btn_Task_1.BackColor;
				btnReadyWorkCheck.UseVisualStyleBackColor = true;

				Button btnCaptureMode = new Button();
				btnCaptureMode.Name = "btnCaptureMode";
                btnCaptureMode.Size = new Size(196, 40);
				btnCaptureMode.Font = new Font("Microsoft Sans Serif", 9.0f);
				btnCaptureMode.Text = "Переход из режима «Память» в режим «Захват»";
				btnCaptureMode.TextAlign = ContentAlignment.MiddleCenter;
                btnCaptureMode.Click += BtnCaptureMode_Click;
				btnCaptureMode.BackColor =  btn_Task_1.BackColor;
				btnCaptureMode.UseVisualStyleBackColor = true;

                Controls.Add(btnReadyWorkCheck);
				Controls.Add(btnCaptureMode);

				btnReadyWorkCheck.Location = new Point(718, 373);
				btnCaptureMode.Location = new Point(482, 373);

                arButtonTask[0] = btn_Task_1;
                arButtonTask[1] = btn_Task_2;
                arButtonTask[2] = btn_Task_3;
                arButtonTask[3] = btn_Task_4;
                arButtonTask[4] = btn_BPK;
                arButtonTask[5] = btn_Zero;
                arButtonTask[6] = btn_Light;
                arButtonTask[7] = bBortSystems;
				arButtonTask[8] = btnReadyWorkCheck;
				arButtonTask[9] = btnCaptureMode;

				btn_BPK.Text = "БПК и импульсные выходы";
            }
			else
			{
                arButtonTask[0] = btn_Task_1;
                arButtonTask[1] = btn_Task_2;
                arButtonTask[2] = btn_Task_3;
                arButtonTask[3] = btn_Task_4;
                arButtonTask[4] = btn_BPK;
                arButtonTask[5] = btn_Zero;
                arButtonTask[6] = btn_Light;
                arButtonTask[7] = bBortSystems;
            }

			IsLoad = true;
		}

        private void BtnCaptureMode_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.FlatStyle == FlatStyle.Standard)
            {
                PressButtonTask(btn);
                var control = new CaptureModeControl(TestTasks, _measure, IsOneChannel);
                panel_Task.Visible = false;
                _activeControl = control;
                Controls.Add(_activeControl);
                control.Location = new Point(3, 3);
                control.Start();
            }
            else
            {
                if (_activeControl is CaptureModeControl)
                {
                    ((CaptureModeControl)_activeControl).Stop();
					if(((CaptureModeControl)_activeControl).IsActiveCT3)
					{
                        StopWorker(false);
                        TaskOff("Task_3_32");
                    }
                    Controls.Remove(_activeControl);
                    _activeControl = null;
                    ImageTaskStop();
                    WrungButtonTask(btn);
                }
            }
        }

        private void ReadyWorkCheck_Click(object sender, EventArgs e)
        {
			Button btn = (Button)sender;
            if(btn.FlatStyle == FlatStyle.Standard)
			{
				PressButtonTask(btn);
				var control = new ReadyWorkTimeControl(TestTasks, _measure);
				panel_Task.Visible = false;
				_activeControl = control;
				Controls.Add(_activeControl);
				control.Location = new Point(3, 3);
				control.Start();
			}
			else
			{
				if(_activeControl is ReadyWorkTimeControl)
				{
					((ReadyWorkTimeControl)_activeControl).Stop();
					Controls.Remove(_activeControl);
					_activeControl = null;
                    ImageTaskStop();
                    WrungButtonTask(btn);
				}
			}
        }

        #region TASK-1

        private void btn_Task_1_Click(object sender, EventArgs e)
		{
			btn_Task_1.Enabled = false;
			if (btn_Task_1.FlatStyle == FlatStyle.Standard)
			{
				if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					grpBox_Corner.Visible = false;
				}
				PressButtonTask(btn_Task_1);
				ImageTask_1();

				if (!TaskOn("Task_W_1_32", "Task_1_32"))
					EventErrorMessage?.Invoke(this,
						string.Format(Res.ErrorOnDevice, Res.ControlTaskBoard, Res.SetValue));

                MessageBox.Show(Resources.Task1Instruction, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);

				InitializeTask1(TestTasks.TypeTestTasks.Task1);
				StartWorker();
				indValue1.Focus();
			}
			else
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				StopWorker();
				
				if (!TaskOff("Task_1_32"))
					EventErrorMessage?.Invoke(this,
						string.Format(Res.ErrorOnDevice, Res.ControlTaskBoard, Res.SetValue));
				rbtn_Zero.PerformClick();
				bool result = true;
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					result &= dbKPK2.ManualRecordDouble("Mnl_Forward_32_RepairTest", indValue1.Value - 17);
					result &= dbKPK2.ManualRecordDouble("Mnl_Left_32_RepairTest", indValue2.Value - 17);
					result &= dbKPK2.ManualRecordDouble("Mnl_Down_32_RepairTest", indValue3.Value - 3);
				}
				else
                {
                    result &= dbKPK2.ManualRecordDouble("Mnl_Forward_32", indValue1.Value);
                    result &= dbKPK2.ManualRecordDouble("Mnl_Left_32", indValue2.Value);
                    result &= dbKPK2.ManualRecordDouble("Mnl_Down_32", indValue3.Value);
                }
				IndicatorUp.CalculationMethod =
					IndicatorLeft.CalculationMethod =
						IndicatorDown.CalculationMethod = CalcMethod.MinMax; //Сброс в базовое состояние

				if (!result)
					EventErrorMessage?.Invoke(this, Resources.CantSaveDbParam);
                ImageTaskStop();
                WrungButtonTask(btn_Task_1);
			}

			btn_Task_1.Enabled = true;
		}

		private void ImageTask_1()
		{

            if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                TaskCustomView(true, 3, true, false, true, false, false);
            else
                TaskCustomView(true, 3, true, true, true, false, false);

			grpBox_Task.Text = string.Format(Resources.ControlTaskN, 1);
			grpBox_Value.Text = Resources.MeasuredSpeedParams;
			grpBox_Speed.Text = string.Format(Resources.SpeedVectorsFromBlockN, 6);
			rbtn_Tangazh.Text = Resources.Pitch;

			SetupIndicatorValue(indValue1, Globals.ValueType.SpeedForward);
			SetupIndicatorValue(indValue2, Globals.ValueType.SpeedLeft);
			SetupIndicatorValue(indValue3, Globals.ValueType.SpeedDown);

			lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN, 6);

			IncludeRadioButtonCorner(rbtn_Zero);
			rbtn_Zero.Checked = rbtn_Tangazh.Checked = rbtn_Kren.Checked = false;

			if (Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
			{
				tiCorner.VisualText = string.Format(Resources.AngleGrad, 0);

				cmbBox_Corner.Items.Clear();
				cmbBox_Corner.Items.AddRange(new[]
				{
					string.Format(Resources.AngleGrad, "+12"), string.Format(Resources.AngleGrad, 0),
					string.Format(Resources.AngleGrad, -12)
				});
				cmbBox_Corner.SelectedItem = string.Format(Resources.AngleGrad, 0);
				cmbBox_1.Items.Clear();
				cmbBox_1.Items.AddRange(_arComboBoxCorner);
				cmbBox_2.Items.Clear();
				cmbBox_2.Items.AddRange(_arComboBoxCorner);
				cmbBox_3.Items.Clear();
				cmbBox_3.Items.AddRange(_arComboBoxCorner);
				cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = cmbBox_Corner.Enabled =
					btn_Corner.Enabled = false;
				cmbBox_1.Visible = cmbBox_2.Visible = cmbBox_3.Visible = true;
				label2.Visible = label3.Visible = label4.Visible = true;
			}
		}

		private void InitializeTask1(IndicatorViewerCreator viewerCreator)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					IndicatorUp.SetValue(viewerCreator.Viewers[0]);
					IndicatorLeft.SetValue(viewerCreator.Viewers[1]);
					IndicatorDown.SetValue(viewerCreator.Viewers[2]);
				}));
			}
			else
			{
				IndicatorUp.SetValue(viewerCreator.Viewers[0]);
				IndicatorLeft.SetValue(viewerCreator.Viewers[1]);
				IndicatorDown.SetValue(viewerCreator.Viewers[2]);
			}

			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var viewer in viewerCreator.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(viewer.UsbBaseNew);
			}

			_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
		}

		#endregion

		#region Task-2

		private void btn_Task_2_Click(object sender, EventArgs e)
		{
			btn_Task_2.Enabled = false;
			if (btn_Task_2.FlatStyle == FlatStyle.Standard)
			{
				PressButtonTask(btn_Task_2);
				ImageTask_2();
				if (!TaskOn("Task_W_1_32", "Task_2_32"))
					return;
				SetDirection();
				MessageBox.Show(Resources.Task2Instruction, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				InitializeTask2();

				StartWorker();
				indValue1.Focus();
			}
			else
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				if (_activeControl is Task2)
				{
					((Task2)_activeControl).Deactivate();
				}

				TimeSpan dataWaitTimeout = TimeSpan.FromSeconds(5);
				DateTime finishTime = DateTime.Now + dataWaitTimeout;
				while (double.IsNaN(_measure.GetValue("Task2_X_32")) && (DateTime.Now < finishTime))
					Thread.Sleep(200);
				GenSinCos.Instance().GeneratorOff();
				StopWorker();
				TaskOff("Task_2_32");
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{

					dbKPK2.ManualRecordDouble("Mnl_Back_32_RepairTest", indValue1.Value - 17);
					dbKPK2.ManualRecordDouble("Mnl_Right_32_RepairTest", indValue2.Value - 17);
					dbKPK2.ManualRecordDouble("Mnl_Up_32_RepairTest", indValue3.Value - 3);
				}
				else
				{
					dbKPK2.ManualRecordDouble("Mnl_Back_32", indValue1.Value);
					dbKPK2.ManualRecordDouble("Mnl_Right_32", indValue2.Value);
					dbKPK2.ManualRecordDouble("Mnl_Up_32", indValue3.Value);
				}
                ImageTaskStop();
                WrungButtonTask(btn_Task_2);
			}

			btn_Task_2.Enabled = true;
		}

		private void ImageTask_2() //todo убрать в другой класс
		{
			TaskCustomView(true, 3, true, false, false, false, false);
			if (DevicesId != DevicesID.Diss32 && DevicesId != DevicesID.Diss32_WorkTest 
				&& Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				Point p = new Point(351, 0);
				Task2 task2 = new Task2(_measure, MeasuringValueInfo, dbKPK2); //TODO убрать форму,
				task2.SetReverseDirection += SetReverseDirection;
				task2.EventInitializeTask2 += (sender, e) => 
				{
					InitializeTask2();
					StartWorker();
				};
				task2.EventInitializeZPNPTask += InitializZPNPTask;
				task2.EventInitializeZZeroTask += InitializeZZeroTask;
				task2.EventInitializeZTask += InitializeZTask;
				task2.EventErrorDetected += ErrorDetected;
				_activeControl = task2;

                panel_Task.Controls.Add(_activeControl);
				_activeControl.Location = p;
			}

			grpBox_Task.Text = string.Format(Resources.ControlTaskN, 2);
			grpBox_Value.Text = Resources.MeasuredSpeedParams;
			grpBox_Speed.Text = string.Format(Resources.SpeedVectorsFromBlockN, 6);

			SetupIndicatorValue(indValue1, Globals.ValueType.SpeedBackward);
			SetupIndicatorValue(indValue2, Globals.ValueType.SpeedRight);
			SetupIndicatorValue(indValue3, Globals.ValueType.SpeedUp);
			
			lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN, 6);
		}

		private void InitializeTask2()
		{
			SettingsObj.GetSettings().MeasList.Clear();
			IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task2.Viewers[0]);
			IndicatorLeft.SetValue(TestTasks.TypeTestTasks.Task2.Viewers[1]);
			IndicatorDown.SetValue(TestTasks.TypeTestTasks.Task2.Viewers[2]);
			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var indicatorViewer in TestTasks.TypeTestTasks.Task2.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
			}

			_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
		}

        public void InitializZPNPTask(object sender, EventArgs e)
        {
            MeasuringValueInfo.ZoptPNPStartValue = double.NaN;
			SettingsObj.GetSettings().MeasList.Clear();
            SetIndicatorLimits((int)Globals.IndicatorNumber.ZoptPNP, TestTasks.TypeTestTasks.ZPNPTask.Viewers[0].ParameterName);
			foreach(var indicatorViewer in TestTasks.TypeTestTasks.ZPNPTask.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
			}
			//_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
            StartWorker();
        }

		public void InitializeZZeroTask(object sender, EventArgs e)
		{
            SettingsObj.GetSettings().MeasList.Clear();
            SetIndicatorLimits((int)Globals.IndicatorNumber.ZoptPVK, TestTasks.TypeTestTasks.ZPNPTask.Viewers[0].ParameterName);
            foreach (var indicatorViewer in TestTasks.TypeTestTasks.ZZeroTask.Viewers)
            {
                SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
            }
            //_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
            StartWorker();
        }

		private void InitializeZTask(object sender, EventArgs e)
        {
            SettingsObj.GetSettings().MeasList.Clear();
            SetIndicatorLimits((int)Globals.IndicatorNumber.ZoptPVK, TestTasks.TypeTestTasks.ZPNPTask.Viewers[0].ParameterName);
            foreach (var indicatorViewer in TestTasks.TypeTestTasks.ZTask.Viewers)
            {
                SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
            }
           // _measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
            StartWorker();
        }

        private void SetDirection()
		{
			int angle = _currentDirection - 90;
			if (angle < 0)
				angle += 360;
			Diss32Functions.SetAngle((ushort)angle); //теоретический разворот через промежуточный угол
			Thread.Sleep(700);
			Diss32Functions.SetAngle(_currentDirection);
		}

		private bool SetReverseDirection()
		{
			if (_directionReversed)
				return false;
			_currentDirection = ReverseDirection(_currentDirection);
			SetDirection();
			_directionReversed = true;
			return true;
		}

		#endregion

		#region Task-3

		private void btn_Task_3_Click(object sender, EventArgs e)
		{
			btn_Task_3.Enabled = false;
			if (btn_Task_3.FlatStyle == FlatStyle.Standard)
			{
				GetTypeVSSFromUser();

				PressButtonTask(btn_Task_3);
				ImageTask_3();
				if (!TaskOn("Task_W_1_32", "Task_3_32"))
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				MessageBox.Show(Resources.Task3_4Instruction, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				InitializeTask3();
				indValue1.Focus();
			}
			else
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;

				if (UseSecondUSPSChannel)
				{
					dbKPK2.ManualRecordDouble(Task3_PC_32_Const, _measure.GetValue(Task3_PC_32));
					dbKPK2.ManualRecordDouble(Task3_Y0_32_Const, _measure.GetValue(Task3_Y0_32));
				}

				if (siWorkTime.Visible)
					HideTimerToWork();

				StopWorker();
				if (!TaskOff("Task_3_32"))
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				bool result = true;
				// запись в БД что ввели ручками   
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task3_32_RepairTest", indValue1.Value - 127);
                    result &= dbKPK2.ManualRecordDouble("Mnl_Alfa_0_32_RepairTest", indValue2.Value);
                }
				else
				{
					result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task3_32", indValue1.Value);
					result &= dbKPK2.ManualRecordDouble("Mnl_Alfa_0_32", indValue2.Value);
				}
				result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task3_32_FlightVSS", indValue1.Value);
				
				if (GetIPControl() != null)
				{
					result &= dbKPK2.ManualRecordDouble("Mnl_Spd2_Task3_32", GetIPControl().indValue1.Value);
					result &= dbKPK2.ManualRecordDouble("Mnl_Alfa2_0_32", GetIPControl().indValue2.Value);
				}

				if (_arBlock8[0] && _arBlock8[1] && _arBlock8[2] && _arBlock8[3])
				{
					// Измерения пишем для Блока 8 
					result &= dbKPK2.ManualRecordDouble("Point25_X", _arBlock8Value[0]);
					result &= dbKPK2.ManualRecordDouble("Point25_Z", _arBlock8Value[1]);
					result &= dbKPK2.ManualRecordDouble("Point25_X_Course", _arBlock8Value[2]);
					result &= dbKPK2.ManualRecordDouble("Point25_Z_Course", _arBlock8Value[3]);
				}

				if (!result)
					EventErrorMessage?.Invoke(this, Resources.CantSaveDbParam);
                ImageTaskStop();
                WrungButtonTask(btn_Task_3);
			}

			btn_Task_3.Enabled = true;
		}

		private void ImageTask_3()
		{
			//bool btnReady = Mode.GetMode().ModeApplication == ModeApplication.WorkCheck;

			TaskCustomView(true, 2, true, false, false, true, false);
			
			grpBox_Task.Text = string.Format(Resources.ControlTaskN, 3); // "Контрольная задача КЗ-3";
			if (Modification != Modification.Diss32_90A && DevicesId != DevicesID.Diss32 &&
			    DevicesId != DevicesID.Diss32_WorkTest)
			{
				_activeControl = new IndicationPanel();
				panel_Task.Controls.Add(_activeControl);
				int spaceBWElements = 15;
				_activeControl.Location = new Point(grpBox_Task.Location.X + grpBox_Task.Width + spaceBWElements,
					grpBox_Task.Location.Y);

				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    ((IndicationPanel)_activeControl).grpBox_Value.Text =
                        string.Format(Resources.MeasuredValuesPSAndUS);
                    ((IndicationPanel)_activeControl).grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
                        string.Format(Resources.US,
                            string.Format(Resources.PS,
                                "")));
                }
				else
				{
					((IndicationPanel)_activeControl).grpBox_Value.Text =
						string.Format(Resources.MeasuredValuesPSAndUSN, 2);
					((IndicationPanel)_activeControl).grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
						string.Format(Resources.US,
							string.Format(Resources.PS,
								"№2")));
                    ((IndicationPanel)_activeControl).indValue3.Visible = false;

                    ((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH); // 1-й индикатор
                    ((IndicationPanel)_activeControl).IndicatorLeft.NameLabelUp = Resources.AlfaGrad; // 2-й индикатор
                    ((IndicationPanel)_activeControl).IndicatorUp.NameLabelDown = string.Format(Resources.PS, 1); // 1-й индикатор
                    ((IndicationPanel)_activeControl).IndicatorLeft.NameLabelDown = string.Format(Resources.US, 0); // 2-й индикатор

                    ((IndicationPanel)_activeControl).IndicatorDown.Visible = false;
                    SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, Task3_PC2_32);
                    SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, Task3_Y20_32);
                }

				SetupIndicatorValue(((IndicationPanel)_activeControl).indValue1, Globals.ValueType.FastSpeed1);
				SetupIndicatorValue(((IndicationPanel)_activeControl).indValue2, Globals.ValueType.ZeroAngle);

				((IndicationPanel)_activeControl).lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN,
					string.Format(Resources.US,
						string.Format(Resources.PS,
							"№2")));

				btn_Memory.Location = new Point(_activeControl.Location.X + _activeControl.Width + spaceBWElements,
					btn_Memory.Location.Y);
				((IndicationPanel)_activeControl).grpBox_Task.Text = grpBox_Task.Text;
			}

			grpBox_Value.Text = Resources.MeasuredValuesPSAndUS;
			grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
				string.Format(Resources.US, string.Format(Resources.PS, "")));
			// "Данные с индикатора УС ПС";
			grpBox_Speed.Enabled = true;
			grpBox_Corner.Visible = false;

			SetupIndicatorValue(indValue1, Globals.ValueType.FastSpeed1);
			SetupIndicatorValue(indValue2, Globals.ValueType.ZeroAngle);

			lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN,
				string.Format(Resources.US, string.Format(Resources.PS, "")));

			tiCorner.VisualText = ZeroGrad;

			cmbBox_Corner.Items.Clear();
			cmbBox_Corner.Items.AddRange(new[]
				{ Plus15Grad, Plus11Grad, Plus5Grad, ZeroGrad, Minus5Grad, Minus11Grad, Minus15Grad });
			cmbBox_Corner.SelectedItem = ZeroGrad;

			cmbBox_1.Visible = cmbBox_2.Visible = cmbBox_3.Visible = false;
			label2.Visible = label3.Visible = label4.Visible = false;
			cmbBox_Corner.Enabled = btn_Corner.Enabled = false;
			if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
			{
				btn_Memory.Visible = false;
				//btn_Memory.Text = @"режим ""Захват""";
			}
			else
				btn_Memory.Text = Resources.MemoryMode;


			IncludeRadioButtonCorner(rbtn_Zero);
			rbtn_Zero.Checked = rbtn_Tangazh.Checked = rbtn_Kren.Checked = false;
			rbtn_Zero.Enabled = rbtn_Kren.Enabled = true;

			arTxtBox_Speed[0] = arTxtBox_Speed[1] = false;
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, Task3_PC_32);
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, Task3_Y0_32);
        }

		private void InitializeTask3()
		{
			SettingsObj.GetSettings().MeasList.Clear();
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task3.Viewers[0]);
					IndicatorLeft.SetValue(TestTasks.TypeTestTasks.Task3.Viewers[1]);
				}));
			}
			else
			{
				IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task3.Viewers[0]);
				IndicatorLeft.SetValue(TestTasks.TypeTestTasks.Task3.Viewers[1]);
			}

			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var indicatorViewer in TestTasks.TypeTestTasks.Task3.Viewers)
			{

				SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
			}

			//_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
			StartWorker();
		}

		#endregion //Todo сделать Task-3

		#region Task-4

		private void btn_Task_4_Click(object sender, EventArgs e)
		{
			btn_Task_4.Enabled = false;
			if (btn_Task_4.FlatStyle == FlatStyle.Standard)
			{
                btn_Memory.Text = Resources.MemoryMode;
				GetTypeVSSFromUser();
				cbShowBlock8.Enabled = false;
				cbShowBlock8.Checked = false;
				cbShowBlock8.Enabled = true;
				PressButtonTask(btn_Task_4);
				ImageTask_4();
				if (!TaskOn("Task_W_1_32", "Task_4_32"))
					EventErrorMessage?.Invoke(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				InitializeTask4();
				//StartWorker();
				MessageBox.Show(Resources.Task3_4Instruction, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				indValue1.Focus();
			}
			else
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;

				if (UseSecondUSPSChannel)
				{
					dbKPK2.ManualRecordDouble(Task4_PC_32_Const,
						_measure.GetValue(Task4_PC_32)); //todo вынести строки в отдельный класс
					dbKPK2.ManualRecordDouble(Task4_Y9_32_Const, _measure.GetValue(Task4_Y9_32));
				}

				if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
                    double valueSave = double.NaN;
                    bool saveDifferencePS = !Double.IsNaN(indValue1.Value) && !Double.IsNaN(MemoryPS.Value);
                    bool saveDifferenceUS = !double.IsNaN(indValue2.Value) && !double.IsNaN(MemoryUS.Value);
                    if (saveDifferencePS)
                    {
						valueSave = MemoryPS.Value - indValue1.Value;
                        dbKPK2.ManualRecordDouble("DeltaTask4_PS_WorkTest", valueSave);
                    }
                    if (saveDifferenceUS)
                    {
						valueSave = MemoryUS.Value - indValue2.Value;
                        dbKPK2.ManualRecordDouble("DeltaTask4_US_WorkTest", valueSave);
                    }
					Control modeCaptor = grpBox_Speed.Controls["nameModeCaptor"];
					Control modeMemory = grpBox_Task.Controls["nameModeMemory"];
					grpBox_Task.Controls.Remove(modeMemory);
					grpBox_Speed.Controls.Remove(modeCaptor);
					MemoryPS.Visible = false;
					MemoryUS.Visible = false;
					MemoryPS.Value = 0;
					MemoryUS.Value = 0;
					MemoryUS.tbValue.TextChanged -= TbValueUS_TextChanged;
					MemoryPS.tbValue.TextChanged -= TbValuePS_TextChanged;
                    grpBox_Speed.Size = new Size(grpBox_Speed.Size.Width - 20, grpBox_Speed.Size.Height);
                    grpBox_Task.Size = new Size(grpBox_Task.Size.Width - 20, grpBox_Task.Size.Height);
                }

				StopWorker();
				if (GetBlock8Control() != null)
				{
					if (GetBlock8Control().btn_8_Off_6.FlatStyle == FlatStyle.Standard)
						GetBlock8Control().btn_8_Off_6.PerformClick();
				}

				if (!TaskOff("Task_4_32"))
					EventErrorMessage?.Invoke(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				bool result = true;
				// запись в БД что ввели вручную
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{

                    result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task4_32_RepairTest", indValue1.Value - 258);
                    result &= dbKPK2.ManualRecordDouble("Mnl_Alfa_9_32_RepairTest", indValue2.Value - 9.5);
                }
				else
				{
					result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task4_32", indValue1.Value);
					result &= dbKPK2.ManualRecordDouble("Mnl_Alfa_9_32", indValue2.Value);
                }
                result &= dbKPK2.ManualRecordDouble("Mnl_Spd_Task4_32_FlightVSS", indValue1.Value);

                if (GetIPControl() != null)
				{
					result &= dbKPK2.ManualRecordDouble("Mnl_Spd2_Task4_32", GetIPControl().indValue1.Value);
					result &= dbKPK2.ManualRecordDouble("Mnl_Alfa2_9_32", GetIPControl().indValue2.Value);
				}

				// п.24 Отчёта 
				if (GetBlock8Control() != null)
					if (!GetBlock8Control().Save())
						EventErrorMessage?.Invoke(this, Resources.CantSaveDbParam);
				ImageTaskStop();
				WrungButtonTask(btn_Task_4);
			}

			btn_Task_4.Enabled = true;
		}

		private void ImageTask_4()
		{
			TaskCustomView(true, 2, true, false, false, false, true);
			Point p = new Point(496, 12);

			if (DevicesId != DevicesID.Diss32 && Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
			{
				cbShowBlock8.Visible = true;
				_activeControl2 =
					new Block8(Modification != Modification.Diss32_90A && DevicesId != DevicesID.Diss32);
				panel_Task.Controls.Add(_activeControl2);
				_activeControl2.Location = p;
				_activeControl2.Visible = false;
				((Block8)_activeControl2).BlockId = SettingsObj.GetSettings().TestId;
				((Block8)_activeControl2).SetDirection += SetDirection;
				((Block8)_activeControl2).btn_8_On_6.Click += btn_8_On_Click;
				((Block8)_activeControl2).btn_8_Off_6.Click += btn_8_Off_Click;
				((Block8)_activeControl2).txtBox_6_1.Validating += txtBox_8_X_Validating;
				((Block8)_activeControl2).txtBox_6_2.Validating += txtBox_8_Z_Validating;
				((Block8)_activeControl2).SetReverseDirection += SetReverseDirection;
				((Block8)_activeControl2).GetMeasValue += GetMeasValue;
			}

			SetupIndicatorValue(indValue1, Globals.ValueType.FastSpeed2);
			SetupIndicatorValue(indValue2, Globals.ValueType.PositiveAngle);

            if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
            {
                grpBox_Task.Text = string.Format(Resources.ControlTaskN, 4);
                grpBox_Value.Text = Resources.MeasuredValuesPSAndUS;
                grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
                    string.Format(Resources.US, string.Format(Resources.PS, "")));
				grpBox_Speed.Size = new Size(grpBox_Speed.Size.Width + 20, grpBox_Speed.Size.Height);
				grpBox_Task.Size = new Size(grpBox_Task.Size.Width + 20, grpBox_Task.Size.Height);
				lbl_Set_Task.Visible = false;

				
                MemoryPS.Visible = true;
                indValue1.tbValue.TextChanged += TbValuePS_TextChanged;
				MemoryPS.tbValue.Text = "";
				MemoryUS.Visible = true;
                indValue2.tbValue.TextChanged += TbValueUS_TextChanged;
				MemoryUS.tbValue.Text = "";

                Label nameModeCaptor = new Label();
                nameModeCaptor.Name = "nameModeCaptor";
                nameModeCaptor.Text = "в режиме ЗАХВАТ";
				nameModeCaptor.AutoSize = true;
				
                grpBox_Speed.Controls.Add(nameModeCaptor);
                nameModeCaptor.Location = new Point(indValue2.Location.X + 15 + 90, indValue2.Location.Y + (indValue2.Size.Height / 2));

                Label nameModeMemory = new Label();
                nameModeMemory.Name = "nameModeMemory";
                nameModeMemory.Text = "в режиме ПАМЯТЬ";
				nameModeMemory.AutoSize = true;

                grpBox_Task.Controls.Add(nameModeMemory);
                nameModeMemory.Location = new Point(MemoryUS.Location.X + 15 + 90, MemoryUS.Location.Y + (MemoryUS.Size.Height / 2));
            }
            else
            {
                grpBox_Task.Text = string.Format(Resources.ControlTaskN, 4);
                grpBox_Value.Text = string.Format(Resources.MeasuredValuesPSAndUSN, 1);
                grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
                    string.Format(Resources.US, string.Format(Resources.PS, "№1")));
            }
            
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, Task4_PC_32);
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, Task4_Y9_32);

            IndicatorDown.NameLabelDown = "";

			IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH); // 1-й индикатор
			IndicatorLeft.NameLabelUp = Resources.AlfaGrad; // 2-й индикатор



			lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN,
				string.Format(Resources.US, string.Format(Resources.PS, "№1")));

			// "Дождитесь отработки указателей индикатора УС ПС";

			btn_Mode_M_C.Text = Resources.MSMode;
			btn_Mode_M_B.Text = Resources.MBMode;
			arTxtBox_Speed[0] = arTxtBox_Speed[1] = false;

			if (Modification == Modification.Diss32_90A && Mode.GetMode().ModeApplication != ModeApplication.RepairCheck)
			{
				_activeControl = _activeControl2;
				cbShowBlock8.Visible = false;
				_activeControl2.Visible = true;
				return;
			}

			if (DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest || DevicesId == DevicesID.Diss32_90_WorkTest)
			{
				cbShowBlock8.Visible = false;
				return;
			}

			// Только для основного ДИСС (не А)
			_activeControl = new IndicationPanel();
			panel_Task.Controls.Add(_activeControl);
			_activeControl.Location = p;

			((IndicationPanel)_activeControl).grpBox_Task.Text = grpBox_Task.Text;
			((IndicationPanel)_activeControl).grpBox_Value.Text = string.Format(Resources.MeasuredValuesPSAndUSN, 2);
			((IndicationPanel)_activeControl).grpBox_Speed.Text = string.Format(Resources.DataFromIndicatorN,
				string.Format(Resources.US,
					string.Format(Resources.PS,
						"№2")));
			SetupIndicatorValue(((IndicationPanel)_activeControl).indValue1, Globals.ValueType.FastSpeed2);
			SetupIndicatorValue(((IndicationPanel)_activeControl).indValue2, Globals.ValueType.PositiveAngle);

			((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
			((IndicationPanel)_activeControl).IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
			((IndicationPanel)_activeControl).lbl_Set_Task.Text = string.Format(Resources.WaitWhileStabilizeUnitN,
				string.Format(Resources.US, string.Format(Resources.PS, "№2")));
            
            ((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH); // 1-й индикатор
            ((IndicationPanel)_activeControl).IndicatorLeft.NameLabelUp = Resources.AlfaGrad; // 2-й индикатор
            ((IndicationPanel)_activeControl).IndicatorUp.NameLabelDown = string.Format(Resources.PS, 2); // 1-й индикатор
            ((IndicationPanel)_activeControl).IndicatorLeft.NameLabelDown = string.Format(Resources.US, 9.5 + " " + Resources.Left); // 2-й индикатор

            ((IndicationPanel)_activeControl).IndicatorDown.Visible = false;
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, Task4_PC2_32);
            SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, Task4_Y29_32);
        }

        private void TbValuePS_TextChanged(object sender, EventArgs e)
        {
			if (indValue1.tbValue.Text == "" || double.IsNaN(indValue1.Value))
			{
				MemoryPS.MinGreenValue = 0;
				MemoryPS.MaxGreenValue = 0;
				return;
			}
			MemoryPS.MaxGreenValue = Math.Round(indValue1.Value + 3, 4);
			MemoryPS.MinGreenValue = Math.Round(indValue1.Value - 3, 4);
			MemoryPS.ValidateChildren();
        }

		private void TbValueUS_TextChanged(object sender, EventArgs e)
		{
			if (indValue2.tbValue.Text == "" || double.IsNaN(indValue1.Value))
			{
				MemoryUS.MinGreenValue = 0;
				MemoryUS.MaxGreenValue = 0;
				return;
			}
			MemoryUS.MaxGreenValue = Math.Round(indValue2.Value + 3, 4);
			MemoryUS.MinGreenValue = Math.Round(indValue2.Value - 3, 4);
            MemoryUS.ValidateChildren();
        }

        private void InitializeTask4()
		{
			SettingsObj.GetSettings().MeasList.Clear();
			IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task4.Viewers[0]);
			IndicatorLeft.SetValue(TestTasks.TypeTestTasks.Task4.Viewers[1]);
			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var indicatorViewer in TestTasks.TypeTestTasks.Task4.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
			}

			//_measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
			StartWorker();
		}

		// Включили Блок 8 
		private void btn_8_On_Click(object sender, EventArgs e)
		{
			indValue1.Enabled = indValue2.Enabled = indValue3.Enabled = false;
		}

		// Выключили Блок 8 
		private void btn_8_Off_Click(object sender, EventArgs e)
		{
			indValue1.Enabled = indValue2.Enabled = indValue3.Enabled = true;
		}

		private void txtBox_8_X_Validating(object sender, CancelEventArgs e)
		{
			Control currentControl = _activeControl;
			if (_activeControl2 is Block8)
				currentControl = _activeControl2;
			if (IsOutputBlock8() && ((Block8)currentControl).txtBox_6_1.Text != "")
			{
				double value = Functions.MoveToDouble(((Block8)currentControl).txtBox_6_1.Text);
				if (double.IsNaN(value))
				{
					ErrorInputTextBox(((Block8)currentControl).txtBox_6_1);
					return;
				}

				if (value < 0 || value > 100)
				{
					ErrorCorridorTextBox(((Block8)currentControl).txtBox_6_1, 0, 100, "\"Путь\"");
					return;
				}

				if (btn_Task_3.FlatStyle == FlatStyle.Flat) // для КЗ-3 
				{
					int k = rbtn_Tangazh.Checked ? 2 : 0;
					_arBlock8[k] = true;
					_arBlock8Value[k] = ((Block8)currentControl).cmbBox_6_1.SelectedItem.ToString() ==
					                    Resources.Forward.ToUpper()
						? value
						: -value;
				}

				// красный/зеленый коридор 
				((Block8)_activeControl).txtBox_6_1.ForeColor = btn_Task_4.FlatStyle == FlatStyle.Flat && // для КЗ-4 
				                                                ((Block8)currentControl).cmbBox_6_1.SelectedItem
				                                                .ToString() ==
				                                                Resources.Forward.ToUpper() &&
				                                                62.3 <= value && value <= 64.9
					? Color.LimeGreen
					: btn_Task_3.FlatStyle == FlatStyle.Flat &&
					  // для КЗ-3 при "Обнулении"
					  ((Block8)currentControl).cmbBox_6_1.SelectedItem.ToString() ==
					  Resources.Forward.ToUpper() && rbtn_Zero.Checked &&
					  20.4 <= value && value <= 30.0
						? Color.LimeGreen
						: btn_Task_3.FlatStyle == FlatStyle.Flat &&
						  // для КЗ-3 при "Курс +10"
						  ((Block8)currentControl).cmbBox_6_1.SelectedItem.ToString() ==
						  Resources.Forward.ToUpper() && rbtn_Tangazh.Checked &&
						  _arBlock8[0] && value < _arBlock8Value[0]
							? Color.LimeGreen
							: btn_Task_3.FlatStyle == FlatStyle.Flat &&
							  // не ввели значение при "Обнулении" для КЗ-3 
							  !_arBlock8[0] && rbtn_Tangazh.Checked
								? SystemColors.ControlText
								: Color.Red;
			}
		}

		private void txtBox_8_Z_Validating(object sender, CancelEventArgs e)
		{
			Control currentControl = _activeControl;
			if (_activeControl2 is Block8)
				currentControl = _activeControl2;
			if (IsOutputBlock8() && ((Block8)currentControl).txtBox_6_2.Text != "")
			{
				double value = Functions.MoveToDouble(((Block8)currentControl).txtBox_6_2.Text);
				if (double.IsNaN(value))
				{
					ErrorInputTextBox(((Block8)currentControl).txtBox_6_2);
					return;
				}

				if (value < 0 || value > 50)
				{
					ErrorCorridorTextBox(((Block8)currentControl).txtBox_6_2, 0, 50, "\"" + Resources.Tangazh + "\"");
					return;
				}

				if (btn_Task_3.FlatStyle == FlatStyle.Flat) // для КЗ-3 
				{
					int k = rbtn_Tangazh.Checked ? 3 : 1;
					_arBlock8[k] = true;
					_arBlock8Value[k] = value; // коридор +/- 0,6 
				}

				// красный/зеленый коридор  Влево 
				((Block8)_activeControl).txtBox_6_2.ForeColor = btn_Task_4.FlatStyle == FlatStyle.Flat && // для КЗ-4 
				                                                ((Block8)_activeControl).cmbBox_6_2.SelectedItem
				                                                .ToString() ==
				                                                Resources.Left.ToUpper() &&
				                                                9.5 <= value && value <= 11.7
					? Color.LimeGreen
					: btn_Task_3.FlatStyle == FlatStyle.Flat &&
					  // для КЗ-3 при "Обнулении"
					  rbtn_Zero.Checked &&
					  -0.6 <= value && value <= 0.6
						? Color.LimeGreen
						: btn_Task_3.FlatStyle == FlatStyle.Flat &&
						  // для КЗ-3 при "Курс +10"
						  rbtn_Tangazh.Checked &&
						  _arBlock8[0] && value > _arBlock8Value[0]
							? Color.LimeGreen
							: btn_Task_3.FlatStyle == FlatStyle.Flat &&
							  // не ввели значение при "Обнулении" для КЗ-3 
							  !_arBlock8[0] && rbtn_Tangazh.Checked
								? SystemColors.ControlText
								: Color.Red;
			}
		}

		private Block8 GetBlock8Control()
		{
			if (_activeControl is Block8)
				return (Block8)_activeControl;
			return (Block8)_activeControl2;
		}

		private double GetMeasValue(string parameter)
		{
			return _measure.GetValue(parameter);
		}

		private bool IsOutputBlock8()
		{
			frmWork32 frm = ((frmWork32)ParentForm);
			bool b = frm.IsOutputBlock8;
			return !b;
		}

		private void ErrorInputTextBox(TextBox txtBox)
		{
			MessageBox.Show(string.Format(Resources.FailValue, txtBox.Text), Resources.ApplicationName,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			txtBox.Focus();
			txtBox.SelectAll();
			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // для КЗ-3 
			{
				int k = 0;
				_arBlock8[k] = false;
			}
		}

		private void ErrorCorridorTextBox(TextBox txtBox, double dMin, double dMax, string str)
		{
			MessageBox.Show(string.Format(Resources.ValueLimit, txtBox.Text, dMin, dMax), Resources.ApplicationName,
				MessageBoxButtons.OK, MessageBoxIcon.Error);
			txtBox.Focus();
			txtBox.SelectAll();
			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // для КЗ-3 
			{
				int k = rbtn_Tangazh.Checked && txtBox == ((Block8)_activeControl).txtBox_6_1
					? 2
					: // Курс и Вперед 
					rbtn_Tangazh.Checked && txtBox == ((Block8)_activeControl).txtBox_6_2
						? 3
						: // Курс и Вправо 
						!rbtn_Tangazh.Checked && txtBox == ((Block8)_activeControl).txtBox_6_1
							? 0
							: 1;
				// Обнуление Вперед:Вправо 
				_arBlock8[k] = false;
			}
		}

		#endregion

		#region BPK

		private void btn_BPK_Click(object sender, EventArgs e)
		{
			if (btn_BPK.FlatStyle == FlatStyle.Standard)
			{
				MessageBox.Show(Sparc.KPK2.Resources.Properties.Resources.PressContrDiss);
				GetTypeVSSFromUser();
				PressButtonTask(btn_BPK);
				ImageBPK();
			}
			else
			{
				var bpkControl = ((BPK)_activeControl); 
				bpkControl.CalculationBPK();
				Application.DoEvents();
				MessageBox.Show(Sparc.KPK2.Resources.Properties.Resources.UnpressContrDiss);
				Thread.Sleep(2000); //Ждём чтобы пользователь увидел результаты

				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					btn_BPK.Text = "БПК и импульсные выходы";
                }
				else
					btn_BPK.Text = Resources.BPK; //вернули старое название

                ImageTaskStop();
                WrungButtonTask(btn_BPK);
            }
		}

		private void ImageBPK()
		{
			var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
			TaskCustomView(false, 0, false, false, false, false, false);

			Point p = new Point(5, 5);
			_activeControl = new BPK(dbKPK2.ManualRecordDouble, IsFlightVSS, IsDiss32_90);
			Controls.Add(_activeControl);
			_activeControl.Location = p;
			btn_BPK.Text = Resources.FindError; // сменили название управляющей кнопки
		}

		#endregion

		#region Wxr Wzr Wyr

		private void btn_Zero_Click(object sender, EventArgs e)
		{
			btn_Zero.Enabled = false;
			if (btn_Zero.FlatStyle == FlatStyle.Standard)
			{
				PressButtonTask(btn_Zero);
				ImageZero();
				Diss32Functions.SetAngle(0);
				if (!TaskOn("Task_W_1_32", "Task_2_32"))
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));

				if (
					MessageBox.Show(Resources.TaskZeroInstruction, Resources.ApplicationName,
						MessageBoxButtons.OKCancel,
						MessageBoxIcon.Information) == DialogResult.Cancel)
				{
					// отмена 
					TaskOff("Task_2_32");
					return;
				}

				bool result = true;
				result &= ControlTask.Instance().ControlTaskOff();
				result &= Diss32Functions.SetAngle(0);
				result &= DiscreteOutputs.Instance().SetOutput("UNCh_32", false);
				if (!result)
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				InitializZero();
			}
			else
			{
				if (!TaskOff("Task_2_32"))
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				StopWorker();
				ImageTaskStop();
				WrungButtonTask(btn_Zero);
			}

			btn_Zero.Enabled = true;
		}

		private void ImageZero()
		{
			TaskCustomView(false, 0, false, false, false, false, false);
			Point p = new Point(4, 4);
			WPanel wPanel = new WPanel(DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest,
				false);
			wPanel.TaskChanged += TaskChanged;
			_activeControl = wPanel;
			Controls.Add(_activeControl);
			_activeControl.Location = p;
		}

		private void InitializZero()
		{
			SettingsObj.GetSettings().MeasList.Clear();
			WPanel control = (WPanel)_activeControl;
			control.si1_1.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[0]);
			control.si1_2.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[1]);
			control.si1_3.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[2]);

            control.si1_4.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[3]);

            if (Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
			{
				if (DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest)
				{
					control.si2_1.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[4]);
					control.si2_2.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[5]);
					control.si2_3.SetValue(TestTasks.TypeTestTasks.Zero.Viewers[6]);
				}
			}


			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var indicatorViewer in TestTasks.TypeTestTasks.Zero.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
			}

			StartWorker();
		}

		#endregion

		#region Light

		private void btn_Light_Click(object sender, EventArgs e)
		{
        if (btn_Light.FlatStyle == FlatStyle.Standard)
			{
				ImageLight();
				PressButtonTask(btn_Light);
				GenSinCos.SetSelsinRefVoltage(GenSinCos.SelsinRefVoltageType.Normal);
			}
			else
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				GenSinCos.SetSelsinRefVoltage(GenSinCos.SelsinRefVoltageType.Normal);
				TaskOff("Task_1_32");
				DO176X.Instance.SetBVKRefuse(false);
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					((RepairLightControl)_activeControl).FinishCheck();
					((RepairLightControl)_activeControl).WriteToDbLight();

                    if (((RepairLightControl)_activeControl).IsCornerTaskActive)
                        dbKPK2.ManualRecordDouble(MapCornerParam, ((frmWork32)ParentForm).tiMapCorner.State ? 1 : 0);

                    if (DevicesId == DevicesID.Diss32_90_WorkTest && Modification == Modification.Diss32_90A)
                        MessageBox.Show(string.Format(Resources.PressButtonOnBlock8, Resources.Off),
                            Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        ((RepairLightControl)_activeControl).UnpressContrDiss();
                }
				else
				{
					((LightControl)_activeControl).WriteToDbLight();


					if (((LightControl)_activeControl).IsCornerTaskActive)
						dbKPK2.ManualRecordDouble(MapCornerParam, ((frmWork32)ParentForm).tiMapCorner.State ? 1 : 0);

					if (DevicesId == DevicesID.Diss32_90_WorkTest && Modification == Modification.Diss32_90A)
						MessageBox.Show(string.Format(Resources.PressButtonOnBlock8, Resources.Off),
							Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					else
						((LightControl)_activeControl).UnpressContrDiss();
				}
                ImageTaskStop();
                WrungButtonTask(btn_Light);
			}
		}

        public void IndicatorStatesReadyFunc(ucIndicatorsState.IndicatorsState states)
        {
            Action<ucIndicatorsState.IndicatorsState> handler = IndicatorStatesReady;
            if (handler != null)
                handler(states);
        }

        private event Action<ucIndicatorsState.IndicatorsState> IndicatorStatesReady;

        public DevicesID _deviceId;

        private void ImageLight()
		{
			var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
			TaskCustomView(false, 0, false, false, false, false, false);
			Point p = new Point(6, 6);
			if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
            {
                _activeControl = new RepairLightControl((frmWork32)ParentForm, this, dbKPK2, DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest,
                    btn_Light.Text);
                Controls.Add(_activeControl);
                _activeControl.Location = p;
                if (DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest || Modification == Modification.Diss32_90A)
                    ((RepairLightControl)_activeControl).HideLastButton();
            }
			else
			{
				_activeControl = new LightControl((frmWork32)ParentForm, this, dbKPK2, DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest,
					btn_Light.Text);
                if (_deviceId == DevicesID.Diss32)
                    IndicatorStatesReady += ((LightControl)_activeControl).RenewIndicators;
                Controls.Add(_activeControl);
                _activeControl.Location = p;
				if (DevicesId == DevicesID.Diss32 || DevicesId == DevicesID.Diss32_WorkTest || Modification == Modification.Diss32_90A)
					((LightControl)_activeControl).HideLastButton();
			}
		}

		#endregion

		#region Wxr Wzr Wyr Bort System

		private void bBortSystems_Click(object sender, EventArgs e)
		{
			Button currentButton = bBortSystems;
			currentButton.Enabled = false;
			if (currentButton.FlatStyle == FlatStyle.Standard)
			{
				PressButtonTask(currentButton);
				ImageWToBortSystems();
				Diss32Functions.SetAngle(0);
				TaskChanged(1);
			}
			else
			{
				StopWorker();
				if (!TaskOff("Task_1_32") || !TaskOff("Task_2_32"))
					EventErrorMessage(this,
						Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
				ImageTaskStop();
				WrungButtonTask(currentButton);
			}

			currentButton.Enabled = true;
		}

		private void ImageWToBortSystems()
		{
			TaskCustomView(false, 0, false, false, false, false, false);
			Point p = new Point(4, 4);
			_activeControl = new WPanel(true, true);
			Controls.Add(_activeControl);
			_activeControl.Location = p;
			((WPanel)_activeControl).TaskChanged += TaskChanged;
		}

		private void TaskChanged(int taskNum)
		{
			StopWorker(SaveResults);
			TaskOff(string.Format("Task_{0}_32", _lastTaskNum));
			if (!TaskOn("Task_W_1_32", string.Format("Task_{0}_32", taskNum)))
				EventErrorMessage(this,
					Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
			_lastTaskNum = taskNum;
			InitializeWToBortSystems(taskNum);
		}

		private void InitializeWToBortSystems(int taskNum)
		{
			TestTasks.TypeTestTasks.InitializeBortSystem(taskNum, SettingsObj.GetSettings().TestId);
			SettingsObj.GetSettings().MeasList.Clear();
			WPanel control = (WPanel)_activeControl;
			control.si1_1.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[0]);
			control.si1_2.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[1]);
			control.si1_3.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[2]);

			control.si1_4.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[6]);
			control.si1_5.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[7]);
			control.si1_6.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[8]);

			control.si2_1.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[3]);
			control.si2_2.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[4]);
			control.si2_3.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[5]);

			control.si2_4.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[9]);
			control.si2_5.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[10]);
			control.si2_6.SetValue(TestTasks.TypeTestTasks.BortSystem.Viewers[11]);

			SettingsObj.GetSettings().MeasList.Clear();
			foreach (var bortSystemViewer in TestTasks.TypeTestTasks.BortSystem.Viewers)
			{
				SettingsObj.GetSettings().MeasList.Add(bortSystemViewer.UsbBaseNew);
			}

			StartWorker();
		}

		#endregion

		#region Memory

		private void btn_Memory_Click(object sender, EventArgs e)
		{
			//if (Mode.GetMode().ModeApplication == ModeApplication.WorkCheck && _workTask4)
			//{
			//	MemoryTask4(sender, e);
			//}
			//else if (_workTask3)
			//{
				MemoryTask3(sender, e);
			//}
		}

		private void MemoryTask3(object sender, EventArgs e)
		{
			string memory2 = Resources.FindDeltaWAndAlfa;
			var mode = Mode.GetMode().ModeApplication;
			Button currentBtn = sender as Button;
            if (btn_Memory.Text == Resources.MemoryMode && mode == ModeApplication.DefaultCheck) // нажали 1-й раз
			{
				btn_Memory.Text = memory2; // меняем название
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
					ip.indValue1.Enabled = ip.indValue2.Enabled = false;
				indValue1.Enabled = indValue2.Enabled = rbtn_Zero.Enabled = rbtn_Kren.Enabled = false;

				IndicatorUp.NameLabelUp = string.Format(Resources.WpN, Resources.KmH);
				IndicatorLeft.NameLabelUp = Resources.AlfaGrad;
				if (ip != null)
				{
					ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
				}

				// запомнили в БД и для расчетов 

				task3_PC1 = _measure.GetValue(Task3_PC_32);
				task3_YC0 = _measure.GetValue(Task3_Y0_32);
				task3_PC1_2 = _measure.GetValue(Task3_PC2_32);
				task3_YC0_2 = _measure.GetValue(Task3_Y20_32);
				MessageBox.Show(string.Format(Resources.TaskMemoryInstruction, memory2), Resources.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (btn_Memory.Text == memory2 && mode == ModeApplication.DefaultCheck) // нажали 2-й раз 
			{
				btn_Memory.Text = Resources.Save;

				IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WN, Resources.KmH);
				IndicatorLeft.NameLabelUp = ((Char)0x0394) + Resources.AlfaGrad;
				if (_activeControl != null && _activeControl is IndicationPanel)
				{
					((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					((IndicationPanel)_activeControl).IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
				}

				task3_PC1_differ = _measure.GetValue(Task3_PC_32) - task3_PC1;
				task3_YC0_differ = _measure.GetValue(Task3_Y0_32) - task3_YC0;
				task3_PC1_differ_2 = _measure.GetValue(Task3_PC2_32) - task3_PC1_2;
				task3_YC0_differ_2 = _measure.GetValue(Task3_Y20_32) - task3_YC0_2;

				StopWorker();
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, ModeMemoryPC);
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, ModeMemoryYC);
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task3_PC1_differ);
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed2, task3_YC0_differ);
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null) //ДИСС-32-90
				{
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, ModeMemoryPC2);
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, ModeMemoryYC2);
					SetIndicatorValue((int)Globals.IndicatorNumber.Speed1_2, task3_PC1_differ_2);
					SetIndicatorValue((int)Globals.IndicatorNumber.Speed2_2, task3_YC0_differ_2);
				}

				return;
			}

			if (btn_Memory.Text == Resources.Save && mode == ModeApplication.DefaultCheck) // нажали 3-й раз 
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				btn_Memory.Text = Resources.MemoryMode; // поменяли название
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
				{
					ip.indValue1.Enabled =
						ip.indValue2.Enabled = true;
				}

				indValue1.Enabled = indValue2.Enabled = true;
				IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH); // "W, км/ч"; // 1-й индикатор
				IndicatorLeft.NameLabelUp = Resources.AlfaGrad;
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, Task3_PC_32);
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, Task3_Y0_32);
				if (ip != null)
				{ 
                    ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, Task3_PC2_32);
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, Task3_Y20_32);
                }

				dbKPK2.ManualRecordDouble(ModeMemoryPC, task3_PC1_differ);
				dbKPK2.ManualRecordDouble(ModeMemoryYC, task3_YC0_differ);
				dbKPK2.ManualRecordDouble(ModeMemoryPC2, task3_PC1_differ_2);
				dbKPK2.ManualRecordDouble(ModeMemoryYC2, task3_YC0_differ_2);
				
				// Запустили измерения в КЗ по новой 
				InitializeTask3();
				rbtn_Zero.Enabled = rbtn_Kren.Enabled = true;
			}
		}

		private void MemoryTask4(object sender, EventArgs e)
		{
			string memory2 = Resources.FindDeltaWAndAlfa;
			if (btn_Memory.Text == Resources.MemoryMode) // нажали 1-й раз
			{
				btn_Memory.Text = memory2; // меняем название
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
					ip.indValue1.Enabled = ip.indValue2.Enabled = false;
				indValue1.Enabled = indValue2.Enabled = rbtn_Zero.Enabled = rbtn_Kren.Enabled = false;

				IndicatorUp.NameLabelUp = string.Format(Resources.WpN, Resources.KmH);
				IndicatorLeft.NameLabelUp = Resources.AlfaGrad;
				if (ip != null)
				{
					ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
				}

				// запомнили в БД и для расчетов 

				task4_PC2 = _measure.GetValue(Task4_PC_32);
				task4_YC9 = _measure.GetValue(Task4_Y9_32);
				task4_PC2_2 = _measure.GetValue(Task4_PC2_32);
				task4_YC9_2 = _measure.GetValue(Task4_Y29_32);
				MessageBox.Show(string.Format(Resources.TaskMemoryInstruction, memory2), Resources.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (btn_Memory.Text == memory2) // нажали 2-й раз 
			{
				btn_Memory.Text = Resources.Save;
				IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WN, Resources.KmH);
				IndicatorLeft.NameLabelUp = ((Char)0x0394) + Resources.AlfaGrad;
				if (_activeControl != null && _activeControl is IndicationPanel)
				{
					((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					((IndicationPanel)_activeControl).IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
				}

				task4_PC2_differ = _measure.GetValue(Task4_PC_32) - task4_PC2;
				task4_YC9_differ = _measure.GetValue(Task4_Y9_32) - task4_YC9;
				task4_PC2_differ_2 = _measure.GetValue(Task4_PC2_32) - task4_PC2_2;
				task4_YC9_differ_2 = _measure.GetValue(Task4_Y29_32) - task4_YC9_2;

				StopWorker();
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, "DeltaTask4_PS_WorkTest"); //todo посмотреть что куда записывается
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, "DeltaTask4_US_WorkTest");
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task4_PC2_differ);
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed2, task4_YC9_differ);
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null) //ДИСС-32-90
				{
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, "DeltaTask4_PS_2_WorkTest");
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, "DelataTask4_US_2_WorkTest");
					SetIndicatorValue((int)Globals.IndicatorNumber.Speed1_2, task4_PC2_differ_2);
					SetIndicatorValue((int)Globals.IndicatorNumber.Speed2_2, task4_YC9_differ_2);
				}

				return;
			}

			if (btn_Memory.Text == Resources.Save) // нажали 3-й раз 
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				btn_Memory.Text = Resources.MemoryMode; // поменяли название
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
				{
					ip.indValue1.Enabled =
						ip.indValue2.Enabled = true;
				}

				indValue1.Enabled = indValue2.Enabled = true;
				IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH); // "W, км/ч"; // 1-й индикатор
				IndicatorLeft.NameLabelUp = Resources.AlfaGrad;
				if (ip != null)
				{
					ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.IndicatorLeft.NameLabelUp = IndicatorLeft.NameLabelUp;
				}

				dbKPK2.ManualRecordDouble("DeltaTask4_PS_WorkTest", task4_PC2_differ);
				dbKPK2.ManualRecordDouble("DeltaTask4_US_WorkTest", task4_YC9_differ);
				dbKPK2.ManualRecordDouble("DeltaTask4_PS_2_WorkTest", task4_PC2_differ_2);
				dbKPK2.ManualRecordDouble("DeltaTask4_US_2_WorkTest", task4_YC9_differ_2);
				// Запустили измерения в КЗ по новой 
				InitializeTask4();
				rbtn_Zero.Enabled = rbtn_Kren.Enabled = true;
			}
		}

		private void ModeWorkCapture(CancellationToken cToken)
		{
			//проверка наличия сигналов захват , 1, 2, 3
			try
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					siWorkTime.Visible = label5.Visible = true;
				}));
				_workDT = DateTime.Now;
				TimeSpan ts;
				BeginInvoke(new MethodInvoker(() => { label5.Text = "Проверка сигналов!"; }));
				var chList = DataManager.Instance.GetDiscreteInputsChannels();
				while (true)
				{
					if (cToken.IsCancellationRequested)
					{
						ChangeLabel5Text("Проверка остановлена");
						BeginInvoke(new MethodInvoker(() =>
						{
							btn_Memory.Text = @"режим ""Захват""";
						}));
						return;
					}
					ts = DateTime.Now - _workDT;
					BeginInvoke(new MethodInvoker(() => { siWorkTime.ValueString = GetTimeString(ts); }));
					if (ts.Seconds >= 15)
					{
						break;
					}

					Thread.Sleep(500);
				}

				bool[] captureSignal = CheckCapture(chList);
				string messageNoCapture = "";
				int countNoCapture = 0;
				if (captureSignal == null)
				{
					messageNoCapture = "Отсутсвуют сигналы ЗАХВАТ, ЗАХВАТ 1, ЗАХВАТ 2, ЗАХВАТ 3.";
					MessageBox.Show(messageNoCapture);
				}
				else
				{
					for (int i = 0; i < captureSignal.Length; i++)
					{
						if (!captureSignal[i])
						{
							if (countNoCapture != 0)
							{
								messageNoCapture += ", ";
							}

							countNoCapture++;
							if (i == 0)
							{
								messageNoCapture += "\"ЗАХВАТ\"";
							}
							else
							{
								messageNoCapture += $"\"ЗАХВАТ {i}\"";
							}
						}
					}

					if (countNoCapture > 1)
					{
						messageNoCapture = "Отсутсвуют сигналы " + messageNoCapture + ".";
						MessageBox.Show(messageNoCapture);
					}
					else if (countNoCapture == 1)
					{
						messageNoCapture = "Отсутсвует сигнал " + messageNoCapture + ".";
						MessageBox.Show(messageNoCapture);
					}
				}


				//проверка сигналов память 1 и 2
				BeginInvoke(new MethodInvoker(() => { label5.Text = "Проверка сигналов память!"; }));
				while (true)
				{
					if (cToken.IsCancellationRequested)
                    {
                        ChangeLabel5Text("Проверка остановлена");
                        BeginInvoke(new MethodInvoker(() =>
						{
							btn_Memory.Text = @"режим ""Захват""";
						}));
						return;
					}
					ts = DateTime.Now - _workDT;
					BeginInvoke(new MethodInvoker(() => { siWorkTime.ValueString = GetTimeString(ts); }));
					if (ts.Seconds >= 20)
					{
						break;
					}

					Thread.Sleep(500);
				}
				
				StopWorker(false);
				TaskOff("Task_3_32");

				Thread.Sleep(1000);
				if (!CheckMemory(chList, 1))
				{
					MessageBox.Show("Сигнал \"ПАМЯТЬ 1\" отсутствует.");
				}

                Logger _logger = LogManager.GetLogger("KPK2Trace");

                if (MessageBox.Show("Включено табло \"ПОИСК\" на УПС?", "Проверка",
					    MessageBoxButtons.YesNo) == DialogResult.OK)
				{
                    _logger.Trace("На УСПС табло \"ПОИСК\" горит.");
				}
				else
				{
					_logger.Trace("На УСПС табло \"ПОИСК\" не горит.");
				}
				
				while (true)
				{
					if(cToken.IsCancellationRequested)
					{
                        TaskOn("Task_W_1_32", "Task_3_32");
						InitializeTask3();
						//StartWorker();
						BeginInvoke(new MethodInvoker(() =>
                        {
                            btn_Memory.Text = @"режим ""Захват""";
                        }));
                        ChangeLabel5Text("Проверка остановлена");
                        return;
					}
					ts = DateTime.Now - _workDT;
					BeginInvoke(new MethodInvoker(() => { siWorkTime.ValueString = GetTimeString(ts); }));
					if (ts.Seconds >= 10)
					{
						break;
					}

					Thread.Sleep(500);
				}

				if (!CheckMemory(chList, 2))
				{
					MessageBox.Show("Сигнал \"ПАМЯТЬ 2\" отсутствует.");
				}

				if (MessageBox.Show("Включено табло \"П\" на УПС?", "Проверка",
					    MessageBoxButtons.YesNo) == DialogResult.OK)
				{
					_logger.Trace("НА УСПС табло \"П\" включено.");
				}
				else
				{
					_logger.Trace("НА УСПС табло \"П\" выключен.");
				}

				//проверка на шум
				BeginInvoke(new MethodInvoker(() => { label5.Text = "Проверка на шум."; }));
				bool[] capture = new bool[4];
				int[] timeEnableCapture = new int[4];
				int[] countEnableCapture = new int[4];
				int timeStop = 180000;
				int currentTime = 0;
				int timeReadLogMillis = 0;
				_workDT = DateTime.Now;
				while (true)
				{
                    if (cToken.IsCancellationRequested)
                    {
                        TaskOn("Task_W_1_32", "Task_3_32");
                        InitializeTask3();
                        //StartWorker();
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            btn_Memory.Text = @"режим ""Захват""";
                        }));
                        ChangeLabel5Text("Проверка остановлена");
                        return;
                    }

                    if (timeStop <= currentTime)
					{
						DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "NotTransitioFromMemoryToCapure", 1.0);
						break;
					}

					ts = DateTime.Now - _workDT;
					BeginInvoke(new MethodInvoker(() =>
					{
						siWorkTime.ValueString = GetTimeString(ts);
					}));
					currentTime += 250;
					bool result = false;
					var checkCapture = CheckCapture(chList);
					for (int i = 0; i < checkCapture.Length; i++)
					{
						if (checkCapture[i])
						{
							if (capture[i])
							{
								timeEnableCapture[i] += 250;
								timeReadLogMillis += 250;
							}
							else
							{
								capture[i] = true;
                                if (i == 0)
                                    _logger.Trace($"Сигнал ЗАХВАТ включился.");
                                else
                                    _logger.Trace($"Сигнал ЗАХВАТ {i} включился.");
							}
						//	_logger
						}
						else
						{
							if (capture[i])
							{
								timeEnableCapture[i] = 0;
								capture[i] = false;
								countEnableCapture[i]++;
                                if (i == 0)
                                    _logger.Trace($"Сигнал ЗАХВАТ выключился.");
                                else
                                    _logger.Trace($"Сигнал ЗАХВАТ {i} выключился.");
							}
						}
					}

					if(timeReadLogMillis == 1000)
					{
						timeReadLogMillis = 0;
						for(int i = 1; i <= checkCapture.Length; i++)
						{
                            if (timeEnableCapture[i] != 0)
                            {
                                _logger.Trace($"Сигнал Захват {i} оставался включенным {timeEnableCapture[i] / 1000} сек.");
                                _logger.Trace($"Сигнал Захват {i} включался {countEnableCapture[i]} раз.");
                            }
						}
					}

					for (int i = 0; i < capture.Length; i++)
					{
						if (timeEnableCapture[i] >= 1500 || countEnableCapture[i] > 3)
						{
							result = true;
							break;
						}
					}

					if (result)
					{
						DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "NotTransitioFromMemoryToCapure",0.0);
						break;
					}

					Thread.Sleep(250);
				}

				TaskOn("Task_W_1_32", "Task_3_32");

				//Время перехода из памяти в захват
				InitializeTask3();
				//StartWorker();

				Thread.Sleep(500);
				bool captureT = false;
				bool serviceability = false;
				double timerStart = 0.0;
				BitArray bitArray = null;
				chList = DataManager.Instance.GetDiscreteInputsChannels(); 
				BeginInvoke(new MethodInvoker(() => { label5.Text = "Время перехода из памяти в захват."; }));
                while (true)
				{
                    if (cToken.IsCancellationRequested)
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            btn_Memory.Text = @"режим ""Захват""";
                        }));
                        return;
                    }
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        siWorkTime.ValueString = GetTimeString(ts);
                    }));
                    timerStart += 0.25;
					bitArray = DiscreteInputs.Instance().GetInputsStates();
					if (!captureT)
					{
						foreach (var item in chList)
						{
							if (item.Value == "Capture")
							{
								captureT = bitArray[item.Key];
							}
						}
					}

                    if (!serviceability)
                    {
                        foreach (var item in chList)
                        {
                            if (item.Value == "GoodCondition")
                            {
                                serviceability = bitArray[item.Key];
                            }
                        }
                    }

                    if (captureT && serviceability)
					{
						DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "TimeTransitioFromMemoryToCapure", timerStart);
						break;
					}

					Thread.Sleep(250);//todo доьавить вкл КЗ-3
				};
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.StackTrace);
			}
			finally
			{
				BeginInvoke(new MethodInvoker(() =>
                {
                    btn_Memory.Text = @"режим ""Захват""";
					WrungButtonCT3(btn_Memory); 
                }));
			}
		}

		private bool[] CheckCapture(Dictionary<int, string> chList)
		{
			BitArray bitArray = DiscreteInputs.Instance().GetInputsStates();
			if (chList.Count == 0 || chList == null)
			{
				return null;
			}

			bool[] result = new[]
			{
				bitArray[GetNumChannel(chList, "Capture")],
				bitArray[GetNumChannel(chList,"Capture_1")],
				bitArray[GetNumChannel(chList,"Capture_2")],
				bitArray[GetNumChannel(chList,"Capture_3")],
			};
			return result;
		}

		private int GetNumChannel(Dictionary<int, string> chList, string value)
		{
			foreach (var item in chList)
			{
				if (item.Value == value)
				{
					return item.Key;
				}
			}
			return -1;
		}

		private bool CheckMemory(Dictionary<int, string> chList, int numberMemory)
		{
			var frmDiss32 = (frmWork32)ParentForm;
			BitArray bitArray = frmDiss32.InputsStates;//DiscreteInputs.Instance().GetInputsStates();
			string nameMemory = "Memory_" + numberMemory; 
			foreach (var item in chList)
			{
				if (item.Value == nameMemory)
				{
					return bitArray[item.Key];
				}
			}

			return false;
		}

		#endregion

		#region Mode_M_C

		private void btn_Mode_M_C_Click(object sender, EventArgs e)
		{
			string modeMC_2 = Resources.FindDeltaW;
			btn_Mode_M_C.Enabled = false;
			if (btn_Mode_M_C.Text == Resources.MSMode) // нажали 1-й раз
			{
				IndicatorUp.MaxValue = 264;
				btn_Mode_M_C.Text = modeMC_2; // изменили назавние btn_Mode_M_C
				btn_Mode_M_B.Enabled = false; // заблокировали кнопки btn_Mode_M_B 
				if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					btn_Memory.Enabled = false;
				}
				indValue1.Enabled = indValue2.Enabled = false;
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
				{
					ip.indValue1.Enabled =
						ip.indValue2.Enabled = false;
					ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.IndicatorLeft.Inactive = true;
				}

				IndicatorUp.NameLabelUp = string.Format(Resources.WmsN, Resources.KmH);

				IndicatorLeft.Inactive = true;

                if (_activeControl is IndicationPanel)
                    ((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;

				task4_PC2 = _measure.GetValue(Task4_PC_32);
				task4_PC2_2 = _measure.GetValue(Task4_PC2_32);
				MessageBox.Show(string.Format(IsDiss32_90? Resources.TaskUSPSInstruction_2USPS : Resources.TaskUSPSInstruction, Resources.MS, modeMC_2),
					Resources.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (_device != null)
					((Imitator.Diss32)_device).SetMS(true);
                if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
                    SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task4_PC2);
                }
				else
					SetIndicatorLimitsPercent((int)Globals.IndicatorNumber.Speed1, task4_PC2, task4_PC2, ModeMSStr);

                btn_Mode_M_C.Enabled = true;
				return;
			}

			if (btn_Mode_M_C.Text == modeMC_2) // нажали 2-й раз
            {
				btn_Mode_M_C.Text = Resources.Save; // изменили название
				if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
					IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WmsN, "%");
				else
				{
					IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WmsN, "");
                }

				if (_activeControl is IndicationPanel)
					((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					task4_PC2_differ = _measure.GetValue(Task4_PC_32) - task4_PC2;
					task4_PC2_differ_2 = _measure.GetValue(Task4_PC2_32) - task4_PC2_2;
                }
				else
				{
					task4_PC2_differ = (_measure.GetValue(Task4_PC_32) - task4_PC2) / task4_PC2 * 100; // %
					task4_PC2_differ_2 = (_measure.GetValue(Task4_PC2_32) - task4_PC2_2) / task4_PC2_2 * 100; // %
				}
				StopWorker();

				IndicatorUp.SetValue(TestTasks.TypeTestTasks.ModeMC.Viewers[0]);
                if (_activeControl is IndicationPanel)
                {
                    //((IndicationPanel)_activeControl).IndicatorUp.SetValue(TestTasks.TypeTestTasks
                    //    .ModeMC.Viewers[1]);
                    ((IndicationPanel)_activeControl).IndicatorUp.SetValue(TestTasks.TypeTestTasks.ModeMC.Viewers[1]);

                }

                SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task4_PC2_differ);
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed1_2, task4_PC2_differ_2);
				btn_Mode_M_C.Enabled = true;
				return;
			}

			if (btn_Mode_M_C.Text == Resources.Save) // нажали 3-й раз
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				btn_Mode_M_C.Text = Resources.MSMode; // изменили название                
				btn_Mode_M_B.Enabled = true;
                if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    btn_Memory.Enabled = true;
                }
                MessageBox.Show(IsDiss32_90? Resources.TaskUSPSInstruction2_2USPS : Resources.TaskUSPSInstruction2, Resources.ApplicationName, MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				if (_device != null)
					((Imitator.Diss32)_device).SetMS(false);
				InitializeTask4();
				//StartWorker();
				IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH);
				
				IndicationPanel ip = _activeControl as IndicationPanel;
				if (ip != null)
				{
					ip.IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					ip.indValue1.Enabled =
						ip.indValue2.Enabled = true;
					
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, Task4_PC2_32);
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, Task4_Y29_32);
				}

				dbKPK2.ManualRecordDouble(ModeMSStr, task4_PC2_differ);
				if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
					dbKPK2.ManualRecordDouble(ModeMS2Str, task4_PC2_differ_2);
				indValue1.Enabled = indValue2.Enabled = true;
				btn_Mode_M_C.Enabled = true;
				IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task4.Viewers[0]);
				
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, Task4_PC_32);
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, Task4_Y9_32);
			}
		}

		#endregion

		#region Mode_M_B

		private void btn_Mode_M_B_Click(object sender, EventArgs e)
		{
			string modeMB_2 = Resources.FindDeltaW;
			string modeMC_2 = Resources.FindDeltaW;
			string modeMB = "Mode_M_B";
			string modeMB2 = "Mode_M_B_2";

			if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
			{
				modeMB += "_WorkTest";
				modeMB2 += "_WorkTest";
			}

			btn_Mode_M_B.Enabled = false;
			if (btn_Mode_M_B.Text == Resources.MBMode) // нажали 1-й раз
			{
				IndicatorUp.MaxValue = 262;
				btn_Mode_M_B.Text = modeMB_2; // изменили назавние btn_Mode_M_B
				indValue1.Enabled = indValue2.Enabled = false;
				btn_Mode_M_C.Enabled = false;
                if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    btn_Memory.Enabled = false;
                }
                IndicatorUp.NameLabelUp = string.Format(Resources.WmbN, Resources.KmH);
				if (_activeControl is IndicationPanel)
				{
					((IndicationPanel)_activeControl).indValue1.Enabled =
						((IndicationPanel)_activeControl).indValue2.Enabled = false;
					((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
				}

				task4_PC2 = _measure.GetValue(Task4_PC_32);
				task4_PC2_2 = _measure.GetValue(Task4_PC2_32);
				MessageBox.Show(string.Format(IsDiss32_90? Resources.TaskUSPSInstruction_2USPS : Resources.TaskUSPSInstruction, Resources.MB, modeMC_2),
					Resources.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (_device != null)
					((Imitator.Diss32)_device).SetMB(true);
				if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
					SetIndicatorLimitsPercent((int)Globals.IndicatorNumber.Speed1, task4_PC2, task4_PC2, modeMB);
				btn_Mode_M_B.Enabled = true;
				return;
			}

			if (btn_Mode_M_B.Text == modeMB_2) // нажали 2-й раз
			{
				btn_Mode_M_B.Text = Resources.Save; // изменили название

				if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WmbN, "");
                }
				else
                    IndicatorUp.NameLabelUp = ((Char)0x0394) + string.Format(Resources.WmbN, "%");
                if (_activeControl is IndicationPanel)
                    ((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;

                if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
				{
					task4_PC2_differ = _measure.GetValue(Task4_PC_32) - task4_PC2;
					task4_PC2_differ_2 = _measure.GetValue(Task4_PC2_32) - task4_PC2_2;
				}
				else
				{
					task4_PC2_differ = (_measure.GetValue(Task4_PC_32) - task4_PC2) / task4_PC2 * 100; // %
					task4_PC2_differ_2 = (_measure.GetValue(Task4_PC2_32) - task4_PC2_2) / task4_PC2_2 * 100; // %
				}
				StopWorker();
				IndicatorUp.SetValue(TestTasks.TypeTestTasks.ModeMB.Viewers[0]);
                if (_activeControl is IndicationPanel)
                {
                    //((IndicationPanel)_activeControl).IndicatorUp.SetValue(TestTasks.TypeTestTasks
                    //    .ModeMB.Viewers[1]);

                    ((IndicationPanel)_activeControl).IndicatorUp.SetValue(TestTasks.TypeTestTasks.ModeMB.Viewers[1]);
                }

                SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task4_PC2_differ);
				SetIndicatorValue((int)Globals.IndicatorNumber.Speed1_2, task4_PC2_differ_2);
				btn_Mode_M_B.Enabled = true;
				return;
			}

			if (btn_Mode_M_B.Text == Resources.Save) // нажали 3-й раз
			{
				var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
				btn_Mode_M_B.Text = Resources.MBMode; // изменили название                
				btn_Mode_M_C.Enabled = true;
                if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
                {
                    btn_Memory.Enabled = true;
                }
                MessageBox.Show(IsDiss32_90? Resources.TaskUSPSInstruction2_2USPS :Resources.TaskUSPSInstruction2, Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (_device != null)
					((Imitator.Diss32)_device).SetMB(false);
				InitializeTask4();
				IndicatorUp.NameLabelUp = string.Format(Resources.WN, Resources.KmH);

				dbKPK2.ManualRecordDouble(modeMB, task4_PC2_differ);
				if(Mode.GetMode().ModeApplication == ModeApplication.DefaultCheck)
					dbKPK2.ManualRecordDouble(modeMB2, task4_PC2_differ_2);
				indValue1.Enabled = indValue2.Enabled = true;
				if (_activeControl is IndicationPanel)
				{
					((IndicationPanel)_activeControl).IndicatorUp.NameLabelUp = IndicatorUp.NameLabelUp;
					((IndicationPanel)_activeControl).indValue1.Enabled =
						((IndicationPanel)_activeControl).indValue2.Enabled = true;
					
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1_2, Task4_PC2_32);
					SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2_2, Task4_Y29_32);
				}

				btn_Mode_M_B.Enabled = true;
				IndicatorUp.SetValue(TestTasks.TypeTestTasks.Task4.Viewers[0]);
				
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, Task4_PC_32);
				SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, Task4_Y9_32);
			}
		}

		#endregion

		#region reset

		private void rbtn_Zero_Click(object sender, EventArgs e)
		{
			if (rbtn_Zero.FlatStyle == FlatStyle.Standard)
			{
				WriteComboBox();
				WritePoint23();

				IncludeRadioButtonCorner(rbtn_Zero);

				grpBox_Speed.Enabled = btn_Memory.Enabled = true;
				indValue1.Focus();

				cmbBox_1.Items.Clear();
				cmbBox_1.Items.AddRange(arComboBoxCorner);
				cmbBox_2.Items.Clear();
				cmbBox_2.Items.AddRange(arComboBoxCorner);
				cmbBox_3.Items.Clear();
				cmbBox_3.Items.AddRange(arComboBoxCorner);
				cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled =
					cmbBox_Corner.Enabled = false;
				cmbBox_Corner.Items.Clear();
				if (btn_Task_1.FlatStyle == FlatStyle.Flat)
					cmbBox_Corner.Items.AddRange(new string[] { Plus12Grad, ZeroGrad, Minus12Grad });
				else
					cmbBox_Corner.Items.AddRange(new string[]
						{ Plus15Grad, Plus11Grad, Plus5Grad, ZeroGrad, Minus5Grad, Minus11Grad, Minus15Grad });
				cmbBox_Corner.SelectedItem = string.Format(Resources.AngleGrad, 0);
				tiCorner.VisualText = cmbBox_Corner.SelectedItem.ToString();
				btn_Corner.Enabled = false;
				try
				{
					DiscreteOutputs.Instance().SetOutput("Pitch", true);
					DiscreteOutputs.Instance().SetOutput("Heel", true);
					Diss32Functions.SetAngle(0);
				}
				catch
				{
				}
			}
		}

		private void WriteComboBox()
		{
			var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
			if (cmbBox_1.Enabled == false) // запись существует 
				return;
			// Без изм.=0; Увелич.=1;  Уменьш.=-1; не было выбора=-2
			int i1 = cmbBox_1.SelectedItem == null
				? -2
				: cmbBox_1.SelectedItem.ToString() == Resources.GreatThan
					? 1
					: cmbBox_1.SelectedItem.ToString() == Resources.NotChanged
						? 0
						: -1;
			int i2 = cmbBox_2.SelectedItem == null
				? -2
				: cmbBox_2.SelectedItem.ToString() == Resources.GreatThan
					? 1
					: cmbBox_2.SelectedItem.ToString() == Resources.NotChanged
						? 0
						: -1;
			int i3 = cmbBox_3.SelectedItem == null
				? -2
				: cmbBox_3.SelectedItem.ToString() == Resources.GreatThan
					? 1
					: cmbBox_3.SelectedItem.ToString() == Resources.NotChanged
						? 0
						: -1;
			// имя сигнала для записи в БД 
			string s1 = isHeel && isPlusCorner
				? "Heel_Plus12_X"
				: isHeel && !isPlusCorner
					? "Heel_Minus12_X"
					: !isHeel && isPlusCorner
						? "Pitch_Plus12_X"
						: "Pitch_Minus12_X";
			string s2 = isHeel && isPlusCorner
				? "Heel_Plus12_Z"
				: isHeel && !isPlusCorner
					? "Heel_Minus12_Z"
					: !isHeel && isPlusCorner
						? "Pitch_Plus12_Z"
						: "Pitch_Minus12_Z";
			string s3 = isHeel && isPlusCorner
				? "Heel_Plus12_Y"
				: isHeel && !isPlusCorner
					? "Heel_Minus12_Y"
					: !isHeel && isPlusCorner
						? "Pitch_Plus12_Y"
						: "Pitch_Minus12_Y";
			if (i1 != -2)
				dbKPK2.ManualRecordDouble(s1, i1);
			if (i2 != -2)
				dbKPK2.ManualRecordDouble(s2, i2);
			if (i3 != -2)
				dbKPK2.ManualRecordDouble(s3, i3);
		}

		private void WritePoint23()
		{
			tmrBlock8.Stop();
			grpBoxPoint23.Visible = false;

			// запись в БД 
			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // КЗ-3 
			{
				if (cmbBox_Corner.SelectedItem.ToString() == Plus5Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus5Grad)
					WriteToDbPoint23("Point23_32_2", arPoint32_2);
				if (cmbBox_Corner.SelectedItem.ToString() == Plus11Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus11Grad)
					WriteToDbPoint23("Point23_32_1", arPoint32_1);
				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus15Grad)
					WriteToDbPoint23("Point23_32_3", arPoint32_3);
			}

			if (btn_Task_1.FlatStyle == FlatStyle.Flat && // КЗ-1 
			    (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ||
			     cmbBox_Corner.SelectedItem.ToString() == Minus15Grad))
				WriteToDbPoint23("Point23_32_4", arPoint32_4);
		}

		/// <summary>
		/// По имени сигнала пишем реультат в БД 
		/// </summary>
		/// <param name="name">Имя сигнала</param>
		/// <param name="array">[4] с данными для записи результата в БД</param>
		private void WriteToDbPoint23(string name, bool[] array)
		{
			var dbKPK2 = SettingsObj.GetSettings().DBKPK2;
			// пишет в БД только когда выбирали "+" и "-" углы 
			if (array[0] && array[1])
			{
				int k = array[2] && array[3] ? 1 : 0;
				dbKPK2.ManualRecordDouble(name, k);
			}
		}

		#endregion

		#region kren

		private void rbtn_Kren_Click(object sender, EventArgs e)
		{
			if (rbtn_Kren.FlatStyle == FlatStyle.Standard)
			{
				WritePoint23();
				bool bl = false;
				try
				{
					DiscreteOutputs.Instance().SetOutput("Pitch", true);
					DiscreteOutputs.Instance().SetOutput("Heel", false);
				}
				catch
				{
					bl = true;
					EventErrorMessage?.Invoke(this, "");
				}

				if (bl) return;
				IncludeRadioButtonCorner(rbtn_Kren);
				grpBox_Speed.Enabled = btn_Memory.Enabled =
					cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = false;
				cmbBox_Corner.Enabled = btn_Corner.Enabled = true;
			}
		}

		#endregion

		#region Tangazh +10

		private void rbtn_Tangazh_Click(object sender, EventArgs e)
		{
			if (rbtn_Tangazh.FlatStyle == FlatStyle.Standard)
			{
				WritePoint23();
				bool bl = false;
				try
				{
					DiscreteOutputs.Instance().SetOutput("Heel", true);
					DiscreteOutputs.Instance().SetOutput("Pitch", false);
				}
				catch
				{
					bl = true;
					EventErrorMessage?.Invoke(this, "");
				}

				if (bl) return;
				IncludeRadioButtonCorner(rbtn_Tangazh);
				grpBox_Speed.Enabled = btn_Memory.Enabled =
					cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = false;
				cmbBox_Corner.Enabled = btn_Corner.Enabled = true;
			}
		}

		#endregion

		#region Block8 checker

		private void cbShowBlock8_CheckedChanged(object sender, EventArgs e)
		{
			if (cbShowBlock8.Enabled)
			{
				btn_Mode_M_B.Enabled = btn_Mode_M_C.Enabled = !cbShowBlock8.Checked;
				Control temp = _activeControl;
				_activeControl.Hide();
				_activeControl = _activeControl2;
				_activeControl2 = temp;
				_activeControl.Show();
			}
		}

		#endregion

		#region Corner

		private void btn_Corner_Click(object sender, EventArgs e)
		{
			cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled =
				cmbBox_Corner.Enabled = true;
			WriteComboBox();
			if (cmbBox_Corner.SelectedItem.ToString() == ZeroGrad) // при 0° => "Обнуление"
			{
				rbtn_Zero.PerformClick();
				IndicatorUp.CalculationMethod = IndicatorLeft.CalculationMethod =
					IndicatorDown.CalculationMethod = CalcMethod.MinMax;
				return;
			}

			if (btn_Task_1.FlatStyle == FlatStyle.Flat) // КЗ-1
			{
				cmbBox_1.Items.Clear();
				cmbBox_1.Items.AddRange(arComboBoxCorner);
				cmbBox_2.Items.Clear();
				cmbBox_2.Items.AddRange(arComboBoxCorner);
				cmbBox_3.Items.Clear();
				cmbBox_3.Items.AddRange(arComboBoxCorner);
				tiCorner.VisualText = cmbBox_Corner.SelectedItem.ToString();

				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus15Grad)
				{
					cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = false;
					int angle = cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ? 15 : 345;
					Diss32Functions.SetAngle((ushort)(angle + CommonSettings.Default.AddAngleKZ1Diss32));
					//ControlSignal(s);
					if (rbtn_Kren.Checked)
						VisiblePoint23(angle);
				}
				else // +/- 12°
				{
					int angle = cmbBox_Corner.SelectedItem.ToString() == Plus12Grad
						? 12
						: 348; //Разворот угла относительно 0 (особенности ДИСС-32)
					Diss32Functions.SetAngle((ushort)(angle + CommonSettings.Default.AddAngleKZ1Diss32));
					//ControlSignal(s);
					isHeel = rbtn_Kren.Checked;
					isPlusCorner = cmbBox_Corner.SelectedItem.ToString() == Plus12Grad;

					ConfigureSIToAngle(isPlusCorner, isHeel);


					if (_device != null)
					{
						if (isHeel)
							((Imitator.Diss32)_device).SetHeel(true, isPlusCorner);
						else
							((Imitator.Diss32)_device).SetPitch(true, isPlusCorner);
					}
				}

				return;
			}

			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // КЗ-3
			{
				tiCorner.VisualText = cmbBox_Corner.SelectedItem.ToString();
				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus15Grad)
				{
					int angle = cmbBox_Corner.SelectedItem.ToString() == Plus15Grad ? 15 : 345;
					Diss32Functions.SetAngle((ushort)angle);
					VisiblePoint23(angle);
					return;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Plus5Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus5Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Minus11Grad ||
				    cmbBox_Corner.SelectedItem.ToString() == Plus11Grad)
				{
					int angle = cmbBox_Corner.SelectedItem.ToString() == Plus5Grad
						? 5
						: cmbBox_Corner.SelectedItem.ToString() == Minus5Grad
							? -5
							: cmbBox_Corner.SelectedItem.ToString() == Plus11Grad
								? 11
								: 349;
					Diss32Functions.SetAngle((ushort)angle);
					VisiblePoint23(angle);
					return;
				}
			}
		}

		private void VisiblePoint23(int angle)
		{
			tmrBlock8.Stop();
			//panel_Block8.Visible = false;
			grpBoxPoint23.Visible = true;
			rbtn_Point23_1.Checked = rbtn_Point23_2.Checked = false;
			string s = angle == 5 || angle == 355
				? // КЗ-3
				Resources.AbsentRemember + ' ' + strPlusMinus + "5" + strGrad
				: angle == 11 || angle == 349
					? Resources.AbsentSignals + " П1,П2,П,ПОИСК"
					: btn_Task_3.FlatStyle == FlatStyle.Flat
						? // КЗ-3
						Resources.AbsentRemember + ' ' + strPlusMinus + "15" + strGrad +
						"(" + Resources.MB + ")"
						: Resources.AbsentRemember + ' ' + strPlusMinus + "15" + strGrad +
						  "(" + Resources.Forward.ToUpper() + ")"; // КЗ-1
			label37.Text = s;
		}

		private void ConfigureSIToAngle(bool isPlus, bool isHeel)
		{
			IndicatorUp.CheckValueFunction =
				IndicatorLeft.CheckValueFunction = IndicatorDown.CheckValueFunction = value => true;
			IndicatorUp.CalculationMethod =
				IndicatorLeft.CalculationMethod = IndicatorDown.CalculationMethod = CalcMethod.Delegate;
		}

		#endregion

		#region corner checker

		private void cmbBox_Corner_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = false;
			WriteComboBox();
		}

		#endregion

		#region cmb selected value 1, 2, 3

		private void cmbBox_1_SelectedValueChanged(object sender, EventArgs e)
		{
			arComboBox[0] = true;
		}

		private void cmbBox_2_SelectedValueChanged(object sender, EventArgs e)
		{
			arComboBox[1] = true;
		}

		// вниз - ввод для крен/тангаж
		private void cmbBox_3_SelectedValueChanged(object sender, EventArgs e)
		{
			arComboBox[2] = true;
		}

		#endregion

		#region PointChecker

		private void rbtn_Point23_1_Click(object sender, EventArgs e)
		{
			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // КЗ-3 
			{
				if (cmbBox_Corner.SelectedItem.ToString() == Plus5Grad)
					arPoint32_2[0] = arPoint32_2[2] = true;
				if (cmbBox_Corner.SelectedItem.ToString() == Minus5Grad)
					arPoint32_2[1] = arPoint32_2[3] = true;
				if (cmbBox_Corner.SelectedItem.ToString() == Plus11Grad)
					arPoint32_1[0] = arPoint32_1[2] = true;
				if (cmbBox_Corner.SelectedItem.ToString() == Minus11Grad)
					arPoint32_1[1] = arPoint32_1[3] = true;
				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad)
					arPoint32_3[0] = arPoint32_3[2] = true;
				if (cmbBox_Corner.SelectedItem.ToString() == Minus15Grad)
					arPoint32_3[1] = arPoint32_3[2] = true;
			}

			if (btn_Task_1.FlatStyle == FlatStyle.Flat) // КЗ-1 
			{
				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad)
					arPoint32_4[0] = arPoint32_4[2] = true;
				else // выбрали -15° 
					arPoint32_4[1] = arPoint32_4[3] = true;
			}
		}

		// Выбрали "НЕТ" для п.23 отчёта
		private void rbtn_Point23_2_Click(object sender, EventArgs e)
		{
			if (btn_Task_3.FlatStyle == FlatStyle.Flat) // КЗ-3 
			{
				if (cmbBox_Corner.SelectedItem.ToString() == Plus5Grad)
				{
					arPoint32_2[0] = true;
					arPoint32_2[2] = false;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Minus5Grad)
				{
					arPoint32_2[1] = true;
					arPoint32_2[3] = false;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Plus11Grad)
				{
					arPoint32_1[0] = true;
					arPoint32_1[2] = false;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Minus11Grad)
				{
					arPoint32_1[1] = true;
					arPoint32_1[3] = false;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad)
				{
					arPoint32_3[0] = true;
					arPoint32_3[2] = false;
				}

				if (cmbBox_Corner.SelectedItem.ToString() == Minus15Grad)
				{
					arPoint32_3[1] = true;
					arPoint32_3[3] = false;
				}
			}

			if (btn_Task_1.FlatStyle == FlatStyle.Flat) // КЗ-1 
			{
				if (cmbBox_Corner.SelectedItem.ToString() == Plus15Grad)
				{
					arPoint32_4[0] = true;
					arPoint32_4[2] = false;
				}
				else // выбрали -15° 
				{
					arPoint32_4[1] = true;
					arPoint32_4[3] = false;
				}
			}
		}

		#endregion

		#region tmrBlock8

		private void tmrBlock8_Tick(object sender, EventArgs e)
		{
			countBlock8--;
			int s = countBlock8 % 60; // кол-во секунд
			int m = countBlock8 / 60; // кол-во минут 

			((Block8)_activeControl).lbl_6_Time.Text = string.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, m,
				Resources.Minutes, s, Resources.Seconds);
			((Block8)_activeControl).progressBar_6.Value++;
			if (countBlock8 == 0) // время кончилось 
				((Block8)_activeControl).btn_8_Off_6.PerformClick();
		}

		#endregion

		private void GetTypeVSSFromUser()
		{
			if (_typeVSS != null)
				return;
			frmVSSType frm = new frmVSSType();
			frm.ShowDialog();
			_typeVSS = frm.VSSType;
			if (_typeVSS != null && SettingsObj.GetSettings().TestId != -1)
				DataManager.Instance.SetAddParam(SettingsObj.GetSettings().TestId, DataManager.ParamTypeVSS, _typeVSS.ToString());
		}

		private void PressButtonTask(Button btn)
		{
			CountButtons = 1;
			btn.FlatStyle = FlatStyle.Flat;
			btn.BackColor = Color.FromArgb(153, 249, 164);
			for (int i = 0; i < arButtonTask.Length; i++)
				arButtonTask[i].Enabled =
					arButtonTask[i].Name != btn.Name ? false : true;
		}

		private void PressButtonCT3(Button btn)
        {
            CountButtons = 1;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(153, 249, 164);
            for (int i = 0; i < btnCT3.Length; i++)
                btnCT3[i].Enabled =
                    btnCT3[i].Name != btn.Name ? false : true;
        }

		private void TaskCustomView(bool panelTask, int indNum, bool panelSpeed, bool panelCorner, bool buttonPitch,
			bool memory, bool otherButtons)
		{
			panel_Task.Visible = panelTask;
			if (panelTask)
			{
				IndicatorUp.Visible = indValue1.Visible = indNum >= 1;
				IndicatorLeft.Visible = indValue2.Visible = indNum >= 2;
				IndicatorDown.Visible = indValue3.Visible = indNum >= 3;
				indValue1.Value = indValue2.Value = indValue3.Value = double.NaN;
				indValue1.Enabled = indValue2.Enabled = indValue3.Enabled = true;
				grpBox_Speed.Enabled = panelSpeed;
				grpBox_Corner.Visible = grpBox_Corner.Enabled = panelCorner;
				rbtn_Tangazh.Visible = rbtn_Tangazh.Enabled = buttonPitch;
				btn_Memory.Visible = btn_Memory.Enabled = memory;
				btn_Mode_M_B.Visible =
					btn_Mode_M_C.Visible = btn_Mode_M_B.Enabled = btn_Mode_M_C.Enabled = otherButtons;
			}
		}

		private void SetupIndicatorValue(IndicatorValue ind, Globals.ValueType valueType) //todo убрать меотод 
		{
			double minGreen = double.NaN, maxGreen = double.NaN;
			int min, max;
			Globals.GetMinAndMaxByValue(valueType, out min, out max);
			ind.MinValue = min;
			ind.MaxValue = max;
			int ttId;

			switch (valueType)
			{
				case Globals.ValueType.SpeedForward:
					ind.UpSign = string.Format(Resources.Wxg, Resources.KmH);
					ind.DownSign = Resources.Forward;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task1_ForwardSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedForward;
					break;

				case Globals.ValueType.SpeedUp:
					ind.UpSign = string.Format(Resources.Wyg, Resources.MSec);
					ind.DownSign = Resources.Up;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task2_UpSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedForward;
					break;

				case Globals.ValueType.SpeedBackward:
					ind.UpSign = string.Format(Resources.Wxg, Resources.KmH);
					ind.DownSign = Resources.Back;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task2_BackwardSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedBackward;
					break;

				case Globals.ValueType.SpeedRight:
					ind.UpSign = string.Format(Resources.Wzg, Resources.KmH);
					ind.DownSign = Resources.Right;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task2_RightSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedRight;
					break;

				case Globals.ValueType.SpeedLeft:
					ind.UpSign = string.Format(Resources.Wzg, Resources.KmH);
					ind.DownSign = Resources.Left;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task1_LeftSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedLeft;
					break;

				case Globals.ValueType.SpeedDown:
					ind.UpSign = string.Format(Resources.Wyg, Resources.MSec);
					ind.DownSign = Resources.Down;
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task1_DownSpeed, ref minGreen,
						ref maxGreen);
					ind.Description = Resources.SpeedDown;
					break;
				case Globals.ValueType.SpeedDownUncontrolled:
					ind.UpSign = string.Format(Resources.Wyg, Resources.MSec);
					ind.DownSign = Resources.Down;
					ind.Description = Resources.SpeedDown;
					break;

				case Globals.ValueType.ZeroAngle:
					ind.UpSign = Resources.AlfaGrad;
					ind.DownSign = string.Format(Resources.US, 0);
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task3_Angle, ref minGreen, ref maxGreen);
					ind.Description = Resources.Angle;
					break;

				case Globals.ValueType.FastSpeed1:
					ind.UpSign = string.Format(Resources.WN, Resources.KmH);
					ind.DownSign = string.Format(Resources.PS, 1);
					ttId = IsFlightVSS
						? (int)TestTypeId_Diss32.Task3_Speed_FlightVSS
						: (int)TestTypeId_Diss32.Task3_Speed;
					DataManager.Instance.GetParamLimit(ttId, ref minGreen, ref maxGreen);
					ind.Description = Resources.Speed;
					break;

				case Globals.ValueType.PositiveAngle:
					ind.UpSign = Resources.AlfaGrad;
					ind.DownSign = string.Format(Resources.US, "9,5 " + Resources.Left);
					DataManager.Instance.GetParamLimit((int)TestTypeId_Diss32.Task4_Angle, ref minGreen, ref maxGreen);
					ind.Description = Resources.Angle;
					break;

				case Globals.ValueType.FastSpeed:
					ind.UpSign = string.Format(Resources.WN, Resources.KmH);
					ind.DownSign = string.Format(Resources.PS, "");
					ind.Description = Resources.Speed;
					break;

				case Globals.ValueType.FastSpeed2:
					ind.UpSign = string.Format(Resources.WN, Resources.KmH);
					ind.DownSign = string.Format(Resources.PS, 2);
					ttId = IsFlightVSS
						? (int)TestTypeId_Diss32.Task4_Speed_FlightVSS
						: (int)TestTypeId_Diss32.Task4_Speed;
					DataManager.Instance.GetParamLimit(ttId, ref minGreen, ref maxGreen);
					ind.Description = Resources.Speed;
					break;
			}

			ind.MinGreenValue = minGreen;
			ind.MaxGreenValue = maxGreen;
			ind.ErrorInputValue += ErrorInputValue;
			ind.ErrorDiapazonValue += ErrorDiapazonValue;
		}

		private void IncludeRadioButtonCorner(RadioButton rbtn) //todo возможно оставить
		{
			rbtn.FlatStyle = FlatStyle.Flat;
			rbtn.BackColor = Color.FromArgb(153, 249, 164);
			Control.ControlCollection coll = rbtn.Parent.Controls;
			for (int i = 0; i < coll.Count; i++)
				if (coll[i].GetType() == typeof(RadioButton) &&
				    ((RadioButton)coll[i]).FlatStyle == FlatStyle.Flat &&
				    coll[i].Name != rbtn.Name)
				{
					((RadioButton)coll[i]).FlatStyle = FlatStyle.Standard;
					coll[i].BackColor = Color.Transparent;
				}
		}

		private void ErrorDiapazonValue(string name, double min, double max, double value)
		{
			MessageBox.Show(string.Format(Resources.ErrorDiapazonValue, name, min, max));
		}

		private void ErrorInputValue(string value)
		{
			MessageBox.Show(string.Format(Resources.ErrorInputNValue, value));
		}

		private void WrungButtonTask(Button btn)
		{
			CountButtons = 0;
			btn.FlatStyle = FlatStyle.Standard;
			btn.BackColor = Color.Transparent;
			for (int i = 0; i < arButtonTask.Length; i++)
				arButtonTask[i].Enabled = true;
		}

		private void WrungButtonCT3(Button btn)
        {
            CountButtons = 0;
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackColor = Color.Transparent;
            for (int i = 0; i < btnCT3.Length; i++)
                btnCT3[i].Enabled = true;
        }

		private void ImageTaskStop() //Todo не забыть про комментарии
		{
			if (_activeControl != null)
				_activeControl.Dispose();

			_measure.StopMeasuring(false);
			_measure.StartMeasuring();
			cbShowBlock8.Visible = false;
			if (_activeControl != null)
				_activeControl.Enabled = false;
			if (panel_Task.Visible) // для всех КЗ
			{
				//todo индикатора ещё  нет
				IndicatorDown.Inactive = IndicatorLeft.Inactive = IndicatorUp.Inactive = true;
				indValue1.Enabled = indValue2.Enabled = indValue3.Enabled =
					grpBox_Corner.Enabled = btn_Memory.Enabled = btn_Mode_M_B.Enabled =
						btn_Mode_M_C.Enabled = 
							false;
				//arTxtBox_Speed[0] = arTxtBox_Speed[1] = false;
			}
		}

		private ushort ReverseDirection(ushort currentDirection)
		{
			currentDirection += 180;
			while (currentDirection >= 360)
				currentDirection -= 360;
			return currentDirection;
		}

		private IndicationPanel GetIPControl()
		{
			Control ctrl;
			if (_activeControl is Block8)
				ctrl = _activeControl2;
			else
				ctrl = _activeControl;
			IndicationPanel ip = ctrl as IndicationPanel;
			return ip;
		}

		public void SetIndicatorValue(int number, double val)
		{
			if (number < 0)
				return;
			switch (number)
			{
				case (int)Globals.IndicatorNumber.Speed1:
					{
						if(_activeControl != null && _activeControl is ReadyWorkTimeControl)
							((ReadyWorkTimeControl)_activeControl).SetValueIndicator(number, val);
						else if(_activeControl != null && _activeControl is CaptureModeControl)
                            ((CaptureModeControl)_activeControl).SetValueIndicator(number, val);
                        else
							IndicatorUp.Value = val;
					}
					return;
				case (int)Globals.IndicatorNumber.Speed2:
                    if (_activeControl != null && _activeControl is ReadyWorkTimeControl)
                        ((ReadyWorkTimeControl)_activeControl).SetValueIndicator(number, val);
                    else
                        IndicatorLeft.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Speed1_2:
					if (_activeControl != null)
						if (_activeControl is IndicationPanel)
							((IndicationPanel)_activeControl).IndicatorUp.Value = val;
						else if (_activeControl2 != null)
							if (_activeControl2 is IndicationPanel)
								((IndicationPanel)_activeControl2).IndicatorUp.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Speed2_2:
					if (_activeControl != null)
						if (_activeControl is IndicationPanel)
							((IndicationPanel)_activeControl).IndicatorLeft.Value = val;
						else if (_activeControl2 != null)
							if (_activeControl2 is IndicationPanel)
								((IndicationPanel)_activeControl2).IndicatorLeft.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Speed3:
					IndicatorDown.Value = val;
					return;

				case (int)Globals.IndicatorNumber.Zero1:
					((WPanel)_activeControl).si1_1.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero2:
					((WPanel)_activeControl).si1_2.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero3:
					((WPanel)_activeControl).si1_3.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero4:
					((WPanel)_activeControl).si1_4.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero5:
					((WPanel)_activeControl).si1_5.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero6:
					((WPanel)_activeControl).si1_6.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero1PKV:
					((WPanel)_activeControl).si2_1.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero2PKV:
					((WPanel)_activeControl).si2_2.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero3PKV:
					((WPanel)_activeControl).si2_3.Value = val;
					return;

				case (int)Globals.IndicatorNumber.Zero1Tester:
					((WPanel)_activeControl).si2_4.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero2Tester:
					((WPanel)_activeControl).si2_5.Value = val;
					return;
				case (int)Globals.IndicatorNumber.Zero3Tester:
					((WPanel)_activeControl).si2_6.Value = val;
					return;

				case (int)Globals.IndicatorNumber.ZoptPVK:
					((Task2)_activeControl).siT2_1.Value = val;
					return;
				case (int)Globals.IndicatorNumber.ZoptPNP:
					if (double.IsNaN(MeasuringValueInfo.ZoptPNPStartValue))
						MeasuringValueInfo.ZoptPNPStartValue = val;
					if (_activeControl is Task2)
						((Task2)_activeControl).siT2_2.Value = val - MeasuringValueInfo.ZoptPNPStartValue;
					return;

				case (int)Globals.IndicatorNumber.MapCorner:
					if (_tiMapCorner == null)
					{
						foreach (Control control in ParentForm.Controls)
						{
							if (control.Name == "tiMapCorner")
							{
								_tiMapCorner = (TextIndicator)control;
								break;
							}
						}
					}
					_tiMapCorner.State = IsMapCornerActive(val);
					return;

			}
		}

		public void SetIndicatorLimits(int number, string paramName, bool reverseSign = false)
		{
			USBBaseNew ub = new USBBaseNew(SettingsObj.GetSettings().TestId, paramName, number);

			double max = ub.Max;
			double min = ub.Min;
			if (reverseSign)
			{
				double temp = max;
				max = min * -1;
				min = temp * -1;
			}

			SetIndicatorLimits(number, min, max, ((int)Math.Abs(max)).ToString().Length, ub.Precision);
		}

		private void SetIndicatorLimits(int number, double min, double max, int wholePart, int fractPart)
		{
			if (number < 0)
				return;
			UserControl control = GetIndicatorByNumber(number);
			if (control.GetType() == typeof(SegmentIndication))
			{
				var current = (SegmentIndication)control;
				current.MinValue = min;
				current.MaxValue = max;
				if (wholePart > 0)
					current.WholePart = wholePart;
				if (fractPart > 0)
					current.FractPart = fractPart;
			}
			else if (control.GetType() == typeof(SegmentIndicatorControl))
			{
				var current = (SegmentIndicatorControl)control;
				current.MinValue = min;
				current.MaxValue = max;
				if (wholePart > 0)
					current.WholePart = wholePart;
				if (fractPart > 0)
					current.FractPart = fractPart;

			}
		} //todo Убрать

		/// <summary>
		/// Если границы заданы в процентах
		/// </summary>
		/// <param name="number"></param>
		/// <param name="startValue"></param>
		/// <param name="baseValue"></param>
		/// <param name="paramName"></param>
		/// <param name="reverseSign"></param>
		public void SetIndicatorLimitsPercent(int number, double startValue, double baseValue, string paramName,
			bool reverseSign = false)
		{
			if (double.IsNaN(startValue) || double.IsNaN(baseValue))
				return;
			USBBaseNew ub = new USBBaseNew(SettingsObj.GetSettings().TestId, paramName, number);
			double max = startValue + baseValue / 100 * ub.Max;
			double min = startValue + baseValue / 100 * ub.Min;
			if (reverseSign)
			{
				double temp = max;
				max = min * -1;
				min = temp * -1;
			}

			SetIndicatorLimits(number, min, max, ((int)Math.Abs(max)).ToString().Length, ub.Precision);
		}

		private bool IsMapCornerActive(double voltage)
		{
			return voltage >= 4;
		}

		private UserControl GetIndicatorByNumber(int number)
		{
			switch (number)
			{
				case (int)Globals.IndicatorNumber.Speed1:
					return IndicatorUp;
				case (int)Globals.IndicatorNumber.Speed2:
					return IndicatorLeft;
				case (int)Globals.IndicatorNumber.Speed1_2:
					if (_activeControl is IndicationPanel)
						return ((IndicationPanel)_activeControl).IndicatorUp;
					return null;
				case (int)Globals.IndicatorNumber.Speed2_2:
					if (_activeControl is IndicationPanel)
						return ((IndicationPanel)_activeControl).IndicatorLeft;
					return null;
				case (int)Globals.IndicatorNumber.Zero1:
					return ((WPanel)_activeControl).si1_1;
				case (int)Globals.IndicatorNumber.Zero2:
					return ((WPanel)_activeControl).si1_2;
				case (int)Globals.IndicatorNumber.Zero3:
					return ((WPanel)_activeControl).si1_3;
				case (int)Globals.IndicatorNumber.Zero4:
					return ((WPanel)_activeControl).si1_4;
				case (int)Globals.IndicatorNumber.Zero5:
					return ((WPanel)_activeControl).si1_5;
				case (int)Globals.IndicatorNumber.Zero6:
					return ((WPanel)_activeControl).si1_6;
				case (int)Globals.IndicatorNumber.Zero1PKV:
					return ((WPanel)_activeControl).si2_1;
				case (int)Globals.IndicatorNumber.Zero2PKV:
					return ((WPanel)_activeControl).si2_2;
				case (int)Globals.IndicatorNumber.Zero3PKV:
					return ((WPanel)_activeControl).si2_3;
				case (int)Globals.IndicatorNumber.Zero1Tester:
					return ((WPanel)_activeControl).si2_4;
				case (int)Globals.IndicatorNumber.Zero2Tester:
					return ((WPanel)_activeControl).si2_5;
				case (int)Globals.IndicatorNumber.Zero3Tester:
					return ((WPanel)_activeControl).si2_6;



				case (int)Globals.IndicatorNumber.ZoptPVK:
                    if (_activeControl is Task2)
                        return ((Task2)_activeControl).siT2_1;
                    break;
				case (int)Globals.IndicatorNumber.ZoptPNP:
					if (_activeControl is Task2)
						return ((Task2)_activeControl).siT2_2;
					break;
			}

			return null;
		}

		public void ImageStop()
		{
			if (_activeControl != null)
				_activeControl = null;
			cbShowBlock8.Visible = false;
			//todo индикатора ещё  нет
			IndicatorDown.Inactive = IndicatorLeft.Inactive = IndicatorUp.Inactive = true;
			indValue1.Enabled = indValue2.Enabled = indValue3.Enabled =
				grpBox_Corner.Enabled = btn_Memory.Enabled = btn_Mode_M_B.Enabled =
					btn_Mode_M_C.Enabled =
						false;
		}


		#region Управление ДИССом

		private bool TaskRele(bool state)
		{
			bool result = true;
			result &= DiscreteOutputs.Instance().SetOutput("UNCh_32", state);
			//result &= DiscreteOutputs.Instance().SetOutput("Course", state);
			result &= DiscreteOutputs.Instance().SetOutput("Pitch", state);
			result &= DiscreteOutputs.Instance().SetOutput("Heel", state);
			return result;
		}

		/// <summary>
		/// Включаем КЗ-3, КЗ-4; true - все нормально 
		/// </summary>
		/// <param name="strCT">команда на включение меандров</param>
		/// <param name="strDOutput">команда на включение КЗ</param>
		/// <returns></returns>
		public bool TaskOn(string strCT, string strDOutput)
		{
			bool result = true;
			EventHigh?.Invoke(this, EventArgs.Empty);
			result &= TaskRele(true);
			result &= ControlTask.Instance().SetControlTask(strCT);
			result &= DiscreteOutputs.Instance().SetOutput(strDOutput, true);
			result &= Diss32Functions.SetAngle(_currentDirection);
			return result;
		}

		/// <summary>
		/// Выключаем КЗ-1 - КЗ-4 
		/// <param name="str">команда на выключение КЗ</param>
		/// </summary>
		public bool TaskOff(string str)
		{
			bool result = true;
			result &= DiscreteOutputs.Instance().SetOutput(str, false);
			result &= ControlTask.Instance().SetControlTask(0, 0, 0);
			result &= Diss32Functions.SetAngle(0);
			result &= TaskRele(false);
			return result;
		}

		#endregion

		private void StartWorker()
		{
			_measure.StopMeasuring(false);
			_measure.StartWorkerWithList(SettingsObj.GetSettings().MeasList);
		}

		private void StopWorker()
		{
			StopWorker(SaveResults);
		}

		private void StopWorker(bool b)
		{
			_measure.StopMeasuring(b);
			if (Power115V.Instance.IsAktor)
				_measure.StartMeasuring();
		}

		private string GetTimeString(TimeSpan ts)
		{
			return String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
		}

		private void ChangeTimerSiWork(TimeSpan ts)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() => { siWorkTime.ValueString = GetTimeString(ts); }));
			}
			else
			{
				siWorkTime.ValueString = GetTimeString(ts);
			}
		}

		private void ChangeLabel5Text(string text)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					label5.Text = text;
				}));
			}
			else
			{
				label5.Text = text;
			}
		}
		
		private void InitializeTimerToWork()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					siWorkTime.Visible = true;
					label5.Visible = true;
				}));
			}
			else
			{
				siWorkTime.Visible = true;
				label5.Visible = true;
			}
		}

		private void HideTimerToWork()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					siWorkTime.Visible = false;
					label5.Visible = false;
				}));
			}
			else
			{
				siWorkTime.Visible = false;
				label5.Visible = false;
			}
		}

		public void ErrorDetected(object sender, string errorString)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, string>(ErrorDetected), errorString);
                return;
            }
            MessageBox.Show(errorString, Resources.ErrorCaption);
        }
    }
}