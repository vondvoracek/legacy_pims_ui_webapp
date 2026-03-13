using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    [Table("ref_iq_manual_guidelines_t")]
    public class Ref_Iq_Manual_Guidelines_T
    {
        public string iq_gdln_id { get; set; }            // varchar(100), Primary Key
        public string iq_gdln_proc_cd { get; set; }       // varchar(20), Not Null
        public string iq_version { get; set; }            // varchar(50)
        public string iq_reference { get; set; }          // varchar(500)
        //public string iq_gdln_desc { get; set; }          // varchar(500)
        //public string iq_gdln_product_nm { get; set; }    // varchar(100)
        //public string iq_gdln_product_desc { get; set; }  // varchar(500)
        //public string iq_gdln_rcmmndtn_id { get; set; }   // varchar(50)
        //public string iq_gdln_rcmmndtn_desc { get; set; } // varchar(500)
        //public string iq_gdln_rcmmndtn_rea { get; set; }  // varchar(3)
        //public DateTime? iq_gdln_rel_dt { get; set; }     // timestamp without time zone
        //public DateTime? iq_gdln_exp_dt { get; set; }     // timestamp without time zone
        //public string iq_gdln_minor_ver { get; set; }     // varchar(50)
        //public string iq_gdln_decision_rsn { get; set; }  // varchar(3)
        //public string iq_gdln_cd_type { get; set; }       // varchar(20)
        //public string iq_gdln_ref_gdln_id { get; set; }   // varchar(50)
        //public string iq_gdln_change_desc { get; set; }   // varchar(500)
    }
}
