using MI.PIMS.BL.Common;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.Admin.Models;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Providers;
using MI.PIMS.UI.Services.MIAuthenticate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Helper = MI.PIMS.UI.Common.Helper;
using IActiveDirectoryService = MI.PIMS.BL.Services.Interfaces.IActiveDirectoryService;

namespace MI.PIMS.UI.Extensions
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private static readonly string RoleClaimType = $"http://{typeof(ClaimsTransformer).FullName.Replace('.', '/')}/role";
        private readonly IRoleProvider _roleProvider;
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly ILoggerService _logger;
        private readonly Helper _helper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IMIAuthenticateService _miAuthenticateService;
        private readonly IUserInfoService _userInfoService;

        public ClaimsTransformer(ICacheProvider cacheProvider
                                , ICacheRepository cacheRepository
                                , Helper helper
                                , IRoleProvider roleProvider
                                , IMIAuthenticateService miAuthenticateService
                                , IActiveDirectoryService activeDirectoryService
                                , ILoggerService logger
                                , IUserInfoService userInfoService)
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _helper = helper;
            _activeDirectoryService = activeDirectoryService;
            _miAuthenticateService = miAuthenticateService;
            _userInfoService = userInfoService;
            _logger = logger;
            _roleProvider = roleProvider ?? throw new ArgumentNullException(nameof(roleProvider));
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Cast the principal identity to a Claims identity to access claims etc...
            var oldIdentity = (ClaimsIdentity)principal.Identity;

            // "Clone" the old identity to avoid nasty side effects.
            // NB: We take a chance to replace the claim type used to define the roles with our own.
            var newIdentity = new ClaimsIdentity(
                                        oldIdentity.Claims,
                                        oldIdentity.AuthenticationType,
                                        oldIdentity.NameClaimType,
                                        RoleClaimType
                                    );

            //_logger.Info(string.Join( ", ", principal.Claims.Select(c => c.ToString())));    

            // Get MSID out of the Claim. 
            //Un-Comment this out for AD SSO Integration 
            
            var msid = principal.Claims.FirstOrDefault(x => x.Type == "msid")?.Value;
            var displayname = principal.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var lastname = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
            var firstname = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            var emailaddress = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var globalgroup = principal.Claims.Where(x => x.Type == "groups").ToList();

            //_logger.Info(msid);

            //var userInfo_T_Dto = _cacheProvider.GetGlobal<UserInfo_T_Dto>("");
            UserInfo_T_Dto userInfo_T_Dto = null; // _cacheProvider.GetGlobal<UserInfo_T_Dto>(msid);            
