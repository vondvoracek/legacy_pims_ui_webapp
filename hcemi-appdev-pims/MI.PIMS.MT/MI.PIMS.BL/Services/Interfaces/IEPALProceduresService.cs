using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IEPALProceduresService
    {
        Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresSearch(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Procedures_V_Dto> CheckIsDonorRecord(string p_EPAL_HIERARCHY_KEY);
        Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresHist(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_PIMSHierarchyCodeCombinationExists_Dto> CheckPIMSHierarchyCodeCombinationExists(EPAL_PIMSHierarchyCodeCombinationExists_Dto obj);
        Task<EPAL_Procedures_V_Dto> GetEPALProcedureByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Procedures_V_Dto> GetEPALProcedureCurrVerByPIMS_ID(EPAL_Procedures_Param_Dto ePAL_Procedures_Param_Dto);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_DiagCodes_Dto>> GetEPALProcedureDGCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_RevCodes_Dto>> GetEPALProcedureRevCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_AllocatedPlaces_Dto>> GetEPALProcedureAllowedPlaceByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto>> GetEPALProcedureChangeHistoryByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>> GetEPALProcedureAPPlTOSTATESByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Procedures_AssoCodes_V_PROCS_MODIFIERS_Dto>> GetEPALProcedurePROCS_MODIFIERSByPIMS_ID(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Proc_Status_Dto> GetEPALProcStatus(EPAL_Procedures_Codes_Dto obj);
        Task<IEnumerable<EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto>> GetPIMSHierarchyCodesXwalkByEPALBusSegCD(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj);
        Task<IEnumerable<EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto>> GetAllPIMSHierarchyCodesXwalk();
        Task<EPAL_Procedures_AssoCodes_Status_Dto> GetPIMS_IDExistStatus(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Additional_Info_History_Dto>> GetPIMSAdditionalInfoHistory(EPAL_Procedures_Param_Dto obj);
        Task<UpdateDto> EPAL_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj);
        Task<UpdateDto> EPAL_HISTORIC_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj);
        Task<UpdateDto> EPAL_DELETE_DRIVER_PRC(EPAL_Ins_Upd_Pkg_Param obj);
        Task<IEnumerable<EPAL_Procedures_Modifier_Dto>> GetEPALProceduresModifiers(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Procs_Sos_T_Dto> GetPIMSProcsSOS(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Catagories_Dto> GetPIMSEPALCategories(EPAL_Catagories_Dto obj);
        Task<IEnumerable<EPAL_Procs_Prog_Mgd_By_V>> GetProgMgdByPIMSID(EPAL_Procedures_Param_Dto obj);
        Task<EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto> GetEPALProcedureTMaxPriorAuthDt(EPAL_Procedures_Param_Dto obj);
        Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByType(EPAL_Catagory_By_Type_Param_Dto ePAL_Catagory_By_Type_Param_Dto);
        Task<IEnumerable<EPAL_Procedures_T_Max_PA_PRE_DT>> GetPIMSEPALMaxPAPREEXPDT(string p_EPAL_HIERARCHY_KEY);
        Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByProcCDDrugNM(EPAL_Catagory_By_ProcCDDrugNM_Param_Dto obj);
        Task<IEnumerable<Ret_Factors_Dto>> GetRetFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj);
        Task<IEnumerable<Red_Factors_Dto>> GetRedFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj);
    }
}
