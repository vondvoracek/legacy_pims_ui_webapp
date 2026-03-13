using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Providers
{
    public interface IRoleProvider
    {
        Task<ICollection<string>> GetUserRolesAsync(string role);
    }
}
