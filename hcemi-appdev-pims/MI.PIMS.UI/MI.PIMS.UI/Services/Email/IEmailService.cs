using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string ToName, string ToEmailAddress, string Subject, string Message, bool isAdmin = false);
        Task SendExceptionEmailAsync(string Message);
        
    }
}
