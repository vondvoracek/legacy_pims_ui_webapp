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
    public class EPALAltrnt_Svc_CatService(EPALAltrnt_Svc_CatRepository _repository) : IEPAL_Altrnt_Svc_CatService
    {
        public async Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateCat(string p_text, string p_column_name, string p_proc_cds)
        {
            //string whereClause = " where 1=1 ";
            //if (!string.IsNullOrEmpty(p_proc_cds))
            //{
            //    // Split the values into individual elements
            //    string[] proc_cds = p_proc_cds.Split(',');

            //    // Constructing the WHERE clause dynamically
            //    var inProcCodes = string.Join(",", proc_cds.Select(v => $"'{v}'"));
            //    whereClause += !string.IsNullOrEmpty(inProcCodes) ? string.Format(" and proc_cd in ({0})", inProcCodes) : string.Empty;
            //}

            //var data = await _repository.GetAltrnt_Svc_CatsAsync(p_text, p_column_name, whereClause, "epal_procedures_t");
            var data = await _repository.GetAlternateCat(p_text, p_column_name, p_proc_cds);
            return data;
        }

        public async Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateSubCat(string p_text, string p_column_name, string p_proc_cds, string epal_altrnt_svc_cat)
        {
            //string whereClause = " where 1=1 ";
            //if (!string.IsNullOrEmpty(p_proc_cds))
            //{
            //    // Split the values into individual elements
            //    string[] proc_cds = p_proc_cds.Split(',');

            //    // Constructing the WHERE clause dynamically
            //    var inProcCodes = string.Join(",", proc_cds.Select(v => $"'{v}'"));
            //    whereClause += !string.IsNullOrEmpty(inProcCodes) ? string.Format(" and proc_cd in ({0})", inProcCodes) : string.Empty;
            //}
            ////epal_altrnt_svc_cat
            //whereClause += !string.IsNullOrEmpty(epal_altrnt_svc_cat) ? string.Format(" altrnt_svc_cat = {0}", epal_altrnt_svc_cat) : string.Empty;

            //var data = await _repository.GetAltrnt_Svc_CatsAsync(p_text, p_column_name, whereClause, "epal_procedures_t");
            var data = await _repository.GetAlternateSubCat(p_text, p_column_name, p_proc_cds, epal_altrnt_svc_cat);
            return data;
        }
    }
}
