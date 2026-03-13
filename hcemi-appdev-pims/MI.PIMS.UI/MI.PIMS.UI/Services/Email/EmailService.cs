using MailKit.Net.Smtp;
using MailKit.Security;
using MI.PIMS.UI.Common;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MI.PIMS.UI.Services.Email
{
    public class EmailService : IEmailService
    {
        readonly Helper _helper;
        public EmailService(Helper helper)
        {
            _helper = helper;
        }

        public async Task SendEmailAsync(string ToName, string ToEmailAddress, string Subject, string Message, bool isAdmin = false)
        {
            var body = new BodyBuilder
            {
                HtmlBody = Message
            };            

            string emailReceivers = ToEmailAddress;            
            foreach(var anEmailReceiver in emailReceivers.Split(','))
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(_helper.SmtpSenderName, _helper.FromEmailAddress));
                mailMessage.To.Add(new MailboxAddress(ToName, anEmailReceiver));
                mailMessage.Subject = Subject + (isAdmin ? " on (" + System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName + ")" : "");
                mailMessage.Body = body.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_helper.SMTPHostMailKit, 587, false).ConfigureAwait(false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.SendAsync(mailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
        }

        public async Task SendExceptionEmailAsync(string Message)
        {
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
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
            var mailToName = "Policy Integration Management System (PIMS) Developer";
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

            string s1 = _helper.ErrorReceivers;
            string[] items1 = s1.Split(',');
            for (int i = 0; i < items1.Length; i++)
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(_helper.SmtpSenderName, _helper.FromEmailAddress));
                mailMessage.To.Add(new MailboxAddress(mailToName, items1[i]));
                mailMessage.Subject = subject;

                var body = new BodyBuilder
                {
                    HtmlBody = emailBody
                };

                mailMessage.Body = body.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_helper.SMTPHostMailKit, 587, false).ConfigureAwait(false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.SendAsync(mailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
        }
    }
}
