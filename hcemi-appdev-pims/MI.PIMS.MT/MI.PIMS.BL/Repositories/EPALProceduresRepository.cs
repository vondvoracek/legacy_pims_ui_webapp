using Dapper;
using Dapper.Oracle;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class EPALProceduresRepository : DapperPostgresBaseRepository
    {
        private ILoggerService _logger;
        public EPALProceduresRepository(Helper helper, ILoggerService logger) : base(helper)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresSearch(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY == null ? DBNull.Value : obj.p_EPAL_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },     // 1.
                new() { ParameterName = "p_EPAL_PRODUCT_CD", Value = obj.p_EPAL_PRODUCT_CD == null ? DBNull.Value : obj.p_EPAL_PRODUCT_CD, NpgsqlDbType = NpgsqlDbType.Char },              // 2. Multi-Select
                new() { ParameterName = "p_PROC_CD", Value = obj.p_PROC_CD == null ? DBNull.Value : obj.p_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },                                      // 3. Multi-Select
                new() { ParameterName = "p_EPAL_PLAN_CD", Value = obj.p_EPAL_PLAN_CD == null ? DBNull.Value : obj.p_EPAL_PLAN_CD, NpgsqlDbType = NpgsqlDbType.Char },                       // 4. Multi-Select
                new() { ParameterName = "p_EPAL_BUS_SEG_CD", Value = obj.p_EPAL_BUS_SEG_CD  == null ? DBNull.Value : obj.p_EPAL_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char },             // 5. Multi-Select
                new() { ParameterName = "p_EPAL_FUND_ARNGMNT_CD", Value = obj.p_EPAL_FUND_ARNGMNT_CD  == null ? DBNull.Value : obj.p_EPAL_FUND_ARNGMNT_CD  , NpgsqlDbType = NpgsqlDbType.Char},        // 6. Multi-Select
                new() { ParameterName = "p_EPAL_ENTITY_CD", Value = obj.p_EPAL_ENTITY_CD == null ? DBNull.Value : obj.p_EPAL_ENTITY_CD , NpgsqlDbType = NpgsqlDbType.Char},                 // 7. Multi-Select
                new() { ParameterName = "p_DRUG_NM", Value = obj.p_DRUG_NM  == null ? DBNull.Value : obj.p_DRUG_NM, NpgsqlDbType = NpgsqlDbType.Char},                                      // 8.
                new() { ParameterName = "p_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT , NpgsqlDbType = NpgsqlDbType.Timestamp},         // 11.
                new() { ParameterName = "p_EPAL_STATUS", Value = obj.p_EPAL_STATUS == null ? DBNull.Value : obj.p_EPAL_STATUS, NpgsqlDbType = NpgsqlDbType.Char},                           // 12.                
                new() { ParameterName = "p_OVERALL_EFF_DT_From", Value = obj.p_OVERALL_EFF_DT_From  == null ? DBNull.Value : obj.p_OVERALL_EFF_DT_From, NpgsqlDbType = NpgsqlDbType.Date},  // 9.
                new() { ParameterName = "p_OVERALL_EFF_DT_To", Value = obj.p_OVERALL_EFF_DT_To == null ? DBNull.Value : obj.p_OVERALL_EFF_DT_To, NpgsqlDbType = NpgsqlDbType.Date},         // 
                new() { ParameterName = "p_OVERALL_EXP_DT_From", Value = obj.p_OVERALL_EXP_DT_From == null ? DBNull.Value : obj.p_OVERALL_EXP_DT_From, NpgsqlDbType = NpgsqlDbType.Date},   // 10.
                new() { ParameterName = "p_OVERALL_EXP_DT_To", Value = obj.p_OVERALL_EXP_DT_To == null ? DBNull.Value : obj.p_OVERALL_EXP_DT_To, NpgsqlDbType = NpgsqlDbType.Date},         // 
                new() { ParameterName = "p_INCLUDE_HISTORICAL", Value = obj.p_INCLUDE_HISTORICAL, NpgsqlDbType = NpgsqlDbType.Integer},                                                     //                 
                new() { ParameterName = "p_STNDRD_SVC_CAT", Value = obj.p_STNDRD_SVC_CAT  == null ? DBNull.Value : obj.p_STNDRD_SVC_CAT, NpgsqlDbType = NpgsqlDbType.Char},                 // 13. Multi-Select
                new() { ParameterName = "p_STNDRD_SVC_SUBCAT", Value = obj.p_STNDRD_SVC_SUBCAT == null ? DBNull.Value : obj.p_STNDRD_SVC_SUBCAT, NpgsqlDbType = NpgsqlDbType.Char},         // 14. Multi-Select
                new() { ParameterName = "p_ALTRNT_SVC_CAT", Value = obj.p_ALTRNT_SVC_CAT == null ? DBNull.Value : obj.p_ALTRNT_SVC_CAT, NpgsqlDbType = NpgsqlDbType.Char},                  // 15. Multi-Select
                new() { ParameterName = "p_ALTRNT_SVC_SUBCAT", Value = obj.p_ALTRNT_SVC_SUBCAT == null ? DBNull.Value : obj.p_ALTRNT_SVC_SUBCAT , NpgsqlDbType = NpgsqlDbType.Char},        // 16. Multi-Select
                new() { ParameterName = "p_SUSP_IND", Value = obj.p_SUSP_IND == null ? DBNull.Value : obj.p_SUSP_IND , NpgsqlDbType = NpgsqlDbType.Char},                                   // 17. Dropdown
                new() { ParameterName = "p_SUSP_TYPE", Value = obj.p_SUSP_TYPE == null ? DBNull.Value : obj.p_SUSP_TYPE , NpgsqlDbType = NpgsqlDbType.Char},                                // 18. Dropdown
                new() { ParameterName = "p_SUSP_EFF_DT", Value = obj.p_SUSP_EFF_DT == null ? DBNull.Value : obj.p_SUSP_EFF_DT , NpgsqlDbType = NpgsqlDbType.Date}                           // 19. Datepicker
            };

            var data = await QueryFuncAsync<EPAL_Procedures_V_Dto>("usp_get_pims_app_epal_procedures_v_byparam_fun", parameters.ToArray(), 120);
            foreach (var item in data)
            {
                // Convert 12/31/2999, 12/31/1999 to null
                item.PROC_CD_EXP_DT = Helper.CheckExpYear(item.PROC_CD_EXP_DT);
                item.EPAL_VER_EXP_DT = Helper.CheckExpYear(item.EPAL_VER_EXP_DT);
                item.OVERALL_EXP_DT = Helper.CheckExpYear(item.OVERALL_EXP_DT);
                item.PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(item.PRIOR_AUTH_EXP_DT);
                item.AUTO_APRVL_EXP_DT = Helper.CheckExpYear(item.AUTO_APRVL_EXP_DT);
                item.MCARE_SPCL_PRCSNG_EXP_DT = Helper.CheckExpYear(item.MCARE_SPCL_PRCSNG_EXP_DT);
            }
            return data;
        }

        public async Task<EPAL_Procedures_V_Dto> CheckIsDonorRecord(string p_EPAL_HIERARCHY_KEY)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = p_EPAL_HIERARCHY_KEY == null ? DBNull.Value : p_EPAL_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };
            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Procedures_V_Dto>("usp_checkisdonorrecord_prc", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_V_Dto>> GetEPALProceduresHist(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_V_Dto>("usp_Get_PIMS_APP_EPAL_PROCEDURES_V_HIST_PRC", parameters.ToArray(), "result_cursor", 60);

            foreach (var item in data)
            {
                // Convert 12/31/2999, 12/31/1999 to null
                item.PROC_CD_EXP_DT = Helper.CheckExpYear(item.PROC_CD_EXP_DT);
                item.EPAL_VER_EXP_DT = Helper.CheckExpYear(item.EPAL_VER_EXP_DT);
                item.OVERALL_EXP_DT = Helper.CheckExpYear(item.OVERALL_EXP_DT);
                item.PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(item.PRIOR_AUTH_EXP_DT);
                item.AUTO_APRVL_EXP_DT = Helper.CheckExpYear(item.AUTO_APRVL_EXP_DT);
                item.MCARE_SPCL_PRCSNG_EXP_DT = Helper.CheckExpYear(item.MCARE_SPCL_PRCSNG_EXP_DT);
            }
            return data;
        }

        public async Task<EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto> GetEPALProcedureTMaxPriorAuthDt(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto ePAL_Procedures_T_Max_Prior_Auth_Dt_Dto = new EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto();
            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto>("USP_GET_PIMS_APP_EPAL_PROCEDURES_T_MAX_PRIOR_AUTH_DT_PRC", parameters.ToArray(), "result_cursor", 60);

            if (data != null)
            {
                data.MAX_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.MAX_PRIOR_AUTH_EXP_DT); // Convert 12/31/2999, 12/31/1999 to null            
                ePAL_Procedures_T_Max_Prior_Auth_Dt_Dto = data;
            }

            return ePAL_Procedures_T_Max_Prior_Auth_Dt_Dto;
        }

        public async Task<EPAL_Procedures_V_Dto> GetEPALProcedureByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "p_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_MS_ID", Value = obj.p_MS_ID, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Procedures_V_Dto>("usp_Get_PIMS_APP_EPAL_PROCEDURES_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            if (data == null) return new EPAL_Procedures_V_Dto();

            // Convert 12/31/2999, 12/31/1999 to null
            data.PROC_CD_EXP_DT = Helper.CheckExpYear(data.PROC_CD_EXP_DT);
            data.EPAL_VER_EXP_DT = Helper.CheckExpYear(data.EPAL_VER_EXP_DT);
            data.OVERALL_EXP_DT = Helper.CheckExpYear(data.OVERALL_EXP_DT);
            data.PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.PRIOR_AUTH_EXP_DT);
            data.AUTO_APRVL_EXP_DT = Helper.CheckExpYear(data.AUTO_APRVL_EXP_DT);
            data.MCARE_SPCL_PRCSNG_EXP_DT = Helper.CheckExpYear(data.MCARE_SPCL_PRCSNG_EXP_DT);

            data.PRE_DET_EXP_DT = Helper.CheckExpYear(data.PRE_DET_EXP_DT);
            data.ADV_NTFCTN_EXP_DT = Helper.CheckExpYear(data.ADV_NTFCTN_EXP_DT);
            data.DRAL_EXP_DT = Helper.CheckExpYear(data.DRAL_EXP_DT);

            // User Story 131025 MFQ 4/15/2025
            data.SUSP_EFF_DT = Helper.CheckExpYear(data.SUSP_EFF_DT);
            data.SUSP_EXP_DT = Helper.CheckExpYear(data.SUSP_EXP_DT);

            return data;
        }

        public async Task<EPAL_Procedures_V_Dto> GetEPALProcedureCurrVerByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_MS_ID", Value = obj.p_MS_ID, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Procedures_V_Dto>("usp_Get_PIMS_APP_EPAL_PROCEDURES_CURR_VER_V_BY_PIMS_ID_PRC", parameters.ToArray());

            // Convert 12/31/2999, 12/31/1999 to null
            data.PROC_CD_EXP_DT = Helper.CheckExpYear(data.PROC_CD_EXP_DT);
            data.EPAL_VER_EXP_DT = Helper.CheckExpYear(data.EPAL_VER_EXP_DT);
            data.OVERALL_EXP_DT = Helper.CheckExpYear(data.OVERALL_EXP_DT);
            data.PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.PRIOR_AUTH_EXP_DT);
            data.AUTO_APRVL_EXP_DT = Helper.CheckExpYear(data.AUTO_APRVL_EXP_DT);
            data.MCARE_SPCL_PRCSNG_EXP_DT = Helper.CheckExpYear(data.MCARE_SPCL_PRCSNG_EXP_DT);

            data.PRE_DET_EXP_DT = Helper.CheckExpYear(data.PRE_DET_EXP_DT);
            data.ADV_NTFCTN_EXP_DT = Helper.CheckExpYear(data.ADV_NTFCTN_EXP_DT);
            data.DRAL_EXP_DT = Helper.CheckExpYear(data.DRAL_EXP_DT);

            // User Story 131025 MFQ 4/15/2025
            data.SUSP_EFF_DT = Helper.CheckExpYear(data.SUSP_EFF_DT);
            data.SUSP_EXP_DT = Helper.CheckExpYear(data.SUSP_EXP_DT);

            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_DiagCodes_Dto>> GetEPALProcedureDGCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value :obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_DiagCodes_Dto>("USP_PIMS_GET_APP_ASSOC_CODES_DG_CODES_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_RevCodes_Dto>> GetEPALProcedureRevCodesByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_RevCodes_Dto>("USP_GET_PIMS_APP_ASSOC_CODES_REV_CODES_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_AllocatedPlaces_Dto>> GetEPALProcedureAllowedPlaceByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_AllocatedPlaces_Dto>("USP_PIMS_GET_APP_ASSOC_CODES_ALLOWED_PLACE_OF_SRVC_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto>> GetEPALProcedureChangeHistoryByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            if (obj.p_EPAL_HIERARCHY_KEY == null || obj.p_EPAL_HIERARCHY_KEY == "") return new List<EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto>();

            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto>("USP_GET_PIMS_APP_PROC_CHANGE_HISTORY_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>> GetEPALProcedureAPPlTOSTATESByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto> data = null;

            if (obj.p_EPAL_VER_EFF_DT != null)
            {
                data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>("USP_GET_PIMS_APP_APPL_TO_STATES_PRC", parameters.ToArray());
            }
            else
            {
                data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>("USP_GET_PIMS_APP_APPL_TO_STATES_CURR_V_PRC", parameters.ToArray());
            }

            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_PROCS_MODIFIERS_Dto>> GetEPALProcedurePROCS_MODIFIERSByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_AssoCodes_V_PROCS_MODIFIERS_Dto>("USP_GET_PIMS_APP_EPAL_PROCS_MODIFIERS_PRC", parameters.ToArray());
            return data;
        }

        public async Task<EPAL_Proc_Status_Dto> GetEPALProcStatus(EPAL_Procedures_Codes_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "I_PROC_CD", Value = obj.p_PROC_CD, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            // Get Proce code as Active or Inactive
            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Proc_Status_Dto>("USP_GET_CHECK_PROC_CD_PRC", parameters.ToArray());
            return data;
        }
        public async Task<IEnumerable<EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto>> GetPIMSHierarchyCodesXwalkByEPALBusSegCD(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "P_EPAL_BUS_SEG_CD", Value = string.IsNullOrEmpty(obj.EPAL_BUS_SEG_CD) ? DBNull.Value: obj.EPAL_BUS_SEG_CD, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "P_COLUMN_NAME", Value = obj.COLUMN_NAME.ToStringNullSafe(), Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "P_ACTIVE", Value = string.IsNullOrEmpty(obj.ACTIVE) ? DBNull.Value : obj.ACTIVE, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar}
            };

            var data = await QueryCursorAsync<EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto>("USP_GET_PIMS_HIERARCHY_CODES_XWALK_V_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto>> GetAllPIMSHierarchyCodesXwalk()
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };
            var data = await QueryCursorAsync<EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto>("USP_GET_ALL_PIMS_APP_PIMS_HIERARCHY_CODES_XWALK_V", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<EPAL_Procedures_AssoCodes_Status_Dto> GetPIMS_IDExistStatus(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            // Get EPAL_HIERARCHY_KEY as Exists or NotExists
            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Procedures_AssoCodes_Status_Dto>("USP_PIMS_GET_APP_ASSOC_CODES_STATUS_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Additional_Info_History_Dto>> GetPIMSAdditionalInfoHistory(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Additional_Info_History_Dto>("USP_GET_EPAL_ADDITION_INFO_HIS_BY_PIMS_ID_PRC", parameters.ToArray());
            return data;
        }

        public async Task<int> EPAL_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync<EPAL_Ins_Upd_Pkg_Param>("USP_EPAL_INS_UPD_DRIVER_PRC", obj, new string[] { "P_EPAL_HIERARCHY_KEY", "P_EPAL_VER_EFF_DT" });
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving EPAL_INS_UPD_DRIVER for PIMS ID: " + obj.P_EPAL_HIERARCHY_KEY + " and EPAL Version Dt: " + obj.P_EPAL_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                }
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in EPAL_INS_UPD_DRIVER for EPAL ID: {obj.P_EPAL_HIERARCHY_KEY}, Version Dt: {obj.P_EPAL_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> EPAL_HISTORIC_INS_UPD_DRIVER(EPAL_Ins_Upd_Pkg_Param obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync<EPAL_Ins_Upd_Pkg_Param>("USP_EPAL_HISTORIC_INS_UPD_DRIVER_PRC", obj, new string[] { "P_EPAL_HIERARCHY_KEY" });
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving EPAL_HISTORIC_INS_UPD_DRIVER for PIMS ID: " + obj.P_EPAL_HIERARCHY_KEY + " and EPAL Version Dt: " + obj.P_EPAL_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                }
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in EPAL_HISTORIC_INS_UPD_DRIVER for EPAL ID: {obj.P_EPAL_HIERARCHY_KEY}, Version Dt: {obj.P_EPAL_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> EPAL_DELETE_DRIVER_PRC(EPAL_Ins_Upd_Pkg_Param obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                EPAL_Del_Pkg_Param param = new()
                {
                    p_EPAL_HIERARCHY_KEY = obj.P_EPAL_HIERARCHY_KEY,
                    p_EPAL_VER_EFF_DT = obj.P_EPAL_VER_EFF_DT,
                    p_USER_ID = obj.P_USER_ID,
                    p_CHANGE_DESC = obj.P_CHANGE_DESC,
                    p_CHANGE_REQ_ID = obj.P_CHANGE_REQ_ID
                };

                var retVal = await ExecuteAsync<EPAL_Del_Pkg_Param>("USP_EPAL_DELETE_DRIVER_PRC", param, null);
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving EPAL_DELETE_DRIVER_PRC for PIMS ID: " + obj.P_EPAL_HIERARCHY_KEY + " and EPAL Version Dt: " + obj.P_EPAL_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                }
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in EPAL_DELETE_DRIVER_PRC for EPAL ID: {obj.P_EPAL_HIERARCHY_KEY}, Version Dt: {obj.P_EPAL_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<IEnumerable<EPAL_Procedures_Modifier_Dto>> GetEPALProcedureModifiersByPIMS_ID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = (obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT), Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_Modifier_Dto>("USP_GET_PIMS_APP_ASSOC_CODES_MODIFIERS_V_PRC", parameters.ToArray());
            return data;
        }

        public async Task<EPAL_Procs_Sos_T_Dto> GetPIMSProcsSOS(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_epal_hierarchy_key", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_EPAL_VER_EFF_DT", Value = (obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT), Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            EPAL_Procs_Sos_T_Dto data = new EPAL_Procs_Sos_T_Dto();

            if (obj.p_EPAL_VER_EFF_DT != null)
            {
                data = await QueryFirstOrDefaultCursorAsync<EPAL_Procs_Sos_T_Dto>("usp_Get_EPAL_PROCS_SOS_V_BY_HIERARCHY_KEY_PRC", parameters.ToArray());
            }
            else
            {
                data = await QueryFirstOrDefaultCursorAsync<EPAL_Procs_Sos_T_Dto>("usp_Get_EPAL_PROCS_SOS_V_BY_HIERARCHY_KEY_CURR_V_PRC", parameters.ToArray());
            }

            // Convert 12/31/2999, 12/31/1999 to null
            data.SOS_EXP_DT = Helper.CheckExpYear(data.SOS_EXP_DT);

            return data;
        }

        public async Task<EPAL_PIMSHierarchyCodeCombinationExists_Dto> CheckPIMSHierarchyCodeCombinationExists(EPAL_PIMSHierarchyCodeCombinationExists_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_BUS_SEG_CD", Value = obj.EPAL_BUS_SEG_CD, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_EPAL_ENTITY_CD", Value = obj.EPAL_ENTITY_CD, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_EPAL_PLAN_CD", Value = obj.EPAL_PLAN_CD, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Char},
                new() { ParameterName = "p_EPAL_PRODUCT_CD", Value = obj.EPAL_PRODUCT_CD, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Char},
                new() { ParameterName = "p_EPAL_FUND_ARNGMNT_CD", Value = obj.EPAL_FUND_ARNGMNT_CD, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Char},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<EPAL_PIMSHierarchyCodeCombinationExists_Dto>("USP_GET_PIMS_APP_PIMS_HIERARCHY_CODES_COMBINATION_EXISTS", parameters.ToArray());

            if (data == null) data = new EPAL_PIMSHierarchyCodeCombinationExists_Dto();

            return data;
        }

        public async Task<EPAL_Catagories_Dto> GetPIMSEPALCategories(EPAL_Catagories_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "I_EPAL_HIERARCHY_KEY", Value = obj.I_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "O_ALTERNATE_CATEGORY", Value = obj.O_ALTERNATE_CATEGORY, Direction = ParameterDirection.Output },
                //new() { ParameterName = "O_ALTERNATE_SUB_CATEGORY", Value = obj.O_ALTERNATE_SUB_CATEGORY, Direction = ParameterDirection.Output },
                //new() { ParameterName = "O_STANDARD_CATEGORY", Value = obj.O_STANDARD_CATEGORY, Direction = ParameterDirection.Output },
                //new() { ParameterName = "O_STANDARD_SUB_CATEGORY", Value = obj.O_STANDARD_SUB_CATEGORY, Direction = ParameterDirection.Output }
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<EPAL_Catagories_Dto>("USP_GET_PIMS_APP_EPAL_CATEGORIES_PRC", parameters.ToArray());

            EPAL_Catagories_Dto ePAL_Catagories_Dto = new EPAL_Catagories_Dto();
            ePAL_Catagories_Dto.I_EPAL_HIERARCHY_KEY = obj.I_EPAL_HIERARCHY_KEY;
            ePAL_Catagories_Dto.O_ALTERNATE_CATEGORY = data.O_ALTERNATE_CATEGORY;// parameter.Get<string>("O_ALTERNATE_CATEGORY");
            ePAL_Catagories_Dto.O_ALTERNATE_SUB_CATEGORY = data.O_ALTERNATE_SUB_CATEGORY;// parameter.Get<string>("O_ALTERNATE_SUB_CATEGORY");
            ePAL_Catagories_Dto.O_STANDARD_CATEGORY = data.O_STANDARD_CATEGORY; // parameter.Get<string>("O_STANDARD_CATEGORY");
            ePAL_Catagories_Dto.O_STANDARD_SUB_CATEGORY = data.O_STANDARD_SUB_CATEGORY; // parameter.Get<string>("O_STANDARD_SUB_CATEGORY");

            return ePAL_Catagories_Dto;
        }

        public async Task<IEnumerable<EPAL_Procs_Prog_Mgd_By_V>> GetProgMgdByPIMSID(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procs_Prog_Mgd_By_V>("USP_GET_PIMS_PROCS_PROG_MGD_BY_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByType(EPAL_Catagory_By_Type_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "P_TEXT", Value = string.IsNullOrEmpty(obj.P_TEXT) ? DBNull.Value: obj.P_TEXT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "P_CATEGORY_TYPE", Value = obj.P_CATEGORY_TYPE.ToStringNullSafe(), Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "P_PARENT_CATEGORY", Value = string.IsNullOrEmpty(obj.P_PARENT_CATEGORY) ? DBNull.Value : obj.P_PARENT_CATEGORY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Catagory_By_Type_Dto>("USP_GET_PIMS_APP_EPAL_CATEGORIES_BY_TYPE_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<EPAL_Catagory_By_Type_Dto>> GetPIMSEPALCategoriesByProcCDDrugNM(EPAL_Catagory_By_ProcCDDrugNM_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "P_PROC_CD", Value = string.IsNullOrEmpty(obj.P_PROC_CD) ? DBNull.Value: obj.P_PROC_CD, Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "P_DRUG_NM", Value = obj.P_DRUG_NM.ToStringNullSafe(), Direction = ParameterDirection.Input , NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "P_ALTERNATE_CATEGORY", Value = string.IsNullOrEmpty(obj.P_ALTERNATE_CATEGORY) ? DBNull.Value : obj.P_ALTERNATE_CATEGORY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Catagory_By_Type_Dto>("USP_GET_PIMS_APP_EPAL_ALT_CATEGORIES_BY_PROC_CD_DRUG_NM_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procedures_T_Max_PA_PRE_DT>> GetPIMSEPALMaxPAPREEXPDT(string p_EPAL_HIERARCHY_KEY)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procedures_T_Max_PA_PRE_DT>("USP_GET_PIMS_EPAL_MAX_PA_PRE_EXP_DT_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<Ret_Factors_Dto>> GetRetFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "p_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_FACTOR_TYPE", Value = obj.p_FACTOR_TYPE, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<Ret_Factors_Dto>("USP_GET_PIMS_APP_EPAL_RET_RED_FACTORS_BY_PIMS_ID_PRC", parameters.ToArray());
            return data;
        }
        public async Task<IEnumerable<Red_Factors_Dto>> GetRedFactorsByPIMSID(EPAL_Red_Ret_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY", Value = obj.p_EPAL_HIERARCHY_KEY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "p_EPAL_VER_EFF_DT", Value = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_FACTOR_TYPE", Value = obj.p_FACTOR_TYPE, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Varchar },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<Red_Factors_Dto>("USP_GET_PIMS_APP_EPAL_RET_RED_FACTORS_BY_PIMS_ID_PRC", parameters.ToArray());
            return data;
        }
    }
}
