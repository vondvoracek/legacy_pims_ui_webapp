using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.UI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AppRoleController : Controller
    {
        readonly IAppRoleService _service;
        readonly Helper _helper;

        public AppRoleController(IAppRoleService service, Helper helper)
        {
            _service = service;
            _helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAppRole()
        {
            var retVal = await _service.GetAppRole();
            return Json(retVal);
        }
    }
}
