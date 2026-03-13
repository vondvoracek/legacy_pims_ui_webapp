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
    public class UserAccessHistRepository: DapperPostgresBaseRepository, IUserAccessHistRepository
    {        
        public UserAccessHistRepository(Helper helper) : base(helper) { }
        public async Task<int> Add(UserAccess_Hist_Dto obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_module_name", obj.MODULE_NAME);
            parameters.Add("p_useraction", obj.USERACTION);
            parameters.Add("p_userselection", obj.USERSELECTION);
            parameters.Add("p_lst_updt_by", obj.LST_UPDT_BY);
            parameters.Add("p_page_id", obj.PAGE_ID);

            //call pims_app_user.usp_pims_useraccess_hist_insert_prc(:p_module_name,:p_useraction,:p_userselection,:p_lst_updt_by,:p_page_id)

            var retVal = await ExecuteAsync("usp_pims_useraccess_hist_insert_prc", parameters, 60);
            return retVal;
        }

        public Task<UpdateDto> Add(string module_name, string useraction, string userselection, int page_id, string lst_updt_by = "")
        {
            throw new NotImplementedException();
        }
    }
}
