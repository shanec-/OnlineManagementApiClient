using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service
{
    interface ILog
    {
        void Information(string message);
        void Debug(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
}
