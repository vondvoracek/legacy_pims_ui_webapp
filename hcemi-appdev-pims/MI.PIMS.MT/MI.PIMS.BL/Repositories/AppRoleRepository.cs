using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class AppRoleRepository : DapperOracleBaseRepository
    {
        public AppRoleRepository(Helper helper) : base(helper) { }

        public async Task<IEnumerable<AppRole_T_Dto>> GetAppRole()
        {
            var data = await QueryAsync<AppRole_T_Dto>("usp_Get_PIMS_APP_ROLE_T_PRC", null, 60);
            return data;
        }

    }
}
