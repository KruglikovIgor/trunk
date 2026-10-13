using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using Sparc.KPK2;
using Sparc.KPK2.NFunctions.Enums;

namespace Sparc.KPK2.NFunctions
{
    //Класс работы с базой данных
  public class DataManager
  {
    public static class AddParameters
    {
      public static string TypeDiss32 = "TypeDiss32";
      public static string Modification = "Modification";
      public static string TypeBlock5 = "TypeBlock5";
    }

    private SqlConnection _connect;
    private static DataManager _instance;
    public const string EnglishLang = "EN";
    public const string SpanishLang = "ES";
    public const string RussianLang = "RU";

    public const string CheckTemperatureParam = "CheckTemperature";

    private DataManager()
    {
    }

    public static bool Initialize(string curConnectionStr)
    {
      if (_instance != null)
        return false;

      try
      {
        _instance = new DataManager();
        _instance._connect = new SqlConnection(curConnectionStr);
        _instance._connect.Open();
      }
      catch
      {
        //LogManager.Instance().WriteError("Не удалось соединиться с базой данных! Строка подключения: "+connectionStr);
        _instance = null;
          return false;
      }
        return true;
    }

    public static void Dispose()
    {
      _instance._connect.Close();
      _instance._connect.Dispose();
    }

    public static DataManager Instance
    {
      get { return _instance; }
    }

    private bool ExecuteNonQuery(SqlCommand command)
    {
      lock (_connect)
      {
        try
        {
          command.Connection = _connect;
          command.ExecuteNonQuery();
          return true;
        }
        catch
        {
          return false;
        }
      }
    }

    public bool ExecuteNonQuery(string cmdText)
    {
      SqlCommand command = new SqlCommand(cmdText, _connect);
      return ExecuteNonQuery(command);
    }

    private SqlDataReader ExecuteReader(SqlCommand command)
    {
      lock (_connect)
      {
        try
        {
          command.Connection = _connect;
          return command.ExecuteReader();
        }
        catch
        {
          return null;
        }
      }
    }

    private object ExecuteScalar(SqlCommand command)
    {
      lock (_connect)
      {
        try
        {
          command.Connection = _connect;
          return command.ExecuteScalar();
        }
        catch
        {
          return null;
        }
      }
    }

    //Устанавливает параметры порта по имени, из базы данных, неполное заполнение по стоп битам и четности
    public bool SetPortParameters(ref SerialPort sp)
    {
      SqlCommand command = new SqlCommand("select * from Port where PortName = '" + sp.PortName + "'");
      lock (_connect)
      {
          SqlDataReader reader = ExecuteReader(command);
          if (reader == null)
              return false;
          try
          {
              if (reader.Read())
              {
                  sp.DataBits = Convert.ToInt32(reader["DataBits"]);
                  sp.BaudRate = Convert.ToInt32(reader["BaudRate"]);
                  sp.ReadTimeout = Convert.ToInt32(reader["ReadTimeout"]);
                  sp.WriteTimeout = Convert.ToInt32(reader["WriteTimeout"]);
                  sp.Parity = Convert.ToInt32(reader["Parity"]) == 0
                      ? Parity.None
                      : Convert.ToInt32(reader["Parity"]) == 1
                          ? Parity.Odd
                          : Parity.Even;
                  sp.StopBits = Convert.ToDouble(reader["StopBits"]) == 0
                      ? StopBits.None
                      : Convert.ToDouble(reader["StopBits"]) == 1
                          ? StopBits.One
                          : StopBits.Two;
              }
          }
          finally
          {
              reader.Close();
          }
      }
      return true;
    }

    public const string Aktor115V = "Aktor115V";
    public const string Lambda40V = "Lambda40V";
    public const string KZ = "KZ";
    public const string SKT = "SKT";
    public const string DiscretInputs = "DiscretInputs";
    public const string DiscretOutputs = "DiscretOutputs";
    public const string Advantech1760 = "Adv1760";

    public string GetModulePort(string device)
    {
      SqlCommand command = new SqlCommand("GetModulePort");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@ModuleName", device);
      lock (_connect)
      {
          return (string) ExecuteScalar(command);
      }
    }

    public bool BackupDatabase(string path)
    {
      SqlCommand command = new SqlCommand("BackupDatabase");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@Path", path);
      return ExecuteNonQuery(command);
    }

    public string GetParamCommand(string param)
    {
      SqlCommand command = new SqlCommand("SELECT Command FROM Parameters WHERE ParamName = @strParam");
      command.Parameters.AddWithValue("@strParam", param);
      return (string) ExecuteScalar(command);
    }


    public bool GetParamLimitKoefAndPrecision(string paramName, ref double min, ref double max, ref double koef,
                                                ref int precision, ref string additional)
    {
      SqlCommand command =
        new SqlCommand(
          "select MinValue, MaxValue, Koef, Precision, AddCommand from Parameters where ParamName = @ParamName");
      command.Parameters.AddWithValue("@ParamName", paramName);
      lock (_connect)
      {
          SqlDataReader dr = ExecuteReader(command);
          try
          {
              if (!dr.HasRows)
              {
                  dr.Close();
                  return false;
              }

              dr.Read();
              min = Convert.ToDouble(dr[0] != DBNull.Value ? dr[0] : double.NaN);
              max = Convert.ToDouble(dr[1] != DBNull.Value ? dr[1] : double.NaN);
              koef = Convert.ToDouble(dr[2]);
              precision = Convert.ToInt32(dr[3]);
              additional = dr[4] != DBNull.Value ? dr[4].ToString() : "";
              dr.Close();
              return true;
          }
          catch (Exception)
          {
              if (dr != null)
                  dr.Close();
              return false;
          }
      }
    }

    public bool GetParamLimitKoefAndPrecisionInnerParam(string paramName, ref double min, ref double max, ref double koef,
                                                ref int precision, ref string additional, ref string inParamName)
    {
      SqlCommand command =
        new SqlCommand(
          "select MinValue, MaxValue, Koef, Precision, AddCommand, ParamName from Parameters where ParamId = (select cast(Command as int) from Parameters where DeviceId = 0 AND ParamName = @ParamName)");
      command.Parameters.AddWithValue("@ParamName", paramName);
      lock (_connect)
      {
          SqlDataReader dr = ExecuteReader(command);
          try
          {
              if (!dr.HasRows)
              {
                  dr.Close();
                  return false;
              }

              dr.Read();
              min = Convert.ToDouble(dr[0] != DBNull.Value ? dr[0] : double.NaN);
              max = Convert.ToDouble(dr[1] != DBNull.Value ? dr[1] : double.NaN);
              koef = Convert.ToDouble(dr[2]);
              precision = Convert.ToInt32(dr[3]);
              additional = dr[4] != DBNull.Value ? dr[4].ToString() : "";
              inParamName = dr[5] != DBNull.Value ? dr[5].ToString() : "";
              dr.Close();
              return true;
          }
          catch (Exception)
          {
              if (dr != null)
                  dr.Close();
              return false;
          }
      }
    }

    public double GetParamKoef(string paramName)
    {
      SqlCommand command = new SqlCommand("select Koef from Parameters where ParamName = @ParamName");
      command.Parameters.AddWithValue("@ParamName", paramName);
      lock (_connect)
      {
          object answer = ExecuteScalar(command);
          return Functions.MoveToDouble(answer.ToString());
      }
    }

    public bool GetParamLimitAndId(string paramName, ref double min, ref double max)
    {
      SqlCommand command = new SqlCommand("select MinValue, MaxValue from Parameters where ParamName = @ParamName");
      command.Parameters.AddWithValue("@ParamName", paramName);
      lock (_connect)
      {
          SqlDataReader dr = ExecuteReader(command);
          try
          {
              if (!dr.HasRows)
                  return false;
              dr.Read();
              min = Convert.ToDouble(dr[0] != DBNull.Value ? dr[0] : double.NaN);
              max = Convert.ToDouble(dr[1] != DBNull.Value ? dr[1] : double.NaN);
              return true;
          }
          catch (Exception)
          {
              return false;
          }
          finally
          {
              dr.Close();
          }
      }
    }

    public bool GetParamsLTR(string paramName, ref int slot, ref int channel, ref double range, ref bool alone,
                               ref bool ACDC)
    {
      SqlCommand command =
        new SqlCommand(
          "select Slot, Channel, Range, Alone, ACDC from Parameters join LTRSignal ON cast(Parameters.Command as int) = LTRSignal.SignalId where ParamName = @ParamName");
      command.Parameters.AddWithValue("@ParamName", paramName);
      lock (_connect)
      {
          SqlDataReader dr = ExecuteReader(command);
          try
          {
              if (!dr.HasRows)
                  return false;

              dr.Read();
              slot = Convert.ToInt32(dr[0]);
              channel = Convert.ToInt32(dr[1]);
              range = Convert.ToDouble(dr[2]);
              alone = Convert.ToBoolean(dr[3]);
              ACDC = Convert.ToBoolean(dr[4]);
              return true;
          }
          catch (Exception)
          {
              return false;
          }
          finally
          {
              dr.Close();
          }
      }
    }

