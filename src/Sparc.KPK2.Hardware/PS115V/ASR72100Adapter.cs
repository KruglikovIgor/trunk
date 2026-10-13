using System;
using System.IO.Ports;
using System.Threading;
using PowerSupplyGWI_ASR72100;
using PowerSupplyGWI_ASR72100.ASRCommands;
using PowerSupplyGWI_ASR72100.Transport;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS115V
{
    internal class ASR72100Adapter : IPower115V
    {
        private ASR72100 _power115V;
        private bool _init = false;
        private const int DelayToMeasVoltage = 500;
        private const double MaxVoltageError = 0.1;
        private string _ipAddr;
        private int _connectionType;
        private PSVisaLanTransport _transportLan;
        private string _defaultPort;
        private string _alternatePort;
        private int _alternateBaudRate;
        SerialPort _port = new SerialPort();

        public ASR72100Adapter(string ipAddr)
        {
            _ipAddr = Power115V.GetIPAddress();
            _connectionType = 1;
        }

        public ASR72100Adapter(string defaultPort, string alternatePort)
        {
            _defaultPort = Power115V.GetDefaultPort();
            _alternatePort = Power115V.GetAlternatePort();
            _alternateBaudRate = Power115V.GetAlternateBaudRate();
        }

        public bool Connect()
        {
            if (_connectionType == 1)
            {
                try
                {
                    _transportLan = new PSVisaLanTransport(_ipAddr, 2268);
                    _power115V = new ASR72100(_transportLan);
                    _transportLan.OpenConnection();
                    _init = _transportLan.IsConnected;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(_defaultPort))
                {
                    _port = SpManager.Get(_defaultPort);
                    if (CheckConnection(_port, PSComTransport.DefaultBaudRate) ||
                        CheckConnection(_port, _alternateBaudRate) ||
                        CheckConnection(SpManager.Get(_alternatePort), _alternateBaudRate))
                    {
                        _init = true;
                        return true;
                    }
                }
                var ports = SerialPort.GetPortNames();
                for (int i = ports.Length - 1; i > 0; i--)
                {
                    for (int bd = 0; bd < 2; bd++) //Перебираем скорости
                    {
                        _port = SpManager.Get(ports[i]);
                        var savedBaudRate = _port.BaudRate;
                        try
                        {
                            if (bd == 1 && PSComTransport.DefaultBaudRate == _alternateBaudRate)
                                break;
                            _port.BaudRate = bd == 0 ? PSComTransport.DefaultBaudRate : _alternateBaudRate;
                            var transport = new PSComTransport(_port);

                            transport.OpenConnection();
                            try
                            {
                                _power115V = new ASR72100(transport);
                                if (_power115V.CheckIdent())
                                {
                                    _init = true;
                                    Power115V.SetDefaultPort(_port.PortName);
                                    return true;
                                }
                            }
                            finally
                            {
                                if (!_init)
                                {
                                    transport.CloseConnection();
                                    _port.BaudRate = savedBaudRate; //Возвращаем обычную скорость
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            _port.BaudRate = savedBaudRate; //Возвращаем обычную скорость
                        }
                    }
                }
                return _init;
            }
        }

        private bool CheckConnection(SerialPort inPort, int baudRate)
        {
            inPort.BaudRate = baudRate;
            var transport = new PSComTransport(inPort);
            transport.OpenConnection();
            try
            {
                _power115V = new ASR72100(transport);
                if (_power115V.CheckIdent())
                {
                    Power115V.SetDefaultPort(inPort.PortName);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (!_init)
                    transport.CloseConnection();
            }

            return false;

        }

        public bool SetVoltageSimple(double voltage)
        {
            if (!_init)
                return false;

            try
            {
                _power115V.SetVoltage(voltage);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void SetVoltageWOCorrection(double voltage)
        {
            if (!_init)
                return;
            _power115V.SetVoltage(voltage);
        }

        public void Disconnect()
        {
            _port.Close();
        }

        public string GetIdentString()
        {
            return _power115V.GetIdentStr();
        }

        public bool SetVoltageInner(double voltage)
        {

            if (!_init)
                return false;
            try
            {
                _power115V.SetVoltage(voltage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private static bool CheckMeasuredVoltage(double voltage, double curVoltage)
        {
            return curVoltage >= (voltage - MaxVoltageError) && curVoltage <= (voltage + MaxVoltageError);
        }

        public bool SetPhase(double phase)
        {
            return true;
        }

        public bool GetOutputState()
        {
            return _power115V.GetOutputState();
        }

        public double GetVoltage(bool correct)
        {
            if (!_init)
                return double.NaN;
            try
            {

                double voltage = _power115V.MeasureVoltageRMS();
                if (correct)
                    CorrectVoltageByCurrent(ref voltage);
                return voltage;
            }
            catch
            {
                return double.NaN;
            }
        }

        private void CorrectVoltageByCurrent(ref double voltage)
        {
            double current = _power115V.MeasureCurrentRMS();
            double downVoltage = current * LineResistance;
            voltage -= downVoltage;
        }

        public double GetClearVoltage()
        {
            return GetVoltage(false);
        }

        public double GetVoltage()
        {
            return GetVoltage(true);
        }

        public double GetSettedVoltage()
        {
            return _power115V.GetVoltage();
        }

        public bool GetStatus()
        {
            return true;
        }

        public double GetCurrent()
        {
            return _power115V.MeasureCurrentRMS();
        }

        public bool SetOnOff(bool state)
        {
            try
            {
                _power115V.SetOutputState(state);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SetPermitOnOff(bool state)
        {
            return true;
        }

        public bool SetCurrentLimit(double current)
        {
            try
            {
                _power115V.SetCurrentRMSLimit(current);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SetMaxVoltageRange(bool state)
        {
            try
            {
                _power115V.SetVoltageRange(VoltageRange.AUTO);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SetFrequency(double frequency)
        {
            try
            {
                _power115V.SetFrequency(frequency);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public double GetFrequency()
        {
            return _power115V.GetFrequency();
        }

        public bool IsConnected()
        {
            return _init;
        }

        public double GetCurrentLimit()
        {
            return _power115V.GetCurrentRMSLimit();
        }

        public double LineResistance { get; set; }
    }
}
