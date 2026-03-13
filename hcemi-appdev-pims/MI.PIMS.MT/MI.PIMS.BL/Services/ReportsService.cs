using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class ReportsService: IReportsService
    {
        private readonly ReportsRepository _repo;

        public ReportsService(ReportsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReportsDto>> GetReportLinks()
        {
            var data = await _repo.GetReportLinks();
            return data;
        }


        public async Task<UpdateDto> UpdateReportLinks(ReportsDto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.UpdateReportLinks(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = "Record saved!";
                updateTO.StatusType = RetValStatus.Success.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Error!";
                updateTO.StatusType = RetValStatus.Error.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
            }

            return updateTO;
        }


    }
}
