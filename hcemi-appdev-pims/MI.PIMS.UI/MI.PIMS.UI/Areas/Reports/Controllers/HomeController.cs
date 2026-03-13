using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class HomeController : Controller
    {

        #region Views
        //[AuthorizeRoles(Roles.SuperAdmin, Roles.ReadWrite)]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> MasterExtractExport()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> ExecutiveSummary()
        {
            return await Task.FromResult(View());
        }
        #endregion

        #region Methods

        #endregion

        #region KendoGrid

        #endregion

    }
}

