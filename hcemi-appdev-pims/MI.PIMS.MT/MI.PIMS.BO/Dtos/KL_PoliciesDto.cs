using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class KL_PoliciesDto
    {
        public string PLCY_HIERARCHY_KEY { get; set; }
        public string PLCY_POLICY_ID { get; set; }
        public string PLCY_NM { get; set; }
        public string PLCY_PROC_CD { get; set; }
    }

    public class KL_PoliciesParamDto
    {
        public string p_dpoc_bus_seg_cd { get; set; }
        public string p_dpoc_entity_cd { get; set; }
        public string p_proc_cd { get; set; }
        public string p_plcy_type_cd { get; set; }
    }
}
