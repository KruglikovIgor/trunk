using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using MoveFileAction.Properties;

namespace MoveFileAction
{
    public class CustomActions
    {
        public const string ErrorInstall = "Не удалось завершить установку:";
        private const string OrigFile = "ltrModulesNet.dll";
        private const string NeedFile = "ltrModulesNet1.dll";

        [CustomAction]
        public static ActionResult OnInstall(Session session)
        {
            string instPath = session.CustomActionData.ToString();
            string fullNeedFile = instPath + NeedFile;
            string fullOrigFile = instPath + OrigFile;
            // Setup не всегда кооректно работает с файлом ltrModuleNet.dll и иногда совсем его удаляет иногда оставляет старую версию
            try
            {
                if (!File.Exists(fullOrigFile))
                {
                    FileStream fsSimple = File.OpenWrite(fullOrigFile);
                    fsSimple.Write(Resources.ltrModulesNet, 0, Resources.ltrModulesNet.Length);
                    fsSimple.Close();
                    return ActionResult.Success;
                }
                 
                FileStream fs = File.OpenWrite(fullNeedFile);
                fs.Write(Resources.ltrModulesNet, 0, Resources.ltrModulesNet.Length);
                fs.Close();
                FileInfo fiNew = new FileInfo(fullNeedFile);
                FileInfo fiStandard = new FileInfo(fullOrigFile);
                if (fiNew.Length != fiStandard.Length)
                {
                    File.Delete(fullOrigFile);
                    File.Move(fullNeedFile, fullOrigFile);
                }
                else
                    File.Delete(fullNeedFile);
            }
            catch (Exception ex)
            {
                Record rcd = new Record(0);
                rcd[0] = ErrorInstall + " " + ex;
                session.Message(InstallMessage.Error, rcd);
                session.Log(ErrorInstall + " " + ex);
            }
            return ActionResult.Success;
        }
    }
}
