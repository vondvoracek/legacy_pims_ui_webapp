using Dapper;
using Dapper.Oracle;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class CategoryUpdateRepository: DapperPostgresBaseRepository
    {
        private readonly ILoggerService _logger;
        public CategoryUpdateRepository(Helper helper, ILoggerService logger) : base(helper) 
        {
            _logger = logger;
        }

        public async Task<IEnumerable<REF_ALT_CAT_SPLCTY_CMBNTNS_V_Dto>> GetSpecialtyCombination(string p_type, string p_parent_value)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_TYPE", Value = p_type == null ? DBNull.Value : p_type, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_PARENT_VALUE", Value = p_parent_value == null ? DBNull.Value : p_parent_value, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<REF_ALT_CAT_SPLCTY_CMBNTNS_V_Dto>("USP_GET_PIMS_REF_ALT_CAT_SPLCTY_CMBNTNS_V_PRC", parameters.ToArray());
            
            return data;
        }

        public async Task<IEnumerable<CategoryUpdate_Dto>> GetCategoryUpdates(CategoryUpdateParam obj)
        {
            /*
            if (obj.P_PROC_CD != null)
            {
                obj.P_PROC_CD = Helper.formatMultiSelectValues(obj.P_PROC_CD);
            }

            if (obj.P_DRUG_NM != null)
            {
                obj.P_DRUG_NM = Helper.formatMultiSelectValues(obj.P_DRUG_NM);
            }

            if (obj.P_ALTERNATE_CATEGORY != null)
            {
                obj.P_ALTERNATE_CATEGORY = Helper.formatMultiSelectValues(obj.P_ALTERNATE_CATEGORY);
            }

            if (obj.P_ALTERNATE_SUB_CATEGORY != null)
            {
                obj.P_ALTERNATE_SUB_CATEGORY = Helper.formatMultiSelectValues(obj.P_ALTERNATE_SUB_CATEGORY);
            }*/

            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PROC_CD", Value = obj.P_PROC_CD == null ? DBNull.Value : obj.P_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DRUG_NM", Value = obj.P_DRUG_NM == null ? DBNull.Value : obj.P_DRUG_NM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_CATEGORY", Value = obj.P_ALTERNATE_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_SUB_CATEGORY", Value = obj.P_ALTERNATE_SUB_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_SUB_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<CategoryUpdate_Dto>("USP_GET_PIMS_REF_ALT_CAT_XWALK_V_PRC", parameters.ToArray());
            return data;
        }

        public async Task<int> GetRecordsImpacted(CategoryUpdateParam obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PROC_CD", Value = obj.P_PROC_CD == null ? DBNull.Value : obj.P_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DRUG_NM", Value = obj.P_DRUG_NM == null ? DBNull.Value : obj.P_DRUG_NM, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_CATEGORY", Value = obj.P_ALTERNATE_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_SUB_CATEGORY", Value = obj.P_ALTERNATE_SUB_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_SUB_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryFirstOrDefaultCursorAsync<int>("USP_GET_PIMS_REF_ALT_CAT_XWALK_RECS_IMPCTD_V_PRC", parameters.ToArray());
            return data;            
        }
        public async Task<int> GetDuplicateAlternateCatetoriesCount(CategoryUpdateParam obj)
        {
            if (obj.P_PROC_CD != null)
            {
                obj.P_PROC_CD = Helper.formatMultiSelectValues(obj.P_PROC_CD);
            }

            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PROC_CD", Value = obj.P_PROC_CD == null ? DBNull.Value : obj.P_PROC_CD, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_DRUG_NM", Value = obj.P_DRUG_NM == null ? DBNull.Value : obj.P_DRUG_NM, NpgsqlDbType = NpgsqlDbType.Char }, // fix for pims-1.2024.3-hotfix-1.7 MFQ 8/5/2024
                new() { ParameterName = "P_ALTERNATE_CATEGORY", Value = obj.P_ALTERNATE_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_SUB_CATEGORY", Value = obj.P_ALTERNATE_SUB_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_SUB_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<int>("USP_GET_PIMS_APP_ALTERNATE_CATEGORY_DUP_V_PRC", parameters.ToArray());
            return data.Single();
        }
        public async Task<CategoryInsertByProcCDRetVal> InsertByProcCode(CategoryInsertByProcCDParam obj)
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_PROC_CD", Value = obj.P_PROC_CD == null ? DBNull.Value : obj.P_PROC_CD, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_CATEGORY", Value = obj.P_ALTERNATE_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_CATEGORY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_ALTERNATE_SUB_CATEGORY", Value = obj.P_ALTERNATE_SUB_CATEGORY == null ? DBNull.Value : obj.P_ALTERNATE_SUB_CATEGORY, Direction = ParameterDirection.Input,NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_LST_UPDT_BY", Value = obj.P_LST_UPDT_BY == null ? DBNull.Value : obj.P_LST_UPDT_BY, Direction = ParameterDirection.Input, NpgsqlDbType = NpgsqlDbType.Char }                
            };
                        
            var O_ALREADY_EXIST_COUNT = new NpgsqlParameter() { ParameterName = "O_ALREADY_EXIST_COUNT", Value = 0 , Direction = ParameterDirection.InputOutput, NpgsqlDbType = NpgsqlDbType.Integer };
            parameters.Add(O_ALREADY_EXIST_COUNT);
            
            (int isFailure, string combinedMessage) notices;
            try
            {
                var retVal = await ExecuteAsync("USP_PIMS_APP_ALTERNATE_CATEGORY_INSERT_BY_PROC_CD_PRC", parameters.ToArray());
                notices = AnalyzeNotices(retVal);
            }
            catch (Exception ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save USP_PIMS_APP_ALTERNATE_CATEGORY_INSERT_PRC: " + ex.Message);
            }

            CategoryInsertByProcCDRetVal categoryInsertByProcCDRetVal = new CategoryInsertByProcCDRetVal();
            categoryInsertByProcCDRetVal.O_ALREADY_EXIST_COUNT = (int)O_ALREADY_EXIST_COUNT.Value;

            return categoryInsertByProcCDRetVal;
        }

        public async Task<int> Insert(CategoryUpdateParam obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);

            try
            {
                var retVal = await ExecuteAsync("USP_PIMS_APP_ALTERNATE_CATEGORY_INSERT_PRC", obj);
                notices = AnalyzeNotices(retVal);
            }
            catch (PostgresException ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save USP_PIMS_APP_ALTERNATE_CATEGORY_INSERT_PRC", ex);
            }

            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error($"Error while saving CategoryUpdate/Insert for Prod CD: {obj.P_PROC_CD} and DRUG_NM: {obj.P_DRUG_NM}. RaisedErrors: --> {notices.combinedMessage}");
            }

            return notices.isFailure;

        }

        public async Task<int> Update(CategoryUpdate_Edit_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("USP_PIMS_APP_ALTERNATE_CATEGORY_UPDATE_PRC", new
                {
                    obj.P_PROC_CD,
                    obj.P_ALTERNATE_CATEGORY_OLD,
                    obj.P_ALTERNATE_SUB_CATEGORY_OLD,
                    obj.P_ALTERNATE_CATEGORY_NEW,
                    obj.P_ALTERNATE_SUB_CATEGORY_NEW,
                    obj.P_LST_UPDT_BY,
                    obj.P_LST_UPDT_DT
                });
                notices = AnalyzeNotices(retVal);
            }
            catch (PostgresException ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save USP_PIMS_APP_ALTERNATE_CATEGORY_UPDATE_PRC: " + ex.Message);
            }

            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error($"Error while saving CategoryUpdate/Insert for Prod CD: {obj.P_PROC_CD} and DRUG_NM: {obj.P_DRUG_NM}. RaisedErrors: --> {notices.combinedMessage}");
            }
            return notices.isFailure;
        }

        public async Task<int> Delete(CategoryUpdateParam obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("USP_PIMS_APP_ALTERNATE_CATEGORY_DELETE_PRC", new { 
                    obj.P_PROC_CD,
                    obj.P_DRUG_NM,
                    obj.P_ALTERNATE_CATEGORY,
                    obj.P_ALTERNATE_SUB_CATEGORY
                });
                notices = AnalyzeNotices(retVal);
            }
            catch (PostgresException ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save USP_PIMS_APP_ALTERNATE_CATEGORY_DELETE_PRC: " + ex.Message);
            }

            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error($"Error while saving CategoryDelete for Prod CD: {obj.P_PROC_CD} and DRUG_NM: {obj.P_DRUG_NM}. RaisedErrors: --> {notices.combinedMessage}");
            }
            return notices.isFailure;
        }

        public async Task<int> UpdateByAltCat(CategoryUpdate_Edit_Dto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                var retVal = await ExecuteAsync("USP_PIMS_APP_ALTERNATE_CATEGORY_DELETE_PRC", new
                {
                    obj.P_ALTERNATE_CATEGORY_OLD,
                    obj.P_ALTERNATE_SUB_CATEGORY_OLD,
                    obj.P_ALTERNATE_CATEGORY_NEW,
                    obj.P_ALTERNATE_SUB_CATEGORY_NEW,
                    obj.P_LST_UPDT_BY,
                    obj.P_LST_UPDT_DT
                });
                notices = AnalyzeNotices(retVal);
            }
            catch (PostgresException ex)
            {
                notices.isFailure = -1;
                _logger.Error("Couldn't save USP_PIMS_APP_ALTERNATE_CATEGORY_DELETE_PRC: " + ex.Message);
            }

            if (notices.isFailure == -1 || notices.isFailure == 2)
            {
                _logger.Error("Error while saving CategoryUpdate/UpdateByAltCat for P_ALTERNATE_SUB_CATEGORY_OLD: " + obj.P_ALTERNATE_CATEGORY_OLD + " and P_ALTERNATE_SUB_CATEGORY_OLD: " + obj.P_ALTERNATE_SUB_CATEGORY_OLD + ". RaisedErrors: --> " + notices);
            }
            return notices.isFailure;
        }

        public async Task<IEnumerable<Admin_Catagory_By_Type_Dto>> GetAdminCategoriesByType(Admin_Catagory_By_Type_Param_Dto obj)
        {            
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "P_TEXT", Value = obj.P_TEXT == null ? DBNull.Value : obj.P_TEXT, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "P_CATEGORY_TYPE", Value = obj.P_CATEGORY_TYPE == null ? DBNull.Value : obj.P_CATEGORY_TYPE, NpgsqlDbType = NpgsqlDbType.Char }, // fix for pims-1.2024.3-hotfix-1.7 MFQ 8/5/2024
                new() { ParameterName = "P_PARENT_CATEGORY", Value = obj.P_PARENT_CATEGORY == null ? DBNull.Value : obj.P_PARENT_CATEGORY, NpgsqlDbType = NpgsqlDbType.Char },
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor }
            };

            var data = await QueryCursorAsync<Admin_Catagory_By_Type_Dto>("USP_GET_PIMS_APP_ADMIN_CATEGORIES_BY_TYPE_PRC", parameters.ToArray());
            return data;
        }
    }
}
