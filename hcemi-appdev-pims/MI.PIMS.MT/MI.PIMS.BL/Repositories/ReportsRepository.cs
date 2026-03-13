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
    public class ReportsRepository : DapperPostgresBaseRepository
    {
        private readonly ILoggerService _logger;
        public ReportsRepository(Helper helper, ILoggerService logger) : base(helper)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<ReportsDto>> GetReportLinks()
        {
            var parameters = new List<NpgsqlParameter>
            {
                new() { ParameterName = "result_cursor", Value = "result_cursor", NpgsqlDbType = NpgsqlDbType.Refcursor, Direction = ParameterDirection.InputOutput },
            };

            var data = await QueryCursorAsync<ReportsDto>("USP_PIMS_GET_REPORT_LINKS", parameters.ToArray(), "result_cursor", 60);
            return data;
        }

        public async Task<int> UpdateReportLinks(ReportsDto obj)
        {
            (int isFailure, string combinedMessage) notices = (1, string.Empty);
            try
            {
                UpdateReportDto updateReportDto = new UpdateReportDto()
                {
                    p_URLPATH = obj.URLPATH,
                    p_PAGE_ID = obj.PAGE_ID,
                    p_LST_UPDT_BY = obj.LST_UPDT_BY
                };
                try
                {
                    var retVal = await ExecuteAsync("USP_PIMS_UPDATE_REPORT_LINKS", updateReportDto);
                    notices = AnalyzeNotices(retVal);
                }
                catch (PostgresException ex)
                {
                    notices.isFailure = -1;
                    _logger.Error("Couldn't save UpdateReportLinks package: " + ex.Message);
                }
                catch (Exception ex)
                {
                    notices.isFailure = -1;
                    _logger.Error("Couldn't save UpdateReportLinks package: " + ex.Message);
                }

                if (notices.isFailure == -1 || notices.isFailure == 2)
                {
                    _logger.Error("Error while saving UpdateReportLinks for ReportsDto " + Newtonsoft.Json.JsonConvert.SerializeObject(updateReportDto) + ". RaisedErrors: --> " + notices);
                }

                return notices.isFailure;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in UpdateReportLinks for PAGE_ID: {obj.PAGE_ID}. Exception: {ex}");
                return -1;
            }
        }
    }
}
