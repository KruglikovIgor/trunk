using KPK2.Common;
using KPK2.MyClasse;
using KPK2.Properties;
using LCards;
using NLog;
using Sparc.Hardware.Advantech.PCI176X;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.Enums;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms
{
  public partial class SelfTest : Form
  {
    private Logger _logger = LogManager.GetLogger("KPK2Error");
    private ManualResetEvent _stopCheck = new ManualResetEvent(false);
    public BitArray ports = new BitArray(3);
    private bool isAllDevicesGood = true;
    //private ToolTip _toolTip = new ToolTip();

    public static class CheckStatus
    {
      public static string InProgress = Resources.Checking + "...";
      public static string Success = Resources.Success;
      public static string Fail = Resources.Fail;
      public static string NoCheck = Resources.CantCheck;
      public static string AnotherSlot = "ћодуль установлен в другой слот!";
        }
    public SelfTest()
    {
      InitializeComponent();
      //dgvSelfTest.CellToolTipTextNeeded += dgvSelfTest_CellToolTipTextNeeded;
      dgvSelfTest.ShowCellToolTips = true;
      //_toolTip.UseFading = false;
      //_toolTip.UseAnimation = false;
      //_toolTip.Active = true;
      //_toolTip.IsBalloon = true;
      //dgvSelfTest.CellMouseEnter += dgvSelfTest_CellMouseEnter;
      //dgvSelfTest.CellMouseLeave += dgvSelfTest_CellMouseLeave;
    }

    private void SelfTest_Load(object sender, EventArgs e)
    {

    }

    private void SetLastRowStatus(bool result, bool isAnotherSlot = false, uint slotNumber = 0)
        {
      if (_stopCheck.WaitOne(0))
        return;
      if (InvokeRequired)
      {
        BeginInvoke(new Action<bool, bool, uint > (SetLastRowStatus), result, isAnotherSlot, slotNumber);
        return;
      }
      int lastRowIndex = dgvSelfTest.Rows.Count - 1;
      string statusText;
      if (isAnotherSlot)
          statusText = CheckStatus.AnotherSlot;
      else
          statusText = result ? CheckStatus.Success : CheckStatus.Fail;
      dgvSelfTest.Rows[dgvSelfTest.Rows.Count - 1].Cells[1].Value = statusText;
      // —охран€ем номер слота в Tag строки
      dgvSelfTest.Rows[lastRowIndex].Tag = slotNumber;
      isAllDevicesGood = isAllDevicesGood && (result &&!isAnotherSlot);
      RepaintGrid(lastRowIndex);
      Application.DoEvents();
    }

    private void AddCheckRow(string checkRow)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new Action<string>(AddCheckRow), checkRow);
        return;
      }
      dgvSelfTest.Rows.Add(checkRow, CheckStatus.InProgress);
      ssStatus.Text = Resources.Checking + " " + checkRow;
      RepaintGrid(dgvSelfTest.Rows.Count - 1);
      Application.DoEvents();
    }

    private void AddRow(string caption)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new Action<string>(AddRow), caption);
        return;
      }
      dgvSelfTest.Rows.Add(caption);
    }

    private void RepaintGrid(int row)
    {
      if (dgvSelfTest.Rows[row].Cells[1].Value == null)
        return;
      DataGridViewCellStyle successStyle = new DataGridViewCellStyle();
      successStyle.BackColor = Color.LightGreen;
      DataGridViewCellStyle failStyle = new DataGridViewCellStyle();
      failStyle.BackColor = Color.Red;
      DataGridViewCellStyle anotherSlotStyle = new DataGridViewCellStyle();
      anotherSlotStyle.BackColor = Color.Orange;
      if (dgvSelfTest.Rows[row].Cells[1].Value.GetHashCode() == CheckStatus.Success.GetHashCode())
        dgvSelfTest.Rows[row].DefaultCellStyle = successStyle;
      if (dgvSelfTest.Rows[row].Cells[1].Value.GetHashCode() == CheckStatus.Fail.GetHashCode())
        dgvSelfTest.Rows[row].DefaultCellStyle = failStyle;
      if (dgvSelfTest.Rows[row].Cells[1].Value.GetHashCode() == CheckStatus.AnotherSlot.GetHashCode())
          dgvSelfTest.Rows[row].DefaultCellStyle = anotherSlotStyle;
    }

    private void CaptionInit(bool isStart)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new Action<bool>(CaptionInit), isStart);
        return;
      }
      if (isStart)
      {
        bClose.Enabled = false;
        bCheck.Enabled = true;
        bCheck.Text = Resources.Stop;
        lCaption.Text = Resources.WaitWhileDeviceTesting;
        lCaption.ForeColor = Color.Black;
        dgvSelfTest.Font = new Font("Microsoft Sans Serif", 11);
        dgvSelfTest.Rows.Add(Res.LTRBoards);
      }
      else
      {
        lCaption.Text = Resources.TestFinished + " ";
        if (isAllDevicesGood)
        {
          lCaption.Text += Resources.AllDevicesGood;
          lCaption.ForeColor = Color.Green;
        }
        else
        {
          lCaption.Text += Resources.SomeDevicesFailed;
          lCaption.ForeColor = Color.DarkRed;
        }
        bClose.Enabled = true;
        bCheck.Enabled = true;
        bCheck.Text = Resources.RepeatTest;
      }
    }

    private void DataGridCheck()
    {
        try
        {

            Globals.SetThreadCulture(Thread.CurrentThread);
            CaptionInit(true);
            LTRManager.CloseAll();

            if (LTRManager.CheckCrate())
            {

                if (_stopCheck.WaitOne(0))
                {
                    CheckInterrupted();
                    return;
                }
                AddCheckRow("LTR114");
                uint configuredSlotLTR114 = LTRSupport.GetLTRSlotFromConfig("LTR114");
                string moduleInSlotLTR114 = LTRManager.GetModuleBySlot(configuredSlotLTR114);

                if (moduleInSlotLTR114 == "LTR114")
                {
                    bool success = LTRManager.LTR114SetActiveChannels(configuredSlotLTR114, 1);
                    SetLastRowStatus(success);
                }
                else
                {
                    uint foundSlotLTR114 = 0;
                        // »щем LTR114 в других слотах
                        bool foundInOtherSlot = false;
                    for (uint i = 0; i < 16; i++)
                    {
                            if (i != configuredSlotLTR114 && LTRManager.GetModuleBySlot(i) == "LTR114")
                        {
                            foundInOtherSlot = true;
                            foundSlotLTR114 = i;
                                break;
                        }
                    }

                    SetLastRowStatus(false, foundInOtherSlot, foundSlotLTR114);
                }

                string[] ltr11ConfigNames = { "LTR11_1", "LTR11_2", "LTR11_3" };
                uint globalStartSlotLTR11 = 0; // общий стартовый слот дл€ поиска, измен€етс€ после находок

                for (int i = 0; i < ltr11ConfigNames.Length; i++)
                {
                    if (_stopCheck.WaitOne(0))
                    {
                        CheckInterrupted();
                        return;
                    }

                    AddCheckRow("LTR11 (" + i + ")");

                    // ѕолучаем ожидаемый слот из конфигурации
                    uint configuredSlotLTR11 = LTRSupport.GetLTRSlotFromConfig(ltr11ConfigNames[i]);
                    string moduleInSlotLTR11 = LTRManager.GetModuleBySlot(configuredSlotLTR11);

                    if (moduleInSlotLTR11 == "LTR11")
                    {
                        // ћодуль в нужном слоте Ц выполн€ем настройку
                        bool success = ExceptionToBool(() => LTRManager.LTR11SetupAdc(configuredSlotLTR11, 1));
                        SetLastRowStatus(success);
                        // ≈сли модуль в нужном слоте, то мы не сдвигаем globalStartSlot,
                        // так как не было найдено никакого "чужого" слота
                    }
                    else
                    {
                        // »щем LTR11 в других слотах, начина€ с globalStartSlot
                        bool foundInOtherSlotLTR11 = false;
                        uint foundSlot = 0;
                        uint startSlot = globalStartSlotLTR11;
                        uint foundSlotLTR11 = 0;

                            while (startSlot < 16)
                        {
                            bool found = false;
                            // »щем первый LTR11 в другом слоте, начина€ с startSlot
                            for (uint slot = startSlot; slot < 16; slot++)
                            {
                                if (slot == 11 || slot == 12 || slot == 13)
                                    continue;
                                if (slot != configuredSlotLTR11 && LTRManager.GetModuleBySlot(slot) == "LTR11")
                                {
                                    foundSlot = slot;
                                    found = true;
                                    foundSlotLTR11 = foundSlot;
                                        break;
                                }
                            }
                            if (!found)
                                break; // больше нет модулей в слотах от startSlot до 15
                            // Ќашли модуль в слоте foundSlot
                            foundInOtherSlotLTR11 = true;
                            // ќбновл€ем глобальный стартовый слот дл€ следующей итерации
                            globalStartSlotLTR11 = foundSlot + 1;
                            break; // выходим из while, так как нам нужен только первый найденный дл€ этой итерации
                        }

                        // ”станавливаем статус: если модуль найден в другом слоте Ц AnotherSlot, иначе Fail
                        SetLastRowStatus(false, foundInOtherSlotLTR11,foundSlotLTR11);
                    }
                }

                string[] ltr22ConfigNames = { "LTR22_1", "LTR22_2" };
                uint globalStartSlotLTR22 = 0; // общий стартовый слот дл€ поиска, измен€етс€ после находок

                for (int i = 0; i < ltr22ConfigNames.Length; i++)
                {
                    if (_stopCheck.WaitOne(0))
                    {
                        CheckInterrupted();
                        return;
                    }

                    AddCheckRow("LTR22 (" + i + ")");
                    // ѕолучаем ожидаемый слот из конфигурации
                    uint configuredSlotLTR22 = LTRSupport.GetLTRSlotFromConfig(ltr22ConfigNames[i]);
                    string moduleInSlotLTR22 = LTRManager.GetModuleBySlot(configuredSlotLTR22);

                    if (moduleInSlotLTR22 == "LTR22")
                    {
                        // ћодуль в нужном слоте Ц выполн€ем настройку
                        bool success = LTRManager.LTR22SetACDCState(configuredSlotLTR22, true);
                        SetLastRowStatus(success);
                        // ≈сли модуль в нужном слоте, то мы не сдвигаем globalStartSlot, 
                        // так как не было найдено никакого "чужого" слота
                    }
                    else
                    {
                        // »щем LTR22 в других слотах, начина€ с globalStartSlotLTR22
                        bool foundInOtherSlotLTR22 = false;
                        uint foundSlot = 0;
                        uint startSlot = globalStartSlotLTR22;
                        uint foundSlotLTR22=0;

                        while (startSlot < 14)
                        {
                            bool found = false;
                            // »щем первый LTR22 в другом слоте, начина€ с startSlot
                            for (uint slot = startSlot; slot < 14; slot++)
                            {
                                if (slot != configuredSlotLTR22 && LTRManager.GetModuleBySlot(slot) == "LTR22")
                                {
                                    foundSlot = slot;
                                    found = true;
                                    foundSlotLTR22= foundSlot;
                                        break;
                                }
                            }
                            if (!found)
                                break; // больше нет модулей в слотах от startSlot до 13
                            // Ќашли модуль в слоте foundSlot
                            foundInOtherSlotLTR22 = true;
                            // ќбновл€ем глобальный стартовый слот дл€ следующей итерации
                            globalStartSlotLTR22 = foundSlot + 1;
                            break; // выходим из while, так как нам нужен только первый найденный дл€ этой итерации
                        }
                        // ”станавливаем статус: если модуль найден в другом слоте Ц AnotherSlot, иначе Fail
                        SetLastRowStatus(false, foundInOtherSlotLTR22,foundSlotLTR22);
                    }
                }
            }
            else
            {
                dgvSelfTest.Rows[0].Cells[1].Value = "LTR-крейт не обнаружен!";
                dgvSelfTest.Rows[0].Cells[1].Style.Font = new Font(dgvSelfTest.Font, FontStyle.Bold);
                dgvSelfTest.Rows[0].Cells[1].Style.ForeColor = Color.DarkRed;
                
                AddCheckRow("LTR114");
                SetLastRowStatus( false);
                
                string[] ltr11ConfigNames = { "LTR11_1", "LTR11_2", "LTR11_3" };
                for (int i = 0; i < ltr11ConfigNames.Length; i++)
                {
                    AddCheckRow("LTR11 (" + i + ")");
                    SetLastRowStatus(false);
                }
                
                string[] ltr22ConfigNames = { "LTR22_1", "LTR22_2" };
                for (int i = 0; i < ltr22ConfigNames.Length; i++)
                {
                    AddCheckRow("LTR22 (" + i + ")");
                    SetLastRowStatus(false);
                }
            }

            if (_stopCheck.WaitOne(0))
            {
                    CheckInterrupted();
                    return;
            }
            AddRow(Resources.PowerSupplies + ":");
            AddCheckRow("27V");
            SetLastRowStatus(CheckPS27V());
            bool result;
            if (_stopCheck.WaitOne(0))
            {
                    CheckInterrupted();
                    return;
            }
            AddCheckRow("115V");
        
            SetLastRowStatus(CheckPS115V());
            if (_stopCheck.WaitOne(0))
            {
              CheckInterrupted();
              return;
            }
            AddRow(Resources.Others + ":");
            AddCheckRow(Resources.SKTBoard);
            SetLastRowStatus(GenSinCos.Instance().GeneratorOff());
            if (_stopCheck.WaitOne(0))
            {
              CheckInterrupted();
              return;
            }
            AddCheckRow(Resources.KZBoard);
            bool gen = false;
            DateTime date = new DateTime();
            SetLastRowStatus(ControlTask.Instance().GetVersion(ref gen, ref date));
            if (_stopCheck.WaitOne(0))
            {
              CheckInterrupted();
              return;
            }
            AddCheckRow(Resources.DiscreteInputsBoard);
            DiscreteInputs di = new DiscreteInputs();

            SetLastRowStatus(di.GetInputsStates() != null);
            if (_stopCheck.WaitOne(0))
            {
              CheckInterrupted();
              return;
            }
            AddCheckRow(Resources.DiscreteOutputsBoard);
            SetLastRowStatus(DiscreteOutputs.Instance().SetChannelsOff());
            if (_stopCheck.WaitOne(0))
            {
              CheckInterrupted();
              return;
            }
            AddCheckRow(Resources.DiscretInputOutputsBoard);
            try
            {
                DO176X.Instance.GetRelaysState();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result = false;
            }
            SetLastRowStatus(result);
                    
            try
            {
                  SpectrumAnalyzer.Instance.Init();
                  if (SpectrumAnalyzer.IsConnected)
                  {
                      AddCheckRow("јнализатор спектра");
                      SetLastRowStatus(true);
                  }
            }
            catch {} //ничего происходить не должно по идее
            CaptionInit(false);
        }
        catch (StopCheckException)
        {
            CheckInterrupted();
        }
        catch (Exception ex)
        {
            ShowMessage(Resources.Error);
            _logger.Error(ex);
            CheckInterrupted();
        }
        dgvSelfTest.ReadOnly = true;
    }

    private bool CheckPS115V()
    {
        try
        {
            double value = Power115V.Instance.GetVoltage();
            if (double.IsNaN(value))
            {
                Power115V.RenewInstance();
                value = Power115V.Instance.GetVoltage();
                if (!double.IsNaN(value))
                    return true;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            return false;
        }

        return false;
    }

    private bool CheckPS27V()
    {
        try
        {
            return Power27V.Instance.GetOutputState() != null;
        }
        catch
        {
            return false;
        }
    }

    private bool ExceptionToBool(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void CheckInterrupted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new Action(CheckInterrupted));
        return;
      }
      bCheck.Text = Resources.RepeatTest;
      bClose.Enabled = true;
      bCheck.Enabled = true;
      lCaption.Text = Resources.CheckInterrupted;
      lCaption.ForeColor = Color.Red;
    }

    private void ShowMessage(string message)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string>(ShowMessage), message);
        return;
      }
      MessageBox.Show(message);
    }

    //private void SetButtonEnabled(Button buttn, bool state)
    //{
    //  if (InvokeRequired)
    //  {
    //    Invoke(new Action<Button, bool>(SetButtonEnabled), buttn, state);
    //    return;
    //  }
    //  buttn.Enabled = state;
    //}

    private void SelfTest_Shown(object sender, EventArgs e)
    {
      DataGridCheckInThread();
    }

    private void dgvSelfTest_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if ((e.ColumnIndex == 1) && (e.RowIndex >= 0))
            RepaintGrid(e.RowIndex);
    }

    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void bCheck_Click(object sender, EventArgs e)
    {
      bCheck.Enabled = false;
      if (bCheck.Text == Resources.Stop)
        _stopCheck.Set();
      else
      {
        LTRManager.CloseAll();
        isAllDevicesGood = true;
        dgvSelfTest.Rows.Clear();
        DataGridCheckInThread();
      }
    }

    private Thread _checkThread;

    private void DataGridCheckInThread()
    {
      _stopCheck.Reset();
      _checkThread = new Thread(DataGridCheck);
      _checkThread.IsBackground = true;
      _checkThread.Start();
    }

    private void SelfTest_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!bCheck.Enabled && !bClose.Enabled)
      {
        e.Cancel = true;
        return;
      }
      if (ParentForm != null)
        ((frmMain)ParentForm).EnableMenu(true);
    }

        private void dgvSelfTest_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string status = e.Value?.ToString();
                if (status == CheckStatus.Fail)
                {
                    dgvSelfTest[1, e.RowIndex].ToolTipText = "ќборудование не найдено!";
                }
                else if (status == CheckStatus.AnotherSlot)
                {
                    object tag = dgvSelfTest.Rows[e.RowIndex+1].Tag;
                    string slotText;
                    if (tag != null)
                    {
                        uint slotNumber = (uint)tag;
                        slotText = (slotNumber + 1).ToString(); // увеличение на 1
                    }
                    else
                        slotText = "неизвестный";
                    dgvSelfTest[1, e.RowIndex].ToolTipText = $"ћодуль установлен в слот {slotText}.\n”становите модуль в слот {e.RowIndex+11}.";
                }
                else
                {
                    dgvSelfTest[1, e.RowIndex].ToolTipText = "—татус: " + status;
                }
            }
        }
    }
}