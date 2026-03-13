using Microsoft.AspNetCore.Http;
using System;

namespace MI.PIMS.UI.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Message { get; set; }
        public int ErrorNumber { get; set; }        
    }
}
