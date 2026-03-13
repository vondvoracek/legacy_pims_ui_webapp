using Kendo.Mvc.UI;
using MI.PIMS.BL;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    public class KLPoliciesController : Controller
    {
        private readonly IKLPoliciesService _KLPoliciesService;
        public KLPoliciesController(IKLPoliciesService kLPoliciesService)
        {
            _KLPoliciesService = kLPoliciesService;
        }

        public async Task<ActionResult> GetKLPolicies(KL_PoliciesParamDto param)
        {
            var data = await _KLPoliciesService.GetKLPolicies(param);
            return Json(data);
        }
    }
}
