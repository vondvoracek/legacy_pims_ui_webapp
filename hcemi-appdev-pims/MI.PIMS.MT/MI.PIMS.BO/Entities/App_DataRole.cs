using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.PIMS.BO.Entities
{
    public class App_DataRole
    {
        [Key]
        public int App_DataRoleID              { get; set; }
        public string App_DataRoleName            { get; set; }
        public string RestrictedPolicy_Flag       { get; set; }
        public bool Active                      { get; set; }
        public DateTime Lst_Updt_Dt                 { get; set; }
        public string Lst_Updt_By                 { get; set; }
    }
}
