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
    public class RefProceduresService: IRefProceduresService
    {
        private readonly RefProceduresRepository _repo;

        public RefProceduresService(RefProceduresRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Ref_Procedures_T_Dto>> GetRefProceduresByFilter(string p_FILTER_TEXT)
        {
            var data = await _repo.GetRefProceduresByFilter(p_FILTER_TEXT);
            return data;
        }
    }
}
