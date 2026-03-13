using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class AppRole_T_Dto
    {
            public string ID            {get;set;}
            public string APP_ROLENAME { get;set;}
            [JsonIgnore]
            public string MODULE_ID     {get;set;}
            [JsonIgnore]
            public string ACTIVE        {get;set;}
            [JsonIgnore]
            public string LST_UPDT_DT   {get;set;}
            [JsonIgnore]
            public string LST_UPDT_BY   {get;set;}
            [JsonIgnore]
            public string CREATE_DT     {get;set;}
            [JsonIgnore]
            public string CREATE_BY     {get;set;}
            public string HAS_ROLE { get; set; }
            public string MS_ID { get; set; }

    }
}
