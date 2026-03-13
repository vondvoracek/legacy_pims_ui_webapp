using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCGuidelinePOSService
    {
        Task<IEnumerable<DPOC_POS_Dto>> GetByDPOC(DPOC_PIMS_ID_Param_Dto dPOC_PIMS_ID_Param_Dto);
        Task<IEnumerable<DPOC_POS_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto dPOC_Inventories_Param_Dto);
    }
}
