using MI.PIMS.BL.Common;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class EPALProceduresService : IEPALProceduresService
    {
        private readonly EPALProceduresRepository _repo;

        public EPALProceduresService(EPALProceduresRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresSearch(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProceduresSearch(obj);
            return data;
        }

        public async Task<EPAL_Procedures_V_Dto> CheckIsDonorRecord(string p_EPAL_HIERARCHY_KEY)
        {
            var data = await _repo.CheckIsDonorRecord(p_EPAL_HIERARCHY_KEY);
            return data;
        }


        public async Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresHist(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProceduresHist(obj);
            return data;
        }


        public async Task<EPAL_PIMSHierarchyCodeCombinationExists_Dto> CheckPIMSHierarchyCodeCombinationExists(EPAL_PIMSHierarchyCodeCombinationExists_Dto obj)
        {
            var data = await _repo.CheckPIMSHierarchyCodeCombinationExists(obj);
            return data;
        }


        public async Task<EPAL_Procedures_V_Dto> GetEPALProcedureByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureByPIMS_ID(obj);
            return data;
        }

        public async Task<EPAL_Procedures_V_Dto> GetEPALProcedureCurrVerByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureCurrVerByPIMS_ID(obj);
            return data;
        }


        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_DiagCodes_Dto>> GetEPALProcedureDGCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureDGCodesByPIMS_ID(obj);
            return data;
        }


        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_RevCodes_Dto>> GetEPALProcedureRevCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureRevCodesByPIMS_ID(obj);
            return data;
        }
        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_AllocatedPlaces_Dto>> GetEPALProcedureAllowedPlaceByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureAllowedPlaceByPIMS_ID(obj);
            return data;
        }
        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto>> GetEPALProcedureChangeHistoryByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureChangeHistoryByPIMS_ID(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>> GetEPALProcedureAPPlTOSTATESByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureAPPlTOSTATESByPIMS_ID(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_PROCS_MODIFIERS_Dto>> GetEPALProcedurePROCS_MODIFIERSByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedurePROCS_MODIFIERSByPIMS_ID(obj);
            return data;
        }
        public async Task<EPAL_Proc_Status_Dto> GetEPALProcStatus(EPAL_Procedures_Codes_Dto obj)
        {
            var data = await _repo.GetEPALProcStatus(obj);
            return data;

        }
        
        public async Task<IEnumerable<EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto>> GetPIMSHierarchyCodesXwalkByEPALBusSegCD(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj)
        {
            var data = await _repo.GetPIMSHierarchyCodesXwalkByEPALBusSegCD(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto>> GetAllPIMSHierarchyCodesXwalk()
        {
            var data = await _repo.GetAllPIMSHierarchyCodesXwalk();
            return data;
        }

        public async Task<EPAL_Procedures_AssoCodes_Status_Dto> GetPIMS_IDExistStatus(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMS_IDExistStatus(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Additional_Info_History_Dto>> GetPIMSAdditionalInfoHistory(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMSAdditionalInfoHistory(obj);
            return data;
        }

        public async Task<UpdateDto> EPAL_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.EPAL_INS_UPD_DRIVER(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = Common.Helper.AlreadyExistMessage;
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Record is not added!";
                updateTO.StatusType = RetValStatus.Error.ToString();
            }
            else if (updateTO.StatusID == 2)
            {
                updateTO.Message = "Record partially updated!";
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
            }

            return updateTO;
        }

        public async Task<UpdateDto> EPAL_HISTORIC_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.EPAL_HISTORIC_INS_UPD_DRIVER(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = Common.Helper.AlreadyExistMessage;
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Record is not added!";
                updateTO.StatusType = RetValStatus.Error.ToString();
            }
            else if (updateTO.StatusID == 2)
            {
                updateTO.Message = "Record partially updated!";
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
            }

            return updateTO;
        }
        public async Task<UpdateDto> EPAL_DELETE_DRIVER_PRC(EPAL_Ins_Upd_Pkg_Param obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.EPAL_DELETE_DRIVER_PRC(obj)
            };            

            return updateTO;
        }
        public async Task<IEnumerable<EPAL_Procedures_Modifier_Dto>> GetEPALProceduresModifiers(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureModifiersByPIMS_ID(obj);
            return data;
        }

        public async Task<EPAL_Procs_Sos_T_Dto> GetPIMSProcsSOS(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPIMSProcsSOS(obj);
            return data;
        }

        public async Task<EPAL_Catagories_Dto> GetPIMSEPALCategories(EPAL_Catagories_Dto obj)
        {
            var data = await _repo.GetPIMSEPALCategories(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procs_Prog_Mgd_By_V>> GetProgMgdByPIMSID(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetProgMgdByPIMSID(obj);
            return data;
        }

        public async Task<EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto> GetEPALProcedureTMaxPriorAuthDt(EPAL_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetEPALProcedureTMaxPriorAuthDt(obj);
            return data;
        }

        public async Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByType(EPAL_Catagory_By_Type_Param_Dto ePAL_Catagory_By_Type_Param_Dto)
        {
            var data = await _repo.GetPIMSEPALCategoriesByType(ePAL_Catagory_By_Type_Param_Dto);
            
            //If filter by Drug name is provided then filter the list down MFQ 1/30/2024
            if (!string.IsNullOrEmpty(ePAL_Catagory_By_Type_Param_Dto.P_DRUG_NM))
            {                
                data = data.Where(ct => ct.DRUG_NM != null && ct.CATEGORY != null && Array.IndexOf(ct.DRUG_NM.Split(','), ePAL_Catagory_By_Type_Param_Dto.P_DRUG_NM) >= 0).ToList();                
            }
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_T_Max_PA_PRE_DT>> GetPIMSEPALMaxPAPREEXPDT(string p_EPAL_HIERARCHY_KEY)
        {
            var data = await _repo.GetPIMSEPALMaxPAPREEXPDT(p_EPAL_HIERARCHY_KEY);
            return data;
        }

        public async Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByProcCDDrugNM(EPAL_Catagory_By_ProcCDDrugNM_Param_Dto obj)
        {
            var data = await _repo.GetPIMSEPALCategoriesByProcCDDrugNM(obj);
            return data;

        }

        public async Task<IEnumerable<Ret_Factors_Dto>> GetRetFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj)
        {
            var data = await _repo.GetRetFactorsByPIMSID(obj);
            return data;
        }
        public async Task<IEnumerable<Red_Factors_Dto>> GetRedFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj)
        {
            var data = await _repo.GetRedFactorsByPIMSID(obj);
            return data;
        }
    }
}
