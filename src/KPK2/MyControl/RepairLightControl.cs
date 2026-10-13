using KPK2.DB;
using KPK2.GeneralForms.AddForms;
using KPK2.GeneralForms.DissForms;
using KPK2.MyControl.DISS.FrmDiss32;
using KPK2.Properties;
using KPK2.Interfaces;
using NLog;
using Sparc.KPK2.Hardware;
using System;
using System.Drawing;
using System.Windows.Forms;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
    public partial class RepairLightControl : UserControl
    {
        private DataBaseKPK2 _dbKPK2;
        private IFrmWork32 _frm;
        private IMeasurementsControl _measControl;
        private string[] _mainTitle;
        private string[] _mainTitleMemorize;
        private string[,] _titles;
        private string[,] _titlesMemorize;
        private const int gbNum = 7;
        private const int btnNum = 9;
        private const int indicatorFrm = 6;
        private const int btnNumIndicatorFrm = 8;
        private const int btnNumMemorize = 10;
        private bool _isDiss32;
        private TimeSpan _waitReadySignal = TimeSpan.FromSeconds(15);

        private bool?[,] _arLight_Mask = new bool?[btnNum, gbNum];
        private bool?[,] _arLight_Value = new bool?[btnNum, gbNum];
        private bool?[,] _arLight_FrmIndicator_Mask = new bool?[btnNumIndicatorFrm, indicatorFrm];
        private bool?[,] _arLight_FrmIndicator_Value = new bool?[btnNumIndicatorFrm, indicatorFrm];
        int MaxButtonNum = 9;

        public bool LastCheckMemorizePassed
        {
            get { return false; }
        }

        public bool CheckMemorizeResult
        {
            get { return _memorizeResult; }
        }

        public bool IsBtn1 => btn_Light_1.FlatStyle == FlatStyle.Flat;

        public bool IsBtn2 => btn_Light_2.FlatStyle == FlatStyle.Flat;

        public bool IsBtn3 => btn_Light_3.FlatStyle == FlatStyle.Flat;

        public bool IsBtn4 => btn_Light_4.FlatStyle == FlatStyle.Flat;

        public bool IsBtn5 => btn_Light_5.FlatStyle == FlatStyle.Flat;
        public bool IsBtn6 => btn_Light_6.FlatStyle == FlatStyle.Flat;
        public bool IsBtn7 => btn_Light_7.FlatStyle == FlatStyle.Flat;
        public bool IsBtn8 => btn_Light_8.FlatStyle == FlatStyle.Flat;

        private string MemorizeParam = "VisualControlMemorize";

        private string[] _arLight_Name = new[]
          {
        "Refuse_VCh", //0
        "Refuse_VSS", //1
        "Search",       //2
        "Display_P", //3
        "Flight", //4
        "Control_Diss", //5 
        "GoodCondition", //6
        "CalcDiss", //7
        "CalcDPSM",//8
        "Refuse_BVK", //9
        "PositionOff",//10
		"ModeNavigation_WorkTest",//11 
        "ModeMemory1_WorkTest", //12
        "ModeMemory2_WorkTest",//13
        "ModeVelocityF_WorkTest",//14
        "ModeVelocity258_WorkTest"//15
      };

        private string _mainButton;

        public RepairLightControl(IFrmWork32 frm, IMeasurementsControl measControl, DataBaseKPK2 dbKpk2, bool isDiss32, string mainButton)
        {
            _mainButton = mainButton;
            _frm = frm;
            _measControl = measControl;
            _isDiss32 = isDiss32;
            if (_isDiss32)
                MaxButtonNum = 6;
            _dbKPK2 = dbKpk2;
            InitializeComponent();
            FillTitles();
            FillArrays();
        }

        public void FinishCheck()
        {
            CheckIndicators();
            Logger logger = LogManager.GetLogger("KPK2Info");

            //кнопка 1
            string messageLogNameParam = _mainTitle[0];
            string[] messageLogValueParam = new string[]
            {
                GetIndicatorValue(_titles[0,0], _arLight_Value[0,0]),
                GetIndicatorValue(_titles[0,1], _arLight_Value[0, 1]),
                GetIndicatorValue(_titles[0,2], _arLight_Value[0, 2]),
                GetIndicatorValue(_titles[0,3], _arLight_Value[0, 3]),
                GetIndicatorValue("Память 1 (КПК2)", _arLight_FrmIndicator_Value[0, 0]),
                GetIndicatorValue("Память 2 (КПК2)", _arLight_FrmIndicator_Value [0, 1]),
                GetIndicatorValue("Отказ ВЧ (КПК2)", _arLight_FrmIndicator_Value[0, 2]),
                GetIndicatorValue("Отказ ВСС (КПК2)", _arLight_FrmIndicator_Value[0, 3])
            };
            logger.Info(messageLogNameParam);
            foreach (string s in messageLogValueParam)
                logger.Info($"{s}");

            //кнопка 2
            messageLogNameParam = _mainTitle[1];
            if (_isDiss32)
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[1, 0], _arLight_Value[1, 0]),
                    GetIndicatorValue(_titles[1, 1], _arLight_Value[1, 1]),
                    GetIndicatorValue(_titles[1, 2], _arLight_Value[1, 2]),
                    GetIndicatorValue(_titles[1, 3], _arLight_Value[1, 3]),
                    GetIndicatorValue("Память 1 (КПК2)", _arLight_FrmIndicator_Value[0, 0]),
                    GetIndicatorValue("Память 2 (КПК2)", _arLight_FrmIndicator_Value[0, 1]),
                    GetIndicatorValue("Отказ ВЧ (КПК2)", _arLight_FrmIndicator_Value[0, 2])
                };
            }
            else
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[1, 0], _arLight_Value[1, 0]),
                    GetIndicatorValue(_titles[1, 1], _arLight_Value[1, 1]),
                    GetIndicatorValue(_titles[1, 2], _arLight_Value[1, 2]),
                    GetIndicatorValue(_titles[1, 3], _arLight_Value[1, 3]),
                    GetIndicatorValue("Память 1 (КПК2)", _arLight_FrmIndicator_Value[1, 0]),
                    GetIndicatorValue("Память 2 (КПК2)", _arLight_FrmIndicator_Value[1, 1]),
                    GetIndicatorValue("Полет (КПК2)", _arLight_FrmIndicator_Value[1, 2]),
                    GetIndicatorValue(_titles[1, 4], _arLight_FrmIndicator_Value[1, 3]),
                    GetIndicatorValue(_titles[1, 5], _arLight_FrmIndicator_Value[1, 4])
                };
            }
            logger.Info(messageLogNameParam);
            foreach (string s in messageLogValueParam)
                logger.Info($"{s}");

            //кнопка 3
            messageLogNameParam = _mainTitle[2];
            messageLogValueParam = new string[]
            {
                GetIndicatorValue(_titles[2,0], _arLight_Value[2, 0]),
                GetIndicatorValue(_titles[2,1], _arLight_Value[2, 1]),
                GetIndicatorValue(_titles[2,2], _arLight_Value[2, 2]),
                GetIndicatorValue(_titles[2,3], _arLight_Value[2, 3]),
                GetIndicatorValue("Память 1 (КПК2)", _arLight_FrmIndicator_Value[2, 0]),
                GetIndicatorValue("Память 2 (КПК2)", _arLight_FrmIndicator_Value [2, 1]),
                GetIndicatorValue("БВК вкл (КПК2)", _arLight_FrmIndicator_Value[2, 2]),
                GetIndicatorValue("полет (КПК2)", _arLight_FrmIndicator_Value[2, 3])
            };
            logger.Info(messageLogNameParam);
            foreach (string s in messageLogValueParam)
                logger.Info($"{s}");

            //кнопка 4
            messageLogNameParam = _mainTitle[3];
            if (_isDiss32)
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[3, 0], _arLight_Value[3, 0]),
                    GetIndicatorValue(_titles[3, 1], _arLight_Value[3, 1]),
                    GetIndicatorValue(_titles[3, 2], _arLight_Value[3, 2]),
                    GetIndicatorValue(_titles[3, 3], _arLight_Value[3, 3]),
                    GetIndicatorValue("БВК вкл (КПК2)", _arLight_FrmIndicator_Value[3, 0]),
                    GetIndicatorValue("Исправность (КПК2)", _arLight_FrmIndicator_Value[3, 1]),
                    GetIndicatorValue("Полет (КПК2)", _arLight_FrmIndicator_Value[3, 2])
                };
            }
            else
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[3, 0], _arLight_Value[3, 0]),
                    GetIndicatorValue(_titles[3, 1], _arLight_Value[3, 1]),
                    GetIndicatorValue(_titles[3, 2], _arLight_Value[3, 2]),
                    GetIndicatorValue(_titles[3, 3], _arLight_Value[3, 3]),
                    GetIndicatorValue("БВК вкл (КПК2)", _arLight_FrmIndicator_Value[3, 0]),
                    GetIndicatorValue("Исправность (КПК2)", _arLight_FrmIndicator_Value[3, 1]),
                    GetIndicatorValue("Полет (КПК2)", _arLight_FrmIndicator_Value[3, 2]),
                    GetIndicatorValue(_titles[3, 4], _arLight_FrmIndicator_Value[3, 3]),
                    GetIndicatorValue(_titles[3, 5], _arLight_FrmIndicator_Value[3, 4]),
                };
            }
            logger.Info(messageLogNameParam);
            foreach (string s in messageLogValueParam)
                logger.Info($"{s}");

            //кнопка 5
            messageLogNameParam = _mainTitle[4];
            if (_isDiss32)
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[4, 0], _arLight_Value[4, 0]),
                    GetIndicatorValue(_titles[4, 1], _arLight_Value[4, 1]),
                    GetIndicatorValue(_titles[4, 2], _arLight_Value[4, 2]),
                    GetIndicatorValue(_titles[4, 3], _arLight_Value[4, 3]),
                    GetIndicatorValue(_titles[4, 4], _arLight_Value[4, 4]),
                    GetIndicatorValue("Навигация (КПК2)", _arLight_FrmIndicator_Value[4, 0]),
                    GetIndicatorValue("БВК (КПК2)", _arLight_FrmIndicator_Value[4, 1]),
                    GetIndicatorValue("Исправность (КПК2)", _arLight_FrmIndicator_Value[4, 2]),
                    GetIndicatorValue("Полет (КПК2)", _arLight_FrmIndicator_Value[4, 3])
                };
            }
            else
            {
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[4, 0], _arLight_Value[4, 0]),
                    GetIndicatorValue(_titles[4, 1], _arLight_Value[4, 1]),
                    GetIndicatorValue(_titles[4, 2], _arLight_Value[4, 2]),
                    GetIndicatorValue(_titles[4, 3], _arLight_Value[4, 3]),
                    GetIndicatorValue(_titles[4, 4], _arLight_Value[4, 4]),
                    GetIndicatorValue("Навигация (КПК2)", _arLight_FrmIndicator_Value[4, 0]),
                    GetIndicatorValue("БВК (КПК2)", _arLight_FrmIndicator_Value[4, 1]),
                    GetIndicatorValue("Исправность (КПК2)", _arLight_FrmIndicator_Value[4, 2]),
                    GetIndicatorValue("Полет (КПК2)", _arLight_FrmIndicator_Value[4, 3]),
                    GetIndicatorValue(_titles[4, 5], _arLight_FrmIndicator_Value[4, 4]),
                    GetIndicatorValue(_titles[4, 6], _arLight_FrmIndicator_Value[4, 5]),
                };
            }
            logger.Info(messageLogNameParam);
            foreach (string s in messageLogValueParam)
                logger.Info($"{s}");
            //кнопка 6
            if (!_isDiss32)
            {
                messageLogNameParam = _mainTitle[5];
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue("Исправность БВК", _arLight_Value[5,0]),
                    GetIndicatorValue(_titles[5,0], _arLight_FrmIndicator_Value[5, 0]),
                    GetIndicatorValue(_titles[5,1], _arLight_FrmIndicator_Value[5, 1]),
                };
                logger.Info(messageLogNameParam);
                foreach (string s in messageLogValueParam)
                    logger.Info($"{s}");
            }

            //кнопка 7
            if (!_isDiss32)
            {
                messageLogNameParam = _mainTitle[6];
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[6,0], _arLight_FrmIndicator_Value[6, 0])
                };
                logger.Info(messageLogNameParam);
                foreach (string s in messageLogValueParam)
                    logger.Info($"{s}");
            }

            //кнопка 8
            if (!_isDiss32)
            {
                messageLogNameParam = _mainTitle[7];
                messageLogValueParam = new string[]
                {
                    GetIndicatorValue(_titles[7,0], _arLight_FrmIndicator_Value[7, 0]),
                    GetIndicatorValue(_titles[7,0], _arLight_Value[7, 0])
                };
                logger.Info(messageLogNameParam);
                foreach (string s in messageLogValueParam)
                    logger.Info($"{s}");
            }
        }

        private string GetIndicatorValue(string name, bool? value)
        {
            if (value == null)
                return $"{name}: Нет выбора индикации";
            return name + ": " + (value.Value ? "Есть индикация" : "Нет индикации");
        }

        private void FillArrays()
        {
            //кнопка 1
            _arLight_Mask[0, 0] = true; //ВЧ
            _arLight_Mask[0, 1] = true; //ВСС
            _arLight_Mask[0, 2] = true; // Поиск
            _arLight_Mask[0, 3] = true; // П
            _arLight_FrmIndicator_Mask[0, 0] = true; //память 1
            _arLight_FrmIndicator_Mask[0, 1] = true; //память 2
            _arLight_FrmIndicator_Mask[0, 2] = true; //отказ ВЧ
            _arLight_FrmIndicator_Mask[0, 3] = true; //отказ ВСС

            //кнопка 2
            _arLight_Mask[1, 0] = true; // Полёт
            _arLight_Mask[1, 1] = true; // Поиск
            _arLight_Mask[1, 2] = true; // табло П
            _arLight_Mask[1, 3] = true; // ВЫК блок 6
            _arLight_FrmIndicator_Mask[1, 0] = true; //память 1
            _arLight_FrmIndicator_Mask[1, 1] = true; //память 2
            _arLight_FrmIndicator_Mask[1, 2] = true; //полет
            _arLight_FrmIndicator_Mask[1, 3] = false; // Счисл. ДИСС
            _arLight_FrmIndicator_Mask[1, 4] = true; // Счисл. ДПСМ

            //кнопка 3
            _arLight_Mask[2, 0] = true; // Контр.ДИСС
            _arLight_Mask[2, 1] = true; // Поиск
            _arLight_Mask[2, 2] = true; // Табло П
            _arLight_Mask[2, 3] = true; // ВЫК блок 6
            _arLight_FrmIndicator_Mask[2, 0] = true; //память 1
            _arLight_FrmIndicator_Mask[2, 1] = true; //память 2
            _arLight_FrmIndicator_Mask[2, 2] = true; //БПК вкл для КОНТР ДИСС
            _arLight_FrmIndicator_Mask[2, 3] = true; //полет


            //кнопка 4
            _arLight_Mask[3, 0] = true; // Контр.ДИСС
            _arLight_Mask[3, 1] = true; // Исправность
            _arLight_Mask[3, 2] = false; // Табло П
            _arLight_Mask[3, 3] = true; // вперед 17
            _arLight_FrmIndicator_Mask[3, 0] = true; //БПК ДЛЯ КОНТР ДИСС
            _arLight_FrmIndicator_Mask[3, 1] = true; //Исправность
            _arLight_FrmIndicator_Mask[3, 2] = true; //полет
            _arLight_FrmIndicator_Mask[3, 3] = true; // Счисл. ДИСС
            _arLight_FrmIndicator_Mask[3, 4] = false; // Счисл. ДПСМ 

            //кнопка 5
            _arLight_Mask[4, 0] = true; // Контр.ДИСС
            _arLight_Mask[4, 1] = true; // Исправность
            _arLight_Mask[4, 2] = false; // Табло П
            _arLight_Mask[4, 3] = true; // скорость
            _arLight_Mask[4, 4] = false; // блок 6
            _arLight_FrmIndicator_Mask[4, 0] = true;// Навигация 
            _arLight_FrmIndicator_Mask[4, 1] = true; //БПК ДЛЯ КОНТР ДИСС
            _arLight_FrmIndicator_Mask[4, 2] = true; //исправность
            _arLight_FrmIndicator_Mask[4, 3] = true; //полет
            _arLight_FrmIndicator_Mask[4, 4] = true; // Счисл. ДИСС
            _arLight_FrmIndicator_Mask[4, 5] = false; // Счисл. ДПСМ

            //кнопка 6
            _arLight_FrmIndicator_Mask[5, 0] = true; // Счисл. ДИСС
            _arLight_FrmIndicator_Mask[5, 1] = false; // Счисл. ДПСМ
            _arLight_Mask[5, 0] = true;

            //кнопка 7
            _arLight_FrmIndicator_Mask[6, 0] = false; // Откл. Коорд.
                                                      
            //кнопка 8
            _arLight_FrmIndicator_Mask[7, 0] = false; // Испр. БВК
            _arLight_Mask[7, 0] = true; // БВК

        }

        /// <summary>
        /// Заполняем строчки для подписей
        /// </summary>
        private void FillTitles()
        {
            _mainTitle = new string[btnNum];
            _mainTitleMemorize = new string[btnNumMemorize];
            _titles = new string[btnNum, gbNum];
            _titlesMemorize = new string[btnNumMemorize, gbNum];
            //первая кнопка
            _mainTitle[0] = string.Format(Resources.TakeOffCorrector, Resources.HVOff);
            _titles[0, 0] = string.Format(Resources.NOnN, Resources.VCh, Resources.BPK);
            _titles[0, 1] = string.Format(Resources.NOnN, Resources.VSS, Resources.BPK);
            _titles[0, 2] = string.Format(Resources.NOnN, Resources.Search, Resources.BPK);
            _titles[0, 3] = string.Format(Resources.NOnN, Resources.P, string.Format(Resources.US, string.Format(Resources.PS, "")));
            //вторая кнопка
            _mainTitle[1] = string.Format(Resources.TakeOnCorrector, Resources.HVOn);
            _titles[1, 0] = string.Format(Resources.NOnN, Resources.Flight, Resources.BPK);
            _titles[1, 1] = string.Format(Resources.NOnN, Resources.Search, Resources.BPK);
            _titles[1, 2] = string.Format(Resources.NOnN, Resources.P,
                                          string.Format(Resources.US, string.Format(Resources.PS, "")));
            _titles[1, 3] = "ВЫК на блоке 6";
            //if (!_isDiss32)
            //{
            //    _titles[1, 4] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
            //    _titles[1, 5] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
            //}

            //третья кнопка
            _mainTitle[2] = Resources.PushContrDiss;
            _titles[2, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
            _titles[2, 1] = string.Format(Resources.NOnN, Resources.Search, Resources.BPK);
            _titles[2, 2] = string.Format(Resources.NOnN, Resources.P, string.Format(Resources.US, string.Format(Resources.PS, "")));
            _titles[2, 3] = "ВЫК на блоке 6";

            // четвертая кнопка
            _mainTitle[3] = Resources.LightInstruction1;
            _titles[3, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
            _titles[3, 1] = string.Format(Resources.NOnN, Resources.CorrectWork, Resources.BPK);
            _titles[3, 2] = string.Format(Resources.NOnN, Resources.P,
                                          string.Format(Resources.US, string.Format(Resources.PS, "")));
            _titles[3, 3] = "ВПЕРЕД 17… на БПК";
            //if (!_isDiss32)
            //{
            //    _titles[3, 4] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
            //    _titles[3, 5] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
            //}

            // пятая кнопка
            _mainTitle[4] = Resources.LightInstruction2;
            _titles[4, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
            _titles[4, 1] = string.Format(Resources.NOnN, Resources.CorrectWork, Resources.BPK);
            _titles[4, 2] = string.Format(Resources.NOnN, Resources.P, string.Format(Resources.US, string.Format(Resources.PS, "")));
            _titles[4, 3] = "СКОРОСТЬ 258… на БПК";
            _titles[4, 4] = "ВЫК на блоке 6";
            //if (!_isDiss32)
            //{
            //    _titles[4, 5] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
            //    _titles[4, 6] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
            //}
            
            // шестая кнопка
            _mainTitle[5] = Resources.LightInstruction3;
            _titles[5, 0] = "\"Исправность\" на БПК";
            //_titles[5, 0] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
            //_titles[5, 1] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
            
            // седьмая кнопка
            _mainTitle[6] = string.Format(Resources.PressButtonOnBlock8, Resources.On);
            //_titles[6, 0] = string.Format(Resources.NOnN, Resources.CoordsOff, Resources.ApplicationName);
            
            // восьмая кнопка
            _mainTitle[7] = string.Format(Resources.PressButtonAndCheck, btn_Refuse_BVK.Text);
            //_titles[7, 0] = string.Format(Resources.NOnN, Resources.RightWorkBVK, Resources.ApplicationName);
            _titles[7, 0] = string.Format(Resources.NOnN, Resources.BVK, Resources.BPK);
            
            // девятая кнопка
            _mainTitle[8] = string.Format(Resources.SetMapCorner, _mainButton);

            // Срабатывание запоминания

            //первая кнопка
            _mainTitleMemorize[0] = Res.WaitUSPSAndCheck;

            _titlesMemorize[0, 0] = string.Format(Resources.NOnN, Resources.Flight, Resources.BPK);
            _titlesMemorize[0, 1] = string.Format(Resources.NOnN, Resources.CorrectWork, Resources.BPK);
        }

        private void rbtn_Click(object sender, EventArgs e)
        {
            int row = -1, col;
            bool state;
            RadioButton rbCurrent = (RadioButton)sender;
            col = Convert.ToInt32(rbCurrent.Name.Substring(rbCurrent.Name.IndexOf("L") + 1, 1)) - 1;
            state = rbCurrent.Name.Substring(rbCurrent.Name.Length - 1, 1) == "Y";
            foreach (Control c in grpBox_Light.Controls)
                if ((c is Button) && (c.Name.Contains("Light")))
                    if (((Button)c).FlatStyle == FlatStyle.Flat)
                    {
                        row = Convert.ToInt32(c.Name.Substring(c.Name.Length - 1, 1)) - 1;
                        break;
                    }
            if (row != -1)
                _arLight_Value[row, col] = state;
        }

        private void btn_Light_Click(object sender, EventArgs e)
        {
            if (sender.Equals(btn_Light_3))
                contrDissPressed = true;
            GenSinCos.SetSelsinRefVoltage(GenSinCos.SelsinRefVoltageType.Normal);
            Button bCurrent = (Button)sender;
            if (bCurrent.FlatStyle == FlatStyle.Standard)
            {
                if (IsCornerTaskActive)
                {
                    _dbKPK2.ManualRecordDouble("MapCorner", _frm.stateMapCorner ? 1 : 0);
                }
                CheckIndicators();
                int bNum = Convert.ToInt32(bCurrent.Name.Substring(bCurrent.Name.Length - 1, 1));
                PressButtonLight(bCurrent, MaxButtonNum, bNum);
                bNum -= 1;
                lbl_Light.Text = _mainTitle[bNum];
                Control[] cCollection;
                for (int i = 0; i < gbNum; i++)
                {
                    cCollection = Controls.Find("grpBox_Light_" + (i + 1), true);
                    if (cCollection.Length > 0)
                        if (!String.IsNullOrEmpty(_titles[bNum, i]))
                        {
                            cCollection[0].Text = _titles[bNum, i];
                            cCollection[0].Visible = true;
                        }
                        else
                            cCollection[0].Visible = false;
                }
                ResetRadioButtons();
            }
        }

        private void CheckIndicators()
        {
            if (IsBtn1)
            {
                _arLight_FrmIndicator_Value[0, 0] = _frm.TiMemory1;
                _arLight_FrmIndicator_Value[0, 1] = _frm.TiMemory2;
                _arLight_FrmIndicator_Value[0, 2] = _frm.TiVCH;
                _arLight_FrmIndicator_Value[0, 3] = _frm.TiVSS;
            }
            if (IsBtn2)
            {
                _arLight_FrmIndicator_Value[1, 0] = _frm.TiMemory1;
                _arLight_FrmIndicator_Value[1, 1] = _frm.TiMemory2;
                _arLight_FrmIndicator_Value[1, 2] = _frm.TiFlight;
                _arLight_FrmIndicator_Value[1, 3] = _frm.TiDiss;
                _arLight_FrmIndicator_Value[1, 4] = _frm.TiDpsm;
            }
            if (IsBtn3)
            {
                _arLight_FrmIndicator_Value[2, 0] = _frm.TiMemory1;
                _arLight_FrmIndicator_Value[2, 1] = _frm.TiMemory2;
                _arLight_FrmIndicator_Value[2, 2] = _frm.TiBPKOn;
                _arLight_FrmIndicator_Value[2, 3] = _frm.TiFlight;
            }
            if (IsBtn4)
            {
                _arLight_FrmIndicator_Value[3, 0] = _frm.TiBPKOn;
                _arLight_FrmIndicator_Value[3, 1] = _frm.TiGood;
                _arLight_FrmIndicator_Value[3, 2] = _frm.TiFlight;
                _arLight_FrmIndicator_Value[3, 3] = _frm.TiDiss;
                _arLight_FrmIndicator_Value[3, 4] = _frm.TiDpsm;
            }
            if (IsBtn5)
            {
                _arLight_FrmIndicator_Value[4, 0] = _frm.TiNavigation;
                _arLight_FrmIndicator_Value[4, 1] = _frm.TiBPKOn;
                _arLight_FrmIndicator_Value[4, 2] = _frm.TiGood;
                _arLight_FrmIndicator_Value[4, 3] = _frm.TiFlight;
                _arLight_FrmIndicator_Value[4, 4] = _frm.TiDiss;
                _arLight_FrmIndicator_Value[4, 5] = _frm.TiDpsm;
            }
            if (IsBtn6)
            {
                _arLight_FrmIndicator_Value[5, 0] = _frm.TiDiss;
                _arLight_FrmIndicator_Value[5, 1] = _frm.TiDpsm;
            }
            if (IsBtn7)
            {
                _arLight_FrmIndicator_Value[6, 0] = _frm.TiPositionOff;
            }
            if (IsBtn8)
            {
                _arLight_FrmIndicator_Value[7, 0] = _frm.TiBVK;
            }
        }

        private void PressButtonLight(Button btn, int maxNum, int bNum)
        {

            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(153, 249, 164);
            string bBaseName;
            bBaseName = btn.Name.Substring(0, btn.Name.Length - 1);
            //int bNum = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1, 1));
            Control[] result = Controls.Find(bBaseName + ((bNum + 1) > maxNum ? 1 : (bNum + 1)), true);
            if (result.Length > 0)
                result[0].Enabled = true;
            result = Controls.Find(bBaseName + ((bNum - 1) < 1 ? maxNum : (bNum - 1)), true);
            if (result.Length > 0)
                ResetButton((Button)result[0]);
        }

        public bool IsCornerTaskActive
        {
            get { return btn_Light_9.FlatStyle == FlatStyle.Flat; }
        }

        private void ResetRadioButtons()
        {
            rbtn_L1_Y.Checked =
                      rbtn_L2_Y.Checked =
                      rbtn_L3_Y.Checked =
                      rbtn_L4_Y.Checked =
                      rbtn_L5_Y.Checked =
                      rbtn_L1_N.Checked =
                      rbtn_L2_N.Checked =
                      rbtn_L3_N.Checked =
                      rbtn_L4_N.Checked =
                      rbtn_L5_N.Checked = false;
        }

        public void HideLastButton()
        {
            btn_Light_9.Visible = false;
            MaxButtonNum--;
        }

        /// <summary>
        /// Сбрасывает кнопку в исходное состояние
        /// </summary>
        /// <param name="bCurrent"></param>
        private void ResetButton(Button bCurrent)
        {
            bCurrent.Enabled = false;
            bCurrent.BackColor = Color.Transparent;
            bCurrent.FlatStyle = FlatStyle.Standard;
        }

        private void btn_Light_BVK_Click(object sender, EventArgs e)
        {
            // Подать команду "Отказ БВК"
            DO176X.Instance.SetBVKRefuse(true);
            btn_Refuse_BVK.BackColor = Color.FromArgb(153, 249, 164);
            btn_Refuse_BVK.FlatStyle = FlatStyle.Flat;
            btn_Refuse_BVK.Enabled = false;
        }

        // ВПЕРЕД - КЗ-1
        private void btn_Light_T1_Click(object sender, EventArgs e)
        {
            if (_measControl.TaskOn("Task_W_1_32", "Task_1_32"))
            {
                btn_Forward.BackColor = Color.FromArgb(153, 249, 164);
                btn_Forward.FlatStyle = FlatStyle.Flat;
                btn_Forward.Enabled = false;
            }
        }

        /// <summary>
        /// Записываем в БД значения "Световой сигнализации" 
        /// </summary>
        public void WriteToDbLight()
        {
            bool?[] resultState = new bool?[_arLight_Name.Length];
            for (int i = 0; i < resultState.Length; i++)
                resultState[i] = true;
            for (int i = 0; i < btnNum; i++)
                for (int j = 0; j < gbNum; j++)
                    if (_arLight_Value[i, j] != _arLight_Mask[i, j])
                    {
                        if (_arLight_Value[i, j] == null)
                            resultState[GetParamId(i, j)] = null;
                        else
                            resultState[GetParamId(i, j)] = false;
                    }


            for (int i = 0; i < btnNumIndicatorFrm; i++)
                for (int j = 0; j < indicatorFrm; j++)
                {
                    if (_arLight_FrmIndicator_Mask[i, j] != _arLight_FrmIndicator_Value[i, j])
                    {
                        int id = GetParamIdfrm(i, j);
                        if (id == 100)
                        {
                            continue;
                        }
                        if (_arLight_Value[i, j] == null)
                        {
                            resultState[GetParamIdfrm(i, j)] = null;
                        }
                        else
                            resultState[GetParamIdfrm(i, j)] = false;
                    }
                }

            for (int i = 0; i < resultState.Length; i++)
                _dbKPK2.ManualRecordDouble(_arLight_Name[i], resultState[i] == null ? -1 : resultState[i] == true ? 1 : 0);

            if (_isDiss32)
            {
                if (LastCheckMemorizePassed)
                    _dbKPK2.ManualRecordDouble(MemorizeParam, _memorizeResult ? 1 : 0);
            }

        }

        private static int GetParamId(int i, int j)
        {

            switch (i)
            {
                //первая кнопка
                case 0:
                    switch (j)
                    {
                        case 0:
                            return 0;
                        case 1:
                            return 1;
                        case 2:
                            return 2;
                        case 3:
                            return 3;
                    }
                    break;
                //вторая кнопка
                case 1:
                    switch (j)
                    {
                        case 0:
                            return 4;
                        case 1:
                            return 2;
                        case 2:
                            return 3;
                        case 3:
                            return 2;
                        case 4:
                            return 7;
                        case 5:
                            return 8;
                    }
                    break;
                //третья кнопка
                case 2:
                    switch (j)
                    {
                        case 0:
                            return 5;
                        case 1:
                            return 2;
                        case 2:
                            return 3;
                        case 3:
                            return 2;
                    }
                    break;
                // четвертая кнопка
                case 3:
                    switch (j)
                    {
                        case 0:
                            return 5;
                        case 1:
                            return 6;
                        case 2:
                            return 3;
                        case 3:
                            return 14;
                        case 4:
                            return 7;
                        case 5:
                            return 8;
                    }

                    break;
                // пятая кнопка
                case 4:
                    switch (j)
                    {
                        case 0:
                            return 5;
                        case 1:
                            return 6;
                        case 2:
                            return 3;
                        case 3:
                            return 15;
                        case 4:
                            return 2;
                        case 5:
                            return 7;
                        case 6:
                            return 8;
                    }
                    break;
                // шестая кнопка
                case 5:
                    switch (j)
                    {
                        case 0:
                            return 7;
                        case 1:
                            return 8;
                    }
                    break;
                // седьмая кнопка
                case 6:
                    return 10;
                // восьмая кнопка
                case 7:
                    return 9;
            }
            return 100;
        }

        private static int GetParamIdfrm(int numberBtn, int numType)
        {
            switch (numberBtn)
            {
                case 0://кнопка 1
                    switch (numType)
                    {
                        case 0:
                            return 12;
                        case 1:
                            return 13;
                        case 2:
                            return 0;
                        case 3:
                            return 1;
                    }
                    break;
                case 1://кнопка 2
                    switch (numType)
                    {
                        case 0:
                            return 12;
                        case 1:
                            return 13;
                        case 2:
                            return 4;
                    }
                    break;
                case 2: //кнопка 3
                    switch (numType)
                    {
                        case 0:
                            return 12;
                        case 1:
                            return 13;
                        case 2:
                            return 5;
                        case 3:
                            return 4;
                    }
                    break;
                case 3: //кнопка 4
                    switch (numType)
                    {
                        case 0:
                            return 5;
                        case 1:
                            return 6;
                        case 2:
                            return 4;
                    }
                    break;
                case 4: //кнопка 5
                    switch (numType)
                    {
                        case 0:
                            return 11;
                        case 1:
                            return 5;
                        case 2:
                            return 6;
                        case 3:
                            return 4;
                    }
                    break;
            }
            return 100;
        }

        private void grpBox_Light_Enter(object sender, EventArgs e)
        {

        }

        private void RepairLightControl_Load(object sender, EventArgs e)
        {
            btn_Light_6.Visible = btn_Light_7.Visible =
             btn_Light_8.Visible = btn_Light_9.Visible = btn_Forward.Visible = btn_Refuse_BVK.Visible = !_isDiss32;

            lbl_Light.Text = "";
            for (int i = 1; i < 6; i++)
            {
                Control[] lst = Controls.Find("grpBox_Light_" + i, true);
                if (lst.Length > 0)
                    lst[0].Visible = false;
            }
            for (int i = 2; i < 10; i++)
            {
                Control[] lst = Controls.Find("btn_Light_" + i, true);
                if (lst.Length > 0)
                    lst[0].Enabled = false;
            }
        }

        private bool contrDissPressed = false;
        public void UnpressContrDiss()
        {
            if (contrDissPressed)
            {
                MessageBox.Show(Sparc.KPK2.Resources.Properties.Resources.UnpressContrDiss);
                contrDissPressed = false;
            }
        }

        private bool _memorizeResult = true;

        private const ushort _baseAngle = 11;
        private const ushort _maxBaseAngle = 30;

        private void lbl_Light_Click(object sender, EventArgs e)
        {
            if (!_isDiss32)
                return;
            var frm = new fSetValue(_baseAngle, _maxBaseAngle, _baseAngle, SetNewAngle);
            frm.ShowDialog();
            lbl_Light.Text = string.Format("{0} {1}", string.Format(Res.PitchAngleNApplied, _newAngle * -1), string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 4));
        }

        private ushort _newAngle;

        private void SetNewAngle(int newAngle)
        {
            _newAngle = (ushort)newAngle;
            GenSinCos.Instance().GeneratorOn((ushort)(360 - _newAngle));
        }
    }
}

