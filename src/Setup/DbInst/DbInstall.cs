using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using Microsoft.Win32;
using Sparc.KPK2.NFunctions.Enums;

namespace DbInst
{
  public class DbDataMover
  {
    /// <summary>
    /// Здесь мы будем временно использовать базу master для переноса таблиц
    /// </summary>
    public const string SwitchToMaster = "use " + CopyBack;

    public const string SwitchToKPK = "use " + CopyFwd;

    public const string TbComposition = "Composition";
    public const string TbEmployees = "Employee";
    public const string TbMeasuringParam = "MeasuringParam";
    public const string TbPost = "Post";
    public const string TbTesting = "Testing";
    public const string TbTOForm = "TOForm";
    public const string TbCalibration = "Calibration";
    public const string TbParams = "Parameters";
    public const string TbAdditionalParameter = "AdditionalParameter";
      public const string tbPassportValues = "PassportValues";
      public const string tbAcType = "AcType";

    public const string CreateTb = "CREATE TABLE dbo.";

    public const string CreateCom =
      "Composition( [TestId] [int] NOT NULL, [BlockId] [int] NOT NULL, [No] [nvarchar](12) COLLATE Cyrillic_General_CI_AS NULL, [TotalRun] [int] NULL, [LastRun] [int] NULL, CONSTRAINT [PK_Composition_1] PRIMARY KEY CLUSTERED ([TestId] ASC, [BlockId] ASC )WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY]";

    public const string CreateEm =
      "Employee( [EmployeeId] [int] IDENTITY(1,1) NOT NULL, [EmployeeNumber] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [EmployeeFirstName] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [EmployeeSecondName] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [EmployeeLastName] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [IsActive] [bit] NULL, [Image] [image] NULL, [Note] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [PostId] [int] NULL, CONSTRAINT [PK_Executors] PRIMARY KEY CLUSTERED  ( [EmployeeId] ASC )WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";

    public const string CreateMp =
      "MeasuringParam( [TestId] [int] NOT NULL, [ParamId] [int] NOT NULL, [Result] [decimal](18, 5) NULL, CONSTRAINT [PK_MeasuringParam] PRIMARY KEY CLUSTERED ( [TestId] ASC, [ParamId] ASC )WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY]";

    public const string CreatePost =
      "Post( [PostId] [int] IDENTITY(1,1) NOT NULL, [PostName] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL, [IsActive] [bit] NULL, CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ( [PostId] ASC )WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY]";

    public const string CreateTesting =
      "Testing( [Id] [int] IDENTITY(1,1) NOT NULL, [DeviceTypeId] [int] NOT NULL, [ProductNumber] [nvarchar](20) COLLATE Cyrillic_General_CI_AS NOT NULL, [MakingDate] [smalldatetime] NOT NULL, [HelicopterMark] [nvarchar](20) COLLATE Cyrillic_General_CI_AS NULL, [HelicopterSign] [nvarchar](20) COLLATE Cyrillic_General_CI_AS NULL, [HelicopterNumber] [nvarchar](20) COLLATE Cyrillic_General_CI_AS NULL, [TestDate] [smalldatetime] NOT NULL, [EmployeeID] [int] NOT NULL, [Note] [nvarchar](MAX) COLLATE Cyrillic_General_CI_AS NULL, [TOFormId] [int] NULL, [TotalRun] [int] NULL, [LastRun] [int] NULL, CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED ( [Id] ASC )WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY]";

    public const string CreateTOForm =
      "TOForm( [TOFormId] [int] IDENTITY(1,1) NOT NULL, [TOFormName] [varchar](150) COLLATE Cyrillic_General_CI_AS NULL ) ON [PRIMARY]";

    public const string CreateCalibration =
      "Calibration ([Id] [int] IDENTITY(1,1) NOT NULL, [ModuleId] [int] NOT NULL, [Channel] [int] NOT NULL, [IdealValue] [decimal](18, 5) NOT NULL, [Value] [int] NULL) ON [PRIMARY]";

    public const string CreateParameters =
      "Parameters (ParamId int IDENTITY(1,1) NOT NULL, ParamName nvarchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL , DeviceId int NOT NULL, Command nvarchar(50) NULL, Koef float NOT NULL, MinValue float NULL, MaxValue float NULL, [Precision] int NOT NULL, Note nvarchar(100) NULL, AddCommand nvarchar(50) NULL) ON [PRIMARY]";

