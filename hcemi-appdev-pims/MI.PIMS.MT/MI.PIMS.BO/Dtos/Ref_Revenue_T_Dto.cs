using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class Ref_Revenue_T_Dto
    {
        public string REV_CD { get; set; }
        public DateTime? REV_CD_EFF_DT { get; set; }
        public DateTime? REV_CD_EXP_DT { get; set; }
        public string REV_CD_DESC { get; set; }
    }
}
