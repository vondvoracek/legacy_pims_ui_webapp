using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MI.PIMS.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ActiveDirectoryController : Controller
    {
        private readonly IActiveDirectoryService _service;
        public ActiveDirectoryController(IActiveDirectoryService service)
        {
            _service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetADUsers([DataSourceRequest] DataSourceRequest request, ActiveDirectoryGetUsersParam param)
        {
            var retVal = await _service.GetActiveDirectoryUser(param.MS_ID, param.Last_Name, param.First_Name);
            return Json(retVal.ToDataSourceResult(request));
        }
    }
}