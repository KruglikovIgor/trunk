using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using KPK2.GeneralForms.AddForms;
using KPK2.GeneralForms.Enums;
using KPK2.MyClasse;
using KPK2.MyControl;
using KPK2.Properties;
using KPK2.Interfaces;
using LCards;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using UserControlsKPK2;
using UserControlsKPK2.Enums;
using Timer = System.Windows.Forms.Timer;
using CommonGlobals = KPK2.Common.Globals;
using Functions = Sparc.KPK2.NFunctions.Functions;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using Globals = Sparc.KPK2.NFunctions.Globals;
using Message = System.Windows.Forms.Message;

namespace KPK2.GeneralForms.DissForms
{
  public partial class frmWork15 : Form, IFrmWork15Base, IFrmWork15ForBlack8Check, IFrmWork15ForCorners
    {

        private static Logger _logger = LogManager.GetLogger("KPK2Error");
        #region Определения

        private const double PowerSupply27 = 27.5;
        private const float Voltage115 = 115;

        private const double MaxCurrent115 = 5;
        private const double MaxCurrent27 = 15;
        private const int PowerSupply115Phase = 0;

        /// <summary>
        /// Разрешаем открытие формы - есть БД, СОМ порты и платы 
        /// </summary>
        private bool isOpenForm = true;

        /// <summary>
        /// Количество нажатых кнопок на вкладке 
        /// </summary>
        private int countButton;

        /// <summary>
        /// № новой проверки для записи в БД
        /// </summary>
        private int testId = -1;

        public int TestId
        {
            get { return testId; }
        }


        private DevicesID _deviceId = DevicesID.Diss15;
        private Modification _modification;
        private string _deviceNumber;
        //private Block5Type _curBlock5Type = Block5Type.None;

        /// <summary>
        /// Нажата кнопка Escape - закрываем эту форму 
        /// </summary>
        private bool isEscape;

        private Measure _measWorker = new Measure();

        private Label[] arUpLabel = new Label[16];
        private Label[] arDownLabel = new Label[16];
        private SegmentIndication[] arInd = new SegmentIndication[16];

        /// <summary>
        /// Для КЗ-4 - введенное значение угла->БД
        /// </summary>
        private double task4_YC0;

        /// <summary>
        /// Для КЗ-4 - введенное значение скорости ПС->БД
        /// </summary>
        private double task4_PC;

        /// <summary>
        /// Для КЗ-4 - вычесленное значение разности угла->БД
        /// </summary>
        private double task4_YC0_dif;

        /// <summary>
        /// Для КЗ-4 - вычесленное значение разности скорости ПС->БД
        /// </summary>
        private double task4_PC_dif;

        /// <summary>
        /// [] что ввели в TextBox-ы => видимоть и что вводить для
        /// вычисления разности 
        /// </summary>
        private bool[] arTask4Visible = new bool[2];

        /// <summary>
        /// [] ввода значений "Путь" и "Боковое уклонение" в TextBox-x (КЗ-4)
        /// и  разблокировки "Обнуление"...
        /// </summary>
        private bool[] arTask4_8_2 = new bool[2];

        /// <summary>
        /// 1-е название btn_Memory ("ПАМЯТЬ")
        /// </summary>
        private string memory1 = String.Format(Res.MemoryN, String.Empty);

        /// <summary>
        /// 2-е название btn_Memory ("Найти...")
        /// </summary>
        private string memory2 = Res.FindDAlfaDW;

        /// <summary>
        /// 3-е название btn_Memory ("Сохранить")
        /// </summary>
        private string memory3 = Res.Save;

        /// <summary>
        /// Счетчик времени выключения Блока 8 для КЗ-4 и КЗ-6 
        /// </summary>
        private int countBlock8;

        /// <summary>
        /// []Button для управления на вкладке КЗ
        /// </summary>
        private List<Button> listButtonTask = new List<Button>();

        /// <summary>
        /// []Button для управления на вкладке "Измерения" 
        /// </summary>
        private Button[] arButton = new Button[6];

        /// <summary>
        /// Массив "видимости" индикаторов - 6 режимов "Проведение измерений"
        /// </summary>
        private int[,] arVisible =
          {
        {1, 1, 1, 1, 1, 1}, // 1-й индикатор 1-й ряд
        {1, 1, 1, 1, 1, 1},
        {0, 0, 1, 0, 0, 0},
        {0, 1, 0, 1, 1, 1}, // 4-й индикатор
        {1, 0, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 0},
        {0, 0, 0, 1, 1, 1}, // 7-й индикатор
        {0, 0, 0, 1, 1, 1},
        {0, 1, 0, 1, 0, 0}, // 1-й индикатор 2-й ряд
        {0, 0, 0, 0, 0, 0},
        {0, 1, 0, 0, 0, 0},
        {0, 1, 0, 1, 0, 0}, // 4-й индикатор 2-й ряд
        {0, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 1, 0, 0}, // 7-й индикатор 2-й ряд
        {0, 0, 0, 0, 0, 0}
      };

        /// <summary>
        /// [6]- "БПК" rbtn Да/Нет (загорелось/погасло) 
        /// </summary>
        //RadioButton[] arRBtn = new RadioButton[6];
        /// <summary>
        /// [11] для измерения питающих напряжений 
        /// </summary>
        private object[] arAllPitan = new object[10];

        /// <summary>
        /// Начало/конец измерения "ПАМЯТЬ", "Скорости отработки",
        /// время вкл."ВН" 
        /// </summary>
        private DateTime time_On;

        /// <summary>
        /// Для отображения времени "ПАМЯТЬ","Скорости отработки",
        /// времени вкл."ВН" 
        /// </summary>
        private double dTime = 0.0;

        /// <summary>
        /// [2]-измерение времени вкл.ВН; 
        /// [0]-пришел сигнал "Изделие"; 
        /// [1]-пришел сигнал "ВН" 
        /// </summary>
        private bool[] arBH = new[] { false, false };

        /// <summary>
        /// Для разблокирования btn_Tok_K; true- есть сигнал "ВН",т.е. можно измерять 
        /// Токи К; false- измерять нельзя!!!! 
        /// </summary>
        private bool forTok_K;

        /// <summary>
        /// Минимум для БП 27В = 26.125В(default)
        /// </summary>
        private double power27_Min = 27.125;

        /// <summary>
        /// Максимум для БП 27В = 28.875В(default) 
        /// </summary>
        private double power27_Max = 29.875;

        /// <summary>
        /// Норма для БП 27В = 27.5В(default) 
        /// </summary>
        private double power27_Norma = 28.5;

        /// <summary>
        /// Минимум для БП 115В = 109.2В(default)
        /// </summary>
        private double power115_Min = 110.4;

        /// <summary>
        /// Максимум для БП 115В = 120.8В(default) 
        /// </summary>
        private double power115_Max = 119.6;

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

        private Thread threadStrip;
        private Imitator.Device _device;

        /// <summary>
        /// Активный контрол, который отображается при включении задачи, который необходиму убрать по завершению
        /// </summary>
        private Control _activeControl;

        private bool saveResults
        {
            get { return testId != -1; }
        }

        private const int HVMidDelayValue = 30;

        #endregion

        public frmWork15(Modification mod)
        {
            _modification = mod;
            if (Globals.Emulation)
                _device = new Imitator.Diss15();
            InitializeComponent();

            //if (mod == Modification.Diss15G)
            //{
            //    tabControl.TabPages.Remove(tbLink);
            //}

            //if (mod == Modification.None)
            if (mod != Modification.Diss15G)
            {
                tabControl.TabPages.Remove(tbLink);
                tiNavigation.Visible = tiWorkability.Visible = tiMapCorner.Visible = false;
            }

            if (isOpenForm)
            {
                InitializArray();
                CreateIndicators();
            }

        }

        private ManualResetEvent _mreMeasureStrip = new ManualResetEvent(false);
        private bool errorPS27VAppear = false;

        private void MeasureStrip()
        {
            if (_strip == null)
                _strip = new PowerStrip(false, testId, ShowStrip, null);
            while (!_mreMeasureStrip.WaitOne(0))
            {
                try
                {
                    _strip.Execute(_mreMeasureStrip);
                    if (!errorPS27VAppear)
                        if (Common.Globals.CheckPS27Protection(Power27V.Instance, ShowError))
                            errorPS27VAppear = true;
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
                Thread.Sleep(500);
            }
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

        private const string indName = "siInd";
        private const string lUpName = "lUp";
        private const string lDownName = "lDown";
        private const int NumIndInRow = 8;

        private void CreateIndicators()
        {
            const int baseX = 5;
            const int baseY = 32;
            const int baseWidth = 108;
            const int baseHeight = 40;
            const int stepX = baseWidth + 11;
            const int stepY = baseHeight + 40;

            SegmentIndication si;
            Label lbl;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < NumIndInRow; j++)
                {
                    si = new SegmentIndication();
                    si.Name = indName + "_" + i + "_" + j;
                    //arInd.Add(si);
                    arInd[j + i * NumIndInRow] = si;
                    SegmentIndicatorDefault(ref si);
                    si.Size = new Size(baseWidth, baseHeight);
                    gbAll.Controls.Add(si);
                    si.Location = new Point(baseX + j * stepX, baseY + i * stepY);
                    //Верхний лейбл
                    lbl = new Label();
                    lbl.Name = lUpName + "_" + i + "_" + j;
                    lbl.Text = String.Format(Resources.UV, Resources.Volt);
                    //arUpLabel.Add(lbl);
                    arUpLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    lbl.AutoSize = true;
                    gbAll.Controls.Add(lbl);
                    lbl.Location = new Point(baseX + j * stepX, baseY + i * stepY - lbl.Size.Height - 3);
                    //Нижний лейбл
                    lbl = new Label();
                    lbl.Name = lDownName + "_" + i + "_" + j;
                    lbl.Text = "";
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    //arDownLabel.Add(lbl);
                    arDownLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    //lbl.AutoSize = true;
                    lbl.Size = new Size(baseWidth, (int)(lbl.Font.Size * 2));
                    gbAll.Controls.Add(lbl);
                    lbl.Location = new Point(baseX + j * stepX, baseY + i * stepY + baseHeight);
                }
        }

        private static void SegmentIndicatorDefault(ref SegmentIndication si)
        {
            si.Font = new Font("Microsoft Sans Serif", 21.75f);
            si.DarkColor = Color.Transparent;
            si.Unit = "";
            si.Value = 0;
            si.Visible = false;
        }

        private static void LabelDefault(ref Label lbl)
        {
            lbl.Font = new Font("Verdana", 8.25f);
            lbl.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">№ новой проверки для записи в БД</param>
        public frmWork15(int id, Modification mod, Block5Type b5Type, string devNum = "")
          : this(mod)
        {
            testId = id;
            _deviceNumber = devNum;
            // _curBlock5Type = b5Type;
        }

        #region Всякое для конструктора

        /// <summary>
        /// Свойство возвращает флаг открытия формы; true-можно 
        /// </summary>
        public bool IsOpenForm
        {
            get { return isOpenForm; }
        }

        /// <summary>
        /// Инициализация [] IndicatorNumbers, Labels и Button для проведения измерений
        /// </summary>
        private void InitializArray()
        {
            listButtonTask.Add(btn_Task_1);
            listButtonTask.Add(btn_Task_2);
            listButtonTask.Add(btn_Task_3);
            listButtonTask.Add(btn_Task_4);
            listButtonTask.Add(btn_Task_5);
            listButtonTask.Add(btn_Task_6);
            listButtonTask.Add(btn_BPK);
            listButtonTask.Add(btn_Zero);
            listButtonTask.Add(btn_Speed);
            listButtonTask.Add(btn_Freq);
            listButtonTask.Add(bAist);
            listButtonTask.Add(bSAU);
            listButtonTask.Add(bSU);
            listButtonTask.Add(bLandysh);
            listButtonTask.Add(bDVS24);

            arButton[0] = btn_FirstPitan;
            arButton[1] = btn_SecPitan;
            arButton[2] = btn_TKM;
            arButton[3] = btn_Tok_K;
            arButton[4] = btn_UNCh;
            arButton[5] = btn_Noise;

            // для постоянного измерения питающих напряжений 
            arAllPitan[0] = testId;
            arAllPitan[1] = "Limit_15"; // Список сигналов для их id(запись результатов в БД)
            arAllPitan[2] = btn_FirstPitan; // "Источники питания"
            arAllPitan[4] = KPK2DataSet;
            arAllPitan[9] = btn_On8; // для измерения max значения тока 
        }

        #endregion

        private void frmWork15_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _measWorker.StopMeasuring(saveResults);
                MeasureStrip(false);
                DiscreteInputs.Instance().StopMeasuring();
                if (!rbtn_Off.Checked)
                    rbtn_Off.PerformClick();

                Power115V.Instance.SetOnOff(false);
                Power27V.Instance.SetOnOff(false);
                //_lambda27V.OnOffState(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(string.Format("{0} {1}",Resources.ErrorsOnDevices, Resources.CloseFormError));
            }
            finally
            {
                MeasureStrip(false);
                // разблокировка меню 
                if (ParentForm != null)
                    ((frmMain)ParentForm).EnableMenu(true);
            }
        }

        private PowerStrip _strip; // "Полоски" 

        private void frmWork15_Load(object sender, EventArgs e)
        {
            bool bl = false;
            try
            {
                if (String.IsNullOrEmpty(_deviceNumber))
                {
                    Text = String.Format(Res.CheckDeviceN, Globals.GetDissCaption(_deviceId, _modification));
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }
                else
                {
                    Text = String.Format(Res.CheckDeviceNN, Globals.GetDissCaption(_deviceId, _modification),
                        _deviceNumber);
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }
                siWorkTime.Inactive = true;
                DiscreteInputs.Instance().StopMeasuring();
                DiscreteOutputs.Instance().SetChannelsOff();
                _measWorker.ResultsReady += ShowResult;
                if (!CommonGlobals.InitPower27V(PowerSupply27, MaxCurrent27))
                    MessageBox.Show(String.Format(Resources.ErrorOnDeviceN, String.Format(Resources.PowerSupplyN, 40)));
                if (!CommonGlobals.InitPower115(PowerSupply115Phase, MaxCurrent115))
                    MessageBox.Show(String.Format(Resources.ErrorOnDeviceN,
                        String.Format(Resources.PowerSupplyN, 115)));
                else
                    Power115V.Instance.SetOnOff(false);

                if (!Globals.Emulation)
                    if (!LTRManager.CheckCrate())
                        MessageBox.Show(Resources.LTRCrateFailed);

                if (Power115V.Instance.IsAktor)
                {
                    AddMeasurement(PS115V_Read_U, (int)Globals.IndicatorNumber.Power115_U, "", "");
                    AddMeasurement(PS115V_Read_I, (int)Globals.IndicatorNumber.Power115_I, "", "");
                    _measWorker.HoldSignals = 2;
                    StartWorker();
                }
            }
            catch (Exception ex)
            {
                bl = true;
                MessageBox.Show(Resources.CommandError + " " + ex.Message);
            }
            if (bl) // все блокировать 
            {
                tabControl.Enabled = grpBox_Mode.Enabled =
                                     gbPowerMode.Enabled = false;
                return;
            }
            ImageDesigner(ModeMeasurementsDiss15.OpenForm);
            // заблокировать "Режим питания"
            gbPowerMode.Enabled = false;

            btn_High.Enabled = false;
            //полоски 
            arAllPitan[0] = testId;
            //arAllPitan[6] = power115;
            //isPower = true;
            //try { Strip = new PowerStrip(arAllPitan); }
            try
            {
                _strip = new PowerStrip(false, testId, ShowStrip, null);
            }
            catch (Exception ex)
            {
                bl = true;
                MessageBox.Show(String.Format(Resources.ErrorCommand, "") + ex.Message);
            }
            if (bl) // все блокировать 
            {
                tabControl.Enabled = grpBox_Mode.Enabled =
                                     gbPowerMode.Enabled = false;
                return;
            }
            tabControl.Enabled = false;
        }


        private List<USBBaseNew> _measList = new List<USBBaseNew>();

        private void ClearMeasurementList()
        {
            _measList.Clear();
        }

        private void AddMeasurement(string paramName, int indicatorNumber, string upString, string downString)
        {
            if (indicatorNumber < 0)
                return;
            USBBaseNew ub = new USBBaseNew(testId, paramName, indicatorNumber);
            _measList.Add(ub);
            SetupIndicator(indicatorNumber, upString, downString, "0.00");
            int wholeNumbers = Math.Max(((int)Math.Abs(ub.Max)).ToString().Length, ((int)Math.Abs(ub.Min)).ToString().Length);
            SetIndicatorLimits(indicatorNumber, ub.Min, ub.Max, wholeNumbers, ub.Precision);
        }

        public void SetupIndicator(int number, string upSide, string downSide, string startValue)
        {
            if (number < 0)
                return;
            Label currentL = GetIndicatorLabel(number, true);
            if (!String.IsNullOrEmpty(upSide))
                if (currentL != null)
                    currentL.Text = upSide;
            currentL = GetIndicatorLabel(number, false);
            if (!String.IsNullOrEmpty(downSide))
                if (currentL != null)
                    currentL.Text = downSide;
            SegmentIndication currentI = GetIndicatorByNumber(number);
            if (currentI == null) return;
            double checkDouble = Functions.MoveToDouble(startValue);
            if (!double.IsNaN(checkDouble))
                currentI.Value = checkDouble;
            else
                currentI.ValueString = startValue;
            currentI.MaxValue = currentI.MinValue = 0;
            currentI.Inactive = true;
        }

        private Label GetIndicatorLabel(int number, bool isUp)
        {
            switch (number)
            {
                case (int)Globals.IndicatorNumber.Speed1:
                    return isUp ? lbl_Task_1_Up : lbl_Task_1;
                case (int)Globals.IndicatorNumber.Speed2:
                    return isUp ? lbl_Task_2_Up : lbl_Task_2;
                default:
                    if (isUp)
                    {
                        if (number < arUpLabel.Length)
                            return arUpLabel[number];
                    }
                    if (!isUp)
                    {
                        if (number < arDownLabel.Length)
                            return arDownLabel[number];
                    }

                    break;
            }
            return null;
        }

