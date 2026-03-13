using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Entities
{
    public class Entity_Xref
    {
        [Key]
        public string Entity { get; set; }  
        public string Type { get; set; }    
        public string Entity_Desc { get; set; } 
        public string BC { get; set; }  
        public string TCM { get; set; }     
        public string HospComLog { get; set; }       
        public string RPM_Incld { get; set; }   
        public bool Active { get; set; }  
        public DateTime Lst_Updt_Dt { get; set; }     
        public string Lst_Updt_By { get; set; }     
    }
}
