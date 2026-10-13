using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sparc.KPK2.NFunctions
{
    public class ConnectionManager 
    {
        private SqlConnection _connect;
        private static ConnectionManager _instance;

        public static string GetAvailibaleInstanceFromServer(string defConnStr)
        {
            string instanceStr = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(defConnStr);
            builder.InitialCatalog = "master";
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
                            _instance = new ConnectionManager();
                            _instance._connect =
                                new SqlConnection(builder.ConnectionString);
                            _instance._connect.Open();
                            if (_instance._connect.State == ConnectionState.Open)
                            {
                                instanceStr = _instance._connect.DataSource;
                                _instance._connect.Close();
                                _instance = null;
                                break;
                            }
                        }
                        catch
                        {
                            _instance = null;
                        }
                    }
                }
            }

            return instanceStr;
        }

        public static string GetDBConnectionString(string defConnStr, string instance)
        {
            string connStr = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(defConnStr);
            try
            {
                builder.DataSource = instance;
                _instance = new ConnectionManager();
                _instance._connect = new SqlConnection(builder.ConnectionString);
                _instance._connect.Open();
                if (_instance._connect.State == ConnectionState.Open)
                {
                    connStr = _instance._connect.ConnectionString;
                    _instance._connect.Close();
                    _instance = null;
                    return connStr;
                }
            }
            catch
            {
                _instance = null;
            }

            return connStr;
        }
    }
}
