using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;

namespace MI.PIMS.BL.Services
{
    public class DPOCGuidelineDTQsService: IDPOCGuidelineDTQsService
    {
        private readonly DPOCGuidelineDTQsRepository _repo;
        public DPOCGuidelineDTQsService(DPOCGuidelineDTQsRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<DPOC_INV_DTQS_V_Dto>> GetConfigurations(DPOC_Gdln_Param_Dto dPOC_PIMS_ID_Param_Dto)
        {
            var data = await _repo.GetConfigurations(dPOC_PIMS_ID_Param_Dto);
            return data;
        }       
    }
}
