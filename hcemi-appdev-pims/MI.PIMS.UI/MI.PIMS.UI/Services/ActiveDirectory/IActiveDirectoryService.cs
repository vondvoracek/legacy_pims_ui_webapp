using MI.PIMS.BO.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.ActiveDirectory
{
    public interface IActiveDirectoryService
    {
        Task<IEnumerable<ActiveDirectoryUserDto>> GetUsers(string ms_id, string last_name, string first_name, string addreadlimit);
        Task<IEnumerable<ActiveDirectoryUserDto>> GetActiveDirectoryUser(string ms_id, string last_name, string first_name);
        Task<bool> FindUserInGroup();
    }
}
