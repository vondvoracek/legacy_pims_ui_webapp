using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Logging
{
    public interface ILoggerService
    {
        void Debug(string message, string arg = null);
        void Info(string message, string arg = null);
        void Warn(string message, string arg = null);
        void Error(string message, Exception e = null);
    }
}
