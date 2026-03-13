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
    public class RefRevenuesService: IRefRevenuesService
    {
        private readonly RefRevenuesRepository _repo;

        public RefRevenuesService(RefRevenuesRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Ref_Revenue_T_Dto>> GetRefRevenuesByFilter(string p_FILTER_TEXT)
        {
            var data = await _repo.GetRefRevenuesByFilter(p_FILTER_TEXT);
            return data;
        }
    }
}
