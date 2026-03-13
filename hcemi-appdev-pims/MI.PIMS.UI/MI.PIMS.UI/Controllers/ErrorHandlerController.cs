using Kendo.Mvc.UI;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MI.PIMS.UI.Controllers
{
    [AllowAnonymous]
    public class ErrorHandlerController : Controller
    {
        readonly Helper _helper;
        readonly ICacheRepository _cacheRepository;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ErrorHandlerController(Helper helper, ICacheRepository cacheRepository)
        {
            _helper = helper;
            _cacheRepository = cacheRepository;            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult Index(int id, string refNo = "")
        {
            switch (id)
            {
                case 401:
                case 403:
                    return Redirect((String.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/Forbidden?refNo=" + HttpUtility.UrlEncode(refNo));
                    //break;
                case 404:
                    return Redirect((String.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/PageNotFound");
                    //break;
                default:                    
                    return Redirect((String.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/InternalServerError");
                    //break;

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult InternalServerError()
        {
#if !DEBUG
            _cacheRepository.Remove("MenuAccessDto", _helper.MS_ID);
            _cacheRepository.RemoveGlobal(_helper.MS_ID);
#endif
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });            
             return View(new ErrorViewModel { RequestId = "500" });
        }

        [AllowAnonymous]
        [Route("ErrorHandler/Forbidden")]
        public IActionResult Forbidden(string refNo)
        {
#if !DEBUG
                _cacheRepository.Remove("MenuAccessDto", _helper.MS_ID);
                _cacheRepository.RemoveGlobal(_helper.MS_ID);
#endif
            return View();
        }
        [AllowAnonymous]
        public IActionResult PageNotFound()
        {
            _cacheRepository.Remove("MenuAccessDto", _helper.MS_ID);
            _cacheRepository.RemoveGlobal(_helper.MS_ID);
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> RecordNotFound(ErrorViewModel errorViewModel = null)
        {
            _cacheRepository.Remove("MenuAccessDto", _helper.MS_ID);
            _cacheRepository.RemoveGlobal(_helper.MS_ID);
            return await Task.FromResult(View(errorViewModel));
        }

    }
}
