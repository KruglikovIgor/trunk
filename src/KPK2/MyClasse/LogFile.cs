using System;
using System.IO;
using System.Windows.Forms;
using KPK2.Properties;

namespace KPK2.MyClasse 
{
  internal class LogFile
  {
    //private static string LogFileName = Application.CommonAppDataPath + @"\LogError.txt";
    //private static string LogFileNameOld = Application.CommonAppDataPath + @"\LogErrorOld.txt";
    //private StreamWriter _logFile = null;
    //private static LogFile _instance = null;

    //public void Init()
    //{
    //  FileThereIs();
    //  FileMoreThan(1048576);
    //  _instance._logFile = new StreamWriter(LogFileName, true);
    //}

    //public static LogFile Instance()
    //{
    //  if (_instance == null)
    //  {
    //    _instance = new LogFile();
    //    _instance.Init();
    //  }
    //  return _instance;
    //}

    //public void WriteLog(Exception ex)
    //{
    //  try
    //  {
    //    lock (_logFile)
    //    {
    //      _logFile.WriteLine(string.Format("Дата исключения: {0}", DateTime.Now));
    //      _logFile.WriteLine(string.Format("Тип исключения: {0}", ex.GetType()));
    //      _logFile.WriteLine(string.Format("Сообщение: {0}", ex.Message));
    //      _logFile.WriteLine(string.Format("Стек процесса:\n {0}", ex.StackTrace));
    //      if (ex.InnerException != null)
    //      {
    //          _logFile.WriteLine(string.Format("Внутреннее исключение: {0}", ex.InnerException.Message));
    //          _logFile.WriteLine(string.Format("Тип исключения: {0}", ex.InnerException.GetType()));
    //          _logFile.WriteLine(string.Format("Стек процесса:\n {0}", ex.InnerException.StackTrace));
    //      }

    //      _logFile.Flush();
    //    }
    //  }
    //  catch (Exception exx)
    //  {
    //    MessageBox.Show(Resources.WriteLogError + "\n" + exx.Message, Resources.Attention, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //  }
    //}

    //public void WriteLog(string text)
    //{
    //    try
    //    {
    //        lock (_logFile)
    //        {
    //            var dNow = DateTime.Now;
    //            _logFile.WriteLine(string.Format("{0} {1} {2}",dNow.ToShortDateString(), dNow.ToLongTimeString(), text));
    //            _logFile.Flush();
    //        }
    //    }
    //    catch (Exception exx)
    //    {
    //        MessageBox.Show(Resources.WriteLogError + "\n" + exx.Message, Resources.Attention, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //    }
    //}

    //    /// <summary>
    //    /// Чистим файл, если его размер больше 1Мб
    //    /// </summary>
    //    /// <param name="p">max размер LogFile</param>
    //    public static void FileMoreThan(int p)
    //{
    //  FileInfo fInf = new FileInfo(LogFileName);
    //  if (p < fInf.Length)
    //  {
    //    File.Copy(LogFileName, LogFileNameOld, true);
    //    File.Delete(LogFileName);
    //    FileThereIs();
    //  }
    //}

    ///// <summary>
    ///// Проверяем есть ли файл, если нет - создаем его 
    ///// </summary>
    //public static void FileThereIs()
    //{
    //  if (!File.Exists(LogFileName))
    //  {
    //    FileStream fs = File.Create(LogFileName);
    //    fs.Close();
    //  }
    //}
  }
}
