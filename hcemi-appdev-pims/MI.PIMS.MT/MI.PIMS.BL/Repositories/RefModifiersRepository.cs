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
    public class RefModifiersRepository: DapperPostgresBaseRepository
    {
        public RefModifiersRepository(Helper helper) : base(helper) { }

        public async Task<IEnumerable<Ref_Modifier_V_Dto>> GetRefModifiers(string p_FILTER_TEXT)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_filter_text", Value = p_FILTER_TEXT == null ?  "" : p_FILTER_TEXT, Direction = ParameterDirection.Input },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<Ref_Modifier_V_Dto>("USP_GET_PIMS_REF_MODIFIERS_V_BY_FITLER_PRC", parameters.ToArray());
            return data;
        }
    }
}
