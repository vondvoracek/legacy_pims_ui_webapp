using MI.PIMS.BL.Repositories;
using MI.PIMS.UI.Areas.DPOC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    [Authorize]
    public class DPOCHierarchyCodesController : Controller
    {
        private readonly DPOCHierarchyCodesXWalkUIRepository _repository;

        public DPOCHierarchyCodesController(DPOCHierarchyCodesXWalkUIRepository dPOCHierarchyCodesXWalkUIRepository)
        {
            _repository = dPOCHierarchyCodesXWalkUIRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetBusSegCd()
        {
            var retVal = await _repository.GetBusSegCd();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetEntityCd(string epal_bus_seg_cd)
        {
            var retVal = await _repository.GetEntityCd(epal_bus_seg_cd);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductCd(string epal_bus_seg_cd, string epal_entity_cd, string epal_fund_arngmnt_cd)
        {
            var retVal = await _repository.GetProductCd(epal_bus_seg_cd, epal_entity_cd, epal_fund_arngmnt_cd);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetFundingArrangement(string epal_bus_seg_cd, string epal_entity_cd, string epal_product_cd)
        {
            var retVal = await _repository.GetFundingArrangement(epal_bus_seg_cd, epal_entity_cd, epal_product_cd);
            return Json(retVal);
        }
    }
}
