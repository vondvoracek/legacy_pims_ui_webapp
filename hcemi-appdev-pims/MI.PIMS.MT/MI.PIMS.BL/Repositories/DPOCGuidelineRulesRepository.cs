using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using MI.PIMS.BO.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class DPOCGuidelineRulesRepository: DapperPostgresBaseRepository
    {
        private readonly AppDbContext _context;
        public DPOCGuidelineRulesRepository(Helper helper, AppDbContext context) : base(helper) {
            _context = context;
        }
        
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetDPOCGuidelineRules(DPOC_Inventories_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_COLUMN_NAME", Value = obj.P_COLUMN_NAME == null ? DBNull.Value : obj.P_COLUMN_NAME, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Rules_V>("USP_GET_PIMS_DPOC_INV_GDLN_RULES_V", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineRulesList(string p_text, string fieldName)
        {
            IEnumerable<DPOC_Inv_Gdln_Rules_V> data = null;
            if (fieldName == "iq_gdln_version")
            {
                data = await _context.dpoc_inv_gdln_rules_act_ret_v
                    .Where(x => !string.IsNullOrEmpty(x.Iq_Gdln_Version) &&
                                EF.Functions.ILike(x.Iq_Gdln_Version, $"{p_text}%"))
                    .Select(x => new DPOC_Inv_Gdln_Rules_V
                    {
                        IQ_GDLN_VERSION = x.Iq_Gdln_Version,
                    })
                    .Distinct()
                    .OrderBy(x => x.IQ_GDLN_VERSION)
                    .ToListAsync();
            }
            else if (fieldName == "iq_gdln_id")
            {
                data = await _context.dpoc_inv_gdln_rules_act_ret_v
                    //.Where(x => !string.IsNullOrEmpty(x.Iq_Gdln_Id) &&
                    //            EF.Functions.ILike(x.Iq_Gdln_Id, $"{p_text}%"))
                    .Select(x => new DPOC_Inv_Gdln_Rules_V
                    {
                        IQ_GDLN_ID = x.Iq_Gdln_Id,
                    })
                    .Distinct()
                    .OrderBy(x => x.IQ_GDLN_ID)
                    .ToListAsync();
            }
            else if (fieldName == "iq_reference")
            {
                data = await _context.dpoc_inv_gdln_rules_act_ret_v
                    .Where(x => !string.IsNullOrEmpty(x.Iq_Gdln_Nm) &&
                                EF.Functions.ILike(x.Iq_Gdln_Nm, $"{p_text}%"))
                    .Select(x => new DPOC_Inv_Gdln_Rules_V
                    {
                        IQ_GDLN_NM = x.Iq_Gdln_Nm,
                    })
                    .Distinct()
                    .OrderBy(x => x.IQ_GDLN_NM)
                    .ToListAsync();
            }

            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V>> GetGuideLineIds(string text)
        {
            var allItems = await _context.dpoc_inv_gdln_rules_t
                .Select(x => new DPOC_Inv_Gdln_Rules_V
                {
                    IQ_GDLN_ID = x.iq_gdln_id,
                })
                .Distinct()
                .OrderBy(x => x.IQ_GDLN_ID)
                .ToListAsync();


            var filteredItems = string.IsNullOrEmpty(text)
                ? allItems
                : allItems.Where(x => x.IQ_GDLN_ID.Contains(text, StringComparison.OrdinalIgnoreCase));

            //var data = await _context.dpoc_inv_gdln_rules_act_ret_v
            //        .Where(x => !string.IsNullOrEmpty(x.Iq_Gdln_Id) &&
            //                    EF.Functions.ILike(x.Iq_Gdln_Id, $"{text}%"))
            //        .Select(x => new DPOC_Inv_Gdln_Rules_V
            //        {
            //            IQ_GDLN_ID = x.Iq_Gdln_Id,
            //        })
            //        .Distinct()
            //        .OrderBy(x => x.IQ_GDLN_ID)
            //        .ToListAsync();
            return filteredItems;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetPendingByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Rules_V_Dto>("usp_Get_PIMS_APP_DPOC_INV_GDLN_RULES_PND_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            //data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); d.IQ_GDLN_REL_DT = Helper.CheckExpYear(d.IQ_GDLN_REL_DT); return d; });
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                //new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Rules_V_Dto>("usp_Get_PIMS_APP_DPOC_INV_GDLN_RULES_ACT_RET_V_BY_PIMS_ID_PRC", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); d.IQ_GDLN_REL_DT = Helper.CheckExpYear(d.IQ_GDLN_REL_DT); return d; });
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetActiveRetiredByPIMSIDSummary(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },                                
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor },
                new() { ParameterName = "p_is_historical", Value = obj.P_IS_HISTORICAL , NpgsqlDbType = NpgsqlDbType.Integer }                
            };
            //proc name truncated for postgres usp_Get_PIMS_APP_DPOC_INV_GDLN_RULES_ACT_RET_V_BY_PIMS_ID_SUMMARY_PRC
            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Rules_V_Dto>("usp_get_pims_app_dpoc_inv_gdln_rules_act_ret_v_by_pims_id_summa", parameters.ToArray(), "result_cursor", 60);

            // Convert 12/31/2999, 12/31/1999 to null
            data = data.Select(d => { d.DPOC_VER_EFF_DT = Helper.CheckExpYear(d.DPOC_VER_EFF_DT); d.IQ_GDLN_REL_DT = Helper.CheckExpYear(d.IQ_GDLN_REL_DT); return d; });
            return data;
        }

        public async Task<IEnumerable<DPOC_INV_PLCY_LKP_V_Dto>> GetInvGntvPlcyLkp(DPOC_INV_PLCY_LKP_Param obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("P_DPOC_BUS_SEG_CD", obj.P_DPOC_BUS_SEG_CD);
            parameter.Add("P_DPOC_ENTITY_CD", obj.P_DPOC_ENTITY_CD);
            parameter.Add("P_PROC_CD", obj.P_PROC_CD);

            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_DPOC_BUS_SEG_CD", Value = obj.P_DPOC_BUS_SEG_CD == null ? DBNull.Value : obj.P_DPOC_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_ENTITY_CD", Value = obj.P_DPOC_ENTITY_CD == null ? DBNull.Value : obj.P_DPOC_ENTITY_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_PROC_CD", Value = obj.P_PROC_CD == null ? DBNull.Value : obj.P_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_INV_PLCY_LKP_V_Dto>("USP_GET_PIMS_APP_DPOC_INV_PLCY_LKP_V_PRC", parameters.ToArray(), "result_cursor", 60);
            return data;
        }
        /// <summary>
        /// REMOVE X
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DPOC_Inv_Gdln_Rules_V_Dto> GetDPOCGuidelineRulesDetailByKey(DPOC_Inv_Gdln_Rules_V_Key obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "p_DPOC_HIERARCHY_KEY", Value = obj.DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "p_DPOC_VER_EFF_DT", Value = obj.DPOC_VER_EFF_DT == null ? DBNull.Value : obj.DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.DPOC_PACKAGE == null ? DBNull.Value : obj.DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_RELEASE", Value = obj.DPOC_RELEASE == null ? DBNull.Value : obj.DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_IQ_GDLN_ID", Value = obj.IQ_GDLN_ID == null ? DBNull.Value : obj.IQ_GDLN_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_RULE_OUTCOME_OUTPAT", Value = obj.RULE_OUTCOME_OUTPAT == null ? DBNull.Value : obj.RULE_OUTCOME_OUTPAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_RULE_OUTCOME_OUTPAT_FCLTY", Value = obj.RULE_OUTCOME_OUTPAT_FCLTY == null ? DBNull.Value : obj.RULE_OUTCOME_OUTPAT_FCLTY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_RULE_OUTCOME_INPAT", Value = obj.RULE_OUTCOME_INPAT == null ? DBNull.Value : obj.RULE_OUTCOME_INPAT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            DPOC_Inv_Gdln_Rules_V_Dto data = new DPOC_Inv_Gdln_Rules_V_Dto();

            data = await QueryFirstOrDefaultCursorAsync<DPOC_Inv_Gdln_Rules_V_Dto>("usp_Get_PIMS_APP_DPOC_INV_GDLN_RULES_V_DETAIL_BY_KEY_PRC", parameters.ToArray(), "result_cursor", 60);
            if (data == null) data = new DPOC_Inv_Gdln_Rules_V_Dto();

            return data;
        }
    }
}
