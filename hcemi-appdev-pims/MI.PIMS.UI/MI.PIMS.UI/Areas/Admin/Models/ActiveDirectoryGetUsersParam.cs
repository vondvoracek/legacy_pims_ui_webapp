using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Models
{
    public class ActiveDirectoryGetUsersParam
    {
        public string MS_ID { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string AddReadLimit { get; set; }
    }
}
