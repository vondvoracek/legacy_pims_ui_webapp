using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class ReportsDto
    {
        public string PAGE_ID { get; set; }
        public string MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string TITLE { get; set; }
        public string PAGE_LABEL { get; set; }
        public string URLPATH { get; set; }
        public string URLIMAGE { get; set; }
        public string SORT_ORDER { get; set; }
        public string NAMEVALUES { get; set; }
        public string ACTIVE { get; set; }
        public string LST_UPDT_DT { get; set; }
        public string LST_UPDT_BY { get; set; }
        public string IS_MENU_ITEM { get; set; }
        public string DISPLAY_INDEX { get; set; }
        public string URLICON { get; set; }
    }

    public class UpdateReportDto
    {
        public string p_URLPATH { get; set; }
        public string p_PAGE_ID { get; set; }
        public string p_LST_UPDT_BY { get; set; }
    }
}
