using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.Email;
using MI.PIMS.UI.Services.Maintenance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Controllers
{
    [Authorize]
    public class MaintenanceController: Controller
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly Helper _helper;
        private readonly IMaintenanceService _maintenanceService;
        public MaintenanceController(ICacheProvider cacheProvider, ICacheRepository cacheRepository, Helper helper, IMaintenanceService maintenanceService)
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _helper = helper;
            _maintenanceService = maintenanceService;
        }

        public async Task<IActionResult> ClearCache(string code)
        {
            bool isAuthorized = false;
            // Local server
            string clearCacheUser = _helper.ClearCacheUsers;

            if (!string.IsNullOrEmpty(code))
            {
                if (code.ToLower().Equals(_helper.ClearCacheCode.ToLower()))
                {
                    _cacheRepository.FlushAll();
                    ViewBag.Message = "MASTER Cache has been cleared for " + _helper.ApplicationName + " application.";
                    await _maintenanceService.SendEmail();
                    isAuthorized = true;
                }           
            }
            else
            {
                if (!string.IsNullOrEmpty(clearCacheUser)) {
                    clearCacheUser = clearCacheUser.ToLower();

                    string[] users = clearCacheUser.Split(new char[] { ',' });

                    if (users.Length > 0)
                    {
                        if (users.Contains<string>(_helper.MS_ID))
                        {
                            _cacheRepository.RemoveGlobal(_helper.MS_ID);
                            _cacheRepository.Remove("MenuAccessDto");
                            ViewBag.Message = "Cache has been cleared for user " + _helper.MS_ID;
                            await _maintenanceService.SendEmail(_helper.MS_ID);
                            isAuthorized = true;
                        }
                    }
                }
            }

            if (!isAuthorized)
            {
                ViewBag.Authorized = false;
                ViewBag.Message = "You are not authorized to clear cache!";
            }
            else
            {
                ViewBag.Authorized = true;
            }

            return await Task.FromResult(View());
        }

        public async Task<IActionResult> ClearCacheByMSID(string ms_id)
        {
            if (string.IsNullOrEmpty(ms_id))
            {
                ViewBag.Message = "No MS ID passed to reset settings!";
                return View();
            }
            else
            {
                ViewBag.Message = "User settings for MS ID: " + ms_id + " has been reset!";
            }

            _cacheRepository.RemoveGlobal(ms_id);
            _cacheRepository.Remove("MenuAccessDto", ms_id);            
            await _maintenanceService.SendEmail(ms_id);

            return View();
        }
    }
}
