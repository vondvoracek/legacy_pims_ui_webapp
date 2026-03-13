using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Maintenance
{
    public class MaintenanceService: IMaintenanceService
    {
        private readonly IEmailService _emailService;
        private readonly Helper _helper;
        public MaintenanceService(IEmailService emailService, Helper helper)
        {
            _emailService = emailService;
            _helper = helper;
        }

        public async Task SendEmail(string ms_id)
        {
            var env = _helper.Environment;
            var emailEnvironment = "<span style='color:red;'> *** " + env + " Environment ***</span>";                        

            var emailSubject = "ClearCache";

            string emailBody = string.Empty;
            if(string.IsNullOrEmpty(ms_id))
                emailBody  = "<h3>Policy Integration Management System (PIMS) " + emailEnvironment + "</h3> <br/>" + "MASTER Cache has been cleared in <b>" + _helper.ApplicationName + "<b/>";
            else
                emailBody = "<h3>Policy Integration Management System (PIMS) " + emailEnvironment + "</h3> <br/>" + "Cache has been cleared for MS ID: <b>'" + ms_id + "'</b> in " + _helper.ApplicationName;

            await _emailService.SendEmailAsync("Policy Integration Management System (PIMS) user", _helper.ErrorReceivers, emailSubject, emailBody, true);
        }
    }
}
