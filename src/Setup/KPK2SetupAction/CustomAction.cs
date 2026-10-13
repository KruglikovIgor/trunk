using System;
using System.IO;
using System.Xml;
using DbInst;
using Microsoft.Deployment.WindowsInstaller;
using Sparc.DatabaseInstaller.Properties;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller;
using Sparc.DatabaseInstaller.Sources.Logger;
using DBConfiguration;
using DBInstaller.Interface;

namespace KPK2SetupAction
{
	public class CustomActions
	{
		public const string ErrorConnectSqlServer = "Не удалось подключиться к SQL Server";
		public const string SetupCancelled = "Установка будет отменена!";
		public const string ErrorRestoreTb = "Не удалось восстановить таблицы!";
		public const string ErrorInstall = "Не удалось завершить установку:";
		public const string ErrorUninstall = "Не удалось завершить удаление программы:";

		[CustomAction]
		public static ActionResult InstallDb(Session session)
		{
			Record rcd = new Record(0);

            var mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                "Sparc\\KPK2");
            var logPath = Path.Combine(mainPath, "SetupLogs.log");

            string instPath = session.CustomActionData.ToString();
            DbDataMover ddM = new DbDataMover(instPath);
             //ddM = null;
            var logger = new FileLogger(logPath);

            var dbInst = new ConfigurationScript();
            //installer.Logger = logger;
			var instOptions = new InstallOptions {Is_Backup = true, Is_Reinstall = false, Backup_Folder = mainPath};
            dbInst.CheckDB(logger);
            DbInfo dbInfo = dbInst.DbInfo;
            if (dbInfo.Is_Overall_Installed && dbInfo.Current_Version.Major != 1)
			{
				try
				{
                    dbInst.CheckDB(logger);
                    XmlDocument doc = new XmlDocument();
				    doc.LoadXml("<item></item>");
				    XmlElement root = doc.DocumentElement;
				    doc.CreateAttribute("insName");
				    root.SetAttribute("insName", ddM.GetAvailibaleInstanceFromServer(Settings.Default.ConnectionString));
                    doc.Save(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Sparc\KPK2\SQLInstanceName.xml");
                    return ActionResult.Success;
				}
				catch (Exception ex)
				{
					rcd[0] = ex.Message;
					session.Message(InstallMessage.Error, rcd);
					return ActionResult.Failure;
				}
			}


			//string instPath = session.CustomActionData.ToString();
			
			try
			{
				ddM = new DbDataMover(instPath);

				if (!ddM.IsConnected)
				{
					rcd[0] = ErrorConnectSqlServer + " " + SetupCancelled;
					session.Message(InstallMessage.Error, rcd);
					//MessageBox.Show(ErrorConnectSqlServer + " " + SetupCancelled);
					return ActionResult.Failure;
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

                if (!ddM.RestoreTables())
					throw new Exception(ErrorRestoreTb);
			}
			catch (Exception ex)
			{
				if (ddM != null)
				{
					rcd[0] = ErrorInstall + " " + ex + " " + ddM.ErrorMessage;
					session.Message(InstallMessage.Error, rcd);
					//MessageBox.Show(ErrorInstall + " " + ex + " " + ddM.ErrorMessage);
					session.Log(ErrorInstall + " " + ex + " " + ddM.ErrorMessage);
					logger.Write(ErrorInstall + " " + ex + " " + ddM.ErrorMessage);
				}
				else
				{
					rcd[0] = ErrorInstall + " " + ex;
					session.Message(InstallMessage.Error, rcd);
					session.Log(ErrorInstall + " " + ex);
					logger.Write(ErrorInstall + " " + ex);
				}
				return ActionResult.Failure;
			}

			return ActionResult.Success;
		}

		[CustomAction]
		public static ActionResult UninstallDb(Session session)
		{
			string instPath = session.CustomActionData.ToString();

            var logger = new FileLogger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Sparc\\KPK2\\Logs"));

            var dbInst = new ConfigurationScript();
            dbInst.CheckDB(logger);
            DbInfo dbInfo = dbInst.DbInfo;
            //installer.Logger = logger;
            var instOptions = new InstallOptions { Is_Backup = false, Is_Reinstall = false };
            try
            {
                dbInst.BackUp(logger);
            }
            catch (Exception ex)
            {
                Record rcd = new Record(0);
                rcd[0] = ErrorUninstall + " " + ex;
                session.Message(InstallMessage.Error, rcd);
                //MessageBox.Show(ErrorUninstall + " " + ex);
                logger.Write(ErrorUninstall + " " + ex);
                return ActionResult.Failure;
            }
			return ActionResult.Success;
		}
	}
}
