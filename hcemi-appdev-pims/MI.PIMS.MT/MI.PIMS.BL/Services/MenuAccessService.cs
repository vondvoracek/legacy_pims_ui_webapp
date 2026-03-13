using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class MenuAccessService : IMenuAccessService
    {
        private readonly MenuAccessRepository _repo;

        public MenuAccessService(MenuAccessRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MenuAccessDto>> GetMenuAccess(string MS_ID)
        {
            var data = await _repo.GetMenuAccess(MS_ID);
            return data;
        }

    }
}
