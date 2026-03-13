using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Catagories_Dto
    {
        public string I_EPAL_HIERARCHY_KEY { get; set; }
        public string O_ALTERNATE_CATEGORY { get; set; }
        public string O_ALTERNATE_SUB_CATEGORY { get; set; }
        public string O_STANDARD_CATEGORY { get; set; }
        public string O_STANDARD_SUB_CATEGORY { get; set; }
    }

    public class EPAL_Catagory_By_Type_Dto
    {
        public string CATEGORY { get; set; }
        public string DRUG_NM { get; set; }
        public string ALT_CAT_TYPE { get; set; }
        public string EPAL_STATUS_CD { get; set; }
    }

    public class EPAL_Catagory_By_Type_Param_Dto
    {
        public string P_TEXT { get; set; }
        public string P_CATEGORY_TYPE { get; set; }
        public string P_PARENT_CATEGORY { get; set; }
        public string P_DRUG_NM { get; set; }
    }

    public class EPAL_Catagory_By_ProcCDDrugNM_Param_Dto
    {
        public string P_TEXT { get; set; }
        public string P_DRUG_NM { get; set; }
        public string P_PROC_CD { get; set; }
        public string P_ALTERNATE_CATEGORY { get; set; }
    }

    public class Altrnt_Cat_Dto
    {
        public string altrnt_svc_cat { get; set; }
        public string altrnt_svc_subcat { get; set; }
    }
}
