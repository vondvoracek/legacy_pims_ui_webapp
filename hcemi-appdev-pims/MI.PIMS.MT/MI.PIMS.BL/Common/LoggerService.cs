using MI.PIMS.UI.Common;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Common
{
    public class LoggerService : ILoggerService
    {
        private static LoggerService instance;
        private static Logger logger; // = LogManager.GetLogger("pimsLoggerFile");

        public LoggerService() { }

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
            if (arg == null)
                GetLogger("pimsLogger").Debug(message);
            else
                GetLogger("pimsLogger").Debug(message, arg);
        }

        public void Error(string message, Exception e = null)
        {
            if (e == null)
                GetLogger("pimsLogger").Error(message.Replace(Environment.NewLine, ""));
            else
                GetLogger("pimsLogger").Error(e, message.Replace(Environment.NewLine, ""));
        }

        public void Info(string message, string arg = null)
        {
            if (arg == null)
                GetLogger("pimsLogger").Info(TextSanitizer.Sanitize(message.Replace(Environment.NewLine, "")));
            else
                GetLogger("pimsLogger").Info(TextSanitizer.Sanitize(message.Replace(Environment.NewLine, "")), arg);
        }


        public void Warn(string message, string arg = null)
        {
            string Safe(string input) =>
                    input?.Replace("\r", "\\r").Replace("\n", "\\n");

            var safeMessage = Safe(message);
            var safeArg = Safe(arg);

            if (safeArg == null)
                GetLogger("pimsLogger").Warn(safeMessage);
            else
                GetLogger("pimsLogger").Warn(safeMessage, safeArg);
        }
    }
}
