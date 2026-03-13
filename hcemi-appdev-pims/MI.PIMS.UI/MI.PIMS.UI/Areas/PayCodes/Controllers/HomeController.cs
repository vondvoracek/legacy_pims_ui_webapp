using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services.PimsStaticDataRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MI.PIMS.UI.Areas.PayCodes.Controllers
{
    [Area("PayCodes")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPayCodeProceduresService _service;
        private readonly PaycHierarchyCodesXWalkManager _paycHierarchyCodesXWalkManager;
        readonly Helper _helper;
        public HomeController(Helper helper, IPayCodeProceduresService service, PaycHierarchyCodesXWalkManager paycHierarchyCodesXWalkManager)
        {
            _helper = helper;
            _service = service;
            _paycHierarchyCodesXWalkManager = paycHierarchyCodesXWalkManager;
        }
        #region Views
        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeReadOnly, Roles.PayCodeAdmin)]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        [Route("PayCodes/Home/DuplicateRecordDetail/{pims_id}")]
        public async Task<IActionResult> DuplicateRecordDetail(string pims_id)
        {
            string s = pims_id;
            string payc_ver_eff_dt = "";
            string[] items = s.Split(',');

            if (items.Length > 1)
            {
                pims_id = items[0];
                payc_ver_eff_dt = items[1];
            }

            PayCode_Procedures_Param_Dto obj = new PayCode_Procedures_Param_Dto();
            {
                obj.p_PAYC_HIERARCHY_KEY = pims_id;
                obj.p_PAYC_VER_EFF_DT = string.IsNullOrEmpty(payc_ver_eff_dt) ? null : DateTime.Parse(payc_ver_eff_dt.Replace("-", "/"));
                obj.p_MS_ID = _helper.MS_ID;
            }

            PayCode_Procedures_T_Dto data = await _service.GetPayCodeProcedureByPIMS_ID(obj);
            data.IS_EDITABLE = true;

            // Bug reported by Kylie 8/21/2024 MFQ -- Missing ShowHistoricalUpdateButton on Duplicate UI
            ViewBag.ShowHistoricalUpdateButton = false;

            return await Task.FromResult(View(data));
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        [Route("PayCodes/Home/EditDetail/{pims_id}")]
        public async Task<IActionResult> EditDetail(string pims_id)
        {
            string s = pims_id;
            string payc_ver_eff_dt = ""; 
            string[] items = s.Split(',');

            if (items.Length > 1)
            {
                pims_id = items[0];
                payc_ver_eff_dt = items[1];
            }

            PayCode_Procedures_Param_Dto obj = new PayCode_Procedures_Param_Dto();
            {
                obj.p_PAYC_HIERARCHY_KEY = pims_id;
                obj.p_PAYC_VER_EFF_DT = string.IsNullOrEmpty(payc_ver_eff_dt) ? null : DateTime.Parse(payc_ver_eff_dt.Replace("-", "/"));
                obj.p_MS_ID = _helper.MS_ID;
            }


            PayCode_Procedures_T_Dto data = await _service.GetPayCodeProcedureByPIMS_ID(obj);

            var NotesFurtherConsideration = await _service.GetPayCodesNotesFurtherConsiderationByPIMS_ID(pims_id);
            string notes = "";
            string furtherConsideration = "";
            foreach (var item in NotesFurtherConsideration)
            {
                if (item.NOTES_TYPE == "NOTES" && item.NOTES_FURTHER_CONSIDERATIONS != null)
                {
                    notes += item.PAYC_VER_EFF_DT.ToString() + " - " + item.NOTES_FURTHER_CONSIDERATIONS + System.Environment.NewLine;
                }

                if (item.NOTES_TYPE == "FURTHER_CONSIDERATION" && item.NOTES_FURTHER_CONSIDERATIONS != null)
                {
                    furtherConsideration += item.PAYC_VER_EFF_DT.ToString() + " - " + item.NOTES_FURTHER_CONSIDERATIONS + System.Environment.NewLine;
                }
            }
            ViewBag.Notes = notes;
            ViewBag.FurtherConsideration = furtherConsideration;

            // Convert MCR_Routed to list
            if (data != null && data.PAYC_MCR_ROUTED != null)
            {
                ViewBag.MCR_Routed = data.PAYC_MCR_ROUTED.Split(',').ToList().Select(s => new SelectListItem() { Text = s, Value = s }).ToList();
            }
            else
            {
                ViewBag.MCR_Routed = new List<SelectListItem>();
            }

            // DY - 03/04/2024: Only show Historical Update button if user is SuperAdmin, PayCodesAdmin
            if (data.IS_CURRENT == "N" && (User.IsInRole("1") || User.IsInRole("7")))
            {
                ViewBag.ShowHistoricalUpdateButton = true;
                ViewBag.ShowDeleteButton = true;
            }
            else
            {
                ViewBag.ShowHistoricalUpdateButton = false;
                   ViewBag.ShowDeleteButton = false;
            }

            // DY - 03/04/2024: ability to delete current records if user is Admin, PayCodesAdmin
            if (User.IsInRole("1") || User.IsInRole("7"))
            {
                ViewBag.ShowDeleteButton = true;
            }
            else
            {
                ViewBag.ShowDeleteButton = false;
            }

            data.IS_EDITABLE = true;
            return await Task.FromResult(View(data));
        }


        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin, Roles.PayCodeReadOnly)]
        [Route("PayCodes/Home/ViewDetail/{pims_id}")]
        public async Task<IActionResult> ViewDetail(string pims_id)
        {
            string s = pims_id;
            string payc_ver_eff_dt = ""; 
            string[] items = s.Split(',');

            if (items.Length > 1) 
            {
                pims_id = items[0];
                payc_ver_eff_dt = items[1];
            }
            
            PayCode_Procedures_Param_Dto obj = new PayCode_Procedures_Param_Dto();
            {
                obj.p_PAYC_HIERARCHY_KEY = pims_id;
                obj.p_PAYC_VER_EFF_DT = string.IsNullOrEmpty(payc_ver_eff_dt) ? null : DateTime.Parse(payc_ver_eff_dt.Replace("-", "/"));
                obj.p_MS_ID = _helper.MS_ID;
            }
           
            PayCode_Procedures_T_Dto data = await _service.GetPayCodeProcedureByPIMS_ID(obj);

            if(data == null)
            {
                return RedirectToAction("RecordNotFound", "ErrorHandler", new { Area = "", ErrorViewModel = new ErrorViewModel() { RequestId = "404", Message = $"PIMS ID:{pims_id}" } });
            }

            var NotesFurtherConsideration = await _service.GetPayCodesNotesFurtherConsiderationByPIMS_ID(pims_id);
            string notes = "";
            string furtherConsideration = "";
            foreach (var item in NotesFurtherConsideration)
            {
                if (item.NOTES_TYPE == "NOTES" && item.NOTES_FURTHER_CONSIDERATIONS != null)
                {
                    notes += item.PAYC_VER_EFF_DT.ToString() + " - " + item.NOTES_FURTHER_CONSIDERATIONS + System.Environment.NewLine;
                }

                if (item.NOTES_TYPE == "FURTHER_CONSIDERATION" && item.NOTES_FURTHER_CONSIDERATIONS != null)
                {
                    furtherConsideration += item.PAYC_VER_EFF_DT.ToString() + " - " + item.NOTES_FURTHER_CONSIDERATIONS + System.Environment.NewLine;
                }
            }
            ViewBag.Notes = notes;
            ViewBag.FurtherConsideration = furtherConsideration;

            // Convert MCR_Routed to list
            if (data != null && data.PAYC_MCR_ROUTED != null)
            {
                ViewBag.MCR_Routed = data.PAYC_MCR_ROUTED.Split(',').ToList().Select(s => new SelectListItem() { Text = s, Value = s }).ToList();
            }
            else
            {
                ViewBag.MCR_Routed = new List<SelectListItem>();
            }

            // DY - 03/04/2024: Only show Historical Update button if user is SuperAdmin, PayCodesAdmin
            if (data.IS_CURRENT == "N" && (User.IsInRole("1") || User.IsInRole("7")))
            {
                ViewBag.ShowHistoricalUpdateButton = true;
            }
            else
            {
                ViewBag.ShowHistoricalUpdateButton = false;
            }

            // DY - 03/04/2024: ability to delete current records if user is Admin, PayCodesAdmin
            if (User.IsInRole("1") || User.IsInRole("7"))
            {
                ViewBag.ShowDeleteButton = true;
            }
            else
            {
                ViewBag.ShowDeleteButton = false;
            }

            data.IS_EDITABLE = false;
            return await Task.FromResult(View(data));
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        [Route("PayCodes/Home/AddDetail/{pims_id}")]
        public async Task<IActionResult> AddDetail(string pims_id)
        {
            string[] items = pims_id.Split('-');
            PayCode_Procedures_T_Dto data = new PayCode_Procedures_T_Dto();
            {
                data.PAYC_BUS_SEG_CD = items[0];
                data.PAYC_ENTITY_CD = items[1];
                data.PAYC_PLAN_CD = items[2];
                data.PAYC_PRODUCT_CD = items[3];
                data.PAYC_FUND_ARNGMNT_CD = items[4];
                data.PAYC_PROC_CD = items[5];
                data.PAYC_HIERARCHY_KEY = pims_id;
                data.PAYC_STATUS = "ACTIVE";
                data.PAGE_TYPE = PayCodePageView.AddDetail.ToString();
            };
            data.IS_EDITABLE = true;
            ViewBag.ShowHistoricalUpdateButton = false;
            ViewBag.ShowDeleteButton = false;
            return await Task.FromResult(View(data));
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        public async Task<IActionResult> AddNew()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        public async Task<IActionResult> BulkInsert()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.PayCodeReadWrite, Roles.PayCodeAdmin)]
        public async Task<IActionResult> BulkUpdate()
        {
            return await Task.FromResult(View());
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<ActionResult> GetPayCodeSearchFilters(string EPAL_BUS_SEG_CD, string COLUMN_NAME, string EPAL_ENTITY_CD)
        {
            var retVal = await _service.GetPayCodeSearchFilters(new EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto(EPAL_BUS_SEG_CD, COLUMN_NAME, EPAL_ENTITY_CD));
            //var retVal = await _paycHierarchyCodesXWalkManager.GetPayCodeHierarchyCodesXwalk(EPAL_BUS_SEG_CD, COLUMN_NAME, EPAL_ENTITY_CD);
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            obj.LST_UPDT_BY = _helper.MS_ID;
            obj.FURTHER_INST = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.FURTHER_INST.ToStringNullSafe()).ToStringOrNull());
            obj.PAYC_NOTES = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.PAYC_NOTES.ToStringNullSafe()).ToStringOrNull());
            obj.CHANGE_REQ_ID = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.CHANGE_REQ_ID.ToStringNullSafe()).ToStringOrNull());
            obj.CHANGE_DESC = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.CHANGE_DESC.ToStringNullSafe()).ToStringOrNull());
            obj.PAYC_COMMENTS = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.PAYC_COMMENTS.ToStringNullSafe()).ToStringOrNull());

            var data = await _service.UpdatePayCodeProcedure(obj);
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HistoricalUpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            obj.LST_UPDT_BY = _helper.MS_ID;
            obj.FURTHER_INST = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.FURTHER_INST.ToStringNullSafe()).ToStringOrNull());
            obj.PAYC_NOTES = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.PAYC_NOTES.ToStringNullSafe()).ToStringOrNull());
            obj.CHANGE_REQ_ID = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.CHANGE_REQ_ID.ToStringNullSafe()).ToStringOrNull());
            obj.CHANGE_DESC = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.CHANGE_DESC.ToStringNullSafe()).ToStringOrNull());
            obj.PAYC_COMMENTS = _helper.UnescapeDataString(_helper.ConvertFromBase64ToString(obj.PAYC_COMMENTS.ToStringNullSafe()).ToStringOrNull());

            var data = await _service.PAYC_HISTORIC_INS_UPD_DRIVER(obj);
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            obj.LST_UPDT_BY = _helper.MS_ID;
            var data = await _service.PAYC_DELETE_DRIVER_PRC(obj);
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPayCodeHierarchyCodesXwalk(string PAYC_BUS_SEG_CD, string PAYC_ENTITY_CD, string PAYC_PLAN_CD, string PAYC_PRODUCT_CD, string COLUMN_NAME)
        {
            var retVal = await _service.GetAllPayCodeHierarchyCodesXwalk2();
            JsonResult toReturn = null;

            List<PayCode_PIMSHierarchyCode_V_Xwalk_Dto> result = retVal.ToList();

            var toDropdown = result.Where(o => o.PAYC_HIERARCHY_STS == "Active"
                    && (string.IsNullOrEmpty(PAYC_BUS_SEG_CD) || o.PAYC_BUS_SEG_CD == PAYC_BUS_SEG_CD)
                    && (string.IsNullOrEmpty(PAYC_ENTITY_CD) || o.PAYC_ENTITY_CD == PAYC_ENTITY_CD)
                    && (string.IsNullOrEmpty(PAYC_PLAN_CD) || o.PAYC_PLAN_CD == PAYC_PLAN_CD)
                    && (string.IsNullOrEmpty(PAYC_PRODUCT_CD) || o.PAYC_PRODUCT_CD == PAYC_PRODUCT_CD)                    
                    );

            if (COLUMN_NAME == "BUS_SEG_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { VV_CD = o.PAYC_BUS_SEG_CD }).Distinct().OrderBy(o => o.VV_CD));
            }
            else if (COLUMN_NAME == "PAYC_ENTITY_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.PAYC_ENTITY_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }
            else if (COLUMN_NAME == "PAYC_PLAN_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.PAYC_PLAN_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }
            else if (COLUMN_NAME == "PAYC_PRODUCT_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.PAYC_PRODUCT_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }           

            return toReturn;
        }
        [HttpGet]
        public async Task<ActionResult> CheckValidNewPIMSID(string pims_id)
        {
            PayCode_Procedures_Param_Dto obj = new PayCode_Procedures_Param_Dto();
            obj.p_PAYC_HIERARCHY_KEY = pims_id;
            obj.p_MS_ID = _helper.MS_ID;
            PayCode_Procedures_T_Dto data = await _service.GetPayCodeProcedureByPIMS_ID(obj);
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region KendoGrid
        public async Task<ActionResult> GetPayCodeProceduresSearch([DataSourceRequest] DataSourceRequest request, PayCode_Procedures_Param_Dto param)
        {
            var data = await _service.GetPayCodeProceduresSearch(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<JsonResult> GetPayCodeEPALSummary([DataSourceRequest] DataSourceRequest request, PayCode_Procedures_Param_Dto param)
        { 
            if (string.IsNullOrEmpty(param.p_PAYC_HIERARCHY_KEY) || param ==null)
            {
                return Json("");
            }
            param.p_MS_ID = _helper.MS_ID;
            var data = await _service.GetPayCodeEPALSummary(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<JsonResult> GetPayCodeHistorical([DataSourceRequest] DataSourceRequest request, PayCode_Procedures_Param_Dto param)
        {
            if (string.IsNullOrEmpty(param.p_PAYC_HIERARCHY_KEY) || param == null)
            {
                return Json("");
            }
            param.p_MS_ID = _helper.MS_ID;
            var data = await _service.GetPayCodeHistorical(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<JsonResult> GetPayCodeChangeHistory([DataSourceRequest] DataSourceRequest request, PayCode_Procedures_Param_Dto param)
        {
            if (string.IsNullOrEmpty(param.p_PAYC_HIERARCHY_KEY) || param == null)
            {
                return Json("");
            }
            param.p_MS_ID = _helper.MS_ID;
            var data = await _service.GetPayCodeChangeHistory(param);
            return Json(data.ToDataSourceResult(request));
        }
        #endregion        

    }
}
