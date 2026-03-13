using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procs_Appl_To_States_T_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime EPAL_EFF_DT { get; set; }
        public int PROC_ATS_SYS_SEQ { get; set; }
        public string STATE_CD { get; set; }
        public string STATE_MANDATED_IND { get; set; }
    }
}
