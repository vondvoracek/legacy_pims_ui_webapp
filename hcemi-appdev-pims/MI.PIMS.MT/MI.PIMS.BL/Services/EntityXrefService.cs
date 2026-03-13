using MI.PIMS.BL.Repositories;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public class EntityXrefService : IEntityXrefService
    {
        private readonly Entity_XrefRepository _repo;

        public EntityXrefService(Entity_XrefRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<EntityXrefDto>> GetEntityXref()
        {
            var data = await _repo.GetEntityXref();
            return data;
        }
    }
}
