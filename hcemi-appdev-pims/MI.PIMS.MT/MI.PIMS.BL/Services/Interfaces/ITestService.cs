using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface ITestService
    {
        Task<UserInfoDto> GetUserInfo(string MS_ID);
    }
}
