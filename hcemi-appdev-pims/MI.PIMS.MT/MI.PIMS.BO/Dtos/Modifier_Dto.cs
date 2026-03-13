using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Ref_Modifier_V_Dto
    {
        public string MODIFIER_CD { get; set; }
        public string MODIFIER_DESC { get; set; }
        public string AMBULANCE_FLAG { get; set; }
    }

    public class Ref_Modifier_Dto
    {        
        public string MODIFIER { get; set; }
        public string MOD_DESC { get; set; }
        public string AMBULANCE_FLAG { get; set; }
    }

    public class EPAL_Procedures_Modifier_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string EPAL_VER_EFF_DT { get; set; }
        public string MOD_SYS_SEQ { get; set; }
        public string MODIFIER { get; set; }
        public string MOD_DESC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }
}
