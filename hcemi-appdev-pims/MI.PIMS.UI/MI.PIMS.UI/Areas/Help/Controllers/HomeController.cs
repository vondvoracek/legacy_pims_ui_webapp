using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MI.PIMS.UI.Areas.Help.Controllers
{
    [Area("Help")]    
    public class HomeController : Controller
    {
        readonly IUserInfoService _userInfoService;
        readonly Helper _helper;
        private readonly IUserAccessHistRepository _userAccessHistRepository;
        public HomeController(IUserInfoService userInfoService
                              ,Helper helper,
                                IUserAccessHistRepository userAccessHistRepository)
        {
            _userInfoService = userInfoService;
            _helper = helper;
            _userAccessHistRepository = userAccessHistRepository;
        }

        //[AuthorizeRoles(Roles.SuperAdmin, Roles.EPALReadOnly, Roles.EPALReadWrite, Roles.ReadOnly, Roles.ReadWrite, Roles.ReadOnly, Roles.ReadWrite)] 
        public async Task<IActionResult> Index()
        {
            UserAccess_Hist_Dto obj = new UserAccess_Hist_Dto();
            {
                obj.MODULE_NAME = "Help";
                obj.USERACTION = "~/Home";
                obj.USERSELECTION = "";
                obj.LST_UPDT_BY = _helper.MS_ID;
            };

            await _userAccessHistRepository.Add(obj);
            return View();
        }
        
        public async Task<IActionResult> Test()
        {
            //throw new Exception();
            return await Task.FromResult(View("Index"));
        }


        public async Task<IActionResult> Support()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> Training()
        {
            return await Task.FromResult(View());
        }
    }
}
