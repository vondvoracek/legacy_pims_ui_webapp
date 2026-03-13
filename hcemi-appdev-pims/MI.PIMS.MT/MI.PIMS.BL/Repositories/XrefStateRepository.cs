using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class XrefStateRepository : DapperOracleBaseRepository
    {        
        public XrefStateRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<Xref_State_T_Dto>> GetPIMSXrefState()
        {
            var data = await QueryAsync<Xref_State_T_Dto>("usp_Get_PIMS_Xref_State_T_prc", null, 60);
            return data;
        }

    }
}
