using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IRefProceduresService
    {
        Task<IEnumerable<Ref_Procedures_T_Dto>> GetRefProceduresByFilter(string p_FILTER_TEXT);
    }
}
