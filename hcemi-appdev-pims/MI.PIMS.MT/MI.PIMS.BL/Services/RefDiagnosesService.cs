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
    public class RefDiagnosesService: IRefDiagnosesService
    {
        private readonly RefDiagnosesRepository _repo;

        public RefDiagnosesService(RefDiagnosesRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Ref_Diagnoses_T_Dto>> GetRefDiagnosesByFilter(string p_FILTER_TEXT)
        {
            var data = await _repo.GetRefDiagnosesByFilter(p_FILTER_TEXT);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_Curr_Ver_V_List(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMS_Diagnosis_Curr_Ver_V_List(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_V_List(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMS_Diagnosis_V_List(obj);
            return data;
        }

        public async Task<IEnumerable<REF_ALL_DIAG_CD_LISTS_V>> GetPIMS_DiagnosisList()
        {
            var data = await _repo.GetPIMS_DiagnosisList();
            return data;
        }

        public async Task<DIAG_LIST_NAME_CNT_Dto> GetPIMS_DiagCount(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMS_DiagCount(obj);
            return data;
        }

        public async Task<List<DPOC_REF_ALL_DIAG_CD_LISTS_V>> GetDPOC_DiagnosisList()
        {
            var data = await _repo.GetDPOC_DiagnosisList();
            return data;
        }
    }
}
