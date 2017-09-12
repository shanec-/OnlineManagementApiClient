using System;
using System.IO;

namespace OnlineManagementApiClient.Utility
{
    public class FileLogService : ILog
    {
        private ILog _log = null;
        private readonly string _filePath;

        public FileLogService()
        {
            this._filePath = "Log.txt";
        }

        public FileLogService(string filePath)
        {
            this._filePath = filePath;
        }

        public FileLogService(ILog log, string filePath)
        {
            this._log = log;
            this._filePath = filePath;
        }

        public void Debug(string message)
        {
            File.AppendAllLines(_filePath, new[] { $"[Debug]:{message}" });
            _log?.Debug(message);
        }

        public void Error(string message)
        {
            File.AppendAllLines(_filePath, new[] { $"[Error]:{message}" });
            _log?.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            File.AppendAllLines(_filePath, new[] { $"[Error]:{message}. Exception: /n/tMessage: {exception.Message} StackTrace: {exception.StackTrace}" });
            _log?.Error(message, exception);
        }

        public void Information(string message)
        {
            File.AppendAllLines(_filePath, new[] { $"[Information]:{message}" });
            _log?.Information(message);
        }
    }
}
