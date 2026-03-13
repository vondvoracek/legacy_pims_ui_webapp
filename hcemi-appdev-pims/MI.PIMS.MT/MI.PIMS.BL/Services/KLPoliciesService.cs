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
    public class KLPoliciesService : IKLPoliciesService
    {
        private readonly KLPoliciesRepository _repo;
        public KLPoliciesService(KLPoliciesRepository repo)
        {
            _repo = repo;   
        }
        public async Task<IEnumerable<KL_PoliciesDto>> GetKLPolicies(KL_PoliciesParamDto obj)
        {
            var retVal = await _repo.GetKLPolicies(obj);
            return retVal;
        }
    }
}
