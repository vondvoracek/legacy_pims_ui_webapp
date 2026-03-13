using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Inv_Gdln_Appl_To_States_T_Dto
    {
        public string DPOC_HIERARCHY_KEY { get;set; }
        public DateTime? DPOC_VER_EFF_DT { get;set; }
        public string DPOC_PACKAGE { get;set; }
        public string DPOC_RELEASE { get; set; }
        public string ATS_IQ_GDLN_ID { get; set; }
        public int GDLN_DTQ_SYS_SEQ { get; set; }
        public string STATE_CD { get; set; }
        public string STATE_NAME { get; set; }
        public string STATE_MANDATED_IND { get; set; }
        public string ATS_INCL_EXCL_CD { get; set; }
        public string ATS_INCL_EXCL_CD_DESC { get; set; }
        public DateTime? ATS_EFF_DT { get; set; }
        public DateTime? ATS_EXP_DT { get; set; }
        public string ATS_ISSUE_GOV { get; set; }
        public string DPOC_VER_NUM { get; set; }
    }
}
