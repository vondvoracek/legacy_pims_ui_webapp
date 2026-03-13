using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Additional_Req_His_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_INV_NOTES { get; set; }
        public string DPOC_ADDTNL_RQRMNTS { get; set; }
    }
}
