using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BO.Dtos;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MI.PIMS.BL
{
    public class DPOCGuidelineDiagnosisCodesRepository: DapperPostgresBaseRepository
    {
        public DPOCGuidelineDiagnosisCodesRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByDPOC_ID(DPOC_Gdln_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                //new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },                
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Diagnoses_Dto>("USP_GET_PIMS_DPOC_INV_GDLN_DIAGNOSES_T_BY_DPOC_ID_PRC", parameters.ToArray(), "result_cursor", 60);
            if (data != null)
            {
                data = data.Select(c => { c.LIST_NAME_CODE = string.IsNullOrEmpty(c.LIST_NAME) ? null :Regex.Replace(c.LIST_NAME, @"\s+", "_").Replace("-","_"); return c; }).ToList();
            }
            return data;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetByGuideline(DPOC_Gdln_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },                
                new() { ParameterName = "P_IQ_GDLN_ID", Value = obj.p_IQ_GDLN_ID == null ? DBNull.Value : obj.p_IQ_GDLN_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_NUM", Value = obj.p_DPOC_VER_NUM == null ? DBNull.Value : obj.p_DPOC_VER_NUM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Diagnoses_Dto>("USP_GET_PIMS_DPOC_INV_GDLN_DIAGNOSES_T_PRC", parameters.ToArray(), "result_cursor", 60);
            if (data != null)
            {
                data = data.Select(c => { c.LIST_NAME_CODE = string.IsNullOrEmpty(c.LIST_NAME) ? null : Regex.Replace(c.LIST_NAME, @"\s+", "_").Replace("-", "_"); return c; }).ToList();
                data = data.Select(c => { c.hasChildren = true; return c; }).ToList();
            }
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Diagnoses_Dto>> GetCodesByGuideline(DPOC_Gdln_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_DPOC_HIERARCHY_KEY", Value = obj.p_DPOC_HIERARCHY_KEY == null ? DBNull.Value : obj.p_DPOC_HIERARCHY_KEY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_EFF_DT", Value = obj.p_DPOC_VER_EFF_DT == null ? DBNull.Value : obj.p_DPOC_VER_EFF_DT, NpgsqlDbType = NpgsqlDbType.Timestamp },
                new() { ParameterName = "P_DPOC_RELEASE", Value = obj.p_DPOC_RELEASE == null ? DBNull.Value : obj.p_DPOC_RELEASE, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_PACKAGE", Value = obj.p_DPOC_PACKAGE == null ? DBNull.Value : obj.p_DPOC_PACKAGE, NpgsqlDbType = NpgsqlDbType.Char },                
                new() { ParameterName = "P_IQ_GDLN_ID", Value = obj.p_IQ_GDLN_ID == null ? DBNull.Value : obj.p_IQ_GDLN_ID, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DPOC_VER_NUM", Value = obj.p_DPOC_VER_NUM == null ? DBNull.Value : obj.p_DPOC_VER_NUM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<DPOC_Inv_Gdln_Diagnoses_Dto>("USP_GET_PIMS_DPOC_INV_GDLN_DIAGNOSES_V_PRC", parameters.ToArray(), "result_cursor", 60);
            if (data != null)
            {
                data = data.Select(c => { c.LIST_NAME_CODE = string.IsNullOrEmpty(c.LIST_NAME) ? null : Regex.Replace(c.LIST_NAME, @"\s+", "_").Replace("-", "_"); return c; }).ToList();
            }
            return data;
        }
    }
}
