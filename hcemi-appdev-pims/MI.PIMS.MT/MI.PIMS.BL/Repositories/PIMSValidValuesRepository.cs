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
    public class PIMSValidValuesRepository : DapperPostgresBaseRepository
    {        
        public PIMSValidValuesRepository(Helper helper) : base(helper) { }
        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            var parameter = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_vv_set_name" , Value  = p_VV_SET_NAME.ToStringNullSafe() , NpgsqlDbType = NpgsqlDbType.Char}, //p_VV_SET_NAME
                new() { ParameterName = "p_bus_seg_cd" , Value  = p_BUS_SEG_CD.ToStringNullSafe() , NpgsqlDbType = NpgsqlDbType.Char},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput }
            };

            var data = await QueryCursorAsync<PIMS_Valid_Values_V_Dto>("usp_Get_PIMS_VALID_VALUES_V_prc", parameter.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues_VVCdOnly(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            var parameter = new List<NpgsqlParameter>()
            {
                new() { ParameterName = "p_vv_set_name" , Value  = p_VV_SET_NAME.ToStringNullSafe() , NpgsqlDbType = NpgsqlDbType.Char},
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
                new() { ParameterName = "p_bus_seg_cd" , Value  =  p_BUS_SEG_CD == null ? DBNull.Value: p_BUS_SEG_CD, NpgsqlDbType = NpgsqlDbType.Char}

            };

            var data = await QueryCursorAsync<PIMS_Valid_Values_V_Dto>("usp_Get_PIMS_VALID_VALUES_V_VVCD_ONLY_prc", parameter.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues()
        {
            var data = await QueryFuncAsync<PIMS_Valid_Values_V_Dto>("usp_get_pims_valid_values_v_fun", null);
            return data;
        }

    }
}
