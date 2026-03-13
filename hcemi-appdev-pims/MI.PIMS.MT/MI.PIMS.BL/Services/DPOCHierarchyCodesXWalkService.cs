using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Services
{
    public class DPOCHierarchyCodesXWalkService: IDPOCHierarchyCodesXWalkService
    {
        private readonly DPOCHierarchyCodesXWalkRepository _repository;
        public DPOCHierarchyCodesXWalkService(DPOCHierarchyCodesXWalkRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DPOC_HIERARCHY_CODES_XWALK_V_Dto>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}
