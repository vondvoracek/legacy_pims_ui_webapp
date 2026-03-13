using MI.PIMS.BL.Common;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MI.PIMS.BL.Services
{
    public class DPOCService(DPOCRepository _repo) : IDPOCService
    {
        public async Task<IEnumerable<DPOC_Inventories_V_Dto>> GetDPOCSearch(DPOC_Param_Dto obj)
        {
            var data = await _repo.GetDPOCSearch(obj);
            return data;
        }

        public async Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesByPIMS_ID(DPOC_Inventories_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInventoriesByPIMS_ID(obj);

            //Check if IsCurrent record if a effective date is passed.
            if (data.DPOC_VER_EFF_DT != null)
            {
                IsCurrentRecordDto obj2 = new IsCurrentRecordDto();
                {
                    obj2.p_PIMS_ID = data.DPOC_HIERARCHY_KEY;
                    obj2.p_PIMS_VER_EFF_DT = data.DPOC_VER_EFF_DT;
                    obj2.p_MODULE_NAME = "DPOC";
                };
                var data3 = await _repo.GetPIMSIsCurrentRecord(obj2);

                if (data3 != null)
                {
                    data.IS_CURRENT = data3.IS_CURRENT;
                }
            }
            else if (data.DPOC_VER_EFF_DT == null)
            {
                data.IS_CURRENT = "Y";
            }


            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_V_Dto>> GetDPOCInvGdlnRulesByPIMSID(DPOC_Inv_Gdln_Rules_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInvGdlnRulesByPIMSID(obj);
            return data;
        }

        public async Task<IEnumerable<DPOC_INV_DTQS_V_Dto>> GetDPOCInvDTQSByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInvDTQSByPIMSID(obj);
            return data;
        }        
        public async Task<IEnumerable<DPOC_INV_DTQS_TGT_V>> GetDPOCInvDTQTGTsByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInvDTQTGTsByPIMSID(obj);
            return data;
        }
        public async Task<IEnumerable<DPOC_INV_DTQS_HOLDING_V>> GetDPOCInvDTQHoldingsByPIMSID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInvDTQHoldingsByPIMSID(obj);
            return data;
        }
        public async Task<IEnumerable<DPOC_Inventories_V_Hist_Dto>> GetDPOCInventoriesHistByPIMS_ID(DPOC_PIMS_ID_Param_Dto obj)
        {
            var data = await _repo.GetDPOCInventoriesHistByPIMS_ID(obj);
            return data;
        }

        public async Task<IEnumerable<DPOC_ChangeHistory_Dto>> GetDPOCChangeHistoryByPIMSID(DPOC_PIMS_ID_Param_Dto param)
        {
            var data = await _repo.GetDPOCChangeHistoryByPIMSID(param);
            return data;
        }

        public async Task<IEnumerable<DPOC_Inv_Dtqs_V_Dto>> GetDPOC_Inv_Dtqs_V()
        {
            var data = await _repo.GetDPOC_Inv_Dtqs_V();
            return data;
        }
        public async Task<string> GetDPOCIDExistStatus(string dpoc_hierarchy_key, string p_dpoc_package)
        {
            var data = await _repo.GetDPOCIDExistStatus(dpoc_hierarchy_key, p_dpoc_package);
            return data;    
        }

        public async Task<UpdateDto> DPOC_INS_UPD_DRIVER_PRC(DPOC_Ins_Upd_Pkg_Param obj)
        {
            UpdateDto updateDto = new UpdateDto()
            {
                StatusID = await _repo.DPOC_INS_UPD_DRIVER_PRC(obj)
            };

            if (updateDto.StatusID == 0)
            {
                updateDto.Message = Common.Helper.AlreadyExistMessage;
                updateDto.StatusType = RetValStatus.Warning.ToString();
            }
            else if (updateDto.StatusID == -1)
            {
                updateDto.Message = "Record is not added!";
                updateDto.StatusType = RetValStatus.Error.ToString();
            }
            else if (updateDto.StatusID == 2)
            {
                updateDto.Message = "Record partially updated!";
                updateDto.StatusType = RetValStatus.Warning.ToString();
            }
            else 
            {
                updateDto.Message = Common.Helper.AddMessage;
                updateDto.StatusType = RetValStatus.Success.ToString();
            }

            return updateDto;
        }
        public async Task<UpdateDto> DPOC_DELETE_DRIVER_PRC(DPOC_Delete_Pkg_Param obj)
        {
            int statusId = await _repo.DPOC_DELETE_DRIVER_PRC(obj);

            var (message, statusType) = statusId switch
            {
                -1 => ("Record is not deleted!", RetValStatus.Error.ToString()),
                2 => ("Error occurred while deleting the record!\nRecord has not been deleted!", RetValStatus.Warning.ToString()),
                _ => (Common.Helper.RecordDeleteMessage, RetValStatus.Success.ToString())
            };

            var updateTO = new UpdateDto
            {
                StatusID = statusId,
                Message = message,
                StatusType = statusType
            };

            return updateTO;
        }
        public async Task<IEnumerable<DPOC_Additional_Req_His_Dto>> GetPIMSAdditionalInfoHistory(string dpoc_hierarchy_key)
        {
            var data = await _repo.GetPIMSAdditionalInfoHistory(dpoc_hierarchy_key);
            return data;
        }

        public async Task<DPOC_Inventories_V_Dto> GetDPOCInventoriesLstUpdtRecordByPIMS_ID(string dpoc_hierarchy_key, string p_dpoc_package)
        {
            var data = await _repo.GetDPOCInventoriesLstUpdtRecordByPIMS_ID(dpoc_hierarchy_key, p_dpoc_package);
            return data;
        }

        public async Task<IEnumerable<DPOC_POS_Dto>> GetPOS(DPOC_PIMS_ID_Param_Dto dPOC_PIMS_ID_Param_Dto)
        {
            var data = await _repo.GetPOS(dPOC_PIMS_ID_Param_Dto);
            return data;
        }

        public async Task<IEnumerable<string>> GetDPOC_SOS_PROVIDER_TIN_EXCL()
        {
            var data = await _repo.GetDPOC_SOS_PROVIDER_TIN_EXCL();
            return data;
        }

        public async Task<IEnumerable<string>> GetDPOCRelease()
        {
            var data = await _repo.GetDPOCRelease();
            return data;
        }
        public async Task<IEnumerable<string>> GetDPOCPackage()
        {
            var data = await _repo.GetDPOCPackage();
            return data;
        }
        
    }
}
 