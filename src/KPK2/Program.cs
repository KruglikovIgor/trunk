using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using KPK2.MyClasse;
using KPK2.Properties;
using NLog;
using Sparc.KPK2.NFunctions;

namespace KPK2
{
    static class Program
    {
        // ReSharper disable once NotAccessedField.Local
        private static Mutex _mMutex;
        //private static Logger _logger = LogManager.GetLogger("KPK2Error");
        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) 
        {

            if (!CreatedMutex())
            {
                MessageBox.Show(Resources.ApplicationAlreadyRunning, Resources.ApplicationName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
#if emulation
                Globals.Emulation = true;
#endif
                WriteVersionToLog();
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += ThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Mode.InitializeMode(args);
                Application.Run(new frmMain(Mode.GetMode()));
            }
            catch (Exception e)
            {
                MessageBox.Show(Resources.UnexpectedExceptionAppearApplicationWillClose);
                _logger.Error(e);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(Resources.CriticalProgramError, Resources.Error);
            _logger.Error(e.ToString);
        }

        private static void WriteVersionToLog()
        {
            //var logger = LogManager.GetLogger("KPK2Info");
            _logger.Info(GetInformationalVersion(Assembly.GetExecutingAssembly()));
        }

        private static string GetInformationalVersion(Assembly assembly)
        {
            var version = FileVersionInfo.GetVersionInfo(assembly.Location);

            var result = string.Format("{0}.{1}.{2}",
                version.ProductMajorPart,
                version.ProductMinorPart,
                version.ProductBuildPart);
            result += "." + version.ProductPrivatePart;
            return result;
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(Resources.CriticalProgramError + "\r\n" + e.Exception, Resources.Error);
            _logger.Error(e.Exception);
        }

        static bool CreatedMutex() 
        {
            bool wasCreate;
            _mMutex = new Mutex(false, Resources.ApplicationName, out wasCreate);
            return wasCreate;
        }

    }
}