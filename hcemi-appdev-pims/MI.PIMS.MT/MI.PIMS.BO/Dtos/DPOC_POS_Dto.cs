using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_POS_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string POS_IQ_GDLN_ID { get; set; }
        public string PLC_OF_SVC_CD { get; set; }
        public string PLC_OF_SVC_DESC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }

    public class POS_APPL_Dto
    {
        public string POS_APPL { get; set; }
        public string POS_APPL_DESC { get; set; }
    }
    public class POS_INCL_EXCL_CD_Dto
    {
        public string POS_INCL_EXCL_CD { get; set; }
        public string POS_INCL_EXCL_DESC { get; set; }
    }
    public class PLC_OF_SVC_CD_Dto
    {
        public string PLC_OF_SVC_CD { get; set; }
        public string PLC_OF_SVC_DESC { get; set; }

    }
}
