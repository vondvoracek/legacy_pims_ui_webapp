using MI.PIMS.BL.Common;
using MI.PIMS.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Email
{
    public class CloudeEmailService : IEmailService
    {
        readonly Common.Helper _helper;
        private ILoggerService _logger;
        public CloudeEmailService(Common.Helper helper, ILoggerService logger)
        {
            _helper = helper;
            _logger = logger;
        }

        public async Task SendEmailAsync(string ToName, string ToEmailAddress, string Subject, string Message, bool isAdmin = false)
        {
            var mailClient = new CloudMailClient(_helper);
            var res = await mailClient.GetAuthTokenAsync();
            if (res != null && res.IsSuccessStatusCode)
            {
                var resToken = await res.Content.ReadAsStreamAsync();
                var cloudToken = await System.Text.Json.JsonSerializer.DeserializeAsync<CloudToken>(resToken);
                var cloudBody = new CloudMailBody()
                {
                    to = ToEmailAddress.Split(',').ToList<string>(),
                    from = _helper.CloudMailServiceFromEmailSender,
                    subject = Subject + (isAdmin ? " on (" + System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName + ")" : ""),
                    html = Message,
                    context = new context { communicationConfig = "use_email" }
                };
                var sendResponse = await mailClient.SendMailAsyn(cloudToken, cloudBody);
            }
        }

        public async Task SendExceptionEmailAsync(string Message)
        {

            var env = _helper.Environment;
            var emailEnvironment = "";
            if (env == "Development")
            {
                emailEnvironment = "<span style='color:red;'> *** Development Environment ***</span>";
            }
            else if (env == "Stage")
            {
                emailEnvironment = "<span style='color:red;'> *** Stage Environment ***</span>";
            }
            else if (env == "Production")
            {
                emailEnvironment = "<span style='color:red;'> *** Production Environment ***</span>";
            }
            //var mailToName = "Policy Integration Management System (PIMS) Developer";
            var subject = "Policy Integration Management System (PIMS) Error Alert on " + Environment.MachineName;
            var emailBody = "<h3 style='color:red;'>Policy Integration Management System (PIMS) API Error in " + emailEnvironment + "</h3> " +
                            "<br/>" +
                            "Hi Policy Integration Management System (PIMS) Developer," +
                            "<br/><br/>" +
                            Message +
                            "<br/><br/>" +
                            "User: " + _helper.MS_ID +
                            "<br/><br/>" +
                            "Policy Integration Management System (PIMS) Exception Handler" +
                            "<br/><br/><br/><br/>" +
                            "This is an automated email. Please do not reply to this email.";

            var mailClient = new CloudMailClient(_helper);
            var res = await mailClient.GetAuthTokenAsync();
            if (res != null && res.IsSuccessStatusCode)
            {
                var resToken = await res.Content.ReadAsStreamAsync();
                var cloudToken = await System.Text.Json.JsonSerializer.DeserializeAsync<CloudToken>(resToken);
                var cloudBody = new CloudMailBody()
                {
                    to = _helper.ErrorReceivers.Split(',').ToList<string>(),
                    from = _helper.CloudMailServiceFromEmailSender.Replace(Environment.NewLine,""),
                    subject = subject.Replace(Environment.NewLine, ""),
                    html = emailBody,              
                    context = new context { communicationConfig = "use_email" },
                    options = new options()
                };
                _logger.Debug(cloudToken.ToString() + string.Empty + Newtonsoft.Json.JsonConvert.SerializeObject(cloudBody));
                var sendResponse = await mailClient.SendMailAsyn(cloudToken, cloudBody);                
                var resContent = await sendResponse.Content.ReadAsStringAsync();
                _logger.Debug("resContent" + Newtonsoft.Json.JsonConvert.SerializeObject(resContent));
                _logger.Debug("sendResponse" + Newtonsoft.Json.JsonConvert.SerializeObject(sendResponse));
            }
            else
            {                
                _logger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject("Failed response: " + res));
            }
            // + "\n Email Subject: " + subject.Replace(Environment.NewLine, "") + "\n Email Body: " + emailBody)

        }
    }
}
