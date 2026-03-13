using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Models
{
    public class AppSettings
    {
        public string ConnectionStrings { get; set; }
        public string AppBaseUrl { get; set; }
        public string WebUrl { get; set; }
        public string VirtualDirectory { get; set; }
        public string ErrorReceivers { get; set; }
        public string AccessGlobalGroup { get; set; }
        public string AddMessage { get; set; }
        public string UpdateMessage { get; set; }
        public string RecordModifiedMessage { get; set; }
        public string RecordDeleteMessage { get; set; }
        public string AlreadyExistMessage { get; set; }
        public string OneOrMoreModifiedMessage { get; set; }
    }


}
