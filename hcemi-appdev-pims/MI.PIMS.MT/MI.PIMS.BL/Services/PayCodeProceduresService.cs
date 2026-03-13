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
    public class PayCodeProceduresService : IPayCodeProceduresService
    {
        private readonly PayCodeProceduresRepository _repo;

        public PayCodeProceduresService(PayCodeProceduresRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeProceduresSearch(PayCode_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPayCodeProceduresSearch(obj);
            return data;
        }
        public async Task<PayCode_Procedures_T_Dto> GetPayCodeProcedureByPIMS_ID(PayCode_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPayCodeProcedureByPIMS_ID(obj);
            return data;
        }

        public async Task<IEnumerable<PayCodeFiltersDto>> GetPayCodeSearchFilters(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj)
        {
            var data = await _repo.GetPayCodeSearchFilters(obj);
            return data;
        }


        public async Task<IEnumerable<PayCode_EPAL_Summary_Dto>> GetPayCodeEPALSummary(PayCode_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPayCodeEPALSummary(obj);
            return data;
        }

        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodesNotesFurtherConsiderationByPIMS_ID(string PAYC_HIERARCHY_KEY)
        {
            var data = await _repo.GetPayCodesNotesFurtherConsiderationByPIMS_ID(PAYC_HIERARCHY_KEY);
            return data;
        }


        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeHistorical(PayCode_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPayCodeHistorical(obj);
            return data;
        }
        public async Task<UpdateDto> UpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            var retVal = await _repo.UpdatePayCodeProcedure(obj);
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = retVal.Item1,
                ReturnObject = retVal.Item2
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = Common.Helper.AlreadyExistMessage;
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateTO.StatusID == 1)
            {
                updateTO.Message = "Record saved!";
                updateTO.StatusType = RetValStatus.Success.ToString();
            }
            else
            {
                updateTO.Message = "Record is not added!";
                updateTO.StatusType = RetValStatus.Error.ToString();
            }

            return updateTO;
        }

        public async Task<UpdateDto> PAYC_HISTORIC_INS_UPD_DRIVER(PayCode_Procedures_T_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.PAYC_HISTORIC_INS_UPD_DRIVER(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = Common.Helper.AlreadyExistMessage;
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Record saved!";
                updateTO.StatusType = RetValStatus.Success.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Error.ToString();
            }

            return updateTO;
        }

        public async Task<UpdateDto> PAYC_DELETE_DRIVER_PRC(PayCode_Procedures_T_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.PAYC_DELETE_DRIVER_PRC(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = Common.Helper.AlreadyExistMessage;
                updateTO.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Record saved!";
                updateTO.StatusType = RetValStatus.Success.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Error.ToString();
            }

            return updateTO;
        }

        public async Task<IEnumerable<PayCode_ChangeHistory_Dto>> GetPayCodeChangeHistory(PayCode_Procedures_Param_Dto obj)
        {
            var data = await _repo.GetPayCodeChangeHistory(obj);
            return data;
        }
        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk2()
        {
            var data = await _repo.GetAllPayCodeHierarchyCodesXwalk2();
            return data;
        }
        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk()
        {
            var data = await _repo.GetAllPayCodeHierarchyCodesXwalk();
            return data;
        }
    }
}
