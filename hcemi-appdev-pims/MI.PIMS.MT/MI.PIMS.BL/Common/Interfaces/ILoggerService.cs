using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Common
{
    public interface ILoggerService
    {
        void Debug(string message, string arg = null);
        void Info(string message, string arg = null);
        void Warn(string message, string arg = null);
        void Error(string message, Exception e = null);
    }
}
