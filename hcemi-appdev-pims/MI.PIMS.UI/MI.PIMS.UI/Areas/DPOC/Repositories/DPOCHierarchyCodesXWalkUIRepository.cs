using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Repositories
{
    public class DPOCHierarchyCodesXWalkUIRepository
    {
        private readonly IDPOCHierarchyCodesXWalkService _iDPOCHierarchyCodesXWalkService;
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        public DPOCHierarchyCodesXWalkUIRepository(IDPOCHierarchyCodesXWalkService iDPOCHierarchyCodesXWalkService, ICacheProvider cacheProvider, ICacheRepository cacheRepository)
        {
            _iDPOCHierarchyCodesXWalkService = iDPOCHierarchyCodesXWalkService;
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
        }

        private async Task<IEnumerable<DPOC_HIERARCHY_CODES_XWALK_V_Dto>> GetAll()
        {
            IEnumerable<DPOC_HIERARCHY_CODES_XWALK_V_Dto> dpocHierarchyCodesXwalk = null;
#if DEBUG
            dpocHierarchyCodesXwalk = await _iDPOCHierarchyCodesXWalkService.GetAll();
#else

            dpocHierarchyCodesXwalk = _cacheProvider.GetGlobalList<DPOC_HIERARCHY_CODES_XWALK_V_Dto>("dpocHierarchyCodesXwalk");
            if (dpocHierarchyCodesXwalk != null)
            {
                return dpocHierarchyCodesXwalk;
            }
            else
            {
                dpocHierarchyCodesXwalk = await _iDPOCHierarchyCodesXWalkService.GetAll();
                _cacheRepository.SetGlobal("dpocHierarchyCodesXwalk", dpocHierarchyCodesXwalk);
            }
#endif
            return dpocHierarchyCodesXwalk;
        }

        public async Task<IEnumerable<DPOC_Hierarchy_Codes_UI_Dto>> GetByColumn(string columnName)
        {
            var dpocHierarchyCodesXwalk = await GetAll();
            if (dpocHierarchyCodesXwalk == null)
            {
                return new List<DPOC_Hierarchy_Codes_UI_Dto>();
            }
            
            var retVal = dpocHierarchyCodesXwalk.Where(d =>
            {
                var property = d.GetType().GetProperty(columnName);
                if (property == null) return false;
                var value = property.GetValue(d, null);
                return value != null;
            }).Select(d =>
            {
                var property = d.GetType().GetProperty(columnName);
                if (property == null) return new DPOC_Hierarchy_Codes_UI_Dto();
                var value = property.GetValue(d, null);
                return new DPOC_Hierarchy_Codes_UI_Dto() { CD = (string)value };
            })
            .DistinctBy(d => d.CD).OrderBy(d => d.CD).ToList();           

            return retVal;
        }

        public async Task<IEnumerable<DPOC_Hierarchy_Codes_UI_Dto>> GetBusSegCd()
        {
            var dpocHierarchyCodesXwalk = await GetAll();
            if (dpocHierarchyCodesXwalk == null)
            {
                return new List<DPOC_Hierarchy_Codes_UI_Dto>();
            }

            var retVal = dpocHierarchyCodesXwalk.Select(d => new DPOC_Hierarchy_Codes_UI_Dto() { CD = d.epal_bus_seg_cd, Desc = d.epal_bus_seg_cd }).DistinctBy(d => d.CD).OrderBy(d => d.CD).ToList();
            return retVal;
        }

        public async Task<IEnumerable<DPOC_Hierarchy_Codes_UI_Dto>> GetEntityCd(string p_epal_bus_seg_cd)
        {
            var dpocHierarchyCodesXwalk = await GetAll();
            if (dpocHierarchyCodesXwalk == null)
            {
                return new List<DPOC_Hierarchy_Codes_UI_Dto>();
            }

            var retVal = dpocHierarchyCodesXwalk
            .Where(v => (string.IsNullOrEmpty(p_epal_bus_seg_cd) ||
                         p_epal_bus_seg_cd.Split(',').Select(s => s.Trim().ToLower()).Contains(v.epal_bus_seg_cd.ToLower())))
            .Select(v => new DPOC_Hierarchy_Codes_UI_Dto() { CD = v.epal_entity_cd, Desc = v.epal_entity_cd })
            .DistinctBy(d => d.CD)
            .OrderBy(v => v.CD)
            .ToList();
            return retVal;
        }

        public async Task<IEnumerable<string>> GetProductCd(string epal_bus_seg_cd, string epal_entity_cd, string epal_fund_arngmnt_cd)
        {
            var dpocHierarchyCodesXwalk = await GetAll();
            if (dpocHierarchyCodesXwalk == null)
            {
                return new List<string>();
            }

            var busSegList = epal_bus_seg_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            var entityList = epal_entity_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();            
            var fundingList = epal_fund_arngmnt_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var data = dpocHierarchyCodesXwalk
                .Where(d =>
                    !string.IsNullOrEmpty(d.epal_product_cd) &&
                    (busSegList == null || busSegList.Count == 0 || busSegList.Contains(d.epal_bus_seg_cd)) &&
                    (entityList == null || entityList.Count == 0 || entityList.Contains(d.epal_entity_cd)) &&                    
                    (fundingList == null || fundingList.Count == 0 || fundingList.Contains(d.epal_fund_arngmnt_cd))
                )
                .Select(x => x.epal_product_cd)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return data;
        }

        public async Task<IEnumerable<string>> GetFundingArrangement(string epal_bus_seg_cd, string epal_entity_cd, string epal_product_cd)
        {
            var dpocHierarchyCodesXwalk = await GetAll();
            if (dpocHierarchyCodesXwalk == null)
            {
                return new List<string>();
            }

            var busSegList = epal_bus_seg_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            var entityList = epal_entity_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            var productList = epal_product_cd?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var data = dpocHierarchyCodesXwalk
                .Where(d =>
                    !string.IsNullOrEmpty(d.epal_fund_arngmnt_cd) &&
                    (busSegList == null || busSegList.Count == 0 || busSegList.Contains(d.epal_bus_seg_cd)) &&
                    (entityList == null || entityList.Count == 0 || entityList.Contains(d.epal_entity_cd)) &&
                    (productList== null || productList.Count == 0 || productList.Contains(d.epal_product_cd))
                )
                .Select(x => x.epal_fund_arngmnt_cd)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return data;
        }
    }
}
