using MI.PIMS.BO.Dtos;
using MI.PIMS.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Models
{
    public class UserValidationModel
    {
        public bool Is_Valid { get; set; }
        public string Message { get; set; }

        public UserInfoDto userInfo { get; set; }

        public UserInfo_T_Dto userInfo_T { get; set; }

        public UserValidationModel()
        {
            Is_Valid = false;
        }
    }
}
