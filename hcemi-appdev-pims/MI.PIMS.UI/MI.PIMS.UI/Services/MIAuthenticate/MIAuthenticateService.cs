using DalSoft.RestClient;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.Admin.Models;
using MI.PIMS.UI.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.MIAuthenticate
{
    public class MIAuthenticateService: IMIAuthenticateService
    {
        private readonly IConfiguration _config;
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly IUserInfoService _userInfoService;
        private readonly Helper _helper;
        public MIAuthenticateService(IConfiguration config
                                    ,ICacheProvider cacheProvider
                                    ,Helper helper
                                    ,IUserInfoService userInfoService
                                    , ICacheRepository cacheRepository)
        {
            _config = config;
            _cacheProvider = cacheProvider;
            _helper = helper;
            _cacheRepository = cacheRepository;
            _userInfoService = userInfoService;
        }
        public async Task<UserInfo_T_Dto> GetUserInfo(UserSearchParam param)
        {            
            var result = await _userInfoService.Get(param.MS_ID);
            return result;
        }

        public async Task<UserInfo_T_Dto> UserInfo_T_Dto2(string ms_id)
        {            
            UserInfo_T_Dto userInfo_T_Dto = await _userInfoService.Get(ms_id);


            if (userInfo_T_Dto != null)
                return userInfo_T_Dto;
            else
            {
                userInfo_T_Dto = GetUserInfo(new UserSearchParam { MS_ID = ms_id }).Result;
#if !DEBUG
                _cacheRepository.SetGlobal(ms_id, userInfo_T_Dto);
#endif
            }

            return userInfo_T_Dto;
        }

        public UserInfo_T_Dto UserInfo_T_Dto(string ms_id)
        {

            UserInfo_T_Dto userInfo_T_Dto = null;
#if !DEBUG
            userInfo_T_Dto = _cacheProvider.GetGlobal<UserInfo_T_Dto>(_helper.MS_ID);
#endif

            if (userInfo_T_Dto != null) 
                return userInfo_T_Dto;
            else
            {
                userInfo_T_Dto = GetUserInfo(new UserSearchParam { MS_ID = ms_id }).Result;
#if !DEBUG
                _cacheRepository.SetGlobal(ms_id, userInfo_T_Dto);
#endif
            }

            return userInfo_T_Dto;
        }
    }
}
