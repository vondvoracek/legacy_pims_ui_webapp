using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class MarketDto
    {
        public string Mkt          {get;set;}
        public string Mkt_Desc     {get;set;}
        public string Region       {get;set;}
        public int cdsa_id      {get;set;}
        public string Active       {get;set;}
        public DateTime Lst_updt_dt  {get;set;}
        public string Lst_updt_by { get; set; }

    }
}
