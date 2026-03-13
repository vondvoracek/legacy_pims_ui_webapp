using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class XrefStateService : IXrefStateService
    {
        private readonly XrefStateRepository _repo;

        public XrefStateService(XrefStateRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Xref_State_T_Dto>> GetPIMSXrefState()
        {
            var data = await _repo.GetPIMSXrefState();
            return data;
        }
    }
}
