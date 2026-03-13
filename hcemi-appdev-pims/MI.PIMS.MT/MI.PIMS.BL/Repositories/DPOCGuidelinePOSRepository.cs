using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public sealed class DPOCGuidelinePOSRepository: DapperOracleBaseRepository
    {
        public DPOCGuidelinePOSRepository(Helper helper) : base(helper) { }

        /// <summary>
        /// XX Removed
        /// </summary>
        /// <param name="dPOC_PIMS_ID_Param_Dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_POS_Dto>> GetByDPOC(DPOC_PIMS_ID_Param_Dto dPOC_PIMS_ID_Param_Dto)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_DPOC_HIERARCHY_KEY", dPOC_PIMS_ID_Param_Dto.p_DPOC_HIERARCHY_KEY);
            parameter.Add("p_DPOC_VER_EFF_DT", dPOC_PIMS_ID_Param_Dto.p_DPOC_VER_EFF_DT);
            parameter.Add("p_DPOC_PACKAGE", dPOC_PIMS_ID_Param_Dto.p_DPOC_PACKAGE);
            parameter.Add("p_DPOC_RELEASE", dPOC_PIMS_ID_Param_Dto.p_DPOC_RELEASE);
            var data = await QueryAsync<DPOC_POS_Dto>("usp_Get_PIMS_APP_DPOC_INV_POS_V_BY_PIMS_ID_PRC", parameter, 60);
            return data;
        }

        /// <summary>
        /// XX Removed
        /// </summary>
        /// <param name="dPOC_Gdln_Param_Dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_POS_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto dPOC_Gdln_Param_Dto)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_DPOC_HIERARCHY_KEY", dPOC_Gdln_Param_Dto.p_DPOC_HIERARCHY_KEY);
            parameter.Add("p_DPOC_VER_EFF_DT", dPOC_Gdln_Param_Dto.p_DPOC_VER_EFF_DT);
            parameter.Add("p_DPOC_PACKAGE", dPOC_Gdln_Param_Dto.p_DPOC_PACKAGE);
            parameter.Add("p_DPOC_RELEASE", dPOC_Gdln_Param_Dto.p_DPOC_RELEASE);
            parameter.Add("P_IQ_GDLN_ID", dPOC_Gdln_Param_Dto.p_IQ_GDLN_ID);
            var data = await QueryAsync<DPOC_POS_Dto>("usp_Get_PIMS_APP_DPOC_INV_GDLN_POS_V_BY_PIMS_ID_PRC", parameter, 60);
            return data;
        }
    }
}
