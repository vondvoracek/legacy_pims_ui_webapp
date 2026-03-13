using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class TestService : ITestService
    {
        private readonly TestRepository _repo;

        public TestService(TestRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserInfoDto> GetUserInfo(string MS_ID)
        {
            var data = await _repo.GetUserInfo(MS_ID);
            return data;
        }
    }
}
