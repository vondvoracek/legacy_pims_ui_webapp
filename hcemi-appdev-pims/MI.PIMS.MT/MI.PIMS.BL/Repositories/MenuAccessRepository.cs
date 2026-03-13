using Dapper;
using MI.PIMS.BO.Dtos;
using MI.PIMS.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MI.PIMS.BL.Common;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace MI.PIMS.BL.Repositories
{
    public class MenuAccessRepository(ILoggerService _logger, Helper helper) : DapperPostgresBaseRepository(helper)
    {
        public async Task<IEnumerable<MenuAccessDto>> GetMenuAccess(string MS_ID)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_ms_id", Value = MS_ID, Direction = ParameterDirection.Input },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };
            var data = await QueryCursorAsync<MenuAccessDto>("USP_PIMS_GETMENUACCESS_PRC", parameters.ToArray(), "result_cursor", 60);

            return data;
        }

        public async Task<int> UpdateAppRole(UserInfo_T_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_MS_ID", Value = obj.MS_ID, Direction = ParameterDirection.Input },
                new() { ParameterName = "p_APP_ROLEID", Value = obj.APP_ROLEID, Direction = ParameterDirection.Input },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var retVal = await ExecuteAsync("usp_PIMS_USERINFO_T_UPDATE_APP_ROLEID_PRC", obj);
            notices = AnalyzeNotices(retVal);
            return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in UpdateAppRole for MS ID: {obj.MS_ID}, App Role ID: {obj.APP_ROLEID}. Exception: {ex}");
                return -1;
            }
        }
    }
}