    public Dictionary<int, string> GetDevices()
    {
      Dictionary<int, string> devices = new Dictionary<int, string>();
      SqlCommand command = new SqlCommand("select DeviceTypeId, DeviceName from DeviceType");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;
          while (dr.Read())
              devices.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
          return devices;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public Dictionary<int, string> GetDevicesShort()
    {
	    Dictionary<int, string> devices = new Dictionary<int, string>();
      SqlCommand command = new SqlCommand("select DeviceTypeId, DeviceShortName from DeviceType");
			SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;
          while (dr.Read())
              devices.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
          return devices;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public Dictionary<int, string> GetModules()
    {
      Dictionary<int, string> devices = new Dictionary<int, string>();
      SqlCommand command = new SqlCommand("select ModuleId, ModuleName from Module");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;
          while (dr.Read())
              devices.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
          return devices;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public List<KeyValuePair<string, string>> GetModulesWithPorts()
    {
      List<KeyValuePair<string, string>> devices = new List<KeyValuePair<string, string>>();
      SqlCommand command =
        new SqlCommand(
          "select ModuleName, PortName from Module join Port ON Module.PortId = Port.PortID where Port.PortID >= 2");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;
          while (dr.Read())
              devices.Add(new KeyValuePair<string, string>(dr[0].ToString(), dr[1].ToString()));
          return devices;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public DataTable GetAllParams()
    {
      DataTable dt = new DataTable();
      SqlCommand command = new SqlCommand("select * from Parameters");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;

          for (int i = 0; i < dr.FieldCount; i++)
              dt.Columns.Add(dr.GetName(i));
          while (dr.Read())
          {
              dt.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8], dr[9]);
          }
          return dt;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public int InsertNewParam(string paramName, int deviceId, string commandStr, double koef, double minValue,
                                double maxValue, int precision, string note, string addCommand)
    {
      SqlCommand command =
        new SqlCommand(
          "insert Parameters select @ParamName, @deviceId, @Command, @Koef, @MinValue, @MaxValue, @Precision, @Note, @AddCommand");
      command.Parameters.AddWithValue("@ParamName", paramName);
      command.Parameters.AddWithValue("@deviceId", deviceId);
      command.Parameters.AddWithValue("@Command", commandStr);
      command.Parameters.AddWithValue("@AddCommand", addCommand);
      command.Parameters.AddWithValue("@Koef", koef);
      command.Parameters.AddWithValue("@MinValue", minValue);
      command.Parameters.AddWithValue("@MaxValue", maxValue);
      command.Parameters.AddWithValue("@Precision", precision);
      command.Parameters.AddWithValue("@Note", note);
      if (!ExecuteNonQuery(command))
        return 0;
      command.CommandText = "select ParamId from Parameters where ParamName = @ParamName";
      object ans = ExecuteScalar(command);
      if (ans == null)
        return 0;
      return (int) ans;
    }

    public bool UpdateParamCommand(string paramName, string commandStr)
    {
      SqlCommand command = new SqlCommand("update Parameters set Command = @Command where ParamName = @ParamName");
      command.Parameters.AddWithValue("@ParamName", paramName);
      command.Parameters.AddWithValue("@Command", commandStr);
      return ExecuteNonQuery(command);
    }

    public bool UpdateParam(int paramId, string paramName, int deviceId, string commandStr, double koef,
                              double minValue, double maxValue, int precision, string note, string addCommand)
    {
      SqlCommand command =
        new SqlCommand(
          "update Parameters set ParamName = @ParamName, DeviceId = @DeviceId, Command = @Command, Koef = @Koef, MinValue = @MinValue, MaxValue = @MaxValue, Precision = @Precision, Note = @Note, AddCommand = @AddCommand where ParamId = @paramId");
      command.Parameters.AddWithValue("@ParamId", paramId);
      command.Parameters.AddWithValue("@ParamName", paramName);
      command.Parameters.AddWithValue("@DeviceId", deviceId);
      command.Parameters.AddWithValue("@Command", commandStr);
      command.Parameters.AddWithValue("@AddCommand", addCommand);
      command.Parameters.AddWithValue("@Koef", koef);
      command.Parameters.AddWithValue("@minValue", minValue);
      command.Parameters.AddWithValue("@maxValue", maxValue);
      command.Parameters.AddWithValue("@Precision", precision);
      command.Parameters.AddWithValue("@Note", note);
      return ExecuteNonQuery(command);
    }

    public DataTable GetAllLTRParams()
    {
      DataTable dt = new DataTable();
      SqlCommand command = new SqlCommand("select * from LTRSignal order by Slot, Channel");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
          if (!dr.HasRows)
              return null;
          for (int i = 0; i < dr.FieldCount; i++)
              dt.Columns.Add(dr.GetName(i));
          while (dr.Read())
          {
              dt.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
          }
          return dt;
      }
      catch (Exception)
      {
          return null;
      }
      finally
      {
          dr.Close();
      }
    }

    public int InsertNewLTRSignal(int slot, int channel, double range, bool alone, bool ACDC, string note)
    {
      if (String.IsNullOrEmpty(note))
        note = null;
      SqlCommand command = new SqlCommand("insert LTRSignal select @Slot, @Channel, @Range, @Alone, @ACDC, @Note");
      command.Parameters.AddWithValue("@Slot", slot);
      command.Parameters.AddWithValue("@Channel", channel);
      command.Parameters.AddWithValue("@Range", range);
      command.Parameters.AddWithValue("@Alone", alone);
      command.Parameters.AddWithValue("@ACDC", ACDC);
      if (note == null)
        command.Parameters.AddWithValue("@Note", DBNull.Value);
      else
        command.Parameters.AddWithValue("@Note", note);
      if (!ExecuteNonQuery(command))
        return 0;
      command.CommandText =
        "select SignalId from LTRSignal where Slot = @Slot and Channel = @Channel and Range = @Range and Alone = @Alone and ACDC = @ACDC";
      object ans = ExecuteScalar(command);
      if (ans == null)
        return 0;
      return (int) ans;
    }

    public bool UpdateLTRSignal(int id, int slot, int channel, double range, bool alone, bool ACDC, string note)
    {
      if (String.IsNullOrEmpty(note))
        note = null;
      SqlCommand command =
        new SqlCommand(
          "update LTRSignal set Slot = @Slot, Channel = @Channel, Range = @Range, Alone = @Alone, ACDC = @ACDC, Note = @Note where SignalId = @SignalId");
      command.Parameters.AddWithValue("@SignalId", id);
      command.Parameters.AddWithValue("@Slot", slot);
      command.Parameters.AddWithValue("@Channel", channel);
      command.Parameters.AddWithValue("@Range", range);
      command.Parameters.AddWithValue("@Alone", alone);
      command.Parameters.AddWithValue("@ACDC", ACDC);
      if (note == null)
        command.Parameters.AddWithValue("@Note", DBNull.Value);
      else
        command.Parameters.AddWithValue("@Note", note);
      return ExecuteNonQuery(command);
    }

    public int GetChannelByName(string moduleName, string channelName)
    {
      const string sChannelName = "@Name";
      const string sModuleName = "@ModuleName";
      SqlCommand command =
        new SqlCommand(
          string.Format(
            "select ChannelNum from Channel join Module ON Module.ModuleId = Channel.ModuleId where ChannelName = {0} AND ModuleName = {1}",
            sChannelName, sModuleName));
      command.Parameters.AddWithValue(sChannelName, channelName);
      command.Parameters.AddWithValue(sModuleName, moduleName);
      object answer = ExecuteScalar(command);
      if (answer == null) 
	      throw new Exception(string.Format("Channel {0} in module {1} not exists!", channelName, moduleName));
      return Convert.ToInt32(answer);
    }

    public bool GetControlTaskParamsByName(string name, ref int number, ref int amplitude, ref int shift)
    {
      const string sName = "@Name";
      SqlCommand command = new SqlCommand("select Number,Amplitude,Shift from ControlTask where Name = " + sName);
      command.Parameters.AddWithValue(sName, name);
      SqlDataReader dr = ExecuteReader(command);
      bool result = false;
      try
      {
        if (dr.Read())
        {
          number = Convert.ToInt32(dr[0]);
          amplitude = Convert.ToInt32(dr[1]);
          shift = Convert.ToInt32(dr[2]);
          result = true;
        }
      }
      finally
      {
        dr.Close();
      }
      return result;
    }

    public Dictionary<int, string> GetDiscreteInputsChannels()
    {
      return GetDeviceChannels("DiscretInputs");
    }

    public Dictionary<int, string> GetDeviceChannels(string module)
    {
      SqlCommand command =
        new SqlCommand(
          "select ChannelNum, ChannelName from Channel join Module ON Module.ModuleId = Channel.ModuleId where ModuleName = @ModuleName");
      command.Parameters.AddWithValue("@ModuleName", module);
      SqlDataReader dr = ExecuteReader(command);
      Dictionary<int, string> result = new Dictionary<int, string>();
      try
      {
        while (dr.Read())
        {
          result.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
        }
      }
      finally
      {
        dr.Close();
      }
      return result;
    }

    public Dictionary<int, string> GetAdv1760Channels()
    {
      return GetDeviceChannels("Adv1760");
    }

    /// <summary>
    /// сохраняет параметр по имени (если его ещё нету то вставляет в ином случае обновляет)
    /// </summary>
    /// <param name="testId">Номер проверки</param>
    /// <param name="paramName">Имя параметра</param>
    /// <param name="result">Булевое значение</param>
    /// <returns></returns>
    public bool SaveParameter(int testId, string paramName, bool result)
    {
      return SaveParameter(testId, paramName, result ? 1 : 0);
    }

    /// <summary>
    /// сохраняет параметр по имени (если его ещё нету то вставляет в ином случае обновляет)
    /// </summary>
    /// <param name="testId">Номер проверки</param>
    /// <param name="paramName">Имя параметра</param>
    /// <param name="result">Измеренное значение</param>
    /// <returns></returns>
    public bool SaveParameter(int testId, string paramName, double result)
    {
      const string sTestId = "@TestId";
      const string sParamName = "@ParamId";
      const string sResult = "@Result";
      SqlCommand command =
        new SqlCommand(
          string.Format(
            "select top 1 Result from MeasuringParam join Parameters ON Parameters.ParamId = MeasuringParam.ParamID where TestId = {0} AND ParamName =  {1}",
            sTestId, sParamName));
      command.Parameters.AddWithValue(sTestId, testId);
      command.Parameters.AddWithValue(sParamName, paramName);
      object ans = ExecuteScalar(command);
      command.Parameters.AddWithValue(sResult, result);
      if (ans == null) // вставляем
        command.CommandText =
          string.Format("insert MeasuringParam select {0},ParamId,{2} from Parameters where ParamName = {1}", sTestId,
                        sParamName, sResult);
      else //обновляем
        command.CommandText =
          string.Format(
            "update MeasuringParam set Result = {2} from MeasuringParam join Parameters ON Parameters.ParamId = MeasuringParam.ParamId where ParamName = {1} AND TestId = {0}",
            sTestId, sParamName, sResult);
      return ExecuteNonQuery(command);
    }

    public double GetParameterValue(int testId, string paramName)
    {
      const string sParamName = "@PName";
      const string sTestId = "@TestId";
      SqlCommand command = new SqlCommand();
      command.CommandText =
        string.Format(
          "select Result from MeasuringParam join Parameters ON Parameters.ParamId = MeasuringParam.ParamId where TestId = {0} AND ParamName = {1}",
          testId, sParamName);
      command.Parameters.AddWithValue(sParamName, paramName);
      command.Parameters.AddWithValue(sTestId, testId);
      object result = ExecuteScalar(command);
      if (result == null)
        return double.NaN;
      return Convert.ToDouble(result);
    }

    /// <summary>
    /// Выборка номеров устройств для новой проверки
    /// </summary>
    /// <param name="deviceTypeId"></param>
    /// <returns></returns>
    public List<string> GetUniqueDeviceNumbers(int deviceTypeId)
    {
      List<string> result = new List<string>();
      const string sDeviceId = "@DeviceId";
      SqlCommand command = new SqlCommand(
        "select distinct ProductNumber from Testing where DeviceTypeId = " + sDeviceId, _connect);
      command.Parameters.AddWithValue(sDeviceId, deviceTypeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(dr[0].ToString());
        }
        catch
        {
        }
        if (dr != null)
          dr.Close();
      }
      return result;
    }

    public int GetDeviceTypeIdByName(string deviceType)
    {
      const string sDeviceType = "@DeviceType";
      SqlCommand command = new SqlCommand("select DeviceTypeId from DeviceType where DeviceShortName = " + sDeviceType);
      command.Parameters.AddWithValue(sDeviceType, deviceType);
      object ans = ExecuteScalar(command);
      if (ans == null)
        return 0;
      return (int) ans;
    }

    public string GetDeviceShortNameById(int deviceTypeId)
    {
      const string sDeviceTypeId = "@DeviceTypeId";
      SqlCommand command = new SqlCommand("select DeviceShortName from DeviceType where DeviceTypeId = " + sDeviceTypeId);
      command.Parameters.AddWithValue(sDeviceTypeId, deviceTypeId);
      object ans = ExecuteScalar(command);
      if (ans == null)
        return String.Empty;
      return ans.ToString();
    }

    public List<string> GetUniqueHelicopterMarks(int deviceTypeId)
    {
      List<string> result = new List<string>();
      const string sDeviceId = "@DeviceId";
      SqlCommand command =
        new SqlCommand("select distinct HelicopterMark from Testing where DeviceTypeId = " + sDeviceId, _connect);
      command.Parameters.AddWithValue(sDeviceId, deviceTypeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(dr[0].ToString());
        }
        catch
        {
          if (dr != null)
            dr.Close();
          throw;
        }
        dr.Close();

      }
      return result;
    }

