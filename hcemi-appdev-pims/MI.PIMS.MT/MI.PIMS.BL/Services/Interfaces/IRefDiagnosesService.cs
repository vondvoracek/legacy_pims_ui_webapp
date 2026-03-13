using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IRefDiagnosesService
    {
        Task<IEnumerable<Ref_Diagnoses_T_Dto>> GetRefDiagnosesByFilter(string p_FILTER_TEXT);

        Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_Curr_Ver_V_List(EPAL_Procedures_Param_Dto obj);

        Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_V_List(EPAL_Procedures_Param_Dto obj);

        Task<IEnumerable<REF_ALL_DIAG_CD_LISTS_V>> GetPIMS_DiagnosisList();

        Task<DIAG_LIST_NAME_CNT_Dto> GetPIMS_DiagCount(EPAL_Procedures_Param_Dto obj);

        Task<List<DPOC_REF_ALL_DIAG_CD_LISTS_V>> GetDPOC_DiagnosisList();
    }
}