        private SegmentIndication GetIndicatorByNumber(int number)
        {
            switch (number)
            {
                case (int)Globals.IndicatorNumber.Speed1:
                    return siSpeed1;
                case (int)Globals.IndicatorNumber.Speed2:
                    return siSpeed2;
                case (int)Globals.IndicatorNumber.Zero1:
                    return ((ZeroVoltage)_activeControl).siZero1;
                case (int)Globals.IndicatorNumber.Zero2:
                    return ((ZeroVoltage)_activeControl).siZero2;
                case (int)Globals.IndicatorNumber.Zero3:
                    return ((ZeroVoltage)_activeControl).siZero3;
                case (int)Globals.IndicatorNumber.Aist4W:
                    return ((ControlAist)_activeControl).siKZ4W;
                case (int)Globals.IndicatorNumber.Aist4A:
                    return ((ControlAist)_activeControl).siKZ4A;
                case (int)Globals.IndicatorNumber.Aist5A:
                    return ((ControlAist)_activeControl).siKZ5A;
                case (int)Globals.IndicatorNumber.AistZeroA:
                    return ((ControlAist)_activeControl).siZero;
                case (int)Globals.IndicatorNumber.Aist6W:
                    return ((ControlAist)_activeControl).siKZ6W;
                case (int)Globals.IndicatorNumber.Aist6A:
                    return ((ControlAist)_activeControl).siKZ6A;

                case (int)Globals.IndicatorNumber.SAU6A:
                    return ((ControlSAU)_activeControl).siKZ6A;

                case (int)Globals.IndicatorNumber.SAU1Wxr:
                    return ((ControlSAU)_activeControl).siKZ1Wxr;
                case (int)Globals.IndicatorNumber.SAU1Wyr:
                    return ((ControlSAU)_activeControl).siKZ1Wyr;
                case (int)Globals.IndicatorNumber.SAU2Wzr:
                    return ((ControlSAU)_activeControl).siKZ2Wzr;
                case (int)Globals.IndicatorNumber.SAU3Wxr:
                    return ((ControlSAU)_activeControl).siKZ3Wxr;
                case (int)Globals.IndicatorNumber.SAU3Wyr:
                    return ((ControlSAU)_activeControl).siKZ3Wyr;
                case (int)Globals.IndicatorNumber.SAU3Wzr:
                    return ((ControlSAU)_activeControl).siKZ3Wzr;
                case (int)Globals.IndicatorNumber.SUWxr:
                    return ((ControlSU)_activeControl).siKZ1Wxr;
                case (int)Globals.IndicatorNumber.SU_KZ1_Wyr:
                    return ((ControlSU)_activeControl).siKZ1Wyr;
                case (int)Globals.IndicatorNumber.SU_KZ1_Wyr2:
                    return ((ControlSU)_activeControl).siKZ1_1Wyr;
                case (int)Globals.IndicatorNumber.SUWzr:
                    return ((ControlSU)_activeControl).siKZ2Wzr;
                case (int)Globals.IndicatorNumber.SUWyr:
                    return ((ControlSU)_activeControl).siKZ1_1Wyr;
                case (int)Globals.IndicatorNumber.SU_KZ4_Wyr:
                    return ((ControlSU)_activeControl).siKZ4Wyr;
                case (int)Globals.IndicatorNumber.SU_KZ5_Wxr:
                    return ((ControlSU)_activeControl).siKZ5Wxr;
                case (int)Globals.IndicatorNumber.SU_KZ5_Wzr:
                    return ((ControlSU)_activeControl).siKZ5Wzr;

                case (int)Globals.IndicatorNumber.LandyshKZ2Wxr:
                    return ((ControlLandysh)_activeControl).siKZ2Wxr;
                case (int)Globals.IndicatorNumber.LandyshKZ2Wzr:
                    return ((ControlLandysh)_activeControl).siKZ2Wzr;
                case (int)Globals.IndicatorNumber.LandyshKZ4Wxr:
                    return ((ControlLandysh)_activeControl).siKZ4Wxr;
                case (int)Globals.IndicatorNumber.LandyshKZ4Wzr:
                    return ((ControlLandysh)_activeControl).siKZ4Wzr;
                case (int)Globals.IndicatorNumber.LandyshKZ5Wzr:
                    return ((ControlLandysh)_activeControl).siKZ5Wzr;
                case (int)Globals.IndicatorNumber.LandyshKZ6Wxr:
                    return ((ControlLandysh)_activeControl).siKZ6Wxr;
                case (int)Globals.IndicatorNumber.LandyshKZ6Wzr:
                    return ((ControlLandysh)_activeControl).siKZ6Wzr;
                case (int)Globals.IndicatorNumber.LandyshTime1:
                    return ((ControlLandysh)_activeControl).siTimeX;
                case (int)Globals.IndicatorNumber.LandyshTime2:
                    return ((ControlLandysh)_activeControl).siTimeZ;
                default:
                    if (number < arInd.Length)
                        return arInd[number];
                    break;
            }
            return null;
        }

        /// <summary>
        /// Если границы заданы в процентах
        /// </summary>
        /// <param name="number"></param>
        /// <param name="startValue"></param>
        /// <param name="baseValue"></param>
        /// <param name="paramName"></param>
        /// <param name="reverseSign"></param>
        public void SetIndicatorLimitsPercent(int number, double startValue, double baseValue, string paramName, bool reverseSign = false)
        {
            USBBaseNew ub = new USBBaseNew(testId, paramName, number);
            double max = startValue + baseValue / 100 * ub.Max;
            double min = startValue + baseValue / 100 * ub.Min;
            if (reverseSign)
            {
                double temp = max;
                max = min * -1;
                min = temp * -1;
            }
            SetIndicatorLimits(number, Convert.ToSingle(min), Convert.ToSingle(max), ((int)Math.Abs(max)).ToString().Length, ub.Precision);
        }

        public void SetIndicatorLimits(int number, double baseValue, string paramName)
        {
            USBBaseNew ub = new USBBaseNew(testId, paramName, number);

            double max = baseValue + ub.Max;
            double min = baseValue + ub.Min;
            SetIndicatorLimits(number, Convert.ToSingle(min), Convert.ToSingle(max), ((int)Math.Abs(max)).ToString().Length, ub.Precision);
        }

        public void SetIndicatorLimits(int number, string paramName, bool reverseSign = false)
        {
            USBBaseNew ub = new USBBaseNew(testId, paramName, number);

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

        public void SetIndicatorLimits(int number, double min, double max, int wholePart, int fractPart)
        {
            if (number < 0)
                return;
            SegmentIndication current = GetIndicatorByNumber(number);
            if (current == null)
                return;
            current.MinValue = min;
            current.MaxValue = max;
            SetIndicatorLimits(number, wholePart, fractPart);
        }

        public void SetIndicatorLimits(int number, int wholePart, int fractPart)
        {
            if (number < 0)
                return;
            SegmentIndication current = GetIndicatorByNumber(number);
            if (current == null)
                return;
            if (wholePart > 0)
                current.WholePart = wholePart;
            if (fractPart > 0)
                current.FractPart = fractPart;
        }

        private void StartWorker()
        {
            _measWorker.StopMeasuring(false);
            _measWorker.StartWorkerWithList(_measList);
        }

        public void ShowStrip(double[] arResult)
        {
            for (int i = 0; i < 2; i++)
                piStrip.Bars[i].Value = (float)Math.Round(arResult[i], 1);
            if (!Power115V.Instance.IsAktor) //если ИП другой, то плат-нормализаторов нет, значит измеряем с источника
            {
                for (int i = 2; i < 4; i++)
                    piStrip.Bars[i].Value = (float)Math.Round(arResult[i], i - 2);
                if (btn_FirstPitan.FlatStyle == FlatStyle.Flat)
                {
                    SetIndicatorValue(4, arResult[2]);
                    SetIndicatorValue(5, arResult[3]);
                }

            }
            if (btn_FirstPitan.FlatStyle == FlatStyle.Flat)
                SetIndicatorValue(1, arResult[1]);
        }

        public void SetIndicatorValue(int number, double val)
        {
            if (number < 0)
                return;
            switch (number)
            {
                case (int)Globals.IndicatorNumber.Power115_U:
                    piStrip.Bars[2].Value = (float)Math.Round(val, 1);
                    if (btn_FirstPitan.FlatStyle == FlatStyle.Flat)
                        SetIndicatorValue(4, val);
                    return;
                case (int)Globals.IndicatorNumber.Power115_I:
                    piStrip.Bars[3].Value = (float)Math.Round(val, 1);
                    if (btn_FirstPitan.FlatStyle == FlatStyle.Flat)
                        SetIndicatorValue(5, val);
                    return;
                case (int)Globals.IndicatorNumber.Speed1:
                case (int)Globals.IndicatorNumber.Speed2:
                case (int)Globals.IndicatorNumber.Zero1:
                case (int)Globals.IndicatorNumber.Zero2:
                case (int)Globals.IndicatorNumber.Zero3:
                case (int)Globals.IndicatorNumber.Aist4W:
                case (int)Globals.IndicatorNumber.Aist4A:
                case (int)Globals.IndicatorNumber.Aist5A:
                case (int)Globals.IndicatorNumber.AistZeroA:
                case (int)Globals.IndicatorNumber.Aist6W:
                case (int)Globals.IndicatorNumber.Aist6A:
                case (int)Globals.IndicatorNumber.SAU6A:
                case (int)Globals.IndicatorNumber.SAU1Wxr:
                case (int)Globals.IndicatorNumber.SAU1Wyr:
                case (int)Globals.IndicatorNumber.SAU2Wzr:
                case (int)Globals.IndicatorNumber.SAU3Wxr:
                case (int)Globals.IndicatorNumber.SAU3Wyr:
                case (int)Globals.IndicatorNumber.SAU3Wzr:
                case (int)Globals.IndicatorNumber.SUWxr:
                case (int)Globals.IndicatorNumber.SU_KZ1_Wyr:
                case (int)Globals.IndicatorNumber.SU_KZ1_Wyr2:
                case (int)Globals.IndicatorNumber.SUWzr:
                case (int)Globals.IndicatorNumber.SUWyr:
                case (int)Globals.IndicatorNumber.SU_KZ4_Wyr:
                case (int)Globals.IndicatorNumber.SU_KZ5_Wxr:
                case (int)Globals.IndicatorNumber.SU_KZ5_Wzr:
                case (int)Globals.IndicatorNumber.LandyshKZ2Wxr:
                case (int)Globals.IndicatorNumber.LandyshKZ2Wzr:
                case (int)Globals.IndicatorNumber.LandyshKZ4Wxr:
                case (int)Globals.IndicatorNumber.LandyshKZ4Wzr:
                case (int)Globals.IndicatorNumber.LandyshKZ5Wzr:
                case (int)Globals.IndicatorNumber.LandyshKZ6Wxr:
                case (int)Globals.IndicatorNumber.LandyshKZ6Wzr:
                    GetIndicatorByNumber(number).Value = val;
                    return;
                case (int)Globals.IndicatorNumber.LandyshStator:
                    ((ControlLandysh)_activeControl).StatorVoltage = val;
                    return;
            }
            arInd[number].Value = val;
        }

        private double GetIndicatorValue(int number)
        {
            if (number < 0)
                return 0;
            switch (number)
            {
                case (int)Globals.IndicatorNumber.Power115_U:
                    return piStrip.Bars[2].Value;
                case (int)Globals.IndicatorNumber.Power115_I:
                    return piStrip.Bars[3].Value;
            }
            return arInd[number].Value;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                isEscape = true;
                Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.ToString().IndexOf("WM_CLOSE") != -1)
                isEscape = true;

            base.WndProc(ref m);
        }

        // Перешли на другую вкладку 
        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Name)
            {
                case "pageWork": // проводим все измерения
                    ImageDesigner(ModeMeasurementsDiss15.OpenForm);
                    break;
                case "pageTask": // проводим Контрольные задачи
                    ImageTask(ModeTaskDiss15.OpenForm);
                    break;
                case "tbLink": // проводим проверки связи с оборудованием
                    break;
                default:
                    MessageBox.Show(Resources.ErrorCaption, Resources.ApplicationName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    break;
            }
        }