    public const string CreateAp =
      "AdditionalParameter(TestId int NOT NULL, ParamId int NOT NULL CONSTRAINT [DF_AdditionalParameter_ParamId] DEFAULT ((1)), [Value] nvarchar(100) COLLATE Cyrillic_General_CI_AS NOT NULL ) ON [PRIMARY]";

    public const string CreatePassportValues = "PassportValues(TestId int NOT NULL, ParamId int NOT NULL, Value decimal(18, 5) NULL) ON [PRIMARY]";

    public const string CreateAcType =
        "ACType( [Id] [int] IDENTITY(1,1) NOT NULL, [ACTypeName] [nvarchar](30) NOT NULL, CONSTRAINT [PK_HelicopterType] PRIMARY KEY CLUSTERED ( [Id] ASC )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] ) ON [PRIMARY] ";

    public const string DropTb = "DROP TABLE dbo.";
    public const string DropDb = "DROP DATABASE ";
    public const string TruncateTb = "TRUNCATE TABLE dbo.";
    public const string ClearTb = "DELETE FROM dbo.";


    public const string ParamsCom = "TestId,BlockId,No,TotalRun,LastRun";

    public const string ParamsEm = "EmployeeId,EmployeeNumber,EmployeeFirstName,EmployeeSecondName,EmployeeLastName,IsActive,Image,Note,PostId";

    public const string ParamsMp = "TestId,ParamId,Result";
    public const string ParamsPost = "PostId,PostName,IsActive";

    public const string ParamsTesting = "Id,DeviceTypeId,ProductNumber,MakingDate,HelicopterMark,HelicopterSign,HelicopterNumber,TestDate,EmployeeID,Note,TOFormId,TotalRun,LastRun";

    public const string ParamsTOForm = "TOFormId,TOFormName";

    public const string ParamsCalibration = "Id, ModuleId, Channel, IdealValue, Value";

    public const string ParamsParameters = "ParamId, ParamName, DeviceId, Command, Koef, MinValue,MaxValue, Precision, AddCommand";

    public const string ParamsAcType = "Id, ACTypeName";

    public const string ParamsPassportValues = "TestId, ParamId, Value";

    public const string UpdateParams = "UPDATE dbo.{0} set {0}.Koef = mparam.Koef from {0} join {1}.dbo.{0} mparam ON {0}.ParamName = mparam.ParamName";

    public const string RestoreAcParams = "SET IDENTITY_INSERT dbo.{0} OFF INSERT dbo.{0} SELECT ACTypeName from {1}.dbo.{0} tb1 where (select count(*) from dbo.{0} where ACTypeName = tb1.ACTypeName) = 0 SET IDENTITY_INSERT dbo.{0} ON";

    public const string ParamsAp2 = "TestId, ParamId, [Value]";

    public const string CopyFwd = "KPK2";
    public const string CopyBack = "master";

    public const string CopyTb = "INSERT {1} SELECT * FROM {0}.dbo.{1}";
    public const string CopyTbParams = "INSERT {1} ({2}) SELECT {2} FROM {0}.dbo.{1}";

    public const string SpecFunc = "SET IDENTITY_INSERT dbo.{0} {1}";
    public const string On = "ON";
    public const string Off = "OFF";

    public const string CheckTb = "select * from {0}.dbo.{1}";

    public const string SetTriggers = "{1} TRIGGER ALL ON dbo.{0}";
    public const string Enable = "ENABLE";
    public const string Disable = "DISABLE";

    public const string DetachKPK = "use " + CopyBack + " \n EXEC master.dbo.sp_detach_db @dbname = N'KPK2', @keepfulltextindexfile=N'true'";

    public const string colName = "Name";
    public const string colCreate = "Create";
    public const string colParams = "Params";
    public const string colAdditional = "Additional";
    public const string colResult = "Result";
    public const string colAddParams = "AddParams";

    public string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=master;Integrated Security=True";

    public const string KPK2Mdf = "KPK2.mdf";
    public const string KPK2Log = "KPK2_log.ldf";
    public const string PathDb = @"\db\" + KPK2Mdf;
    public const string PathLog = @"\db\" + KPK2Log;
    public string BaseDBPath = @"C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\Data\";
    public string BaseDBPath2008 = @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA";
    public string BaseDBPath2012 = @"C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\";

