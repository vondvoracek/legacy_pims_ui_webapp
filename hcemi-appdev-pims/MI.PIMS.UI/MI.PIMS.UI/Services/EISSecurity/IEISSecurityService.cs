using MI.PIMS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services
{
    public interface IEISSecurityService
    {
        void SetAppInfo(ref LogEvent logEvent);
        void SetSourceHost(ref LogEvent logEvent);
        void SetSourceUser(ref LogEvent logEvent);
        void LogLogin();
        void LogRequestData(string requestWithParameters, int rowCount);
        void LogApplicationError(string error);
        void LogSpParams(string sp_name, string gLoginID, string serviceParams, int rowCount);
        void LogParams(string paramList, string gLoginID, int rowCount);
    }
}
