using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IKLPoliciesService
    {
        Task<IEnumerable<KL_PoliciesDto>> GetKLPolicies(KL_PoliciesParamDto kL_PoliciesParamDto);
    }
}
