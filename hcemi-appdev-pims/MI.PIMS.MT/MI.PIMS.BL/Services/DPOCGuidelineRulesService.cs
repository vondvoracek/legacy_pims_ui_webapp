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
    public class DPOCGuidelineRulesService : IDPOCGuidelineRulesService
    {
        private readonly DPOCGuidelineRulesRepository _repo;

        public DPOCGuidelineRulesService(DPOCGuidelineRulesRepository repo)
        {
            _repo = repo;
        }
        public async Task<DPOC_Inv_Gdln_Rules_V_Dto> GetDPOCGuidelineRulesDetailByKey(DPOC_Inv_Gdln_Rules_V_Key obj)
        {
            var data = await _repo.GetDPOCGuidelineRulesDetailByKey(obj);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetDPOCGuidelineRules(DPOC_Inventories_Param_Dto obj)
        {
            var data = await _repo.GetDPOCGuidelineRules(obj);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineRulesList(string p_text, string fielName)
        {
            var data = await _repo.GetGuideLineRulesList(p_text, fielName);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineIds(string text)
        {
            var data = await _repo.GetGuideLineIds(text);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetPendingByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var data = await _repo.GetPendingByPIMSID(obj);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var data = await _repo.GetActiveRetiredByPIMSID(obj);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSIDSummary(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var data = await _repo.GetActiveRetiredByPIMSIDSummary(obj);
            return data;


        }
        public async Task<IEnumerable<DPOC_INV_PLCY_LKP_V_Dto>> GetInvPlcyLkp(DPOC_INV_PLCY_LKP_Param dPOC_INV_PLCY_LKP_Param)
        {
            var data = await _repo.GetInvGntvPlcyLkp(dPOC_INV_PLCY_LKP_Param);
            return data;
        }
    }
}
