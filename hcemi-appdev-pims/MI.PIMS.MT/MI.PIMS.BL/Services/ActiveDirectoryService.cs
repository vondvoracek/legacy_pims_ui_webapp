using DalSoft.RestClient;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
//using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly Helper _helper;

        //private readonly IGraphHandlerService _graphHandlerService;
        public ActiveDirectoryService(Helper helper
            //, IGraphHandlerService graphHandlerService
            )
        {
            _helper = helper;
            //_graphHandlerService = graphHandlerService;
        }

        public async Task<IEnumerable<ActiveDirectoryUserDto>> GetActiveDirectoryUser(string ms_id, string last_name, string first_name)
        {
            var client = new RestClient(_helper.MIServicesAPIUrl);
            var result = await client.Resource("ActiveDirectory/ActiveDirectoryUserTOList")
                .Query(new { ms_id = ms_id?.ToString() ?? "", last_name = last_name?.ToString() ?? "", first_name = first_name?.ToString() ?? "", adReadLimit = 50 }).Get();
            return result;
        }
        public async Task<bool> FindUserInGroup(string ms_id)
        {
            var client = new RestClient(_helper.MIServicesAPIUrl);
            var result = await client.Resource("ActiveDirectory/FindUserInGroup").Query(new { ms_id = ms_id, groups = _helper.AccessGlobalGroup }).Get();
            return result;
        }

        public async Task<ActiveDirectoryUserTO> ActiveDirectoryUserTO(string ms_id)
        {
            var client = new RestClient(_helper.MIServicesAPIUrl);
            string queryString = "ActiveDirectory/ActiveDirectoryUserTO";
            var task = await client.Resource(queryString).Query(new { ms_id = ms_id?.ToString() }).Get();
            return task;
        }
    }
}
