using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Repositories
{
    public class AppDataRoleRepository: BaseRepository
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly ICacheProvider _cacheProvider;
        public AppDataRoleRepository(ICacheRepository cacheRepository, ICacheProvider cacheProvider, IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<AppDataRoleDto>> GetSet()
        {
            var appRoles = _cacheProvider.GetGlobal<IEnumerable<AppDataRoleDto>>("AppRoles");
            if (appRoles == null)
            {
                appRoles = await GetAsyncList<AppDataRoleDto>("AppRole/");
                _cacheRepository.SetGlobal("AppRoles", appRoles);
            }
            return appRoles;
        }
    }
}
