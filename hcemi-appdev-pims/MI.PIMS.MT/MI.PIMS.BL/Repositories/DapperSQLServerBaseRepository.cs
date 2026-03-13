using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MI.PIMS.BL.Common;

namespace MI.PIMS.BL.Repositories
{
    public class DapperSQLServerBaseRepository
    {

        protected static async Task<IEnumerable<T>> QueryAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var list = await connection.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                return list;
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var obj = await connection.QueryFirstOrDefaultAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                return obj;
            }
        }

        protected async Task<int> ExecuteAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }

        protected async Task<byte[]> ExecuteScalarAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.ExecuteScalarAsync<byte[]>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }
    }
}

