using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Controllers
{
    public class UserAccessHistController : Controller
    {
        private readonly IUserAccessHistService _userAccessHistService;
        public UserAccessHistController(IUserAccessHistService userAccessHistService)
        {
            _userAccessHistService = userAccessHistService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserAccess_Hist_Dto userAccess_Hist_Dto)
        {
            userAccess_Hist_Dto.LST_UPDT_DT = DateTime.Now;
            await _userAccessHistService.Add(userAccess_Hist_Dto);
            return Json(new { update = 1 });
        }
    }
}
