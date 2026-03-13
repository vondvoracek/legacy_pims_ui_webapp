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
    public class Entity_XrefRepository : DapperSQLServerBaseRepository
    {
        public async Task<IEnumerable<EntityXrefDto>> GetEntityXref()
        {
            var parameter = new DynamicParameters();
            var data = await QueryAsync<EntityXrefDto>("usp_Entity_Xref_Test", parameter, 60, Helper.GetSQLConnectionString_BCRT);
            return data;
        }
    }
}
