using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_HIERARCHY_CODES_XWALK_V_Dto
    {
        public string data_source { get; set; }
        public string source_tab { get; set; }
        public string epal_bus_seg_cd { get; set; }
        public string epal_entity_cd { get; set; }
        public string epal_plan_cd { get; set; }
        public string epal_product_cd { get; set; }
        public string epal_fund_arngmnt_cd { get; set; }
        public string epal_hierarchy_sts { get; set; }
    }

    public class DPOC_Hierarchy_Codes_UI_Dto
    {
        public string CD { get; set; }
        public string Desc { get; set; }
    }
}
