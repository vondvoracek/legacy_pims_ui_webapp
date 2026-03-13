using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Dtos
{
    public class MenuAccessDto
    {
        public string Page_ID { get; set; }
        public string Module_ID { get; set; }
        public string Module_ID_String { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URLPath { get; set; }
        public string URLImage { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Method_Name { get; set; }
        public string Display_Name { get; set; }
        public string UserInitials { get; set; }
        public string Page_Label { get; set; }
    }

    public class MenuAccessParamDto
    {
        public string MS_ID { get; set; }
    }
}

