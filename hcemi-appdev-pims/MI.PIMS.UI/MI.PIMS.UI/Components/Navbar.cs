using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MI.PIMS.BL.Services.Interfaces;

namespace MI.PIMS.UI.Components
{
    [ViewComponent(Name = "Navbar")]
    public class Navbar : ViewComponent
    {
        private static IMenuAccessService _menuAccessService;

        public Navbar(IMenuAccessService menuAccessService)
        {
            _menuAccessService = menuAccessService;
        }
   
        public async Task<IViewComponentResult> InvokeAsync(string ms_id)
        {
            var menuAccessDtos = await _menuAccessService.GetMenuAccess(ms_id);
            return View(menuAccessDtos);
        }
    }
}