    public const string BackupDirectory = "Backup";

    private const int bitRequired = 1; //обязательная таблица
    private const int bitDisTrig = 2; //отключение триггеров у таблицы при копировании
    private const int bitSpec = 4; //специальная таблица
    private const int bitNoClearPersonalUpdate = 8; // таблица "номер один"


    private bool useClearPath;
    private SqlConnection cn;
    private static SqlCommand cmd;
    private DataTable ListOfTables;
    private string errorMessage;
    private string instPath;
    private bool _connected;

    public bool UseClearPath
    {
      set { useClearPath = value; }
    }

    public string PathDbStr
    {
      get
      {
        if (useClearPath)
          return instPath + KPK2Mdf;
        return instPath + PathDb;
      }
    }

    private string PathLogStr
    {
      get
      {
        if (useClearPath)
          return instPath + @"\" + KPK2Log;
        return instPath + @"\" + PathLog;
      }
    }

    public bool IsConnected
    {
      get { return _connected; }
    }

    public string ErrorMessage
    {
      get { return errorMessage; }
    }

    public enum CopyDirection
    {
      Forward,
      Back
    }

    public DbDataMover(string inInstPath)
    {
      instPath = inInstPath;
      cn = new SqlConnection(ConnectionString);
      cmd = new SqlCommand("", cn);
      try
      {
        cn.Open();
        _connected = true;
      }
      catch
      {
        ConnectionString = ConnectionString.Replace(@".\sqlexpress", GetAvailibaleInstanceFromServer(ConnectionString));
        cn.ConnectionString = ConnectionString;
        cn.Open();
          if (cn.State == ConnectionState.Open)
              _connected = true;
      }
      InitDataTable();
      FillTableList();
    }

      public string GetAvailibaleInstanceFromServer(string defConnStr)
      {
          string instanceStr = "";
          SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(defConnStr);
          RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
          using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
          {
              RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL",
                  false);

              if (instanceKey != null)
              {
                  foreach (var instanceName in instanceKey.GetValueNames())
                  {
                      try
                      {
                          builder.DataSource = @".\" + instanceName;
                          SqlConnection cn = new SqlConnection(builder.ConnectionString);
                          cn.Open();
                          if (cn.State == ConnectionState.Open)
                          {
                              instanceStr = builder.DataSource;
                              cn.Close();
                              break;
                          }
                      }
                      catch
                      {
                      }
                  }
              }
          }
          return instanceStr;
      }

