using System.Threading;
using NLog;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS115V
{
    public static class Power115VExtension
    {
        private const double MaxVoltageError = 0.1;
        private const int DelayToMeasVoltage = 500;
        private static Logger _logger = LogManager.GetLogger("KPK2Trace");

        public static bool SetVoltageGradually(this IPower115V power115V, double voltage)
        {
            try
            {
                if (!power115V.GetOutputState())
                {
                    if (!power115V.SetVoltageInner(voltage / 4))
                        return false;
                    if (!power115V.SetOnOff(true))
                        return false;
                    Thread.Sleep(4000);
                    for (int i = 2; i <= 4; i++)
                    {
                        try
                        {
                            power115V.SetVoltageInner(voltage / 4 * i);
                                
                        }
                        catch { }
                        Thread.Sleep(600);
                    }
                }

                return power115V.SetVoltage(voltage);
            }
            catch
            {
                return false;
            }
        }

        public static bool CorrectVoltageIfBigError(this IPower115V power115V, double voltage, double curVoltage)
        {
            if (power115V is AktorAdapter)
                return true;
            if (!CheckMeasuredVoltage(voltage, curVoltage))
            {
                bool result = CorrectVoltage(power115V, voltage, curVoltage);
                Thread.Sleep(DelayToMeasVoltage);
                return result;
            }
            return false;
        }

        public static bool CheckMeasuredVoltage(double voltage, double curVoltage)
        {
            return curVoltage >= (voltage - MaxVoltageError) && curVoltage <= (voltage + MaxVoltageError);
        }

        public static bool CorrectVoltage(this IPower115V power115V, double voltage, double rmsValue)
        {
            if (power115V is AktorAdapter)
                return true;

            double curValue = power115V.GetSettedVoltage();
            _logger.Trace("Корректировка: Установленное: {0} Измеренное: {1} Требуемое: {2}", curValue, rmsValue, voltage);
            try
            {
                var setVoltage = curValue / rmsValue * voltage;
                //Проверка на адекватность напряжения после формулы, не разрешаем делать корректировку больше 15 процентов
                //Иначе получается если измеритель сломается мы может запулить вообще любое напряжение на устройство
                if (setVoltage > voltage + voltage * 0.15 || setVoltage < voltage - voltage * 0.15)
                {
                    _logger.Trace("Устанавливаемое напряжение слишком большое! " + setVoltage);
                    return false;
                }

                power115V.SetVoltageWOCorrection(setVoltage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CorrectVoltage(this IPower115V power115V, double voltage)
        {
            if (power115V is AktorAdapter)
                return true;

            double rmsValue = power115V.GetVoltage();
            return CorrectVoltage(power115V, voltage, rmsValue);
        }

        public static bool SetVoltage(this IPower115V power115V, double voltage)
        {
            try
            {
                power115V.SetVoltageInner(voltage);
                if (!power115V.GetOutputState())
                {
                    power115V.SetOnOff(true);
                    Thread.Sleep(4000); //Увеличено время с 3-х до 4-х секунд т.к. бывали случаи что не хватало времени
                }
                else
                    Thread.Sleep(DelayToMeasVoltage);
                const int maxSteps = 11;
                int step = 0;
                double curVoltage = power115V.GetVoltage();
                while (!CheckMeasuredVoltage(voltage, curVoltage) && (step <= maxSteps))
                {
                    CorrectVoltage(power115V, voltage);
                    Thread.Sleep(DelayToMeasVoltage);
                    curVoltage = power115V.GetVoltage();
                    step++;
                }
                return CheckMeasuredVoltage(voltage, curVoltage);
            }
            catch
            {
                return false;
            }
        }
    }
}
