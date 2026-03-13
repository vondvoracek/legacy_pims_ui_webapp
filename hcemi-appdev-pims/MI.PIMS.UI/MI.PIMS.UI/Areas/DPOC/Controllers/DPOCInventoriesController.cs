using MI.PIMS.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Controllers
{
    [Area("DPOC")]
    public class DPOCInventoriesController(IDPOCService _service, IEPAL_Altrnt_Svc_CatService epal_Altrnt_Svc_Cat) : Controller
    {        
        [HttpGet]
        public async Task<ActionResult> GetDPOC_SOS_PROVIDER_TIN_EXCL()
        {
            var data = await _service.GetDPOC_SOS_PROVIDER_TIN_EXCL();//await _service.GetDistinctListByColumn("DPOC_SOS_PROVIDER_TIN_EXCL", "dpoc_inventories_t");
            //data = await _service.GetDistinctListByColumn("DPOC_SOS_PROVIDER_TIN_EXCL", "dpoc_inventories_t");
            var result = data.Select(x => new { Value = x }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAlternateCategory(string p_text, string p_column_name, string p_proc_cds)
        {
            var data = await epal_Altrnt_Svc_Cat.GetAlternateCat(p_text, p_column_name, p_proc_cds);
            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetAlternateSubCategory(string p_text, string p_column_name, string p_proc_cds, string p_epal_altrnt_svc_cat)
        {
            var data = await epal_Altrnt_Svc_Cat.GetAlternateSubCat(p_text, p_column_name, p_proc_cds, p_epal_altrnt_svc_cat);
            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetDPOCRelease()
        {
            var data = await _service.GetDPOCRelease();
            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetDPOCPackage()
        {
            var data = await _service.GetDPOCPackage();
            return Json(data);
        }
    }
}
