using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_INV_PLCY_LKP_V_Dto
    {
        public string DPOC_BUS_SEG_CD { get; set; }
        public string DPOC_ENTITY_CD { get; set; }
        public string PROC_CD { get; set; }
        public string PLCY_BUS_SEG_CD { get; set; }
        public string PLCY_ENTITY_CD { get; set; }
        public string PLCY_TYPE_CD { get; set; }
        public string PLCY_POLICY_ID { get; set; }
        public string PLCY_NM { get; set; }
        public string PLCY_EFF_DT { get; set; }
        public string PLCY_ARCH_DT { get; set; }
    }

    public class DPOC_INV_PLCY_LKP_Param
    {
        public string P_DPOC_BUS_SEG_CD { get; set; }
        public string P_DPOC_ENTITY_CD { get; set; }
        public string P_PROC_CD { get; set; }
    }
}
