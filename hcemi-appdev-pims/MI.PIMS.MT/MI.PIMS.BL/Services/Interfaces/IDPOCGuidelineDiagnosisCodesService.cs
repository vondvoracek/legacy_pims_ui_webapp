using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL
{
    public interface IDPOCGuidelineDiagnosisCodesService
    {
        Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetCodesByGuideline(DPOC_Gdln_Param_Dto obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByDPOC_ID(DPOC_Gdln_Param_Dto obj);
    }
}
