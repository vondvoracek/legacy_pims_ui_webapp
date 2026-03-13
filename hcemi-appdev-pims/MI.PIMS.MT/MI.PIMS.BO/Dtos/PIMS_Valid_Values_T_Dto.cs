using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class PIMS_Valid_Values_T_Dto
    {
        public string VV_SET_NAME { get; set; }
        public string BUS_SEG_CD { get; set; }
        public string VV_CD { get; set; }
        public string VV_CD_DESC { get; set; }
    }

    public class PIMS_Valid_Values_V_Dto
    {
        public string VV_SET_NAME { get; set; }
        public string BUS_SEG_CD { get; set; }
        public string VV_CD { get; set; }
        public string VV_CD_DESC { get; set; }
    }

    public class PIMS_Valid_Values_Dto
    {
        public string VV_CD { get; set; }
        public string VV_CD_DESC { get; set; }
    }

}
