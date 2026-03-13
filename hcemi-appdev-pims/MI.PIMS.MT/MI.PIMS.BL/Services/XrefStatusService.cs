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
    public class XrefStatusService : IXrefStatusService
    {
        private readonly XrefStatusRepository _repo;

        public XrefStatusService(XrefStatusRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Xref_Status_T_Dto>> GetPIMSXrefStatus()
        {
            var data = await _repo.GetPIMSXrefStatus();
            return data;
        }
    }
}