//#if DEBUG            
//            //_logger.Debug("right after checknig global caching  userInfo_T_Dto : " + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo_T_Dto));
//#else
//            userInfo_T_Dto = _cacheProvider.GetGlobal<UserInfo_T_Dto>(msid);
//            //_logger.Debug("right after checknig global caching  userInfo_T_Dto : " + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo_T_Dto));
//#endif            
            if (userInfo_T_Dto == null)
            {
                var isValid = false;
                if (globalgroup.Where(x => x.Value == _helper.AccessGlobalGroup).Count() > 0)  //"AZU_PIMS_ONLINE_ACCESS"
                {
                    isValid = true;
                }
              
                if (isValid)
                {
                    try
                    {
                        //var testMSID = "dyu1001";
                        //msid = testMSID;
                        userInfo_T_Dto = await _userInfoService.Get(msid);
                        //userInfo_T_Dto = await _miAuthenticateService.GetUserInfo(new UserSearchParam { MS_ID = msid });
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message, ex);
                        newIdentity.AddClaim(new Claim(RoleClaimType, "errorInUserGet"));
                        return new ClaimsPrincipal(newIdentity);
                    }
                    
                    if (userInfo_T_Dto != null)
                    {
                        // If the user has been set PIMS_USER to false OR ACTIVE to false then return forbidden error to the user.
                        if(userInfo_T_Dto.ACTIVE == "0" || String.IsNullOrEmpty(userInfo_T_Dto.ACTIVE) || userInfo_T_Dto.PIMS_USER == "0" || String.IsNullOrEmpty(userInfo_T_Dto.PIMS_USER))
                        {
                            newIdentity.AddClaim(new Claim(RoleClaimType, "inactive"));
                            return new ClaimsPrincipal(newIdentity);
                        }

                        //Else if User.ACTIVE is true then SET cache for the user.
//#if DEBUG
//#else
//                        _cacheRepository.SetGlobal(msid, userInfo_T_Dto);
//#endif
                    }
                    else if (userInfo_T_Dto == null)
                    {
                        //DY: 12/02/24 - Add user into UserInfo_T table if users has global group from claims data. 
                        UserInfo_AddDto newUserInfo_T_Dto = new UserInfo_AddDto();
                        {
                            //newUserInfo_T_Dto.P_MS_ID = msid.ToLower();
                            newUserInfo_T_Dto.P_MS_ID = msid.ToLower();
                            newUserInfo_T_Dto.P_SUP_MSID = "";
                            newUserInfo_T_Dto.P_SUP_NAME = "";
                            newUserInfo_T_Dto.P_LNAME = lastname;
                            newUserInfo_T_Dto.P_FNAME = firstname;
                            newUserInfo_T_Dto.P_EMAIL = emailaddress;
                            newUserInfo_T_Dto.P_PHONE = "";
                            newUserInfo_T_Dto.P_DIV_CODE = "";
                            newUserInfo_T_Dto.P_DIVISION_NAME = "";
                            newUserInfo_T_Dto.P_DEPARTMENT_NAME =   "";
                            newUserInfo_T_Dto.P_ACTIVE = "1";
                            //newUserInfo_T_Dto.LST_UPDT_DT = DateTime.Today;
                            newUserInfo_T_Dto.P_MANUALUPDT = "0";
                            newUserInfo_T_Dto.P_LST_UPDT_BY = msid.ToLower();
                            newUserInfo_T_Dto.P_DISPLAYNAME = displayname;
                            newUserInfo_T_Dto.P_APP_ROLEIDS = "2";
                            newUserInfo_T_Dto.P_APP_ROLEID = 2;
                            newUserInfo_T_Dto.P_PIMS_USER = "1";
                        };

                        await _userInfoService.Add(newUserInfo_T_Dto);

                        _logger.Debug("added new userinfo when cache and db doesn't have record : " + Newtonsoft.Json.JsonConvert.SerializeObject(newUserInfo_T_Dto));

                        UserSearchParam userSearchParam = new UserSearchParam
                        {
                            MS_ID = msid,
                            IsUserRecord = true
                        };
                        userInfo_T_Dto = await _userInfoService.Get(msid);
//#if !DEBUG
//                        _cacheRepository.SetGlobal(msid, userInfo_T_Dto);
//#endif
                    }
                }

                // Fetch the roles for the user and add the claims of the correct type so that roles can be recognized.
                if (userInfo_T_Dto == null)
                {
                    return new ClaimsPrincipal(newIdentity);
                }
                else
                {
                    if (!string.IsNullOrEmpty(userInfo_T_Dto.APP_ROLEIDS.ToStringNullSafe()))
                    {
                        //_logger.Debug("userInfo_T_Dto null but valid group and get userInfo_T_Dto from db : " + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo_T_Dto));

                        var roles = await _roleProvider.GetUserRolesAsync(userInfo_T_Dto.APP_ROLEIDS.ToStringNullSafe());
                        newIdentity.AddClaims(roles.Select(r => new Claim(RoleClaimType, r)));
                    }
                    else
                    {
                        _logger.Debug("APP_ROLEIDS is null in userInfo_T_Dto : " + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo_T_Dto));
                        newIdentity.AddClaim(new Claim(RoleClaimType, "noroles"));
                        return new ClaimsPrincipal(newIdentity);
                    }
                }

                // Create and return a new claims principal
                return new ClaimsPrincipal(newIdentity);
            }

            else {

                if (!string.IsNullOrEmpty(userInfo_T_Dto.APP_ROLEIDS.ToStringNullSafe()))
                {
                    //_logger.Debug("userInfo_T_Dto has caching value : " + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo_T_Dto));

                    var roles = await _roleProvider.GetUserRolesAsync(userInfo_T_Dto.APP_ROLEIDS.ToStringNullSafe());
                    newIdentity.AddClaims(roles.Select(r => new Claim(RoleClaimType, r)));                    
                }

                // Create and return a new claims principal
                return new ClaimsPrincipal(newIdentity);
            }  
        }
    }
}
