using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms;
using DBInstaller.Interface;
using MefContrib.Integration.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller;
using Sparc.DatabaseInstaller.Sources.Logger;

namespace DBInstallerGUI
{
    /// <summary>
    ///   Главное окно.
    /// </summary>
    [Export(typeof(IGUIDBInstaller))]
    public partial class MainWnd : Form, IGUIDBInstaller
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region VARs, CTORs

        //--------------------------------------------------------------------------------------------------------------
        private readonly ILogger _logger;
        

        private ConfigurationSetupImport _import;
        //--------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Последняя операция выполнена успешно?
        /// </summary>
        public bool IsSuccess { get; private set; }

        //--------------------------------------------------------------------------------------------------------------
        public MainWnd()
        {
            InitializeComponent();
            _logger = CreateLogger();
            InitializeConfSetup();
            IsSuccess = false;
            Update_Gui_State();
        }

        private void InitializeConfSetup()
        {
            try
            {
                IUnityContainer _unity;
                _unity = new UnityContainer();
                _unity.RegisterInstance(_unity);
                _unity.RegisterCatalog(new DirectoryCatalog("."));


                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(_unity));
                _import = ServiceLocator.Current.GetInstance<ConfigurationSetupImport>();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка конфигурации!\n" + e.Message);
                throw;
            }
        }

        #endregion
        private ILogger CreateLogger()
        {
            var textBoxLogger = new TextBoxLogger(rtb_Log);

            var multiLogger = new MultiLogger();

            multiLogger.AddLogger(textBoxLogger);

            return multiLogger;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region GUI

        //--------------------------------------------------------------------------------------------------------------
        private void chk_IsBackup_CheckedChanged(object sender, EventArgs e)
        {
            Update_Gui_State();
        }

        //--------------------------------------------------------------------------------------------------------------
        private void tbx_Backup_Folder_TextChanged(object sender, EventArgs e)
        {
            Update_Gui_State();
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Update_Gui_State()
        {
            bool hasBackupFolder = tbx_Backup_Folder.Text != string.Empty;

            btn_Install.Enabled = hasBackupFolder || !chk_IsBackup.Checked;
            btn_Backup.Enabled = hasBackupFolder;
            btn_Restore.Enabled = hasBackupFolder;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void btn_Backup_Folder_Click(object sender, EventArgs e)
        {
            tbx_Backup_Folder.Text =
                Select_Folder(tbx_Backup_Folder.Text, "Select backup folder") ?? tbx_Backup_Folder.Text;
        }

        //--------------------------------------------------------------------------------------------------------------
        private string Select_Folder(string init_path, string message)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = message;
                dlg.SelectedPath = init_path;

                DialogResult dlg_res = dlg.ShowDialog(this);

                if (dlg_res == DialogResult.OK)
                    return dlg.SelectedPath;

                return null;
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region COMMANDs

        //--------------------------------------------------------------------------------------------------------------
        private void Execute_Command(Action command)
        {
            Enabled = false;
            Refresh();

            try
            {
                command();
                MessageBox.Show(this, "Completed SUCCESSFULLY!", Text, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                IsSuccess = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsSuccess = false;
            }
            finally
            {
                Enabled = true;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void btn_Check_Click(object sender, EventArgs e)
        {
            Execute_Command(Check);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Check()
        {
            _import.ReaderUIInstDB.CheckDB(_logger);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void btn_Install_Click(object sender, EventArgs e)
        {
            Execute_Command(Install);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Install()
        {
            var options = new InstallOptions
            {
                Is_Reinstall = chk_IsReinstall.Checked,
                Is_Backup = chk_IsBackup.Checked,
                Backup_Folder = tbx_Backup_Folder.Text
            };
            _import.ReaderUIInstDB.InstallDB(options, _logger);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void btn_Backup_Click(object sender, EventArgs e)
        {
            Execute_Command(Backup);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Backup()
        {
            _import.ReaderUIInstDB.BackUp(_logger);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void btn_Restore_Click(object sender, EventArgs e)
        {
            string path = Select_Folder(tbx_Backup_Folder.Text, "Select backup");

            if (path != null)
                Execute_Command(() => Restore(path));
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Restore(string path)
        {
            _import.ReaderUIInstDB.Restore(path,_logger);
        }

        public Form GetUI()
        {
            return this;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}