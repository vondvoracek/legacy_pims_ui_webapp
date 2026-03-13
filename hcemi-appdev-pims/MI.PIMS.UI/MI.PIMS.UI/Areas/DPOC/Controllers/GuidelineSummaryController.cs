using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    public class GuidelineSummaryController : Controller
    {
        private readonly IDPOCGuidelineRulesService _dPOCGuidelineRulesService;
        private readonly IPIMSValidValuesService _pimsValidValuesService;
        private readonly IDPOCGuidelineDTQsService _dPOCGuidelineDTQsService;
        private readonly IDPOCGuidelineStatesService _dpocGuidelineStatesService;

        public GuidelineSummaryController(Helper helper, 
            IDPOCGuidelineRulesService dPOCGuidelineRulesService,
            IPIMSValidValuesService pimsValidValuesService,
            IDPOCGuidelineDTQsService dPOCGuidelineDTQsService,
            IDPOCGuidelineStatesService dpocGuidelineStatesService)
        {
            _dPOCGuidelineRulesService = dPOCGuidelineRulesService;
            _pimsValidValuesService = pimsValidValuesService;
            _dPOCGuidelineDTQsService = dPOCGuidelineDTQsService;
            _dpocGuidelineStatesService = dpocGuidelineStatesService;            
        }

        public async Task<ActionResult> GetPendingByPIMSID([DataSourceRequest] DataSourceRequest request, DPOC_Inv_Gdln_Rules_Param_Dto param)
        {
            var data = await _dPOCGuidelineRulesService.GetPendingByPIMSID(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetActiveRetiredByPIMSID([DataSourceRequest] DataSourceRequest request, DPOC_Inv_Gdln_Rules_Param_Dto param)
        {
            var data = await _dPOCGuidelineRulesService.GetActiveRetiredByPIMSID(param);            
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetActiveRetiredByPIMSIDSummary([DataSourceRequest] DataSourceRequest request, DPOC_Inv_Gdln_Rules_Param_Dto param)
        {
            var data = await _dPOCGuidelineRulesService.GetActiveRetiredByPIMSIDSummary(param);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetConfigurations([DataSourceRequest] DataSourceRequest request, DPOC_Gdln_Param_Dto param)
        {
            var data = await _dPOCGuidelineDTQsService.GetConfigurations(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetGSStatesByDPOC([DataSourceRequest] DataSourceRequest request, DPOC_Gdln_Param_Dto param)
        {
            var data = await _dpocGuidelineStatesService.Get(param);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetByGuidelineConfigID([DataSourceRequest] DataSourceRequest request, DPOC_Gdln_Param_Dto param)
        {
            var data = await _dpocGuidelineStatesService.Get(param);
            return Json(data.ToDataSourceResult(request));
        }

        #region "Methods"
        [HttpGet]
        public async Task<ActionResult> GetDPOCGuidelineRules(string P_COLUMN_NAME)
        {
            DPOC_Inventories_Param_Dto obj = new DPOC_Inventories_Param_Dto();
            {
                obj.P_COLUMN_NAME = P_COLUMN_NAME;
            };
            var retVal = await _dPOCGuidelineRulesService.GetDPOCGuidelineRules(obj);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetInPatientTypeRules()
        {
            var retVal = await _pimsValidValuesService.GetInPatientTypeRules();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetOutPatientTypeRules()
        {
            var retVal = await _pimsValidValuesService.GetOutPatientTypeRules();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetOutPatientFacTypeRules()
        {
            var retVal = await _pimsValidValuesService.GetOutPatientFacTypeRules();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetRuleTypeReason()
        {
            var retVal = await _pimsValidValuesService.GetRuleTypeReason();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetDTQ_TYPE()
        {
            var retVal = await _pimsValidValuesService.GetDTQ_TYPE();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetDTQ_RSN()
        {
            var retVal = await _pimsValidValuesService.GetDTQ_RSN();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<IActionResult> GetPOS_APPL()
        {
            var retVal = await _pimsValidValuesService.GetPLC_OF_SVC_CD();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<IActionResult> GetPOS_INCL_EXCL_CD()
        {
            var retVal = await _pimsValidValuesService.GetPOS_INCL_EXCL_CD();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<IActionResult> GetInvPlcyLkp(DPOC_INV_PLCY_LKP_Param dPOC_INV_PLCY_LKP_Param)
        {
            var retVal = await _dPOCGuidelineRulesService.GetInvPlcyLkp(dPOC_INV_PLCY_LKP_Param);
            return Json(retVal);
        }

        #region "GuideLineID"
        [AllowAnonymous]
        public async Task<IActionResult> GetGuideLineIds([DataSourceRequest] DataSourceRequest request)
        {
            var retVal = await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, "iq_gdln_id");
            return Json(retVal.ToDataSourceResult(request));
        }
        public async Task<IActionResult> GetGetGuideLineIds_ValueMapper(string[] values)
        {
            var allItems = await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, "iq_gdln_id");

            var matchedItems = allItems
                .Where(item => values.Contains(item.IQ_GDLN_ID))
                .Select(item => new { IQ_GDLN_ID = item.IQ_GDLN_ID })
                .ToList();

            return Json(matchedItems);

            //var indices = new List<string>();

            //if (values != null && values.Any())
            //{
            //    var index = 0;

            //    foreach (var gdln in await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, "iq_gdln_id"))
            //    {
            //        if (values.Contains(gdln.IQ_GDLN_ID))
            //        {
            //            indices.Add(gdln.IQ_GDLN_ID);
            //        }

            //        index += 1;
            //    }
            //}

            //return Json(indices);
        }
        [HttpGet]
        public async Task<IActionResult> GetGuideLineIds(string text)
        {
            var data = await _dPOCGuidelineRulesService.GetGuideLineIds(text);
            return Json(data);
        }
        #endregion

        #region "GuideLineVersions"
        public async Task<IActionResult> GetGuideLineVersions([DataSourceRequest] DataSourceRequest request)
        {
            /*if (p_text == null)
            {
                return Json("");
            }*/
            var retVal = await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, "iq_gdln_version");
            return Json(retVal.ToDataSourceResult(request));
        }
        public async Task<IActionResult> GetGuideLineVersions_ValueMapper(string[] values)
        {
            var indices = new List<string>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var gdln in await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, "iq_gdln_version"))
                {
                    if (values.Contains(gdln.IQ_GDLN_VERSION))
                    {
                        indices.Add(gdln.IQ_GDLN_VERSION);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetGuideLineRulesList(string p_column_name)
        {
            var retVal = await _dPOCGuidelineRulesService.GetGuideLineRulesList(string.Empty, p_column_name);
            return Json(retVal);
        }
        #endregion

        #region "Components"
        public async Task<IActionResult> GuidelineConfigurationHover(DPOC_Gdln_Param_Dto param)
        {
            return await Task.FromResult(ViewComponent("_Guideline_Configuration_Hover", new {
                param.p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT = (param.p_DPOC_VER_EFF_DT == null ? Convert.ToDateTime("1/1/1900") : param.p_DPOC_VER_EFF_DT),
                param.p_DPOC_PACKAGE,
                param.p_DPOC_RELEASE
            }));
        }
        public async Task<IActionResult> GuidelineDiagnosisHover(DPOC_Gdln_Param_Dto param)
        {
            return await Task.FromResult(ViewComponent("_Guideline_DiagnosisCodes_Hover", new
            {
                param.p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT = (param.p_DPOC_VER_EFF_DT == null ? Convert.ToDateTime("1/1/1900") : param.p_DPOC_VER_EFF_DT),
                param.p_DPOC_PACKAGE,
                param.p_DPOC_RELEASE
            })); 
        }
        public async Task<IActionResult> GuidelineStatesHover(DPOC_Gdln_Param_Dto param)
        {
            return await Task.FromResult(ViewComponent("_Guideline_States_Hover", new
            {
                param.p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT = (param.p_DPOC_VER_EFF_DT == null ? Convert.ToDateTime("1/1/1900") : param.p_DPOC_VER_EFF_DT),
                param.p_DPOC_PACKAGE,
                param.p_DPOC_RELEASE
            }));
        }
        #endregion
    }
}
