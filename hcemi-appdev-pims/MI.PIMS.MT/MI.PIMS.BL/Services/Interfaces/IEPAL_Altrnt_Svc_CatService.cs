using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IEPAL_Altrnt_Svc_CatService
    {
        Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateCat(string p_text, string p_column_name, string p_proc_cds);
        Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateSubCat(string p_text, string p_column_name, string p_proc_cds, string p_epal_altrnt_svc_cat);
    }
}
