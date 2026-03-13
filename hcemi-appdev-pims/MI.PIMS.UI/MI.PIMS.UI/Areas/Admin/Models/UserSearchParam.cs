using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Models
{
    public class UserSearchParam
    {
        public string MS_ID { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string App_Role_ID { get; set; }
        public string Active { get; set; }
        public string PIMS_user { get; set; }

        public bool IsUserRecord { get; set; }
    }
}
