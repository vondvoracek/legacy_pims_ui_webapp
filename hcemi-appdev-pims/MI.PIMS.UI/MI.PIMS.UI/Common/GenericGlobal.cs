using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System.Collections.Generic;

namespace MI.PIMS.UI.Common
{
    public class GenericGlobal
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IPIMSValidValuesService _pIMSValidValuesService;

        public GenericGlobal(ICacheRepository cacheRepository,
                        ICacheProvider cacheProvider,
                        IPIMSValidValuesService pIMSValidValuesService) {
            _cacheRepository = cacheRepository;
            _cacheProvider = cacheProvider;
            _pIMSValidValuesService = pIMSValidValuesService;
        }

        public IEnumerable<State_CD_Dto> States
        {
            get
            {
                //Set STATES in cache
                IEnumerable<State_CD_Dto> states = null;
#if DEBUG
                states = _pIMSValidValuesService.GetStateCDs().Result;
#else
                states = _cacheProvider.GetGlobal<IEnumerable<State_CD_Dto>>("states");                
                if (states == null)
                {
                    states = _pIMSValidValuesService.GetStateCDs().Result;
                    _cacheRepository.SetGlobal<IEnumerable<State_CD_Dto>>("states", states);
                }
#endif
                return states;
            }
        }
    }
}
