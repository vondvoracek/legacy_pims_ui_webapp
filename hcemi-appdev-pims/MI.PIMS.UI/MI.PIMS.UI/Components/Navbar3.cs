using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MI.PIMS.UI.Controllers;
using System.Text.Json;
using MI.PIMS.BO.Dtos;
using Microsoft.AspNetCore.Http;
using MI.PIMS.UI.Common;
using Microsoft.Extensions.DependencyInjection;
using MI.PIMS.BL.Services.Interfaces;

namespace MI.PIMS.UI.Components
{
    [ViewComponent(Name = "Navbar3")]
    public class Navbar3 : ViewComponent
    {
        private static IMenuAccessService _menuAccessService;
        private readonly Helper _helper;

        public Navbar3(IMenuAccessService menuAccessService, Helper helper)
        {
            _menuAccessService = menuAccessService;
            _helper = helper;
        }
   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuAccessDtos = await _menuAccessService.GetMenuAccess(_helper.MS_ID);
            return View(menuAccessDtos);
        }
    }
}
