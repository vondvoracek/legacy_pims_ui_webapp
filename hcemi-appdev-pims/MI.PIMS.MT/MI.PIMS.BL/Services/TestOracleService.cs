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
    public class TestOracleService: ITestOracleService
    {
        private readonly TestOracleRepository _repo;

        public TestOracleService(TestOracleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DY_Test>> Get_DY_Test()
        {
            var data = await _repo.Get_DY_Test();
            return data;
        }
    }
}
