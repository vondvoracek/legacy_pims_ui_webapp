using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Entities;

namespace MI.PIMS.BL.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly UserInfoRepository _repo;
        public UserInfoService(UserInfoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AppRole_T_Dto>> GetRoleUserAssignByMSID(string ms_id)
        {
            var data = await _repo.GetRoleUserAssignByMSID(ms_id);
            return data;
        }


        public async Task<UpdateDto> DeleteRoleUserAssign(DeleteRoleUserAssignParam_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.DeleteRoleUserAssign(obj)
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

        public async Task<UpdateDto> AddRoleUserAssign(AppRoleUserAssign_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.AddRoleUserAssign(obj)
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


        public async Task<UserInfo_T_Dto> Get(string MS_ID)
        {
            var data = await _repo.Get(MS_ID);
            return data;
        }

        public async Task<IEnumerable<UserInfo_T_Dto>> GetUsers(string ms_id
                                                                   , string fname
                                                                   , string lname
                                                                   , string approleid
                                                                   , string active
                                                                   , string pims_user)
        {
            var data = await _repo.GetUsers(ms_id, fname, lname, approleid, active, pims_user);
            return data;
        }

        public async Task<IEnumerable<ActiveDirectoryUserDto>> GetETL_ADExportUsersByParam(string ms_id, string fname, string lname)
        {
            var data = await _repo.GetETL_ADExportUsersByParam(ms_id, fname, lname);
            return data;
        }

        public async Task<UpdateDto> UpdateUserInfoAppRole(UserInfo_T_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto()
            {
                StatusID = await _repo.UpdateUserInfoAppRole(obj)
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

        public async Task<UpdateDto> TogglePIMSUserStatus(TogglePIMSUserStatusParam_Dto obj)
        {
            UpdateDto updateTO = new()
            {
                StatusID = await _repo.TogglePIMSUserStatus(obj)
            };
            if (updateTO.StatusID == 0)
            {
                updateTO.Message = "Active status has been updated!";
                updateTO.StatusType = RetValStatus.Success.ToString();
            }
            else if (updateTO.StatusID == -1)
            {
                updateTO.Message = "Error while Toggling User status";
                updateTO.StatusType = RetValStatus.Error.ToString();
            }
            else
            {
                updateTO.Message = Common.Helper.AddMessage;
                updateTO.StatusType = RetValStatus.Error.ToString();
            }

            return updateTO;
        }

        public async Task<UpdateDto> Add(UserInfo_AddDto obj)
        {            
            UpdateDto updateTO = new UpdateDto();
            try
            {
                var userInfo = await Get(obj.P_MS_ID);

                if (userInfo == null)
                {
                    updateTO.StatusID = await _repo.Add(obj);
                    updateTO.Message = Common.Helper.AddMessage;
                    updateTO.StatusType = RetValStatus.Success.ToString();
                }
                else
                {
                    updateTO.StatusID = 0;
                    updateTO.Message = Common.Helper.AlreadyExistMessage;
                    updateTO.StatusType = RetValStatus.Warning.ToString();
                }
            }
            catch(Exception ex)
            {
                updateTO.Message = "Error occured while saving the record";
                updateTO.StatusType = RetValStatus.Error.ToString();
                updateTO.ReturnObject = ex;
            }
            
            return updateTO;
        }

        public async Task<UpdateDto> Delete(DeleteUserInfoTParam_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto();

            updateTO.StatusID = await _repo.Delete(obj);
            
            return updateTO;
        }
    }
}
