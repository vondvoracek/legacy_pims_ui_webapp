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
    public class KLPoliciesRepository : DapperPostgresBaseRepository
    {
        public KLPoliciesRepository(Helper helper) : base(helper){}

        public async Task<IEnumerable<KL_PoliciesDto>> GetKLPolicies(KL_PoliciesParamDto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_dpoc_bus_seg_cd", Value = obj.p_dpoc_bus_seg_cd == null ? DBNull.Value : obj.p_dpoc_bus_seg_cd, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_dpoc_entity_cd", Value = obj.p_dpoc_entity_cd == null ? DBNull.Value : obj.p_dpoc_entity_cd, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_proc_cd", Value = obj.p_proc_cd == null ? DBNull.Value : obj.p_proc_cd, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_plcy_type_cd", Value = obj.p_plcy_type_cd == null ? DBNull.Value : obj.p_plcy_type_cd, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<KL_PoliciesDto>("usp_Get_PIMS_KL_POLICIES_PROCEDURES_V_prc", parameters.ToArray(), "result_cursor", 60);
            return data;
        }
    }
}
