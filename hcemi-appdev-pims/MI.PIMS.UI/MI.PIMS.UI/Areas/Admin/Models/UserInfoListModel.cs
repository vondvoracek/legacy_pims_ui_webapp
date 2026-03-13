using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.Admin.Models
{
    public class UserInfoListModel
    {
        public UserInfoDto[] userInfoDtos { get; set; }
    }

    public class UserInfoQS
    {
        public string request { get; set; }
    }
}
