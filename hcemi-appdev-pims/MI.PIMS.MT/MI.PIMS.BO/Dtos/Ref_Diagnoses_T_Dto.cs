using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Ref_Diagnoses_T_Dto
    {
        public string DIAG_CD { get; set; }
        public DateTime? DIAG_CD_EFF_DT { get; set; }
        public DateTime? DIAG_CD_EXP_DT { get; set; }
        public string DIAG_CD_TYPE { get; set; }
        public string DIAG_CD_DESC { get; set; }
        public string DIAG_CD_GENDER_CD { get; set; }
        public string STATUS { get; set; }
    }

    public class EPAL_Procs_Diagnoses_V_Dto
    {
       public string LIST_NAME { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
        public string PROG_MGD_BY { get; set; }
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime EPAL_VER_EFF_DT { get; set; }
    }

    public class REF_ALL_DIAG_CD_LISTS_V
    {
        public string LIST_NAME { get; set; }
    }

    public class DPOC_REF_ALL_DIAG_CD_LISTS_V
    {
        public string display_listname { get; set; }
        public string diag_cd { get; set; }
        public string diag_cd_desc { get; set; }
        public DateTime? diag_cd_eff_dt { get; set; }
        public DateTime? diag_cd_exp_dt { get; set; }
    }

    public class DIAG_LIST_NAME_CNT_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public int LIST_NAME_CNT { get; set; }
    }

}
