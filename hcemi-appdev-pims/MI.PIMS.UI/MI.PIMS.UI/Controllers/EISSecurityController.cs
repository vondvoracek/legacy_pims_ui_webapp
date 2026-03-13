using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MI.PIMS.UI.Controllers
{
    public class EISSecurityController : Controller
    {
        private readonly IEISSecurityService _eISSecurityService;
        private readonly Helper _helper;
        public EISSecurityController(IEISSecurityService eISSecurityService, Helper helper)
        {
            _eISSecurityService = eISSecurityService;
            _helper = helper;
        }

        [HttpGet]
        public async Task<JsonResult> LogRequestData(string requestWithParameters, string rowCount)
        {
            try
            {
                await  Task.Run(() => _eISSecurityService.LogRequestData(requestWithParameters, Convert.ToInt32(rowCount)));
                return Json(new { success = true, message = "EIS Security logged!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "EIS Security logged issue: " + ex.Message });
            }
        }
    }
}