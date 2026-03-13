using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services
{
    public class StaticDataCachingService: IStaticDataCachingService
    {
        private readonly ICacheRepository _cacheRepo;
        private readonly ICacheProvider _cacheProvider;
        
        private readonly PIMSValidValuesRepository _pIMSValidValuesRepository;
        private readonly IPayCodeProceduresService _payCodeProceduresService;

        private readonly ILoggerService _logger;
        public StaticDataCachingService(ICacheRepository cacheRepo, PIMSValidValuesRepository pIMSValidValuesRepository, ICacheProvider cacheProvider,
            ILoggerService loggerService, IPayCodeProceduresService payCodeProceduresService) 
        {
            _cacheRepo = cacheRepo;
            _pIMSValidValuesRepository = pIMSValidValuesRepository;
            _cacheProvider = cacheProvider;
            _logger = loggerService;
            _payCodeProceduresService = payCodeProceduresService;
        }

        public void Set()
        {
#if DEBUG
            return;
#endif
            try
            {
                // caching pims_valid_values
                var pims_v_v = _cacheProvider.GetGlobalList<PIMS_Valid_Values_V_Dto>("pims_valid_values");
                if (pims_v_v == null)
                {
                    pims_v_v = _pIMSValidValuesRepository.GetPIMSValidValues().Result;
                    _cacheRepo.SetGlobal("pims_valid_values", pims_v_v);
                }

                // caching pims_hierarchy_codes_xwalk
                var pims_hier_codes = _cacheProvider.GetGlobalList<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("pims_hier_codes");
                if (pims_hier_codes == null)
                {
                    pims_hier_codes = _payCodeProceduresService.GetAllPayCodeHierarchyCodesXwalk().Result;
                    _cacheRepo.SetGlobal("pims_hier_codes", pims_hier_codes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
}
