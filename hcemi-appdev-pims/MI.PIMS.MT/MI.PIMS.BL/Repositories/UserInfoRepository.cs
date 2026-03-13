using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class UserInfoRepository : DapperPostgresBaseRepository
    {
        private ILoggerService _logger;
        public UserInfoRepository(Helper helper, ILoggerService logger) : base(helper)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<AppRole_T_Dto>> GetRoleUserAssignByMSID(string ms_id)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_ms_id", Value = ms_id ==  null ? DBNull.Value: ms_id, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };
            var data = await QueryCursorAsync<AppRole_T_Dto>("USP_GET_PIMS_APP_ROLE_USER_ASSIGN_BY_MSID_PRC", parameters.ToArray());
            return data;
        }
        public async Task<IEnumerable<ActiveDirectoryUserDto>> GetETL_ADExportUsersByParam(string ms_id, string fname, string lname)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_ms_id", Value = ms_id ==  null ? DBNull.Value: ms_id, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_fname", Value = fname ==  null ? DBNull.Value: fname, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_lname", Value = lname == null ? DBNull.Value : lname, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar}
            };

            var data = await QueryCursorAsync<ActiveDirectoryUserDto>("usp_get_pims_etl_adexportusers_t_byparam_prc", parameters.ToArray());
            return data;
        }


        public async Task<int> AddRoleUserAssign(AppRoleUserAssign_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var obj2 = new AppRoleUserAssignPG_Dto()
                {
                    p_ms_id = obj.p_MS_ID,
                    p_app_roleids = obj.p_APP_ROLEIDS?.TrimEnd(','),
                    p_lst_updt_by = obj.p_LST_UPDT_BY,
                };

                var retVal = await ExecuteAsync("USP_PIMS_APP_ROLE_USER_ASSIGN_INSERT_PRC", obj2);
                notices = AnalyzeNotices(retVal);
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in AddRoleUserAssign for MS ID: {obj.p_MS_ID}, App Role Ids: {obj.p_APP_ROLEIDS}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> DeleteRoleUserAssign(DeleteRoleUserAssignParam_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var obj2 = new AppRoleUserAssignPG_Dto()
                {
                    p_ms_id = obj.p_MS_ID,
                    p_app_roleids = obj.p_APP_ROLEID.TrimEnd(','),
                    p_lst_updt_by = obj.p_LST_UPDT_BY,
                };

                var retVal = await ExecuteAsync("USP_PIMS_APP_ROLE_USER_ASSIGN_DELETE_PRC", obj2);
                notices = AnalyzeNotices(retVal);
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in DeleteRoleUserAssign for DPOC ID: {obj.p_MS_ID}, app_roleids: {obj.p_APP_ROLEID}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> Add(UserInfo_AddDto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            var obj2 = new UserInfo_PG_AddDto()
            {
                p_ms_id = obj.P_MS_ID,
                p_sup_msid = obj.P_SUP_MSID,
                p_sup_name = obj.P_SUP_NAME,
                p_lname = obj.P_LNAME,
                p_fname = obj.P_FNAME,
                p_mi = obj.P_MI,
                p_email = obj.P_EMAIL,
                p_phone = obj.P_PHONE,
                p_fax = obj.P_FAX,
                p_div_code = obj.P_DIV_CODE,
                p_division_name = obj.P_DIVISION_NAME,
                p_department_name = obj.P_DEPARTMENT_NAME,
                p_active = obj.P_ACTIVE,
                p_lst_updt_by = obj.P_LST_UPDT_BY,
                p_manualupdt = obj.P_MANUALUPDT,
                p_autosaveset = obj.P_AUTOSAVESET,
                p_displayname = obj.P_DISPLAYNAME,
                p_app_roleid = obj.P_APP_ROLEID,
                p_pims_user = obj.P_PIMS_USER,
            };

            try
            {
                var retVal = await ExecuteAsync("usp_PIMS_USERINFO_T_INSERT_PRC", obj2);
                notices = AnalyzeNotices(retVal);
            }
            catch (Exception ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save usp_PIMS_USERINFO_T_INSERT_PRC : " + ex.Message);
            }

            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error("Error while saving usp_PIMS_USERINFO_T_INSERT_PRC for MS ID: " + obj.P_MS_ID + ". RaisedErrors: --> " + notices);
            }
            return notices.isFailure;
        }

        public async Task<int> Delete(DeleteUserInfoTParam_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var parameters = new List<NpgsqlParameter>
                {
                    new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID ==  null ? DBNull.Value: obj.p_MS_ID, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar }
                };
                var retVal = await ExecuteAsync("USP_PIMS_USERINFO_T_DELETE_PRC", obj);
                notices = AnalyzeNotices(retVal);
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in UserInfo/Delete for MS ID: {obj.p_MS_ID}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> UpdateUserInfoAppRole(UserInfo_T_Dto obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_MS_ID", obj.MS_ID);
            parameters.Add("p_APP_ROLEID", obj.APP_ROLEID);
            parameters.Add("p_LST_UPDT_BY", obj.LST_UPDT_BY);
            var retVal = await ExecuteAsync("usp_PIMS_USERINFO_T_UPDATE_APP_ROLEID_PRC", parameters, 60);

            return retVal;
        }

        public async Task<int> TogglePIMSUserStatus(TogglePIMSUserStatusParam_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("usp_PIMS_USERINFO_T_TOGGLE_PIMS_USER_STATUS_PRC", obj);
                notices = AnalyzeNotices(retVal);
            }
            catch (PostgresException ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save TogglePIMSUserStatus package: " + ex.Message);
            }
            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error("Error while saving TogglePIMSUserStatus for MS ID: " + obj.p_MS_ID + ". RaisedErrors: --> " + notices);
            }
            return notices.isFailure;
        }
        public async Task<UserInfo_T_Dto> Get(string MS_ID)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_MS_ID", Value = MS_ID, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<UserInfo_T_Dto>("usp_Get_PIMS_USERINFO_T_BY_MSID_PRC", parameters.ToArray(), "result_cursor", 60);

            return data;
        }

        public async Task<IEnumerable<UserInfo_T_Dto>> GetUsers(string ms_id
                                                                , string fname
                                                                , string lname
                                                                , string approleid
                                                                , string active
                                                                , string pims_user)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_MS_ID", Value = ms_id ==  null ? DBNull.Value: ms_id, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_FNAME", Value = fname ==  null ? DBNull.Value: fname, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_LNAME", Value = lname == null ? DBNull.Value : lname, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_APP_ROLEID", Value = approleid == null ? DBNull.Value : approleid, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Integer},
                new() { ParameterName = "p_ACTIVE", Value = active == null ? DBNull.Value : active, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "p_PIMS_USER", Value = pims_user == null ? DBNull.Value : pims_user, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar}
            };

            var data = await QueryCursorAsync<UserInfo_T_Dto>("usp_Get_PIMS_USERINFO_T_BYPARM_PRC", parameters.ToArray());
            return data;
        }



    }
}
