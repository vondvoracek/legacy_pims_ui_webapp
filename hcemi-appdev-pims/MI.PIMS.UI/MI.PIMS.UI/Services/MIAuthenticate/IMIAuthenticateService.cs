using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.Admin.Models;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.MIAuthenticate
{
    public interface IMIAuthenticateService
    {
        Task<UserInfo_T_Dto> GetUserInfo(UserSearchParam param);
        UserInfo_T_Dto UserInfo_T_Dto(string ms_id);
        Task<UserInfo_T_Dto> UserInfo_T_Dto2(string ms_id);
    }
}
