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
    public class XrefStatusRepository : DapperPostgresBaseRepository
    {        
        public XrefStatusRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<Xref_Status_T_Dto>> GetPIMSXrefStatus()
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };
            var data = await QueryCursorAsync<Xref_Status_T_Dto>("usp_Get_PIMS_Xref_Status_T_prc", parameters.ToArray());
            return data;
        }

    }
}
