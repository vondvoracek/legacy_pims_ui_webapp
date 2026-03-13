using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IDPOCHierarchyCodesXWalkService
    {
        Task<IEnumerable<DPOC_HIERARCHY_CODES_XWALK_V_Dto>> GetAll();
    }
}
