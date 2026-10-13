namespace KPK2
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mnuCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWork = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss15 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDISS15G = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss32 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss32_90 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss32_90A = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss13 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss3228 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiss3228_Monoblock = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuChecksHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUtils = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyDb = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAttestation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiagnostic = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuReportHeaderEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPoverkaEnabled = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSysMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDocumentLinkEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.îïðîñÊÇToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuKZCalibrate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGenSinCosCalibration = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClearDB = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheckComDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCalibrate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuReport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPS115VCalibrate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportLogs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBlockNumbers = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuComPorts = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAttenuationsCalibrate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSettingsFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLists = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTypeAC = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmployees = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPosts = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuManual = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLanguage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmRussian = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnglish = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEspanol = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			resources.ApplyResources(this.menuStrip, "menuStrip");
			this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCheck,
            this.mnuUtils,
            this.mnuLists,
            this.mnuHelp,
            this.mnuLanguage});
			this.menuStrip.Name = "menuStrip";
			// 
			// mnuCheck
			// 
			this.mnuCheck.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWork,
            this.mnuChecksHistory,
            this.toolStripSeparator1,
            this.mnuExit});
			this.mnuCheck.Name = "mnuCheck";
			resources.ApplyResources(this.mnuCheck, "mnuCheck");
			// 
			// mnuWork
			// 
			this.mnuWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
           });
			this.mnuWork.Name = "mnuWork";
			resources.ApplyResources(this.mnuWork, "mnuWork");
			// 
			// mnuChecksHistory
			// 
			this.mnuChecksHistory.Name = "mnuChecksHistory";
			resources.ApplyResources(this.mnuChecksHistory, "mnuChecksHistory");
			this.mnuChecksHistory.Click += new System.EventHandler(this.mnuChecksHistory_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			resources.ApplyResources(this.mnuExit, "mnuExit");
			this.mnuExit.Click += new System.EventHandler(this.MenuItem_Exit_Click);
			// 
			// mnuUtils
			// 
			this.mnuUtils.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyDb,
            this.mnuAttestation,
            this.mnuDiagnostic,
            this.mnuReportHeaderEditor,
            this.mnuPoverkaEnabled,
            this.mnuSysMenu});
			this.mnuUtils.Name = "mnuUtils";
			resources.ApplyResources(this.mnuUtils, "mnuUtils");
			// 
			// mnuCopyDb
			// 
			this.mnuCopyDb.Name = "mnuCopyDb";
			resources.ApplyResources(this.mnuCopyDb, "mnuCopyDb");
			this.mnuCopyDb.Click += new System.EventHandler(this.MenuItem_CopyDb_Click);
			// 
			// mnuAttestation
			// 
			this.mnuAttestation.Name = "mnuAttestation";
			resources.ApplyResources(this.mnuAttestation, "mnuAttestation");
			this.mnuAttestation.Click += new System.EventHandler(this.MenuItem_Attest_Click);
			// 
			// mnuDiagnostic
			// 
			this.mnuDiagnostic.Name = "mnuDiagnostic";
			resources.ApplyResources(this.mnuDiagnostic, "mnuDiagnostic");
			this.mnuDiagnostic.Click += new System.EventHandler(this.MenuItem_Diagnostic_Click);
			// 
			// mnuReportHeaderEditor
			// 
			this.mnuReportHeaderEditor.Name = "mnuReportHeaderEditor";
			resources.ApplyResources(this.mnuReportHeaderEditor, "mnuReportHeaderEditor");
			this.mnuReportHeaderEditor.Click += new System.EventHandler(this.mnuReportHeaderEditor_Click);
			// 
			// mnuPoverkaEnabled
			// 
			this.mnuPoverkaEnabled.CheckOnClick = true;
			this.mnuPoverkaEnabled.Name = "mnuPoverkaEnabled";
			resources.ApplyResources(this.mnuPoverkaEnabled, "mnuPoverkaEnabled");
			this.mnuPoverkaEnabled.Click += new System.EventHandler(this.mnuEnablePoverka_Click);
			// 
			// mnuSysMenu
			// 
			this.mnuSysMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDocumentLinkEditor,
            this.îïðîñÊÇToolStripMenuItem,
            this.mnuKZCalibrate,
            this.mnuGenSinCosCalibration,
            this.mnuClearDB,
            this.mnuCheckComDevices,
            this.mnuCalibrate,
            this.mnuReport,
            this.mnuPS115VCalibrate,
            this.mnuExportLogs,
            this.mnuBlockNumbers,
            this.mnuComPorts,
            this.mnuSettings,
            this.mnuAttenuationsCalibrate,
            this.mnuSettingsFile});
			this.mnuSysMenu.Name = "mnuSysMenu";
			resources.ApplyResources(this.mnuSysMenu, "mnuSysMenu");
			// 
			// mnuDocumentLinkEditor
			// 
			this.mnuDocumentLinkEditor.Name = "mnuDocumentLinkEditor";
			resources.ApplyResources(this.mnuDocumentLinkEditor, "mnuDocumentLinkEditor");
			this.mnuDocumentLinkEditor.Click += new System.EventHandler(this.mnuDocumentLinkEditor_Click);
			// 
			// îïðîñÊÇToolStripMenuItem
			// 
			this.îïðîñÊÇToolStripMenuItem.Name = "îïðîñÊÇToolStripMenuItem";
			resources.ApplyResources(this.îïðîñÊÇToolStripMenuItem, "îïðîñÊÇToolStripMenuItem");
			this.îïðîñÊÇToolStripMenuItem.Click += new System.EventHandler(this.îïðîñÊÇToolStripMenuItem_Click);
			// 
			// mnuKZCalibrate
			// 
			this.mnuKZCalibrate.Name = "mnuKZCalibrate";
			resources.ApplyResources(this.mnuKZCalibrate, "mnuKZCalibrate");
			this.mnuKZCalibrate.Click += new System.EventHandler(this.mnuKZCalibrate_Click);
			// 
			// mnuGenSinCosCalibration
			// 
			this.mnuGenSinCosCalibration.Name = "mnuGenSinCosCalibration";
			resources.ApplyResources(this.mnuGenSinCosCalibration, "mnuGenSinCosCalibration");
			this.mnuGenSinCosCalibration.Click += new System.EventHandler(this.mnuGenSinCosCalibration_Click);
			// 
			// mnuClearDB
			// 
			this.mnuClearDB.Name = "mnuClearDB";
			resources.ApplyResources(this.mnuClearDB, "mnuClearDB");
			this.mnuClearDB.Click += new System.EventHandler(this.mnuClearDB_Click);
			// 
			// mnuCheckComDevices
			// 
			this.mnuCheckComDevices.Name = "mnuCheckComDevices";
			resources.ApplyResources(this.mnuCheckComDevices, "mnuCheckComDevices");
			this.mnuCheckComDevices.Click += new System.EventHandler(this.mnuCheckComDevices_Click);
			// 
			// mnuCalibrate
			// 
			this.mnuCalibrate.Name = "mnuCalibrate";
			resources.ApplyResources(this.mnuCalibrate, "mnuCalibrate");
			this.mnuCalibrate.Click += new System.EventHandler(this.mnuCalibrate_Click);
			// 
			// mnuReport
			// 
			this.mnuReport.Name = "mnuReport";
			resources.ApplyResources(this.mnuReport, "mnuReport");
			this.mnuReport.Click += new System.EventHandler(this.mnuReport_Click);
			// 
			// mnuPS115VCalibrate
			// 
			this.mnuPS115VCalibrate.Name = "mnuPS115VCalibrate";
			resources.ApplyResources(this.mnuPS115VCalibrate, "mnuPS115VCalibrate");
			this.mnuPS115VCalibrate.Click += new System.EventHandler(this.mnuPS115VCalibrate_Click);
			// 
			// mnuExportLogs
			// 
			this.mnuExportLogs.Name = "mnuExportLogs";
			resources.ApplyResources(this.mnuExportLogs, "mnuExportLogs");
			this.mnuExportLogs.Click += new System.EventHandler(this.mnuExportLogs_Click);
			// 
			// mnuBlockNumbers
			// 
			this.mnuBlockNumbers.Name = "mnuBlockNumbers";
			resources.ApplyResources(this.mnuBlockNumbers, "mnuBlockNumbers");
			this.mnuBlockNumbers.Click += new System.EventHandler(this.mnuBlockNumbers_Click);
			// 
			// mnuComPorts
			// 
			this.mnuComPorts.Name = "mnuComPorts";
			resources.ApplyResources(this.mnuComPorts, "mnuComPorts");
			this.mnuComPorts.Click += new System.EventHandler(this.mnuComPorts_Click);
			// 
			// mnuSettings
			// 
			this.mnuSettings.Name = "mnuSettings";
			resources.ApplyResources(this.mnuSettings, "mnuSettings");
			this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
			// 
			// mnuAttenuationsCalibrate
			// 
			this.mnuAttenuationsCalibrate.Name = "mnuAttenuationsCalibrate";
			resources.ApplyResources(this.mnuAttenuationsCalibrate, "mnuAttenuationsCalibrate");
			this.mnuAttenuationsCalibrate.Click += new System.EventHandler(this.mnuAttenuationsCalibrate_Click);
			// 
			// mnuSettingsFile
			// 
			this.mnuSettingsFile.Name = "mnuSettingsFile";
			resources.ApplyResources(this.mnuSettingsFile, "mnuSettingsFile");
			this.mnuSettingsFile.Click += new System.EventHandler(this.mnuSettingsFile_Click);
			// 
			// mnuLists
			// 
			this.mnuLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTypeAC,
            this.mnuEmployees,
            this.mnuPosts});
			this.mnuLists.Name = "mnuLists";
			resources.ApplyResources(this.mnuLists, "mnuLists");
			// 
			// mnuTypeAC
			// 
			this.mnuTypeAC.Name = "mnuTypeAC";
			resources.ApplyResources(this.mnuTypeAC, "mnuTypeAC");
			this.mnuTypeAC.Click += new System.EventHandler(this.mnuTypeAC_Click);
			// 
			// mnuEmployees
			// 
			this.mnuEmployees.Name = "mnuEmployees";
			resources.ApplyResources(this.mnuEmployees, "mnuEmployees");
			this.mnuEmployees.Click += new System.EventHandler(this.MenuItem_Empl_Click);
			// 
			// mnuPosts
			// 
			this.mnuPosts.Name = "mnuPosts";
			resources.ApplyResources(this.mnuPosts, "mnuPosts");
			this.mnuPosts.Click += new System.EventHandler(this.MenuItem_Post_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuManual,
            this.toolStripSeparator2,
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			resources.ApplyResources(this.mnuHelp, "mnuHelp");
			// 
			// mnuManual
			// 
			this.mnuManual.Name = "mnuManual";
			resources.ApplyResources(this.mnuManual, "mnuManual");
			this.mnuManual.Click += new System.EventHandler(this.MenuItem_Help_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			resources.ApplyResources(this.mnuAbout, "mnuAbout");
			this.mnuAbout.Click += new System.EventHandler(this.MenuItem_TheProgramm_Click);
			// 
			// mnuLanguage
			// 
			this.mnuLanguage.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.mnuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRussian,
            this.tsmEnglish,
            this.tsmEspanol});
			this.mnuLanguage.Name = "mnuLanguage";
			resources.ApplyResources(this.mnuLanguage, "mnuLanguage");
			// 
			// tsmRussian
			// 
			this.tsmRussian.Checked = true;
			this.tsmRussian.CheckOnClick = true;
			this.tsmRussian.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmRussian.Name = "tsmRussian";
			resources.ApplyResources(this.tsmRussian, "tsmRussian");
			this.tsmRussian.Click += new System.EventHandler(this.tsmRussian_Click);
			// 
			// tsmEnglish
			// 
			this.tsmEnglish.CheckOnClick = true;
			this.tsmEnglish.Name = "tsmEnglish";
			resources.ApplyResources(this.tsmEnglish, "tsmEnglish");
			this.tsmEnglish.Click += new System.EventHandler(this.tsmEnglish_Click);
			// 
			// tsmEspanol
			// 
			this.tsmEspanol.CheckOnClick = true;
			this.tsmEspanol.Name = "tsmEspanol";
			resources.ApplyResources(this.tsmEspanol, "tsmEspanol");
			// 
			// frmMain
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::KPK2.Properties.Resources.helicopter;
			this.Controls.Add(this.menuStrip);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.IsMdiContainer = true;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuUtils;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyDb;
        private System.Windows.Forms.ToolStripMenuItem mnuLists;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuManual;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        public System.Windows.Forms.ToolStripMenuItem mnuChecksHistory;
        public System.Windows.Forms.ToolStripMenuItem mnuWork;
        public System.Windows.Forms.ToolStripMenuItem mnuAttestation;
        public System.Windows.Forms.ToolStripMenuItem mnuEmployees;
        public System.Windows.Forms.ToolStripMenuItem mnuPosts;
        public System.Windows.Forms.ToolStripMenuItem mnuDiss15;
        public System.Windows.Forms.ToolStripMenuItem mnuDiss32_90A;
        private System.Windows.Forms.ToolStripMenuItem mnuDiagnostic;
        private System.Windows.Forms.ToolStripMenuItem mnuReportHeaderEditor;
        private System.Windows.Forms.ToolStripMenuItem mnuDiss13;
        private System.Windows.Forms.ToolStripMenuItem mnuSysMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuDocumentLinkEditor;
        private System.Windows.Forms.ToolStripMenuItem îïðîñÊÇToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuKZCalibrate;
        private System.Windows.Forms.ToolStripMenuItem mnuGenSinCosCalibration;
        private System.Windows.Forms.ToolStripMenuItem mnuClearDB;
        private System.Windows.Forms.ToolStripMenuItem mnuCheckComDevices;
        private System.Windows.Forms.ToolStripMenuItem mnuCalibrate;
        private System.Windows.Forms.ToolStripMenuItem mnuReport;
        private System.Windows.Forms.ToolStripMenuItem mnuPoverkaEnabled;
        private System.Windows.Forms.ToolStripMenuItem mnuPS115VCalibrate;
        private System.Windows.Forms.ToolStripMenuItem mnuExportLogs;
        private System.Windows.Forms.ToolStripMenuItem mnuDISS15G;
        private System.Windows.Forms.ToolStripMenuItem mnuDiss32_90;
        private System.Windows.Forms.ToolStripMenuItem mnuDiss3228;
        private System.Windows.Forms.ToolStripMenuItem mnuDiss32;
        private System.Windows.Forms.ToolStripMenuItem mnuBlockNumbers;
        private System.Windows.Forms.ToolStripMenuItem mnuComPorts;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuLanguage;
        private System.Windows.Forms.ToolStripMenuItem tsmRussian;
        private System.Windows.Forms.ToolStripMenuItem tsmEnglish;
        private System.Windows.Forms.ToolStripMenuItem tsmEspanol;
        private System.Windows.Forms.ToolStripMenuItem mnuTypeAC;
        private System.Windows.Forms.ToolStripMenuItem mnuAttenuationsCalibrate;
        private System.Windows.Forms.ToolStripMenuItem mnuSettingsFile;
        private System.Windows.Forms.ToolStripMenuItem mnuDiss3228_Monoblock;
    }
}

