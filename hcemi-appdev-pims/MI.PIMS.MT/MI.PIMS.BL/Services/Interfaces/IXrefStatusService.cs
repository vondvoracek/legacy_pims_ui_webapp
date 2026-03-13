using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IXrefStatusService
    {
        Task<IEnumerable<Xref_Status_T_Dto>> GetPIMSXrefStatus();
    }
}
