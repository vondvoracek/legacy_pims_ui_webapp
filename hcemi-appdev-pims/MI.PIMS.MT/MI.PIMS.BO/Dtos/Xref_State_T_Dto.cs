using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Xref_State_T_Dto
    {
        public Int64 ID { get; set; }
        public string STATE_CD { get; set; }
        public string STATE_NAME { get; set; }
        public int ACTIVE { get; set; }
        public DateTime? LST_UPDT_DT { get; set; }
        public string LST_UPDT_BY { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CREATE_BY { get; set; }
    }
}
