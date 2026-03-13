using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class DPOCGuidelineStatesRepository: DapperPostgresBaseRepository
    {
        public DPOCGuidelineStatesRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Appl_To_States_T_Dto>> Get(DPOC_Gdln_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },                
                new() { ParameterName = "p_IQ_GDLN_ID", Value = obj.p_IQ_GDLN_ID == null ? DBNull.Value : obj.p_IQ_GDLN_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_GDLN_DTQ_SYS_SEQ", Value = obj.p_GDLN_DTQ_SYS_SEQ.ToString() == "0" ? DBNull.Value : obj.p_GDLN_DTQ_SYS_SEQ.ToString(), NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_NUM", Value = obj.p_DPOC_VER_NUM == null ? DBNull.Value : obj.p_DPOC_VER_NUM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Appl_To_States_T_Dto>("USP_GET_PIMS_APP_DPOC_INV_GDLN_APPL_TO_STATES_T_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }
    }
}
