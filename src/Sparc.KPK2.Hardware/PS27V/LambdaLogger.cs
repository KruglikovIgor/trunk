using System;
using NLog;
using ILogger = Sparc.Hardware.LambdaGEN.Log.ILogger;

namespace Sparc.KPK2.Hardware.PS27V
{
    internal class LambdaLogger : ILogger
    {
        //private Logger _logger = LogManager.GetCurrentClassLogger();
        private static Logger _logger = LogManager.GetLogger("KPK2Error");
        public void Log(string message)
        {
            _logger.Error(message);
        }

        public void Log(string message, Exception exception)
        {
            _logger.Error(message);
            _logger.Error(exception.ToString());
        }
    }
}
