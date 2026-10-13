using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using KPK2.Backup;
using KPK2.GeneralForms;
using KPK2.GeneralForms.ReferenceForm;
using KPK2.GeneralForms.ServiceForm;
using KPK2.Modes;
using KPK2.Properties;
using KPK2.Reports;
using KPK2.Threading;
using KPK2.Users;
using NLog;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Timer = System.Windows.Forms.Timer;
using CommonGlobals = KPK2.Common.Globals;
using CommonSettings = KPK2.Common.CommonSettings;

namespace KPK2
{
  public partial class frmMain : Form
  {
    private static Logger _logger = LogManager.GetLogger("KPK2Error");
    private const int WM_DBLCLICK = 0x00A3;
    private const int WM_LBTNDOWN = 0x00A1;
    private const int WM_RBTNDOWN = 0x00A4;
    private const int WM_SPACEBTN = 0x0020;
    private const int HTCAPTION = 2;
    private Timer _menuTimer;
    private Mode _mode;
    private ModeApplicationState _state;

    private void StartMenuTimer()
    {
      _menuTimer = new Timer { Interval = 300 };
      _menuTimer.Tick += menuTimer_Tick;
      _menuTimer.Start();
    }

    private void menuTimer_Tick(object sender, EventArgs e)
    {
      bool bMain = true;

      foreach (Form frm in Application.OpenForms)
      {
        if (!(frm is frmMain))
        {
          bMain = false;
          break;
        }
      }
      if (_adminMode)
        return;
      mnuCheck.Enabled = mnuUtils.Enabled = mnuLists.Enabled = bMain;
    }

    /// <summary>
    /// Закрываем главную форму 
    /// </summary>
    public static bool IsFormClose;

    public frmMain(Mode mode)
    {
	  _mode = mode;
	  _state = _mode.GetModeApplication();
	  InitializeComponent();
      _state.Initialization(this);
    }

    #region Проверки

    public void  EnableMenu(bool state)
    {
      if (!state && _adminMode)
        return;
      mnuCheck.Enabled = mnuUtils.Enabled = mnuLists.Enabled = state;
    }

