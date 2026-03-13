using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class CategoryUpdate_Dto
    {
        public string PROC_CD { get; set; }
        public string DRUG_NM { get; set; }
        public string ALTERNATE_CATEGORY { get; set; }
        public string ALTERNATE_SUB_CATEGORY { get; set; }
        public string STNDRD_SVC_CAT { get; set; }
        public string STNDRD_SVC_SUBCAT { get; set; }
        public string EDITABLE_IND { get; set; }
    }

    public class CategoryUpdateParam
    {
        public string P_PROC_CD { get; set; }
        public string P_DRUG_NM { get; set; }
        public string P_ALTERNATE_CATEGORY { get; set; }
        public string P_ALTERNATE_SUB_CATEGORY { get; set; }
        public string P_LST_UPDT_BY { get; set; }
    }
    public class CategoryInsertByProcCDParam
    {
        public string P_PROC_CD { get; set; }        
        public string P_ALTERNATE_CATEGORY { get; set; }
        public string P_ALTERNATE_SUB_CATEGORY { get; set; }
        public string P_LST_UPDT_BY { get; set; }
    }

    public class CategoryInsertByProcCDRetVal
    {
        public int O_ALREADY_EXIST_COUNT { get; set; }
    }

    public class CategoryRecordImpacted
    {
        public int RECORDS_IMPACTED { get; set; }
    }

    public class CategoryUpdate_Edit_Dto
    {
        public string P_PROC_CD { get; set; }
        public string P_DRUG_NM { get; set; }
        public string P_ALTERNATE_CATEGORY_OLD { get; set; }
        public string P_ALTERNATE_SUB_CATEGORY_OLD { get; set; }
        public string P_ALTERNATE_CATEGORY_NEW { get; set; }
        public string P_ALTERNATE_SUB_CATEGORY_NEW { get; set; }
        public string P_LST_UPDT_BY { get; set; }
        public DateTime? P_LST_UPDT_DT { get; set; }
    }

    public class Admin_Catagory_By_Type_Param_Dto
    {
        public string P_TEXT { get; set; }
        public string P_CATEGORY_TYPE { get; set; }
        public string P_PARENT_CATEGORY { get; set; }
        public string P_DRUG_NM { get; set; }
    }

    public class Admin_Catagory_By_Type_Dto
    {
        public string CATEGORY { get; set; }
        public string DRUG_NM { get; set; }
    }

    [Serializable]
    public class InsertRetDto
    {
        public int StatusID { get; set; }
        public string StatusType { get; set; }
        public byte[] Upsize_Ts { get; set; }
        public string Upsize_TsString { get; set; }
        public IEnumerable<CategoryInsertByProcCDParam> InsertedRecords { get; set; }
        public IEnumerable<CategoryInsertByProcCDParam> ErrorRecords { get; set; }
        public IEnumerable<CategoryInsertByProcCDParam> DupRecords { get; set; }
        public string Message { get; set; }
        public InsertRetDto(RetValStatus retValStatus = RetValStatus.Success) //By default set StatusType = Success
        {
            StatusType = retValStatus.ToString();
        }
        public InsertRetDto() { }
    }
}
