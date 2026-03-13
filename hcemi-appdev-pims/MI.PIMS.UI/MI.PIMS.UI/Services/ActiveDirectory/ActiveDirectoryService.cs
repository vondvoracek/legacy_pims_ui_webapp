using DalSoft.RestClient;
using DalSoft.RestClient.DependencyInjection;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models.Config;
using MI.PIMS.UI.Services.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.ActiveDirectory
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly Helper _helper;
        private readonly AppSettings _appSettings;
        private readonly IRestClientFactory _restClientFactory;
        private readonly IRestClient _restClient;
        private readonly ILoggerService _logger;
        public ActiveDirectoryService(Helper helper, IOptions<AppSettings> appSettings, IRestClientFactory restClientFactory, ILoggerService logger)
        {
            _helper = helper;
            _appSettings = appSettings.Value;
            _restClientFactory = restClientFactory;
            _logger = logger;

            _restClient = _restClientFactory.CreateClient("MIServicesAPI");
        }

        public async Task<IEnumerable<ActiveDirectoryUserDto>> GetUsers(string ms_id, string last_name, string first_name, string addreadlimit)
        {
            string queryString = "ActiveDirectory/ActiveDirectoryUserTOList?ms_id=" + ms_id + "&last_name=" + last_name + "&first_name=" + first_name + "&adReadLimit=" + addreadlimit;            
            var result = await _restClient.Resource(queryString).Get();
            return result;
        }

        public async Task<IEnumerable<ActiveDirectoryUserDto>> GetActiveDirectoryUser(string ms_id, string last_name, string first_name)
        {            
            var result = await _restClient.Resource("ActiveDirectory/ActiveDirectoryUserTOList").Query(new { ms_id = ms_id?.ToString() ?? "", last_name = last_name?.ToString() ?? "", first_name = first_name?.ToString() ?? "", adReadLimit = 50 }).Get();
            return result;
        }

        public async Task<bool> FindUserInGroup()
        {
            bool isFound= false;
            string queryString = "ActiveDirectory/FindUserInGroup?ms_id=" + _helper.MS_ID + "&groups=" + _appSettings.AccessGlobalGroup;
            try
            {
                isFound = await _restClient.Resource(queryString).Get();
            }            
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return isFound;
        }
    }
}