    // выход из приложения +
    private void MenuItem_Exit_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindow", SetLastError = true)]
    private static extern IntPtr GetNextWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] int param);

    [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr GetWindow(IntPtr hWnd, int param);

    [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString,
                                             int nMaxCount);

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpClassName,
                                           int nMaxCount);

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern bool CloseHandle(IntPtr obj);

    private const short GW_CHILD = 5;
    private const int GW_HWNDNEXT = 2;
    private const int WM_USER = 0x400;

    // закрывается форма - все закрываем 
    private void frmMain_FormClosing(object sender, FormClosingEventArgs e) { }

    #endregion

    #region Утилиты

    // создаем копию БД + 
    private void MenuItem_CopyDb_Click(object sender, EventArgs e)
    {
      BackUPDataBase frm = new BackUPDataBase();
      frm.ShowDialog();
    }

    // при загрузке формы - копию БД +
    private void frmMain_Load(object sender, EventArgs e)
    {
      LoadLanguage(CommonSettings.Default.CurrentLanguage);
      mnuLanguage.Visible = Settings.Default.ShowLanguage;
      SafeReportThread.Start(FormThreadLoad, ReportProgress, ThreadError, EndOfThread);
      StartMenuTimer();

      if (_mode.ModeApplication == ModeApplication.RepairCheck)
      {
	      mnuCheck.Text = "Ремонтная проверка";
          mnuDISS15G.Dispose();
          mnuDiss15.Dispose();
	      mnuDiss13.Dispose();
          mnuDiss32_90.Dispose();
	      mnuDiss3228.Dispose();
          mnuDiss3228_Monoblock.Dispose();
      }
    }

    /// <summary>
    ///   Копирует файл логотипа в место указанное в настройках
    /// </summary>
    private void CopyLogo()
    {
      try
      {
        string oldPath = Application.StartupPath + "\\" + CommonSettings.Default.LogoFile;
        if (File.Exists(oldPath))
          File.Copy(oldPath, CommonGlobals.PathToLogo, true);
      }
      catch (Exception ex)
      {
        _logger.Error(Resources.CopyLogoFailedNN, ex.Message, ex.StackTrace);
      }
    }

    private void ThreadError(Exception ex)
    {
      if (InvokeRequired)
      {
        Invoke(new SafeReportThread.ErrorDelegate(ThreadError), ex);
        return;
      }
      if (ex is Exceptions.CantConnectDbException)
      {
        MessageBox.Show(Resources.CantConnectToDb + " " + Resources.ApplicationMustBeClosed,
                        Resources.ErrorCaption);

        CommonGlobals.CloseProgressForm();
        CloseIt();
        return;
      }
      MessageBox.Show(Resources.UnknownError + ' ' + ex.Message);
      _logger.Fatal(Resources.UnknownError + ' ' + ex.GetType() + " " + ex.Message + " " + ex.StackTrace);
      CommonGlobals.CloseProgressForm();
      CloseIt();
    }

    private void CloseIt()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(CloseIt));
        return;
      }
      Close();
    }

    private void EndOfThread()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new MethodInvoker(EndOfThread));
        return;
      }
      CommonGlobals.CloseProgressForm();
      if (Globals.Emulation)
        MessageBox.Show(Resources.EmulationText);
    }

    private void ReportProgress(string text)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new Action<string>(ReportProgress), text);
        return;
      }
      CommonGlobals.ReportProgressStandart(text);
    }

    private void FormThreadLoad(SafeReportThread.ReportProgress report)
    {
        report(5.ToString());
        report(Resources.ConnectingDb);
#if (!emulation)
        StartReportPreloading();
#endif
        TryConnectToDb();

        report(Sparc.KPK2.Resources.Properties.Resources.InitialSettingsPrepare);

        report(Resources.LoadingSettings);
        string currentDbLang = DataManager.Instance.GetCurrentLanguage();

        if (currentDbLang != CommonSettings.Default.CurrentLanguage)
        {
            if (Settings.Default.FirstRun)
                DataManager.Instance.ChangeLanguage(CommonSettings.Default.CurrentLanguage);
            else
            {
                LoadLanguage(currentDbLang);
                SaveLanguage(currentDbLang);
            }
        }
        else
            LoadLanguage(currentDbLang);

        CommonGlobals.SetThreadCulture(Thread.CurrentThread);

        if (Settings.Default.FirstRun)
        {
            CopyLogo();
            RestoreSettings();
            Settings.Default.FirstRun = true;
            Settings.Default.Save();
            CommonSettings.Default.Save();
        }

        LoadSettings();
        report(Resources.CheckBackup);
        CheckBackup();
        KPK2HardwareSettings.GetSettings();
        if (!KPK2HardwareSettings.AvailabilityDataKPK2HardwareSettings())
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show($"Неверный формат файла\n{KPK2HardwareSettings.pathSharedFolder}\\KPK2Hardware.config.\n\nПО РАБОТАЕТ В ОГРАНИЧЕННОМ РЕЖИМЕ!",
                        "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.mnuWork.Enabled = false;
                    this.mnuDiagnostic.Enabled = false;
                    this.mnuAttestation.Enabled = false;
                }));
            }
            else
            {
                MessageBox.Show($"Неверный формат файла\n{KPK2HardwareSettings.pathSharedFolder}\\KPK2Hardware.config.\n\nПО РАБОТАЕТ В ОГРАНИЧЕННОМ РЕЖИМЕ!",
                    "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.mnuWork.Enabled = false;
                this.mnuDiagnostic.Enabled = false;
                this.mnuAttestation.Enabled = false;
                }
            return;
        }
        Power115V.GetDefaultPort = () => Settings.Default.PS115COMPort;
        Power115V.GetAlternatePort = () => KPK2HardwareSettings.GetSettings().ACPowerSupply.Port.PortName;
        Power115V.GetAlternateBaudRate = () => KPK2HardwareSettings.GetSettings().ACPowerSupply.Port.BaudRate;
        Power115V.GetIPAddress = () => KPK2HardwareSettings.GetSettings().ACPowerSupplyLan.LanPort.IPAddress;
        Power115V.SetDefaultPort = s => Settings.Default.PS115COMPort = s;
            report(Resources.SetBoardParams);
        if (!ControlTask.Instance().GetVersion())
		  MessageBox.Show(string.Format(Resources.NoConnectionWithBoardN, Resources.KZBoard));
		else
		  CommonGlobals.SetKZBeginParams(false);

    }

    private void StartReportPreloading()
    {
      Thread th = new Thread(new ThreadStart(delegate
      {
        frmForPrint frm = new frmForPrint();
        frm.InitReportViewer();
      }));
      th.Start();
    }

    private void TryConnectToDb()
    {
      if (!ConnectToDb()) //если не можем подключиться = закрываем приложение
      {
        bool result = false;
        while (Globals.EarlyUptime())
        {
          if (ConnectToDb())
          {
            result = true;
            break;
          }
        }
        if (!result)
        {
          throw new Exceptions.CantConnectDbException();
        }
      }
    }

    private const string backImageFileName = "background.jpg";

    private void LoadSettings()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(LoadSettings));
        return;
      }
      if (CommonSettings.Default.PoverkaEnabled)
        mnuAttestation.Text = Resources.Poverka;
      else
        mnuAttestation.Text = Resources.Attestation;
      
      foreach (var item in mnuWork.DropDownItems)
      {
          DeviceAndMod curDevAndMod = GetSettingsDevAndModFromCaption(((ToolStripMenuItem) item).Text);
          ((ToolStripMenuItem) item).Visible = curDevAndMod.CheckState;
      }
      CommonSettings setting = CommonSettings.Default;
      if (CommonSettings.LoadException != null)
        MessageBox.Show(Sparc.KPK2.Resources.Properties.Resources.ErrorLoadParameters);
      setting.Save();
    }

    private DeviceAndMod GetSettingsDevAndModFromCaption(string caption)
    {
        foreach (var item in CommonSettings.Default.EnableChecks)
        {
            string cap = Globals.GetDissCaption(item.DeviceId, item.Modification);
            if (cap == caption)
                return item;
        }
        throw new NotImplementedException();
    }

    private static bool ConnectToDb()
    {
        bool existFileSettings = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) +
                                             @"\Sparc\KPK2\SQLInstanceName.xml");

        if (Settings.Default.FirstRun && existFileSettings)
        {
            InstanceFile ins = new InstanceFile();

            Settings.Default.DBConnectionStr =
                ConnectionManager.GetDBConnectionString(Settings.Default.DbDiss15_32ConnectionString,
                    ins.ReadInstanceFromFile());

                Settings.Default.Save();
                ins.DeleteInstanceFile();

            return DataManager.Initialize(Settings.Default.DBConnectionStr);
        }

        return  DataManager.Initialize(Settings.Default.DbDiss15_32ConnectionString); 
    }

    private static void CheckBackup()
    {
        try
        {
            BackupAlgorithm bAlg = new BackupAlgorithm();
            if (!bAlg.CheckBackupDate())
            {
                BackUPDataBase fB = new BackUPDataBase();
                fB.BackupDB(true);
            }
        }
      catch (Exception ex)
      {
          _logger.Error(ex);
          MessageBox.Show(Resources.CopyDbFailed);
      }
    }

    // аттестация стенда +
    public static bool isOpenAttestion; // правильность введенного пароля

    private void MenuItem_Attest_Click(object sender, EventArgs e)
    {
      frmStandAttest frm = new frmStandAttest();
      frm.IsPoverka = CommonSettings.Default.PoverkaEnabled;
      isOpenAttestion = false;
      if (!frm.ErrorInitialize)
        StartForm(frm);
      else
        Globals.CantOpenForm(frm);
    }

    #endregion

    #region Справочники

    // справочник по сотрудникам 
    private void MenuItem_Empl_Click(object sender, EventArgs e)
    {
      frmUsers frm = new frmUsers();
      if (frm.isShow)
        StartForm(frm);
      else
        Globals.CantOpenForm(frm);
    }

    // справочник по должностям 
    private void MenuItem_Post_Click(object sender, EventArgs e)
    {
      frmPost frm = new frmPost();
      if (frm.isShow)
        StartForm(frm);
      else
        Globals.CantOpenForm(frm);
    }

    #endregion

    #region Помощь

    // о программе + 
    private void MenuItem_TheProgramm_Click(object sender, EventArgs e)
    {
      //AboutPO frm = new AboutPO();
      frmAbout frm = new frmAbout();
      frm.ShowDialog();
    }

    // справка 
    private void MenuItem_Help_Click(object sender, EventArgs e)
    {
      string helpFilePath = "KPK2.pdf";
      if (File.Exists(helpFilePath))
      {
        Process p = new Process();
        p.StartInfo.FileName = helpFilePath;
        p.Start();
      }
      else
        MessageBox.Show(Resources.OpenManualError, Resources.ErrorCaption);
    }
    #endregion

    /// <summary>
    /// Запускаем только одну форму один раз 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="f"></param>
    private void StartForm<T>(T f) where T : Form
    {
      f.MdiParent = this;
      f.Show();
      if (!(f is frmPost) && !(f is frmACType) && !(f is frmCalibrateAttenuations))
        f.WindowState = FormWindowState.Maximized;
      EnableMenu(false);
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case WM_SPACEBTN:
          if (systemKeys)
            return;
          break;
        case WM_DBLCLICK:
          {
            if (m.WParam.ToInt32() == HTCAPTION)
              return;
          }
          break;
        case WM_LBTNDOWN:
          {
            if (m.WParam.ToInt32() == HTCAPTION)
              return;
          }
          break;
        case WM_RBTNDOWN:
          {
            if (m.WParam.ToInt32() == HTCAPTION)
              return;
          }
          break;
      }
      if (m.ToString().IndexOf("WM_CLOSE") != -1)
        IsFormClose = true;
      base.WndProc(ref m);
    }

    private void MenuItem_Diagnostic_Click(object sender, EventArgs e)
    {
      SelfTest frm = new SelfTest();
      frm.ShowDialog();
    }

    private void mnuDocumentLinkEditor_Click(object sender, EventArgs e)
    {
      FrmDocumentLink frm = new FrmDocumentLink();
      frm.ShowDialog();
    }

    private void mnuReportHeaderEditor_Click(object sender, EventArgs e)
    {
      frmReportHeader frm = new frmReportHeader();
      frm.ShowDialog();
    }

    private void EnableSysMenu(bool state)
    {
      mnuSysMenu.Visible = state;
      mnuPoverkaEnabled.Visible = state;
      mnuLanguage.Visible = state;
    }

    private bool systemKeys = true;
    private bool _adminMode = false;

    private void frmMain_KeyDown(object sender, KeyEventArgs e)
    {
      systemKeys = e.Control & e.Alt;
      if (systemKeys)
      {
        if (e.KeyCode == Keys.O)
        {
          EnableSysMenu(true);
          _adminMode = true;
          return;
        }
        if (e.KeyCode == Keys.P)
        {
          EnableSysMenu(false);
          _adminMode = false;
        }
      }
    }



    private void опросКЗToolStripMenuItem_Click(object sender, EventArgs e)
    {
      KZTest frm = new KZTest();
      frm.ShowDialog();
    }

    private void mnuKZCalibrate_Click(object sender, EventArgs e)
    {
      frmKZCalibrate frm = new frmKZCalibrate();
      frm.ShowDialog();
    }

    private void mnuGenSinCosCalibration_Click(object sender, EventArgs e)
    {
      frmGenSinCosCalibrate frm = new frmGenSinCosCalibrate();
      frm.ShowDialog();
    }

    private void mnuClearDB_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resources.ClearDbRequest, Resources.Warning, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;

      if (DataManager.Instance.ClearDB())
        MessageBox.Show(Resources.ClearDbSuccessful);
      else
        MessageBox.Show(Resources.ClearDbFailed);
    }

    private void mnuCheckComDevices_Click(object sender, EventArgs e)
    {
      frmCheckComDevices frm = new frmCheckComDevices();
      frm.ShowDialog();
    }

    private void mnuCalibrate_Click(object sender, EventArgs e)
    {
      frmCalibrate frm = new frmCalibrate();
      frm.MdiParent = MdiParent;
      frm.Show();
    }

    private void mnuReport_Click(object sender, EventArgs e)
    {
      frmReportEditor frm = new frmReportEditor();
      frm.ShowDialog();
    }

    private void mnuEnablePoverka_Click(object sender, EventArgs e)
    {
      CommonSettings.Default.PoverkaEnabled = mnuPoverkaEnabled.Checked;
      CommonSettings.Default.Save();
      LoadSettings();
    }

    private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
    {
      // CloseLTRServer();
    }

    private void mnuPS115VCalibrate_Click(object sender, EventArgs e)
    {
      frmPS115VCalibrate f = new frmPS115VCalibrate();
      f.ShowDialog();
    }

    private void mnuExportLogs_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      if (fbd.ShowDialog() == DialogResult.OK)
      {
        try
        {
          int exCount = 0;
          string[] files = Directory.GetFiles(Application.CommonAppDataPath);
          foreach (string fName in files)
            if (String.Compare(Path.GetExtension(fName), ".log", StringComparison.CurrentCultureIgnoreCase) == 0 ||
                (String.Compare(Path.GetExtension(fName), ".txt", StringComparison.CurrentCultureIgnoreCase) == 0))
            {
              File.Copy(fName, fbd.SelectedPath + "\\" + Path.GetFileName(fName), true);
              exCount++;
            }
          MessageBox.Show(string.Format(Resources.LogsCopyOutSuccessfulFilesN, exCount));
        }
        catch (Exception)
        {
          MessageBox.Show(Resources.LogsCopyOutFailed);
        }
      }
    }

    private void mnuChecksHistory_Click(object sender, EventArgs e)
    {
      frmViewTest frm = new frmViewTest(DevicesID.Diss15, _mode.ModeApplication); // ДИСС-15
      if (frm.isShow)
        StartForm(frm);
      else
        Globals.CantOpenForm(frm);
    }
    
    private void mnuBlockNumbers_Click(object sender, EventArgs e)
    {
      frmBlockService frm = new frmBlockService();
      frm.ShowDialog();
    }

    private void mnuComPorts_Click(object sender, EventArgs e)
    {
      frmComPorts frm = new frmComPorts();
      frm.ShowDialog();
    }

    private void mnuSettings_Click(object sender, EventArgs e)
    {
      frmSettings frm = new frmSettings();
      frm.ShowDialog();
    }

    private void tsmRussian_Click(object sender, EventArgs e)
    {
      const string lang = DataManager.RussianLang;
      LoadLanguage(lang);
      if (!SaveLanguage(lang)) return;
    }

    private void tsmEnglish_Click(object sender, EventArgs e)
    {
      const string lang = DataManager.EnglishLang;
      LoadLanguage(lang);
      if (!SaveLanguage(lang)) return;
    }

    private void LoadLanguage(string lang)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string>(LoadLanguage), lang);
        return;
      }
      switch (lang)
      {
        case DataManager.SpanishLang:
          CheckLanguage(tsmEspanol);
          SetApplicationCulture(lang.ToLower());
          break;
        case DataManager.EnglishLang:
          CheckLanguage(tsmEnglish);
          SetApplicationCulture(lang.ToLower());
          break;
        default:
          CheckLanguage(tsmRussian);
          SetApplicationCulture(lang.ToLower());
          break;
      }
      mnuLanguage.Image = null;
      Settings.Default.UniqueProgramText = Resources.ApplicationAlreadyRunning;
      Settings.Default.Save();
    }

    private void SetApplicationCulture(string culture)
    {
      CultureInfo ci = new CultureInfo(culture);
      Application.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
      Application.CurrentInputLanguage = InputLanguage.FromCulture(ci);
      Thread.CurrentThread.CurrentUICulture = ci;
      SetMainFormCulture(ci);
      Form[] frms = new Form[] { this };
      LocalizeForm(this, ci, ref frms);
      foreach (Form frm in Application.OpenForms)
      {
        frms = new[] { frm };
        LocalizeForm(frm, ci, ref frms);
      }
    }

    private void SetMainFormCulture(CultureInfo ci)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<CultureInfo>(SetMainFormCulture), ci);
        return;
      }
      Thread.CurrentThread.CurrentUICulture = ci;
    }

    /// <summary>
    ///   Локализует открытые формы "на лету"
    /// </summary>
    /// <param name="frm"></param>
    /// <param name="cultureInfo"></param>
    /// <param name="closeForms"></param>
    public static void LocalizeForm(Form frm, CultureInfo cultureInfo, ref Form[] closeForms)
    {
      Type frmType = frm.GetType();
      ResourceManager res = new ResourceManager(frmType);

      //зададим список свойств объектов, которые будем извлекать из файла ресурсов  
      string[] properties = { "Text", "HeaderText", "DataSource" };

      foreach (string propertyName in properties)
      {
        //выбор всех свойств класса формы, извлечение из файла ресурсов значения, и их установка  
        foreach (
          FieldInfo fieldInfo in
          frmType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance))
        {
          //это необходимо чтоб закрыть формы имеющие данные которые также локализованы
          //потом по closeForms[] можно пробежаться и закрыть открытые формы
          if (closeForms[0].Name != frm.Name)
          {
            if (fieldInfo.FieldType == typeof(BindingSource))
            {
              Array.Resize(ref closeForms, closeForms.Length + 1);
              closeForms[closeForms.Length - 1] = frm;
              return;
            }
          }
          TranslateFieldInfo(fieldInfo, propertyName, cultureInfo, res, frm);
        }
        //TranslateFieldInfo(frmType.GetField("toolStripStatusLabel1"), propertyName, cultureInfo, res, frm);
        //Не знаю как его иначе найти, пусть пока будет так
        //код для установки свойств самих форм  
        PropertyInfo propertyInfo1 = frmType.GetProperty(propertyName);
        if (propertyInfo1 == null)
          continue;
        object objProperty1 = res.GetObject("$this." + propertyInfo1.Name, cultureInfo);
        if (objProperty1 == null) continue;
        propertyInfo1.SetValue(frm, objProperty1, null);
      }
    }

    private static void TranslateFieldInfo(FieldInfo fieldInfo, string propertyName, CultureInfo cultureInfo,
      ResourceManager res, Form frm)
    {
      PropertyInfo propertyInfo = fieldInfo.FieldType.GetProperty(propertyName);
      if (propertyInfo == null)
        return;
      object objProperty = res.GetObject(fieldInfo.Name + '.' + propertyInfo.Name, cultureInfo);
      if (objProperty == null) return;
      object field = fieldInfo.GetValue(frm);
      if (field != null)
        propertyInfo.SetValue(field, objProperty, null);
    }

    private void CheckLanguage(ToolStripMenuItem sender)
    {
      foreach (ToolStripMenuItem tsm in mnuLanguage.DropDownItems)
        tsm.Checked = (tsm == sender);
    }

    /// <summary>
    ///   Сохранить новый язык приложения в БД и настройках
    /// </summary>
    /// <param name="lng"></param>
    /// <returns></returns>
    private bool SaveLanguage(string lng)
    {
      switch (lng)
      {
        case DataManager.EnglishLang:
          if (!DataManager.Instance.ChangeLanguage(DataManager.EnglishLang))
          {
            MessageBox.Show(Resources.CantChangeDbLanguage);
            return false;
          }
          break;
        case DataManager.SpanishLang:
          if (!DataManager.Instance.ChangeLanguage(DataManager.SpanishLang))
          {
            MessageBox.Show(Resources.CantChangeDbLanguage);
            return false;
          }
          break;
        default:
          if (!DataManager.Instance.ChangeLanguage(DataManager.RussianLang))
          {
            MessageBox.Show(Resources.CantChangeDbLanguage);
            return false;
          }
          break;
      }
      CommonSettings.Default.CurrentLanguage = DataManager.Instance.GetCurrentLanguage();
      CommonSettings.Default.Save();
      return true;
    }

    private void mnuTypeAC_Click(object sender, EventArgs e)
    {
      var frm = new frmACType();
      StartForm(frm);
    }

    private void mnuAttenuationsCalibrate_Click(object sender, EventArgs e)
    {
      var frm = new frmCalibrateAttenuations();
      StartForm(frm);
    }

    private void mnuSettingsFile_Click(object sender, EventArgs e)
    {
      var frm = new frmSettingsFile();
      frm.ShowDialog();
    }



      private static bool _needRestore = true;

      private bool RestoreSettings()
      {
          try
          {
              string path = GetBaseRestorePath();
              RestoreSettingsByPath(path, null, DateTime.MinValue);
              return true;
          }
          catch
          {
              return false;
          }
      }

      private void RestoreSettingsByPath(string path, string actualDir, DateTime actualDateTime)
      {
          if (String.Compare(path, Application.CommonAppDataPath, StringComparison.CurrentCultureIgnoreCase) == 0) //Application.UserAppDataPath
                return;
          string[] dirs = Directory.GetDirectories(path);
          foreach (string dir in dirs)
          {
              TakeRestoreFile(dirs, actualDir, actualDateTime);
              RestoreSettingsByPath(dir, actualDir, actualDateTime);
          }
      }

      private void TakeRestoreFile(string[] dirs, string actualDir, DateTime actualDateTime)
      {
          if (dirs.Length > 1)
          {
              foreach (var item in dirs)
              {
                  string[] filesConf = Directory.GetFiles(item);
                  if (filesConf.Length == 0) continue;
                  foreach (string file in filesConf)
                  {
                      if (String.Compare(Path.GetFileName(file), "settings.all",
                              StringComparison.CurrentCultureIgnoreCase) == 0)
                      {
                          if (actualDateTime == DateTime.MinValue)
                          {
                              actualDateTime = Directory.GetLastWriteTime(file);
                              actualDir = file;
                          }
                          else
                          {
                              if (Directory.GetLastWriteTime(file) > actualDateTime)
                              {
                                  actualDir = file;
                                  actualDateTime = Directory.GetLastWriteTime(file);
                              }
                          }
                      }
                  }
              }

              if (String.Compare(Path.GetFileName(actualDir), "settings.all",
                      StringComparison.CurrentCultureIgnoreCase) == 0 && _needRestore)
              {
                  RestoreSettingsFromFile(actualDir);
                  _needRestore = false;
              }
          }
      }

      private void RestoreSettingsFromFile(string file)
      {
          XmlDocument document = new XmlDocument();
          document.Load(file);
          XmlNode xnReportSettings = document.GetElementsByTagName("CommonSettings")[0];
          if (xnReportSettings == null)
              return;
          if (xnReportSettings.ChildNodes.Count == 0)
              return;
          foreach (XmlNode settingsNode in xnReportSettings.ChildNodes)
          {
              RestoreKpk2Settings(settingsNode); 
          }
      }

      private void RestoreKpk2Settings(XmlNode settingsNode)
      {
          var currentDevAndMod = 0;
          foreach (XmlNode node in settingsNode)
          {
              string value = null;
              try
              {
                  if (node.ChildNodes.Count > 0)
                  {
                      for (int i = 0; i < node.ChildNodes.Count; i++)
                      {
                          value = node.ChildNodes[i].ChildNodes[0].Value;
                          CheckKpk2DevAndModAttribute(node.ChildNodes[i], value, currentDevAndMod);
                      }
                      currentDevAndMod++;
                  }
                  else
                  {
                      value = node.Value;
                      if (value == null)
                          continue;

                      CheckKpk2SettingsAttribute(settingsNode, value);
                  }
              }
              catch (Exception e)
              {
                  var error = string.Format("Ошибка восстановления настроек {0}. Атрибут {1}, значение {2}: {3}", settingsNode.Name, node.Value, value, e.Message);
                  LogManager.GetLogger("KPK2Error").Error(error);
              }

              Settings.Default.Save();
          }
      }

      private void CheckKpk2DevAndModAttribute(XmlNode node, string value, int currentDevAndMod)
      {
          if(node.Name == "DeviceId")
              CommonSettings.Default.EnableChecks[currentDevAndMod].DeviceId = (DevicesID)Enum.Parse(typeof(DevicesID), value);
          if(node.Name == "Modification")
              CommonSettings.Default.EnableChecks[currentDevAndMod].Modification = (Modification)Enum.Parse(typeof(Modification), value);
          if(node.Name == "CheckState")
              CommonSettings.Default.EnableChecks[currentDevAndMod].CheckState = Convert.ToBoolean(value);
      }

      private void CheckKpk2SettingsAttribute(XmlNode xa, string value)
      {
          if (xa.Name == "PathToBackup")
              CommonSettings.Default.PathToBackup = value;
          if (xa.Name == "ReportHeader")
              CommonSettings.Default.ReportHeader = value;
          if (xa.Name == "RightReportHeader")
              CommonSettings.Default.RightReportHeader = value;
          if (xa.Name == "ShowReportLogo")
              CommonSettings.Default.ShowReportLogo = Convert.ToBoolean(value);
          if (xa.Name == "LogoFile")
              CommonSettings.Default.LogoFile = value;
          if (xa.Name == "IsSilkWay")
              CommonSettings.Default.IsSilkway = Convert.ToBoolean(value);
          if (xa.Name == "PoverkaEnabled")
              CommonSettings.Default.PoverkaEnabled = Convert.ToBoolean(value);
          if (xa.Name == "IsNSK")
              CommonSettings.Default.IsNSK = Convert.ToBoolean(value);
          if (xa.Name == "ShowDiss32")
              CommonSettings.Default.ShowDiss32 = Convert.ToBoolean(value);
          if (xa.Name == "SpectrumAnalyzerActive")
              CommonSettings.Default.SpectrumAnalyzerActive = Convert.ToBoolean(value);
          if (xa.Name == "AttenDbmKoef")
              CommonSettings.Default.AttenDbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "CabelDiss15DbmKoef")
              CommonSettings.Default.CabelDiss15DbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "VPDbmKoef")
              CommonSettings.Default.VPDbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "KVPDiss13DbmKoef")
              CommonSettings.Default.KVPDiss13DbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "KVPDiss15DbmKoef")
              CommonSettings.Default.KVPDiss15DbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "CabelDbmKoef")
              CommonSettings.Default.CabelDbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "AdapterDbmKoef")
              CommonSettings.Default.AdapterDbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "PlugDiss13DbmKoef")
              CommonSettings.Default.PlugDiss13DbmKoef = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "AddFlyDirectionDiss32")
              CommonSettings.Default.AddFlyDirectionDiss32 = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "AddAngleKZ1Diss32")
              CommonSettings.Default.AddAngleKZ1Diss32 = Convert.ToDouble(value, CultureInfo.InvariantCulture);
          if (xa.Name == "SettingsVersion")
              CommonSettings.Default.SettingsVersion = Convert.ToInt32(value);
      }

      private string GetBaseRestorePath()
      {
          StringBuilder sbPath = new StringBuilder(Application.CommonAppDataPath);
          string company = GetCompanyName();
          if (company == String.Empty)
              return "";
          if (sbPath.ToString().Contains(company))
          {
              int begin = sbPath.ToString().IndexOf(company, StringComparison.CurrentCultureIgnoreCase) + company.Length;
              int length = sbPath.Length - begin;
              sbPath.Remove(begin, length);
          }
          return sbPath.ToString();
        }

      private string GetCompanyName()
      {
          Assembly ass = Assembly.GetExecutingAssembly();
          AssemblyCompanyAttribute[] aca =
              (AssemblyCompanyAttribute[])ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
          if (aca.Length > 0)
              return aca[0].Company;
          return String.Empty;
      }
  }
}