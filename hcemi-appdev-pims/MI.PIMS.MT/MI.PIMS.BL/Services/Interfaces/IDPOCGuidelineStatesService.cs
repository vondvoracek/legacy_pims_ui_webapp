using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCGuidelineStatesService
    {
        Task<IEnumerable<DPOC_Inv_Gdln_Appl_To_States_T_Dto>> Get(DPOC_Gdln_Param_Dto obj);
    }
}
