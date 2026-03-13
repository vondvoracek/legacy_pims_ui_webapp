using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.Admin.Models;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Extensions;
using MI.PIMS.UI.Providers;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Middlewares
{
    public class PermissionMiddleware
    {
        private static readonly string RoleClaimType = $"http://{typeof(ClaimsTransformer).FullName.Replace('.', '/')}/role";        
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly ILoggerService _logger;
        private readonly Helper _helper;
        private readonly IUserInfoService _userInfoService;
        private readonly IRoleProvider _roleProvider;

        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next,
            ICacheProvider cacheProvider,
            ICacheRepository cacheRepository,
            Helper helper,
            ILoggerService logger,
            IUserInfoService userInfoService,
            IRoleProvider roleProvider)
        {
            _next = next;
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _helper = helper;
            _logger = logger;
            _userInfoService = userInfoService;
            _roleProvider = roleProvider ?? throw new ArgumentNullException(nameof(roleProvider));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value == "/ErrorHandler/Forbidden")
            {
                await _next(context);
            }else if (context.Request.Path.Value == "/MicrosoftIdentity/Account/AccessDenied")
            {
                var guidRefNo = Guid.NewGuid().ToString();
                var msid = context.User.Claims.FirstOrDefault(x => x.Type == "msid")?.Value;
                _logger.Error("Ref No: " + guidRefNo + " - User landed to a page which is not in list of rights for MS_ID: " + msid);
                context.Response.Headers.Add("refNo", guidRefNo);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            if (context.User.Identity.IsAuthenticated)
            {
                var principal = context.User;
                var rolesClaims = principal.Claims.Where(x => x.Type == RoleClaimType).ToList();
                string guidRefNo = string.Empty;

                if (rolesClaims.Count() > 0 && rolesClaims.Where(v => !int.TryParse(v.Value, out _)).ToList().Count > 0)
                {
                    var msid = principal.Claims.FirstOrDefault(x => x.Type == "msid")?.Value;
                    
                    guidRefNo = Guid.NewGuid().ToString();
                    switch (rolesClaims.FirstOrDefault().Value)
                    {
                        case "inactive":                            
                            _logger.Error("Ref No: " + guidRefNo + " - User record is in-active for MS_ID: " + msid);
                            break;
                        case "errorInUserGet":                            
                            _logger.Error("Ref No: " + guidRefNo + " - Error occured while getting UserInfo for MS_ID: " + msid);
                            break;
                        case "noroles":
                            _logger.Error("Ref No: " + guidRefNo + " - User record has no roles set for MS_ID: " + msid);
                            break;
                        default: 
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(guidRefNo) && context.Request.Path.Value != "/ErrorHandler/Forbidden")
                {
                    context.Response.Headers.Add("refNo", guidRefNo);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;                    
                    return;
                }
            }            

            await _next(context);
        }        
    }
}
