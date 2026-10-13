using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DbInst;
using System.Security.Principal;
using System.Xml;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller;
using Sparc.DatabaseInstaller.Sources.Logger;
using Sparc.DatabaseInstaller.Properties;
using DBConfiguration;
using DBInstaller.Interface;

namespace InstallKPK2Db
{
    public partial class fMain : Form
    {

        private string instPath;

        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {

            Thread.Sleep(5000);
        }

        private bool IsElevated()
        {
            WindowsIdentity ident = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(ident);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            if (!IsElevated())
            {
                MessageBox.Show("Требуются права администратора");
                Close();
                return;
            }

            try
            {
                instPath = Path.GetDirectoryName(Application.ExecutablePath);
                DbDataMover ddM = new DbDataMover(instPath);

                //var logger = new FileLogger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                //    "Sparc\\KPK2\\Logs"));
                var instOptions = new InstallOptions() { Is_Backup = false, Is_Reinstall = false };
                Thread.Sleep(10000);
                //ddM = null;
                try
                {
                    var dbInst = new ConfigurationScript();
                    var mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                        "Sparc\\KPK2");
                    var logPath = Path.Combine(mainPath, "SetupLogs.log");

                    var logger = new FileLogger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                        "Sparc\\KPK2\\Logs"));
                    instOptions = new InstallOptions { Is_Backup = true, Is_Reinstall = false, Backup_Folder = mainPath };

                    dbInst.CheckDB(logger);                    
                    DbInfo dbInfo = dbInst.DbInfo;

                    if (dbInfo.Is_Overall_Installed && dbInfo.Current_Version.Major != 1)
                    {
                        try
                        {
                            dbInst.InstallDB(instOptions,logger);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml("<item></item>");
                            XmlElement root = doc.DocumentElement;
                            doc.CreateAttribute("insName");
                            root.SetAttribute("insName", ddM.GetAvailibaleInstanceFromServer(Settings.Default.ConnectionString));
                            doc.Save(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) +
                                     @"\Sparc\KPK2\SQLInstanceName.xml");
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            return;
                        }
                    }

                    ddM = null;
                    instPath = Path.GetDirectoryName(Application.ExecutablePath);

                    try
                    {
                        ddM = new DbDataMover(instPath);
                        if (!ddM.IsConnected)
                        {
                            MessageBox.Show("Не удалось подключиться к Sql-серверу");
                            return;
                        }

                        if (ddM.KPKExist())
                        {
                            if (!ddM.CopyTables())
                                throw new Exception();
                            ddM.DropDatabase();
                        }
                        else
                        {
                            ddM.CheckTables();
                        }

                        dbInst.InstallDB(instOptions, logger);

                        if (!ddM.RestoreTables()) throw new Exception("Ошибка восстановления таблиц");
                        return;
                    }
                    catch (Exception ex)
                    {
                        if (ddM != null)
                        {

                            MessageBox.Show("Ошибка установки" + " " + ex + " " + ddM.ErrorMessage);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Ошибка установки" + " " + ex);
                            return;
                        }
                    }
                }
                finally
                {
                    NewEvent("Все операции выполнены успешно!");
                    MessageBox.Show("Все операции выполнены успешно!", "Информация");
                    Close();
                }


            }
            catch (Exception ex)
            {
                string addError = "";
                //if (ddM != null)
                //    addError = ddM.ErrorMessage;
                MessageBox.Show(
                    "Ошибка при выполнении операции: " + lState.Text + "\nТекст ошибки: " + ex.Message + " " + addError,
                    "Ошибка!");
            }

            Close();
        }

        private bool CheckCurrentPath()
        {
            instPath = Path.GetDirectoryName(Application.ExecutablePath);
            if (instPath == null)
                return false;
            return !instPath.Contains("Desktop") && !instPath.Contains("Рабочий стол");
        }

        /// <summary>
        /// Выполняет необходимые действия при смене события (обновляет метку, двигает прогресс бар)
        /// </summary>
        /// <param name="text">Текст события</param>
        /// <param name="doProgress">Двигать ли прогресс бар?</param>
        private void NewEvent(string text, bool doProgress)
        {
            lState.Text = text;
            if (doProgress)
                pbProgress.Value++;
            Application.DoEvents();

        }
        private void NewEvent(string text)
        {
            NewEvent(text, true);
        }
    }
}
