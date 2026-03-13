using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Inventories_V_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public string DPOC_BUS_SEG_CD { get; set; }
        public string DPOC_BUS_SEG_DESC { get; set; }
        public string DPOC_ENTITY_CD { get; set; }
        public string DPOC_ENTITY_DESC { get; set; }
        public string DPOC_PLAN_CD              {get;set;}
        public string DPOC_PLAN_DESC            {get;set;}
        public string DPOC_PRODUCT_CD           {get;set;}
        public string DPOC_PRODUCT_DESC         {get;set;}
        public string DPOC_FUND_ARNGMNT_CD      {get;set;}
        public string DPOC_FUND_ARNGMNT_DESC    {get;set;}
        public string PROC_CD                   {get;set;}
        public string DRUG_NM                   {get;set;}
        public DateTime? DPOC_VER_EFF_DT           {get;set;}
        public DateTime? DPOC_VER_EXP_DT           {get;set;}
        public string DPOC_PACKAGE              {get;set;}
        public string DPOC_RELEASE              {get;set;}
        public DateTime? DPOC_EFF_DT               {get;set;}
        public DateTime? DPOC_EXP_DT               {get;set;}
        public string DPOC_ELIGIBLE_IND         {get;set;}
        public string DPOC_INELIGIBLE_RSN       {get;set;}
        public string DPOC_IMPLEMENTED_IND      {get;set;}
        public string DPOC_UNIMPLEMENTED_RSN    { get; set; }
        public string DPOC_INV_NOTES            { get; set; }
        public string DPOC_ADDTNL_RQRMNTS       { get; set; }
        public string EPAL_HIERARCHY_KEY        {get;set;}
        public DateTime? EPAL_VER_EFF_DT        {get;set;}
        public DateTime? EPAL_VER_EXP_DT        {get;set;}
        public string EPAL_STNDRD_SVC_CAT       {get;set;}
        public string EPAL_STNDRD_SVC_SUBCAT    {get;set;}
        public string EPAL_ALTRNT_SVC_CAT       {get;set;}
        public string EPAL_ALTRNT_SVC_SUBCAT    {get;set;}
        public string EPAL_PROC_CD_TYPE         {get;set;}
        public string EPAL_PROC_CD_DESC         {get;set;}
        public string EPAL_PROC_CD_STATUS       {get;set;}
        public DateTime? EPAL_PRIOR_AUTH_EFF_DT    {get;set;}
        public DateTime? EPAL_PRIOR_AUTH_EXP_DT    {get;set;}
        public string EPAL_SOS_IND              {get;set;}
        public DateTime? EPAL_SOS_EFF_DT           {get;set;}
        public DateTime? EPAL_SOS_EXP_DT           {get;set;}                        
        public string IS_CURRENT { get; set; }
        public string DTQ_APPLIES { get; set; }
        /// <summary>
        /// DPOC Not Implemented Reason
        /// </summary>
        public string UIR_RSN { get; set; }
        //public string DPOC_SOS_PROVIDER_TIN_EXCL { get; set; }  //User Story 132092 MFQ 5/14/2025

        //10/4/2023 MFQ As per Specification 9.28.23
        public string DPOC_VER_NUM { get; set; }
        public string dateErrorString { get; set; }
        public string EPAL_SVC_CAT { get; set; }
        public string EPAL_SVC_SUBCAT { get; set; }
        public string DPOC_STATUS { get; set; }

        /*USER STORY 132079 MFQ 5/6/2025*/
        public string prog_mgd_by { get; set; }
    }

    public class DPOC_Inventories_Param_Dto
    {
        public string p_DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? p_DPOC_VER_EFF_DT { get; set; }
        public string p_MS_ID { get; set; }
        public string P_COLUMN_NAME { get; set; }
        // 10-3-2024 MFQ changes after confirmation with Mike B.
        public string P_DPOC_RELEASE { get; set; }
        public string P_DPOC_PACKAGE { get; set; }

        public DPOC_Inventories_Param_Dto(){}

        public DPOC_Inventories_Param_Dto(string pims_id, string ms_id)
        {
            string s = pims_id;
            string dpoc_ver_eff_dt = string.Empty;
            string[] splt_pims_id = s.Split(',');

            if (splt_pims_id.Length > 0)
                pims_id = splt_pims_id[0];

            if (splt_pims_id.Length > 1)
                dpoc_ver_eff_dt = splt_pims_id[1];

            p_DPOC_HIERARCHY_KEY = pims_id;
            p_MS_ID = ms_id;
            p_DPOC_VER_EFF_DT = string.IsNullOrEmpty(dpoc_ver_eff_dt) ? null : DateTime.Parse(dpoc_ver_eff_dt.Replace("-", "/"));
        }
    }

    public class DPOC_Inv_Gdln_Rules_V
    {
        public string IQ_GDLN_ID { get; set; }
        public string IQ_GDLN_VERSION { get; set; }
        public string IQ_CRITERIA { get; set; }
        public string IQ_REFERENCE { get; set; }
        public string IQ_GDLN_NM { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }

    }

    public class DPOC_Inv_Dtqs_V_Dto
    {
        public string DTQ_NM { get; set; }

    }

    public class DPOC_PIMS_ID_Param_Dto
    {
        public string p_DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? p_DPOC_VER_EFF_DT { get; set; }
        public string p_DPOC_PACKAGE { get; set; }
        public string p_DPOC_RELEASE { get; set; }
    }

    public class DPOC_Inventories_V_Hist_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public string DPOC_BUS_SEG_CD { get; set; }
        public string DPOC_BUS_SEG_DESC { get; set; }
        public string DPOC_ENTITY_CD { get; set; }
        public string DPOC_ENTITY_DESC { get; set; }
        public string DPOC_PLAN_CD { get; set; }
        public string DPOC_PLAN_DESC { get; set; }
        public string DPOC_PRODUCT_CD { get; set; }
        public string DPOC_PRODUCT_DESC { get; set; }
        public string DPOC_FUND_ARNGMNT_CD { get; set; }
        public string DPOC_FUND_ARNGMNT_DESC { get; set; }
        public string PROC_CD { get; set; }
        public string DRUG_NM { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public DateTime? DPOC_VER_EXP_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public DateTime? DPOC_EFF_DT { get; set; }
        public DateTime? DPOC_EXP_DT { get; set; }
        public string DPOC_ELIGIBLE_IND { get; set; }
        public string DPOC_INELIGIBLE_RSN { get; set; }
        public string DPOC_IMPLEMENTED_IND { get; set; }
        public string DPOC_UNIMPLEMENTED_RSN { get; set; }
        public string DPOC_INV_NOTES { get; set; }
        public string DPOC_ADDTNL_RQRMNTS { get; set; }
        public string KL_PLCY_POLICY_ID { get; set; }
        public string KL_PLCY_NM { get; set; }
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public string EPAL_STNDRD_SVC_CAT { get; set; }
        public string EPAL_STNDRD_SVC_SUBCAT { get; set; }
        public string EPAL_ALTRNT_SVC_CAT { get; set; }
        public string EPAL_ALTRNT_SVC_SUBCAT { get; set; }
        public string EPAL_PROC_CD_TYPE { get; set; }
        public string EPAL_PROC_CD_DESC { get; set; }
        public string EPAL_PROC_CD_STATUS { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EXP_DT { get; set; }
        public string EPAL_SOS_IND { get; set; }
        public DateTime? EPAL_SOS_EFF_DT { get; set; }
        public DateTime? EPAL_SOS_EXP_DT { get; set; }
        public string DPOC_STATUS { get; set; }
        public string IS_CURRENT { get; set; }
        public string DTQ_APPLIES { get; set; } // MFQ 05/24/2023 ADDED


        // GUIDELINE

        public string IQ_CRITERIA { get; set; }
        public string IQ_GDLN_ID { get; set; }
        public string IQ_GDLN_NM { get; set; }
        public string IQ_GDLN_PRODUCT_DESC { get; set; }
        public string IQ_GDLN_PRODUCT_NM { get; set; }
        public string IQ_GDLN_RECOMMENDATION_DESC { get; set; }
        public DateTime? IQ_GDLN_REL_DT { get; set; }
        public int IQ_GDLN_RULES_SYS_SEQ { get; set; }
        public string IQ_GDLN_STATUS { get; set; }
        public string IQ_GDLN_VERSION { get; set; }
        public string RULE_COMMENTS { get; set; }
        public string RULE_IMP_TYPE { get; set; }
        public string RULE_IMP_WITH { get; set; }
        public string RULE_OUTCOME_INPAT { get; set; }
        public string RULE_TYPE_OUTCOME_INPAT { get; set; }
        public string RULE_OUTCOME_INPAT_RSN { get; set; }
        public string RULE_OUTCOME_OUTPAT { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT { get; set; }
        public string RULE_OUTCOME_OUTPAT_RSN { get; set; }
        public string RULE_OUTCOME_OUTPAT_FCLTY { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT_FCLTY { get; set; }
        public string RULE_OUTCOME_OUTPAT_FCLTY_RSN { get; set; }
        public DateTime? IQ_GDLN_EXP_DT { get; set; }
        public string RULE_EXCLUSIONS { get; set; }
    }

    public class DPOC_ChangeHistory_Dto
    {
        public string CHANGEBY { get; set; }
        public string CHANGEDATE { get; set; }
        public string CHANGESOURCE { get; set; }
        public string CHANGEDESCRIPTION { get; set; }
        public string ChangeType { get; set; }
        public string DPOC_VER_EFF_DT { get; set; }
        public string RECORD_ENTRY_METHOD { get; set; }
    }
}
