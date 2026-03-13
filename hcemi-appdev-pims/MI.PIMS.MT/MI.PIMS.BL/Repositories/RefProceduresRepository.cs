using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class RefProceduresRepository: DapperPostgresBaseRepository
    {
        public RefProceduresRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<Ref_Procedures_T_Dto>> GetRefProceduresByFilter(string p_FILTER_TEXT)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "P_FILTER_TEXT" , Value  = p_FILTER_TEXT.ToStringNullSafe() , Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar}, 
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<Ref_Procedures_T_Dto>("USP_GET_PIMS_REF_PROCEDURES_T_BY_FITLER_PRC", parameters.ToArray());
            return data;
        }
    }
}
