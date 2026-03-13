using Dapper;
using Dapper.Oracle;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class PayCodeProceduresRepository : DapperPostgresBaseRepository
    {
        private ILoggerService _logger;
        private readonly AppDbContext _context;
        public PayCodeProceduresRepository(Helper helper, ILoggerService logger, AppDbContext context) : base(helper)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeProceduresSearch(PayCode_Procedures_Param_Dto obj)
        {

            ///////////////////////////////////////////////////////////////////////////
            //  DY: 12/05/2024 - Commented out formatMultiSelectValues loop,
            //  no need in UI, Postgres DB will handle.
            ///////////////////////////////////////////////////////////////////////////

            //var parameter = new DynamicParameters();

            //if (obj.p_PAYC_PRODUCT_CD != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_PRODUCT_CD = Helper.formatMultiSelectValues(obj.p_PAYC_PRODUCT_CD);
            //} 

            //if (obj.p_PAYC_BUS_SEG_CD != null)  // < -- Multi-Select value
            //{
            //    obj.p_PAYC_BUS_SEG_CD = Helper.formatMultiSelectValues(obj.p_PAYC_BUS_SEG_CD);
            //} 

            //if (obj.p_PAYC_ENTITY_CD != null)  // < -- Multi-Select value
            //{
            //    obj.p_PAYC_ENTITY_CD = Helper.formatMultiSelectValues(obj.p_PAYC_ENTITY_CD);
            //} 

            //if (obj.p_iCES != null) // < -- Multi-Select value
            //{
            //    obj.p_iCES = Helper.formatMultiSelectValues(obj.p_iCES);
            //} 

            //if (obj.p_PAYC_KL_PCS != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_KL_PCS = Helper.formatMultiSelectValues(obj.p_PAYC_KL_PCS);
            //} 

            //if (obj.p_PAYC_NDB_PCS != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_NDB_PCS = Helper.formatMultiSelectValues(obj.p_PAYC_NDB_PCS);
            //}

            //if (obj.p_PAYC_PLAN_CD != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_PLAN_CD = Helper.formatMultiSelectValues(obj.p_PAYC_PLAN_CD);
            //}

            //if (obj.p_PROC_CD != null) // < -- Multi-Select value
            //{
            //    obj.p_PROC_CD = Helper.formatMultiSelectValues(obj.p_PROC_CD);
            //}

            //if (obj.p_PAYC_ICES_EDIT_NAME != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_ICES_EDIT_NAME = Helper.formatMultiSelectValues(obj.p_PAYC_ICES_EDIT_NAME);
            //}

            //if (obj.p_PAYC_ICES_EDIT_ACTION != null) // < -- Multi-Select value
            //{
            //    obj.p_PAYC_ICES_EDIT_ACTION = Helper.formatMultiSelectValues(obj.p_PAYC_ICES_EDIT_ACTION);
            //}

            var parameters = new List<NpgsqlParameter>
            {
                    new() { ParameterName = "p_include_historical", Value = obj.p_INCLUDE_HISTORICAL, NpgsqlDbType = NpgsqlDbType.Integer },
                    //new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                    new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char},
                    new() { ParameterName = "p_payc_product_cd", Value = obj.p_PAYC_PRODUCT_CD == null ? DBNull.Value : obj.p_PAYC_PRODUCT_CD, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_plan_cd", Value = obj.p_PAYC_PLAN_CD == null ? DBNull.Value : obj.p_PAYC_PLAN_CD, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_bus_seg_cd", Value = obj.p_PAYC_BUS_SEG_CD == null ? DBNull.Value : obj.p_PAYC_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_entity_cd", Value = obj.p_PAYC_ENTITY_CD == null ? DBNull.Value : obj.p_PAYC_ENTITY_CD, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_current_eff_dt", Value = obj.p_CURRENT_EFF_DT == null ? DBNull.Value : obj.p_CURRENT_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                    new() { ParameterName = "p_current_exp_dt", Value = obj.p_CURRENT_EXP_DT == null ? DBNull.Value : obj.p_CURRENT_EXP_DT, NpgsqlDbType = NpgsqlDbType.Date },
                    new() { ParameterName = "p_payc_ver_eff_dt", Value = obj.p_PAYC_VER_EFF_DT == null ? DBNull.Value : obj.p_PAYC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                    new() { ParameterName = "p_proc_cd", Value = obj.p_PROC_CD == null ? DBNull.Value : obj.p_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_status", Value = obj.p_PAYC_STATUS == null ? DBNull.Value : obj.p_PAYC_STATUS, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_altrnt_svc_cat", Value = obj.p_ALTRNT_SVC_CAT == null ? DBNull.Value : obj.p_ALTRNT_SVC_CAT, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_altrnt_svc_subcat", Value = obj.p_ALTRNT_SVC_SUBCAT == null ? DBNull.Value : obj.p_ALTRNT_SVC_SUBCAT, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_ices", Value = obj.p_iCES == null ? DBNull.Value : obj.p_iCES , NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_prior_auth_status", Value = obj.p_PRIOR_AUTH_STATUS == null ? DBNull.Value : obj.p_PRIOR_AUTH_STATUS, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_kl_pcs", Value = obj.p_PAYC_KL_PCS == null ? DBNull.Value : obj.p_PAYC_KL_PCS, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_ndb_pcs", Value = obj.p_PAYC_NDB_PCS == null ? DBNull.Value : obj.p_PAYC_NDB_PCS, NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_payc_ices_edit_name", Value = obj.p_PAYC_ICES_EDIT_NAME == null ? DBNull.Value : obj.p_PAYC_ICES_EDIT_NAME, NpgsqlDbType = NpgsqlDbType.Char},
                    new() { ParameterName = "p_payc_ices_edit_action", Value = obj.p_PAYC_ICES_EDIT_ACTION == null ? DBNull.Value : obj.p_PAYC_ICES_EDIT_ACTION, NpgsqlDbType = NpgsqlDbType.Char },
            };

            //var data = await QueryCursorAsync<PayCode_Procedures_V_Dto>("usp_Get_PIMS_APP_PAY_CODE_PROCEDURES_V_ByParam_prc", parameters.ToArray(), "result_cursor", 600);
            var data = await QueryFuncAsync<PayCode_Procedures_V_Dto>("usp_get_pims_app_pay_code_procedures_v_byparam_fun", parameters.ToArray(), 600);
            return data;
        }


        public async Task<IEnumerable<PayCodeFiltersDto>> GetPayCodeSearchFilters(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                    new() { ParameterName = "p_column_name", Value = obj.COLUMN_NAME.ToLower(), NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                    new() { ParameterName = "p_active", Value = obj.ACTIVE == null ? DBNull.Value : obj.ACTIVE , NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_epal_entity_cd", Value = obj.EPAL_ENTITY_CD  == null ? DBNull.Value : obj.EPAL_ENTITY_CD , NpgsqlDbType = NpgsqlDbType.Char },
                    new() { ParameterName = "p_epal_bus_seg_cd", Value = obj.EPAL_BUS_SEG_CD == null ? DBNull.Value : obj.EPAL_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char }

            };

            var data = await QueryCursorAsync<PayCodeFiltersDto>("USP_GET_PIMS_PAYC_HIERARCHY_CODES_XWALK_V_PRC", parameters.ToArray(), "result_cursor", 600);

            return data;
        }

        public async Task<PayCode_Procedures_T_Dto> GetPayCodeProcedureByPIMS_ID(PayCode_Procedures_Param_Dto obj)
        {
            var stored_procedure = "";
            if (obj.p_PAYC_VER_EFF_DT != null)
            {
                //stored_procedure = "USP_GET_PIMS_APP_PAY_CODE_PROCEDURES_V_BY_PIMS_ID_PRC";
                stored_procedure = "usp_get_pims_app_pay_code_procedures_v_by_pims_id_fun";
            }
            else if (obj.p_PAYC_VER_EFF_DT == null)
            {
                //stored_procedure = "USP_GET_PIMS_APP_PAY_CODE_PROCEDURES_CURR_VER_V_BY_PIMS_ID_PRC";
                stored_procedure = "usp_get_pims_app_pay_code_procedures_curr_ver_v_by_pims_id_fun";
            }

            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_payc_ver_eff_dt", Value = obj.p_PAYC_VER_EFF_DT  == null ? DBNull.Value : obj.p_PAYC_VER_EFF_DT , NpgsqlDbType = NpgsqlDbType.Timestamp }
            };

            //var data = await QueryFirstOrDefaultCursorAsync<PayCode_Procedures_T_Dto>(stored_procedure, parameters.ToArray(), "result_cursor", 600);
            var data = await QueryFirstOrDefaultFuncAsync<PayCode_Procedures_T_Dto>(stored_procedure, parameters.ToArray());

            if (data == null)
            {
                return data;
            }

            // Convert 12/31/2999, 12/31/1999 to null
            data.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EFF_DT);
            data.EPAL_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EXP_DT);
            data.PAYC_EFF_DT = Helper.CheckExpYear(data.PAYC_EFF_DT);
            data.PAYC_EXP_DT = Helper.CheckExpYear(data.PAYC_EXP_DT);
            //
            // Get MCR_Route
            //
            var parameter2 = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_payc_ver_eff_dt", Value = data.PAYC_VER_EFF_DT  == null ? DBNull.Value : data.PAYC_VER_EFF_DT , NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID, NpgsqlDbType = NpgsqlDbType.Char},
                //new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            //var data2 = await QueryCursorAsync<PayCode_MCR_Routed_Dto>("usp_Get_PIMS_APP_PAY_CODE_MCR_ROUTED_BY_PIMS_ID_PRC", parameter2.ToArray(), "result_cursor", 600);
            var data2 = await QueryFuncAsync<PayCode_MCR_Routed_Dto>("usp_get_pims_app_pay_code_mcr_routed_by_pims_id_fun", parameter2.ToArray());
            // 
            StringBuilder s = new StringBuilder();
            if (data2 != null && data2.Count() > 0)
            {
                int i = data2.Count();
                foreach (var m in data2)
                {
                    if (m.MCR_ROUTED != null)
                    {
                        s.Append(m.MCR_ROUTED);
                        i--;
                    }
                    /*Fix to remove below check, it was not setting anything to the UI*/
                    /*if (i == 0)
                    {
                        if (data.PAYC_MCR_ROUTED!=null) {
                            data.PAYC_MCR_ROUTED = s.ToString();
                            break;
                        }               
                    }
                    else
                    {
                        s.Append(',');
                    }*/
                }

                if (s.Length > 0)
                {
                    data.PAYC_MCR_ROUTED = s.ToString();
                }
            }

            //Check if IsCurrent record if a effective date is passed.
            if (obj.p_PAYC_VER_EFF_DT != null)
            {
                IsCurrentRecordDto obj2 = new IsCurrentRecordDto();
                {
                    obj2.p_PIMS_ID = obj.p_PAYC_HIERARCHY_KEY;
                    obj2.p_PIMS_VER_EFF_DT = obj.p_PAYC_VER_EFF_DT;
                    obj2.p_MODULE_NAME = "PAYCODES";
                };
                var data3 = await GetPIMSIsCurrentRecord(obj2);

                if (data3 != null)
                {
                    data.IS_CURRENT = data3.IS_CURRENT;
                }
            }
            else if (obj.p_PAYC_VER_EFF_DT == null)
            {
                data.IS_CURRENT = "Y";
            }

            return data;
        }

        public async Task<IsCurrentRecordDto> GetPIMSIsCurrentRecord(IsCurrentRecordDto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_pims_ver_eff_dt", Value = obj.p_PIMS_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_module_name", Value = obj.p_MODULE_NAME.ToLower(), NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_pims_id", Value = obj.p_PIMS_ID  == null ? DBNull.Value : obj.p_PIMS_ID , NpgsqlDbType = NpgsqlDbType.Char },
            };

            var data = await QueryFirstOrDefaultCursorAsync<IsCurrentRecordDto>("USP_GET_PIMS_IS_CURRENT_RECORD_PRC", parameters.ToArray(), "result_cursor", 600);

            return data;
        }

        public async Task<IEnumerable<PayCode_EPAL_Summary_Dto>> GetPayCodeEPALSummary(PayCode_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID, NpgsqlDbType = NpgsqlDbType.Char }
                //new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            //var data = await QueryCursorAsync<PayCode_EPAL_Summary_Dto>("usp_Get_PIMS_APP_PAY_CODE_EPAL_SUMMARY_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 600);            
            var data = await QueryFuncAsync<PayCode_EPAL_Summary_Dto>("usp_get_pims_app_pay_code_epal_summary_by_pims_id_fun", parameters.ToArray(), 120);
            return data;
        }

        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodesNotesFurtherConsiderationByPIMS_ID(string PAYC_HIERARCHY_KEY)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_payc_hierarchy_key", Value = PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<PayCode_Procedures_V_Dto>("usp_get_pims_app_paycode_notes_further_considerations_by_pimsid", parameters.ToArray(), "result_cursor", 600);
            return data;
        }


        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeHistorical(PayCode_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID, NpgsqlDbType = NpgsqlDbType.Char }
                //new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            //var data = await QueryCursorAsync<PayCode_Procedures_V_Dto>("usp_Get_PIMS_APP_PAY_CODE_HISTORY_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 600);
            var data = await QueryFuncAsync<PayCode_Procedures_V_Dto>("usp_get_pims_app_pay_code_history_by_pims_id_fun", parameters.ToArray());
            return data;

        }

        public async Task<int> PAYC_HISTORIC_INS_UPD_DRIVER(PayCode_Procedures_T_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var obj2 = new PayCode_Historic_Ins_Upd_Pkg_Param()
                {
                    p_payc_bus_seg_cd = obj.PAYC_BUS_SEG_CD,
                    p_payc_entity_cd = obj.PAYC_ENTITY_CD,
                    p_payc_proc_cd = obj.PAYC_PROC_CD,
                    p_payc_ver_eff_dt = obj.PAYC_VER_EFF_DT,
                    p_payc_plan_cd = obj.PAYC_PLAN_CD,
                    p_payc_product_cd = obj.PAYC_PRODUCT_CD,
                    p_payc_kl_pcs = obj.PAYC_KL_PCS,
                    p_payc_ndb_pcs = obj.PAYC_NDB_PCS,
                    p_payc_ndb_remark_cd = obj.PAYC_NDB_REMARK_CD,
                    p_payc_ices_ind = obj.PAYC_ICES_IND,
                    p_payc_ices_edit_action = obj.PAYC_ICES_EDIT_ACTION,
                    p_payc_advn_notif = obj.PAYC_ADVN_NOTIF,
                    p_payc_pred_eff_dt = obj.PAYC_PRED_EFF_DT,
                    p_payc_pred_exp_dt = obj.PAYC_PRED_EXP_DT,
                    p_payc_mcr_routed = obj.PAYC_MCR_ROUTED,
                    p_payc_bifurcated = obj.PAYC_BIFURCATED,
                    p_payc_ns88_compliance = obj.PAYC_NS88_COMPLIANCE,
                    p_payc_additional_edits = obj.PAYC_ADDITIONAL_EDITS,
                    p_payc_comments = obj.PAYC_COMMENTS,
                    p_payc_ices_edit_name = obj.PAYC_ICES_EDIT_NAME,
                    p_payc_pred_ind = obj.PAYC_PRED_IND,
                    p_payc_eff_dt = obj.PAYC_EFF_DT,
                    p_payc_exp_dt = obj.PAYC_EXP_DT,
                    p_payc_fund_arngmnt_cd = obj.PAYC_FUND_ARNGMNT_CD,
                    p_user_id = obj.LST_UPDT_BY,
                    p_change_req_id = obj.CHANGE_REQ_ID,
                    p_change_desc = obj.CHANGE_DESC,
                    p_payc_further_considerations = obj.PAYC_FURTHER_CONSIDERATIONS,
                    p_payc_notes = obj.PAYC_NOTES,
                };

                var retVal = await ExecuteAsync<PayCode_Historic_Ins_Upd_Pkg_Param>("USP_PAYC_HISTORIC_INS_UPD_DRIVER_PRC", obj2, null);
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving PAYC_HISTORIC_INS_UPD_DRIVER_PRC for PIMS ID: " + obj.PAYC_HIERARCHY_KEY + " and Paycode Version Dt: " + obj.PAYC_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                }
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in PAYC_HISTORIC_INS_UPD_DRIVER_PRC for PIMS ID: {obj.PAYC_HIERARCHY_KEY}, Version Dt: {obj.PAYC_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }


        public async Task<int> PAYC_DELETE_DRIVER_PRC(PayCode_Procedures_T_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var obj2 = new PayCode_Delete_Pkg_Param()
                {
                    p_payc_hierarchy_key = obj.PAYC_HIERARCHY_KEY,
                    p_payc_ver_eff_dt = obj.PAYC_VER_EFF_DT,
                    p_user_id = obj.LST_UPDT_BY,
                    p_change_req_id = obj.CHANGE_REQ_ID,
                    p_change_desc = obj.CHANGE_DESC
                };

                var retVal = await ExecuteAsync<PayCode_Delete_Pkg_Param>("USP_PAYC_DELETE_DRIVER_PRC", obj2, null);
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving PAYC_DELETE_DRIVER_PRC for PIMS ID: " + obj.PAYC_HIERARCHY_KEY + " and PayCode Version Dt: " + obj.PAYC_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                }
                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in PAYC_DELETE_DRIVER_PRC for PIMS ID: {obj.PAYC_HIERARCHY_KEY}, Version Dt: {obj.PAYC_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }

        /// <summary>
        /// Get Max Version date by Hierarchy Key - USER STORY 129781 MFQ 3/14/2025
        /// </summary>
        /// <param name="payc_hierarchy_key"></param>
        /// <returns></returns>
        public async Task<(string, DateTime)> GetMaxDateByKey(string payc_hierarchy_key)
        {
            var parameters = new { p_payc_hierarchy_key = payc_hierarchy_key };

            //Bug 138864 MFQ 9/25/2025

            var data = await _context.payc_procs_curr_ver_eff_dt_v
                .Where(p => p.payc_hierarchy_key == payc_hierarchy_key)
                .OrderByDescending(p => p.payc_curr_ver_eff_dt) // Assuming VerEffDt is the date column
                .Select(p => new ValueTuple<string, DateTime>(p.payc_hierarchy_key, p.payc_curr_ver_eff_dt))
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<(int, DateTime)> UpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var obj2 = new PayCode_Ins_Upd_Pkg_Param()
                {
                    p_payc_hierarchy_key = obj.PAYC_HIERARCHY_KEY,
                    p_payc_bus_seg_cd = obj.PAYC_BUS_SEG_CD,
                    p_payc_entity_cd = obj.PAYC_ENTITY_CD,
                    p_payc_proc_cd = obj.PAYC_PROC_CD,
                    p_payc_plan_cd = obj.PAYC_PLAN_CD,
                    p_payc_product_cd = obj.PAYC_PRODUCT_CD,
                    p_payc_fund_arngmnt_cd = obj.PAYC_FUND_ARNGMNT_CD,
                    p_payc_eff_dt = obj.PAYC_EFF_DT,
                    p_payc_exp_dt = obj.PAYC_EXP_DT,
                    p_payc_pred_eff_dt = obj.PAYC_PRED_EFF_DT,
                    p_payc_pred_exp_dt = obj.PAYC_PRED_EFF_DT,
                    p_payc_kl_pcs = obj.PAYC_KL_PCS,
                    p_payc_ndb_pcs = obj.PAYC_NDB_PCS,
                    p_payc_ndb_remark_cd = obj.PAYC_NDB_REMARK_CD,
                    p_payc_ices_edit_name = obj.PAYC_ICES_EDIT_NAME,
                    p_payc_ices_ind = obj.PAYC_ICES_IND,
                    p_payc_pred_ind = obj.PAYC_PRED_IND,
                    p_payc_ices_edit_action = obj.PAYC_ICES_EDIT_ACTION,
                    p_payc_advn_notif = obj.PAYC_ADVN_NOTIF,
                    p_payc_mcr_routed = obj.PAYC_MCR_ROUTED,
                    p_payc_bifurcated = obj.PAYC_BIFURCATED,
                    p_payc_ns88_compliance = obj.PAYC_NS88_COMPLIANCE,
                    p_payc_additional_edits = obj.PAYC_ADDITIONAL_EDITS,
                    p_payc_comments = obj.PAYC_COMMENTS,
                    p_further_inst = obj.FURTHER_INST,
                    p_notes = obj.PAYC_NOTES,
                    p_user_id = obj.LST_UPDT_BY,
                    p_change_req_id = obj.CHANGE_REQ_ID,
                    p_change_desc = obj.CHANGE_DESC
                };

                var retVal = await ExecuteAsync<PayCode_Ins_Upd_Pkg_Param>("usp_PAYC_INS_UPD_DRIVER", obj2, null);
                notices = AnalyzeNotices(retVal);

                Nullable<DateTime> newVerEffDate = null; //USER STORY 129781 MFQ 3/14/2025
                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving PAYC_INS_UPD_DRIVER for PIMS ID: " + obj.PAYC_HIERARCHY_KEY + " and PayCode Version Dt: " + obj.PAYC_VER_EFF_DT.ToString() + ". RaisedErrors: --> " + notices);
                    newVerEffDate = obj.PAYC_VER_EFF_DT; //USER STORY 129781 MFQ 3/14/2025
                }
                else
                {
                    var maxDateSet = await GetMaxDateByKey(obj.PAYC_HIERARCHY_KEY); //USER STORY 129781 MFQ 3/14/2025
                    newVerEffDate = maxDateSet.Item2;
                }
                return ((int)notices.isFailure, (DateTime)newVerEffDate); //USER STORY 129781 MFQ 3/14/2025
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in UpdatePayCodeProcedure for PIMS ID: {obj.PAYC_HIERARCHY_KEY}, Version Dt: {obj.PAYC_VER_EFF_DT}. Exception: {ex}");
                return (-1, new DateTime());
            }
        }

        public async Task<IEnumerable<PayCode_ChangeHistory_Dto>> GetPayCodeChangeHistory(PayCode_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_payc_hierarchy_key", Value = obj.p_PAYC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_ms_id", Value = obj.p_MS_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<PayCode_ChangeHistory_Dto>("usp_Get_PIMS_APP_PAY_CODE_CHANGE_HISTORY_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 600);
            return data;

        }
        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk2()
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("USP_GET_ALL_PAYC_HIERARCHY_CODES_XWALK_V", parameters.ToArray(), "result_cursor", 600);
            return data;
        }

        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk()
        {
            var data = await QueryFuncAsync<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("usp_get_pims_payc_hierarchy_codes_xwalk_v_fun", null);
            return data;
        }
    }
}
