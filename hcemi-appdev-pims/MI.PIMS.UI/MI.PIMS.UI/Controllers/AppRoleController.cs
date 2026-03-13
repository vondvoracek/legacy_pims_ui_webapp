using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MI.PIMS.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MI.PIMS.UI.Controllers
{
    public class AppRoleController : Controller
    {
        readonly IAppRoleService _service;

        public AppRoleController(IAppRoleService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
        //    var data = await _repo.GetAppRole();
        //    return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        //}
    }
}