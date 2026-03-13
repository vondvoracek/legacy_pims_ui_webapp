using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCService
    {
        Task<IEnumerable<DPOC_Inventories_V_Dto>> GetDPOCSearch(DPOC_Param_Dto obj);
        Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesByPIMS_ID(DPOC_Inventories_Param_Dto obj); //DPOC_Inventories_Param_Dto obj
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetDPOCInvGdlnRulesByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj);
        Task<IEnumerable<DPOC_INV_DTQS_V_Dto>> GetDPOCInvDTQSByPIMSID(DPOC_PIMS_ID_Param_Dto obj);
        Task<IEnumerable<DPOC_INV_DTQS_TGT_V>> GetDPOCInvDTQTGTsByPIMSID(DPOC_PIMS_ID_Param_Dto obj);
        Task<IEnumerable<DPOC_INV_DTQS_HOLDING_V>> GetDPOCInvDTQHoldingsByPIMSID(DPOC_PIMS_ID_Param_Dto obj);
        Task<IEnumerable<DPOC_Inventories_V_Hist_Dto>> GetDPOCInventoriesHistByPIMS_ID(DPOC_PIMS_ID_Param_Dto obj);
        Task<IEnumerable<DPOC_ChangeHistory_Dto>> GetDPOCChangeHistoryByPIMSID(DPOC_PIMS_ID_Param_Dto param);
        
        Task<IEnumerable<DPOC_Inv_Dtqs_V_Dto>> GetDPOC_Inv_Dtqs_V();
        Task<string> GetDPOCIDExistStatus(string dpoc_hierarchy_key, string p_dpoc_package);
        Task<UpdateDto> DPOC_INS_UPD_DRIVER_PRC(DPOC_Ins_Upd_Pkg_Param obj);
        Task<IEnumerable<DPOC_Additional_Req_His_Dto>> GetPIMSAdditionalInfoHistory(string dpoc_hierarchy_key);
        Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesLstUpdtRecordByPIMS_ID(string dpoc_hierarchy_key, string p_dpoc_package);
        Task<IEnumerable<DPOC_POS_Dto>> GetPOS(DPOC_PIMS_ID_Param_Dto dPOC_PIMS_ID_Param_Dto);
        Task<UpdateDto> DPOC_DELETE_DRIVER_PRC(DPOC_Delete_Pkg_Param obj);
        Task<IEnumerable<string>> GetDPOC_SOS_PROVIDER_TIN_EXCL();
        Task<IEnumerable<string>> GetDPOCRelease();
        Task<IEnumerable<string>> GetDPOCPackage();
    }
}
