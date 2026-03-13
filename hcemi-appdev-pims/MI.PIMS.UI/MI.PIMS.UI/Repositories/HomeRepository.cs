using MI.PIMS.BO.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Repositories
{
    public class HomeRepository
    {
        private readonly MarketRepository _marketRepository;
        private readonly AppDataRoleRepository _appRoleRepository;


        public HomeRepository(MarketRepository marketRepository, AppDataRoleRepository appRoleRepository)
        {            
            _marketRepository = marketRepository;
            _appRoleRepository = appRoleRepository;
        }

        public async void SetStaticDataToCache()
        {
            await _marketRepository.GetSet();
            await _appRoleRepository.GetSet();   
        }
    }
}
