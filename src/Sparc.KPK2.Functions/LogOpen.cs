using System;
using System.IO;
using System.Windows.Forms;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace Sparc.KPK2.NFunctions
{
  /// <summary>
  /// Регистрация открытия рабочих форм 
  /// </summary>
  public class LogOpen
  {
    //private static string pathFile = Application.CommonAppDataPath + "\\LogOpen2.txt";
    //private static string pathFileOld = Application.CommonAppDataPath + "\\LogOpenOld2.txt";
    //private static object _lockObject = new object();

    ///// <summary>
    ///// Запись результата проверки наличия плат и открытия СОМ портов 
    ///// </summary>
    ///// <param name="str">текст сообщения</param>
    //public static void WriteLogOpen(string str)
    //{
    //  try
    //  {
    //      lock (_lockObject)
    //      {
    //          FileExists();
    //          FileSize(1048576);

    //          StreamWriter flOpen = new StreamWriter(pathFile, true);
    //          flOpen.WriteLine(DateTime.Now.ToString() + " " + str);
    //          flOpen.Close();
    //      }
    //  }
    //  catch
    //  {
    //    MessageBox.Show(Res.WriteLogError, Res.Attention, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //  }
    //}

    ///// <summary>
    ///// Чистим файл, если его размер больше 1Мб 
    ///// </summary>
    ///// <param name="m">max размер LogOpen</param>
    //private static void FileSize(int m)
    //{
    //  FileInfo flInf = new FileInfo(pathFile);
    //  if (flInf.Length > m)
    //  {
    //    File.Copy(pathFile, pathFileOld, true);
    //    File.Delete(pathFile);
    //  }
    //}

    ///// <summary>
    ///// Проверяем есть ли файл, если нет - создаем его 
    ///// </summary>
    //private static void FileExists()
    //{
    //  if (!File.Exists(pathFile))
    //  {
    //    FileStream fs = File.Create(pathFile);
    //    fs.Close();
    //  }
    //}
  }
}
