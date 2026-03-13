using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IRefModifiersService
    {
        Task<IEnumerable<Ref_Modifier_Dto>> GetRefModifiersByFilter(string p_FILTER_TEXT);
    }
}
