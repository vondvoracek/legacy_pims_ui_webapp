using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IMenuAccessService
    {
        Task<IEnumerable<MenuAccessDto>> GetMenuAccess(string MS_ID);
    }
}
