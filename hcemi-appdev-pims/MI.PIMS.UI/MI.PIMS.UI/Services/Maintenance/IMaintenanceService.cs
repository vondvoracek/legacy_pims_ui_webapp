using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.Maintenance
{
    public interface IMaintenanceService
    {
        /// <summary>
        /// Send email for cache clearance. If No MS_ID being passed, it will considered as MASTER Cache clearance.
        /// </summary>
        /// <param name="ms_id"></param>
        /// <returns></returns>
        Task SendEmail(string ms_id = "");
    }
}
