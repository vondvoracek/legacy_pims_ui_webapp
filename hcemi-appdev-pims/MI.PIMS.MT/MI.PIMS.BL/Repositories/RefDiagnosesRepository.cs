using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
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
    public class RefDiagnosesRepository : DapperPostgresBaseRepository
    {
        private readonly AppDbContext _context;
        public RefDiagnosesRepository(Helper helper, AppDbContext context) : base(helper) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Ref_Diagnoses_T_Dto>> GetRefDiagnosesByFilter(string p_FILTER_TEXT)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "P_FILTER_TEXT" , Value  = p_FILTER_TEXT , Direction = ParameterDirection.Input},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<Ref_Diagnoses_T_Dto>("USP_GET_PIMS_REF_DIAGNOSES_V_BY_FITLER_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_Curr_Ver_V_List(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY" , Value  = obj.p_EPAL_HIERARCHY_KEY , Direction = ParameterDirection.Input}, //p_VV_SET_NAME
                new() { ParameterName = "p_EPAL_VER_EFF_DT" , Value  = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT, Direction = ParameterDirection.Input},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procs_Diagnoses_V_Dto>("USP_GET_PIMS_EPAL_PROCS_DIAGNOSES_CURR_VER_V_LIST_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<EPAL_Procs_Diagnoses_V_Dto>> GetPIMS_Diagnosis_V_List(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY" , Value  = obj.p_EPAL_HIERARCHY_KEY , Direction = ParameterDirection.Input}, //p_VV_SET_NAME
                new() { ParameterName = "p_EPAL_VER_EFF_DT" , Value  = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT , Direction = ParameterDirection.Input},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<EPAL_Procs_Diagnoses_V_Dto>("USP_GET_PIMS_EPAL_PROCS_DIAGNOSES_V_LIST_PRC", parameters.ToArray());
            return data;
        }

        public async Task<IEnumerable<REF_ALL_DIAG_CD_LISTS_V>> GetPIMS_DiagnosisList()
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<REF_ALL_DIAG_CD_LISTS_V>("USP_GET_PIMS_APP_REF_ALL_DIAG_CD_LISTS_V_PRC", parameters.ToArray());
            return data;
        }

        public async Task<DIAG_LIST_NAME_CNT_Dto> GetPIMS_DiagCount(EPAL_Procedures_Param_Dto obj)
        {
            var parameters = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_EPAL_HIERARCHY_KEY" , Value  = obj.p_EPAL_HIERARCHY_KEY , Direction = ParameterDirection.Input}, //p_VV_SET_NAME
                new() { ParameterName = "p_EPAL_VER_EFF_DT" , Value  = obj.p_EPAL_VER_EFF_DT == null ? DBNull.Value : obj.p_EPAL_VER_EFF_DT , Direction = ParameterDirection.Input},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryFirstOrDefaultCursorAsync<DIAG_LIST_NAME_CNT_Dto>("USP_GET_PIMS_DIAG_LIST_NAME_CNT_PRC", parameters.ToArray());
            return data;
        }

        public async Task<List<DPOC_REF_ALL_DIAG_CD_LISTS_V>> GetDPOC_DiagnosisList()
        {
            var result = await _context.ref_all_diag_cd_lists_v
                .Where(x => x.display_listname.StartsWith("DPOC - "))
                .GroupBy(x => x.display_listname)
                .Select(g => new DPOC_REF_ALL_DIAG_CD_LISTS_V
                {
                    display_listname = g.Key
                })
                .ToListAsync();

            return result;
        }
    }
}
