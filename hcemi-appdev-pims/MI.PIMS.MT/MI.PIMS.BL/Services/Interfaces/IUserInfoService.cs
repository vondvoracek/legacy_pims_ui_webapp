using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IUserInfoService
    {
        Task<IEnumerable<AppRole_T_Dto>> GetRoleUserAssignByMSID(string ms_id);
        Task<UpdateDto> DeleteRoleUserAssign(DeleteRoleUserAssignParam_Dto obj);
        Task<UpdateDto> AddRoleUserAssign(AppRoleUserAssign_Dto obj);
        Task<UserInfo_T_Dto> Get(string MS_ID);
        Task<IEnumerable<UserInfo_T_Dto>> GetUsers(string ms_id, string fname, string lname , string approleid, string active, string pims_user);
        Task<UpdateDto> Add(UserInfo_AddDto obj);
        Task<UpdateDto> Delete(DeleteUserInfoTParam_Dto obj);
        Task<UpdateDto> UpdateUserInfoAppRole(UserInfo_T_Dto obj);        
        Task<UpdateDto> TogglePIMSUserStatus(TogglePIMSUserStatusParam_Dto obj);
        Task<IEnumerable<ActiveDirectoryUserDto>> GetETL_ADExportUsersByParam(string ms_id, string fname, string lname);
    }
}
