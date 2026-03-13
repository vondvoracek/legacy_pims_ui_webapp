using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Ins_Upd_Pkg_Param
    {
        public string P_EPAL_BUS_SEG_CD             { get; set; }
        public string P_EPAL_ENTITY_CD              {get;set;}
        public string P_EPAL_PLAN_CD                {get;set;}
        public string P_EPAL_PRODUCT_CD             {get;set;}
        public string P_EPAL_FUND_ARNGMNT_CD        {get;set;}
        public string P_PROC_CD                     {get;set;}
        public string P_DRUG_NM                     {get;set;}
        public string P_DRUG_RVW_AT_LAUNCH_IND      {get;set;}
        public DateTime? P_PRIOR_AUTH_EFF_DT        {get;set;}
        public DateTime? P_PRIOR_AUTH_EXP_DT         {get;set;}
        public int P_PRIOR_AUTH_AGE_MIN             {get;set;}
        public int P_PRIOR_AUTH_AGE_MAX             {get;set;}
        public DateTime? P_AUTO_APRVL_EFF_DT           {get;set;}
        public DateTime? P_AUTO_APRVL_EXP_DT         {get;set;}
        public string P_MCARE_SPCL_PRCSNG_TYPE      {get;set;}
        public DateTime? P_MCARE_SPCL_PRCSNG_EFF_DT  {get;set;}
        public DateTime? P_MCARE_SPCL_PRCSNG_EXP_DT {get;set;}
        public DateTime? P_PRE_DET_EFF_DT { get; set; }
        public DateTime? P_PRE_DET_EXP_DT { get; set; }

        public string P_SOS_IND                     {get;set;}
        public string P_ALTRNT_SVC_CAT { get; set; }
        public string P_ALTRNT_SVC_SUBCAT { get; set; }
        public string P_FURTHER_INST { get; set; }
        public string P_NOTES { get; set; }
        public string P_USER_ID { get; set; }
        public string P_CHANGE_REQ_ID { get; set; }
        public string P_CHANGE_DESC { get; set; }
        public string P_STATE_CD { get; set; }
        public string P_STATE_MANDATED_IND { get; set; }
        public string P_ATS_INCL_EXCL_CD { get; set; }
        public string P_DIAG_CD { get; set; }
        public string P_DIAG_INCL_EXCL_CD { get; set; }
        public string P_MODIFIER { get; set; }
        public string P_MOD_INCL_EXCL_CD { get; set; }
        public string P_PLC_OF_SVC_CD { get; set; }
        public string P_POS_INCL_EXCL_CD { get; set; }
        public string P_REV_CD { get; set; }
        public string P_REV_INCL_EXCL_CD { get; set; }
        public DateTime? P_EPAL_CURR_REVIEW_DT { get; set; }
        public DateTime? P_EPAL_PREV_REVIEW_DT { get; set; }
        public DateTime? P_SOS_EFF_DT { get; set; }
        public DateTime? P_SOS_EXP_DT { get; set; }
        public string P_SOS_TYPE { get; set; }
        public string P_SOS_SITE_IND { get; set; }
        public string P_SOS_URG_CAT_MDLTY { get; set; }
        //FEATURE 44618 MFQ 1/9/2023
        public string P_ATS_EFF_DT { get; set; }
        public string P_ATS_EXP_DT { get; set; }
        public string P_ATS_ISSUE_GOV { get; set; }
        public string P_ATS_PROG_MGD_BY { get; set; }
        public string P_DIAG_PROG_MGD_BY { get; set; }
        //FEATURE 44779 DYU 1/23/2023
        public string P_LIST_NAME { get; set; }
        public string P_PMB_EFF_DT { get; set; }
        public string P_PMB_EXP_DT { get; set; }
        public string P_PMB_PROG_MGD_BY { get; set; }
        public string P_PMB_DELEGATED_UM { get; set; }
        public string P_PMB_BASED_ON_DX_IND { get; set; }
        public string P_PMB_BASED_ON_ST_APP_IND { get; set; }
        public string P_PMB_BASED_ON_AGE_MIN { get; set; }
        public string P_PMB_BASED_ON_AGE_MAX { get; set; }
        public string P_FACTOR_TYPE { get; set; }
        public string P_FACTOR_NM { get; set; }
        public string P_FACTOR_EFF_DT { get; set; }
        public string P_FACTOR_EXP_DT { get; set; }
        public string P_FACTOR_NOTES { get; set; }
        // 03/09/2024 - Added new advanced notification dates
        public DateTime? P_ADV_NTFCTN_EFF_DT { get; set; }
        public DateTime? P_ADV_NTFCTN_EXP_DT { get; set; }
        public DateTime? P_DRAL_EFF_DT { get; set; }
        public DateTime? P_DRAL_EXP_DT { get; set; }
        //public string P_SVC_CAT                     {get;set;}
        //public string P_SVC_SUBCAT                  {get;set;}
        //public string P_HCE_REP_CAT                 {get;set;}
        //public string P_PROG_MGD_BY                 {get;set;}
        //public string P_DELEGATED_UM                {get;set;}
        public string EPALPageView { get; set; }
        public string P_EPAL_HIERARCHY_KEY { get; set; }
        //public string P_PRE_DET_IND { get; set; }

        public DateTime? P_EPAL_VER_EFF_DT { get; set; }
    }
    
    public class EPAL_Del_Pkg_Param
    {
        public string p_EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? p_EPAL_VER_EFF_DT { get; set; }
        public string p_USER_ID { get; set; }
        public string p_CHANGE_REQ_ID { get; set; }
        public string p_CHANGE_DESC { get; set; }
    }
}
