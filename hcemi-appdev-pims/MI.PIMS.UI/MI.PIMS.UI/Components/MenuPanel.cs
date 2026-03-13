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
    [ViewComponent(Name = "MenuPanel")]
    public class MenuPanel : ViewComponent
    {
        private static IMenuAccessService _menuAccessService;
        private readonly ICacheProvider _cacheProvider;
        private readonly Helper _helper;

        public MenuPanel(IMenuAccessService menuAccessService, ICacheProvider cacheProvider, Helper helper)
        {
            _menuAccessService = menuAccessService;
            _cacheProvider = cacheProvider;
            _helper = helper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<MenuAccessDto> menuAccessDtos = null;            
#if DEBUG
            //When debugging, always get it from db, so if any changes made would be incorporated quickly without clearing cache            
#else
            menuAccessDtos = _cacheProvider.Get<IEnumerable<MenuAccessDto>>("MenuAccessDto");
#endif 

            if (menuAccessDtos == null)
            {
                menuAccessDtos = await _menuAccessService.GetMenuAccess(_helper.MS_ID);
            }

            return View(menuAccessDtos);
        }

    }
}
