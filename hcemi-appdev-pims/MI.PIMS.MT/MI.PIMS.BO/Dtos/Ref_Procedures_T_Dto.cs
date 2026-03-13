using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Ref_Procedures_T_Dto
    {
        public string PROC_CD { get; set; }
        public DateTime? PROC_CD_EFF_DT { get; set; }
        public DateTime? PROC_CD_EXP_DT { get; set; }
        public string PROC_CD_TYPE { get; set; }
        public string PROC_CD_DESC { get; set; }
        public int PROC_CD_AGE_MIN { get; set; }
        public int PROC_CD_AGE_MAX { get; set; }
        public string PROC_CD_GENDER_CD { get; set; }
        public string STATUS { get; set; }
    }

    public class Ref_Procedures_V_Dto
    {
        public string proc_cd { get; set; }
        public DateTime? proc_cd_eff_dt { get; set; }
        public DateTime? proc_cd_exp_dt { get; set; }
        public string proc_cd_type { get; set; }
        public string proc_cd_desc { get; set; }
        public int proc_cd_age_min { get; set; }
        public int proc_cd_age_max { get; set; }
        public string proc_cd_gender_cd { get; set; }
        public string status { get; set; }
    }
}
