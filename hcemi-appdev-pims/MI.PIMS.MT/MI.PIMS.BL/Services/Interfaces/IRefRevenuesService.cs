using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IRefRevenuesService
    {
        Task<IEnumerable<Ref_Revenue_T_Dto>> GetRefRevenuesByFilter(string p_FILTER_TEXT);
    }
}
