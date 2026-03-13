using Dapper;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MI.PIMS.BL.Repositories
{
    public class DapperPostgresBaseRepository
    {
        private readonly Helper _helper;
        private string _connectionString = string.Empty;
        public DapperPostgresBaseRepository(Helper helper)
        {
            _helper = helper;
        }
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null, CommandType commandType = CommandType.StoredProcedure)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var list = await connection.QueryAsync<T>(sp, parameters, commandTimeout: timeout, commandType: commandType);
                return list;
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var obj = await connection.QueryFirstOrDefaultAsync<T>(_helper.DatabaseSchema + "." + sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                return obj;
            }
        }

        protected async Task<int> ExecuteAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null, CommandType commandType = CommandType.Text)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                sp = ("call " + _helper.DatabaseSchema + "." + sp + "(" + await GetParamsFromObject(parameters) + ")").ToLower();

                return await connection.ExecuteAsync(sp, parameters, commandType: commandType, commandTimeout: timeout);
            }
        }
        protected async Task<string[]> ExecuteAsync<T>(string sp, T paramObj, string[] ignoreColumns = null)
        {
            _connectionString = _helper.GetConnectionString;
            List<string> retVal = new();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using var command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "call " + _helper.DatabaseSchema.ToLower() + "." + sp.ToLower() + "(" + await GetParamsFromObject<T>(paramObj, ignoreColumns) + ")";
                
                connection.Notice += (sender, e) => retVal.Add(e.Notice.MessageText); // Console.WriteLine($"NOTICE: {e.Notice.MessageText}")

                foreach (System.Reflection.PropertyInfo prop in paramObj.GetType().GetProperties())
                {
                    var igCount = ignoreColumns == null ? false : ignoreColumns.Contains(prop.Name);

                    if (prop.Name.Substring(0, 2).ToLower() == "p_" && igCount == false)
                    {
                        command.Parameters.AddWithValue("@" + prop.Name.ToLower(), prop.GetValue(paramObj, null) == null ? DBNull.Value : prop.GetValue(paramObj, null));
                    }
                }
                await command.ExecuteNonQueryAsync();
            }
            return retVal.ToArray();
        }
        protected async Task<string[]> ExecuteAsync(string sp, NpgsqlParameter[] parameters, string connectionString = null)
        {            
            _connectionString = connectionString ?? _helper.GetConnectionString;
            List<string> retVal = new();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    using (var command = new NpgsqlCommand(_helper.DatabaseSchema + "." + sp, conn))
                    {                        
                        command.CommandText = "call " + _helper.DatabaseSchema.ToLower() + "." + sp.ToLower() + "(" + await GetParamFromNpgParams(parameters) + ")";
                        conn.Notice += (sender, e) => retVal.Add(e.Notice.MessageText); // Console.WriteLine($"NOTICE: {e.Notice.MessageText}")

                        if (parameters != null)
                        {
                            foreach (NpgsqlParameter param in parameters)
                            {
                                param.ParameterName = param.ParameterName.ToLower();
                            }
                        }
                        //command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            return retVal.ToArray();
        }
        protected async Task<byte[]> ExecuteScalarAsync(string sp, DynamicParameters parameters, int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<byte[]>(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }

        protected async Task<IEnumerable<T>> QueryCursorAsync<T>(string sp, NpgsqlParameter[] parameters, string refcursor = "result_cursor", int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using var conn = new NpgsqlConnection(_connectionString);            
            conn.Open();
            
            using var trans = await conn.BeginTransactionAsync();
            using var command = new NpgsqlCommand(_helper.DatabaseSchema + "." + sp, conn);
            command.CommandTimeout = timeout;
            command.CommandText = "call " + _helper.DatabaseSchema.ToLower() + "." + sp.ToLower() + "(" + await GetParamFromNpgParams(parameters) + ")";

            foreach (NpgsqlParameter param in parameters)
            {
                command.Parameters.Add("@" + param.ParameterName.ToLower(), param.NpgsqlDbType).Value = param.Value;
            }
            await command.ExecuteNonQueryAsync();
            var d = await conn.QueryAsync<T>("fetch all in \"" + refcursor + "\"", null, null, timeout, CommandType.Text);
            return d;
        }

        protected async Task<T> QueryFirstOrDefaultCursorAsync<T>(string sp, NpgsqlParameter[] parameters, string refcursor = "result_cursor", int timeout = 60, string connectionString = null)
        {
            _connectionString = connectionString ?? _helper.GetConnectionString;
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var trans = await conn.BeginTransactionAsync();
            using var command = new NpgsqlCommand(_helper.DatabaseSchema + "." + sp, conn);
            command.CommandText = "call " + _helper.DatabaseSchema.ToLower() + "." + sp.ToLower() + "(" + await GetParamFromNpgParams(parameters) + ")";

            foreach (NpgsqlParameter param in parameters)
            {
                command.Parameters.Add("@" + param.ParameterName.ToLower(), param.NpgsqlDbType).Value = param.Value;
            }
            await command.ExecuteNonQueryAsync();

            var d = await conn.QueryFirstOrDefaultAsync<T>("fetch all in \"" + refcursor + "\"", null, null, timeout, CommandType.Text);
            return d;
        }

        protected async Task<IEnumerable<T>> QueryFuncAsync<T>(string funcName, NpgsqlParameter[] parameters, int timeout = 60, string connectionString = null)
        {
            IList<T> retValList = null;
            _connectionString = connectionString ?? _helper.GetConnectionString;
            var connString = new NpgsqlConnectionStringBuilder(_connectionString) {  CommandTimeout = timeout }; 
            using (var connection = new NpgsqlConnection(connString.ConnectionString))
            {
                await connection.OpenAsync();                                
                var query = "select * from " + _helper.DatabaseSchema + "." + funcName + "(" + (parameters != null ? GetParamValueByCommaFromNpgParams(parameters) : "") + ");";

                var data = await connection.QueryAsync<T>(query);
                retValList  = data.ToList();
            }

            return retValList;
        }        

        protected async Task<IEnumerable<T>> QuerySQLAsync<T>(string sSQL, object parameters, int timeout = 60, string connectionString = null)
        {
            IList<T> retValList = null;
            _connectionString = connectionString ?? _helper.GetConnectionString;
            var connString = new NpgsqlConnectionStringBuilder(_connectionString) { CommandTimeout = timeout };
            using (var connection = new NpgsqlConnection(connString.ConnectionString))
            {
                await connection.OpenAsync();
                var query = sSQL;
                var data = await connection.QueryAsync<T>(query, parameters);
                retValList = data.ToList();
            }

            return retValList;
        }

        protected async Task<T> QueryFirstOrDefaultFuncAsync<T>(string funcName, NpgsqlParameter[] parameters, int timeout = 60, string connectionString = null)
        {
            T retVal;
            _connectionString = connectionString ?? _helper.GetConnectionString;
            var connString = new NpgsqlConnectionStringBuilder(_connectionString) { CommandTimeout = timeout };
            using (var connection = new NpgsqlConnection(connString.ConnectionString))
            {
                await connection.OpenAsync();

                var query = "select * from " + _helper.DatabaseSchema + "." + funcName + "(" + (parameters != null ? GetParamValueByCommaFromNpgParams(parameters) : "") + ");";
                var data = await connection.QueryFirstOrDefaultAsync<T>(query);
                retVal = data;
            }

            return retVal;
        }

        #region "Functions"
        protected (int isFailure, string combinedMessage) AnalyzeNotices(string[] notices)
        {
            int isFailure = 0;

            bool hasFailure = notices.Any(not => not.Contains("failure", StringComparison.OrdinalIgnoreCase));
            bool hasSuccess = notices.Any(not => not.Contains("successfully", StringComparison.OrdinalIgnoreCase));

            if (hasFailure && hasSuccess)
                isFailure = -1;
            else if (hasSuccess)
                isFailure = 1;
            else if (hasFailure)
                isFailure = -1;

            return (isFailure, string.Join(", ", notices));
        }

        private async Task<string> GetParamsFromObject(DynamicParameters parameters)
        {
            string dParams =string.Empty;
            foreach(var param in parameters.ParameterNames)
            {
                dParams += (string.IsNullOrEmpty(dParams) ? "": ",") + "@" + param.ToString()?.ToLower();
            }
            return await Task.FromResult(dParams);
        }

        private async Task<string> GetParamsFromObject<T>(T obj, string[] ignoreColumns)
        {
            string dParams = string.Empty;

            // Get the type of the object
            Type type = obj.GetType();

            // Get all the properties of the object
            PropertyInfo[] properties = type.GetProperties();

            // Loop through each property
            foreach (PropertyInfo property in properties)
            {
                // Get the property name
                string propertyName = property.Name;

                var igCount = ignoreColumns == null ? false : ignoreColumns.Contains(propertyName);

                if (propertyName.ToLower().Substring(0, 2) == "p_" && igCount == false)
                    dParams += (string.IsNullOrEmpty(dParams) ? "" : ",") + "@" + propertyName.ToString()?.ToLower();
            }
            return await Task.FromResult(dParams);
        }

        private async Task<string> GetParamFromNpgParams(NpgsqlParameter[] parameters)
        {
            string dParams = string.Empty;
            foreach (NpgsqlParameter param in parameters)
            {
                dParams += (string.IsNullOrEmpty(dParams) ? "" : ",") + "@" + param.ParameterName.ToString()?.ToLower();
            }
            return await Task.FromResult(dParams);
        }
        private static string GetParamValueByCommaFromNpgParams(NpgsqlParameter[] parameters)
        {
            string dParams = string.Empty;
            foreach (NpgsqlParameter param in parameters)
            {
                dParams += (string.IsNullOrEmpty(dParams) ? "" : ",") + (param.Value == DBNull.Value ? "null": "'" + param.Value + "'") ;
            }
            return dParams;
        }
        #endregion
    }
}
