using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Ret_Factors_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime EPAL_VER_EFF_DT { get; set; }
        public DateTime FACTOR_EFF_DT_RET { get; set; }
        public DateTime? FACTOR_EXP_DT_RET { get; set; }
        public string FACTOR_NM_RET { get; set; }
        public string FACTOR_NOTES_RET { get; set; }
        public string FACTOR_TYPE_RET { get; set; }
        public string RRF_SYS_SEQ_RET { get; set; }
        public string FACTOR_DESC_RET { get; set; }
    }

    public class Red_Factors_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime EPAL_VER_EFF_DT { get; set; }
        public DateTime FACTOR_EFF_DT_RED { get; set; }
        public DateTime? FACTOR_EXP_DT_RED { get; set; }
        public string FACTOR_NM_RED { get; set; }
        public string FACTOR_NOTES_RED { get; set; }
        public string FACTOR_TYPE_RED { get; set; }
        public string RRF_SYS_SEQ_RED { get; set; }
        public string FACTOR_DESC_RED { get; set; }
    }

    public class Ret_Factor_DD_Dto
    {
        public string FACTOR_NM_RET { get; set; }        
        public string FACTOR_DESC_RET { get; set; }
    }

    public class Red_Factor_DD_Dto
    {
        public string FACTOR_NM_RED { get; set; }        
        public string FACTOR_DESC_RED { get; set; }
    }
}
