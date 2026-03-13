using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class RefModifiersService: IRefModifiersService
    {
        private readonly RefModifiersRepository _repo;

        public RefModifiersService(RefModifiersRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Ref_Modifier_Dto>> GetRefModifiersByFilter(string p_FILTER_TEXT)
        {
            var data = await _repo.GetRefModifiers(p_FILTER_TEXT);
            IEnumerable<Ref_Modifier_Dto> modifier_Dtos = data.Select(item => new Ref_Modifier_Dto()
            {
                MODIFIER        = item.MODIFIER_CD,
                MOD_DESC        = item.MODIFIER_DESC,
                AMBULANCE_FLAG  = item.AMBULANCE_FLAG
            }).ToList();
            return modifier_Dtos;
        }
    }
}
