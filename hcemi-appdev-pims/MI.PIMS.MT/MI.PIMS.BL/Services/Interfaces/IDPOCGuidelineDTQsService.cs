using MI.PIMS.BO.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCGuidelineDTQsService
    {
        Task<IEnumerable<DPOC_INV_DTQS_V_Dto>> GetConfigurations(DPOC_Gdln_Param_Dto dPOC_PIMS_ID_Param_Dto);
    }
}