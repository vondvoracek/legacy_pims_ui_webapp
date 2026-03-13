using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{

    public interface IManageGuidelinesService
    {
        Task<IEnumerable<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>> GetGuidelinesByParams(
            string proc_cd, string iq_reference, string iq_gdln_id, string iq_gdln_version);

        Task<List<Ref_Procedures_V_Dto>> GetActiveProcedureCodes(string proc_cds);
    }

}
