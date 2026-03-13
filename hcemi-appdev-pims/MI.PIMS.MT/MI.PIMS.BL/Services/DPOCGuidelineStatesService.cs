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
    public class DPOCGuidelineStatesService: IDPOCGuidelineStatesService
    {
        private readonly DPOCGuidelineStatesRepository _repo;

        public DPOCGuidelineStatesService(DPOCGuidelineStatesRepository repo)
        {
            _repo = repo;
        }       

        public async Task<IEnumerable<DPOC_Inv_Gdln_Appl_To_States_T_Dto>> Get(DPOC_Gdln_Param_Dto obj)
        {
            var data = await _repo.Get(obj);
            return data;
        }
    }
}