    public List<string> GetUniqueHelicopterSigns(int deviceTypeId)
    {
      List<string> result = new List<string>();
      const string sDeviceId = "@DeviceId";
      SqlCommand command =
        new SqlCommand("select distinct HelicopterSign from Testing where DeviceTypeId = " + sDeviceId, _connect);
      command.Parameters.AddWithValue(sDeviceId, deviceTypeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(dr[0].ToString());
        }
        catch
        {
          if (dr != null)
            dr.Close();
          throw;
        }
        dr.Close();
      }
      return result;
    }

    public List<string> GetUniqueHelicopterNumbers(int deviceTypeId)
    {
      List<string> result = new List<string>();
      const string sDeviceId = "@DeviceId";
      SqlCommand command =
        new SqlCommand("select distinct HelicopterNumber from Testing where DeviceTypeId = " + sDeviceId, _connect);
      command.Parameters.AddWithValue(sDeviceId, deviceTypeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(dr[0].ToString());
        }
        catch
        {
          if (dr != null)
            dr.Close();
          throw;
        }
        dr.Close();

      }
      return result;
    }

    public Dictionary<int, string> GetEmployeesForFilter()
    {
      Dictionary<int, string> result = new Dictionary<int, string>();
      SqlCommand command = new SqlCommand("SELECT Distinct Employee.EmployeeId, dbo.FullNameShortForm(Employee.EmployeeId) from Employee join Testing ON Employee.EmployeeID = Testing.EmployeeID", _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
        }
        catch
        {
          if (dr != null)
            dr.Close();
          throw;
        }
        dr.Close();
      }
      return result;
    }

    public bool IsEmployeeExist(string name, string surname, string lastName)
    {
      string EmpFirstName = "@FName";
      string EmpSecondName = "@SName";
      string EmpLastName = "@LName";
      SqlCommand command = new SqlCommand(string.Format("SELECT EmployeeId from Employee where EmployeeFirstName = {0} AND EmployeeSecondName = {1} AND EmployeeLastName = {2}", EmpFirstName, EmpSecondName, EmpLastName));
      command.Parameters.AddWithValue(EmpFirstName, surname);
      command.Parameters.AddWithValue(EmpSecondName, name);
      command.Parameters.AddWithValue(EmpLastName, lastName);
      object answer = ExecuteScalar(command);
      if (answer == null)
        return false;
      return !answer.Equals(DBNull.Value);
    }

    public Dictionary<int, string> GetWorkingEmployees()
    {
      Dictionary<int, string> result = new Dictionary<int, string>();
      SqlCommand command =
        new SqlCommand("SELECT EmployeeId, dbo.FullNameShortForm(EmployeeId) from Employee where IsActive = 1", _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            result.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
        }
        catch
        {
          if (dr != null)
            dr.Close();
          throw;
        }
        dr.Close();
      }
      return result;
    }

    public bool UpdateTesting(int testId, string productNumber, DateTime dateMaking, string helicopterMark,
                                string helicopterSign, string helicopterNumber, DateTime dateRepair, int employeeId,
                                string note, string toFormName, int totalRun, int lastRun, 
                                int? mestreId = null, int? mestreBTKId = null)
    {
      SqlCommand command = new SqlCommand("UpdateTesting");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@TestId", testId);
      command.Parameters.AddWithValue("@ProductNumber", productNumber);
      command.Parameters.AddWithValue("@MakingDate", dateMaking);
      command.Parameters.AddWithValue("@HelicopterMark", helicopterMark);
      command.Parameters.AddWithValue("@HelicopterSign", helicopterSign);
      command.Parameters.AddWithValue("@HelicopterNumber", helicopterNumber);
      command.Parameters.AddWithValue("@TestDate", dateRepair);
      command.Parameters.AddWithValue("@EmployeeId", employeeId);
      command.Parameters.AddWithValue("@Note", note);
      if (String.IsNullOrEmpty(toFormName))
        command.Parameters.AddWithValue("@TOFormName", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TOFormName", toFormName);
      if (totalRun == 0)
        command.Parameters.AddWithValue("@TotalRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TotalRun", totalRun);
      if (lastRun == 0)
        command.Parameters.AddWithValue("@LastRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@LastRun", lastRun);
            command.Parameters.AddWithValue("@MestreName", mestreId);
            command.Parameters.AddWithValue("@MestreNameBTK", mestreBTKId);
      return ExecuteNonQuery(command);
    }

    public int? InsertTesting(int deviceTypeId, string productNumber, DateTime dateMaking, string helicopterMark,
                                string helicopterSign, string helicopterNumber, DateTime dateRepair, int employeeId,
                                string note, string toFormName, int totalRun, int lastRun, ModeApplication modeApplication, 
                                int? mestreNameId = null, int? mestreNameBTKId = null)
    { 
	    int testModeInt = (int)modeApplication;
      SqlCommand command = new SqlCommand("InsertTesting");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@DeviceTypeId", deviceTypeId);
      command.Parameters.AddWithValue("@ProductNumber", productNumber);
      command.Parameters.AddWithValue("@MakingDate", dateMaking);
      command.Parameters.AddWithValue("@HelicopterMark", helicopterMark);
      command.Parameters.AddWithValue("@HelicopterSign", helicopterSign);
      command.Parameters.AddWithValue("@HelicopterNumber", helicopterNumber);
      command.Parameters.AddWithValue("@TestDate", dateRepair);
      command.Parameters.AddWithValue("@EmployeeId", employeeId);
      command.Parameters.AddWithValue("@Note", note);
      command.Parameters.AddWithValue("@TestModeId", (testModeInt));
      if (String.IsNullOrEmpty(toFormName))
        command.Parameters.AddWithValue("@TOFormName", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TOFormName", toFormName);
      if (totalRun == 0)
        command.Parameters.AddWithValue("@TotalRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TotalRun", totalRun);
      if (lastRun == 0)
        command.Parameters.AddWithValue("@LastRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@LastRun", lastRun);
      object result = ExecuteScalar(command);
      if (result == null)
        return null;
      return Convert.ToInt32(result);
    }

    public int GetCurrentTestingId()
    {
      SqlCommand command = new SqlCommand("SELECT Max(Id) FROM Testing");
      object ans = ExecuteScalar(command);
      if (ans == null)
        throw new Exception();
      return (int) ans;
    }

    public bool GetTestings(int deviceTypeId, ref DataTable dtView)
    {
      const string sDeviceTypeId = "@DeviceTypeId";
      SqlCommand command =
        new SqlCommand(
          "SELECT ProductNumber, MakingDate, TestDate, dbo.FullNameShortForm(Testing.EmployeeId), HelicopterMark, HelicopterNumber, HelicopterSign, Testing.Note, Testing.EmployeeId, Id, DeviceTypeId FROM Testing left join Employee ON Employee.EmployeeId = Testing.EmployeeId where DeviceTypeId = " +
          sDeviceTypeId + " order by TestDate desc");
      command.Parameters.AddWithValue(sDeviceTypeId, deviceTypeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = ExecuteReader(command);
          while (dr.Read())
            dtView.Rows.Add(dr[0].ToString(), Convert.ToDateTime(dr[1]).ToShortDateString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(),
                            dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(),
                            dr[10].ToString());
          dr.Close();
        }
        catch
        {
          return false;
        }
        finally
        {
          if (dr != null)
            dr.Close(); 
        }
      }
      return true;
    }

    public bool GetTestings(int deviceTypeId, Modification deviceModification, ModeApplication modeApplication, ref DataTable dtView)
    {
      const string sDeviceTypeId = "@DeviceTypeId";
      const string paramName = "@ParamName";
      const string modName = "@ModName";
      const string testModePar = "@IdTestMode";
      string modStr = deviceModification.ToString();
      SqlCommand command;

      if (deviceModification != Modification.None)
      command =
        new SqlCommand(
          string.Format("SELECT ProductNumber, MakingDate, TestDate, dbo.FullNameShortForm(Testing.EmployeeId), HelicopterMark, HelicopterNumber, HelicopterSign, Testing.Note, Testing.EmployeeId, Id, DeviceTypeId FROM Testing left join Employee ON Employee.EmployeeId = Testing.EmployeeId join AdditionalParameter ap ON ap.TestId = Testing.Id  where ap.ParamId = (select ParamId from AdditionalParameterReference where ParamName = {0}) AND ap.Value = {1} AND DeviceTypeId = {2} AND IdTestMode = {3} order by TestDate desc",paramName, modName, sDeviceTypeId, (int)modeApplication));
      else
        command =
          new SqlCommand(
            string.Format("SELECT ProductNumber, MakingDate, TestDate, dbo.FullNameShortForm(Testing.EmployeeId), HelicopterMark, HelicopterNumber, HelicopterSign, Testing.Note, Testing.EmployeeId, Id, DeviceTypeId FROM Testing left join Employee ON Employee.EmployeeId = Testing.EmployeeId left join AdditionalParameter ap ON ap.TestId = Testing.Id AND ap.ParamId = (select ParamId from AdditionalParameterReference where ParamName = {0}) where ap.Value is NULL AND DeviceTypeId = {1} AND IdTestMode = {2} order by TestDate desc", paramName, sDeviceTypeId, (int)modeApplication));
      command.Parameters.AddWithValue(modName, modStr);
      command.Parameters.AddWithValue(paramName, AddParameters.Modification);
      command.Parameters.AddWithValue(sDeviceTypeId, deviceTypeId);
      command.Parameters.AddWithValue(testModePar, (int)modeApplication);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = ExecuteReader(command);
          if (dr == null)               // добавить проверку
              return false;
          while (dr.Read())
            dtView.Rows.Add(dr[0].ToString(), Convert.ToDateTime(dr[1]).ToShortDateString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(),
              dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(),
              dr[10].ToString());
          dr.Close();
        }
        catch
        {
          return false;
        }
        finally
        {
          if (dr != null)
            dr.Close();
        }
      }
      return true;
    }

    public bool DeleteTest(int testId)
    {
      const string sTestId = "@TestId";
      SqlCommand command = new SqlCommand("DeleteTesting");
      command.Parameters.AddWithValue(sTestId, testId);
      command.CommandType = CommandType.StoredProcedure;
      return ExecuteNonQuery(command);
    }

    public string[] GetTesting(int testID)
    {
      const int strCount = 7;
      string[] result = new string[strCount];
      const string sId = "@Id";
      SqlCommand command =
        new SqlCommand(
          "select ProductNumber, MakingDate, TestDate, dbo.FullNameShortForm(EmployeeId), HelicopterMark, HelicopterNumber, HelicopterSign from Testing where Id = " +
          sId, _connect);
      command.Parameters.AddWithValue(sId, testID);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
            for (int i = 0; i < strCount; i++)
              result[i] = dr[i].ToString();
          dr.Close();
        }
        catch
        {
          if (dr != null)
            dr.Close();
          return null;
        }
      }
      return result;
    }

    public DataTable GetReport(int testId)
    {
      DataTable result = new DataTable();
      const string sTestId = "@TestId";
      SqlCommand command = new SqlCommand("GetReportValues", _connect);
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue(sTestId, testId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          for (int col = 0; col < dr.FieldCount; col++)
            result.Columns.Add(dr.GetName(col));
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int col = 0; col < dr.FieldCount; col++)
              values[col] = dr[col];
            result.Rows.Add(values);
          }
          dr.Close();
        }
        catch
        {
          if (dr != null)
            dr.Close();
          return null;
        }
      }
      return result;
    }

    public void FillTesting(int testID, int modeId, DataTable dataTable)
    {
      const string sTestId = "@TestId";
      const string testModeId = "@TestModeId";
      SqlCommand command = new SqlCommand("SelectTesting", _connect);
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue(sTestId, testID);
      command.Parameters.AddWithValue(testModeId, modeId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int col = 0; col < dr.FieldCount; col++)
              values[col] = dr[col];
            dataTable.Rows.Add(values);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return;
    }

    public void FillComposition(int testID, DataTable dataTable)
    {
      const string sTestId = "@TestId";
      SqlCommand command =
        new SqlCommand(
          "select BlockShortName, No, Composition.LastRun, Composition.TotalRun from Composition join CompositionReference ON Composition.BlockId = CompositionReference.BlockId join Testing ON Testing.DeviceTypeId = CompositionReference.DeviceId AND Testing.Id = Composition.TestId where Testing.Id = " +
          sTestId, _connect);
      command.Parameters.AddWithValue(sTestId, testID);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (dataTable.Columns.Count == 0)
            for (int col = 0; col < dr.FieldCount; col++)
              dataTable.Columns.Add(dr.GetName(col));
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int col = 0; col < dr.FieldCount; col++)
              values[col] = dr[col];
            dataTable.Rows.Add(values);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return;
    }

    public void FillCompositionWithIdBlocks(int testID, DataTable dataTable)
    {
      const string sTestId = "@TestId";
      SqlCommand command =
        new SqlCommand(
          "select Composition.BlockId, BlockShortName, No, Composition.TotalRun, Composition.LastRun from Composition join CompositionReference ON Composition.BlockId = CompositionReference.BlockId join Testing ON Testing.DeviceTypeId = CompositionReference.DeviceId AND Testing.Id = Composition.TestId where Testing.Id = " +
          sTestId, _connect);
      command.Parameters.AddWithValue(sTestId, testID);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (dataTable.Columns.Count == 0)
            for (int col = 0; col < dr.FieldCount; col++)
              dataTable.Columns.Add(dr.GetName(col));
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int col = 0; col < dr.FieldCount; col++)
              values[col] = dr[col];
            dataTable.Rows.Add(values);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return;
    }

    public void FillCompositionReference(int deviceID, DataTable dataTable)
    {
      const string sDeviceId = "@DeviceId";
      SqlCommand command =
        new SqlCommand("select BlockId, BlockShortName  from CompositionReference where DeviceId = " + sDeviceId,
                       _connect);
      command.Parameters.AddWithValue(sDeviceId, deviceID);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          for (int col = 0; col < dr.FieldCount; col++)
            dataTable.Columns.Add(dr.GetName(col));
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int col = 0; col < dr.FieldCount; col++)
              values[col] = dr[col];
            dataTable.Rows.Add(values);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return;
    }

    public void FillParameters(int testID, DataTable dataTable)
    {
      DataTable dt = GetReport(testID);
      foreach (DataRow dr in dt.Rows)
      {
        dataTable.Rows.Add(dr[3], dr[4], dr[5], dr[6], dr[10], dr[13], dr[0], dr[1]);
      }
            
    }

        public void FillParametersNumber(int testId, DataTable parameters)
        {

            int number = 1;
            DataTable dt = GetReport(testId);
            foreach (DataRow dr in dt.Rows)
            {
                parameters.Rows.Add(dr[3], dr[4], dr[5], dr[6], dr[10], dr[13], dr[0], dr[1], number);
                number++;
            }

        }

        public bool UpdateComposition(int testId, int blockId, string blockNumber, int? totalRun, int? lastRun)
    {
      SqlCommand command = new SqlCommand("UpdateComposition");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@TestId", testId);
      command.Parameters.AddWithValue("@BlockId", blockId);
      command.Parameters.AddWithValue("@BlockNumber", blockNumber);
      if (totalRun == null)
        command.Parameters.AddWithValue("@TotalRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TotalRun", totalRun);
      if (lastRun == null)
        command.Parameters.AddWithValue("@LastRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@LastRun", lastRun);
      return ExecuteNonQuery(command);
    }

    public bool InsertComposition(int id, int blockId, string blockNumber, int? totalRun, int? lastRun)
    {
      SqlCommand command = new SqlCommand("InsertComposition");
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.AddWithValue("@TestId", id);
      command.Parameters.AddWithValue("@BlockId", blockId);
      command.Parameters.AddWithValue("@BlockNumber", blockNumber);
      if (totalRun == null)
        command.Parameters.AddWithValue("@TotalRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@TotalRun", totalRun);
      if (lastRun == null)
        command.Parameters.AddWithValue("@LastRun", DBNull.Value);
      else
        command.Parameters.AddWithValue("@LastRun", lastRun);
      return ExecuteNonQuery(command);
    }

    public List<string> GetTOForms()
    {
      List<string> result = new List<string>();
      SqlCommand command = new SqlCommand("select TOFormName from TOForm", _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          while (dr.Read())
            result.Add(dr[0].ToString());
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return result;
    }

    public DataTable GetDocumentLinks()
    {
      DataTable dtResult = new DataTable();
      SqlCommand command =
        new SqlCommand(
          "select Id, DeviceId, TestReferenceId, Text, Comment from DocumentLink order by DeviceId, TestReferenceId",
          _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          for (int i = 0; i < dr.FieldCount; i++)
            dtResult.Columns.Add(dr.GetName(i));
          while (dr.Read())
            dtResult.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4]);
        }
        catch
        {
        }
        if (dr != null)
          dr.Close();
      }
      return dtResult;
    }

    public Dictionary<int, string> GetTestReferences(int deviceId)
    {
      Dictionary<int, string> result = new Dictionary<int, string>();
      SqlCommand command =
        new SqlCommand(
          "select Item, CheckName from Report where DeviceTypeId = @DeviceId AND SubItem = 1 order by Item", _connect);
      command.Parameters.AddWithValue("@DeviceId", deviceId);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          while (dr.Read())
            result.Add((int) dr[0], dr[1].ToString());
        }
        catch
        {
        }
        if (dr != null)
          dr.Close();
        return result;
      }
    }

    public int InsertNewLink(int deviceId, int testReferenceId, string text, string comment)
    {
      SqlCommand command = new SqlCommand("insert DocumentLink select @DeviceId, @TestReferenceId, @Text, @Comment");
      command.Parameters.AddWithValue("@DeviceId", deviceId);
      command.Parameters.AddWithValue("@TestReferenceId", testReferenceId);
      command.Parameters.AddWithValue("@Text", text);
      if (String.IsNullOrEmpty(comment))
        command.Parameters.AddWithValue("@Comment", DBNull.Value);
      else
        command.Parameters.AddWithValue("@Comment", comment);
      if (!ExecuteNonQuery(command))
        return 0;
      command.CommandText =
        "select Id from DocumentLink where DeviceId = @DeviceId AND TestReferenceId = @TestReferenceId";
      object ans = ExecuteScalar(command);
      if (ans == null)
        return 0;
      return (int) ans;
    }

    public bool UpdateLink(int id, int deviceId, int testReferenceId, string text)
    {
      SqlCommand command =
        new SqlCommand(
          "update DocumentLink set DeviceId = @DeviceId, TestReferenceId = @TestReferenceId, Text = @Text where Id = @Id");
      command.Parameters.AddWithValue("@Id", id);
      command.Parameters.AddWithValue("@DeviceId", deviceId);
      command.Parameters.AddWithValue("@TestReferenceId", testReferenceId);
      command.Parameters.AddWithValue("@Text", text);
      return ExecuteNonQuery(command);
    }

    public void GetWorkingEmployees(ref DataTable dt)
    {
      SqlCommand command =
        new SqlCommand(
          "select EmployeeId, EmployeeNumber, EmployeeFirstName, EmployeeSecondName, EmployeeLastName, PostName, Employee.IsActive, Note from Employee left join Post ON Employee.PostId = Post.PostId where Employee.IsActive = 1",
          _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          for (int i = 0; i < dr.FieldCount; i++)
            dt.Columns.Add();
          while (dr.Read())
          {
            object[] param = new object[dr.FieldCount];
            for (int i = 0; i < dr.FieldCount; i++)
              param[i] = dr[i];
            dt.Rows.Add(param);
          }
          dr.Close();
        }
        catch
        {
          if (dr != null)
            dr.Close();
        }
      }
    }

    public MemoryStream GetEmployeePicture(int id)
    {
      const string sEmployeeId = "@EmployeeId";
      SqlCommand command = new SqlCommand(string.Format("select Image from Employee where EmployeeId = {0} AND Image is not NULL", sEmployeeId), _connect);
      command.Parameters.AddWithValue(sEmployeeId, id);
      MemoryStream ms = null;
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
          {
            dr.Close();
            return null;
          }
          dr.Read();
          Byte[] buffer = new Byte[1000000];
          dr.GetBytes(0, 0, buffer, 0, buffer.Length);
          bool result = false;
          foreach (byte b in buffer)
            if (b != 0)
              result = true;
          if (result)
            ms = new MemoryStream(buffer, 0, buffer.Length);
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
        }
      }
      return ms;
    }

    public void GetAllEmployees(ref DataTable dt)
    {
      SqlCommand command =
        new SqlCommand(
          "select EmployeeId, EmployeeNumber, EmployeeFirstName, EmployeeSecondName, EmployeeLastName, PostName, Employee.IsActive, Note from Employee left join Post ON Employee.PostId = Post.PostId",
          _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          if (!dr.HasRows)
            throw new Exception();
          for (int i = 0; i < dr.FieldCount; i++)
            dt.Columns.Add();
          while (dr.Read())
          {
            object[] param = new object[dr.FieldCount];
            for (int i = 0; i < dr.FieldCount; i++)
              param[i] = dr[i];
            dt.Rows.Add(param);
          }
          dr.Close();
        }
        catch
        {
          if (dr != null)
            dr.Close();
        }
      }
    }

    public bool InsertEmployee(string number, string surname, string name, string lastName, int postId, bool isActive,
                                 string note, byte[] picture)
    {
      const string sNumber = "@Number";
      const string sSurname = "@Surname";
      const string sName = "@Name";
      const string sLastName = "@LastName";
      const string sPostId = "@PostId";
      const string sIsActive = "@IsActive";
      const string sImage = "@Image";
      const string sNote = "@Note";
      SqlCommand command =
        new SqlCommand(string.Format("insert Employee select {0},{1},{2},{3},{4},{5},{6},{7}", sNumber, sSurname, sName,
                                     sLastName, sIsActive, sImage, sNote, sPostId));
      command.Parameters.AddWithValue(sNumber, number);
      command.Parameters.AddWithValue(sSurname, surname);
      command.Parameters.AddWithValue(sName, name);
      command.Parameters.AddWithValue(sLastName, lastName);
      command.Parameters.AddWithValue(sIsActive, isActive);
      command.Parameters.Add(sImage, SqlDbType.Binary);
      if (picture == null)
        command.Parameters[sImage].Value = DBNull.Value;
      else
        command.Parameters[sImage].Value = picture;
      command.Parameters.AddWithValue(sNote, note);
      command.Parameters.AddWithValue(sPostId, postId == 0? DBNull.Value : (object)postId);
      return ExecuteNonQuery(command);
    }

    public bool GetPosts(ref DataTable dt)
    {
      SqlCommand command = new SqlCommand("select PostId, PostName, IsActive from Post where IsActive = 1", _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
          {
            dt.Rows.Add(dr[0], dr[1], dr[2]);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
          return false;
        }
      }
      return true;
    }

    public bool GetAllPosts(ref DataTable dt)
    {
      SqlCommand command = new SqlCommand("select PostId, PostName, IsActive from Post", _connect);
      lock (_connect)
      {
        SqlDataReader dr = null;
        try
        {
          dr = command.ExecuteReader();
          while (dr.Read())
          {
            dt.Rows.Add(dr[0], dr[1], dr[2]);
          }
          dr.Close();
        }
        catch (Exception)
        {
          if (dr != null)
            dr.Close();
          return false;
        }
      }
      return true;
    }

    public bool UpdateEmployee(int id, string number, string surname, string name, string lastName, int postId,
                                 bool isActive, string note)
    {
      const string sId = "@Id";
      const string sNumber = "@Number";
      const string sSurname = "@Surname";
      const string sName = "@Name";
      const string sLastName = "@LastName";
      const string sPostId = "@PostId";
      const string sIsActive = "@IsActive";
      const string sNote = "@Note";
      SqlCommand command =
        new SqlCommand(
          string.Format(
            "update Employee set EmployeeNumber = {0}, EmployeeFirstName = {1}, EmployeeSecondName = {2}, EmployeeLastName = {3}, IsActive = {4}, Note = {5}, PostId = {6} where EmployeeId = {7}",
            sNumber, sSurname, sName, sLastName, sIsActive, sNote, sPostId, sId));
      command.Parameters.AddWithValue(sId, id);
      command.Parameters.AddWithValue(sNumber, number);
      command.Parameters.AddWithValue(sSurname, surname);
      command.Parameters.AddWithValue(sName, name);
      command.Parameters.AddWithValue(sLastName, lastName);
      command.Parameters.AddWithValue(sIsActive, isActive);
      command.Parameters.AddWithValue(sNote, note);
      command.Parameters.AddWithValue(sPostId, postId);
      return ExecuteNonQuery(command);
    }

    public bool UpdateEmployeePicture(int id, byte[] picture)
    {
      const string sId = "@Id";
      const string sImage = "@Image";
      SqlCommand command =
        new SqlCommand(string.Format("update Employee set Image = {0} where EmployeeId = {1}", sImage, sId));
      command.Parameters.AddWithValue(sId, id);
      command.Parameters.AddWithValue(sImage, picture);
      return ExecuteNonQuery(command);
    }

    public bool IsPostExist(string postName)
    {
      const string sPostName = "@PostName";
      SqlCommand command = new SqlCommand( string.Format("select PostId from Post where PostName = {0}", sPostName));
      command.Parameters.AddWithValue(sPostName, postName);
      object ans =  ExecuteScalar(command);
      return ans != null;
    }

    public bool InsertPost(string postName)
    {
      const string sPostName = "@PostName";
      SqlCommand command =
        new SqlCommand(
          string.Format(" if not EXISTS (select * from Post where PostName = {0}) insert Post select {0}, 1", sPostName));
      command.Parameters.AddWithValue(sPostName, postName);
      return ExecuteNonQuery(command);
    }

    public bool UpdatePost(int postId, string postName, bool isActive)
    {
      const string sPostId = "@PostId";
      const string sPostName = "@PostName";
      const string sIsActive = "@IsActive";
      SqlCommand command =
        new SqlCommand(string.Format("update Post set PostName = {0}, IsActive = {1} where PostId = {2}", sPostName,
                                     sIsActive, sPostId));
      command.Parameters.AddWithValue(sPostId, postId);
      command.Parameters.AddWithValue(sPostName, postName);
      command.Parameters.AddWithValue(sIsActive, isActive);
      return ExecuteNonQuery(command);
    }

    public bool UpdateReportParam(int id, int itemNum, int subItemNum, string orderNum, string checkName, string units,
                                    string requirement, string paramName)
    {
      const string sId = "@Id";
      const string sItemNum = "@ItemNum";
      const string sSubItemNum = "@SubItemNum";
      const string sOrderNum = "@OrderNum";
      const string sCheckName = "@CheckName";
      const string sUnits = "@Units";
      const string sRequirement = "@Requirement";
      const string sParamName = "@ParamName";
      const string sParamId = "@ParamId";
      SqlCommand cmd = new SqlCommand("select ParamId from Parameters where ParamName = " + sParamName);
      cmd.Parameters.AddWithValue(sParamName, paramName);
      object ans = ExecuteScalar(cmd);
      int paramId;
      if (ans == null)
        paramId = 0;
      else
        paramId = (int) ans;
      cmd =
        new SqlCommand(
          string.Format(
            "update Report set Item = {1}, SubItem = {2}, OrderNumber = {3}, CheckName = {4}, Units = {5}, Requirement = {6}, ParamId = {7} where Id = {0}",
            sId, sItemNum, sSubItemNum, sOrderNum, sCheckName, sUnits, sRequirement, sParamId));
      cmd.Parameters.AddWithValue(sId, id);
      cmd.Parameters.AddWithValue(sItemNum, itemNum);
      cmd.Parameters.AddWithValue(sSubItemNum, subItemNum);
      if (String.IsNullOrEmpty(orderNum))
        cmd.Parameters.AddWithValue(sOrderNum, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sOrderNum, orderNum);
      cmd.Parameters.AddWithValue(sCheckName, checkName);
      if (String.IsNullOrEmpty(units))
        cmd.Parameters.AddWithValue(sUnits, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sUnits, units);
      cmd.Parameters.AddWithValue(sRequirement, requirement);
      if (paramId == 0)
        cmd.Parameters.AddWithValue(sParamId, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sParamId, paramId);
      return ExecuteNonQuery(cmd);
    }

    public bool UpdateReportParamLang(int id, int itemNum, int subItemNum, string orderNum, string checkName, string units,
                                    string requirement, string paramName, string checkNameLang, string requirementLang, string lang)
    {
      const string sId = "@Id";
      const string sItemNum = "@ItemNum";
      const string sSubItemNum = "@SubItemNum";
      const string sOrderNum = "@OrderNum";
      const string sCheckName = "@CheckName";
      const string sUnits = "@Units";
      const string sRequirement = "@Requirement";
      const string sParamName = "@ParamName";
      const string sParamId = "@ParamId";
      const string sCheckNameLang = "@CheckNameLang";
      const string sRequirementLang = "@RequirementLang";
      const string sLangId = "@LangId";

      
      SqlCommand cmd = new SqlCommand("select ParamId from Parameters where ParamName = " + sParamName);
      cmd.Parameters.AddWithValue(sParamName, paramName);
      object ans = ExecuteScalar(cmd);
      int paramId;
      if (ans == null)
        paramId = 0;
      else
        paramId = (int)ans;
      cmd =
        new SqlCommand(
          string.Format(
            "update Report set Item = {1}, SubItem = {2}, OrderNumber = {3}, CheckName = {4}, Units = {5}, Requirement = {6}, ParamId = {7} where Id = {0}",
            sId, sItemNum, sSubItemNum, sOrderNum, sCheckName, sUnits, sRequirement, sParamId));
      cmd.Parameters.AddWithValue(sId, id);
      cmd.Parameters.AddWithValue(sItemNum, itemNum);
      cmd.Parameters.AddWithValue(sSubItemNum, subItemNum);
      if (String.IsNullOrEmpty(orderNum))
        cmd.Parameters.AddWithValue(sOrderNum, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sOrderNum, orderNum);
      cmd.Parameters.AddWithValue(sCheckName, checkName);
      if (String.IsNullOrEmpty(units))
        cmd.Parameters.AddWithValue(sUnits, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sUnits, units);
      cmd.Parameters.AddWithValue(sRequirement, requirement);
      if (paramId == 0)
        cmd.Parameters.AddWithValue(sParamId, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sParamId, paramId);
      bool result =  ExecuteNonQuery(cmd);

      int langId = GetLanguageId(lang);
      cmd.Parameters.AddWithValue(sLangId, langId);

      if (!String.IsNullOrEmpty(checkNameLang))
      {
        cmd.CommandText = string.Format(
          "if (EXISTS(select * from Translate join Report ON Report.Id = Translate.PropId where LangId = {1} and TableId = 1 and Id = {0})) update Translate set Text = {2} from Translate where LangId = {1} and TableId = 1 and PropId = {0} else insert Translate select {1}, 1, {0}, {2}",
          sId, sLangId, sCheckNameLang);

        cmd.Parameters.AddWithValue(sCheckNameLang, checkNameLang);
        result &= ExecuteNonQuery(cmd);
      }

      if (!String.IsNullOrEmpty(requirementLang))
      {
        cmd.CommandText = string.Format(
          "if (EXISTS(select * from Translate join Report ON Report.Id = Translate.PropId where LangId = {1} and TableId = 2 and Id = {0})) update Translate set Text = {2} from Translate where LangId = {1} and TableId = 2 and PropId = {0} else insert Translate select {1}, 2, {0}, {2}",
          sId, sLangId, sRequirementLang);
        cmd.Parameters.AddWithValue(sRequirementLang, requirementLang);
        result &= ExecuteNonQuery(cmd);
      }

      return result;
    }

    public int InsertReportParam(int deviceId, int itemNum, int subItemNum, string orderNum, string checkName,
                                   string units, string requirement, string paramName)
    {
      const string sDevId = "@DevId";
      const string sItemNum = "@ItemNum";
      const string sSubItemNum = "@SubItemNum";
      const string sOrderNum = "@OrderNum";
      const string sCheckName = "@CheckName";
      const string sUnits = "@Units";
      const string sRequirement = "@Requirement";
      const string sParamName = "@ParamName";
      SqlCommand cmd =
        new SqlCommand(
          string.Format(
            "insert Report select {0},{1},{2},{3},{4},{5},{6}, (select ParamId from Parameters where ParamName = {7})",
            sDevId, sItemNum, sSubItemNum, sOrderNum, sCheckName, sUnits, sRequirement, sParamName));
      cmd.Parameters.AddWithValue(sDevId, deviceId);
      cmd.Parameters.AddWithValue(sItemNum, itemNum);
      cmd.Parameters.AddWithValue(sSubItemNum, subItemNum);
      if (String.IsNullOrEmpty(orderNum))
        cmd.Parameters.AddWithValue(sOrderNum, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sOrderNum, orderNum);
      cmd.Parameters.AddWithValue(sCheckName, checkName);
      if (String.IsNullOrEmpty(units))
        cmd.Parameters.AddWithValue(sUnits, DBNull.Value);
      else
        cmd.Parameters.AddWithValue(sUnits, units);
      cmd.Parameters.AddWithValue(sRequirement, requirement);
      cmd.Parameters.AddWithValue(sParamName, paramName);
      ExecuteNonQuery(cmd);
      cmd = new SqlCommand("select max(Id) from Report");
      object ans = ExecuteScalar(cmd);
      if (ans == null)
        return 0;
      return (int) ans;
    }

    public DataTable GetAllReportParams(int deviceId)
    {
      const string sDevTypeId = "@DevTypeId";
      DataTable dt = new DataTable();
      SqlCommand command =
        new SqlCommand(string.Format("select Id, Item, SubItem, OrderNumber, CheckName, Units, Requirement, ParamName from Report left join Parameters ON Report.ParamId = Parameters.ParamId where DeviceTypeId={0} order by Item, SubItem", sDevTypeId));
      command.Parameters.AddWithValue(sDevTypeId, deviceId);
      SqlDataReader dr = ExecuteReader(command);
      try
      {
        if (!dr.HasRows)
        {
          dr.Close();
          return null;
        }
        for (int i = 0; i < dr.FieldCount; i++)
          dt.Columns.Add(dr.GetName(i));
        while (dr.Read())
        {
          dt.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7]);
        }
        dr.Close();
        return dt;
      }
      catch (Exception)
      {
        if (dr != null)
          dr.Close();
        return null;
      }
    }

    public DataTable GetAllReportParamsLang(int deviceId, string language)
    {
      string currentLanguage = GetCurrentLanguage();
      ChangeLanguage(language);
      const string sDevTypeId = "@DevTypeId";
      DataTable dt = new DataTable();
      SqlCommand command =
        new SqlCommand(string.Format("select Report.Id, Report.Item, Report.SubItem, Report.OrderNumber, Report.CheckName, Report.Units, Report.Requirement, ParamName, ReportLang.CheckName as CheckNameLang, ReportLang.Requirement as RequirementLang from Report left join Parameters ON Report.ParamId = Parameters.ParamId left join ReportLang ON Report.Id = ReportLang.Id where Report.DeviceTypeId={0} order by Item, SubItem", sDevTypeId));
      command.Parameters.AddWithValue(sDevTypeId, deviceId);
      SqlDataReader dr = ExecuteReader(command);
      try
      {
        if (!dr.HasRows)
          return null;

        for (int i = 0; i < dr.FieldCount; i++)
          dt.Columns.Add(dr.GetName(i));
        while (dr.Read())
        {
          object[] values = new object[dr.FieldCount];
          for (int i = 0; i < dr.FieldCount; i++)
            values[i] = dr[i];
          dt.Rows.Add(values);
        }
        return dt;
      }
      catch (Exception)
      {
        return null;
      }
      finally
      {
          dr.Close();
          ChangeLanguage(currentLanguage);
      }
    }

    public double GetPassportValue(int testId, int number)
    {
      lock (_connect)
      {
        double value = double.NaN;

        SqlCommand command = new SqlCommand("EXEC GetPassportValue @TestId, @Number", _connect);
        command.Parameters.AddWithValue("@TestId", testId);
        command.Parameters.AddWithValue("@Number", number);
        SqlDataReader reader = command.ExecuteReader();
        try
        {
          if (reader.Read())
          {
            value = !reader.IsDBNull(0) ? decimal.ToSingle(reader.GetDecimal(0)) : double.NaN;
          }
        }
        finally
        {
          reader.Close();
        }


        return value;
      }
    }

    public DataTable GetPassportValues(object testId)
    {
      DataTable result = new DataTable();
      string sTestId = "@TestId";
      SqlCommand cmd = new SqlCommand("SelectPassportValues", _connect);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(sTestId, testId);
      lock (_connect)
      {
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
          if (dr.RecordsAffected != 0)
          {
            for (int i = 0; i < dr.FieldCount; i++)
              result.Columns.Add(dr.GetName(i));
            while (dr.Read())
            {
              object[] values = new object[dr.FieldCount];
              for (int i = 0; i < dr.FieldCount; i++)
                values[i] = dr[i];
              result.Rows.Add(values);
            }
          }
          dr.Close();
        }
        catch
        {
          dr.Close();
        }
      }
      return result;
    }

    public bool SetPassportValue(int testId, int paramId, double value)
    {
      string sTestId = "@TestId";
      string sParamId = "@ParamId";
      string sValue = "@Value";

      SqlCommand cmd = new SqlCommand("SetPassportValue");
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(sTestId, testId);
      cmd.Parameters.AddWithValue(sParamId, paramId);
      cmd.Parameters.AddWithValue(sValue, value);
      return ExecuteNonQuery(cmd);
    }

    public int GetMaxTestId()
    {
      SqlCommand cmd = new SqlCommand("select Max(Id) from Testing");
      return Convert.ToInt32(ExecuteScalar(cmd));
    }

    public const int PS115VModuleId = 1;
    public const int KZModuleId = 4;
    public const int GenSinCosModuleId = 5;

    public int GetKZChannelShift(int channel)
    {
      const string channelParam = "@Channel";
      SqlCommand cmd =
        new SqlCommand(string.Format("select Value from Calibration where ModuleId = {0} and Channel = {1}", KZModuleId,
                                     channelParam));
      cmd.Parameters.AddWithValue(channelParam, channel);
      object result = ExecuteScalar(cmd);
      if (result == null)
        return -1;
      return Convert.ToInt32(result);
    }

    public DataTable GetCalibration(int moduleId, int channel)
    {
      const string moduleIdStr = "@ModuleId";
      const string channelStr = "@Channel";
      DataTable dtResult = new DataTable();
      SqlCommand cmd =
        new SqlCommand(string.Format("select * from Calibration where ModuleId = {0} and Channel = {1}", moduleIdStr,
                                     channelStr));
      cmd.Parameters.AddWithValue(moduleIdStr, moduleId);
      cmd.Parameters.AddWithValue(channelStr, channel);
      SqlDataReader dr = null;
      try
      {
        dr = ExecuteReader(cmd);
        if (dr.HasRows)
        {
          for (int i = 0; i < dr.FieldCount; i++)
            dtResult.Columns.Add(dr.GetName(i));
          while (dr.Read())
          {
            object[] values = new object[dr.FieldCount];
            for (int i = 0; i < dr.FieldCount; i++)
              values[i] = dr[i];
            dtResult.Rows.Add(values);
          }
        }
      }
      finally
      {
        if (dr != null)
          dr.Close();
      }

      return dtResult;
    }

    public bool SaveKZChannelShift(int channel, int amplitude)
    {
      SqlCommand cmd = new SqlCommand("SaveCalibration");
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue("@ModuleId", KZModuleId);
      cmd.Parameters.AddWithValue("@Channel", channel);
      cmd.Parameters.AddWithValue("@IdealValue", 0);
      cmd.Parameters.AddWithValue("@Value", amplitude);
      cmd.Parameters.AddWithValue("@Check", 1);
      return ExecuteNonQuery(cmd);
    }

    public int SaveCalibration(int moduleId, int channel, double idealValue, int amplitude)
    {
      const string moduleIdStr = "@ModuleId";
      const string channelStr = "@Channel";
      const string idealValueStr = "@IdealValue";
      const string valueStr = "@Value";
      SqlCommand cmd = new SqlCommand("SaveCalibration");
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(moduleIdStr, moduleId);
      cmd.Parameters.AddWithValue(channelStr, channel);
      cmd.Parameters.AddWithValue(idealValueStr, idealValue);
      cmd.Parameters.AddWithValue(valueStr, amplitude);
      cmd.Parameters.AddWithValue("@Check", 0);
      bool result = ExecuteNonQuery(cmd);
      if (!result)
        return 0;
      cmd =
        new SqlCommand(
          string.Format(
            "select Id from Calibration where ModuleId = {0} AND Channel = {1} AND Idealvalue = {2} AND Value = {3}",
            moduleIdStr, channelStr, idealValueStr, valueStr));
      cmd.Parameters.AddWithValue(moduleIdStr, moduleId);
      cmd.Parameters.AddWithValue(channelStr, channel);
      cmd.Parameters.AddWithValue(idealValueStr, idealValue);
      cmd.Parameters.AddWithValue(valueStr, amplitude);
      object ans = ExecuteScalar(cmd);
      if (ans == null)
        return 0;
      return Convert.ToInt32(ans);
    }

    public bool UpdateCalibration(int id, double idealValue, int amplitude)
    {
      const string IdStr = "@Id";
      const string idealValueStr = "@IdealValue";
      const string valueStr = "@Value";
      SqlCommand cmd = new SqlCommand("UpdateCalibration");
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(IdStr, id);
      cmd.Parameters.AddWithValue(idealValueStr, idealValue);
      cmd.Parameters.AddWithValue(valueStr, amplitude);
      return ExecuteNonQuery(cmd);
    }

    public void RemoveCalibration(int id)
    {
      SqlCommand cmd = new SqlCommand(string.Format("delete from Calibration where Id = {0}", id));
      ExecuteNonQuery(cmd);
    }


    public bool ClearDB()
    {
      const string TruncateTb = "TRUNCATE TABLE dbo.";
      const string ClearTb = "DELETE FROM dbo.";

      const string TbEmployees = "Employee";
      const string TbComposition = "Composition";
      const string TbTesting = "Testing";
      const string TbMeasuringParam = "MeasuringParam";
      const string TbPost = "Post";
      const string TbTOForm = "TOForm";

      List<string> tlist = new List<string>();
      tlist.Add(TbTesting);
      tlist.Add(TbEmployees);
      tlist.Add(TbMeasuringParam);
      tlist.Add(TbPost);
      tlist.Add(TbComposition);
      tlist.Add(TbTOForm);
      foreach (string sTable in tlist)
      {
        if (!ExecuteNonQuery(TruncateTb + sTable))
          if (!ExecuteNonQuery(ClearTb + sTable))
            return false;
      }
      return true;
    }

    public bool RemoveComposition(int testId, int blockId)
    {
      SqlCommand command = new SqlCommand("delete from Composition where TestId = @TestId AND BlockId = @BlockId");
      command.Parameters.AddWithValue("@TestId", testId);
      command.Parameters.AddWithValue("@BlockId", blockId);
      return ExecuteNonQuery(command);
    }

    public bool GetParamLimit(int paramId, ref double min, ref double max)
    {
      const string ParamIdStr = "@ParamId";
      SqlCommand command =
        new SqlCommand(string.Format("select MinValue, MaxValue from Parameters where ParamId = {0}", ParamIdStr));
      command.Parameters.AddWithValue(ParamIdStr, paramId);
      SqlDataReader dr = ExecuteReader(command);
      try
      {
        if (!dr.HasRows)
        {
          dr.Close();
          return false;
        }
        dr.Read();
        min = Convert.ToSingle(dr[0] != DBNull.Value ? dr[0] : Single.NaN);
        max = Convert.ToSingle(dr[1] != DBNull.Value ? dr[1] : Single.NaN);
        dr.Close();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public const string ParamTypeVSS = "TypeVSS";

    public string GetAddParam(int testId, string paramName)
    {
      const string paramNameStr = "@ParamName";
      const string testIdStr = "@TestId";
      SqlCommand command =
        new SqlCommand(
          "select [Value] from AdditionalParameter ad join AdditionalParameterReference adr ON ad.ParamId = adr.ParamId where TestId = @TestId and ParamName = @ParamName");
      command.Parameters.AddWithValue(testIdStr, testId);
      command.Parameters.AddWithValue(paramNameStr, paramName);
      object answer = ExecuteScalar(command);
      if (answer == null)
        return String.Empty;
      return answer.ToString();
    }

    public bool SetAddParam(int testId, string paramName, string value)
    {
      const string paramNameStr = "@ParamName";
      const string testIdStr = "@TestId";
      const string valueStr = "@Value";
      SqlCommand command;
      if (string.IsNullOrEmpty(paramName))
      {
        command = new SqlCommand("delete AdditionalParameter where testId = @TestId and ParamId IN (select ParamId from AdditionalParameterReference where ParamName = @ParamName)");
        command.Parameters.AddWithValue(testIdStr, testId);
        command.Parameters.AddWithValue(paramNameStr, paramName);
        ExecuteNonQuery(command);
        return true;
      }
      command = new SqlCommand( "if (EXISTS(select * from AdditionalParameter ad join AdditionalParameterReference adr ON ad.ParamId = adr.ParamId where TestId = @TestId and ParamName = @ParamName)) update AdditionalParameter set [Value] = @Value from AdditionalParameter ad join AdditionalParameterReference adr ON ad.ParamId = adr.ParamId where TestId = @TestId and ParamName = @paramName else insert AdditionalParameter select @TestId, ParamId, @Value from AdditionalParameterReference where ParamName = @ParamName");
      command.Parameters.AddWithValue(testIdStr, testId);
      command.Parameters.AddWithValue(paramNameStr, paramName);
      command.Parameters.AddWithValue(valueStr, value);
      return ExecuteNonQuery(command);
    }

    public void GetMinAndMaxTestDate(out DateTime dtMin, out DateTime dtMax)
    {
      dtMin = new DateTime();
      dtMax = new DateTime();
      SqlCommand command = new SqlCommand("select Max(TestDate), Min(TestDate) from Testing");
      SqlDataReader dr = ExecuteReader(command);
      try
      {
        if (!dr.HasRows)
          return;
        dr.Read();
        if (DBNull.Value != dr[1])
          dtMin = Convert.ToDateTime(dr[1]);
        else
          dtMin = DateTime.Now;
        if (DBNull.Value != dr[0])
          dtMax = Convert.ToDateTime(dr[0]);
        else
          dtMax = DateTime.Now;
      }
      finally
      {
        dr.Close();
      }
    }

    public int GetLastCheckDissId(out Modification mod, int testModeId)
    {
      string nameParamTestModeId = "@TestModeId";
      string ParamNameStr = "@ParamName";
      mod = Modification.None;
      SqlCommand cmd = new SqlCommand("SelectLastTestingType", _connect);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(nameParamTestModeId, testModeId);
      cmd.Parameters.AddWithValue(ParamNameStr, AddParameters.Modification);
      object[] answer = null;
        lock (_connect)//todo сделать отдельным методом
        {
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    answer = new object[dr.FieldCount];
                    for (int col = 0; col < dr.FieldCount; col++)
                        answer[col] = dr[col];
                }
                dr.Close();
            }
            catch (Exception)
            {
                if (dr != null)
                    dr.Close();
                answer = null;
            }
        }

      if (answer == null)
        return 0;
      int result = Convert.ToInt32(answer[0]);
            if (!String.IsNullOrEmpty(answer[1].ToString()))
                mod = (Modification)Enum.Parse(typeof(Modification), answer[1].ToString());
            
      //cmd = new SqlCommand(string.Format("select ap.Value from Testing left join AdditionalParameter ap ON ap.TestId = Testing.Id AND ap.ParamId = (select ParamId from AdditionalParameterReference where ParamName = {0})  where Id = (select max(id) from Testing)",ParamNameStr));
      //cmd.Parameters.AddWithValue(ParamNameStr, AddParameters.Modification);
      //answer = ExecuteScalar(cmd);
      //  if (answer != null)
      //      if (!String.IsNullOrEmpty(answer.ToString()))
      //          mod = (Modification)Enum.Parse(typeof(Modification), answer.ToString());
      return result;
    }

    public bool SaveTestingNote(int testId, string note)
    {
      const string TestIdParam = "@TestId";
      const string NoteParam = "@Note";
      SqlCommand cmd = new SqlCommand(string.Format("update Testing set Note = {1} where Testing.Id = {0}",TestIdParam, NoteParam));
      cmd.Parameters.AddWithValue(TestIdParam, testId);
      cmd.Parameters.AddWithValue(NoteParam, note);
      return ExecuteNonQuery(cmd);
    }

    public string GetTestingNote(int testId)
    {
      SqlCommand cmd = new SqlCommand(string.Format("select Note from Testing where Testing.Id = {0}", testId));
      object answer = ExecuteScalar(cmd);
      if (answer == null)
        return String.Empty;
      return answer.ToString();
    }

    /// <summary>
    /// Возвращает результат проведения проверки
    /// </summary>
    /// <param name="testID">Номер проверки</param>
    /// <param name="isManualResult">Признак ручного выставления результата, возвращаемое значение</param>
    /// <returns></returns>
    public bool? GetTestResult(int testID, ref bool isManualResult)
    {
      bool? result = null;
      SqlCommand cmd = new SqlCommand(string.Format("select Result, IsManualResult from Testing where Testing.Id = {0}", testID));
      SqlDataReader sdr = ExecuteReader(cmd);
      try
      {
          if (sdr.Read())
          {
              if (sdr[0] != DBNull.Value)
                  result = (bool) sdr[0];
              isManualResult = (bool) sdr[1];
          }
      }
      finally
      {
          sdr.Close();
      }
      return result;
    }

    public bool SetTestResult(int testID, bool testResult, bool isManualResult)
    {
      const string ResultStr = "@Result";
      const string IsManualResultStr = "@IsManualResult";
      SqlCommand cmd = new SqlCommand(string.Format("update Testing set Result = {0}, isManualResult = {1} where Testing.Id = {2}", ResultStr, IsManualResultStr, testID));
      cmd.Parameters.AddWithValue(ResultStr, testResult);
      cmd.Parameters.AddWithValue(IsManualResultStr, isManualResult);
      return ExecuteNonQuery(cmd);
    }

    public string GetPortByModuleId(int moduleId)
    {
      const string ModuleIdStr = "@ModuleId";
      SqlCommand cmd = new SqlCommand(string.Format("select PortName from Port join Module on Module.PortId = Port.PortID where ModuleId = {0}", ModuleIdStr));
      cmd.Parameters.AddWithValue(ModuleIdStr, moduleId);
      return ExecuteScalar(cmd).ToString();
    }

    public string GetFormTO(int _testId)
    {
      const string TestIdStr = "@TestId";
      SqlCommand cmd = new SqlCommand(string.Format("select TOFormName from Testing join TOForm on Testing.TOFormId = TOForm.TOFormId where Id = {0}", TestIdStr));
      object result = ExecuteScalar(cmd);
      if (result == null)
        return null;
      return result.ToString();
    }

    /// <summary>
    ///   Изменяет язык БД
    /// </summary>
    /// <param name="lang"></param>
    /// <returns></returns>
    public bool ChangeLanguage(string lang)
    {
      lock (_connect)
      {
        try
        {
          const string langParam = "@Lang";
          SqlCommand command = _connect.CreateCommand();
          command.CommandText = "UPDATE Lang SET ShortName = " + langParam + " where Id = 0";
          command.Parameters.Add(langParam, SqlDbType.VarChar);
          command.Parameters[langParam].Value = lang;
          command.ExecuteNonQuery();
          return true;
        }
        catch
        {
          return false;
        }
      }
    }

    public string GetCurrentLanguage()
    {
      return ExecuteScalar(new SqlCommand("select ShortName from Lang where Id = 0")).ToString();
    }

    public int GetLanguageId(string lang)
    {
      const string sLang = "@LangName";
      SqlCommand cmd = new SqlCommand(string.Format("select Id from Lang where ShortName = {0} AND Id <> 0", sLang));
      cmd.Parameters.AddWithValue(sLang, lang);
      return Convert.ToInt32(ExecuteScalar(cmd));
    }

    public List<string> GetAllACTypes()
    {
      List<string> result = new List<string>();
      DataTable tb = GetACTypes();
      for (int i=0;i<tb.Rows.Count;i++)
        result.Add(tb.Rows[i][1].ToString());
      return result;
    }

    /// <summary>
    /// Возвращает все типы 
    /// </summary>
    /// <returns></returns>
    public DataTable GetACTypes()
    {
      DataTable tb = new DataTable();
      try
      {
        SqlCommand cmd = new SqlCommand("select Id, ACTypeName from ACType");
        SqlDataReader dr = ExecuteReader(cmd);
        for (int i = 0; i < dr.FieldCount; i++)
          tb.Columns.Add(dr.GetName(i));
        while (dr.Read())
        {
          var values = new object[dr.FieldCount];
          for (int i = 0; i < dr.FieldCount; i++)
            values[i] = dr[i];
          tb.Rows.Add(values);
        }
        dr.Close();
      }
      catch
      {

      }
      return tb;
    }

    /// <summary>
    /// Добавляет тип воздушного судна
    /// </summary>
    /// <param name="acType"> название </param>
    /// <returns></returns>
    public bool AddACType(string acType)
    {
      SqlCommand cmd = new SqlCommand(string.Format("insert ACType select '{0}'", acType));
      return ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Правит тип воздушного судна
    /// </summary>
    /// <param name="Id"> идентификатор </param>
    /// <param name="acType"> название </param>
    /// <returns></returns>
    public bool CorrectACType(int Id, string acType)
    {
      const string AcTypeParam = "@ACType";
      const string ACTypeIdParam = "@Id";
      SqlCommand cmd = new SqlCommand(string.Format("update ACType set ACTypeName = {0} where Id = {1}", AcTypeParam, ACTypeIdParam));
      cmd.Parameters.AddWithValue(ACTypeIdParam, Id);
      cmd.Parameters.AddWithValue(AcTypeParam, acType);
      return ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Удаляет тип воздушного судна
    /// </summary>
    /// <param name="Id"> идентификатор </param>
    /// <returns></returns>
    public bool RemoveAcType(int Id)
    {
      const string ACTypeIdParam = "@Id";
      SqlCommand cmd = new SqlCommand(string.Format("delete from ACType where Id = {0}", ACTypeIdParam));
      cmd.Parameters.AddWithValue(ACTypeIdParam, Id);
      return ExecuteNonQuery(cmd);
    }
  }
}
