using System.Collections.Generic;
using System.IO.Ports;
using System;
using NLog;
using Sparc.Hardware.LambdaGEN;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Config.Settings;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
    // класс для открытого управления портами
    public class SpManager
    {
        //пул открытых портов
        private static readonly List<SerialPort> Pool = new List<SerialPort>();

        //либо возвращает ссылку на порт из пула, либо если его там нет, открывает, помещает в пул и отдает ссылку
        //если неудалось открыть = возращает null
        public static SerialPort Open(string name)
        {
           // bool Opened = false;
            try
            {
                foreach (SerialPort port in Pool)
                {
                    if (port.PortName != name) continue;
                    if (!port.IsOpen)
                        port.Open();
                    if (!port.IsOpen)
                        throw new Exception("Невозможно открыть порт " + port.PortName);
                    return port;
                }
            }
            catch
            {
                return null;
            }
            if (Globals.Emulation)
                return new SerialPort();

            SerialPort sPort = new SerialPort(name);            
            
                try
                {
                    DataManager.Instance.SetPortParameters(ref sPort);                    
                    sPort.Open();
                    if (!sPort.IsOpen)
                        throw new Exception("Невозможно открыть порт " + sPort.PortName);
                    Pool.Add(sPort);
                    return sPort;
                }
                catch (Exception)
                {
                    return null;
                }                        
        }

        /// <summary>
        ///   либо возвращает ссылку на порт из пула, либо если его там нет, открывает, помещает в пул и отдает ссылку
        ///   если неудалось открыть = возращает null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SerialPort Open(SerialPortElement portConfig)
        {
            try
            {
                foreach (SerialPort port in Pool)
                {
                    if (port.PortName != portConfig.PortName) continue;
                    if (!port.IsOpen)
                        port.Open();
                    if (!port.IsOpen)
                        throw new Exception("Невозможно открыть порт " + port.PortName);
                    return port;
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex.ToString());
                throw;
            }

            SerialPort sPort = SettingsHelper.CreatePortFromConfig(portConfig);

            try
            {
                sPort.Open();
                if (!sPort.IsOpen)
                    throw new Exception("Невозможно открыть порт " + sPort.PortName);
                Pool.Add(sPort);
                return sPort;
            }
            catch 
            {
                return null;
            }
        }

        //либо возвращает ссылку на порт из пула, либо если его там нет, открывает, помещает в пул и отдает ссылку
        //если неудалось открыть = возращает null
        public static SerialPort Get(string name)
        {
            // bool Opened = false;
            try
            {
                foreach (SerialPort port in Pool)
                {
                    if (port.PortName != name) continue;
                    return port;
                }
            }
            catch
            {
                return null;
            }
            if (Globals.Emulation)
                return new SerialPort();

            SerialPort sPort = new SerialPort(name);

            try
            {
                DataManager.Instance.SetPortParameters(ref sPort);
                Pool.Add(sPort);
                return sPort;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //закрывает и удаляет порт из пула
        public static bool Close(string name)
        {
            if (Globals.Emulation)
                return true;
            for (int i = 0; i < Pool.Count; i++)
                if (Pool[i].PortName == name)
                {
                    Pool[i].Close();
                    Pool.RemoveAt(i);
                    return true;
                }
            return false;                                
        }        

        public static void CloseAll()
        {            
            lock (Pool)
            {
                for (int i = 0; i < Pool.Count; i++)
                    Pool[i].Close();             
                Pool.Clear();
            }
        }
    }
}
