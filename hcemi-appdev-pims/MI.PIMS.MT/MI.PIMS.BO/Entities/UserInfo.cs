using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.PIMS.BO.Entities
{
    public class UserInfo
    {
        [Key]
        public  string MS_ID                {get;set;}
        public  string Sup_MSID             {get;set;}
        public  string Lname                {get;set;}
        public  string Fname                {get;set;}
        public  string MI                   {get;set;}
        public  string Email                {get;set;}
        public  string Phone                {get;set;}
        public  string Fax                  {get;set;}
        public  string Div_Code             {get;set;}
        public string App_DataRoleID       {get;set;}
        public string App_GroupID          {get;set;}
        public  bool Active               {get;set;}
        public  DateTime Lst_Updt_Dt          {get;set;}
        public  string Lst_Updt_By          {get;set;}
        public bool Manualupdt           {get;set;}
        public  DateTime Lst_Access_Dt        {get;set;}
        public DateTime News_Lst_Viewed_Dt    { get; set; }
    }

}
