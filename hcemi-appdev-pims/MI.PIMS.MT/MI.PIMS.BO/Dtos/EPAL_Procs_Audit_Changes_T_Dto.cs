using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procs_Audit_Changes_T_Dto
    {
        public string AUDIT_EVENT_TYPE { get; set; }
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_EFF_DT { get; set; }
        public string USER_ID { get; set; }
        public DateTime? EVENT_TMSTMP { get; set; }
        public string EVENT_SQL { get; set; }
        public string CHANGE_REQ_ID { get; set; }
        public string CHANGE_DESC { get; set; }
    }
}
