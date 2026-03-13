using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Procedure_Dto
    {
        public string DPOC_ENTITY_CD { get; set; }
        public string DPOC_BUS_SEG_CD { get; set; }
        public string DPOC_STATUS { get; set; }
        public string IS_CURRENT { get; set; }
        public string HIERARCHY_CODES_IS_ACTIVE { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public IEnumerable<string> states { get; set; }
    }

    public class DPOC_Param_Dto
    {
        public string p_DPOC_HIERARCHY_KEY                {get;set;}
        public string p_DPOC_BUS_SEG_CD                  {get;set;}
        public string p_DPOC_ENTITY_CD                   {get;set;}
        public string p_PROC_CD                          {get;set;}
        public string p_IQ_GDLN_ID                       {get;set;}
        public string p_IQ_GDLN_VERSION                  {get;set;}
        public string p_IQ_CRITERIA                      {get;set;}
        public string p_IQ_REFERENCE                     {get;set;}
        public DateTime? p_DPOC_EFF_DT                      {get;set;}
        public string p_DPOC_PACKAGE                     {get;set;}
        public string p_DPOC_RELEASE                     {get;set;}
        public string p_DPOC_ELIGIBLE_IND                {get;set;}
        public string p_DPOC_IMPLEMENTED_IND             {get;set;}  
        public string p_EPAL_ALTRNT_SVC_CAT              {get;set;} 
        public string p_EPAL_ALTRNT_SVC_SUBCAT           {get;set;}
        public DateTime? p_DPOC_VER_EFF_DT                  {get;set;}
        public string p_DTQ_NM                           {get;set;}
        public string p_DTQ_TYPE                         {get;set;}
        public string p_DTQ_RSN                          {get;set;}
        public string p_RULE_TYPE_OUTCOME_OUTPAT         {get;set;}
        public string p_RULE_TYPE_OUTPAT                 {get;set;}
        public string p_RULE_TYPE_OUTCOME_OUTPAT_FCLTY   {get;set;}
        public string p_RULE_TYPE_OUTPAT_FCLTY           {get;set;}
        public string p_RULE_TYPE_OUTCOME_INPAT          {get;set;}
        public string p_RULE_TYPE_INPAT                  {get;set;}
        public string p_JRSDCTN_NM                       {get;set;}
        public int p_INCLUDE_HISTORICAL { get; set; }
        public string p_PRODUCT_CD { get; set; }
        public string p_FUND_ARNGMNT_CD { get; set; }
    }
}
