using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.DPOC.Models;
using MI.PIMS.UI.Areas.DPOC.Repositories;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    [Authorize]
    public class HomeController(
        IDPOCService _dpocService,
        Helper _helper,
        IUserAccessHistService _userAccessHistService,
        IPIMSValidValuesService _pimsValidValuesService,
        ILoggerService _logger
        ) : Controller
    {
        #region Views
        //[AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCReadOnly, Roles.DPOCAdmin)]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        public async Task<IActionResult> AddNew()
        {
            List<PageApiInfo> _pageApiInfos = new List<PageApiInfo>();
            _pageApiInfos.AddRange(new List<PageApiInfo>()
                {
                    new PageApiInfo() { Url = Url.Action("EditDetail", "Home", new { Area = "DPOC"  }) , Type = "dpoc-edit-detail-url" },
                    new PageApiInfo() { Url = Url.Action("AddNew", "Home", new { Area = "DPOC"  }), Type = "add-new-url" }
                });

            ViewBag.AddNewUrls = JsonConvert.SerializeObject(_pageApiInfos);

            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        [Route("DPOC/Home/AddDetail/{pims_id}/{p}")]
        public async Task<IActionResult> AddDetail(string pims_id, string p)
        {
            string s = pims_id;
            string[] items = s.Split('-');

            ViewBag.DPOCPageView = DPOCPageView.AddDetail.ToString();
            ViewBag.ShowDuplicateButton = false;
            ViewBag.AddRecord = "Y";
            ViewBag.DuplicateRecord = "N";

            DPOC_Inventories_V_Dto dpocInventoryVDto = new()
            {
                DPOC_BUS_SEG_CD = items[0],
                DPOC_ENTITY_CD = items[1],
                DPOC_PLAN_CD = items[2],
                DPOC_PRODUCT_CD = items[3],
                DPOC_FUND_ARNGMNT_CD = items[4],
                PROC_CD = items[5],
                DRUG_NM = items.Length > 6 ? items[6] : null,
                DPOC_HIERARCHY_KEY = pims_id.Contains(" ") ? pims_id.Substring(0, pims_id.IndexOf(" ")) : pims_id,
                IS_CURRENT = "Y",
                DPOC_PACKAGE = p
            };

            return await Task.FromResult(View(dpocInventoryVDto));
        }
        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        [Route("DPOC/Home/EditDetail/{pims_id}")]
        public async Task<IActionResult> EditDetail(string pims_id)
        {
            if (string.IsNullOrWhiteSpace(pims_id))
            {
                return RedirectToError("Invalid PIMS ID", pims_id);
            }

            var decodedId = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(pims_id));
            var pimsIdArray = decodedId.Split(',');

            if (pimsIdArray.Length < 2)
            {
                return RedirectToError("PIMS ID must contain at least two parts", pims_id);
            }

            var dpocInventoryVDto = await GetDpocInventoryDtoAsync(pimsIdArray, pims_id);

            ViewBag.AddRecord = "N";
            ViewBag.DuplicateRecord = "N";

            if (!string.IsNullOrEmpty(dpocInventoryVDto?.DPOC_HIERARCHY_KEY))
            {
                var additionalInfo = await _dpocService.GetPIMSAdditionalInfoHistory(dpocInventoryVDto.DPOC_HIERARCHY_KEY);
                ViewBag.Notes = GetDPOCInvAdditionalInfoHistory(additionalInfo, "Note");
                ViewBag.AdditionalRequirements = GetDPOCInvAdditionalInfoHistory(additionalInfo, "AdditionalInformation");
            }

            ViewBag.ShowEditButton = UserHasEditRole();

            return View(dpocInventoryVDto);
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        [Route("DPOC/Home/DuplicateRecordDetail/{pims_id}")]
        public async Task<IActionResult> DuplicateRecordDetail(string pims_id)
        {
            if (string.IsNullOrWhiteSpace(pims_id))
            {
                return RedirectToError("Invalid PIMS ID", pims_id);
            }

            var decodedId = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(pims_id));
            var pimsIdArray = decodedId.Split(',');

            if (pimsIdArray.Length < 2)
            {
                return RedirectToError("PIMS ID must contain at least two parts", pims_id);
            }

            ViewBag.DuplicateRecord = "Y";
            ViewBag.ShowDuplicateButton = false;

            var dpocInventoryVDto = await GetDpocInventoryDtoAsync(pimsIdArray, pims_id);

            if (string.IsNullOrEmpty(dpocInventoryVDto?.DPOC_HIERARCHY_KEY))
            {
                return RedirectToAction("RecordNotFound", "ErrorHandler");
            }

            ViewBag.orig_DPOC_HIERARCHY_KEY = dpocInventoryVDto.DPOC_HIERARCHY_KEY;
            ViewBag.orig_DPOC_VER_EFF_DT = dpocInventoryVDto.DPOC_VER_EFF_DT;
            ViewBag.orig_DPOC_BUS_SEG_CD = dpocInventoryVDto.DPOC_BUS_SEG_CD;
            ViewBag.orig_DPOC_ENTITY_CD = dpocInventoryVDto.DPOC_ENTITY_CD;

            var additionalInfo = await _dpocService.GetPIMSAdditionalInfoHistory(dpocInventoryVDto.EPAL_HIERARCHY_KEY);
            ViewBag.Notes = GetDPOCInvAdditionalInfoHistory(additionalInfo, "Note");
            ViewBag.AdditionalRequirements = GetDPOCInvAdditionalInfoHistory(additionalInfo, "AdditionalInformation");

            dpocInventoryVDto = await validateDates(dpocInventoryVDto);

            if (!string.IsNullOrEmpty(dpocInventoryVDto.dateErrorString))
            {
                return RedirectToAction("ViewDetail", "Home", new { pims_id, Area = "DPOC" });
            }

            return View(dpocInventoryVDto);
        }

        [Route("DPOC/Home/ViewDetail/{pims_id}")]
        [AuthorizeRoles(Roles.DPOCAdmin, Roles.DPOCReadWrite, Roles.DPOCReadOnly, Roles.SuperAdmin)]
        public async Task<IActionResult> ViewDetail(string pims_id)
        {
            if (string.IsNullOrWhiteSpace(pims_id))
            {
                return RedirectToError("Missing PIMS ID", pims_id);
            }

            var decodedId = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(pims_id));
            var pimsIdArray = decodedId.Split(',');

            if (pimsIdArray.Length < 2)
            {
                return RedirectToError("PIMS ID must contain at least two parts", pims_id);
            }

            var dpocInventoryVDto = await GetDpocInventoryDtoAsync(pimsIdArray, decodedId);

            if (string.IsNullOrEmpty(dpocInventoryVDto?.DPOC_HIERARCHY_KEY))
            {
                return RedirectToError("Invalid or missing hierarchy key for PIMS ID", pims_id);
            }

            var additionalInfo = await _dpocService.GetPIMSAdditionalInfoHistory(dpocInventoryVDto.EPAL_HIERARCHY_KEY);
            ViewBag.Notes = GetDPOCInvAdditionalInfoHistory(additionalInfo, "Note");
            ViewBag.AdditionalRequirements = GetDPOCInvAdditionalInfoHistory(additionalInfo, "AdditionalInformation");
            ViewBag.DuplicateRecord = "N";

            return View(dpocInventoryVDto);
        }

        private IActionResult RedirectToError(string message, string pims_id)
        {
            return RedirectToAction("RecordNotFound", "ErrorHandler", new
            {
                area = "",
                ErrorViewModel = new ErrorViewModel
                {
                    RequestId = "404",
                    Message = $"{message}: {pims_id}"
                }
            });
        }

        private async Task<DPOC_Inventories_V_Dto> GetDpocInventoryDtoAsync(string[] pimsIdArray, string pims_id)
        {
            if (pimsIdArray.Length == 2)
            {
                return await _dpocService.GetDPOCInventoriesLstUpdtRecordByPIMS_ID(pimsIdArray[0], pimsIdArray[1]);
            }

            var paramDto = new DPOC_Inventories_Param_Dto(pims_id, _helper.MS_ID)
            {
                P_DPOC_PACKAGE = pimsIdArray[2]
            };

            return await _dpocService.GetDPOCInventoriesByPIMS_ID(paramDto);
        }

        private bool UserHasEditRole()
        {
            var userRoles = new[]
            {
                ((int)MI.PIMS.UI.Roles.SuperAdmin).ToString(),
                ((int)MI.PIMS.UI.Roles.DPOCAdmin).ToString(),
                ((int)MI.PIMS.UI.Roles.DPOCReadWrite).ToString()
            };

            return userRoles.Any(role => User.IsInRole(role));
        }

        private static string GetDPOCInvAdditionalInfoHistory(IEnumerable<DPOC_Additional_Req_His_Dto> data, string additionalInfoOrNote)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (var v in data)
            {
                if (additionalInfoOrNote == "Note")
                {
                    if (v != null && v.DPOC_INV_NOTES != null && v.DPOC_INV_NOTES.Trim().Length > 0)
                    {
                        s.Append(v.DPOC_VER_EFF_DT.ToString() + " - " + v.DPOC_INV_NOTES.Trim() + "\r\n\r\n");
                    }
                }
                else
                {
                    if (v != null && v.DPOC_ADDTNL_RQRMNTS != null && v.DPOC_ADDTNL_RQRMNTS.Trim().Length > 0)
                    {
                        s.Append(v.DPOC_VER_EFF_DT.ToString() + " - " + v.DPOC_ADDTNL_RQRMNTS.Trim() + "\r\n\r\n");
                    }
                }
            }
            return s.ToString();
        }
        private async Task<DPOC_Inventories_V_Dto> validateDates(DPOC_Inventories_V_Dto dpoc)
        {
            dpoc.dateErrorString = string.Empty;

            if (dpoc.DPOC_EFF_DT > dpoc.DPOC_EXP_DT)
            {
                dpoc.dateErrorString += "DPOC Exp Date cannot be less than DPOC Eff Date!\n";
            }

            if (dpoc.DPOC_VER_EFF_DT > dpoc.DPOC_VER_EXP_DT)
            {
                dpoc.dateErrorString += "DPOC Version Exp Date cannot be less than DPOC Version Eff Date!\n";
            }

            if (!string.IsNullOrEmpty(dpoc.dateErrorString))
            {
                dpoc.dateErrorString += "\nYou have been redirected to View mode, Please ask Administrator for data correction!";
            }

            dpoc.dateErrorString = dpoc.dateErrorString.Trim();

            return await Task.FromResult(dpoc);
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        public async Task<IActionResult> BulkInsert()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.DPOCReadWrite, Roles.DPOCAdmin)]
        public async Task<IActionResult> BulkUpdate()
        {
            return await Task.FromResult(View());
        }
        public async Task<IActionResult> Load_Guideline_Summary_Detail_AddNew()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_Summary_Detail_AddNew.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_Summary_Detail()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_Summary_Detail.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_DiagnosisCodes_AddNew()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_DiagnosisCodes_AddNew.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_DiagnosisCodes()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_DiagnosisCodes.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_States_AddNew()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_States_AddNew.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_States()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_States.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_POS_AddNew()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_POS_AddNew.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_POS()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_POS.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_Configuration_AddNew()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_Configuration_AddNew.cshtml"));
        }
        public async Task<IActionResult> Load_Guideline_Configuration()
        {
            return await Task.FromResult(PartialView("~/Areas/DPOC/Views/Shared/Guideline/_Guideline_Configuration.cshtml"));

        }
        #endregion

        #region Components
        public async Task<IActionResult> DTQSummaryComponent(string dpoc_hierarchy_key, string dpoc_ver_eff_dt)
        {
            return await Task.FromResult(ViewComponent("DTQSummary", new { dpoc_hierarchy_key, dpoc_ver_eff_dt }));
        }
        public async Task<IActionResult> GuidelineSummaryDetailComponent(string DPOC_HIERARCHY_KEY, string DPOC_VER_EFF_DT, string DPOC_PACKAGE, string DPOC_RELEASE, string IQ_GDLN_ID, string RULE_OUTCOME_OUTPAT, string RULE_OUTCOME_OUTPAT_FCLTY, string RULE_OUTCOME_INPAT, string PageType)
        {
            return await Task.FromResult(ViewComponent("GuidelineSummaryDetail", new DPOC_Inv_Gdln_Rules_V_Key
            {
                DPOC_HIERARCHY_KEY = DPOC_HIERARCHY_KEY,
                DPOC_PACKAGE = DPOC_PACKAGE,
                DPOC_RELEASE = DPOC_RELEASE,
                DPOC_VER_EFF_DT = Convert.ToDateTime(DPOC_VER_EFF_DT),
                IQ_GDLN_ID = IQ_GDLN_ID,
                RULE_OUTCOME_INPAT = RULE_OUTCOME_INPAT,
                RULE_OUTCOME_OUTPAT = RULE_OUTCOME_OUTPAT,
                RULE_OUTCOME_OUTPAT_FCLTY = RULE_OUTCOME_OUTPAT_FCLTY,
                PageType = PageType
            }));
        }
        #endregion

        #region KendoGrid
        public async Task<ActionResult> GetDPOCSearch([DataSourceRequest] DataSourceRequest request, DPOC_Param_Dto param)
        {
            var data = await _dpocService.GetDPOCSearch(param);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDPOCInventoriesHistByPIMS_ID([DataSourceRequest] DataSourceRequest request, DPOC_PIMS_ID_Param_Dto param)
        {
            param.p_DPOC_HIERARCHY_KEY = _helper.DecodeBase64AndUri(param.p_DPOC_HIERARCHY_KEY);
            var data = await _dpocService.GetDPOCInventoriesHistByPIMS_ID(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetDPOCChangeHistoryByPIMSID([DataSourceRequest] DataSourceRequest request, DPOC_PIMS_ID_Param_Dto param)
        {
            IEnumerable<DPOC_ChangeHistory_Dto> data = null;
            if (param.p_DPOC_HIERARCHY_KEY != null && param.p_DPOC_HIERARCHY_KEY != "")
                data = await _dpocService.GetDPOCChangeHistoryByPIMSID(param);
            else
                data = new List<DPOC_ChangeHistory_Dto>();

            return Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<ActionResult> GetDPOCIDExistStatus(string dpoc_hierarchy_key, string dpoc_package)
        {
            var retVal = await _dpocService.GetDPOCIDExistStatus(dpoc_hierarchy_key, dpoc_package);
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DPOC_Ins_Upd_Pkg_Param obj)
        {
            DecodePrefixedFields(obj);

            obj.DPOC_ID = _helper.DecodeBase64AndUri(obj.DPOC_ID);

            obj.P_USER_ID = _helper.MS_ID;

            //obj.P_KL_PLCY_ID = _helper.DecodeBase64AndUri(obj.P_KL_PLCY_ID);
            //obj.P_KL_PLCY_NAME = _helper.DecodeBase64AndUri(obj.P_KL_PLCY_NAME);

            var data = await _dpocService.DPOC_INS_UPD_DRIVER_PRC(obj);

            // Log user activity
            await _userAccessHistService.Add("DPOC", "DPOC/Home/DPOC_INS_UPD_DRIVER_PRC?dpocPageView=" + obj.DPOCPageView, JsonConvert.SerializeObject(obj), 7, _helper.MS_ID);

            return Json(data, new JsonSerializerSettings());
        }
        [HttpGet]
        public async Task<ActionResult> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            var retVal = await _pimsValidValuesService.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
            return Json(retVal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVersion(DPOC_Delete_Pkg_Param deletePkgParam)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                var errorMessage = "Invalid input. Record was not saved due to the following reasons:\n- " +
                string.Join("\n- ", errors);

                return BadRequest(new UpdateDto
                {
                    StatusID = -1,
                    Message = errorMessage,
                    StatusType = RetValStatus.Error.ToString()
                });
            }

            try
            {
                deletePkgParam.P_DPOC_HIERARCHY_KEY = _helper.DecodeBase64AndUri(deletePkgParam.P_DPOC_HIERARCHY_KEY ?? string.Empty);
                deletePkgParam.P_DPOC_PACKAGE = _helper.DecodeBase64AndUri(deletePkgParam.P_DPOC_PACKAGE ?? string.Empty);
                deletePkgParam.P_CHANGE_REQ_ID = _helper.DecodeBase64AndUri(deletePkgParam.P_CHANGE_REQ_ID ?? string.Empty);
                deletePkgParam.P_CHANGE_DESC = _helper.DecodeBase64AndUri(deletePkgParam.P_CHANGE_DESC ?? string.Empty);
                deletePkgParam.P_USER_ID = _helper.MS_ID;
                deletePkgParam.P_RECORD_ENTRY_METHOD = _helper.RecordEntryMethod;

                UpdateDto updateDto = await _dpocService.DPOC_DELETE_DRIVER_PRC(deletePkgParam);

                if (updateDto.StatusID == -1 || updateDto.StatusID == 2)
                {
                    return StatusCode(500, new UpdateDto
                    {
                        StatusID = updateDto.StatusID,
                        Message = updateDto.Message,
                        StatusType = updateDto.StatusType
                    });
                }

                return Ok(new UpdateDto
                {
                    StatusID = updateDto.StatusID,
                    Message = updateDto.Message,
                    StatusType = updateDto.StatusType
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error in SaveDriverVersion: {ex}");

                return StatusCode(500, new UpdateDto
                {
                    StatusID = -1,
                    Message = "An unexpected error occurred while saving the record.",
                    StatusType = RetValStatus.Error.ToString()
                });
            }
        }

        #endregion

        private void setTestValues(DPOC_Inventories_V_Dto dpocInventoryVDto)
        {
            dpocInventoryVDto.DPOC_HIERARCHY_KEY = "EnI-MAHP-COM-ALL-ALL-19328-NA"; // "EnI-MAHP-COM-ALL-ALL-19331-NA"; //"EnI-MAHP-COM-ALL-ALL-28299-NA";
            dpocInventoryVDto.DPOC_BUS_SEG_CD = "EnI";
            dpocInventoryVDto.DPOC_ENTITY_CD = "MAHP";
            dpocInventoryVDto.DPOC_PLAN_CD = "COM";
            dpocInventoryVDto.DPOC_PRODUCT_CD = "ALL";
            dpocInventoryVDto.DPOC_FUND_ARNGMNT_CD = "ALL";
            dpocInventoryVDto.PROC_CD = "19328";// "28299";
            dpocInventoryVDto.DRUG_NM = "NA";
            dpocInventoryVDto.DPOC_PACKAGE = "BREAST_RECONSTRUCTION"; //"Breast Reconstruction"; //"Foot Surgery";
            dpocInventoryVDto.DPOC_VER_EFF_DT = Convert.ToDateTime("06/20/2025 07:58:03 PM"); //1/16/2025 2:00:00 AM
            //dpocInventoryVDto.DPOC_RELEASE = "R79";
            //dpocInventoryVDto.DPOC_VER_NUM = "1";
            dpocInventoryVDto.EPAL_HIERARCHY_KEY = "EnI-MAHP-COM-ALL-ALL-19328-NA";// "EnI-MAHP-COM-ALL-ALL-28299-NA";
            dpocInventoryVDto.IS_CURRENT = "Y";
            dpocInventoryVDto.DPOC_ELIGIBLE_IND = "Yes";
            dpocInventoryVDto.DPOC_INELIGIBLE_RSN = "";
            dpocInventoryVDto.DPOC_IMPLEMENTED_IND = "Yes";
            dpocInventoryVDto.DPOC_UNIMPLEMENTED_RSN = "";
            dpocInventoryVDto.DPOC_EFF_DT = Convert.ToDateTime("06/01/2025");
            dpocInventoryVDto.DPOC_EXP_DT = null;
            ///dpocInventoryVDto.KL_PLCY_NM = "";
        }

        private void DecodePrefixedFields(object obj)
        {
            if (obj == null) return;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                // Only decode string properties that start with "P_"
                if (prop.CanRead && prop.CanWrite &&
                    prop.PropertyType == typeof(string) &&
                    prop.Name.StartsWith("P_"))
                {
                    var value = prop.GetValue(obj) as string;

                    if (!string.IsNullOrEmpty(value))
                    {
                        try
                        {
                            var decodedValue = _helper.DecodeBase64AndUri(value);
                            prop.SetValue(obj, decodedValue);
                        }
                        catch (Exception ex)
                        {
                            // Optional: log or handle decoding errors
                            Console.WriteLine($"Error decoding property {prop.Name}: {ex.Message}");
                        }
                    }
                }
            }
        }

    }
}
