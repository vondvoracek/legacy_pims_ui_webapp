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
    public class TestOracleRepository : DapperOracleBaseRepository
    {
        public TestOracleRepository(Helper helper) : base(helper)
        {
        }

        public async Task<IEnumerable<DY_Test>> Get_DY_Test()
        {
            var parameter = new DynamicParameters();
            var data = await QueryAsync<DY_Test>("DY_TEST_PRC", parameter);
            return data;
        }

    }
}
