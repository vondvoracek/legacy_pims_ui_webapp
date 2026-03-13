using MI.PIMS.BO.Dtos;
//using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> FindUserInGroup(string ms_id);
        Task<IEnumerable<ActiveDirectoryUserDto>> GetActiveDirectoryUser(string ms_id, string last_name, string first_name);
        Task<ActiveDirectoryUserTO> ActiveDirectoryUserTO(string ms_id);
    }
}
