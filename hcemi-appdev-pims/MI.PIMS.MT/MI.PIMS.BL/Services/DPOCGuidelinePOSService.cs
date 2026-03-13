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
    public sealed class DPOCGuidelinePOSService: IDPOCGuidelinePOSService
    {
        private readonly DPOCGuidelinePOSRepository _dPOCGuidelinePOSRepository;
        public DPOCGuidelinePOSService(DPOCGuidelinePOSRepository dPOCGuidelinePOSRepository)
        {
            _dPOCGuidelinePOSRepository = dPOCGuidelinePOSRepository;
        }

        public async Task<IEnumerable<DPOC_POS_Dto>> GetByDPOC(DPOC_PIMS_ID_Param_Dto dPOC_PIMS_ID_Param_Dto)
        {
            var data = await _dPOCGuidelinePOSRepository.GetByDPOC(dPOC_PIMS_ID_Param_Dto);
            return data;
        }

        public async Task<IEnumerable<DPOC_POS_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto dPOC_Inventories_Param_Dto)
        {
            var data = await _dPOCGuidelinePOSRepository.GetByGuideline(dPOC_Inventories_Param_Dto);
            return data;
        }
    }
}
