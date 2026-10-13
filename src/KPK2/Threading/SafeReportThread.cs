using System;
using System.Threading;

namespace KPK2.Threading
{    
    /// <summary>
    /// Класс для безопасного (в try catch) запуска функции в отдельном потоке
    /// с возвратом отчётов о выполнении и возникших ошибках
    /// </summary>    
    public class SafeReportThread
    {
        public delegate void ReportProgress(string text);
        public delegate void ThreadFunctionDelegate(ReportProgress report);
        public delegate void ErrorDelegate(Exception ex);
        public delegate void EndThreadDelegate();

        private readonly ErrorDelegate _errorEvent;
        private readonly EndThreadDelegate _threadFinished;
        private readonly ThreadFunctionDelegate _func;
        private readonly ReportProgress _report;

        SafeReportThread(ThreadFunctionDelegate func, ReportProgress report, ErrorDelegate error, EndThreadDelegate endThread)
        {
            _func = func;
            _report = report;
            _errorEvent = error;
            _threadFinished = endThread;
        }

        public static void Start(ThreadFunctionDelegate func, ReportProgress report, ErrorDelegate error, EndThreadDelegate endThread)
        {
            if (func == null)
                return;
            SafeReportThread srt = new SafeReportThread(func, report, error, endThread);
            srt.StartThread();
        }

        void StartThread()
        {
            Thread t = new Thread(WorkThread);
            t.Start();
        }

        void WorkThread()
        {
            try
            {                
                _func(InsideReportProgress);
                if (_threadFinished != null)
                    _threadFinished.BeginInvoke(null, null);
            }
            catch (Exception ex)
            {
                if (_errorEvent != null)
                    _errorEvent.BeginInvoke(ex,null,null);
            }
        }

        void InsideReportProgress(string text)
        {
            if (_report != null)
                _report.BeginInvoke(text,null,null);
        }
    }
}
