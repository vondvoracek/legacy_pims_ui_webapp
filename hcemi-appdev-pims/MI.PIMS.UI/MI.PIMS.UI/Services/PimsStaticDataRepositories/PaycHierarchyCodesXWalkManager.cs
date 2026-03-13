using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Common;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.PimsStaticDataRepositories
{
    public class PaycHierarchyCodesXWalkManager
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;

        private readonly IPayCodeProceduresService _payCodeProceduresService;

        public PaycHierarchyCodesXWalkManager(ICacheProvider cacheProvider, ICacheRepository cacheRepository, IPayCodeProceduresService payCodeProceduresService)
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _payCodeProceduresService = payCodeProceduresService;
        }

        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk()
        {
            IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto> pims_hier_codes = null;
#if DEBUG
            pims_hier_codes = await _payCodeProceduresService.GetAllPayCodeHierarchyCodesXwalk();
#else
            pims_hier_codes = _cacheProvider.GetGlobalList<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("pims_hier_codes");
#endif
            return await Task.FromResult(pims_hier_codes);
        }

        public async Task<IEnumerable<PayCodeHierarchyCodesDto>> GetPayCodeHierarchyCodesXwalk(string p_epal_bus_seg_cd, string p_column_name, string p_epal_entity_cd)
        {
            var data = _cacheProvider.GetGlobalList<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("pims_hier_codes");

            if (data == null)
            {
                data = await _payCodeProceduresService.GetAllPayCodeHierarchyCodesXwalk();
                _cacheRepository.SetGlobal("pims_hier_codes", data);
            }

            IEnumerable<PayCodeHierarchyCodesDto> payCodeFiltersDtos = null;
            
            switch (p_column_name.ToLower())
            {                //data.Select(x => x.PAYC_BUS_SEG_CD).Distinct().OrderBy(x => x).Select(x => new PayCodeHierarchyCodesDto() { COLUMN_NAME = x }).ToList()
                case "epal_bus_seg_cd":
                    payCodeFiltersDtos = data.Where(x => p_epal_bus_seg_cd == null || p_epal_bus_seg_cd.ToLower().Split(',').Contains(p_epal_bus_seg_cd.ToLower()))
                        .Select(x => x.PAYC_BUS_SEG_CD).Distinct().OrderBy(x => x).Select(x => new PayCodeHierarchyCodesDto() { COLUMN_NAME = x }).ToList();
                    break;
                case "epal_entity_cd":
                    payCodeFiltersDtos = data.Where(x => p_epal_bus_seg_cd == null || p_epal_bus_seg_cd.ToLower().Split(',').Contains(p_epal_bus_seg_cd.ToLower()))
                        .Select(x => x.PAYC_ENTITY_CD).Distinct().OrderBy(x => x).Select(x => new PayCodeHierarchyCodesDto() { COLUMN_NAME = x }).ToList();
                    break;
                case "epal_plan_cd":
                    payCodeFiltersDtos = data.Where(x => p_epal_bus_seg_cd == null || p_epal_bus_seg_cd.ToLower().Split(',').Contains(p_epal_bus_seg_cd.ToLower()))
                        .Select(x => x.PAYC_PLAN_CD).Distinct().OrderBy(x => x).Select(x => new PayCodeHierarchyCodesDto() { COLUMN_NAME = x }).ToList();
                    break;
                case "epal_product_cd":
                    payCodeFiltersDtos = data.Where(x => (p_epal_bus_seg_cd == null || p_epal_bus_seg_cd.ToLower().Split(',').Contains(p_epal_bus_seg_cd.ToLower())) &&
                                    (p_epal_entity_cd == null || p_epal_entity_cd.ToLower().Split(',').Contains(p_epal_bus_seg_cd.ToLower())))
                        .Select(x => x.PAYC_PRODUCT_CD).Distinct().OrderBy(x => x).Select(x => new PayCodeHierarchyCodesDto() { COLUMN_NAME = x }).ToList();
                    break;
                default:
                    break;
            }

            return payCodeFiltersDtos;
        }
    }
}