    public bool RemoveDbFiles(string path)
    {
      try //удаляем файлы БД
      {
          var mdfFile = Path.Combine(path, KPK2Mdf);
          var logFile = Path.Combine(path, KPK2Log);
          if (File.Exists(mdfFile))
              File.Delete(path + KPK2Mdf);
          if (File.Exists(logFile))
                File.Delete(path + KPK2Log);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool CheckDbSystemPath()
    {
      return File.Exists(BaseDBPath + KPK2Mdf) || File.Exists(BaseDBPath2008 + KPK2Mdf) ||
             File.Exists(BaseDBPath2012 + KPK2Mdf);
    }

    private bool CopyDatabaseToSystemPath()
    {
      try
      {
        string myPathDb = PathDb[0] == '\\' ? PathDb.Remove(0, 1) : PathDb;
        string myPathLog = PathLog[0] == '\\' ? PathLog.Remove(0, 1) : PathLog;
        File.Copy(myPathDb, BaseDBPath + KPK2Mdf, true);
        File.Copy(myPathLog, BaseDBPath + KPK2Log, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private bool BackupOldDatabase()
    {
      try
      {
        string backupPath = BaseDBPath + BackupDirectory + @"\";
        if (!Directory.Exists(backupPath))
          Directory.CreateDirectory(backupPath);
        File.Copy(BaseDBPath + KPK2Mdf, backupPath + KPK2Mdf, true);
        File.Copy(BaseDBPath + KPK2Log, backupPath + KPK2Log, true);
        return true;
      }
      catch
      {
        return false;
      }

    }

    public bool CopyAndAttachDbOnSystemPath()
    {
      if (!BackupOldDatabase())
        return false;
      if (!CopyDatabaseToSystemPath())
        return false;
      return AttachDB(BaseDBPath);
    }

    ~DbDataMover()
    {      
      try
      {
        if (cn != null)
          if (cn.State != ConnectionState.Closed)
            cn.Close();
      }
      catch
      {
      }
    }

    /// <summary>
    /// Инициализирует таблицу которая в будущем будет содержать список таблиц для переноса
    /// Name - имя таблицы, Create - скрипт создание таблицы(без CREATE TABLE), Params - столбцы для копирования
    /// Additional - дополнительные опции для копирования(пока не знаю для чего, но пригодится), 
    /// Result - результат копирования (если таблица не была скопирована то и восстанавливать её бессмысленно)
    /// </summary>
    private void InitDataTable()
    {
      ListOfTables = new DataTable();
      ListOfTables.Columns.Add(colName, typeof (string));
      ListOfTables.Columns.Add(colCreate, typeof (string));
      ListOfTables.Columns.Add(colParams, typeof (string));
      ListOfTables.Columns.Add(colAdditional, typeof (int));
      ListOfTables.Columns.Add(colResult, typeof (bool));
      ListOfTables.Columns.Add(colAddParams, typeof (string));
    }

    /// <summary>
    /// Здесь добавляем таблицы для переноса
    /// </summary>
    private void FillTableList()
    {
      ListOfTables.Rows.Add(TbPost, CreatePost, ParamsPost, bitRequired | bitSpec, false, null);
      ListOfTables.Rows.Add(TbEmployees, CreateEm, ParamsEm, bitRequired | bitSpec, false, null);
      ListOfTables.Rows.Add(TbTesting, CreateTesting, ParamsTesting, bitRequired | bitDisTrig | bitSpec, false, null);
      ListOfTables.Rows.Add(TbComposition, CreateCom, ParamsCom, bitRequired, false, null);
      ListOfTables.Rows.Add(TbMeasuringParam, CreateMp, ParamsMp, bitRequired, false, null);
      ListOfTables.Rows.Add(TbTOForm, CreateTOForm, ParamsTOForm, bitRequired | bitSpec, false, null);
      ListOfTables.Rows.Add(TbCalibration, CreateCalibration, ParamsCalibration, bitSpec, false, null);
      ListOfTables.Rows.Add(TbParams, CreateParameters, ParamsParameters,
                            bitRequired | bitDisTrig | bitSpec | bitNoClearPersonalUpdate, false, UpdateParams);
      ListOfTables.Rows.Add(TbAdditionalParameter, CreateAp, ParamsAp2, 0, false, null);
      ListOfTables.Rows.Add(tbPassportValues, CreatePassportValues, "", 0, false, null);
      ListOfTables.Rows.Add(tbAcType, CreateAcType, ParamsAcType, bitRequired | bitSpec| bitNoClearPersonalUpdate, false, RestoreAcParams);
    }

    public bool KPKExist()
    {
      try
      {
        ExecNonQuery(cmd, SwitchToKPK);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static void ExecNonQuery(SqlCommand cmd, string cmdText)
    {
      cmd.CommandText = cmdText;
      cmd.ExecuteNonQuery();
    }

    public static bool ExecNonQuery(SqlCommand cmd)
    {
      try
      {
        cmd.ExecuteNonQuery();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool CopyTables()
    {
      try
      {
        ExecNonQuery(cmd, SwitchToMaster);
        for (int i = 0; i < ListOfTables.Rows.Count; i++)
        {
          try
          {
            try
            {
              ExecNonQuery(cmd, DropTb + ListOfTables.Rows[i][colName]);
            }
            catch
            {
            }

            if (IsEmptyTable(cmd, ListOfTables.Rows[i][colName].ToString()))
            {
              ListOfTables.Rows[i][colResult] = false;
              continue;
            }

            ExecNonQuery(cmd, CreateTb + ListOfTables.Rows[i][colCreate]);

            if (CheckBit(ListOfTables.Rows[i][colAdditional], bitSpec))
              ExecNonQuery(cmd, string.Format(SpecFunc, ListOfTables.Rows[i][colName], On));
                //выставляем Identity_insert ON для копирования с инкрементным полем

            if (String.IsNullOrEmpty(ListOfTables.Rows[i][colParams].ToString()))
              ExecNonQuery(cmd, string.Format(CopyTb, CopyFwd, ListOfTables.Rows[i][colName]));
            else
              ExecNonQuery(cmd,
                           string.Format(CopyTbParams, CopyFwd, ListOfTables.Rows[i][colName],
                                         ListOfTables.Rows[i][colParams]));

            if (CheckBit(ListOfTables.Rows[i][colAdditional], bitSpec))
              ExecNonQuery(cmd, string.Format(SpecFunc, ListOfTables.Rows[i][colName], Off));
            ListOfTables.Rows[i][colResult] = true;
          }
          catch (Exception)
          {
            ListOfTables.Rows[i][colResult] = false;
            if (CheckBit(ListOfTables.Rows[i][colAdditional], bitRequired))
            {
              errorMessage = "Неудалось скопировать обязательную таблицу " + ListOfTables.Rows[i][colName];
              return false;
            }
          }
        }
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private bool IsEmptyTable(SqlCommand cmd, string tName)
    {
      bool result = false;
      cmd.CommandText = string.Format(CheckTb, CopyFwd, tName);
      SqlDataReader dr = null;
      try
      {
        dr = cmd.ExecuteReader();
        result = !dr.HasRows;
        dr.Close();
      }
      catch
      {
        if (dr != null)
          dr.Close();
        result = true;
      }
      return result;
    }

    private static bool CheckBit(object intValue, int bitMask)
    {
      try
      {
        return (Convert.ToInt32(intValue) & bitMask) == bitMask;
      }
      catch
      {
        return false;
      }
    }

    public bool DetachDB()
    {
      try //отсоединяем старую базу
      {
        ExecNonQuery(cmd, SwitchToMaster);
        ExecNonQuery(cmd, DetachKPK);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public void CheckTables()
    {
      for (int i = 0; i < ListOfTables.Rows.Count; i++)
      {
        //проверяем есть ли таблица, если нет, значит и не надо копировать
        try
        {
          ExecNonQuery(cmd, string.Format(CheckTb, CopyBack, ListOfTables.Rows[i][colName]));
          ListOfTables.Rows[i][colResult] = true;
        }
        catch
        {
        }
      }
    }

    public bool AttachDB()
    {
      try
      {
        ExecNonQuery(cmd,
                     "CREATE DATABASE [KPK2] ON ( FILENAME = N'" + instPath + PathDb + "' ), ( FILENAME = N'" + instPath +
                     PathLog + "' ) FOR ATTACH");
        return true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        return false;
      }
    }

    public bool AttachDB(string fullPath)
    {
      try
      {
        ExecNonQuery(cmd,
                     "CREATE DATABASE [KPK2] ON ( FILENAME = N'" + fullPath + KPK2Mdf + "' ), ( FILENAME = N'" +
                     fullPath + KPK2Log + "' ) FOR ATTACH");
        return true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        return false;
      }
    }

    public bool CheckDbFileExist()
    {
      try
      {
        if (!File.Exists(PathDbStr))
          return false;
        if (!File.Exists(PathLogStr))
          return false;
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        return false;
      }
      return true;
    }

    public bool RestoreTables()
    {
      try
      {
        ExecNonQuery(cmd, SwitchToKPK);
        // очищаем все таблицы (если не очистили при подготовке базы) в обратном порядке
        for (int i = ListOfTables.Rows.Count - 1; i > 0; i--)
        {
          try
          {
            if ((bool) ListOfTables.Rows[i][colResult])
            {
              if (!CheckBit(ListOfTables.Rows[i][colAdditional], bitNoClearPersonalUpdate))
                if (!IsEmptyTable(cmd, ListOfTables.Rows[i][colName].ToString()))
                  try
                  {
                    ExecNonQuery(cmd, TruncateTb + ListOfTables.Rows[i][colName]);
                  }
                  catch
                  {
                    ExecNonQuery(cmd, ClearTb + ListOfTables.Rows[i][colName]);
                  }
            }
          }
          catch
          {
          }
        }
        for (int i = 0; i < ListOfTables.Rows.Count; i++)
        {
          try
          {

            if ((bool) ListOfTables.Rows[i][colResult])
            {
              if (!CheckBit(ListOfTables.Rows[i][colAdditional], bitNoClearPersonalUpdate))
                if (!IsEmptyTable(cmd, ListOfTables.Rows[i][colName].ToString()))
                  try
                  {
                    ExecNonQuery(cmd, TruncateTb + ListOfTables.Rows[i][colName]);
                  }
                  catch
                  {
                    ExecNonQuery(cmd, ClearTb + ListOfTables.Rows[i][colName]);
                  }

              if (CheckBit(ListOfTables.Rows[i][colAdditional], bitSpec))
                ExecNonQuery(cmd, string.Format(SpecFunc, ListOfTables.Rows[i][colName], On));
              //выставляем Identity_insert ON для копирования с инкрементным полем

              if (CheckBit(ListOfTables.Rows[i][colAdditional], bitDisTrig))
                ExecNonQuery(cmd, string.Format(SetTriggers, ListOfTables.Rows[i][colName], Disable));
              //отключаем триггера у таблицы для вставки

              CopyTable(ListOfTables.Rows[i], CopyDirection.Back);

              if (CheckBit(ListOfTables.Rows[i][colAdditional], bitDisTrig))
                ExecNonQuery(cmd, string.Format(SetTriggers, ListOfTables.Rows[i][colName], Enable));
              //включаем триггера у таблицы для вставки

              if (CheckBit(ListOfTables.Rows[i][colAdditional], bitSpec))
                ExecNonQuery(cmd, string.Format(SpecFunc, ListOfTables.Rows[i][colName], Off));
              //выставляем Identity_insert ON для копирования с инкрементным полем
            }
            
          }
          catch (Exception ex)
          {

            errorMessage = "Неудалось восстановить таблицу " + ListOfTables.Rows[i][colName] + " " +
                           ListOfTables.Rows[i][colAddParams] + " " + ex.Message + " " + ex.StackTrace;
            return false;
          }
        }
        CheckUpdateModAfterRestore();
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }

    private void CheckUpdateModAfterRestore()
    {
      SqlDataReader sr = null;
      try
      {
        SqlCommand command = cn.CreateCommand();
        command.CommandText = "select * from master.dbo.Parameters where ParamId > 320";
        sr = command.ExecuteReader();
        if (sr.HasRows)
          return;
        sr.Close();
        command.CommandText = "select Id from Testing where DeviceTypeId = 2";
        sr = command.ExecuteReader();
        if (!sr.HasRows)
          return;
        List<int> listTestId = new List<int>();
        while (sr.Read())
          listTestId.Add(Convert.ToInt32(sr[0]));
        sr.Close();
        foreach (int testId in listTestId)
          if (!SetAddParam(testId, "Modification", Modification.Diss32_90A.ToString()))
            throw new Exception();
      }
      catch
      {

      }
      finally
      {
        if (sr != null)
          if (!sr.IsClosed)
            sr.Close();
      }
    }

    private bool SetAddParam(int testId, string paramName, string value)
    {
      const string paramNameStr = "@ParamName";
      const string testIdStr = "@TestId";
      const string valueStr = "@Value";
      SqlCommand command = cn.CreateCommand();
      command.CommandText = "if (EXISTS(select * from AdditionalParameter ad join AdditionalParameterReference adr ON ad.ParamId = adr.ParamId where TestId = @TestId and ParamName = @ParamName)) update AdditionalParameter set [Value] = @Value from AdditionalParameter ad join AdditionalParameterReference adr ON ad.ParamId = adr.ParamId where TestId = @TestId and ParamName = @paramName else insert AdditionalParameter select @TestId, ParamId, @Value from AdditionalParameterReference where ParamName = @ParamName";
      command.Parameters.AddWithValue(testIdStr, testId);
      command.Parameters.AddWithValue(paramNameStr, paramName);
      command.Parameters.AddWithValue(valueStr, value);
      return  ExecNonQuery(command);
    }

    /// <summary>
    /// Осуществляет копирование из одной таблицы в другую
    /// </summary>
    /// <param name="dr">Строка параметров</param>
    /// <param name="cd">Копировать в мастер или KPK6</param>
    private static void CopyTable(DataRow dr, CopyDirection cd)
    {
      string copyD = cd == CopyDirection.Forward ? CopyFwd : CopyBack;
      if (cd == CopyDirection.Back)
        if (CheckBit(dr[colAdditional], bitNoClearPersonalUpdate))
        {
          ExecNonQuery(cmd, string.Format(dr[colAddParams].ToString(), dr[colName], copyD));
          return;
        }

      if (String.IsNullOrEmpty(dr[colParams].ToString()))
        ExecNonQuery(cmd, string.Format(CopyTb, copyD, dr[colName]));
      else
        ExecNonQuery(cmd, string.Format(CopyTbParams, copyD, dr[colName], dr[colParams]));
    }

    public void DropDatabase()
    {
      try
      {
          ExecNonQuery(cmd, SwitchToMaster);
          ExecNonQuery(cmd, DropDb + CopyFwd);
      }
      catch
      {
      }
    }

    public string GetDBPath()
    {
        SqlCommand sc = cn.CreateCommand();
        sc.CommandText = string.Format("select filename from master.dbo.sysdatabases where name = '{0}'", CopyFwd);
        return Path.GetDirectoryName(sc.ExecuteScalar().ToString());
    }
  }

  [RunInstaller(true)]
  public partial class DbInstall : Installer
  {
    public const string ErrorDetachDB = "Неудалось отключить базу данных!";
    public const string ErrorAttachDB = "Неудалось подключить базу данных!";
    public const string ErrorRestoreTb = "Неудалось восстановить таблицы!";
    public const string ErrorInstall = "Неудалось завершить установку:";
    public const string ErrorUninstall = "Неудалось завершить удаление программы:";
    public const string ErrorDeleteDbFiles = "Неудалось удалить файлы БД";

    private string ErrorMessage;
    private string instPath;

    public DbInstall()
    {
      InitializeComponent();
    }

    public void Inst()
    {
      instPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      DbDataMover ddM = null;
      try
      {
        ddM = new DbDataMover(instPath);
        if (ddM.KPKExist())
        {
          if (!ddM.CopyTables())
            throw new Exception();
          if (!ddM.DetachDB())
            throw new Exception(ErrorDetachDB);
        }
        else
        {
          ddM.DropDatabase();
          // На всякий пожарный дропаем БД, если вдруг её по факту нет, а ссылка на sql-сервере осталась (случаи были)
          ddM.CheckTables();
        }
        if (!ddM.AttachDB())
          throw new Exception(ErrorAttachDB);

        if (!ddM.RestoreTables())
          throw new Exception(ErrorRestoreTb);
      }
      catch (Exception ex)
      {
        if (ddM != null)
          MessageBox.Show(ErrorInstall + " " + ex + " " + ddM.ErrorMessage);
        else
          MessageBox.Show(ErrorInstall + " " + ex);
      }
    }

    public override void Install(System.Collections.IDictionary stateSaver)
    {
      Inst();
      base.Install(stateSaver);
    }

    public void Uninst()
    {
      instPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      DbDataMover ddM = null;
      try
      {
        ddM = new DbDataMover(instPath);
        if (!ddM.CopyTables())
          throw new Exception(ErrorMessage);
      }
      catch (Exception ex)
      {
        if (ddM != null)
          if (!String.IsNullOrEmpty(ddM.ErrorMessage))
            MessageBox.Show(ErrorUninstall + " " + ex + " " + ddM.ErrorMessage);
          else
            MessageBox.Show(ErrorUninstall + " " + ex + " " + ErrorMessage);
        else
          MessageBox.Show(ErrorUninstall + " " + ex);
      }
      if (ddM != null)
      {
        try
        {
          if (!ddM.DetachDB())
            throw new Exception(ErrorDetachDB);
          if (!DeleteDbFiles(ddM))
            throw new Exception(ErrorDeleteDbFiles);
        }
        catch (Exception ex)
        {
          if (!String.IsNullOrEmpty(ddM.ErrorMessage))
            MessageBox.Show(ErrorUninstall + " " + ex + " " + ddM.ErrorMessage);
          else
            MessageBox.Show(ErrorUninstall + " " + ex + " " + ErrorMessage);
        }
      }
    }

    private bool DeleteDbFiles(DbDataMover ddM)
    {
      try
      {
        File.Delete(instPath + DbDataMover.PathDb);
        File.Delete(instPath + DbDataMover.PathLog);
        return true;
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
        return false;
      }
    }

    public override void Uninstall(System.Collections.IDictionary savedState)
    {
      Uninst();
      base.Uninstall(savedState);
    }
  }
}