using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Providers
{
    public class RoleProvider: IRoleProvider
    {
        public RoleProvider(){}

        public async Task<ICollection<string>> GetUserRolesAsync(string roles)
        {
            ICollection<string> result = new string[0];

            // Here, John is a basic user and Arnold an admin user
            // Feel free to load roles from any source you like.
     
            if (!string.IsNullOrEmpty(roles))
            {
                result = roles.Split(',');              
            }

            return await Task.FromResult(result);
        }
    }
}
