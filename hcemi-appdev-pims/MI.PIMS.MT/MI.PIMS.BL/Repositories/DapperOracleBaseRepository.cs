using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using MI.PIMS.BL.Common;
using Dapper.Oracle;

namespace MI.PIMS.BL.Repositories
{
    public class DapperOracleBaseRepository
    {
        public readonly Helper _helper;
        private string _connectionString = string.Empty;
        public DapperOracleBaseRepository(Helper helper)
        {
            _helper = helper;
        }
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetOracleConnectionString;

            using (var connection = new OracleConnection(_connectionString))
            {
                var list = await connection.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                return list;
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetOracleConnectionString;

            using (var connection = new OracleConnection(_connectionString))
            {
                var obj = await connection.QueryFirstOrDefaultAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                return obj;
            }
        }

        protected async Task<int> ExecuteAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetOracleConnectionString;

            using (var connection = new OracleConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }
        protected async Task<int> ExecuteAsync(string sp, OracleDynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetOracleConnectionString;

            using (var connection = new OracleConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }
        protected async Task<byte[]> ExecuteScalarAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetOracleConnectionString;

            using (var connection = new OracleConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<byte[]>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }

    }
}