        // Начинаем переход по вкладкам
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // При нажатых кнопках на вкладке запрещаем переход на другую вкладку
            if (countButton != 0)
            {
                e.Cancel = true;
                MessageBox.Show(Resources.CantChangeTab, Resources.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!(arBH[0] && arBH[1])) // на "КЗ" переход после измерен.времени.вкл."ВН"
            {
                e.Cancel = true;
                MessageBox.Show(Resources.WaitWhileMeasureHVTime, Resources.ApplicationName, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void TaskCustomView(bool panelTask, bool firstInd, bool SecondInd, bool Memory, bool panelKZ4)
        {
            gbTask.Visible = panelTask;
            if (panelTask)
            {
                lbl_Task_1.Visible =
                  lbl_Task_1_Up.Visible = siSpeed1.Visible = indValue1.Visible = indValue1.Enabled = firstInd;
                lbl_Task_2.Visible =
                  lbl_Task_2_Up.Visible = siSpeed2.Visible = indValue2.Visible = indValue2.Enabled = SecondInd;
                indValue1.Value = indValue2.Value = double.NaN;
                indValue1.ForeColor = indValue2.ForeColor = SystemColors.WindowText;

            }
            pMemory.Visible = btn_Memory.Enabled = Memory;
            if (Memory)
                label46.Visible = siMemory.Visible = lMemoryUpLabel.Visible = false;
            gbTask4.Visible = panelKZ4;
        }

        #region Обработка click-ов по всей форме

        #region Режимы работы

        // ДИСС выключен 
        private void rbtn_Off_Click(object sender, EventArgs e)
        {
            if (rbtn_Off.FlatStyle == FlatStyle.Standard)
            {
                StopDiss();
                rbtn_Off.Text = Resources.ButtonDissOffAlready;
                rbtn_Off.FlatStyle = FlatStyle.Flat;
                rbtn_On.FlatStyle = FlatStyle.Standard;
            }
        }

        // ДИСС выключить
        private void rbtn_Off_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_Off.FlatStyle == FlatStyle.Flat)
                rbtn_Off.Text = Resources.ButtonDissReadyToOff;
        }

        // ДИСС включен 
        private void rbtn_On_Click(object sender, EventArgs e)
        {
            if (rbtn_On.FlatStyle == FlatStyle.Standard)
            {
                StartDiss();
                rbtn_On.Text = Resources.ButtonDissOnAlready;
                rbtn_On.FlatStyle = FlatStyle.Flat;
                rbtn_Off.FlatStyle = FlatStyle.Standard;
            }
        }

        // ДИСС включить 
        private void rbtn_On_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_On.FlatStyle == FlatStyle.Flat)
                rbtn_On.Text = Resources.ButtonDissReadyToOn;
        }

        private bool SetHighVoltage(bool state)
        {
            _highVoltageState = state;
            return DiscreteOutputs.Instance().SetOutput("High_15", !state);
        }

        // ВЫСОКОЕ включить/выключить + 
        private void btn_High_Click(object sender, EventArgs e)
        {
            if (btn_High.FlatStyle == FlatStyle.Standard)
            {
                try
                {
                    SetHighVoltage(true);
                    //DiscreteOutputs.Instance().SetOutput("High_15", false);
                    //ControlSignal("Higt_On_15"); 
                }
                catch
                {
                }
                btn_High.Text = Resources.HVOff;
                btn_High.FlatStyle = FlatStyle.Flat;
                btn_High.BackColor = Globals.SuccessColor;
            }
            else
            {
                try
                {
                    SetHighVoltage(false);
                    //DiscreteOutputs.Instance().SetOutput("High_15", true);
                    //ControlSignal("Higt_Off_15"); 
                }
                catch
                {
                }
              ;
                btn_High.Text = Resources.HVOn;
                btn_High.FlatStyle = FlatStyle.Standard;
                btn_High.BackColor = Color.Transparent;
            }
        }

        #endregion

        #region Режим питания

        /// <summary>
        /// Обработчик кнопки "27,5В-5% ~115В-4%" - сигналы "27_5_Minus" и "115V_Minus"
        /// </summary>
        private void rbtn_Minus_Click(object sender, EventArgs e)
        {
            if (rbtn_Minus.FlatStyle == FlatStyle.Standard)
            {
                //_lambda27V.SetVoltage(power27_Min);
                Power27V.Instance.SetVoltage(power27_Min);
                Power115V.Instance.SetVoltage(power115_Min);
                //Aktor115VSetVoltage(power115_Min);
                //ControlSignal_27V(power27_Min);
                //ControlSignal_115V((float)power115_Min); 
                rbtn_Minus.FlatStyle = FlatStyle.Flat;
                rbtn_Norma.FlatStyle = rbtn_Plus.FlatStyle = FlatStyle.Standard;
                if (btn_On8.Enabled) // Откл.Блок 8 
                {
                    if (btn_On8.FlatStyle == FlatStyle.Flat)
                        btn_On8.PerformClick();
                    btn_On8.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Обработчик кнопки "27,5B ~115B" - сигналы "27_5_On" и "115V_Norma"
        /// </summary>
        private void rbtn_Norma_Click(object sender, EventArgs e)
        {
            if (rbtn_Norma.FlatStyle == FlatStyle.Standard)
            {
                //_lambda27V.SetVoltage(power27_Norma);
                Power27V.Instance.SetVoltage(power27_Norma);
                Power115V.Instance.SetVoltage(power115_Norma);
                //Aktor115VSetVoltage(power115_Norma);
                //ControlSignal_27V(power27_Norma);
                //ControlSignal_115V((float)power115_Norma);
                rbtn_Norma.FlatStyle = FlatStyle.Flat;
                rbtn_Minus.FlatStyle = rbtn_Plus.FlatStyle = FlatStyle.Standard;
                if (btn_On8.Enabled) // Откл.Блок 8 
                {
                    if (btn_On8.FlatStyle == FlatStyle.Flat)
                        btn_On8.PerformClick();
                    btn_On8.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Обработчик кнопки "27,5В+5% ~115В+4%" - сигналы "27_5_Plus" и "115V_ Plus"
        /// </summary>
        private void rbtn_Plus_Click(object sender, EventArgs e)
        {
            if (rbtn_Plus.FlatStyle == FlatStyle.Standard)
            {
                //_lambda27V.SetVoltage(power27_Max);
                Power27V.Instance.SetVoltage(power27_Max);
                Power115V.Instance.SetVoltage(power115_Max);
                //Aktor115VSetVoltage(power115_Max);
                //ControlSignal_27V(power27_Max);
                //ControlSignal_115V((float)power115_Max);
                rbtn_Plus.FlatStyle = FlatStyle.Flat;
                rbtn_Minus.FlatStyle = rbtn_Norma.FlatStyle = FlatStyle.Standard;
                btn_On8.Enabled = btn_FirstPitan.FlatStyle == FlatStyle.Flat ? true : false;
            }
        }

        #region Подстройка БП

        // -0,1В для 27В
        private void btn_27_Minus_Click(object sender, EventArgs e)
        {
            btn_27_Minus.Enabled = false;
            // можем прибавлять напряжение 
            btn_27_Plus.Enabled = arPower_27[1] = true;
            if (!arPower_27[0]) // достигли min 
                return;
            if (rbtn_Minus.Checked)
                power27_Min -= 0.1;
            if (rbtn_Norma.Checked)
                power27_Norma -= 0.1;
            if (rbtn_Plus.Checked)
                power27_Max -= 0.1;
            double ddd = rbtn_Minus.Checked
                           ? power27_Min
                           : rbtn_Norma.Checked ? power27_Norma : power27_Max;
            // выставили min на БП 
            arPower_27[0] = ddd <= 26.0 ? false : true;
            try
            {
                //ControlSignal_27V((float)ddd);
                Power27V.Instance.SetVoltage(ddd); // _lambda27V.SetVoltage(ddd)
            }
            catch
            {
            }
            lbl_Edit_27V.Text = arPower_27[0] ? "" : "Минимальное значение";
            btn_27_Minus.Enabled = arPower_27[0];
        }

        // +0,1В для 27В
        private void btn_27_Plus_Click(object sender, EventArgs e)
        {
            btn_27_Plus.Enabled = false;
            // можем отнимать напряжение 
            btn_27_Minus.Enabled = arPower_27[0] = true;
            if (!arPower_27[1]) // достигли max  
                return;
            if (rbtn_Minus.Checked)
                power27_Min += 0.1;
            if (rbtn_Norma.Checked)
                power27_Norma += 0.1;
            if (rbtn_Plus.Checked)
                power27_Max += 0.1;
            double ddd = rbtn_Minus.Checked
                           ? power27_Min
                           : rbtn_Norma.Checked ? power27_Norma : power27_Max;
            // выставили max на БП 
            arPower_27[1] = ddd >= 30.5 ? false : true;
            try
            {
                //ControlSignal_27V((float)ddd);
                Power27V.Instance.SetVoltage(ddd); //_lambda27V.SetVoltage(ddd)
            }
            catch
            {
            }
            lbl_Edit_27V.Text = arPower_27[1] ? "" : "Максимальное значение";
            btn_27_Plus.Enabled = arPower_27[1];
        }

        // -0,1В для 115В
        private void btn_115_Minus_Click(object sender, EventArgs e)
        {
            btn_115_Minus.Enabled = false;
            // можем прибавлять напряжение 
            btn_115_Plus.Enabled = arPower_115[1] = true;
            if (!arPower_115[0]) // достигли min 
                return;
            if (rbtn_Minus.Checked)
                power115_Min -= 0.25;
            if (rbtn_Norma.Checked)
                power115_Norma -= 0.25;
            if (rbtn_Plus.Checked)
                power115_Max -= 0.25;
            double ddd = rbtn_Minus.Checked
                           ? power115_Min
                           : rbtn_Norma.Checked ? power115_Norma : power115_Max;
            // выставили min на БП 
            arPower_115[0] = ddd <= 110.0 ? false : true;
            Power115V.Instance.SetVoltage(ddd);
            //try
            //{
            //    ControlSignal_115V((float)ddd);                
            //}
            //catch { }
            lbl_Edit_115V.Text = arPower_115[0] ? "" : Resources.MinValue;
            btn_115_Minus.Enabled = arPower_115[0];
        }

        // +0,1В для 115В 
        private void btn_115_Plus_Click(object sender, EventArgs e)
        {
            btn_115_Plus.Enabled = false;
            // можем прибавлять напряжение 
            btn_115_Minus.Enabled = arPower_115[0] = true;
            if (!arPower_115[1]) // достигли max 
                return;
            if (rbtn_Minus.Checked)
                power115_Min += 0.25;
            if (rbtn_Norma.Checked)
                power115_Norma += 0.25;
            if (rbtn_Plus.Checked)
                power115_Max += 0.25;
            double ddd = rbtn_Minus.Checked
                           ? power115_Min
                           : rbtn_Norma.Checked ? power115_Norma : power115_Max;
            // выставили min на БП 
            arPower_115[1] = ddd >= 121.0 ? false : true;
            Power115V.Instance.SetVoltage(ddd);
            //try { ControlSignal_115V((float)ddd); }
            //catch { }
            lbl_Edit_115V.Text = arPower_115[1] ? "" : "Максимальное значение";
            btn_115_Plus.Enabled = arPower_115[1];
        }

        #endregion Подстройка БП

        #endregion Режим питания

        #region Проведение измерений-click

        // Источники питания 27,5В и 115В 
        //UsbSimple Plus_27B;
        //Usb_I_115B Read_I_Card;

        private const string PS115V_Read_U = "115V_Read_U_LCard";
        private const string PS115V_Read_I = "115V_Read_I_LCard";

        private void btn_FirstPitan_Click(object sender, EventArgs e)
        {
            if (btn_FirstPitan.FlatStyle == FlatStyle.Standard)
            {
                PressButton(btn_FirstPitan);
                ImageDesigner(ModeMeasurementsDiss15.FirstPitanie);
                InitializFirstPitan();
            }
            else
            {
                //сохраняем результат и останавливаем измерения
                if (saveResults)
                {

                    double power115U;
                    double power115I;
                    if (Power115V.Instance.IsAktor)
                    {
                        power115U = _measWorker.GetValue(PS115V_Read_U);
                        power115I = _measWorker.GetValue(PS115V_Read_I);
                    }
                    else
                    {
                        power115U = GetIndicatorValue(4);
                        power115I = GetIndicatorValue(5);
                    }
                    DataManager.Instance.SaveParameter(testId, PS115V_Read_U, power115U);
                    DataManager.Instance.SaveParameter(testId, PS115V_Read_I, power115I);
                }
                _measWorker.StopMeasuring(saveResults); //напряжение 27В                
                if (btn_On8.Visible) // Откл.Блок 8 
                {
                    if (btn_On8.FlatStyle == FlatStyle.Flat)
                        btn_On8.PerformClick();
                    if(!_isCurMeas)
                        MessageBox.Show("Ток потребления по цепи 27В не измерен.");
                    btn_On8.Visible = false;
                }
                WrungButton(btn_FirstPitan);
                ImageDesigner(ModeMeasurementsDiss15.Stop);
            }
        }

        private bool _isCurMeas = false;

        // Вкл Блока 8 
        private void btn_On8_Click(object sender, EventArgs e)
        {
            if (btn_On8.FlatStyle == FlatStyle.Standard)
            {
                btn_On8.FlatStyle = FlatStyle.Flat;
                btn_On8.BackColor = Globals.SuccessColor;
                btn_On8.Text = Resources.Block8Off;
                DiscreteOutputs.Instance().SetOutput("Block_8", true);
            }
            else
            {
                btn_On8.FlatStyle = FlatStyle.Standard;
                btn_On8.BackColor = Color.Transparent;
                btn_On8.Text = Resources.Block8On;
                DiscreteOutputs.Instance().SetOutput("Block_8", false);
                if (saveResults)
                {
                    _strip.Save(); //сохраняем ток 27В
                    _isCurMeas = true;
                }
                                   
            }
        }

        private double CalculateDelta27Percent(double signal1, double delta)
        {
            if (Math.Abs(signal1 - 0) > double.Epsilon)
                return Math.Abs(delta / 1000 / signal1 * 100 * _specSignalKoef);
            return 100; //возвращаем условно 100% если опорного сигнала нет, иначе деление на 0 это бесконечность
        }

        // Вторичные источники питания 
        private void btn_SecPitan_Click(object sender, EventArgs e)
        {
            if (btn_SecPitan.FlatStyle == FlatStyle.Standard)
            {
                PressButton(btn_SecPitan);
                ImageDesigner(ModeMeasurementsDiss15.SecPitanie);
                InitializSecPitan();
            }
            else
            {
                DataManager.Instance
                           .SaveParameter(testId, specSignal[0],
                                          CalculateDelta27Percent(_measWorker.GetValue(specSignal[1]),
                                                           _measWorker.GetValue(specSignal[2])));
                //сохраняем вручную т.к. дельта 27 теперь вычисляется
                _measWorker.StopMeasuring(saveResults);
                WrungButton(btn_SecPitan);
                ImageDesigner(ModeMeasurementsDiss15.Stop);
            }
        }

        // Магнетрон и ТКМ 
        private void btn_TKM_Click(object sender, EventArgs e)
        {
            if (btn_TKM.FlatStyle == FlatStyle.Standard)
            {
                PressButton(btn_TKM);
                InitializTKM();
                ImageDesigner(ModeMeasurementsDiss15.TKM);
            }
            else
            {
                _measWorker.StopMeasuring(saveResults);
                WrungButton(btn_TKM);
                ImageDesigner(ModeMeasurementsDiss15.Stop);
            }
        }

        #region Излучение +

        // Эквивалент 
        private void rbtn_Equivalent_Click(object sender, EventArgs e)
        {
            if (rbtn_Equivalent.FlatStyle == FlatStyle.Standard)
            {
                bool bl = false;
                try
                {
                    DiscreteOutputs.Instance().SetOutput("Equivalent", true);
                }
                catch
                {
                    bl = true;
                    MessageBox.Show(Resources.ErrorDoDriveCommand, Resources.ErrorDriveCommand);
                }
                if (bl)
                    return;
                rbtn_Equivalent.FlatStyle = FlatStyle.Flat;
                rbtn_Antenna.FlatStyle = FlatStyle.Standard;
            }
        }

        // Антенна 
        private void rbtn_Antenna_Click(object sender, EventArgs e)
        {
            if (rbtn_Antenna.FlatStyle == FlatStyle.Standard)
            {
                bool bl = false;
                //try { //ControlSignal("Antenna"); }
                try
                {
                    DiscreteOutputs.Instance().SetOutput("Equivalent", false);
                }
                catch
                {
                    bl = true;
                    MessageBox.Show(Resources.ErrorDoDriveCommand, Resources.ErrorDriveCommand);
                }
                if (bl)
                    return;
                rbtn_Antenna.FlatStyle = FlatStyle.Flat;
                rbtn_Equivalent.FlatStyle = FlatStyle.Standard;
            }
        }

        #endregion

        // Ток К 
        //Usb_K15 K_1, K_2, K_3;
        private const string I_K_1 = "I_K_1";
        private const string I_K_2 = "I_K_2";
        private const string I_K_3 = "I_K_3";
        private const int indK1Num = 8;
        private const int indK2Num = 11;
        private const int indK3Num = 14;

        private void btn_Tok_K_Click(object sender, EventArgs e)
        {
            if (btn_Tok_K.FlatStyle == FlatStyle.Standard)
            {
                bool bb = false;
                //try { ControlSignal("UNCh_On_15"); }
                if (!DiscreteOutputs.Instance().SetOutput("UNCh_15", true))
                {
                    bb = true;
                    MessageBox.Show(Resources.ErrorCommand, Resources.ErrorCaption);
                }
                if (bb)
                    return;
                //is_I_K15 = true;
                //AddingTok_K();
                PressButton(btn_Tok_K);
                InitializTok_K();
                ImageDesigner(ModeMeasurementsDiss15.Tok_K);
            }
            else
            {
                if (saveResults)
                {
                    DataManager.Instance.SaveParameter(testId, I_K_1, GetIndicatorValue(indK1Num));
                    DataManager.Instance.SaveParameter(testId, I_K_2, GetIndicatorValue(indK2Num));
                    DataManager.Instance.SaveParameter(testId, I_K_3, GetIndicatorValue(indK3Num));
                }
                _measWorker.StopMeasuring(saveResults);
                ImageDesigner(ModeMeasurementsDiss15.Stop);
                DiscreteOutputs.Instance().SetOutput("UNCh_15", false);
                //is_I_K15 = false;
                WrungButton(btn_Tok_K); // отключаем измерения 
            }
        }

        private void btn_UNCh_Click(object sender, EventArgs e)
        {
            if (btn_UNCh.FlatStyle == FlatStyle.Standard)
            {
                btn_UNCh.Enabled = false;
                if (!SetTask(true, "Task_1_15"))
                    return;
                //AddingUNCh();
                PressButton(btn_UNCh);
                InitializUNCh();
                ImageDesigner(ModeMeasurementsDiss15.UNCh);
            }
            else
            {
                _measWorker.StopMeasuring(saveResults);
                //DeletingAll();
                SetTask(false, String.Empty);
                WrungButton(btn_UNCh);
                ImageDesigner(ModeMeasurementsDiss15.Stop);

                try
                {
                    DiscreteOutputs.Instance().SetOutput("RRU", false);
                    //ControlSignal("RRU_Off"); 
                }
                catch
                {
                }
            }
            btn_UNCh.Enabled = true;
        }

        private void btn_Noise_Click(object sender, EventArgs e)
        {
            if (btn_Noise.FlatStyle == FlatStyle.Standard)
            {
                bool bb = false;
                try
                {
                    DiscreteOutputs.Instance().SetOutput("Band_2", false);
                    DiscreteOutputs.Instance().SetOutput("RRU", false);
                }
                catch (Exception ex)
                {
                    bb = true;
                    MessageBox.Show(Resources.ErrorCommand + " " + ex.Message, Resources.ErrorCaption);
                }
                if (bb)
                    return;
                if (!SetTask(true, "Task_W_1_15")) // вставил 18.04.2012 
                    return;
                MessageBox.Show(String.Format(Resources.AfterStabilizeBlockN, 6) + " " +
                                String.Format(Res.ZeroModeMessage, Res.ZeroOn));
                SetTask(false, String.Empty);
                PressButton(btn_Noise);
                InitializNoise();
                ImageDesigner(ModeMeasurementsDiss15.Noise);
                rbtn_I.PerformClick();
            }
            else
            {
                _measWorker.StopMeasuring(saveResults);
                //DeletingAll();
                WrungButton(btn_Noise);
                ReturnBackConfigureIndicators();
                ImageDesigner(ModeMeasurementsDiss15.Stop);
                // вставил 18.04.2012
                MessageBox.Show(String.Format(Res.ZeroModeMessage, Res.ZeroOff));
                try
                {
                    DiscreteOutputs.Instance().SetOutput("Band_2", false);
                }
                catch
                {
                }
                try
                {
                    DiscreteOutputs.Instance().SetOutput("RRU", false);
                }
                catch
                {
                }
            }
        }

        #region Номер полосы для шума +

        // Полоса 1 
        private void rbtn_I_Click(object sender, EventArgs e)
        {
            if (rbtn_I.FlatStyle == FlatStyle.Standard)
            {
                ReturnBackConfigureIndicators();
                rbtn_I.FlatStyle = FlatStyle.Flat;
                rbtn_II.FlatStyle = FlatStyle.Standard;
                bool bb = false;
                _measWorker.StopMeasuring(saveResults);
                try
                {
                    DiscreteOutputs.Instance().SetOutput("Band_2", false);
                    //ControlSignal("Band_1"); 
                }
                catch (Exception ex)
                {
                    bb = true;
                    MessageBox.Show(String.Format(Resources.ErrorCommand, "\"Band 1\"") +
                                    ex.Message, Resources.Error);
                }
                if (bb)
                    return;
                InitializNoise();
            }
        }

        private void ReturnBackConfigureIndicators()
        {
            List<SegmentIndication> siList = new List<SegmentIndication>();
            for (int i = 0; i < arInd.Length; i++)
                if (arInd[i].Visible)
                    siList.Add(arInd[i]);

            for (int i = 0; i < siList.Count; i++)
            {
                siList[i].CalculationMethod = CalcMethod.MinMax;
            }
        }

        // Полоса 2 
        private void rbtn_II_Click(object sender, EventArgs e)
        {
            if (rbtn_II.FlatStyle == FlatStyle.Standard)
            {
                SaveBandAndConfigureIndicators();
                bool bb = false;
                _measWorker.StopMeasuring(saveResults);
                rbtn_II.FlatStyle = FlatStyle.Flat;
                rbtn_I.FlatStyle = FlatStyle.Standard;
                try
                {
                    //ControlSignal("Band_2"); 
                    DiscreteOutputs.Instance().SetOutput("Band_2", true);
                }
                catch (Exception ex)
                {
                    bb = true;
                    MessageBox.Show(String.Format(Resources.ErrorCommand, "\"Band 2\"") +
                                      ex.Message, Resources.Error);
                }
                if (bb)
                    return;
                //Thread.Sleep(500);
                InitializNoiseII();
            }
        }

        private void SaveBandAndConfigureIndicators()
        {
            List<SegmentIndication> siList = new List<SegmentIndication>();
            for(int i=0;i<arInd.Length;i++)
                if (arInd[i].Visible)
                    siList.Add(arInd[i]);

            for (int i = 0; i < siList.Count; i++)
            {
                var curValue = siList[i].Value;
                siList[i].CheckValueFunction = value => value > curValue;
                siList[i].CalculationMethod = CalcMethod.MinMaxAndDelegate;
            }
        }

    #endregion

    #endregion Проведение измерений

    #region Контрольные задачи-click
        
        // КЗ-1 (ВПЕРЕД-ВВЕРХ)        
        private void btn_Task_1_Click(object sender, EventArgs e)
        {
            btn_Task_1.Enabled = false;  
            bool isToSet = (btn_Task_1.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
                DisableOtherTaskTabButtons( btn_Task_1 );
                ImageTask( ModeTaskDiss15.Task1 );
                InitializTask_1();
                indValue1.Focus();
                if (!SetTask( true, "Task_W_1_15" ))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }

            if (!isToSet)
            {
                tmrMemory.Stop();
                _measWorker.StopMeasuring( saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordDouble("Mnl_Forward_15", indValue1.Value);
                ManualRecordDouble("Mnl_Up_15", indValue2.Value);
                SetTask( false, String.Empty );

                EnableTaskTabButtons( btn_Task_1 );
            }
            btn_Task_1.Enabled = true;
        }

        // КЗ-2 (НАЗАД-ВПРАВО)        
        private void btn_Task_2_Click(object sender, EventArgs e)
        {
            btn_Task_2.Enabled = false;
            bool isToSet = (btn_Task_2.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
                DisableOtherTaskTabButtons( btn_Task_2 );
                ImageTask( ModeTaskDiss15.Task2 );
                InitializTask_2();
                indValue1.Focus();
                if (!SetTask(true, "Task_W_2_15"))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }
            if (!isToSet)
            {
                ((Corners)_activeControl).Dectivate();
                _measWorker.StopMeasuring(saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordDouble("Mnl_Back_15", indValue1.Value);
                ManualRecordDouble("Mnl_Right_15", indValue2.Value);
                SetTask( false, String.Empty );
                EnableTaskTabButtons( btn_Task_2 );
            }
            btn_Task_2.Enabled = true;
        }

        // КЗ-3(ВНИЗ)        
        private void btn_Task_3_Click(object sender, EventArgs e)
        {
            btn_Task_3.Enabled = false;
            bool isToSet = (btn_Task_3.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
                DisableOtherTaskTabButtons( btn_Task_3 );
                ImageTask( ModeTaskDiss15.Task3 );
                InitalizTask_3();
                indValue1.Focus();
                if (!SetTask(true, "Task_W_3_15"))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }
            if (!isToSet)
            {
                _measWorker.StopMeasuring(saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordDouble("Mnl_Down_15", indValue1.Value);
                SetTask( false, String.Empty );
                EnableTaskTabButtons( btn_Task_3 );
            }
            btn_Task_3.Enabled = true;
        }

        private void InitalizTask_3()
        {
            ClearMeasurementList();
            AddMeasurement("W_Down_15", (int)Globals.IndicatorNumber.Speed1,
                String.Format(Resources.Wyg, Resources.MSec), Resources.Down);
            StartWorker();
        }

        private string alfa_0_15 = "Alfa_0_15";
        private string spd_task4_15 = "Spd_Task4_15";
        private string Task4String = "Task_W_4_15";

        private void btn_Task_4_Click(object sender, EventArgs e)
        {
            btn_Task_4.Enabled = false;
            bool isToSet = (btn_Task_4.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
              DisableOtherTaskTabButtons( btn_Task_4 );
                //AddingTask_4();
                ImageTask(ModeTaskDiss15.Task4);
                InitializTask_4();
                indValue1.Focus();
                if (!SetTask( true, Task4String ))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }
            if (!isToSet)
            {
                if (!rbtn_4_Zero.Checked)
                    rbtn_4_Zero.PerformClick();
                if (btn_8_Off_4.FlatStyle == FlatStyle.Standard)
                    btn_8_Off_4.PerformClick();
                _measWorker.StopMeasuring(saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordAlfaDouble("Mnl_Alfa_0_15", indValue1.Value);
                ManualRecordDouble("Mnl_Spd_Task4_15", indValue2.Value);
                SetTask( false, String.Empty );
                EnableTaskTabButtons( btn_Task_4 );
            }
            btn_Task_4.Enabled = true;
        }

        // КЗ-5 (УС=+18)        
        //UsbSecond_T15Alfa Alfa_18Plus;
        private void btn_Task_5_Click(object sender, EventArgs e)
        {
            btn_Task_5.Enabled = false;
            bool isToSet = (btn_Task_5.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
                DisableOtherTaskTabButtons( btn_Task_5 );
                //AddingTask_5();
                ImageTask(ModeTaskDiss15.Task5);
                InitializTask_5();
                indValue1.Focus();
                if (!SetTask( true, "Task_W_5_15" ))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }
            if (!isToSet)
            {
                //DeletingAll();
                _measWorker.StopMeasuring(saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordAlfaDouble("Mnl_Alfa_18Plus", indValue1.Value);
                SetTask( false, String.Empty );
                EnableTaskTabButtons( btn_Task_5 );
            }
            btn_Task_5.Enabled = true;
        }

        // КЗ-6 (УС=-18, ПС=215)
        //UsbSecond_T15 Spd_Task6_15;
        public string spd_task6_15
        {
            get { return "Spd_Task6_15"; }
        }
        //public string spd_task6_15 = "Spd_Task6_15";


        //UsbSecond_T15Alfa Alfa_18Minus;
        public string alfa_18minus
        {
            get { return "Alfa_18Minus"; }
        }
        //public string alfa_18minus = "Alfa_18Minus";


        private void btn_Task_6_Click(object sender, EventArgs e)
        {
            btn_Task_6.Enabled = false;
            bool isToSet = (btn_Task_6.FlatStyle == FlatStyle.Standard);
            if (isToSet)
            {
                DisableOtherTaskTabButtons( btn_Task_6 );
                ImageTask(ModeTaskDiss15.Task6);
                InitializTask_6();
                indValue1.Focus();
                if (!SetTask( true, "Task_W_6_15" ))
                {
                    _logger.Error( "Неудача включения SetTask()." );
                    isToSet = false;
                }
            }
            if (!isToSet)
            {
                ((Block8Check)_activeControl).Save();
                _measWorker.StopMeasuring(saveResults);
                ImageTask(ModeTaskDiss15.Stop);
                ManualRecordAlfaDouble("Mnl_Alfa_18Minus", indValue1.Value);
                ManualRecordDouble("Mnl_Spd_Task6_15", indValue2.Value);
                SetTask( false, String.Empty );

                EnableTaskTabButtons( btn_Task_6 );
            }
            btn_Task_6.Enabled = true;
        }

        // БПК 
        private void btn_BPK_Click(object sender, EventArgs e)
        {
            btn_BPK.Enabled = false;
            if (btn_BPK.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(btn_BPK);
                tiBoard.State = true;
                Diss15Functions.SetAngle(0);
                DiscreteOutputs.Instance().SetOutput("Equivalent", false);
                ImageTask(ModeTaskDiss15.BPK);
            }
            else
            {
                ImageTask(ModeTaskDiss15.Stop);
                GenSinCos.Instance().GeneratorOff();
                DiscreteOutputs.Instance().SetOutput("Equivalent", true);
                EnableTaskTabButtons(btn_BPK);
                tiBoard.State = false;
            }
            btn_BPK.Enabled = true;
        }

        // Wxr, Wyr, Wzr при обнулении         
        private void btn_Zero_Click(object sender, EventArgs e)
        {
            btn_Zero.Enabled = false;
            if (btn_Zero.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons( btn_Zero );
                if (!SetTask(true, "Task_2_15")) return;
                ImageTask(ModeTaskDiss15.Zero);
                MessageBox.Show(String.Format(Res.ZeroModeMessage, Res.ZeroOn), Resources.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializZero();
            }
            else
            {
                _measWorker.StopMeasuring(saveResults);
                SetTask( false, String.Empty );
                ImageTask(ModeTaskDiss15.Stop);
                MessageBox.Show(String.Format(Res.ZeroModeMessage, Res.ZeroOff), Resources.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableTaskTabButtons( btn_Zero );
            }
            btn_Zero.Enabled = true;
        }

        public double GetMeasuredValue(string paramName)
        {
            return _measWorker.GetValue(paramName);
        }

        //char _deltaChar = (char)0x0394;
        public char _deltaChar
        {
            get { return (char)0x0394; }
        }

        // ПАМЯТЬ 
        private void btn_Memory_Click(object sender, EventArgs e)
        {
            // КЗ-1 - измерение задержки "ПАМЯТЬ"
            if (btn_Task_1.FlatStyle == FlatStyle.Flat)
            {
                InitializMemoryOn();
                btn_Memory.Enabled = false;

                //DeletingAll();
                _measWorker.StopMeasuring(saveResults);
                ManualRecordDouble("Mnl_Forward_15", indValue1.Value);
                ManualRecordDouble("Mnl_Up_15", indValue2.Value);

                SetTask(false, String.Empty);
                time_On = DateTime.Now;
                tmrMemory.Start();
                return;
            }
            if (btn_Memory.Text == memory1) // 1-й click
            {
                task4_YC0 = _measWorker.GetValue(alfa_0_15);
                task4_PC = _measWorker.GetValue(spd_task4_15);
                ManualRecordDouble("Alfa_0_15", task4_YC0);
                ManualRecordDouble("Spd_Task4_15", task4_PC);
                SetTask(false, String.Empty);

                btn_Memory.Text = memory2; // поменяли название
                rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled =
                                      indValue1.Enabled = indValue2.Enabled = false;
                lbl_Task_1_Up.Text = Resources.AlfaMGrad;
                lbl_Task_2_Up.Text = String.Format(Resources.WmN, Resources.KmH);
                MessageBox.Show(String.Format(Resources.AfterPLightPressButtonN, memory2), Resources.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (btn_Memory.Text == memory2) // 2-й click -> расчет Дельта Альфа и W 
            {
                task4_YC0_dif = _measWorker.GetValue(alfa_0_15) - task4_YC0;
                task4_PC_dif = _measWorker.GetValue(spd_task4_15) - task4_PC;
                _measWorker.StopMeasuring(saveResults);
                //DeletingAll(); // д.остановится измерения 
                //Чтобы выставить параметры на индикаторах
                AddMeasurement("Memory_Alfa_15", (int)Globals.IndicatorNumber.Speed1, _deltaChar + Resources.AlfaGrad, "");
                AddMeasurement("Memory_W_15", (int)Globals.IndicatorNumber.Speed2, _deltaChar + String.Format(Resources.WN, Resources.KmH), "");

                SetIndicatorValue((int)Globals.IndicatorNumber.Speed1, task4_YC0_dif);
                SetIndicatorValue((int)Globals.IndicatorNumber.Speed2, task4_PC_dif);

                //ShowOnIndicator("Memory_Alfa_15", task4_YC0_dif, indN_Speed_1);
                //ShowOnIndicator("Memory_W_15", task4_PC_dif, indN_Speed_2);

                btn_Memory.Text = memory3; // поменяли название
                return;
            }
            if (btn_Memory.Text == memory3) // 3-й click -> Дельта альфа и W в БД 
            {
                ManualRecordDouble("Memory_Alfa_15", task4_YC0_dif);
                ManualRecordDouble("Memory_W_15", task4_PC_dif);

                btn_Memory.Text = memory1; // поменяли название
                lbl_Task_1_Up.Text = Resources.AlfaGrad;
                lbl_Task_2_Up.Text = String.Format(Resources.WN, Resources.KmH);
                rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled = indValue1.Enabled = indValue2.Enabled = true;
                if (!SetTask(true, Task4String))
                    return;
                InitializTask_4();
            }
        }

        // Измерение скоростей отработки 
        private void btn_Speed_Click(object sender, EventArgs e)
        {
            btn_Speed.Enabled = false;
            if (btn_Speed.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons( btn_Speed);
                ImageTask(ModeTaskDiss15.Speed);
            }
            else
            {
                SetTask(false, String.Empty);
                ImageTask(ModeTaskDiss15.Stop);
                EnableTaskTabButtons(btn_Speed);
            }
            btn_Speed.Enabled = true;
        }

        #region Частота и Мощность СВЧ

        private void btn_Freq_Click(object sender, EventArgs e)
        {
            if (btn_Freq.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(btn_Freq);
                ImageTask(ModeTaskDiss15.Freq);
                // LoadKoef();
            }
            else
            {
                ((SVChFreqAndPower)_activeControl).Stop();
                ImageTask(ModeTaskDiss15.Stop);
                EnableTaskTabButtons(btn_Freq);
            }
        }

        #endregion

        #region Введение углов для КЗ-4

        // Обнуление
        private void rbtn_4_Zero_Click(object sender, EventArgs e)
        {
            if (rbtn_4_Zero.FlatStyle == FlatStyle.Standard)
            {
                ManualRecordTask4();

                IncludeRadioButtonCorner(rbtn_4_Zero);

                btn_Memory.Enabled = true;
                lbl_4_Time.Visible = progressBar_4.Visible = false;
                cmbBox_4_Course.SelectedItem = " 0°";
                tiCourse.VisualText = " 0°";
                btn_4_Course.Enabled = cmbBox_4_Course.Enabled = false;
                btn_8_Off_4.PerformClick(); // выключили блок 8
                btn_8_Off_4.Enabled = btn_8_On_4.Enabled = txtBox_4_1.Enabled = txtBox_4_2.Enabled =
                                                                                cmbBox_4_1.Enabled = cmbBox_4_2.Enabled = false;
                indValue1.Enabled = indValue2.Enabled = true;
                indValue1.Focus();
                try
                {
                    Diss15Functions.SetAngle(0);
                    DiscreteOutputs.Instance().SetOutput("Course", true);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString);
                }
            }
        }

        // Курс
        private void rbtn_4_Course_Click(object sender, EventArgs e)
        {
            if (rbtn_4_Course.FlatStyle == FlatStyle.Standard)
            {
                IncludeRadioButtonCorner(rbtn_4_Course);
                DiscreteOutputs.Instance().SetOutput("Course", false);
                btn_Memory.Enabled = false;
                indValue1.Enabled = indValue2.Enabled =
                                    txtBox_4_1.Enabled = txtBox_4_2.Enabled =
                                                         cmbBox_4_1.Enabled = cmbBox_4_2.Enabled = false;
                btn_4_Course.Enabled = cmbBox_4_Course.Enabled = true;
            }
        }

        // Ввести угол
        private void btn_4_Course_Click(object sender, EventArgs e)
        {
            btn_4_Course.Enabled = false;
            if (cmbBox_4_Course.Text == "") return;
            if (cmbBox_4_Course.Text == " 0°") // обнуление 
            {
                rbtn_4_Zero.PerformClick();
                return;
            }

            ManualRecordTask4();
            //if (_curBlock5Type == Block5Type.Block5I)
            //    dissAngleShift = 0;
            int angle = (cmbBox_4_Course.Text == "+45°" ? 45 : 135);
            if (Settings.Default.IsInverseAngeForDISS15CT4)
                angle *= -1;
            Diss15Functions.SetAngle(angle);
            RenewGeneratorVoltageIndication();
            //ControlSignal(s);

            tiCourse.VisualText = cmbBox_4_Course.Text;
            txtBox_4_1.Enabled = txtBox_4_2.Enabled =
                                 cmbBox_4_1.Enabled = cmbBox_4_2.Enabled = false;
            Application.DoEvents();
            TimeSpan waitTs = TimeSpan.FromSeconds(15); //Ожидание 15 секунд чтобы угол успел установиться
            ShowWaitForm(waitTs);
            btn_4_Course.Enabled = btn_8_On_4.Enabled = btn_8_Off_4.Enabled = true;
        }

        private void ShowWaitForm(TimeSpan waitTs)
        {
            frmInformation frmI = new frmInformation(Res.SettingAngle, waitTs);
            frmI.ShowDialog();
        }

        // Включить блок 8
        private void btn_8_On_4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.Block8OrtWarning, Resources.Attention);
            if (btn_8_On_4.FlatStyle == FlatStyle.Standard)
            {
                Diss15Functions.Block8On();

                btn_8_On_4.FlatStyle = FlatStyle.Flat;
                btn_8_On_4.BackColor = Globals.SuccessColor;
                btn_8_Off_4.FlatStyle = FlatStyle.Standard;
                btn_8_Off_4.BackColor = Color.Transparent;
                rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled =
                                      cmbBox_4_Course.Enabled = btn_4_Course.Enabled = false;
                progressBar_4.Visible = true;
                progressBar_4.Value = 0;
                lbl_4_Time.Visible = true;
                lbl_4_Time.Text = String.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, 5, Resources.Minutes, "00", Resources.Seconds);

                arTask4_8_2[0] = arTask4_8_2[1] = false;

                txtBox_4_1.Text = txtBox_4_2.Text = "";
                cmbBox_4_1.Items.Clear();
                cmbBox_4_1.Items.AddRange(new[] { Resources.Forward, Resources.Back });
                cmbBox_4_2.Items.Clear();
                cmbBox_4_2.Items.AddRange(new[] { Resources.Left, Resources.Right });

                countBlock8 = 300; // по таймеру д. отработать 5 мин.
                tmrBlock8.Start(); // запустили таймер
            }
        }

        // Выключить блок 8
        private void btn_8_Off_4_Click(object sender, EventArgs e)
        {
            if (btn_8_Off_4.FlatStyle == FlatStyle.Standard)
            {
                tmrBlock8.Stop();

                btn_8_Off_4.FlatStyle = FlatStyle.Flat;
                btn_8_Off_4.BackColor = Globals.SuccessColor;
                btn_8_On_4.FlatStyle = FlatStyle.Standard;
                btn_8_On_4.BackColor = Color.Transparent;
                progressBar_4.Visible = lbl_4_Time.Visible = false;

                Diss15Functions.Block8Off();

                if (countBlock8 == 0) // таймер отработал 5 мин
                {
                    cmbBox_4_1.Enabled = cmbBox_4_2.Enabled = true;
                    btn_8_On_4.Enabled = btn_8_Off_4.Enabled = false;
                    MessageBox.Show(Resources.FixCounters, Resources.ApplicationName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // выключили Блок 8 раньше 5 мин
                {
                    rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled = cmbBox_4_Course.Enabled =
                                                                  btn_4_Course.Enabled = true;
                    btn_8_On_4.Enabled = btn_8_Off_4.Enabled = false;
                }
            }
        }

        #endregion Введение углов для КЗ-4

        #endregion Контрольные задачи

        #endregion Обработка click-ов по всей форме

        private enum TestTypeId_Diss15
        {
            Task1_ForwardSpeed = 128,
            Task2_BackwardSpeed = 129,
            Task3_DownSpeed = 131,
            Task4_Angle = 135,
            Task5_Angle = 176,
            Task6_Angle = 175,
            Task6_Speed = 134,
            Task4_Speed = 133,
            Task1_UpSpeed = 130,
            Task2_RightSpeed = 132
        }

        #region Контрольная задача 4

        // Путь 
        private void txtBox_4_1_Validating(object sender, CancelEventArgs e)
        {
            if (IsOutputTask4() && txtBox_4_1.Text != "")
            {
                double d = 0.0;
                try
                {
                    d = double.Parse(txtBox_4_1.Text);
                }
                catch
                {
                    ErrorInputTextBox(txtBox_4_1);
                    return;
                }
                if (!(0 <= d && d <= 999))
                {
                    ErrorCorridorTextBox(txtBox_4_1, 0, 999, "\"ПУТЬ\"");
                    return;
                }
                // красный/зеленый коридор
                txtBox_4_1.ForeColor = 7 <= d && d <= 9 &&
                                       (cmbBox_4_1.SelectedIndex == 0 &&
                                        cmbBox_4_Course.SelectedIndex == 1 ||
                                        cmbBox_4_1.SelectedIndex == 1 &&
                                        cmbBox_4_Course.SelectedIndex == 2)
                                         ? Color.LimeGreen
                                         : Color.Red;
                arTask4_8_2[0] = true;

                if (arTask4_8_2[0] && arTask4_8_2[1]) // разблокируем "Обнуление" ...
                {
                    rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled = cmbBox_4_Course.Enabled =
                                                                  btn_4_Course.Enabled = true;
                }
            }
        }

        private void txtBox_4_1_TextChanged(object sender, EventArgs e)
        {
            txtBox_4_1.ForeColor = SystemColors.WindowText;
            txtBox_4_1.Text = txtBox_4_1.Text.Trim();
        }

        private void cmbBox_4_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBox_4_1.Enabled = true;
            txtBox_4_1.Text = "";
            UnlockBlock8Controls();
        }

        // Боковое уклонение 
        private void txtBox_4_2_Validating(object sender, CancelEventArgs e)
        {
            if (IsOutputTask4() && txtBox_4_2.Text != "")
            {
                double d = UserControlsKPK2.Functions.MoveToDouble(txtBox_4_2.Text);
                if (double.IsNaN(d))
                {
                    ErrorInputTextBox(txtBox_4_2);
                    return;
                }
                if (!(0 <= d && d <= 999))
                {
                    ErrorCorridorTextBox(txtBox_4_2, 0, 999, "\"Боковое уклонение\"");
                    return;
                }
                // красный/зеленый коридор
                txtBox_4_2.ForeColor = 7 <= d && d <= 9 &&
                                       (cmbBox_4_2.SelectedIndex == 0 &&
                                        cmbBox_4_Course.SelectedIndex == 1 ||
                                        cmbBox_4_2.SelectedIndex == 1 &&
                                        cmbBox_4_Course.SelectedIndex == 2)
                                         ? Color.LimeGreen
                                         : Color.Red;
                arTask4_8_2[1] = true;

                if (arTask4_8_2[0] && arTask4_8_2[1]) // разблокируем "Обнуление" ...
                    UnlockBlock8Controls();
            }
        }

        private void UnlockBlock8Controls()
        {
            rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled = cmbBox_4_Course.Enabled =
                                                              btn_4_Course.Enabled = true;
        }

        private void txtBox_4_2_TextChanged(object sender, EventArgs e)
        {
            txtBox_4_2.ForeColor = SystemColors.WindowText;
            txtBox_4_2.Text = txtBox_4_2.Text.Trim();
            UnlockBlock8Controls();
        }

        private void cmbBox_4_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBox_4_2.Enabled = true;
            txtBox_4_2.Text = "";
        }

        #endregion Контрольная задача 4

        #region Вспомогательные функции

        Timer tWorkTimer;
        private DateTime _startDissTime;

        /// <summary>
        /// Включаем ДИСС 
        /// </summary>
        private void StartDiss()
        {
            bool bl = false;
            try
            {
                errorPS27VAppear = false;
                Power115V.Instance.SetPhase(90);
                if (!Power115V.Instance.SetVoltageGradually(Voltage115))
                    throw new Exception("Ошибка установки напряжения 115В");
                Power115V.Instance.SetOnOff(true);
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("ИП 115В включен!"));
                Power27V.Instance.SetVoltage(28.5); //_lambda27V.SetVoltage(28.5)
                Power27V.Instance.SetOnOff(true); //_lambda27V.OnOffState(true)

                _logger.Info(string.Format("ИП постоянного тока 27В включен!"));
                TimerHVStart(true);

                MeasureStrip(true);
                Application.DoEvents();

                DiscreteOutputs.Instance().SetOutput("High_15", false);
                DiscreteOutputs.Instance().SetOutput("Equivalent", true);
                DiscreteOutputs.Instance().SetOutput("Block_8", false);
                DiscreteOutputs.Instance().SetOutput("RRU", false);
                RunWorkTimer();

            }
            catch
            {
                bl = true;
                tabControl.Enabled = grpBox_Mode.Enabled = gbPowerMode.Enabled = false;
                rbtn_On.Checked = false;
                rbtn_On.FlatStyle = FlatStyle.Standard;
                MessageBox.Show(Resources.CommandError + Environment.NewLine + Resources.ApplicationMustBeClosed,
                                Resources.ErrorCaption);
            }
            if (bl)
                return;
            

            // включаем "Высокое" 
            if (arBH[0] && arBH[1])
                btn_High.Enabled = true; // пока не отработает время вкл.ВН 
            btn_High.Text = Resources.HVOff;
            btn_High.FlatStyle = FlatStyle.Flat;
            btn_High.BackColor = Globals.SuccessColor;
            // разблокировать "Режим питания"
            gbPowerMode.Enabled = true;
            tabControl.Enabled = true; // разблокировали вкладки    
        }

        private void RunWorkTimer()
        {
            _startDissTime = DateTime.Now;
            tWorkTimer = new Timer();
            tWorkTimer.Interval = 1000;
            tWorkTimer.Tick += TWorkTimerOnTick;
            tWorkTimer.Start();
        }

        private void TWorkTimerOnTick(object sender, EventArgs eventArgs)
        {
            TimeSpan ts = DateTime.Now - _startDissTime;
            siWorkTime.ValueString = GetTimeString(ts);
        }

        private string GetTimeString(TimeSpan ts)
        {
            return String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void MeasureStrip(bool state)
        {
            if (state)
            {
                if (threadStrip != null)
                    if (threadStrip.IsAlive)
                        return;
                threadStrip = new Thread(MeasureStrip);
                _mreMeasureStrip.Reset();
                threadStrip.Start();
            }
            else
            {
                _mreMeasureStrip.Set();
                Thread.Sleep(200);
                if (threadStrip != null)
                    if (threadStrip.IsAlive)
                        threadStrip.Abort();
            }
        }

        private int VHTimeLimit = 120; //предел ожидания высокого напряжения в секундах

        private void DiscreteInputsReceived(BitArray ba)
        {
            if (ba == null)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action<BitArray>(DiscreteInputsReceived), ba);
                return;
            }

            Dictionary<int, string> chList = DataManager.Instance.GetDiscreteInputsChannels();
            foreach (KeyValuePair<int, string> rec in chList)
            {
                SetTextIndicatorState(rec.Value, ba[rec.Key]);
            }

            if (tmrHV.Enabled && (tiHV.State || dTime > VHTimeLimit))
            {
                HVEnabled();
                TimerHVStart(false);
                if (dTime >= VHTimeLimit) // истекло время ожидания 
                    ManualRecordDouble("BH_Time_15", VHTimeLimit * -1);
                else
                    ManualRecordDouble("BH_Time_15", dTime);
                arBH[1] = true;
            }
        }

        private void HVEnabled()
        {
            bTraining.Enabled = true;
        }

        private void SetTextIndicatorState(string chName, bool state)
        {
            TextIndicator current;
            switch (chName)
            {
                case "Memory_1":
                    current = tiMemory;
                    break;
                case "Izdelie":
                    current = tiWork;
                    break;
                case "HV":
                    current = tiHV;
                    break;
                case "Refuse":
                    current = tiRefuse;
                    break;
                case "Capture_1":
                    current = tiMapCorner;
                    break;
                case "Capture_2":
                    current = tiWorkability;
                    break;
                case "Capture_3":
                    current = tiNavigation;
                    break;
                default:
                    return;
            }
            current.State = state;
        }

        /// <summary>
        /// Выключаем ДИСС 
        /// </summary>
        private void StopDiss()
        {
            tWorkTimer.Stop();
            siWorkTime.Inactive = true;
            _measWorker.StopMeasuring(false);
            _measWorker.StartMeasuring();
            rbtn_Norma.PerformClick(); // чтобы кнопка была на 27,5В 
                                       // заблокировать "Режим питания"
            gbPowerMode.Enabled = false;
            try
            {
                //_lambda27V.OnOffState(false);
                Power27V.Instance.SetOnOff(false);
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
            } // выключение нового БП 27В
            catch { }
            try
            {
                Power115V.Instance.SetOnOff(false);
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("ИП 115В выключен!"));
            } // выключили БП 115В 
            catch { }

            if (btn_High.FlatStyle == FlatStyle.Flat) // выключаем "Высокое" 
                btn_High.PerformClick();
            tmrHV.Stop(); // время вкл.ВН 
            btn_High.Enabled = false;
            // Выключить все кнопки на активной вкладке
            if (countButton > 0) // есть нажатые кнопки
            {
                if (tabControl.SelectedTab == pageWork)
                    for (int i = 0; i < arButton.Length; i++)
                        if (arButton[i].FlatStyle == FlatStyle.Flat)
                            arButton[i].PerformClick();
                if (tabControl.SelectedTab == pageTask || tabControl.SelectedTab == tbLink)
                    for (int i = 0; i < listButtonTask.Count; i++)
                        if (listButtonTask[i].FlatStyle == FlatStyle.Flat)
                            listButtonTask[i].PerformClick();
            }
            tabControl.SelectTab(pageWork); // переход на первую вкладку
            tabControl.Enabled = false;
            GenSinCos.Instance().GeneratorOff();
            arBH[0] = false;
            arBH[1] = false;
            bTraining.Enabled = false;
        }

        internal static bool SetTask(bool state, string str)
        {
            bool result = Diss15Functions.SetTask(state, str);
            if (!result)
                MessageBox.Show(Resources.ErrorDoCommand + "\n", Resources.Error);
            return result;
        }

        #region Запись в БД значений из TextBox

        /// <summary>
        /// Запись в БД введенных значений в TextBox 
        /// </summary>
        /// <param name="str">имя сигнала</param>
        /// <param name="text">TextBox для ввода</param>
        public void ManualRecord(string str, string text)
        {
            bool bb = false; // выход по ошибкам 
            double value = 0.0; // что ввели 
            value = Functions.MoveToDouble(text);
            if (double.IsNaN(value))
                bb = true;
            if (bb) return;
            ManualRecordDouble(str, value);
        }

        /// <summary>
        /// Запись в БД введенных значений УГЛА АЛЬФА в TextBox 
        /// </summary>
        /// <param name="paramName">имя сигнала</param>
        /// <param name="text">TextBox для ввода</param>
        private void ManualRecordAlfa(string paramName, string text)
        {
            bool bb = false; // выход по ошибкам 
            double value = 0.0; // что ввели 
            value = Functions.MoveToDouble(text);
            if (double.IsNaN(value))
                bb = true;
            if (bb) return;
            value = (int)value + (value - (int)value) * 0.6; // град и доли в град и мин
            ManualRecordDouble(paramName, value);
        }

        /// <summary>
        /// Запись в БД введенных значений УГЛА АЛЬФА в TextBox 
        /// </summary>
        /// <param name="paramName">имя сигнала</param>
        /// <param name="text">TextBox для ввода</param>
        private void ManualRecordAlfaDouble(string paramName, double value)
        {
            //доли градуса оставляем долями, а при выводе отчёта конвертируем только для вывода
            ManualRecordDouble(paramName, value);
        }

        /// <summary>
        /// Запись в БД числа(double) по имени сигнала 
        /// </summary>
        /// <param name="paramName">имя сигнала</param>
        /// <param name="value">число</param>
       public void ManualRecordDouble(string paramName, double value)
        {
            if (!double.IsNaN(value))
                DataManager.Instance.SaveParameter(testId, paramName, value);
        }

        /// <summary>
        /// Запись в БД числа(double) по имени сигнала 
        /// </summary>
        /// <param name="paramName">имя сигнала</param>
        /// <param name="value">число</param>
        internal void ManualRecordBool(string paramName, bool value)
        {
            DataManager.Instance.SaveParameter(testId, paramName, value ? 1 : 0);
        }

        /// <summary>
        /// Запись в БД 0/1 по выбору RadioButton; Null - выбора не было 
        /// </summary>
        /// <param name="str">имя сигнала</param>
        /// <param name="rbtn_1">RadioButton - "ДА"</param>
        /// <param name="rbtn_2">RadioButton - "НЕТ"</param>
        public void ManualRecordBool(string str, RadioButton rbtn_1, RadioButton rbtn_2)
        {
            if (rbtn_1.Checked) ManualRecordDouble(str, 1);
            if (rbtn_2.Checked) ManualRecordDouble(str, 0);
        }

        /// <summary>
        /// Запись в БД значений введенных в при проверки Блока 8 КЗ-4 
        /// </summary>
        private void ManualRecordTask4()
        {
            // нет новых введенных значений 
            if (txtBox_4_1.Enabled == false && txtBox_4_2.Enabled == false ||
                tiCourse.VisualText == " 0°") return;
            double dd = 0.0; // что ввели 
            bool bl = false;
            string s; // имя сигнала 
            if (txtBox_4_1.Enabled) // путь "Вперед", "Назад"
            {
                dd = Functions.MoveToDouble(txtBox_4_1.Text);
                if (double.IsNaN(dd))
                {
                    bl = true;
                    dd = 0;
                }
                if (!bl) // ошибка -> уходим 
                {
                    s = tiCourse.VisualText == "+45°" ? "Block8_Forward" : "Block8_Back";
                    dd = tiCourse.VisualText == "+45°" &&
                         cmbBox_4_1.SelectedItem.ToString() == Resources.Forward ||
                         tiCourse.VisualText == "+135°" &&
                         cmbBox_4_1.SelectedItem.ToString() == Resources.Back
                           ? dd
                           : -dd;
                    ManualRecordDouble(s, dd);
                }
            }
            bl = false;
            if (txtBox_4_2.Enabled) // уклонение "Влево", "Вправо"
            {
                dd = Functions.MoveToDouble(txtBox_4_2.Text);
                if (double.IsNaN(dd))
                {
                    bl = true;
                    dd = 0;
                }
                if (!bl) // ошибка -> уходим 
                {
                    s = tiCourse.VisualText == "+45°" ? "Block8_Left" : "Block8_Right";
                    dd = tiCourse.VisualText == "+45°" &&
                         cmbBox_4_2.SelectedItem.ToString() == Resources.Left ||
                         tiCourse.VisualText == "+135°" &&
                         cmbBox_4_2.SelectedItem.ToString() == Resources.Right
                           ? dd
                           : -dd;
                    ManualRecordDouble(s, dd);
                }
            }
        }

        #endregion

        #region Click по кнопкам

        /// <summary>
        /// Блокируем все кнопки на TabPage "Проведение измерений",
        /// запрещаем переход на другие вкладки
        /// </summary>
        /// <param name="btn">кнопка, которую нажали</param>
        private void PressButton(Button btn)
        {
            countButton = 1;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Globals.SuccessColor;
            Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                collButton[i].Enabled = collButton[i].GetType() != typeof(Button) || collButton[i].Name == btn.Name || collButton[i] == btn_On8;
        }

        /// <summary>
        /// Блокируем все кнопки на TabPage "Контрольные задачи",
        /// запрещаем переход на другие вкладки.
        /// </summary>
        /// <param name="pressedBtn"> Кнопка, которую нажали (включили). </param>
        private void DisableOtherTaskTabButtons( Button pressedBtn)
        {
            countButton = 1;
            pressedBtn.FlatStyle = FlatStyle.Flat;
            pressedBtn.BackColor = Globals.SuccessColor;
            for (int i = 0; i < listButtonTask.Count; i++)
                if (listButtonTask[i].Name != pressedBtn.Name)
                    listButtonTask[i].Enabled = false;
        }

        /// <summary>
        /// Разблокируем все кнопки TabPage "Проведение измерений",
        /// разрешаем переход на другие вкладки
        /// </summary>
        /// <param name="btn">кнопка, которую отжали</param>
        private void WrungButton( Button btn)
        {
            countButton = 0;
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackColor = Color.Transparent;
            Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button))
                {
                    // Разблокируем btn_Tok_K только при наличии ВН - иначе нет смысла измерять 
                    if (collButton[i] == btn_Tok_K && !forTok_K) continue;
                    collButton[i].Enabled = true;
                }
            if (btn == btn_TKM) // убираем Излучение и Частоту... 
            {
                rbtn_Equivalent.PerformClick();
                grpBox_Radiat.Visible = false;
            }
            if (btn == btn_Noise)
                grpBox_Band.Visible = false;
            if (btn == btn_FirstPitan) // убрали регулировку новых БП на 27В и 115В 
                grpBox_27V.Visible = grpBox_115V.Visible = false;
        }

        /// <summary>
        /// Разблокируем все кнопки TabPage "Контрольные задачи",
        /// разрешаем переход на другие вкладки
        /// </summary>
        /// <param name="btn"> Кнопка, которую отжали. </param>
        private void EnableTaskTabButtons( Button btn)
        {
            countButton = 0;
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackColor = Color.Transparent;
            for (int i = 0; i < listButtonTask.Count; i++)
                listButtonTask[i].Enabled = true;

            //if (btn == btn_Task_2 && !rbtn_Zero.Checked)
            //    rbtn_Zero.PerformClick();
            if (btn == btn_Task_4)
            {
                if (btn_8_Off_4.FlatStyle == FlatStyle.Standard)
                    btn_8_Off_4.PerformClick();
            }
            if (btn == btn_Task_6)
            {
                if (((Block8Check)_activeControl).b8.btn_8_Off_6.FlatStyle == FlatStyle.Standard)
                    ((Block8Check)_activeControl).b8.btn_8_Off_6.PerformClick();
            }
        }

        #endregion

        /// <summary>
        /// Переключение RadioButton в groupBox на
        /// вкладке "Контрольные задачи" 
        /// </summary>
        /// <param name="rbtn">нажали RadioButton</param>
        internal static void IncludeRadioButtonCorner(RadioButton rbtn)
        {
            rbtn.FlatStyle = FlatStyle.Flat;
            rbtn.BackColor = Globals.SuccessColor;
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

        #region Выход из режимов

        /// <summary>
        /// Выходим из режима ввода данных в КЗ 
        /// </summary>
        /// <returns></returns>
        private bool IsOutputTask()
        {
            bool bl = isEscape || frmMain.IsFormClose ||
                      btn_Task_1.Focused || btn_Task_2.Focused || btn_Task_3.Focused || btn_Task_4.Focused ||
                      btn_Task_5.Focused || btn_Task_6.Focused || btn_BPK.Focused || btn_Zero.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused ||
                      (btn_Memory.Text == memory3 && btn_Memory.Focused);
            return !bl;
        }

        /// <summary>
        /// КЗ-4: выход из ввода "Пути" и "Бокового уклонения"
        /// </summary>
        /// <returns></returns>
        private bool IsOutputTask4()
        {
            bool bl = isEscape || frmMain.IsFormClose || btn_Task_4.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
            return !bl;
        }

        /// <summary>
        /// КЗ-6: выход из ввода "Пути" и "Бокового уклонения" 
        /// </summary>
        /// <returns></returns>
        private bool IsOutputTask6()
        {
            bool bl = isEscape || frmMain.IsFormClose || btn_Task_6.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
            return !bl;
        }

        /// <summary>
        /// БПК: выходим из ввода данных в TextBox-ы 
        /// </summary>
        /// <returns></returns>
        private bool IsOutputBPK()
        {
            bool bl = isEscape || frmMain.IsFormClose || btn_Task_1.Focused || btn_Task_2.Focused ||
                      btn_Task_3.Focused || btn_Task_4.Focused || btn_Task_5.Focused ||
                      btn_Task_6.Focused || btn_Zero.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
            return !bl;
        }

        /// <summary>
        /// Магнетрон и ТКМ: выходим из ввода данных в TextBox-ы 
        /// </summary>
        /// <returns></returns>
        private bool IsOutputTKM()
        {
            bool bl = isEscape || frmMain.IsFormClose || btn_TKM.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
            return !bl;
        }

        /// <summary>
        /// "Частота и Мощность СВЧ" - выходим из ввода данных в TextBox-ы 
        /// </summary>
        /// <returns></returns>
        private bool IsOutputFreq()
        {
            bool bl = isEscape || frmMain.IsFormClose || btn_Freq.Focused ||
                      rbtn_Off.Focused || rbtn_On.Focused || btn_High.Focused ||
                      rbtn_Minus.Focused || rbtn_Norma.Focused || rbtn_Plus.Focused;
            return !bl;
        }

        /// <summary>
        /// Ошибка ввода в TextBox - ввели не число(double) 
        /// </summary>
        /// <param name="txtBox">TextBox для ввода</param>
        private void ErrorInputTextBox(TextBox txtBox)
        {
            MessageBox.Show(String.Format(Resources.ErrorInputNValue, txtBox.Text), Resources.KPK2, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            txtBox.Focus();
            txtBox.SelectAll();
        }

        /// <summary>
        /// Ошибка ввода в TextBox - ввели число, которое выходит
        /// за допустимые границы
        /// </summary>
        /// <param name="txtBox">TextBox для ввода</param>
        /// <param name="dMin">минимальное вводимое значение</param>
        /// <param name="dMax">максимальное вводимое значение</param>
        /// <param name="str">название параметра</param>
        private void ErrorCorridorTextBox(TextBox txtBox, double dMin, double dMax, string str)
        {
            MessageBox.Show(String.Format(Resources.ErrorDiapazonValue, str, dMin, dMax), Resources.KPK2, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            txtBox.Focus();
            txtBox.SelectAll();
        }

        #endregion

        #region Настройка индикаторов для вкладки "Проведения измерений"

        /// <summary>
        /// Настройка вида индикаторов в GroupBox
        /// </summary>
        /// <param name="modeMeasurements"></param>
        private void ImageDesigner(ModeMeasurementsDiss15 modeMeasurements)
        {
            try
            {
                pDefaultValues.Visible = modeMeasurements == ModeMeasurementsDiss15.OpenForm;
                switch (modeMeasurements)
                {
                    case ModeMeasurementsDiss15.OpenForm: // открыли форму
                        ImageOpenForm();
                        break;
                    case ModeMeasurementsDiss15.Stop: // остановили измерения
                        ImageStop();
                        break;
                    case ModeMeasurementsDiss15.FirstPitanie: // первичные источники 27В и 115В
                        ImageFirstPitanie();
                        break;
                    case ModeMeasurementsDiss15.SecPitanie: // вторичные источники питания
                        ImageSecPitanie();
                        break;
                    case ModeMeasurementsDiss15.TKM: // магнетрон и ТКМ
                        ImageTKM();
                        break;
                    case ModeMeasurementsDiss15.Tok_K: // Ток К
                        ImageTok_K();
                        break;
                    case ModeMeasurementsDiss15.UNCh: // сигналы УНЧ
                        ImageUNCh();
                        break;
                    case ModeMeasurementsDiss15.Noise: // Шум УНЧ
                        ImageNoise();
                        break;
                    default:
                        ImageOpenForm();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// При открытии формы - все скрываем 
        /// </summary>
        private void ImageOpenForm()
        {
            gbAll.Text = "";
            grpBox_Radiat.Visible = grpBox_Band.Visible = false;
            for (int i = 0; i < arInd.Length; i++)
                arInd[i].Visible = arUpLabel[i].Visible = arDownLabel[i].Visible = false;
        }

        /// <summary>
        /// При остановке измерений - меняем фон индикаторов
        /// </summary>
        private void ImageStop()
        {
            for (int i = 0; i < arInd.Length; i++)
                if (arInd[i].Visible)
                    arInd[i].Inactive = true;
        }

        private void SetIndicatorsVisibility(int buttonNum)
        {
            for (int i = 0; i < arInd.Length; i++)
                arInd[i].Visible = arUpLabel[i].Visible = arDownLabel[i].Visible = arVisible[i, buttonNum] == 1;
        }

        /// <summary>
        /// Настройка вида индикаторов для измерений 27В и 115В
        /// </summary>
        private void ImageFirstPitanie()
        {
            //gbAll.Text = "Параметры источников питания";
            gbAll.Text = Resources.PSParams;
            SetIndicatorsVisibility(0);
            // для вкл.Блока 8
            btn_On8.Visible = true;
            btn_On8.Enabled = rbtn_Plus.Checked;
            // регулировка новых БП на 27В и 115В 
            grpBox_27V.Visible = grpBox_115V.Visible = true;
        }

        /// <summary>
        /// Настройка вида индикаторов для измерений Вторичных источников питания
        /// </summary>
        private void ImageSecPitanie()
        {
            gbAll.Text = Resources.SecondaryPSVoltage + " " + Res.DiffVoltageSecPower27;
            // индикаторы и подписи - видимы/не видимы
            SetIndicatorsVisibility(1);
        }

        /// <summary>
        /// Настройка вида индикаторов для измерений параметров Магнетрона и ТКМ
        /// </summary>
        private void ImageTKM()
        {
            //gbAll.Text = "Высокое напряжение, ток магнетрона и контрольная мощность СВЧ. Напряжение смещения модуляторных диодов";
            gbAll.Text = Resources.TKM;

            grpBox_Radiat.Visible = true;

            SetIndicatorsVisibility(2);
        }

        /// <summary>
        /// Настройка вида индикаторов для индикации Ток К - 
        /// дельта U и абс.дельта U
        /// </summary>
        private void ImageTok_K()
        {
            //gbAll.Text = "Токи кристаллических смесителей. Разброс токов диодов в паре";
            gbAll.Text = Resources.CrystalMixCurrents;
            // индикаторы и подписи - видимы/не видимы
            SetIndicatorsVisibility(3);
            //for (int i = 1; i < arIndNumber.Length; i++)
            //{
            //    arIndNumber[i].BackgroundIndicator = Color.FromArgb(64, 0, 0);
            //    arIndNumber[i].IndicatorColor = Color.Gray; // цвет цифр
            //    arIndNumber[i].Visible = arVisible[i, 3] == 1 ? true : false;
            //    arLabel[i].Visible = arVisible[i, 3] == 1 ? true : false;
            //}
            //// набираем в лоб все надписи и подписи
            //arIndNumber[1].DecimalsNumber = 3; // 1-й индикатор
            //arIndNumber[1].iLText = "I, мА";
            //arIndNumber[1].strText = "0.000";
            //arLabel[1].Text = "ТК 1-1";
            //arIndNumber[2].DecimalsNumber = 3; // 2-й индикатор
            //arIndNumber[2].iLText = "I, мА";
            //arIndNumber[2].strText = "0.000";
            //arLabel[2].Text = "ТК 1-2";
            //arIndNumber[4].DecimalsNumber = 3; // 4-й индикатор
            //arIndNumber[4].iLText = "I, мА";
            //arIndNumber[4].strText = "0.000";
            //arLabel[4].Text = "ТК 2-1";
            //arIndNumber[5].DecimalsNumber = 3; // 5-й индикатор
            //arIndNumber[5].iLText = "I, мА";
            //arIndNumber[5].strText = "0.000";
            //arLabel[5].Text = "ТК 2-2";
            //arIndNumber[7].DecimalsNumber = 3; // 7-й индикатор
            //arIndNumber[7].iLText = "I, мА";
            //arIndNumber[7].strText = "0.000";
            //arLabel[7].Text = "ТК 3-1";
            //arIndNumber[8].DecimalsNumber = 3; // 8-й индикатор
            //arIndNumber[8].iLText = "I, мА";
            //arIndNumber[8].strText = "0.000";
            //arLabel[8].Text = "ТК 3-2";
            //arIndNumber[9].DecimalsNumber = 2; // 1-й индикатор 2-й ряд
            //arIndNumber[9].iLText = "I, мА";
            //arIndNumber[9].strText = "0.00";
            //arLabel[9].Text = ((Char)0x0394).ToString() + "ТК 1 абс";
            //arIndNumber[12].DecimalsNumber = 2; // 4-й индикатор 2-й ряд
            //arIndNumber[12].iLText = "I, мА";
            //arIndNumber[12].strText = "0.00";
            //arLabel[12].Text = ((Char)0x0394).ToString() + "ТК 2 абс";
            //arIndNumber[15].DecimalsNumber = 2; // 7-й индикатор 2-й ряд
            //arIndNumber[15].iLText = "I, мА";
            //arIndNumber[15].strText = "0.00";
            //arLabel[15].Text = ((Char)0x0394).ToString() + "ТК 3 абс";
        }

        /// <summary>
        /// Настройка вида индикаторов для измерений параметров Сигналы УНЧ
        /// </summary>
        private void ImageUNCh()
        {
            gbAll.Text = Resources.UNChVoltage; // "Напряжение сигнала на выходах УНЧ";
                                                // индикаторы и подписи - видимы/не видимы
            SetIndicatorsVisibility(4);
        }

        /// <summary>
        /// Настройка вида индикаторов для измерений параметров Шум УНЧ 
        /// </summary>
        private void ImageNoise()
        {
            gbAll.Text = Resources.UNChNoiseVoltage; // "Напряжение шумов на выходах УНЧ";
            grpBox_Band.Visible = true;
            // индикаторы и подписи - видимы/не видимы
            SetIndicatorsVisibility(5);
        }

        #endregion Настройка индикаторов для вкладки "Проведения измерений"

        #region Настройка индикаторов для вкладки "Контрольные задачи"

        /// <summary>
        /// Настройка вида индикаторов для вкладки Контрольных задач
        /// </summary>
        /// <param name="modeTask"></param>
        private void ImageTask(ModeTaskDiss15 modeTask)
        {
            if (modeTask != ModeTaskDiss15.Stop)
                if (_activeControl != null)
                    _activeControl.Dispose();

            switch (modeTask)
            {
                case ModeTaskDiss15.OpenForm: // открыли вкладку
                    ImageOpenTask();
                    break;
                case ModeTaskDiss15.Stop: // остановили измерения
                    ImageStopTask();
                    break;
                case ModeTaskDiss15.Task1: // КЗ-1
                    ImageTask_1();
                    break;
                case ModeTaskDiss15.Task2: // КЗ-2
                    ImageTask_2();
                    break;
                case ModeTaskDiss15.Task3: // КЗ-3
                    ImageTask_3();
                    break;
                case ModeTaskDiss15.Task4: // КЗ-4
                    ImageTask_4();
                    break;
                case ModeTaskDiss15.Task5: // КЗ-5
                    ImageTask_5();
                    break;
                case ModeTaskDiss15.Task6: // КЗ-6
                    ImageTask_6();
                    break;
                case ModeTaskDiss15.BPK: // БПК
                    ImageTask_BPK();
                    break;
                case ModeTaskDiss15.Zero: // Обнуление
                    ImageTask_Zero();
                    break;
                case ModeTaskDiss15.Speed: // Скорости отработки
                    ImageTask_Speed();
                    break;
                case ModeTaskDiss15.Freq: // Частота и Мощность СВЧ 
                    ImageTask_Freq();
                    break;
                case ModeTaskDiss15.Aist:
                    ImageTask_Aist();
                    break;
                case ModeTaskDiss15.SAU:
                    ImageTask_SAU();
                    break;
                case ModeTaskDiss15.SU:
                    ImageTask_SU();
                    break;
                case ModeTaskDiss15.DVS24:
                    ImageTask_DVS24();
                    break;
                case ModeTaskDiss15.Landysh:
                    ImageTask_Landysh();
                    break;
                default:
                    ImageOpenTask();
                    break;
            }
            Application.DoEvents();
        }

        private void ImageTask_Landysh()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlLinkAtPosition(new ControlLandysh(), 6, 6);
            ((ControlLandysh)_activeControl).TaskActivated += OnLandyshTaskActivated;
            ((ControlLandysh)_activeControl).TaskDeactivated += OnLandyshTaskDeactivated;
        }

        private void OnLandyshTaskActivated(ControlLandysh.Task task)
        {
            switch (task)
            {
                case ControlLandysh.Task.Task2:
                    InitializeLandyshTask2();
                    break;
                case ControlLandysh.Task.Task4:
                    InitializeLandyshTask4();
                    break;
                case ControlLandysh.Task.Task5:
                    InitializeLandyshTask5();
                    break;
                case ControlLandysh.Task.Task6:
                    InitializeLandyshTask6();
                    break;
                case ControlLandysh.Task.Task4_ExceptB10:
                    InitializeLandyshTask4_ExceptB10();
                    break;
            }
        }

        private void InitializeLandyshTask4_ExceptB10()
        {
            AddMeasurement(ControlLandysh.LandyshStatorXParam, (int)Globals.IndicatorNumber.LandyshTime1, Resources.Volt, String.Empty);
            AddMeasurement(ControlLandysh.LandyshStatorZParam, (int)Globals.IndicatorNumber.LandyshTime2, Resources.Volt, String.Empty);
            ClearMeasurementList();
            AddMeasurement("LandyshStatorValue", (int)Globals.IndicatorNumber.LandyshStator, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeLandyshTask6()
        {
            ClearMeasurementList();
            AddMeasurement("Landysh_KZ6_Wxr", (int)Globals.IndicatorNumber.LandyshKZ6Wxr, Resources.Volt, String.Empty);
            AddMeasurement("Landysh_KZ6_Wzr", (int)Globals.IndicatorNumber.LandyshKZ6Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeLandyshTask5()
        {
            ClearMeasurementList();
            AddMeasurement("Landysh_KZ5_Wzr", (int)Globals.IndicatorNumber.LandyshKZ5Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeLandyshTask4()
        {
            ClearMeasurementList();
            AddMeasurement("Landysh_KZ4_Wxr", (int)Globals.IndicatorNumber.LandyshKZ4Wxr, Resources.Volt, String.Empty);
            AddMeasurement("Landysh_KZ4_Wzr", (int)Globals.IndicatorNumber.LandyshKZ4Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeLandyshTask2()
        {
            ClearMeasurementList();
            AddMeasurement("Landysh_KZ2_Wxr", (int)Globals.IndicatorNumber.LandyshKZ2Wxr, Resources.Volt, String.Empty);
            AddMeasurement("Landysh_KZ2_Wzr", (int)Globals.IndicatorNumber.LandyshKZ2Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void OnLandyshTaskDeactivated(ControlLandysh.Task task)
        {
            _measWorker.StopMeasuring(saveResults);
            Diss15Functions.TaskOff();
            DO176X.Instance.SetChannelsOff();
        }

        private void ImageTask_DVS24()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlLinkAtPosition(new ControlDVS_24(), 6, 6);
            ((ControlDVS_24)_activeControl).SaveParameter += ManualRecordDouble;
            ((ControlDVS_24)_activeControl).CheckAndSaveMapCorner += OnCheckAndSaveMapCorner;
        }

        private void OnCheckAndSaveMapCorner()
        {
            ManualRecordBool(ControlDVS_24.MapCorner, tiMapCorner.State);
        }

        private void ImageTask_SU()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlLinkAtPosition(new ControlSU(), 6, 6);
            ((ControlSU)_activeControl).TaskActivated += OnSUTaskActivated;
            ((ControlSU)_activeControl).TaskDeactivated += OnSUTaskDeactivated;
        }

        private void OnSUTaskDeactivated(ControlSU.Task obj)
        {
            _measWorker.StopMeasuring(saveResults);
            Diss15Functions.TaskOff();
        }

        private void OnSUTaskActivated(ControlSU.Task task)
        {
            switch (task)
            {
                case ControlSU.Task.Task1:
                    InitalizeSUTask1();
                    break;
                case ControlSU.Task.Task2:
                    InitalizeSUTask2();
                    break;
                case ControlSU.Task.Task3:
                    InitalizeSUTask3();
                    break;
                case ControlSU.Task.Task4:
                    InitalizeSUTask4();
                    break;
                case ControlSU.Task.Task5:
                    InitalizeSUTask5();
                    break;
                case ControlSU.Task.Task1_1:
                    InitalizeSUTask1_1();
                    break;
            }
        }

        private void InitalizeSUTask1_1()
        {
            ClearMeasurementList();
            AddMeasurement("SU_KZ1_Wyr2", (int)Globals.IndicatorNumber.SU_KZ1_Wyr2, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSUTask1()
        {
            ClearMeasurementList();
            AddMeasurement("SU_KZ1_Wxr", (int)Globals.IndicatorNumber.SUWxr, Resources.Volt, String.Empty);
            AddMeasurement("SU_KZ1_Wyr", (int)Globals.IndicatorNumber.SU_KZ1_Wyr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSUTask2()
        {
            ClearMeasurementList();
            AddMeasurement("SU_KZ2_Wzr", (int)Globals.IndicatorNumber.SUWzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSUTask3()
        {
            ClearMeasurementList();
            AddMeasurement("SU_Wyr", (int)Globals.IndicatorNumber.SUWyr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSUTask4()
        {
            ClearMeasurementList();
            AddMeasurement("SU_KZ4_Wyr", (int)Globals.IndicatorNumber.SU_KZ4_Wyr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSUTask5()
        {
            ClearMeasurementList();
            AddMeasurement("SU_KZ5_Wxr", (int)Globals.IndicatorNumber.SU_KZ5_Wxr, Resources.Volt, String.Empty);
            AddMeasurement("SU_KZ5_Wzr", (int)Globals.IndicatorNumber.SU_KZ5_Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void ImageTask_SAU()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlLinkAtPosition(new ControlSAU(), 6, 6);
            ((ControlSAU)_activeControl).TaskActivated += OnSAUTaskActivated;
            ((ControlSAU)_activeControl).TaskDeactivated += OnSAUTaskDeactivated;
            ((ControlSAU)_activeControl).SaveParameterEvent += ManualRecordBool;
            ((ControlSAU)_activeControl).CheckWorkabilityEvent += OnCheckWorkabilityEvent;
            ((ControlSAU)_activeControl).CommandMessage += OnCommandMessage;
            ((ControlSAU)_activeControl).StopMeasuring += OnStopMeasuring;
        }

        private void OnStopMeasuring()
        {
            _measWorker.StopMeasuring(saveResults);
        }

        private bool OnCommandMessage(string message, string caption)
        {
            if (InvokeRequired)
                return (bool)Invoke(new ControlSAU.CommandMessageDelegate(OnCommandMessage), message, caption);
            return MessageBox.Show(message, caption, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        private bool OnCheckWorkabilityEvent()
        {
            return tiWorkability.State;
        }

        private void OnSAUTaskDeactivated(ControlSAU.Task task)
        {
            _measWorker.StopMeasuring(saveResults);
            Diss15Functions.TaskOff();
            DO176X.Instance.SetChannelsOff();
        }

        private void OnSAUTaskActivated(ControlSAU.Task task)
        {
            switch (task)
            {
                case ControlSAU.Task.Task6:
                    InitializeSAUTask6();
                    break;
                case ControlSAU.Task.Task5:
                    InitializeSAUTask5();
                    break;
                case ControlSAU.Task.Task1:
                    InitalizeSAUTask1();
                    break;
                case ControlSAU.Task.Task2:
                    InitalizeSAUTask2();
                    break;
                case ControlSAU.Task.Task3:
                    InitalizeSAUTask3();
                    break;
                default:
                    InitializeSAUTask3_2();
                    break;
            }
        }

        private void InitalizeSAUTask2()
        {
            ClearMeasurementList();
            Diss15Functions.SetSAU_Wzr();
            AddMeasurement("SAU_KZ2_Wzr", (int)Globals.IndicatorNumber.SAU2Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSAUTask3()
        {
            ClearMeasurementList();
            //Diss15Functions.SetSAU_Wxr();
            AddMeasurement("SAU_KZ3_Wxr", (int)Globals.IndicatorNumber.SAU3Wxr, Resources.Volt, String.Empty);
            AddMeasurement("SAU_KZ3_Wyr", (int)Globals.IndicatorNumber.SAU3Wyr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeSAUTask3_2()
        {
            ClearMeasurementList();
            Diss15Functions.SetSAU_Wzr();
            AddMeasurement("SAU_KZ3_Wzr", (int)Globals.IndicatorNumber.SAU3Wzr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitalizeSAUTask1()
        {
            ClearMeasurementList();
            Diss15Functions.SetSAU_Wxr();
            AddMeasurement("SAU_KZ1_Wxr", (int)Globals.IndicatorNumber.SAU1Wxr, Resources.Volt, String.Empty);
            AddMeasurement("SAU_KZ1_Wyr", (int)Globals.IndicatorNumber.SAU1Wyr, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeSAUTask5()
        {
            ClearMeasurementList();
            Diss15Functions.SetSAU_Alfa();
            AddMeasurement("SAU_KZ5_A", (int)Globals.IndicatorNumber.SAU6A, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeSAUTask6()
        {
            ClearMeasurementList();
            Diss15Functions.SetSAU_Alfa();
            AddMeasurement("SAU_KZ6_A", (int)Globals.IndicatorNumber.SAU6A, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void ImageTask_Aist()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlLinkAtPosition(new ControlAist(), 6, 6);
            ((ControlAist)_activeControl).TaskActivated += OnAistTaskActivated;
            ((ControlAist)_activeControl).TaskDeactivated += OnAistTaskDeactivated;
        }

        private void OnAistTaskDeactivated(ControlAist.Task task)
        {
            switch (task)
            {
                case ControlAist.Task.SKTZero:
                    _measWorker.StopMeasuring(false);
                    ControlAist aist = (ControlAist)_activeControl;
                    if (aist.StateAMove != null)
                        ManualRecordBool("AistAMove", (bool)aist.StateAMove);
                    if (aist.StateAClose != null)
                        ManualRecordBool("AistAClose", (bool)aist.StateAClose);
                    break;
                default:
                    _measWorker.StopMeasuring(saveResults);
                    Diss15Functions.TaskOff();
                    break;
            }
        }

        private void OnAistTaskActivated(ControlAist.Task task)
        {
            switch (task)
            {
                case ControlAist.Task.Task4:

                    InitializeAistTask4();
                    break;
                case ControlAist.Task.Task5:
                    InitializeAistTask5();
                    break;
                case ControlAist.Task.Task6:
                    InitializeAistTask6();
                    break;
                case ControlAist.Task.SKTZero:
                    InitializeAistTaskZero();
                    break;
            }
        }

        private void InitializeAistTask6()
        {
            ClearMeasurementList();
            AddMeasurement("AistKZ6W", (int)Globals.IndicatorNumber.Aist6W, Resources.Volt, String.Empty);
            AddMeasurement("AistKZ6Alpha", (int)Globals.IndicatorNumber.Aist6A, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeAistTask5()
        {
            ClearMeasurementList();
            AddMeasurement("AistKZ5Alpha", (int)Globals.IndicatorNumber.Aist5A, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeAistTaskZero()
        {
            ClearMeasurementList();
            AddMeasurement("AistKZZeroAlpha", (int)Globals.IndicatorNumber.AistZeroA, Resources.Volt, String.Empty);
            StartWorker();
        }

        private void InitializeAistTask4()
        {
            ClearMeasurementList();
            AddMeasurement("AistKZ4W", (int)Globals.IndicatorNumber.Aist4W, Resources.Volt, String.Empty);
            AddMeasurement("AistKZ4Alpha", (int)Globals.IndicatorNumber.Aist4A, Resources.Volt, String.Empty);
            StartWorker();
        }

        /// <summary>
        /// Открыли вкладку "Контрольные задачи"
        /// </summary>
        private void ImageOpenTask()
        {
            TaskCustomView(false, false, false, false, false);
        }

        /// <summary>
        /// Остановили измерения на вкладке 
        /// </summary>
        private void ImageStopTask()
        {
            _measWorker.StopMeasuring(false);
            _measWorker.StartMeasuring();
            if (_activeControl != null)
                _activeControl.Enabled = false;
            siSpeed1.Inactive = siSpeed2.Inactive = true;
            gbTask4.Enabled = false;
            indValue1.Enabled = indValue2.Enabled = false;
            btn_Memory.Enabled = false;
            if (gbTask.Visible && siMemory.Visible) // КЗ-1
            {
                tmrMemory.Stop();
                btn_Memory.Enabled = false; // 
                btn_Memory.Text = memory1;
            }
            indValue3.Visible = false;
            Application.DoEvents();
        }

        /// <summary>
        /// Настройка для КЗ-1
        /// </summary>
        private void ImageTask_1()
        {
            TaskCustomView(true, true, true, true, false);
            btn_Memory.Text = memory1;

            gbTask.Text = String.Format(Resources.ControlTaskN, 1);
            grpBox_Value.Text = Resources.MeasuredSpeedParams;
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 6);

            SetupIndicatorValue(indValue1, Globals.ValueType.SpeedForward);
            SetupIndicatorValue(indValue2, Globals.ValueType.SpeedUp);

            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 6);
        }

        private void SetupIndicatorValue(IndicatorValue ind, Globals.ValueType valueType)
        {
            double minGreen = double.NaN, maxGreen = Double.NaN;
            int min, max;
            Globals.GetMinAndMaxByValue(valueType, out min, out max);
            ind.MaxValue = max;
            ind.MinValue = min;

            switch (valueType)
            {
                case Globals.ValueType.SpeedForward:
                    ind.UpSign = String.Format(Resources.Wxg, Resources.KmH);
                    ind.DownSign = Resources.Forward;
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task1_ForwardSpeed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.SpeedForward;
                    break;

                case Globals.ValueType.SpeedUp:
                    ind.UpSign = String.Format(Resources.Wyg, Resources.MSec);
                    ind.DownSign = Resources.Up;
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task1_UpSpeed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.SpeedForward;
                    break;

                case Globals.ValueType.SpeedBackward:
                    ind.UpSign = String.Format(Resources.Wxg, Resources.KmH);
                    ind.DownSign = Resources.Back;
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task2_BackwardSpeed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.SpeedBackward;
                    break;

                case Globals.ValueType.SpeedRight:
                    ind.UpSign = String.Format(Resources.Wzg, Resources.KmH);
                    ind.DownSign = Resources.Right;
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task2_RightSpeed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.SpeedRight;
                    break;

                case Globals.ValueType.SpeedDown:
                    ind.UpSign = String.Format(Resources.Wyg, Resources.MSec);
                    ind.DownSign = Resources.Down;
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task3_DownSpeed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.SpeedDown;
                    break;
                case Globals.ValueType.SpeedDownUncontrolled:
                    ind.UpSign = String.Format(Resources.Wyg, Resources.MSec);
                    ind.DownSign = Resources.Down;
                    ind.Description = Resources.SpeedDown;
                    break;
                case Globals.ValueType.SpeedUpUncontrolled:
                    ind.UpSign = String.Format(Resources.Wyg, Resources.MSec);
                    ind.DownSign = Resources.Up;
                    ind.Description = Resources.SpeedUp;
                    break;

                case Globals.ValueType.ZeroAngle:
                    ind.UpSign = Resources.AlfaGrad;
                    ind.DownSign = String.Format(Resources.US, 0);
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task4_Angle, ref minGreen, ref maxGreen);
                    ind.Description = Resources.Angle;
                    break;

                case Globals.ValueType.FastSpeed1:
                    ind.UpSign = String.Format(Resources.WN, Resources.KmH);
                    ind.DownSign = String.Format(Resources.PS, 136);
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task4_Speed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.Speed;
                    break;

                case Globals.ValueType.PositiveAngle:
                    ind.UpSign = Resources.AlfaGrad;
                    ind.DownSign = String.Format(Resources.US, "+18");
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task5_Angle, ref minGreen, ref maxGreen);
                    ind.Description = Resources.Angle;
                    break;

                case Globals.ValueType.FastSpeed:
                    ind.UpSign = String.Format(Resources.WN, Resources.KmH);
                    ind.DownSign = String.Format(Resources.PS, "");
                    ind.Description = Resources.Speed;
                    break;

                case Globals.ValueType.NegativeAngle:
                    ind.UpSign = Resources.AlfaGrad;
                    ind.DownSign = String.Format(Resources.US, "-18");
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task6_Angle, ref minGreen, ref maxGreen);
                    ind.Description = Resources.Angle;
                    break;
                case Globals.ValueType.FastSpeed2:
                    ind.UpSign = String.Format(Resources.WN, Resources.KmH);
                    ind.DownSign = String.Format(Resources.PS, 215);
                    DataManager.Instance.GetParamLimit((int)TestTypeId_Diss15.Task6_Speed, ref minGreen, ref maxGreen);
                    ind.Description = Resources.Speed;
                    break;
            }
            ind.MinGreenValue = minGreen;
            ind.MaxGreenValue = maxGreen;
            ind.ResetErrorInputValue();
            ind.ErrorInputValue += ErrorInputValue;
            ind.ResetErrorDiapazonValue();
            ind.ErrorDiapazonValue += ErrorDiapazonValue;
        }

        private void ErrorDiapazonValue(string name, double min, double max, double value)
        {
            MessageBox.Show(String.Format(Resources.ErrorDiapazonValue, name, min, max));
        }

        private void ErrorInputValue(string value)
        {
            MessageBox.Show(String.Format(Resources.ErrorInputNValue, value));
        }

        /// <summary>
        /// Добавляем контрол сразу на нужную позицию
        /// </summary>
        /// <param name="newCtrl"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void AddTaskControlAtPosition(Control newCtrl, int xPos, int yPos)
        {
            AddTaskControlAtPositionExtended(pageTask, newCtrl, xPos, yPos);
        }

        private void AddTaskControlAtPositionExtended(TabPage tb, Control newCtrl, int xPos, int yPos)
        {
            _activeControl = newCtrl;
            tb.Controls.Add(_activeControl);
            Point p = new Point(xPos, yPos);
            _activeControl.Location = p;
        }

        /// <summary>
        /// Добавляем контрол сразу на нужную позицию
        /// </summary>
        /// <param name="newCtrl"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void AddTaskControlLinkAtPosition(Control newCtrl, int xPos, int yPos)
        {
            AddTaskControlAtPositionExtended(tbLink, newCtrl, xPos, yPos);
        }

        /// <summary>
        /// Настройка для КЗ-2
        /// </summary>
        private void ImageTask_2()
        {
            TaskCustomView(true, true, true, false, false);
            AddTaskControlAtPosition(new Corners(this, _device), 330, 6);

            gbTask.Text = String.Format(Resources.ControlTaskN, 2);
            grpBox_Value.Text = Resources.MeasuredSpeedParams;
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 6);

            SetupIndicatorValue(indValue1, Globals.ValueType.SpeedBackward);
            SetupIndicatorValue(indValue2, Globals.ValueType.SpeedRight);
            SetupIndicatorValue(indValue3, Globals.ValueType.SpeedUpUncontrolled);

            indValue3.Visible = true;


            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 6);
        }

        /// <summary>
        /// Настройка для КЗ-3
        /// </summary>
        private void ImageTask_3()
        {
            TaskCustomView(true, true, false, false, false);

            gbTask.Text = String.Format(Resources.ControlTaskN, 3);
            grpBox_Value.Text = String.Format(Resources.MeasuredValueN, String.Format(Resources.Wyg, Resources.MSec));
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 6);

            SetupIndicatorValue(indValue1, Globals.ValueType.SpeedDown);

            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 6);
        }

        /// <summary>
        /// Настройка для КЗ-4
        /// </summary>
        private void ImageTask_4()
        {
            TaskCustomView(true, true, true, true, true);

            btn_Memory.Text = memory1;

            gbTask.Text = String.Format(Resources.ControlTaskN, 4);
            grpBox_Value.Text = Resources.MeasuredValuesPSAndUS;
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 7);

            SetupIndicatorValue(indValue1, Globals.ValueType.ZeroAngle);
            SetupIndicatorValue(indValue2, Globals.ValueType.FastSpeed1);
            indValue2.Visible = false;

            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 7);

            gbTask4.Enabled = true;
            progressBar_4.Value = 0;
            btn_8_Off_4.PerformClick();
            btn_4_Course.Enabled = cmbBox_4_Course.Enabled = cmbBox_4_1.Enabled =
                                                             cmbBox_4_2.Enabled = txtBox_4_1.Enabled = txtBox_4_2.Enabled =
                                                                                                       btn_8_On_4.Enabled =
                                                                                                       btn_8_Off_4.Enabled =
                                                                                                       false;

            lbl_4_Time.Visible = progressBar_4.Visible = false;
            rbtn_4_Zero.Checked = rbtn_4_Course.Checked = false;
            rbtn_4_Zero.Enabled = rbtn_4_Course.Enabled = true;
            rbtn_4_Zero.PerformClick();

            cmbBox_4_1.Items.Clear();
            cmbBox_4_1.Items.AddRange(new[] { Resources.Forward, Resources.Back });
            cmbBox_4_2.Items.Clear();
            cmbBox_4_2.Items.AddRange(new[] { Resources.Left, Resources.Right });

            arTask4Visible[0] = arTask4Visible[1] = false;
        }

        /// <summary>
        /// Настройка для КЗ-5
        /// </summary>
        private void ImageTask_5()
        {
            TaskCustomView(true, true, true, false, false);

            gbTask.Text = String.Format(Resources.ControlTaskN, 5);
            grpBox_Value.Text = String.Format(Resources.MeasuredValuesPSAndUS);
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 7);

            SetupIndicatorValue(indValue1, Globals.ValueType.PositiveAngle);
            SetupIndicatorValue(indValue2, Globals.ValueType.FastSpeed);
            indValue2.Visible = false;

            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 7);
        }

        /// <summary>
        /// Настройка для КЗ-6
        /// </summary>
        private void ImageTask_6()
        {
            TaskCustomView(true, true, true, false, false);
            AddTaskControlAtPosition(new Block8Check(this, _device), 290, 13);
            gbTask.Text = String.Format(Resources.ControlTaskN, 6);
            grpBox_Value.Text = Resources.MeasuredValuesPSAndUS;
            grpBox_Block.Text = String.Format(Resources.DataFromUnitN, 7);

            SetupIndicatorValue(indValue1, Globals.ValueType.NegativeAngle);
            SetupIndicatorValue(indValue2, Globals.ValueType.FastSpeed2);

            lbl_Set_Task.Text = String.Format(Resources.WaitWhileStabilizeUnitN, 7);
        }

        /// <summary>
        /// Настройка для БПК/Расчитать погрешность 
        /// </summary>
        private void ImageTask_BPK()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlAtPosition(new PK(this), 12, 12);
        }

        /// <summary>
        /// Настройка при обнулении составляющих .........
        /// </summary>
        private void ImageTask_Zero()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlAtPosition(new ZeroVoltage(), 12, 11);
        }

        /// <summary>
        /// Настройка при Измерении скоростей отработки 
        /// </summary>
        private void ImageTask_Speed()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlAtPosition(new StabilizeSpeed(this), 6, 6);
        }

        /// <summary>
        /// Настройка для "Частота и Мощность СВЧ" 
        /// </summary>
        private void ImageTask_Freq()
        {
            TaskCustomView(false, false, false, false, false);
            AddTaskControlAtPosition(new SVChFreqAndPower(this), 6, 6);
        }

        #endregion Настройка индикаторов для вкладки "Контрольные задачи"

        #region Initializ

        /// <summary>
        /// Инициализация для измерения УНЧ 
        /// </summary>
        private void InitializUNCh()
        {
            ClearMeasurementList();
            AddSameMeasuring("UNCh_{0}_{1}", Resources.UNChNN, String.Format(Resources.UV, Resources.Volt), 0);
            StartWorker();
        }

        /// <summary>
        /// Инициализация для измерения Шума  
        /// </summary>
        private void InitializNoise()
        {
            ClearMeasurementList();
            AddSameMeasuring("UNCh_{0}_{1}_Noise_I", Resources.NoiseNN, String.Format(Resources.UV, Resources.miliVolt), 0);
            StartWorker();
        }

        /// <summary>
        /// Инициализация для измерения ТКМ  
        /// </summary>
        private void InitializTKM()
        {
            ClearMeasurementList();
            AddMeasurement("HV", 0, String.Format(Resources.UV, Resources.Volt), Resources.HV);
            AddMeasurement("I_Mgn", 1, String.Format(Resources.IA, Resources.miliAmper), Resources.CurrentMagnetron);
            AddMeasurement("SVCh_P", 2, String.Format(Resources.IA, Resources.miliAmper), Resources.SVChPowerControl);
            AddMeasurement("TKM_1", 4, String.Format(Resources.UV, Resources.Volt),
                String.Format(Resources.CurrentMagnetronN, 1));
            AddMeasurement("TKM_2", 5, String.Format(Resources.UV, Resources.Volt),
                String.Format(Resources.CurrentMagnetronN, 2));
            StartWorker();

        }

        /// <summary>
        /// Инициализация для измерения Ток К 
        /// </summary>
        private void InitializTok_K()
        {
            ClearMeasurementList();
            AddSameMeasuring("I_K_{0}_{1}_15", "ТК {0}-{1}", String.Format(Resources.IA, Resources.miliAmper), 0);
            StartWorker();
            AddMeasurement(I_K_1, indK1Num, String.Format(Resources.IA, Resources.miliAmper), String.Format(Res.DeltaTKNAbs, 1));
            AddMeasurement(I_K_2, indK2Num, String.Format(Resources.IA, Resources.miliAmper), String.Format(Res.DeltaTKNAbs, 2));
            AddMeasurement(I_K_3, indK3Num, String.Format(Resources.IA, Resources.miliAmper), String.Format(Res.DeltaTKNAbs, 3));
        }

        private void AddSameMeasuring(string paramName, string iName, string Units, int shiftIndication)
        {
            const int numGroup1 = 3;
            for (int group1 = 1; group1 <= numGroup1; group1++)
                for (int group2 = 1; group2 <= 2; group2++)
                    AddMeasurement(String.Format(paramName, group1, group2), (group1 - 1) * numGroup1 + group2 + shiftIndication - 1,
                                   Units,
                                   String.Format(iName, group1, group2));
        }

        private static string Delta27V = "Delta_27V";

        /// <summary>
        /// Инициализация для измерения Вторичных источников питания 
        /// </summary>
        private void InitializSecPitan()
        {
            ClearMeasurementList();
            AddMeasurement("Plus_12V", 0, String.Format(Resources.UV, Resources.Volt), "+12,6" + Resources.Volt);
            AddMeasurement("Minus_12V", 1, String.Format(Resources.UV, Resources.Volt), "-12,6" + Resources.Volt);
            AddMeasurement("Minus_6V", 3, String.Format(Resources.UV, Resources.Volt), "-6,3" + Resources.Volt);
            AddMeasurement("Minus_20V", 5, String.Format(Resources.UV, Resources.Volt), "-20" + Resources.Volt);
            AddMeasurement("Minus_27V", 8, String.Format(Resources.UV, Resources.Volt), "-27" + Resources.Volt);
            AddMeasurement("Minus_27V_Op", 10, String.Format(Resources.UV, Resources.Volt), Res._27VRef);
            AddMeasurement("Plus_27V_Sl", 11, String.Format(Resources.UV, Resources.Volt), Res._27VTs);
            AddMeasurement(Delta27V, 13, "", "");
            AddMeasurement(specSignal[0], 12, String.Format(((Char)0x0394) + Resources.UV, Resources.Percent),
                           ((Char)0x0394) + "27" + Resources.Volt);
            //_specSignalKoef = DataManager.Instance.GetParamKoef(specSignal[0]);
            StartWorker();
        }

        /// <summary>
        /// Инициализация для измерения Источников питания 
        /// </summary>
        private void InitializFirstPitan()
        {
            //Ток 27В
            SetupIndicator(1, String.Format(Resources.IA, Resources.Amper),
                String.Format(Resources.CurrentByN, "27" + Resources.Volt), "0");
            SetIndicatorLimits(1, 0, 0, 2, 3);
            //            
            AddMeasurement(PS115V_Read_U, 4, String.Format(Resources.UV, Resources.Volt),
                String.Format(Resources.Positive115V, Resources.Volt));
            AddMeasurement(PS115V_Read_I, 5, String.Format(Resources.IA, Resources.Amper),
                String.Format(Resources.CurrentByN, "115" + Resources.Volt));
            SetIndicatorLimits(5, 2, 0);
            ClearMeasurementList();
            AddMeasurement("Plus_27V", 0, String.Format(Resources.UV, Resources.Volt),
                String.Format(Resources.Positive27V, Resources.Volt));
            StartWorker();
        }

        /// <summary>
        /// Инициализация для КЗ-1 
        /// </summary>
        private void InitializTask_1()
        {
            ClearMeasurementList();
            AddMeasurement("W_Forward_15", (int)Globals.IndicatorNumber.Speed1,
                String.Format(Resources.Wxg, Resources.KmH), Resources.Forward);
            AddMeasurement("W_Up_15", (int)Globals.IndicatorNumber.Speed2,
                String.Format(Resources.Wyg, Resources.MSec), Resources.Up);
            StartWorker();
        }

        public string strW_Back_15 
        {
            get { return "W_Back_15"; }
        }

        public string strW_Right_15
        {
            get { return "W_Right_15"; }
        }

/// <summary>
/// Инициализация для КЗ-2 
/// </summary>
private void InitializTask_2()
        {
            ClearMeasurementList();
            AddMeasurement(strW_Back_15, (int)Globals.IndicatorNumber.Speed1,
                String.Format(Resources.Wxg, Resources.KmH), Resources.Back);
            AddMeasurement(strW_Right_15, (int)Globals.IndicatorNumber.Speed2,
                String.Format(Resources.Wzg, Resources.KmH), Resources.Right);
            StartWorker();
        }

        /// <summary>
        /// Инициализация для КЗ-4 
        /// </summary>
        private void InitializTask_4()
        {
            ClearMeasurementList();
            AddMeasurement(alfa_0_15, (int)Globals.IndicatorNumber.Speed1,
                           Resources.AlfaGrad, String.Format(Resources.US, 0));
            AddMeasurement(spd_task4_15, (int)Globals.IndicatorNumber.Speed2,
                String.Format(Resources.WN, Resources.KmH), String.Format(Resources.PS, ""));
            StartWorker();
        }

        /// <summary>
        /// "Память" - показываем индикатор 
        /// </summary>
        private void InitializMemoryOn()
        {
            label46.Visible = siMemory.Visible = lMemoryUpLabel.Visible = true;
            siMemory.ValueString = "0.0";
            siMemory.FractPart = 1;
            dTime = 0.0;
        }

        /// <summary>
        /// Инициализация для КЗ-5 
        /// </summary>
        private void InitializTask_5()
        {
            ClearMeasurementList();
            AddMeasurement("Alfa_18Plus", (int)Globals.IndicatorNumber.Speed1, Resources.AlfaGrad,
                String.Format(Resources.US, "+18"));
            AddMeasurement("Spd_Task6_15", (int)Globals.IndicatorNumber.Speed2, String.Format(Resources.WN, Resources.KmH),
                String.Format(Resources.PS, ""));
            StartWorker();
        }

        /// <summary>
        /// Инициализация для КЗ-6 
        /// </summary>
        public void InitializTask_6()
        {
            ClearMeasurementList();
            AddMeasurement(alfa_18minus, (int)Globals.IndicatorNumber.Speed1, Resources.AlfaGrad,
                String.Format(Resources.US, "-18"));
            AddMeasurement(spd_task6_15, (int)Globals.IndicatorNumber.Speed2, String.Format(Resources.WN, Resources.KmH),
                String.Format(Resources.PS, 215));
            StartWorker();
        }

        /// <summary>
        /// Инициализация для Wxr, Wyr, Wzr при обнулении 
        /// </summary>
        private void InitializZero()
        {
            ClearMeasurementList();
            AddMeasurement("Wxr_Zero_15", (int)Globals.IndicatorNumber.Zero1, "", "");
            AddMeasurement("Wzr_Zero_15", (int)Globals.IndicatorNumber.Zero2, "", "");
            AddMeasurement("Wyr_Zero_15", (int)Globals.IndicatorNumber.Zero3, "", "");
            SetIndicatorLimits((int)Globals.IndicatorNumber.Zero1, 3, 1);
            SetIndicatorLimits((int)Globals.IndicatorNumber.Zero2, 3, 1);
            SetIndicatorLimits((int)Globals.IndicatorNumber.Zero3, 3, 1);
            StartWorker();
        }

        #endregion

        #region Adding


        /// <summary>
        /// Добавляем все сигналы для Шума - Полоса 2 
        /// </summary>
        private void InitializNoiseII()
        {
            ClearMeasurementList();
            AddSameMeasuring("UNCh_{0}_{1}_Noise_II", Resources.NoiseNN, String.Format(Resources.UV, Resources.miliVolt), 0);
            StartWorker();
        }

        #endregion

        #region Deleting

        /// <summary>
        /// Чистим все списки и останавливаем все платы 
        /// </summary>
        public void DeletingAll()
        {
            _measWorker.StopMeasuring(saveResults);
        }

        #endregion

        #endregion Вспомогательные функции

        #region Таймеры

        // Время для отработки Блока 8 (через 1сек)
        private void tmrBlock8_Tick(object sender, EventArgs e)
        {
            countBlock8--;
            int s = countBlock8 % 60; // кол-во секунд
            int m = countBlock8 / 60; // кол-во секунд
            if (btn_8_On_4.FlatStyle == FlatStyle.Flat) // для КЗ-4
            {
                lbl_4_Time.Text = String.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, m, Resources.Minutes, s, Resources.Seconds);
                progressBar_4.Value++;
                if (countBlock8 == 0)
                    btn_8_Off_4.PerformClick(); // время кончилось
            }
        }

        // "ПАМЯТЬ" - 0,1 сек
        private void tmrMemory_Tick(object sender, EventArgs e)
        {
            dTime = (DateTime.Now - time_On).TotalSeconds;
            siMemory.Value = dTime;
            // проверка сигнала "ПАМЯТЬ" 
            if (tiMemory.State) // появился сигнал "ПАМЯТЬ" 
            {
                tmrMemory.Stop();
                ManualRecordDouble("Memory_Time_15", dTime);
                siMemory.FractPart = 2;
                btn_Memory.Enabled = false;

                EnableTaskTabButtons(btn_Task_1);
                ImageTask(ModeTaskDiss15.Stop);
            }
            else if (dTime >= 5) // 5 сек ждем сигнал "ПАМЯТЬ" 
            {
                tmrMemory.Stop();
                EnableTaskTabButtons(btn_Task_1);
                ImageTask(ModeTaskDiss15.Stop);
                btn_Memory.Enabled = false;
                MessageBox.Show(String.Format(Resources.SignalNNotFound, Resources.Memory.ToUpper()));

            }
        }

        // Время включение ВН 
        private void tmrHV_Tick(object sender, EventArgs e)
        {
            if (!arBH[0])
            {
                arBH[0] = tiWork.State;
                if (arBH[0])
                    dtStart = DateTime.Now;
            }
            if (arBH[0])
                dTime += 1;
            siTimeHV.Value = dTime;
        }

        private DateTime dtStart;

        /// <summary>
        /// Старт/стоп измерения времени появления "ВН" 
        /// </summary>
        private void TimerHVStart(bool state)
        {
            if (state)
            {
                dTime = 0;
                arBH[0] = tiWork.State;
                dtStart = DateTime.Now;
                forTok_K = false;
                tmrHV.Start();
            }
            else
            {
                tmrHV.Stop();
                TimeSpan ts = DateTime.Now - dtStart;
                dTime = Math.Round(ts.TotalSeconds + ts.Milliseconds / 1000d, 2);
                siTimeHV.MinValue = HVMidDelayValue;
                siTimeHV.Value = dTime;
                siTimeHV.LightColor = Color.Lime;
                siTimeHV.Inactive = true;
                btn_High.Enabled = true;

                if (dTime >= VHTimeLimit)
                {
                    lbl_BH.Visible = true;
                    btn_Tok_K.Enabled = forTok_K = false;
                }
                else
                {
                    forTok_K = true;
                    btn_Tok_K.Enabled = (countButton == 0);
                }
            }
        }
        
        private string[] specSignal = new[] { "Delta_27V_15", "Minus_27V_Op", Delta27V };
        private double _specSignalKoef = 1;

        private void ShowResult(List<USBBaseNew> chList)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<List<USBBaseNew>>(ShowResult), new object[] { chList });
                return;
            }
            try
            {
                if (isEscape || frmMain.IsFormClose)
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
                        SetIndicatorValue(signal.IndicatorNumber, CalculateDelta27Percent(voltage1, voltage2));
                    else
                        SetIndicatorValue(signal.IndicatorNumber, signal.Result);
                }
                AdditionalShowResult();
            }
            catch
            {
                //для отладки
            }
        }

        private void AdditionalShowResult()
        {
            if (btn_Tok_K.FlatStyle == FlatStyle.Flat)
            {
                SetIndicatorValue(8, Math.Abs(Math.Abs(GetIndicatorValue(0)) - Math.Abs(GetIndicatorValue(1))));
                SetIndicatorValue(11, Math.Abs(Math.Abs(GetIndicatorValue(3)) - Math.Abs(GetIndicatorValue(4))));
                SetIndicatorValue(14, Math.Abs(Math.Abs(GetIndicatorValue(6)) - Math.Abs(GetIndicatorValue(7))));
            }
        }

        #endregion Таймеры

        private void frmWork15_Shown(object sender, EventArgs e)
        {
            // команда на запуск tablo 
            try
            {
                if (!DiscreteInputs.Instance().StartMeasuring(DiscreteInputsReceived))
                    MessageBox.Show(Globals.GetDeviceErrorString(Globals.Device.DiscreteInputs, Globals.Operation.GetValue));
            }
            catch
            {
                tabControl.Enabled = grpBox_Mode.Enabled = gbPowerMode.Enabled = false;
                MessageBox.Show(Resources.ErrorCommand + Environment.NewLine + Resources.ApplicationMustBeClosed,
                                Resources.ErrorCaption);
            }
            //if (_curBlock5Type == Block5Type.None)
            //{
            //  frmBlock5Type frm = new frmBlock5Type();
            //  frm.StartPosition = FormStartPosition.CenterScreen;
            //  frm.ShowDialog();
            //  _curBlock5Type = frm.CurrentBlock5Type;
            //}
        }

        private void bAist_Click(object sender, EventArgs e)
        {
            bAist.Enabled = false;
            if (bAist.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(bAist);
                ImageTask(ModeTaskDiss15.Aist);
            }
            else
            {
                ((ControlAist)_activeControl).StopCurrentTask();
                EnableTaskTabButtons(bAist);
                ImageTask(ModeTaskDiss15.Stop);
            }
            bAist.Enabled = true;
        }

        private void bSAU_Click(object sender, EventArgs e)
        {
            bSAU.Enabled = false;
            if (bSAU.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(bSAU);
                ImageTask(ModeTaskDiss15.SAU);
            }
            else
            {
                ((ControlSAU)_activeControl).StopCurrentTask();
                EnableTaskTabButtons(bSAU);
                ImageTask(ModeTaskDiss15.Stop);
            }
            bSAU.Enabled = true;
        }

        private void bSU_Click(object sender, EventArgs e)
        {
            bSU.Enabled = false;
            if (bSU.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(bSU);
                ImageTask(ModeTaskDiss15.SU);
            }
            else
            {
                ((ControlSU)_activeControl).StopCurrentTask();
                EnableTaskTabButtons(bSU);
                ImageTask(ModeTaskDiss15.Stop);
            }
            bSU.Enabled = true;
        }

        private void bDVS24_Click(object sender, EventArgs e)
        {
            bDVS24.Enabled = false;
            if (bDVS24.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(bDVS24);
                ImageTask(ModeTaskDiss15.DVS24);
            }
            else
            {
                ((ControlDVS_24)_activeControl).StopCurrentTask();
                Diss15Functions.Block8Off();
                EnableTaskTabButtons(bDVS24);
                ImageTask(ModeTaskDiss15.Stop);
            }
            bDVS24.Enabled = true;
        }

        private void bLandysh_Click(object sender, EventArgs e)
        {
            Button bCurrent = bLandysh;
            bCurrent.Enabled = false;
            if (bCurrent.FlatStyle == FlatStyle.Standard)
            {
                DisableOtherTaskTabButtons(bCurrent);
                ImageTask(ModeTaskDiss15.Landysh);
            }
            else
            {
                ((ControlLandysh)_activeControl).StopCurrentTask();
                EnableTaskTabButtons(bCurrent);
                ImageTask(ModeTaskDiss15.Stop);
            }
            bCurrent.Enabled = true;
        }

        DateTime dtStartTraining = DateTime.Now;
        private Timer tTraining = new Timer();
        TimeSpan tsWaitTraining = new TimeSpan(0, 0, 1, 0);
        TimeSpan tsActiveTraining = new TimeSpan(0, 0, 3, 0);
        private int TrainingInterval = 3; //секунды
        private int _curTrainingInterval = 0;
        private bool _highVoltageState = false;

        private void bTraining_Click(object sender, EventArgs e)
        {
            bTraining.Enabled = false;
            if (tTraining.Enabled)
            {
                Training_Stop();
                bTraining.Text = Res.MagnetronTraining;
            }
            else
            {
                EnableButtonsByTraining(false);
                bTraining.Text = Res.StopMagnetronTraining;
                if (!SetHighVoltage(false))
                {
                    MessageBox.Show(Res.ErrorOnDeviceN,
                        Res.DiscreteOutputsBoard);
                    Training_Stop();
                    return;
                }
                dtStartTraining = DateTime.Now;
                tTraining = new Timer();
                tTraining.Interval = 1000;
                tTraining.Tick += TTrainingOnTick;
                tTraining.Start();
            }
            bTraining.Enabled = true;
        }

        private void TTrainingOnTick(object sender, EventArgs eventArgs)
        {
            TimeSpan tsLast = DateTime.Now - dtStartTraining;
            if (tsLast >= tsWaitTraining)
                Training_Step2();
            TimeSpan ts = tsWaitTraining - tsLast;
            if (ts.Seconds < 0)
                ts = new TimeSpan(0);
            siTrainingLeft.ValueString = GetTimeString(ts);
        }

        private void Training_Step2()
        {
            tTraining.Stop();
            if (!SetHighVoltage(true))
            {
                MessageBox.Show(Res.ErrorOnDeviceN,
                    Res.DiscreteOutputsBoard);
                Training_Stop();
                return;
            }
            dtStartTraining = DateTime.Now;
            _curTrainingInterval = 0;
            tTraining = new Timer();
            tTraining.Tick += TTrainingOnTick2;
            tTraining.Interval = 1000;
            tTraining.Start();
        }

        private const string MagnetronTrainingStr = "MagnetronTraining";

        private void TTrainingOnTick2(object sender, EventArgs eventArgs)
        {
            TimeSpan tsLast = DateTime.Now - dtStartTraining;
            _curTrainingInterval++;
            if (_curTrainingInterval >= TrainingInterval)
            {
                SetHighVoltage(!_highVoltageState);
                _curTrainingInterval = 0;
            }
            if (tsLast >= tsActiveTraining)
            {
                ManualRecordBool(MagnetronTrainingStr, true);
                Training_Stop();
            }
            siTrainingLeft.ValueString = GetTimeString(tsActiveTraining - tsLast);
        }

        private void Training_Stop()
        {
            tTraining.Stop();
            bTraining.Text = Sparc.KPK2.Resources.Properties.Resources.MagnetronTraining;
            siTrainingLeft.ValueString = GetTimeString(new TimeSpan(0));
            SetHighVoltage(true);
            EnableButtonsByTraining(true);
        }

        void EnableButtonsByTraining(bool state)
        {
            EnableButtons(state);
            rbtn_On.Enabled = rbtn_Off.Enabled = btn_High.Enabled = state;
            gbPowerMode.Enabled = state;
            countButton = state ? 0 : 1;
            if (!state)
                lTraningMagnetron.Visible = siTrainingLeft.Visible = true;
        }

        private void EnableButtons(bool state)
        {
            Control.ControlCollection ctrls = pageWork.Controls;
            foreach (Control ctrl in ctrls)
                if (ctrl is Button)
                    ctrl.Enabled = state;
        }

        private void bPlusGenerator_Click(object sender, EventArgs e)
        {
            GenSinCos.SelsinRefVoltage += 50;
            RenewGeneratorVoltageIndication();
        }

        private void bMinusGenerator_Click(object sender, EventArgs e)
        {
            GenSinCos.SelsinRefVoltage -= 50;
            RenewGeneratorVoltageIndication();
        }

        private void RenewGeneratorVoltageIndication()
        {
            nudSinVoltage.Value = GenSinCos.Instance()._voltage1;
            nudCosVoltage.Value = GenSinCos.Instance()._voltage2;
            nudSinAngle.Value = GenSinCos.Instance()._shift1;
            nudCosAngle.Value = GenSinCos.Instance()._shift2;
            lGeneratorVoltage.Text = GenSinCos.SelsinRefVoltage + Resources.miliVolt;
        }

        private void bSetGenerator_Click(object sender, EventArgs e)
        {
            GenSinCos.Instance().GeneratorOnLastAngle();
            RenewGeneratorVoltageIndication();
        }

        private void bPlusHighGenerator_Click(object sender, EventArgs e)
        {
            GenSinCos.SelsinRefVoltage += 1000;
            RenewGeneratorVoltageIndication();
        }

        private void bMinusHighGenerator_Click(object sender, EventArgs e)
        {
            GenSinCos.SelsinRefVoltage -= 1000;
            RenewGeneratorVoltageIndication();
        }

        private void bSetSin_Click(object sender, EventArgs e)
        {
            if (!GenSinCos.Instance().SetRealSignal(1, (int)nudSinVoltage.Value, GenSinCos.SelsinFreq,
                (int)nudSinAngle.Value))
                MessageBox.Show("Error!");
        }

        private void bSetCos_Click(object sender, EventArgs e)
        {
            if (!GenSinCos.Instance().SetRealSignal(2, (int)nudCosVoltage.Value, GenSinCos.SelsinFreq,
                (int)nudCosAngle.Value))
                MessageBox.Show("Error!");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            RenewGeneratorVoltageIndication();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            Diss15Functions.SetAngle(180);
        }

        private void rbtn_4_Course_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbZeroCourse_CheckedChanged(object sender, EventArgs e)
        {
            DiscreteOutputs.Instance().SetOutput("Course", cbZeroCourse.Checked);
        }
    }
}
