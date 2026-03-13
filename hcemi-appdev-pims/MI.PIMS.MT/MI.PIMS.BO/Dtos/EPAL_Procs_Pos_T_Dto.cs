using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procs_Pos_T_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_EFF_DT { get; set; }
        public int POS_SYS_SEQ { get; set; }
        public string ALLWD_PLC_OF_SVC { get; set; }
    }
}
