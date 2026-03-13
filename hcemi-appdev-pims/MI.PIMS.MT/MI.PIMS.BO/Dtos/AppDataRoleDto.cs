using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Dtos
{
    public class AppDataRoleDto
    {
        public int App_DataRoleID { get; set; }
        public string App_DataRoleName { get; set; }
        public string RestrictedPolicy_Flag { get; set; }
        public bool bit { get; set; }
        public DateTime Lst_Updt_Dt { get; set; }
        public string Lst_Updt_By { get; set; }

    }
}
