using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class AppRoleService: IAppRoleService
    {
        private readonly AppRoleRepository _repo;

        public AppRoleService(AppRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AppRole_T_Dto>> GetAppRole()
        {
            var data = await _repo.GetAppRole();
            return data;
        }

    }
}
