using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.UI.Areas.Admin.Models;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class ManageGuidelinesController : Controller
    {
        private readonly IManageGuidelinesService _service;
        private readonly IManualGuidelineService _addSevice;
        private readonly Helper _helper;
        public ManageGuidelinesController(IManageGuidelinesService service, IManualGuidelineService addSevice, Helper helper)
        {
            _service = service;
            _addSevice = addSevice;
            _helper = helper;
        }

        [AuthorizeRoles(Roles.SuperAdmin,
                        Roles.DPOCAdmin,
                        Roles.EPALAdmin,
                        Roles.PayCodeAdmin,
                        Roles.CMPAdmin
                        )]
        public async Task<IActionResult> Index()
        {
            List<PageApiInfo> _pageApiInfos = new List<PageApiInfo>();
            _pageApiInfos.AddRange(new List<PageApiInfo>()
                {
                    new PageApiInfo() { Url = Url.Action("AddGuidelines", "ManageGuidelines", new { Area = "Admin"  }) , Type = "admin-add-guidelines-url" },
                });

            ViewBag.ManageGuidelinesUrls = JsonConvert.SerializeObject(_pageApiInfos);

            return await Task.FromResult(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetGuidelinesByParams([DataSourceRequest] DataSourceRequest request,
                    ManageGuidelineSearchParam manageGuidelineSearchParam)
        {
            var data = await _service.GetGuidelinesByParams(
                manageGuidelineSearchParam.proc_cd, 
                manageGuidelineSearchParam.iq_reference, 
                manageGuidelineSearchParam.iq_gdln_id, 
                manageGuidelineSearchParam.iq_gdln_version
            );

            // Apply Kendo Grid operations (paging, sorting, filtering)
            return Json(data.ToDataSourceResult(request));
        }


        [HttpGet]
        public async Task<IActionResult> GetProcedureCodes(string searchTerm)
        {
            var codes = await _service.GetActiveProcedureCodes(searchTerm);
            return Json(codes);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGuidelines([FromBody] AddGuidelineRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.iq_gdln_id) ||
                string.IsNullOrWhiteSpace(request.iq_gdln_proc_cd) ||
                string.IsNullOrWhiteSpace(request.iq_version) ||
                string.IsNullOrWhiteSpace(request.iq_reference))
            {
                return BadRequest("All fields are required.");
            }

            var (insertedRecords, invalidProcCodes, duplicateProcCodes) = await _addSevice.AddGuidelinesAsync(
                _helper.DecodeBase64AndUri(request.iq_gdln_id),
                _helper.DecodeBase64AndUri(request.iq_gdln_proc_cd),
                _helper.DecodeBase64AndUri(request.iq_version),
                _helper.DecodeBase64AndUri(request.iq_reference)
            );

            return Ok(new
            {
                message = "Guidelines processed.",
                insertedRecords = insertedRecords,
                invalidProcCodes = invalidProcCodes,
                duplicateProcCodes = duplicateProcCodes
            });
        }
    }
}
