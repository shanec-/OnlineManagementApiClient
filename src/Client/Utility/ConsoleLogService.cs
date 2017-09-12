using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Utility
{
    public class ConsoleLogService : ILog
    {
        private ILog _log = null;

        public ConsoleLogService()
        {
        }

        public ConsoleLogService(ILog log)
        {
            this._log = log;
        }

        public void Debug(string message)
        {
            Console.WriteLine($"[Debug]:{message}");
            _log?.Debug(message);
        }

        public void Error(string message)
        {
            Console.WriteLine($"[Error]:{message}");
            _log?.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine($"[Error]:{message}. Exception: /n/tMessage: {exception.Message} StackTrace: {exception.StackTrace}");
            _log?.Error(message, exception);
        }

        public void Information(string message)
        {
            Console.WriteLine($"[Information]:{message}");
            _log?.Information(message);
        }
    }
}
