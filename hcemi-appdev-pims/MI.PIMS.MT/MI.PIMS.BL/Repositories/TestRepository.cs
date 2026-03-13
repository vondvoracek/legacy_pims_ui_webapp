using Dapper;
using MI.PIMS.BO.Dtos;
using MI.PIMS.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MI.PIMS.BL.Common;

namespace MI.PIMS.BL.Repositories
{
    public class TestRepository : DapperSQLServerBaseRepository
    {
        public async Task<UserInfoDto> GetUserInfo(string MS_ID)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MS_ID", MS_ID);

            var data = await QueryFirstOrDefaultAsync<UserInfoDto>("usp_GetUserInfo", parameter, 60, Helper.GetSQLConnectionString_BCRT);
            return data;
        }
    }
}
