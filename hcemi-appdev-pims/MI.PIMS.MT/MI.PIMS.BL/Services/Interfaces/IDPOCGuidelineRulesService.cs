using MI.PIMS.BO.Dtos;
using MI.PIMS.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCGuidelineRulesService
    {
        Task<DPOC_Inv_Gdln_Rules_V_Dto> GetDPOCGuidelineRulesDetailByKey(DPOC_Inv_Gdln_Rules_V_Key obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetDPOCGuidelineRules(DPOC_Inventories_Param_Dto obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetPendingByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSIDSummary(DPOC_Inv_Gdln_Rules_Param_Dto obj);
        Task<IEnumerable<DPOC_INV_PLCY_LKP_V_Dto>> GetInvPlcyLkp(DPOC_INV_PLCY_LKP_Param dPOC_INV_PLCY_LKP_Param);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineRulesList(string p_text, string columnName);
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineIds(string text);
    }
}
