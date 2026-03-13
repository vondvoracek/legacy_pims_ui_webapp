using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procs_Prog_Mgd_By_V
    {
        public string? EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public int PMB_SYS_SEQ { get; set; }
        public DateTime? PMB_EFF_DT { get; set; }
        public DateTime? PMB_EXP_DT { get; set; }
        public string PROG_MGD_BY { get; set; }
        public string DELEGATED_UM { get; set; }
        public string INT_EXT_CD { get; set; }
        public string PMB_BASED_ON_DX_IND { get; set; }
        public string PMB_BASED_ON_ST_APP_IND { get; set; }
        public int PMB_BASED_ON_AGE_MIN { get; set; }
        public int PMB_BASED_ON_AGE_MAX { get; set; }
    }
}
