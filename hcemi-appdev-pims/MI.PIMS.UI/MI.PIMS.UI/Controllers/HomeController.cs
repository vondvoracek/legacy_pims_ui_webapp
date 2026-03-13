using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MI.PIMS.UI.Services.Logging;
using MI.PIMS.UI.Services;
using MI.PIMS.UI.Common;
using MI.PIMS.BO.Dtos;
using Microsoft.AspNetCore.Authorization;
using MI.PIMS.BL.Services.Interfaces;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using MI.PIMS.BL.Common.Telemetry;

namespace MI.PIMS.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly TelemetryLogger<HomeController> _telemetry;
        private readonly IEISSecurityService _eISSecurityService;
        private readonly ILoggerService _logger;
        readonly Helper _helper;

        public HomeController(IEISSecurityService eISSecurityService
                                , ILoggerService logger
                                , Helper helper
                                , TelemetryLogger<HomeController> telemetry)
        {
            _eISSecurityService = eISSecurityService;
            _logger = logger;
            _helper = helper;
            _telemetry = telemetry;            
        }

        //   [AuthorizeRoles(Roles.SuperAdmin, Roles.Initiator, Roles.Network, Roles.SCANegotiator, Roles.ViewOnly)]
        public async Task<IActionResult> Index()
        {
            //_logger.Info("test /Home/Index");

            /*  Set EIS Security login here since HttpContext won't be ready at startup, 
                using this place to log it.
            */
            if (!Startup.IsAppLoaded)
            {
                try
                {
                    _eISSecurityService.LogLogin(); // feature-eissecuritylog 
                    Startup.IsAppLoaded = true;
                }
                catch (Exception ex)
                {
                    _telemetry.TrackException(ex, new LogInfo
                    {
                        Operation = $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}",
                        UserId = _helper.MS_ID,
                        AdditionalInfo = "Invalid Retrieve Data"
                    });

                    _eISSecurityService.LogApplicationError(ex.Message); // feature-eissecuritylog 
                    throw new InvalidOperationException("Invalid Retrieve Data: " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                }
            }

            UserAccess_HistDto obj = new UserAccess_HistDto();
            {
                obj.Module_Name = "Home";
                obj.UserAction = "~/Home";
                obj.UserSelection = "";
                obj.Lst_Updt_By = _helper.MS_ID;
            };

            //await _userInfoRepo.AddUserAccess_Hist(obj);

            // get application version 
            //var appVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            //ViewBag.AppVersion = appVersion;
            //AssemblyInformationalVersionAttribute infoVersion = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();
            //ViewBag.AppVersion = infoVersion.InformationalVersion;

            return await Task.FromResult(View());
        }

        //   [AuthorizeRoles(Roles.SuperAdmin, Roles.Initiator, Roles.Network, Roles.SCANegotiator, Roles.ViewOnly)]
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _telemetry.TrackEvent("User Logout", new LogInfo
            {
                Operation = $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}",
                UserId = _helper?.MS_ID
            });

            return await Task.FromResult(ViewComponent("Logout"));
        }

        //[HttpGet]
        //public async Task<ActionResult> GetUserRoles(string ms_id)
        //{
        //    var roles = await _userInfoRepository.GetRoles(ms_id);
        //    return Json(roles);
        //}
        //[HttpGet]
        //public async Task<ActionResult> GetUserRoleByMarketStateName(string ms_id, string market_statename)
        //{
        //    var role = await _userInfoRepository.GetUserRoleByMarketStateName(ms_id, market_statename);
        //    return Json(role);
        //}

    }
}
