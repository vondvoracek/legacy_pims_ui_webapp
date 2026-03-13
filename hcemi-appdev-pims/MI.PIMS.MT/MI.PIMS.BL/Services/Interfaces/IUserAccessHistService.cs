using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IUserAccessHistService
    {
        Task<UpdateDto> Add(UserAccess_Hist_Dto obj);
        Task<UpdateDto> Add(string module_name, string useraction, string userselection, int page_id, string lst_updt_by = "");
    }
}
