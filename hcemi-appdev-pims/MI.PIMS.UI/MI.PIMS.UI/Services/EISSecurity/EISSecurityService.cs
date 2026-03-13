using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using Newtonsoft.Json;
using NLog;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services
{
    public class EISSecurityService: IEISSecurityService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Models.Config.EISSecurityModel _eisSecurity;
        private readonly Logger _logger = LogManager.GetLogger("nessLogger");
        private readonly Helper _helper;

        public EISSecurityService(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IOptions<Models.Config.EISSecurityModel> eisSecurityAccessor, Helper helper)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _eisSecurity = eisSecurityAccessor.Value;
            _helper = helper;
        }

        //public EISSecurityService() { }

        public string ToJSON(LogEvent logEvent)
        {
            string result = JsonConvert.SerializeObject(logEvent, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return result;
        }

        public void SetAppInfo(ref LogEvent logEvent)
        {

            // Fill application
            if (logEvent.application == null)
                logEvent.application = new AppData();

            try
            {

                logEvent.application.askId = _eisSecurity.AskId;
                logEvent.application.name = _helper.ApplicationName;

                if (_hostingEnvironment.EnvironmentName.ToStringNullSafe().ToLower().Contains("dev"))
                    logEvent.application.environment = Models.Environment.DEV;
                else if (_hostingEnvironment.EnvironmentName.ToStringNullSafe().ToLower().Contains("stag"))
                    logEvent.application.environment = Models.Environment.STAGE;
                else if (_hostingEnvironment.EnvironmentName.ToStringNullSafe().ToLower().Contains("prod"))
                    logEvent.application.environment = Models.Environment.PROD;

                // Fill device
                if (logEvent.device == null)
                    logEvent.device = new DeviceData();

                logEvent.device.vendor = _eisSecurity.Vendor;
                logEvent.device.product = _eisSecurity.Product;

                logEvent.device.hostname = System.Net.Dns.GetHostEntry(Dns.GetHostName()).HostName;

                IPAddress ip = Helper.GetLocalIpAddress();
                //ip = _httpContextAccessor.HttpContext.Connection.LocalIpAddress;

                if (ip != null)
                {
                    logEvent.device.ip4 = (long)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(ip.GetAddressBytes(), 0));
                }

                logEvent.device.version = "All";

            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Set the SourceHost from source 
        /// </summary>
        /// <param name="logEvent"></param>
        public void SetSourceHost(ref LogEvent logEvent)
        {

            try
            {
                if (logEvent.sourceHost == null)
                    logEvent.sourceHost = new SHostData();

                //string ipString = (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                //   HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();

                string ipString = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToStringNullSafe();

                if (!string.IsNullOrEmpty(ipString))
                {
                    logEvent.sourceHost.ip4 = (long)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(IPAddress.Parse(ipString).GetAddressBytes(), 0));
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Set the SourceUser from source
        /// </summary>
        /// <param name="logEvent"></param>
        public void SetSourceUser(ref LogEvent logEvent)
        {
            try
            {
                if (logEvent.sourceUser == null)
                    logEvent.sourceUser = new SUserData();

                logEvent.sourceUser.uid = _httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]; //HttpContext.Current.User.Identity.Name.Split('\\')[1];
            }
            catch (Exception e)
            {

            }
        }

        public void SetRequestData(ref LogEvent logEvent, int rowCount)
        {
            if (logEvent.request == null)
                logEvent.request = new RequestData();

            logEvent.request.request = _httpContextAccessor.HttpContext.Request.Host.Value; // .GetLeftPart(UriPartial.Path); //HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            logEvent.request.userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToStringNullSafe(); //HttpContext.Current.Request.UserAgent;
            logEvent.request.method = _httpContextAccessor.HttpContext.Request.Method.ToStringNullSafe();//HttpContext.Current.Request.HttpMethod;
            logEvent.request.out_field = rowCount;
        }


        public void LogLogin()
        {
            LogEvent logEvent = new LogEvent();
            SetAppInfo(ref logEvent);
            SetSourceHost(ref logEvent);
            SetSourceUser(ref logEvent);
            logEvent.logClass = LogClass.SECURITY_SUCCESS;
            logEvent.msg = "CreateUserSession:SUCCESS";
            // Add 000 to the end to pass Ness valication
            logEvent.receivedTime = Convert.ToInt64((DateTime.Now - (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds * 1000);

            logEvent.severity = Severity.INFO;
            //logEvent.requestData.in_field = 1;
            string logMsg = ToJSON(logEvent);
            _logger.Info(logMsg);
        }

        public void LogRequestData(string requestWithParameters, int rowCount)
        {
            LogEvent logEvent = new LogEvent();
            SetAppInfo(ref logEvent);
            SetSourceHost(ref logEvent);
            SetSourceUser(ref logEvent);
            SetRequestData(ref logEvent, rowCount);

            logEvent.logClass = LogClass.SECURITY_AUDIT;

            //DataAccess:SUCCESS with parameters
            logEvent.msg = requestWithParameters;
            logEvent.receivedTime = Convert.ToInt64((DateTime.Now - (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds * 1000);
            logEvent.severity = Severity.INFO;

            string logMsg = ToJSON(logEvent);
            _logger.Info(logMsg);
            
        }
        public void LogApplicationError(string error)
        {
            LogEvent logEvent = new LogEvent();
            SetAppInfo(ref logEvent);
            logEvent.logClass = LogClass.SECURITY_AUDIT;
            logEvent.msg = error;
            logEvent.receivedTime = Convert.ToInt64((DateTime.Now - (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds * 1000);
            logEvent.severity = Severity.INFO;

            string logMsg = ToJSON(logEvent);
            _logger.Info(logMsg);
        }

        public void LogSpParams(string sp_name, string gLoginID, string serviceParams, int rowCount)
        {
            StringBuilder s = new StringBuilder();
            s.Append("DataAccess:SUCCESS with parameters SP_Name: " + sp_name + "; ");

            try
            {
                foreach (string param in serviceParams.Split("&"))
                {
                    s.Append(param.Split("=")[0] + ":" + param.Split("=")[1].ToStringNullSafe() + "; ");
                }

                LogRequestData(s.ToString(), rowCount);
            }
            catch (Exception e) { }

        }
        public void LogParams(string paramList, string gLoginID, int rowCount)
        {
            StringBuilder s = new StringBuilder();
            s.Append("DataAccess:SUCCESS with parameters ");

            try
            {
                s.Append(paramList);
                LogRequestData(s.ToString(), rowCount);
            }
            catch (Exception e) { }

        }
    }
}
