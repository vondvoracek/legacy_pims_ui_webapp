using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Dtos
{
    [Serializable]
    public class UpdateDto
    {
        public int StatusID { get; set; }
        public string StatusType { get; set; }
        public byte[] Upsize_Ts { get; set; }
        public string Upsize_TsString { get; set; }
        public object ReturnObject { get; set; }
        public string Message { get; set; }
        public UpdateDto(RetValStatus retValStatus = RetValStatus.Success) //By default set StatusType = Success
        {
            StatusType = retValStatus.ToString();
        }
        public UpdateDto() { }
    }    
}

namespace MI.PIMS.BO
{
    public enum RetValStatus
    {
        Success = 1,
        Error = 2,
        Warning = 3
    }
}
