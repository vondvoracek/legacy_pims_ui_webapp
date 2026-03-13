using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procs_Sos_T_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_EFF_DT { get; set; }
        public int? SOS_SYS_SEQ { get; set; }
        public DateTime? SOS_EFF_DT { get; set; }
        public DateTime? SOS_EXP_DT { get; set; }
        public string SOS_DPLMNT_PHASE { get; set; }
        public string SOS_TYPE { get; set; }
        public string SOS_SITE_IND { get; set; }
        public string SOS_MED_NEC_IND { get; set; }
        public string SOS_SOFT_STEERAGE_IND { get; set; }
        public string SOS_URG_APP_IND { get; set; }
        public string SOS_URG_CAT_MDLTY { get; set; }
        public string SOS_PLATFORM { get; set; }
    }

    public class SOS_Site_Ind_Dto
    {
        public string VV_CD { get; set; }
        public string VV_CD_DESC { get; set; }
    }
}
