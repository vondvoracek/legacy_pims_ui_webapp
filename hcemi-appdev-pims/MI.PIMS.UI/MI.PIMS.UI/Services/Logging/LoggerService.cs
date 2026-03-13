using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Logging
{
    public class LoggerService : ILoggerService
    {
        private static LoggerService instance;
        private static Logger logger; // = LogManager.GetLogger("pimsLoggerFile");

        public LoggerService(){}

        public static LoggerService GetInstance()
        {
            if (instance == null)
                instance = new LoggerService();
            return instance;
        }

        private Logger GetLogger(string theLogger)
        {
            if (logger == null)
                logger = LogManager.GetLogger(theLogger);
            return logger;
        }

        public void Debug(string message, string arg = null)
        {
            string sanitizedMessage = message.Replace(Environment.NewLine, "").Replace("\n", "");
            if (arg == null)
                GetLogger("pimsLogger").Debug(sanitizedMessage);
            else
                GetLogger("pimsLogger").Debug(sanitizedMessage, arg);
        }

        public void Error(string message, Exception e = null)
        {
            string sanitizedMessage = message.Replace(Environment.NewLine, "").Replace("\n", "");

            if (e == null)
                GetLogger("pimsLogger").Error(sanitizedMessage);
            else
                GetLogger("pimsLogger").Error(e, sanitizedMessage);
        }

        public void Info(string message, string arg = null)
        {
            string sanitizedMessage = message.Replace(Environment.NewLine, "").Replace("\n", "");
            if (arg == null)
                GetLogger("pimsLogger").Info(sanitizedMessage);
            else
                GetLogger("pimsLogger").Info(sanitizedMessage, arg);
        }


        public void Warn(string message, string arg = null)
        {
            string sanitizedMessage = message.Replace(Environment.NewLine, "").Replace("\n", "");
            if (arg == null)
                GetLogger("pimsLogger").Warn(sanitizedMessage);
            else
                GetLogger("pimsLogger").Warn(sanitizedMessage, arg);
        }

    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
