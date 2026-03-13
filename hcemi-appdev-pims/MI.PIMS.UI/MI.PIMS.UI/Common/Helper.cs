using Enyim.Caching.Configuration;
using MI.PIMS.BL.Common;
using MI.PIMS.UI.Models;
using MI.PIMS.UI.Models.Config;
using MI.PIMS.UI.Services;
using MI.PIMS.UI.Services.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace MI.PIMS.UI.Common
{
    public class Helper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static IServiceProvider serviceProvider;
        private readonly AppSettings _appSettings;
        private readonly SmtpSettings _smtpSettings;
        private readonly IUrlHelper _urlHelper;
        private readonly AzureMailServiceSettings _azureMailServiceSettings;

        public Helper(IHttpContextAccessor httpContextAccessor,
                        IOptions<AppSettings> appSettings,
                        IOptions<SmtpSettings> smtpSettings,
                        MIUrlHelper mIUrlHelper,
                        IOptions<AzureMailServiceSettings> azureMailServiceSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _smtpSettings = smtpSettings.Value;
            _azureMailServiceSettings = azureMailServiceSettings.Value;
        }

        public static string AppSettings(string getSection)
        {
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, reloadOnChange: true);
            //return builder.Build().GetSection("AppSettings").GetSection(getSection).Value;
            return Startup.StaticConfig.GetSection("AppSettings").GetSection(getSection).Value;
        }

        public static string SmtpSettings(string getSection)
        {
            return Startup.StaticConfig.GetSection("SmtpSettings").GetSection(getSection).Value;
        }

        public static string GetAppInsightCustomEventName(RouteData routeData, string controllerName, string actionName)
        {
            var area = (string)routeData?.Values["area"];
            var aiEventName = "";

            if (area != null && area.Trim() != "")
            {
                aiEventName = $"Server.{area}.{controllerName}.{actionName}Action";
            }
            else
            {
                aiEventName = $"Server.{controllerName}.{actionName}Action";
            }

            return aiEventName;
        }

        public string AzureRedisCacheConnectionString
        {
            get
            {
                return Startup.StaticConfig.GetSection("AppSettings:AzureRedisCacheConnectionString").Value;
                //return Startup.StaticConfig.GetSection("AzureRedisCache").GetSection("ConnectionString").Value;
            }
        }

        public string Environment
        {
            get
            {
                return _appSettings.Environment.ToStringNullSafe();
            }
        }

        public string CacheType
        {
            get
            {
                return _appSettings.CacheType.ToString();
            }
        }

        public string EnvironmentFirstChar
        {
            get
            {
                if (Environment.Length >= 1)
                    return _appSettings.Environment.ToStringNullSafe().Substring(0, 1);
                else
                    return "";
            }
        }

        public string SmtpHost
        {
            get
            {
                return _smtpSettings.SMTPHost;
            }
        }

        public string SMTPHostMailKit
        {
            get
            {
                return _smtpSettings.SMTPHostMailKit;
            }
        }

        public string FromEmailAddress
        {
            get
            {
                return _smtpSettings.FromEmailAddress;
            }
        }

        public int SmtpPort
        {
            get
            {
                return _smtpSettings.SMTPPort;
            }
        }

        public string SmtpSenderName
        {
            get
            {
                return _smtpSettings.SMTPSenderName;
            }
        }

        public string CloudMailServiceAuthTokenUrl
        {
            get
            {
                return _azureMailServiceSettings.AuthTokenUrl;
            }
        }
        public string CloudMailServiceSendMailUrl
        {
            get
            {
                return _azureMailServiceSettings.SendMailUrl;
            }
        }
        public string CloudMailServiceGrantType
        {
            get
            {
                return _azureMailServiceSettings.GrantType;
            }
        }

        public string CloudMailServiceClientId
        {
            get
            {
                return _azureMailServiceSettings.ClientId;
            }
        }
        public string CloudMailServiceClientSecret
        {
            get
            {
                return _azureMailServiceSettings.ClientSecret;
            }
        }
        public string CloudMailServiceFromEmailSender
        {
            get
            {
                return _azureMailServiceSettings.FromEmailSender;
            }
        }


        public string ServiceUrl
        {
            get
            {
                return _appSettings.ServiceUrl;
            }
        }

        public string AppBaseUrl
        {
            get
            {
                return _appSettings.AppBaseUrl;
                //return _config.GetSection("AppSettings:AppBaseUrl").ToString();
            }
        }

        public string VirtualDirectory
        {
            get
            {
                return _appSettings.VirtualDirectory;
            }
        }

        public string AccessGlobalGroup
        {
            get
            {
                //return _appSettings.AccessGlobalGroup;
                return Startup.StaticConfig.GetSection("AppSettings:AccessGlobalGroup").Value;
            }
        }

        //public static string ApplicationName
        //{
        //    get
        //    {
        //        return AppSettings("ApplicationName");
        //    }
        //}

        public string ApplicationName
        {
            get
            {
                return _appSettings.ApplicationName;
            }
        }

        public string ClearCacheUsers
        {
            get
            {
                return _appSettings.ClearCacheUsers;
            }
        }
        public string ClearCacheCode
        {
            get
            {
                return _appSettings.ClearCacheCode;
            }
        }
        /// <summary>
        /// "CacheServers": {
        ///     "Address": [
        ///         "10.201.156.219"
        ///     ],
        ///     "Port": [
        ///         "11214"
        ///     ]
        /// }
        /// </summary>
        public static List<string> CacheServers
        {
            get
            {
                var configServers = Startup.StaticConfig.GetSection("AppSettings:CacheServers:Address").Get<List<string>>();
                return configServers;
            }
        }

        public static int[] CacheServerPorts
        {
            get
            {
                var configServersPorts = Startup.StaticConfig.GetSection("AppSettings:CacheServers:Port").Get<int[]>();
                return configServersPorts;
            }
        }

        public static List<Server> GetCacheServers()
        {
            List<Server> servers = new List<Server>();
            int i = 0;
            foreach (string cacheServer in CacheServers)
            {
                servers.Add(new Server
                {
                    Address = cacheServer,
                    Port = CacheServerPorts[i]
                });
                i++;
            }
            return servers;
        }

        public string ErrorReceivers
        {
            get
            {
                return _appSettings.ErrorReceivers;
            }
        }

        public string RecordEntryMethod
        {
            get
            {
                return _appSettings.RecordEntryMethod;
            }
        }

        public string MS_ID
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "msid");
                if (claims == null)
                {
                    return _httpContextAccessor.HttpContext.User.Identity.Name.Replace("MS\\", "");
                }
                else
                {
                    return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "msid")?.Value;
                }
            }
        }

        public string GetAppVersion
        {
            get
            {
                // get application version 
                try
                {
                    var appVersion = Assembly.GetEntryAssembly()?.GetName().Version;
                    return appVersion.ToString();
                }
                catch (Exception ex)
                {
                    return "Error";
                }
                //AssemblyInformationalVersionAttribute infoVersion = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();
                //ViewBag.AppVersion = infoVersion.InformationalVersion;
            }
        }

        public void SendMail(IWebHostEnvironment env, string mailFrom, string mailTo, string[] body, string subject, bool isHtml, string machineName, string MS_ID = "", string headingStart = "Server Error in 'SCANS-Exchange' Application.")
        {
            try
            {
                string[] body2 = new string[2];

                if (body == null || body.Length == 0)
                {
                    return;
                }

                if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("Development"))
                {
                    SmtpClient smtpClient = new SmtpClient()
                    {
                        EnableSsl = false,
                        Host = SmtpHost,
                        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                    };

                    body2[0] = body[0];

                    if (body.Length == 1)
                    {
                        body2[1] = "";
                    }
                    else
                    {
                        body2[1] = body[1];
                    }

                    MailMessage email = new MailMessage()
                    {
                        From = new MailAddress(mailFrom + "@uhc.com"),
                        Body = String.Format("<h3><p style='color:red; font-size:20px'>" + headingStart + "<p></h3><p>Error: <b>{0}</b><br>{1}</p><p>User: {2}</p>", body2[0], body2[1], MS_ID),
                        Subject = subject + " on " + machineName,
                        IsBodyHtml = isHtml,
                        BodyEncoding = System.Text.Encoding.UTF8
                    };

                    email.To.Add(mailTo);

                    smtpClient.Send(email);
                    email.Dispose();
                }
            }
            catch (Exception e) { }
        }

        public static IPAddress GetLocalIpAddress()
        {
            IPAddress addressTemp = null;
            IPAddress addressEthernet = null;

            try
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters)
                {

                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                    if (uniCast.Count > 0)
                    {

                        foreach (UnicastIPAddressInformation uni in uniCast)
                        {
                            if (uni.Address.AddressFamily != AddressFamily.InterNetwork)
                                continue;

                            if (uni.IsDnsEligible == true)
                            {
                                // If Ethernet 
                                if (addressTemp == null)
                                {
                                    addressTemp = uni.Address;
                                }

                                // Try to get Ethernet IP
                                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                {
                                    addressEthernet = uni.Address;
                                    break;
                                }
                            }

                        }

                    }

                    if (addressEthernet != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return addressEthernet == null ? addressTemp : addressEthernet;
        }

        /// <summary>
        /// Get Class object's properties name and values in query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetObjectValuesToQS<T>(T obj)
        {
            string queryString = string.Empty;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                queryString += property.Name + "=" + property.GetValue(obj) + "&";
            }
            queryString = queryString.TrimEnd('&'); //.Remove(queryString.Length - 1, 1)
            return queryString;
        }

        public static string GetPIMSAdditionalInfoHistory(IEnumerable<BO.Dtos.EPAL_Additional_Info_History_Dto> data, string furtherOrNote)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (var v in data)
            {
                if (furtherOrNote == "Note")
                {
                    if (v != null && v.NOTES != null && v.NOTES.Trim().Length > 0)
                    {
                        s.Append(v.EPAL_VER_EFF_DT.ToString() + " - " + v.NOTES.Trim() + "\r\n\r\n");
                    }
                }
                else
                {
                    if (v != null && v.FURTHER_INST != null && v.FURTHER_INST.Trim().Length > 0)
                    {
                        s.Append(v.EPAL_VER_EFF_DT.ToString() + " - " + v.FURTHER_INST.Trim() + "\r\n\r\n");
                    }
                }
            }
            return s.ToString();
        }

        public string ConvertFromBase64ToString(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public string UnescapeDataString(string value)
        {
            return value == null ? null: Uri.UnescapeDataString(value);
        }

        public string DecodePayload(string encodedPayload)
        {
            if(string.IsNullOrEmpty(encodedPayload)) return null;

            string decodedBase64 = Uri.UnescapeDataString(encodedPayload);
            byte[] data = Convert.FromBase64String(decodedBase64);
            return Encoding.UTF8.GetString(data);
        }

        public T DecodeSanitizedFields<T>(T obj)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string encodedValue = property.GetValue(obj) as string;
                    if (!string.IsNullOrEmpty(encodedValue))
                    {
                        string decodedValue = DecodeBase64AndUri(encodedValue);
                        property.SetValue(obj, decodedValue);
                    }
                }
            }

            return obj;
        }

        public string DecodeBase64AndUri(string encodedValue)
        {
            if (string.IsNullOrEmpty(encodedValue)) return null;

            var base64toString = Encoding.UTF8.GetString(Convert.FromBase64String(encodedValue));
            string decodedUri = Uri.UnescapeDataString(base64toString);
            
            return decodedUri;
        }
    }

    public class ErrorType
    {
        private readonly Helper _helper;
        public ErrorType(Helper helper)
        {
            _helper = helper;
        }
        public string Forbidden
        {
            get
            {
                string url = MyHttpContext.AppBaseUrl + "/ErrorHandler/Forbidden";
                return url;
                //return (string.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/Forbidden";
            }
        }

        public string InternalServerError
        {
            get
            {
                string url = MyHttpContext.AppBaseUrl + "/ErrorHandler/InternalServerError?id=_id_";

                return url;
                //return (string.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/InternalServerError";
            }
        }
        public string PageNotFound
        {
            get
            {
                string url = MyHttpContext.AppBaseUrl + "/ErrorHandler/PageNotFound";
                return url;
                //return (string.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/PageNotFound";
            }
        }
    }

    public class MIUrlHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        //private readonly Helper _helper;
        public MIUrlHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, IConfiguration config) //Helper helper
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            //_helper = helper;
        }

        public string GenerateLink(string action, string controller, object objectValues)
        {
            return _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, action, controller, objectValues, host: new HostString(_config.GetSection("AppSettings:Host").Value));
        }

        public string BaseApiUrls
        {
            get
            {
                List<PageApiInfo> _pageApiInfos = new List<PageApiInfo>();
                _pageApiInfos.AddRange(new List<PageApiInfo>()
                {
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("Index", "Home", new { Area = "" })), Type = "home-url" },
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("Logout", "Home", new { Area = "" })), Type = "log-out-url" }
                });

                return JsonConvert.SerializeObject(_pageApiInfos);
            }
        }

        public string DPOCUrls
        {
            get
            {
                List<PageApiInfo> _pageApiInfos = new List<PageApiInfo>();
                _pageApiInfos.AddRange(new List<PageApiInfo>()
                {
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("ViewDetail", "Home", new { Area = "DPOC", pims_id = "__pims_id__" })), Type = "viewdetail" },
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("EditDetail", "Home", new { Area = "DPOC", pims_id = "__pims_id__" })), Type = "editdetail" },
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("AddDetail", "Home", new { Area = "DPOC", pims_id = "__pims_id__", p = "__p__"})), Type = "adddetail" },
                    new PageApiInfo() { Url = HttpUtility.UrlEncode(GenerateLink("DuplicateRecordDetail", "Home", new { Area = "DPOC", pims_id = "__pims_id__"})), Type = "duplicaterecorddetail" },
                });

                return JsonConvert.SerializeObject(_pageApiInfos);
            }
        }
    }

    public class EncryptionHelper
    {
        public static string EncryptToGuid(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Initialization vector with zeros

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        string base64String = Convert.ToBase64String(encrypted);
                        return new Guid(base64String.Substring(0, 22).PadRight(22, 'A')).ToString();
                    }
                }
            }
        }
    }

    public class DecryptionHelper
    {
        public static string DecryptFromGuid(string guidText, string key)
        {
            string base64String = guidText.Replace("-", "").Replace("A", "=");
            byte[] cipherText = Convert.FromBase64String(base64String);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Initialization vector with zeros

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
