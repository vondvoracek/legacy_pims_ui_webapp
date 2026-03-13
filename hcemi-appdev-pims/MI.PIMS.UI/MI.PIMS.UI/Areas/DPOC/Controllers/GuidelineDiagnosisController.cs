using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    public class GuidelineDiagnosisController : Controller
    {
        private readonly IDPOCGuidelineDiagnosisCodesService _dPOCGuidelineDiagnosisCodesService;
        private readonly IRefDiagnosesService _refDiagnosesService;
        public GuidelineDiagnosisController(IDPOCGuidelineDiagnosisCodesService dPOCGuidelineDiagnosisCodesService, IRefDiagnosesService refDiagnosesService)
        {
            _dPOCGuidelineDiagnosisCodesService = dPOCGuidelineDiagnosisCodesService;
            _refDiagnosesService = refDiagnosesService;
        }
        public async Task<ActionResult> GetList([DataSourceRequest] DataSourceRequest request, DPOC_Gdln_Param_Dto param)
        {
            IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto> data = null;
            if (string.IsNullOrEmpty(param.p_IQ_GDLN_ID))
                data = await _dPOCGuidelineDiagnosisCodesService.GetByDPOC_ID(param);
            else
                data = await _dPOCGuidelineDiagnosisCodesService.GetByGuideline(param);

            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetCodesByGuideline([DataSourceRequest] DataSourceRequest request, DPOC_Gdln_Param_Dto param)
        {            
            var data = await _dPOCGuidelineDiagnosisCodesService.GetCodesByGuideline(param);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDPOC_DiagnosisList()
        {
            var data = await _refDiagnosesService.GetDPOC_DiagnosisList();
            return Json(data);
        }
    }
}
