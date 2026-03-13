using MI.PIMS.BL.Repositories;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL
{
    public class DPOCGuidelineDiagnosisCodesService : IDPOCGuidelineDiagnosisCodesService
    {
        private readonly DPOCGuidelineDiagnosisCodesRepository _repo;

        public DPOCGuidelineDiagnosisCodesService(DPOCGuidelineDiagnosisCodesRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto obj)
        {
            var data = await _repo.GetByGuideline(obj);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByDPOC_ID(DPOC_Gdln_Param_Dto obj)
        {
            var data = await _repo.GetByDPOC_ID(obj);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetCodesByGuideline(DPOC_Gdln_Param_Dto obj)
        {
            var data = await _repo.GetCodesByGuideline(obj);
            return data;
        }
    }
}
