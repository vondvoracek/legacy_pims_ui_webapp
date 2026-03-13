using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class DPOCRepository : DapperPostgresBaseRepository
    {
        private readonly ILoggerService _logger;
        private readonly AppDbContext _context;
        public DPOCRepository(Helper helper, ILoggerService logger, AppDbContext context) : base(helper) 
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<DPOC_Inventories_V_Dto>> GetDPOCSearch(DPOC_Param_Dto obj)
        {
            var parameter = new DynamicParameters();
            
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor },
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_BUS_SEG_CD", Value = obj.p_DPOC_BUS_SEG_CD == null ? DBNull.Value : obj.p_DPOC_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_ENTITY_CD", Value = obj.p_DPOC_ENTITY_CD == null ? DBNull.Value : obj.p_DPOC_ENTITY_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_PROC_CD", Value = obj.p_PROC_CD == null ? DBNull.Value : obj.p_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_IQ_GDLN_ID", Value = obj.p_IQ_GDLN_ID == null ? DBNull.Value : obj.p_IQ_GDLN_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_IQ_GDLN_VERSION", Value = obj.p_IQ_GDLN_VERSION == null ? DBNull.Value : obj.p_IQ_GDLN_VERSION, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_IQ_CRITERIA", Value = obj.p_IQ_CRITERIA == null ? DBNull.Value : obj.p_IQ_CRITERIA, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_IQ_REFERENCE", Value = obj.p_IQ_REFERENCE == null ? DBNull.Value : obj.p_IQ_REFERENCE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_EFF_DT", Value = obj.p_DPOC_EFF_DT == null ? DBNull.Value : obj.p_DPOC_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                new() { ParameterName = "p_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_ELIGIBLE_IND", Value = obj.p_DPOC_ELIGIBLE_IND == null ? DBNull.Value : obj.p_DPOC_ELIGIBLE_IND, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_IMPLEMENTED_IND", Value = obj.p_DPOC_IMPLEMENTED_IND == null ? DBNull.Value : obj.p_DPOC_IMPLEMENTED_IND, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_EPAL_ALTRNT_SVC_CAT", Value = obj.p_EPAL_ALTRNT_SVC_CAT == null ? DBNull.Value : obj.p_EPAL_ALTRNT_SVC_CAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_EPAL_ALTRNT_SVC_SUBCAT", Value = obj.p_EPAL_ALTRNT_SVC_SUBCAT == null ? DBNull.Value : obj.p_EPAL_ALTRNT_SVC_SUBCAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                new() { ParameterName = "p_DTQ_TYPE", Value = obj.p_DTQ_TYPE == null ? DBNull.Value : obj.p_DTQ_TYPE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_RULE_TYPE_OUTCOME_OUTPAT", Value = obj.p_RULE_TYPE_OUTCOME_OUTPAT == null ? DBNull.Value : obj.p_RULE_TYPE_OUTCOME_OUTPAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_RULE_TYPE_OUTCOME_OUTPAT_FCLTY", Value = obj.p_RULE_TYPE_OUTCOME_OUTPAT_FCLTY == null ? DBNull.Value : obj.p_RULE_TYPE_OUTCOME_OUTPAT_FCLTY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_RULE_TYPE_OUTCOME_INPAT", Value = obj.p_RULE_TYPE_OUTCOME_INPAT == null ? DBNull.Value : obj.p_RULE_TYPE_OUTCOME_INPAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_JRSDCTN_NM", Value = obj.p_JRSDCTN_NM == null ? DBNull.Value : obj.p_JRSDCTN_NM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_INCLUDE_HISTORICAL", Value = obj.p_INCLUDE_HISTORICAL, NpgsqlDbType = NpgsqlDbType.Integer },
                new() { ParameterName = "p_PRODUCT_CD", Value = obj.p_PRODUCT_CD == null ? DBNull.Value : obj.p_PRODUCT_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_FUND_ARNGMNT_CD", Value = obj.p_FUND_ARNGMNT_CD == null ? DBNull.Value : obj.p_FUND_ARNGMNT_CD, NpgsqlDbType = NpgsqlDbType.Char },
            };

            var data = await QueryCursorAsync<DPOC_Inventories_V_Dto>("USP_GET_PIMS_DPOC_INVENTORIES_V_BYPARAM_PRC", parameters.ToArray(), "result_cursor", 10000);

            return data;
        }

        public async Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesByPIMS_ID(DPOC_Inventories_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.P_DPOC_PACKAGE == null ? DBNull.Value : obj.P_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "P_DPOC_RELEASE", Value = obj.P_DPOC_RELEASE == null ? DBNull.Value : obj.P_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryFirstOrDefaultCursorAsync<DPOC_Inventories_V_Dto>("usp_Get_PIMS_APP_DPOC_INVENTORIES_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 3000);

            if (data == null) data = new DPOC_Inventories_V_Dto();

            // Convert 12/31/2999, 12/31/1999 to null

            //data.DPOC_EFF_DT = Helper.CheckExpYear(data.DPOC_EFF_DT);   --> DY: 12/10/2024 - Bug 118562: Blank DPOC Start Date when data shows 01/01/1900

            data.DPOC_EXP_DT = Helper.CheckExpYear(data.DPOC_EXP_DT);
            data.DPOC_VER_EFF_DT = Helper.CheckExpYear(data.DPOC_VER_EFF_DT);
            data.DPOC_VER_EXP_DT = Helper.CheckExpYear(data.DPOC_VER_EXP_DT);
            data.EPAL_VER_EFF_DT = Helper.CheckExpYear(data.EPAL_VER_EFF_DT);
            data.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EFF_DT);
            data.EPAL_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EXP_DT);
            return data;
        }

        public async Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesLstUpdtRecordByPIMS_ID(string dpoc_hierarchy_key, string p_dpoc_package)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_DPOC_HIERARCHY_KEY", Value = dpoc_hierarchy_key == null ? DBNull.Value : dpoc_hierarchy_key, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_dpoc_package", Value = p_dpoc_package == null ? DBNull.Value : p_dpoc_package, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "p_dpoc_release", Value = p_dpoc_release == null ? DBNull.Value : p_dpoc_release, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryFirstOrDefaultCursorAsync<DPOC_Inventories_V_Dto>("usp_Get_PIMS_APP_DPOC_INVENTORIES_LST_UPDT_REC_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            if (data == null) data = new DPOC_Inventories_V_Dto();

            // Convert 12/31/2999, 12/31/1999 to null
            data.DPOC_EFF_DT = Helper.CheckExpYear(data.DPOC_EFF_DT);
            data.DPOC_EXP_DT = Helper.CheckExpYear(data.DPOC_EXP_DT);
            data.DPOC_VER_EFF_DT = Helper.CheckExpYear(data.DPOC_VER_EFF_DT);
            data.DPOC_VER_EXP_DT = Helper.CheckExpYear(data.DPOC_VER_EXP_DT);
            data.EPAL_VER_EFF_DT = Helper.CheckExpYear(data.EPAL_VER_EFF_DT);
            data.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EFF_DT);
            data.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EFF_DT);
            data.EPAL_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EXP_DT);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inventories_V_Hist_Dto>> GetDPOCInventoriesHistByPIMS_ID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inventories_V_Hist_Dto>("usp_Get_PIMS_APP_DPOC_INVENTORIES_V_HIST_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            foreach (var item in data)
            {
                item.DPOC_EFF_DT = Helper.CheckExpYear(item.DPOC_EFF_DT);
                item.DPOC_EXP_DT = Helper.CheckExpYear(item.DPOC_EXP_DT);
                item.DPOC_VER_EFF_DT = Helper.CheckExpYear(item.DPOC_VER_EFF_DT);
                item.DPOC_VER_EXP_DT = Helper.CheckExpYear(item.DPOC_VER_EXP_DT);
                item.EPAL_VER_EFF_DT = Helper.CheckExpYear(item.EPAL_VER_EFF_DT);
                item.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(item.EPAL_PRIOR_AUTH_EFF_DT);
                item.EPAL_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(item.EPAL_PRIOR_AUTH_EXP_DT);
            }
            return data;
        }

        public async Task<IEnumerable<DPOC_ChangeHistory_Dto>> GetDPOCChangeHistoryByPIMSID(DPOC_PIMS_ID_Param_Dto obj)  
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PIMS_ID", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };
            var data = await QueryCursorAsync<DPOC_ChangeHistory_Dto>("USP_GET_PIMS_APP_DPOC_CHANGE_HISTORY_PRC", parameters.ToArray(), "result_cursor", 60);

            return data;
        }

        public async Task<string> GetDPOCIDExistStatus(string dpoc_hierarchy_key, string p_dpoc_package)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_dpoc_hierarchy_key", Value = dpoc_hierarchy_key == null ? DBNull.Value : dpoc_hierarchy_key, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_dpoc_package", Value = p_dpoc_package == null ? DBNull.Value : p_dpoc_package, NpgsqlDbType = NpgsqlDbType.Char },                
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };
            // Get dpoc_hierarchy_key as Exists or NotExists
            var data = await QueryFirstOrDefaultCursorAsync<string>("USP_GET_PIMS_APP_DPOC_ID_EXISTS_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }       

        public async Task<IEnumerable<DPOC_Additional_Req_His_Dto>> GetPIMSAdditionalInfoHistory(string dpoc_hierarchy_key)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_dpoc_hierarchy_key", Value = dpoc_hierarchy_key == null ? DBNull.Value : dpoc_hierarchy_key, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Additional_Req_His_Dto>("USP_GET_PIMS_APP_DPOC_ADDITIONAL_REQ_HIS_BY_PIMS_ID_PRC", parameters.ToArray(),"result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<DPOC_POS_Dto>> GetPOS(DPOC_PIMS_ID_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_POS_Dto>("usp_Get_PIMS_APP_DPOC_INV_POS_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IsCurrentRecordDto> GetPIMSIsCurrentRecord(IsCurrentRecordDto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_pims_ver_eff_dt", Value = obj.p_PIMS_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "p_module_name", Value = obj.p_MODULE_NAME.ToLower(), NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_pims_id", Value = obj.p_PIMS_ID  == null ? DBNull.Value : obj.p_PIMS_ID , NpgsqlDbType = NpgsqlDbType.Char }
            };

            var data = await QueryFirstOrDefaultCursorAsync<IsCurrentRecordDto>("USP_GET_PIMS_IS_CURRENT_RECORD_PRC", parameters.ToArray(), "result_cursor", 600);
            return data;
        }

        /// <summary>
        /// XX Remove the SP
        /// </summary>
        /// <param name="obj">DPOC_Inv_Gdln_Rules_Param_Dto</param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetDPOCInvGdlnRulesByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                new() { ParameterName = "p_IQ_GDLN_STATUS", Value = obj.p_IQ_GDLN_STATUS == null ? DBNull.Value : obj.p_IQ_GDLN_STATUS, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Rules_V_Dto>("usp_Get_PIMS_APP_DPOC_INV_GDLN_RULES_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); d.IQ_GDLN_REL_DT = Helper.CheckExpYear(d.IQ_GDLN_REL_DT); return d; });
            return data;
        }
        /// <summary>
        /// XX REMOVE THE SP
        /// </summary>
        /// <param name="obj">DPOC_PIMS_ID_Param_Dto</param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_INV_DTQS_V_Dto>> GetDPOCInvDTQSByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {

            var parameter = new DynamicParameters();
            parameter.Add("p_DPOC_HIERARCHY_KEY", obj.p_DPOC_HIERARCHY_KEY);
            parameter.Add("p_DPOC_VER_EFF_DT", obj.p_DPOC_VER_EFF_DT);
            var data = await QueryAsync<DPOC_INV_DTQS_V_Dto>("usp_Get_PIMS_APP_DPOC_INV_DTQS_V_BY_PIMS_ID_PRC", parameter, 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); return d; });

            return data;
        }

        /// <summary>
        /// XX REMOVE THE SP
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_INV_DTQS_TGT_V>> GetDPOCInvDTQTGTsByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {

            var parameter = new DynamicParameters();
            parameter.Add("p_DPOC_HIERARCHY_KEY", obj.p_DPOC_HIERARCHY_KEY);
            parameter.Add("p_DPOC_VER_EFF_DT", obj.p_DPOC_VER_EFF_DT);
            var data = await QueryAsync<DPOC_INV_DTQS_TGT_V>("usp_Get_PIMS_APP_DPOC_INV_DTQS_TGT_V_BY_PIMS_ID_PRC", parameter, 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); return d; });

            return data;
        }

        /// <summary>
        /// XX REMOVE THE SP
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_INV_DTQS_HOLDING_V>> GetDPOCInvDTQHoldingsByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Date },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };
            var data = await QueryCursorAsync<DPOC_INV_DTQS_HOLDING_V>("usp_Get_PIMS_APP_DPOC_INV_DTQS_HOLDING_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); return d; });

            return data;
        }

        /// <summary>
        /// XX Remove
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DPOC_Inv_Dtqs_V_Dto>> GetDPOC_Inv_Dtqs_V()
        {
            var parameter = new DynamicParameters();
            var data = await QueryAsync<DPOC_Inv_Dtqs_V_Dto>("USP_GET_PIMS_DPOC_INV_DTQS_V", null, 60);
            return data;
        }

        public async Task<int> DPOC_INS_UPD_DRIVER_PRC(DPOC_Ins_Upd_Pkg_Param obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("USP_DPOC_INS_UPD_DRIVER_PRC", obj);
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Warn($"Business rule failure in DPOC_INS_UPD_DRIVER_PRC for DPOC ID: {obj.DPOC_ID}, Version Dt: {obj.DPOC_VER_EFF_DT}. Notices: {notices}");
                }

                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in DPOC_INS_UPD_DRIVER_PRC for DPOC ID: {obj.DPOC_ID}, Version Dt: {obj.DPOC_VER_EFF_DT}. Exception: {ex}");
                return -1;
            }
        }

        public async Task<int> DPOC_DELETE_DRIVER_PRC(DPOC_Delete_Pkg_Param obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("usp_dpoc_delete_driver_prc", obj, null);
                notices = AnalyzeNotices(retVal);

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error($"Error while saving DPOC_DELETE_DRIVER_PRC for DPOC ID: {obj.P_DPOC_HIERARCHY_KEY} and DPOC Version Dt: {obj.P_DPOC_VER_EFF_DT}. RaisedErrors: --> {notices}");
                }

                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in DPOC_DELETE_DRIVER_PRC: {ex.Message}", ex);
                return -1;
            }
        }

        public async Task<IEnumerable<string>> GetDPOC_SOS_PROVIDER_TIN_EXCL()
        {
            var data = await _context.dpoc_inventories_t.Where(d => d.DPOC_Sos_Provider_Tin_Excl != null)
                .Select(x => x.DPOC_Sos_Provider_Tin_Excl)
                .OrderBy(x => x)
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<string>> GetDPOCRelease()
        {
            var data = await _context.dpoc_inv_gdln_rules_act_ret_v.Where(d => !string.IsNullOrEmpty(d.Dpoc_Release)) //DevOps 128804 MFQ 7/22/2023
                .Select(x => x.Dpoc_Release)
                .Distinct()
                .OrderBy(x => x)                
                .ToListAsync();

            return data;
        }
        public async Task<IEnumerable<string>> GetDPOCPackage()
        {
            var data = await _context.dpoc_inventories_t.Where(d => !string.IsNullOrEmpty(d.DPOC_Package))
                .Select(x => x.DPOC_Package)
                .Distinct()
                .OrderBy(x => x)                
                .ToListAsync();

            return data;
        }
    }
}
