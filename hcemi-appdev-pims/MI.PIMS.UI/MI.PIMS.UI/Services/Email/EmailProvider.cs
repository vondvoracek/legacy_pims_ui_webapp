using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Email
{
    public class EmailProvider
    {
        readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Helper _helper;

        public EmailProvider(IEmailService emailService, IHttpContextAccessor httpContextAccessor, Helper helper)
        {
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
        }

        public void SendMail(string mailFrom, string mailTo, string[] body, string subject, bool isHtml, string machineName, string MS_ID = "", string headingStart = "Server Error in 'SCANS-Exchange' Application.")
        {
            try
            {
                string[] body2 = new string[2];

                if (body == null || body.Length == 0)
                {
                    return;
                }

                    SmtpClient smtpClient = new SmtpClient()
                    {
                        EnableSsl = false,
                        Host = _helper.SmtpHost,
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
            catch (Exception e) { }
        }

        public void SendRecipientMail_Smtp(string mailToEmail, string your_Perameter)
        {
            try
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var emailEnvironment = "";
                if (env == "Development")
                {
                    emailEnvironment = "<span style='color:red;'> *** Development Environment ***</span>";
                }
                else if (env == "Stage")
                {
                    emailEnvironment = "<span style='color:red;'> *** Stage Environment ***</span>";
                }
                else
                {
                    emailEnvironment = "";
                }
                SmtpClient smtpClient = new SmtpClient()
                {
                    EnableSsl = false,
                    Host = _helper.SmtpHost,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                };

                var mailFrom = "Policy Integration Management System (PIMS)";
                var subject = "Policy Integration Management System (PIMS): ";
                var emailBody = "<h3>Policy Integration Management System (PIMS) " + emailEnvironment + "</h3> " +
                    "<br/>" +
                    "Dear Policy Integration Management System (PIMS) User," +
                    "<br/><br/>" +
                    "Place email body here...: <a href='" + _helper.AppBaseUrl + _helper.VirtualDirectory + "/CreateCase/Main/Detail?AuthNo=" + your_Perameter + "'>" + your_Perameter + "</a> is ready for review as of " + DateTime.Now.ToString() + "." +
                    "<br/>" +
                    "Please click on the above link to view the case details. " +
                    "<br/><br/>" +
                    "Thank you, " +
                    "<br/><br/>" +
                    "Policy Integration Management System (PIMS) Development Team" +
                    "<br/><br/><br/><br/>" +
                    "This is an automated email. Please do not reply to this email.";

                MailMessage email = new MailMessage()
                {
                    From = new MailAddress(mailFrom + "@uhc.com"),
                    Body = String.Format(emailBody),
                    Subject = subject,
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8
                };

                email.To.Add(mailToEmail);

                smtpClient.Send(email);
                email.Dispose();

            }
            catch (Exception e) { }
        }
 
    }
}
