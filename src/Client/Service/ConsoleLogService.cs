using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service
{
    public class ConsoleLogService : ILog
    {
        public void Debug(string message)
        {
            Console.WriteLine($"[Debug]:{message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"[Error]:{message}");
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine($"[Error]:{message}. Exception: /n/tMessage: {exception.Message} StackTrace: {exception.StackTrace}");
        }

        public void Information(string message)
        {
            Console.WriteLine($"[Information]:{message}");
        }
    }
}
