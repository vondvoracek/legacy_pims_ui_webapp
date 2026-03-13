using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using MI.PIMS.BO.Dtos;
using System.Diagnostics;

namespace MI.PIMS.BL.Common
{
    public class Helper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public Helper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
        }

        public string GetOracleConnectionString
        {
            get
            {
                var conString = _config["AppSettings:OracleConnectionStrings"];
                return conString;
            }
        }

        public string GetConnectionString
        {
            get
            {
                var conString = _config["AppSettings:PostgresConnectionStrings"];
                return conString;
            }
        }

        public static string GetSQLConnectionString_BCRT
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("SQLConnectionStrings_BCRT").Value;
            }
        }

        public static string GetSQLConnectionString_DMA
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("SQLConnectionStrings_DMA").Value;
            }
        }
        public static string AppSettings(string getSection)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, reloadOnChange: true);
            return builder.Build().GetSection("AppSettings").GetSection(getSection).Value;
        }
        public string MIServicesAPIUrl
        {
            get
            {
                return AppSettings("MIServicesAPIUrl");
            }
        }
        public string AccessGlobalGroup
        {
            get
            {
                return AppSettings("AccessGlobalGroup");
            }
        }
        public static string AddMessage
        {
            get
            {
                return AppSettings("AddMessage");
            }
        }
        public static string UpdateMessage
        {
            get
            {
                return AppSettings("UpdateMessage");
            }
        }
        public static string RecordModifiedMessage
        {
            get
            {
                return AppSettings("RecordModifiedMessage");
            }
        }
        public static string RecordDeleteMessage
        {
            get
            {
                return AppSettings("RecordDeleteMessage");
            }
        }
        public static string AlreadyExistMessage
        {
            get
            {
                return AppSettings("AlreadyExistMessage");
            }
        }
        public static string OneOrMoreModifiedMessage
        {
            get
            {
                return AppSettings("OneOrMoreModifiedMessage");
            }
        }
        public static string NotExistMessage
        {
            get
            {
                return AppSettings("NotExistMessage");
            }
        }
        public static string AddAttachmentText
        {
            get
            {
                return AppSettings("AddAttachmentText");
            }
        }
        public static string DeleteAttachmentText
        {
            get
            {
                return AppSettings("DeleteAttachmentText");
            }
        }
        public string GetMSID()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name.Replace("MS\\", "");
        }

        public string DatabaseSchema
        {
            get
            {
                var conString = _config["AppSettings:DatabaseSchema"];
                return conString;
            }
        }

        public static string formatMultiSelectValues(string value)
        {
            string s = value;
            string[] items = s.Split(',');
            value = "";
            for (int i = 0; i < items.Length; i++)
            {
                value += "'" + items[i] + "',";
            }
            value = value.TrimEnd(',');
            value = "(" + value + ")";

            return value;
        }

        public static int CountMultiSelectValues(string value)
        {
            if (value == null)
            {
                return 0;
            }
            else {
                string s = value;
                string[] items = s.Split(',');
                return items.Length;
            }

        }

        public static string PartialEPAL_ID(string p_EPAL_BUS_SEG_CD, 
                                            string p_EPAL_ENTITY_CD,
                                            string p_EPAL_PLAN_CD,
                                            string p_EPAL_PRODUCT_CD,
                                            string p_EPAL_FUND_ARNGMNT_CD,
                                            string p_PROC_CD,
                                            string p_DRUG_NM) 
        {
            var partialEPAL_ID = "";
            bool isValid = false;

            if (CountMultiSelectValues(p_EPAL_BUS_SEG_CD) == 1)
            {
                partialEPAL_ID += p_EPAL_BUS_SEG_CD + "-";
                isValid = true;
            }
            else 
            {
                isValid = false;
            }

            if (CountMultiSelectValues(p_EPAL_ENTITY_CD) == 1)
            {
                partialEPAL_ID += p_EPAL_ENTITY_CD + "-";
                isValid = true;
            }
            else 
            {
                isValid = false;
            }

            if (CountMultiSelectValues(p_EPAL_PLAN_CD) == 1)
            {
                if (isValid)
                {
                    partialEPAL_ID += p_EPAL_PLAN_CD + "-";
                    isValid = true;
                }
            }
            else 
            {
                isValid = false;
            }

            if (CountMultiSelectValues(p_EPAL_PRODUCT_CD) == 1)
            {          
                if (isValid)
                {
                    partialEPAL_ID += p_EPAL_PRODUCT_CD + "-";
                    isValid = true;
                }
            }
            else
            {
                isValid = false;
            }


            if (CountMultiSelectValues(p_EPAL_FUND_ARNGMNT_CD) == 1)
            {
                if (isValid)
                {
                    partialEPAL_ID += p_EPAL_FUND_ARNGMNT_CD + "-";
                }
            }



            //if (CountMultiSelectValues(p_PROC_CD) == 1)
            //{
            //    partialEPAL_ID += p_PROC_CD + "-";
            //}

            //if (CountMultiSelectValues(p_DRUG_NM) == 1)
            //{
            //    partialEPAL_ID += p_DRUG_NM;
            //}

            return partialEPAL_ID;


        }

        public static DateTime? CheckExpYear(DateTime? dateToCheck)
        {
            DateTime? dateOut = dateToCheck;

            if (dateToCheck.HasValue && (dateToCheck == DateTime.Parse("12/31/2999") || dateToCheck == DateTime.Parse("12/31/1999") || dateToCheck == DateTime.Parse("01/01/1900")))
            {
                dateOut = null;
            }

            return dateOut;
        }

        public static DateOnly? CheckExpYear(DateOnly? dateToCheck)
        {
            DateOnly? dateOut = dateToCheck;

            if (dateToCheck.HasValue && (dateToCheck == DateOnly.Parse("12/31/2999") || dateToCheck == DateOnly.Parse("12/31/1999")))
            {
                dateOut = null;
            }

            return dateOut;
        }        
    }
}
