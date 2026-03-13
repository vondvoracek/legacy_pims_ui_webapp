using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.Admin.Models;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.ActiveDirectory;
using MI.PIMS.UI.Services.MIAuthenticate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Handlers
{
    public class AccessGlobalGroupAuthorizationHandler : AuthorizationHandler<AccessGlobalGroupRequirement>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly Helper _helper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IMIAuthenticateService _miAuthenticate;

        public AccessGlobalGroupAuthorizationHandler(ICacheProvider cacheProvider, ICacheRepository cacheRepository, Helper helper, IActiveDirectoryService activeDirectoryService,
            IMIAuthenticateService miAuthenticate)
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _helper = helper;
            _activeDirectoryService = activeDirectoryService;
            _miAuthenticate = miAuthenticate;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AccessGlobalGroupRequirement requirement)
        {
            UserInfoDto userInfoDto = null;
#if !DEBUG
            userInfoDto = _cacheProvider.GetGlobal<UserInfoDto>(_helper.MS_ID);
#endif
            if (userInfoDto == null)
            {
                var isValid = _activeDirectoryService.FindUserInGroup().Result;
                if (isValid)
                {
                    var userInfo_T = await _miAuthenticate.GetUserInfo(new UserSearchParam { MS_ID = _helper.MS_ID });
                    if (userInfo_T != null)
                    {
                        if (userInfo_T.ACTIVE == "1" && userInfo_T.PIMS_USER == "1")
                        {
#if !DEBUG
                            _cacheRepository.SetGlobal(_helper.MS_ID, userInfo_T);
#endif
                            context.Succeed(requirement);
                        }
                    }                    
                }
            }
            else
            {
                context.Succeed(requirement);
            }
            return await Task.FromResult(Task.CompletedTask);
        }
    }

    public class AccessGlobalGroupRequirement : IAuthorizationRequirement { }
}
