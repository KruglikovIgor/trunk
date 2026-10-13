using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using KPK2.Common.DissFunctions;
using KPK2.Interfaces;
using KPK2.DB;
using KPK2.GeneralForms.AddForms;
using KPK2.GeneralForms.DissForms;
using KPK2.MyControl.DISS.FrmDiss32;
using KPK2.Properties;
using Microsoft.SqlServer.Server;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class LightControl : UserControl
  {
    private DataBaseKPK2 _dbKPK2;
    private IFrmWork32 _frm;
    private IMeasurementsControl _measControl;
    private string[] _mainTitle;
    private string[] _mainTitleMemorize;
    private string[,] _titles;
    private string[,] _titlesMemorize;
    private const int gbNum = 5;
    private const int btnNum = 9;
    private const int btnNumMemorize = 10;
    private bool _isDiss32;
    private TimeSpan _waitReadySignal = TimeSpan.FromSeconds(15);

    private bool?[,] _arLight_Mask = new bool?[btnNum,gbNum];
    private bool?[,] _arLight_Value = new bool?[btnNum,gbNum];
    int MaxButtonNum = 9;

    public bool LastCheckMemorizePassed
    {
      get { return _lastCheckPassed; }
    }

    public bool CheckMemorizeResult
    {
      get { return _memorizeResult; }
    }

    private string MemorizeParam = "VisualControlMemorize";

    private string[] _arLight_Name = new[]
      {
        "Refuse_VCh", "Refuse_VSS", "Search",
        "Display_P", "Flight", "Control_Diss", "GoodCondition", "CalcDiss", "CalcDPSM",
        "Refuse_BVK", "PositionOff",
		"ModeNavigation_WorkTest", "ModeMemory_WorkTest"
	  };

    private string _mainButton;

    public LightControl(IFrmWork32 frm,IMeasurementsControl measControl, DataBaseKPK2 dbKpk2, bool isDiss32, string mainButton)
    {
        _mainButton = mainButton;
        _frm = frm;
        _measControl = measControl;
      _isDiss32 = isDiss32;
      if (_isDiss32)
        MaxButtonNum = 6;
      _dbKPK2 = dbKpk2;
      InitializeComponent();
      if (_isDiss32)
        gbMemorize.Visible = true;
      FillTitles();
      FillArrays();
    }

    private void FillArrays()
    {
      //кнопка 1
      _arLight_Mask[0, 0] = true; //ВЧ
      _arLight_Mask[0, 1] = true; //ВСС
      _arLight_Mask[0, 2] = true; // Поиск
      _arLight_Mask[0, 3] = true; // П

      //кнопка 2
      _arLight_Mask[1, 0] = true; // Полёт
      _arLight_Mask[1, 1] = true; // Поиск
      _arLight_Mask[1, 2] = true; // П
      _arLight_Mask[1, 3] = false; // Счисл. ДИСС
      _arLight_Mask[1, 4] = true; // Счисл. ДПСМ
      //кнопка 3
      _arLight_Mask[2, 0] = true; // Контр.ДИСС
      _arLight_Mask[2, 1] = true; // Поиск
      _arLight_Mask[2, 2] = true; // Табло П
      if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
      {
          gbMemorize.Visible = false;
      }
      

      //кнопка 4
      _arLight_Mask[3, 0] = true; // Контр.ДИСС
      _arLight_Mask[3, 1] = true; // Исправность
      _arLight_Mask[3, 2] = false; // Табло П
      _arLight_Mask[3, 3] = true; // Счисл. ДИСС
      _arLight_Mask[3, 4] = false; // Счисл. ДПСМ            
      //кнопка 5
      _arLight_Mask[4, 0] = true; // Контр.ДИСС
      _arLight_Mask[4, 1] = true; // Исправность
      _arLight_Mask[4, 2] = false; // Табло П
      _arLight_Mask[4, 3] = true; // Счисл. ДИСС
      _arLight_Mask[4, 4] = false; // Счисл. ДПСМ            

      //кнопка 6
      _arLight_Mask[5, 0] = true; // Счисл. ДИСС
      _arLight_Mask[5, 1] = false; // Счисл. ДПСМ
      //кнопка 7
      _arLight_Mask[6, 0] = false; // Откл. Коорд.
      //кнопка 8
      _arLight_Mask[7, 0] = false; // Испр. БВК
      _arLight_Mask[7, 1] = true; // БВК

    }

    /// <summary>
    /// Заполняем строчки для подписей
    /// </summary>
    private void FillTitles()
    {
      _mainTitle = new string[btnNum];
      _mainTitleMemorize = new string[btnNumMemorize];
      _titles = new string[btnNum,gbNum];
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
      if (!_isDiss32)
      {
        _titles[1, 3] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
        _titles[1, 4] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
      }
      //третья кнопка
      _mainTitle[2] = Resources.PushContrDiss;
      _titles[2, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
      _titles[2, 1] = string.Format(Resources.NOnN, Resources.Search, Resources.BPK);
      _titles[2, 2] = string.Format(Resources.NOnN, Resources.P, string.Format(Resources.US, string.Format(Resources.PS, "")));

      // четвертая кнопка
      _mainTitle[3] = Resources.LightInstruction1;
      _titles[3, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
      _titles[3, 1] = string.Format(Resources.NOnN, Resources.CorrectWork, Resources.BPK);
      _titles[3, 2] = string.Format(Resources.NOnN, Resources.P,
                                    string.Format(Resources.US, string.Format(Resources.PS, "")));
      if (!_isDiss32)
      {
        _titles[3, 3] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
        _titles[3, 4] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
      }
      // пятая кнопка
      _mainTitle[4] = Resources.LightInstruction2;
      _titles[4, 0] = string.Format(Resources.NOnN, Resources.ControlDiss, Resources.BPK);
      _titles[4, 1] = string.Format(Resources.NOnN, Resources.CorrectWork, Resources.BPK);
      _titles[4, 2] = string.Format(Resources.NOnN, Resources.P, string.Format(Resources.US, string.Format(Resources.PS, "")));
      if (!_isDiss32)
      {
        _titles[4, 3] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
        _titles[4, 4] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
      }
      // шестая кнопка
      _mainTitle[5] = Resources.LightInstruction3;
      _titles[5, 0] = string.Format(Resources.NOnN, Resources.CalcDiss, Resources.ApplicationName);
      _titles[5, 1] = string.Format(Resources.NOnN, Resources.CalcDPSM, Resources.ApplicationName);
      // седьмая кнопка
      _mainTitle[6] = string.Format(Resources.PressButtonOnBlock8, Resources.On);
      _titles[6, 0] = string.Format(Resources.NOnN, Resources.CoordsOff, Resources.ApplicationName);
      // восьмая кнопка
      _mainTitle[7] = string.Format(Resources.PressButtonAndCheck, btn_Refuse_BVK.Text);
      _titles[7, 0] = string.Format(Resources.NOnN, Resources.RightWorkBVK, Resources.ApplicationName);
      _titles[7, 1] = string.Format(Resources.NOnN, Resources.BVK, Resources.BPK);
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
      RadioButton rbCurrent = (RadioButton) sender;
      col = Convert.ToInt32(rbCurrent.Name.Substring(rbCurrent.Name.IndexOf("L") + 1, 1)) - 1;
      state = rbCurrent.Name.Substring(rbCurrent.Name.Length - 1, 1) == "Y";
      foreach (Control c in grpBox_Light.Controls)
        if ((c is Button) && (c.Name.Contains("Light")))
          if (((Button) c).FlatStyle == FlatStyle.Flat)
          {
            row = Convert.ToInt32(c.Name.Substring(c.Name.Length - 1, 1)) - 1;
            break;
          }
      if (row != -1)
        _arLight_Value[row, col] = state;
    }

    private void btn_Light_Click(object sender, EventArgs e)
    {
      RemoveDiss32();
        if (sender.Equals(btn_Light_3))
            contrDissPressed = true;
      GenSinCos.SetSelsinRefVoltage(GenSinCos.SelsinRefVoltageType.Normal);
      Button bCurrent = (Button) sender;
      if (bCurrent.FlatStyle == FlatStyle.Standard)
      {
        if (IsCornerTaskActive)
          _dbKPK2.ManualRecordDouble("MapCorner", _frm.stateMapCorner ? 1 : 0);
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

    private void RemoveDiss32()
    {
      if (_isDiss32)
        if (_memIndState != null)
          grpBox_Light_1.Parent.Controls.Remove(_memIndState);
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

    private void PressButtonLight(Button btn, int maxNum, int bNum)
    {
      
      btn.FlatStyle = FlatStyle.Flat;
      btn.BackColor = Color.FromArgb(153, 249, 164);
      string bBaseName;
      if (btn.Name == bMemorize10.Name)
        bBaseName = btn.Name.Substring(0, btn.Name.Length - 2);
      else
        bBaseName = btn.Name.Substring(0, btn.Name.Length - 1);
      //int bNum = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1, 1));
      Control[] result = Controls.Find(bBaseName + ((bNum + 1) > maxNum ? 1 : (bNum + 1)), true);
      if (result.Length > 0)
        result[0].Enabled = true;
      result = Controls.Find(bBaseName + ((bNum - 1) < 1 ? maxNum : (bNum - 1)), true);
      if (result.Length > 0)
        ResetButton((Button)result[0]);
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

        // Запись основных параметров светосигнализации (всегда)
        for (int i = 0; i < resultState.Length; i++)
            _dbKPK2.ManualRecordDouble(_arLight_Name[i],
                resultState[i] == null ? -1 : (resultState[i] == true ? 1 : 0));

        // Запись параметра "VisualControlMemorize" – только для ДИСС‑32, но всегда
        if (_isDiss32)
        {
            StopMemorize();
            //MessageBox.Show($"_memorizeResult= {_memorizeResult}\n LastCheckMemorizePassed= {LastCheckMemorizePassed}");
            int value = (LastCheckMemorizePassed && _memorizeResult) ? 1 : 0;
            _dbKPK2.ManualRecordDouble(MemorizeParam, value);

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
              return 7;
            case 4:
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
	            return 11;
            case 4:
	            return 1;
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
              return 7;
            case 4:
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
              return 7;
            case 4:
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

    private void grpBox_Light_Enter(object sender, EventArgs e)
    {

    }

    private void LightControl_Load(object sender, EventArgs e)
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

      for (int i = 2; i < 10; i++)
      {
        Control[] lst = Controls.Find("bMemorize" + i, true);
        if (lst.Length > 0)
          lst[0].Enabled = false;
      }
    }

    private void bMemorize1_Click(object sender, EventArgs e)
    {
      Button bCurrent = (Button) sender;
        if (bCurrent == bMemorize1)
            UnpressContrDiss();
      GenSinCos.SetSelsinRefVoltage(GenSinCos.SelsinRefVoltageType.Low);
      if (bCurrent.FlatStyle == FlatStyle.Standard)
      {
        int bNum = Convert.ToInt32(bCurrent.Name.Substring(bCurrent.Name.Length - 1, 1));
        if (bCurrent.Name == bMemorize10.Name)
          bNum = 10;
        PressButtonLight(bCurrent, btnNumMemorize, bNum);
        bNum -= 1;
        //lbl_Light.Text = _mainTitleMemorize[bNum];
        SetMemorizeStep(bNum+1);
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

    private void SetMemorizeStep(int bNum)
    {
      bool result = true;
      switch (bNum)
      {
        case 0:
          StopMemorize();
          break;
        case 1:
          result = CheckMemorize1();
          break;
        case 2:
          result = CheckMemorize2();
          break;
        case 3:
          result = CheckMemorize3();
          break;
        case 4:
          result = CheckMemorize4();
          break;
        case 5:
          result = CheckMemorize5();
          break;
        case 6:
          CheckMemorize6();
          break;
        case 7:
          result = CheckMemorize7();
          break;
        case 8:
          result = CheckMemorize8();
          break;
        case 9:
          result = CheckMemorize9();
          break;
        case 10:
          result = CheckMemorize10();
          break;
      }
      if (!result)
        StopMemorize();
    }

    private bool _memorizeResult = true;

    private bool CheckMemorize1()
    {
        Imitator.Diss32.PushSearchButton(false);
      if (_lastCheckPassed)
        StopMemorize();
      _memorizeResult = true;
      btn_Light_1.Enabled = false;
      ImageMemorizeControl();
      if (!Diss32Functions.Task3On())
      {
        MessageBox.Show(Sparc.KPK2.Resources.Properties.Resources.SetTaskError);
        return false;
      }
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.KZNSetted, 3), Res.WaitUSPSReady, string.Format(Res.MarkSignalsBPK_6AndPressButtonN, 2));
      bMemorize2.Focus();
      return true;
    }

    private ucIndicatorsState _memIndState;

    private void ImageMemorizeControl()
    {
      _memIndState = new ucIndicatorsState(ucIndicatorsState.Modes.ModeA);
      _memIndState.Location = grpBox_Light_1.Location;
      grpBox_Light_1.Parent.Controls.Add(_memIndState);
      _memIndState.BringToFront();
    }

    private bool WaitServiceabilityReady(TimeSpan timeout)
    {
      DateTime startWait = DateTime.Now;
      bool result = false;
      while (!result && ((DateTime.Now -startWait) <= timeout))
      {
        Application.DoEvents();
        if (_currentStates != null)
          result = _currentStates.Serviceability;
      }
      return result;
    }

      private const ushort _baseAngle = 11;
      private const ushort _maxBaseAngle = 30;

    private bool CheckMemorize2()
    {
      _memIndState.UnblockRenewIndicators();
      _memIndState.SetMode(ucIndicatorsState.Modes.ModeB);
      if (!GenSinCos.Instance().GeneratorOn(_baseAngle))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }

      if (!Diss15Functions.SetPitch())
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.PitchAngleNApplied, _baseAngle), Res.SetUSPSSwitchToMS, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 3));
      _memIndState.StartTime();
      bMemorize3.Focus();
      return true;
    }
    

    private bool CheckMemorize3()
    {
      _memIndState.UnblockRenewIndicators();
      _lastCheckPassed = false;
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.ResetIndicators();
      _memIndState.StopTime();
      short angle = _baseAngle*-1;
      if (!GenSinCos.Instance().GeneratorOn((ushort)(360 + angle)))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      lbl_Light.Text = string.Format("{0} {1}", string.Format(Res.PitchAngleNApplied, angle), string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 4));
      _memIndState.StartTime();
      bMemorize4.Focus();
      return true;
    }

    private bool CheckMemorize4()
    {
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.StopTime();
      _memIndState.SetMode(ucIndicatorsState.Modes.ModeC);
      ushort angle = 5;
      if (!GenSinCos.Instance().GeneratorOn(angle))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Imitator.Diss32.PushSearchButton(true);
      lbl_Light.Text = string.Format("{0} {1}", string.Format(Res.PitchAngleNApplied, angle), string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 5));
      _memIndState.StartTime();
      bMemorize5.Focus();
      return true;
    }

    private bool CheckMemorize5()
    {
        Imitator.Diss32.PushSearchButton(false);
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.StopTime();
      _memIndState.SetMode(ucIndicatorsState.Modes.ModeA);
      short angle = -5;
      if (!GenSinCos.Instance().GeneratorOn((ushort)(360 + angle)))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.PitchAngleNApplied, angle), Res.WaitUSPSReady, string.Format(Res.MarkSignalsBPK_6AndPressButtonN, 6));
      bMemorize6.Focus();
      return true;
    }

    private void CheckMemorize6()
    {
       Imitator.Diss32.PushSearchButton(true);
      _memIndState.UnblockRenewIndicators();
      _memIndState.SetMode(ucIndicatorsState.Modes.ModeC);
      _memIndState.StopTime();
      lbl_Light.Text = string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 7);
      _memIndState.StartTime();
      bMemorize7.Focus();
    }

    private bool CheckMemorize7()
    {
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.ResetIndicators();
      _memIndState.StopTime();
      ushort angle = 15;
      if (!GenSinCos.Instance().GeneratorOn(angle))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Diss32Functions.Task3Off();
      if (!Diss32Functions.Task1On())
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Imitator.Diss32.PushSearchButton(true);
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text = string.Format("{0} {1} {2} {3}", string.Format(Res.KZNSetted, 1), string.Format(Res.PitchAngleNApplied, angle), Res.WaitBlock6Ready, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 8));
      _memIndState.StartTime();
      bMemorize8.Focus();
      return true;
    }

    private bool CheckMemorize8()
    {
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.ResetIndicators();
      _memIndState.StopTime();
      short angle = -15;
      if (!GenSinCos.Instance().GeneratorOn((ushort)(360 + angle)))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Imitator.Diss32.PushSearchButton(true);
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.PitchAngleNApplied, angle), Res.WaitBlock6Ready, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 9));
      _memIndState.StartTime();
      bMemorize9.Focus();
      return true;
    }

    private bool CheckMemorize9()
    {
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.ResetIndicators();
      _memIndState.StopTime();
      if (!Diss32Functions.Task1Off() || !Diss32Functions.Task3On())
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Imitator.Diss32.PushSearchButton(true);
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text =  string.Format("{0} {1} {2}", string.Format(Res.KZNSetted, 3), Res.SetUSPSSwitchToMB, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 10));
      _memIndState.StartTime();
      bMemorize10.Focus();
      return true;
    }

    private bool CheckMemorize10()
    {
      _memIndState.UnblockRenewIndicators();
      _memorizeResult &= _memIndState.GetCurrentState();
      _memIndState.ResetIndicators();
      _memIndState.StopTime();
      if (!GenSinCos.Instance().GeneratorOn(15))
      {
        MessageBox.Show(Resources.Error);
        return false;
      }
      Imitator.Diss32.PushSearchButton(true);
      if (!WaitServiceabilityReady(_waitReadySignal))
        MessageBox.Show(string.Format(Res.ServiceabilityNotFoundInNSeconds, _waitReadySignal.TotalSeconds));
      lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.PitchAngleNApplied, 15), Res.WaitUSPSReady, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, _mainButton));
      _memIndState.StartTime();
      //btn_Light_1.Enabled = true;
      bMemorize1.Focus();
      return true;
    }

    private bool _lastCheckPassed;

    private void StopMemorize()
    {
      if (_memIndState != null)
      {
        _memIndState.StopTime();
        _memorizeResult &= _memIndState.GetCurrentState();
      }
      _lastCheckPassed = bMemorize10.Enabled && bMemorize10.FlatStyle == FlatStyle.Flat;
      ResetButton(bMemorize2);
      ResetButton(bMemorize3);
      ResetButton(bMemorize4);
      ResetButton(bMemorize5);
      ResetButton(bMemorize6);
      ResetButton(bMemorize7);
      ResetButton(bMemorize8);
      ResetButton(bMemorize9);
      ResetButton(bMemorize10);
      bMemorize10.Enabled = false;


      if (!GenSinCos.Instance().GeneratorOff())
        MessageBox.Show(Resources.Error);
      Diss32Functions.Task1Off();
      Diss32Functions.Task3Off();
      MessageBox.Show(Res.SetUSPSSwitchToS);
      bMemorize1.Enabled = true;
      btn_Light_1.Enabled = true;
    }

    private ucIndicatorsState.IndicatorsState _currentStates;

    public void RenewIndicators(ucIndicatorsState.IndicatorsState states)
    {
      _currentStates = states;
      if (_memIndState != null)
        _memIndState.RenewIndicators(states);
    }

    private void lbl_Light_Click(object sender, EventArgs e)
    {
        if (!_isDiss32)
            return;
        if (bMemorize2.FlatStyle != FlatStyle.Flat && bMemorize3.FlatStyle != FlatStyle.Flat)
            return;
        var frm = new fSetValue(_baseAngle, _maxBaseAngle, _baseAngle, SetNewAngle);
        frm.ShowDialog();
        if (bMemorize2.FlatStyle == FlatStyle.Flat)
            lbl_Light.Text = string.Format("{0} {1} {2}", string.Format(Res.PitchAngleNApplied, _newAngle), Res.SetUSPSSwitchToMS, string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 3));
        else
            lbl_Light.Text = string.Format("{0} {1}", string.Format(Res.PitchAngleNApplied, _newAngle*-1), string.Format(Res.PushSearchOnAndCheckIndicators, Res.IndicatorButtonTurnOnSearch, 4));
    }

    private ushort _newAngle;

    private void SetNewAngle(int newAngle)
    {
        _newAngle = (ushort)newAngle;
        if (bMemorize2.FlatStyle == FlatStyle.Flat)
            GenSinCos.Instance().GeneratorOn(_newAngle);
        else
            GenSinCos.Instance().GeneratorOn((ushort)(360 - _newAngle));
    }
  }
}

