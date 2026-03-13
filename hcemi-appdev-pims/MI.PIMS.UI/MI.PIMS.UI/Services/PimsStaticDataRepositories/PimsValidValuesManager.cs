using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Services.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Services.PimsStaticDataRepositories
{
    public class PimsValidValuesManager
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;

        private readonly ILoggerService _loggerService;

        private readonly IPIMSValidValuesService _pIMSValidValuesService;        
        public PimsValidValuesManager(ICacheProvider cacheProvider, IPIMSValidValuesService pIMSValidValuesService, ILoggerService loggerService,
            ICacheRepository cacheRepository)
        {
            _cacheProvider = cacheProvider;                
            _pIMSValidValuesService = pIMSValidValuesService;
            _cacheRepository = cacheRepository;
            _loggerService = loggerService;
        }

        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues()
        {
            IEnumerable<PIMS_Valid_Values_V_Dto> pims_valid_values = null;
#if DEBUG
            pims_valid_values = await _pIMSValidValuesService.GetPIMSValidValues();
#else
            pims_valid_values = _cacheProvider.GetGlobalList<PIMS_Valid_Values_V_Dto>("pims_valid_values");
#endif
            return await Task.FromResult(pims_valid_values);
        }

        public async Task<IEnumerable<PIMS_Valid_Values_Dto>> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {            
            var data = _cacheProvider.GetGlobalList<PIMS_Valid_Values_V_Dto>("pims_valid_values");

            if (data == null)
            {
                _loggerService.Warn("pims_valid_values is null from caching for p_VV_SET_NAME = " + p_VV_SET_NAME);
                data = await _pIMSValidValuesService.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
                _cacheRepository.SetGlobal("pims_valid_values", data);
            }

            IEnumerable<PIMS_Valid_Values_Dto> pIMS_Valid_Values_Dtos = null;
            pIMS_Valid_Values_Dtos = data.Where(x => x.VV_SET_NAME == p_VV_SET_NAME &&
                                (p_BUS_SEG_CD == null || p_BUS_SEG_CD.Trim().Length == 0 || p_BUS_SEG_CD.Split(',').Contains(x.BUS_SEG_CD)))
                .Select(x => new { x.VV_CD, x.VV_CD_DESC }).Distinct()
                .Select(x => new PIMS_Valid_Values_Dto() { VV_CD = x.VV_CD, VV_CD_DESC = x.VV_CD_DESC }).Distinct().ToList();

            return pIMS_Valid_Values_Dtos;
        }
    }
}
