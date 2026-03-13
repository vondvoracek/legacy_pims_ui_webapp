using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class UserAccess_Hist_Dto
    {
        public string MODULE_NAME { get; set; }
        public string USERACTION { get; set; }
        public string USERSELECTION { get; set; }
        public DateTime LST_UPDT_DT { get; set; }
        public string LST_UPDT_BY { get; set; }
        public int PAGE_ID { get; set; }
    }
}
