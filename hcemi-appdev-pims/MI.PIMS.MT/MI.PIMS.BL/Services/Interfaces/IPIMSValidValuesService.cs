using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IPIMSValidValuesService
    {
        Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD);
        Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues();
        Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues_VVCdOnly(string p_VV_SET_NAME, string p_BUS_SEG_CD);
        Task<IEnumerable<State_Mandated_Ind_Dto>> GetStateMandatedInds();
        Task<IEnumerable<INCL_EXCL_CD_Dto>> GetInclExclCDs();
        Task<IEnumerable<Allwd_plc_of_svc_Dto>> GetAllwdPlcOfSvcs();
        Task<IEnumerable<State_CD_Dto>> GetStateCDs();
        Task<IEnumerable<Delegate_Um_Dto>> GetDelegateUms();
        Task<IEnumerable<Prog_Mgd_By_Dto>> GetProgMgdBy();
        Task<IEnumerable<DX_Ind_Dto>> GetDXInds();
        Task<IEnumerable<St_App_Ind_Dto>> GetStAppInds();
        Task<IEnumerable<Int_Ext_CD_Dto>> GetIntExtCDs();
        Task<IEnumerable<DTQ_Applies_Dto>> GetDTQApplies();
        Task<IEnumerable<InPatientTypeRule>> GetInPatientTypeRules();
        Task<IEnumerable<OutPatientTypeRule>> GetOutPatientTypeRules();
        Task<IEnumerable<OutPatientFacTypeRule>> GetOutPatientFacTypeRules();
        Task<IEnumerable<RuleTypeReason>> GetRuleTypeReason();
        Task<IEnumerable<Ret_Factor_DD_Dto>> GetRetentionFactors();
        Task<IEnumerable<Red_Factor_DD_Dto>> GetReductionFactors();
        Task<IEnumerable<DTQ_TYPE_Dto>> GetDTQ_TYPE();
        Task<IEnumerable<DTQ_RSN_Dto>> GetDTQ_RSN();
        Task<SOS_Site_Ind_Dto> GetSOSSiteAndService();
        Task<SOS_Site_Ind_Dto> GetSOSSiteOnly();
        Task<IEnumerable<POS_APPL_Dto>> GetPLC_OF_SVC_CD();
        Task<IEnumerable<POS_INCL_EXCL_CD_Dto>> GetPOS_INCL_EXCL_CD();
        Task<IEnumerable<ATS_ISSUE_GOV_Dto>> GetStateIssueGov();
    }
}
 