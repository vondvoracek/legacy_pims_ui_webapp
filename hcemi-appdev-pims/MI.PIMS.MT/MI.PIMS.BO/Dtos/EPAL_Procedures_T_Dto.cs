using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class EPAL_Procedures_T_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string PROC_CD { get; set; }
        public string DRUG_NM { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public DateTime? EPAL_VER_EXP_DT { get; set; }
        public string DRUG_RVW_AT_LAUNCH_IND { get; set; }
        public string PRIOR_AUTH_IND { get; set; }
        public DateTime? PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? PRIOR_AUTH_EXP_DT { get; set; }
        public string PRIOR_AUTH_AGE_MIN { get; set; }
        public string PRIOR_AUTH_AGE_MAX { get; set; }
        public string AUTO_APRVL_IND { get; set; }
        public DateTime? AUTO_APRVL_EFF_DT { get; set; }
        public DateTime? AUTO_APRVL_EXP_DT { get; set; }
        public string MCARE_SPCL_PRCSNG_IND { get; set; }
        public string MCARE_SPCL_PRCSNG_TYPE { get; set; }
        public DateTime? MCARE_SPCL_PRCSNG_EFF_DT { get; set; }
        public DateTime? MCARE_SPCL_PRCSNG_EXP_DT { get; set; }
        public string MDCL_NCSSTY_IND { get; set; }
        public string SOS_IND { get; set; }
        public string LVL_CARE_RVW_RQD_IND { get; set; }
        public string HPBP_ID { get; set; }
        public string SVC_CAT { get; set; }
        public string SVC_SUBCAT { get; set; }
        public string HCE_REP_CAT { get; set; }
        public string PROC_CD_AHRQ_CAT { get; set; }
        public string PROG_MGD_BY { get; set; }
        public string DELEGATED_UM { get; set; }
        public string INT_EXT_CD { get; set; }
        public string FURTHER_INST { get; set; }
        public string NOTES { get; set; }
        public string EPAL_ORIGIN_SOURCE { get; set; }
        public string EPAL_ORIGIN_TAB { get; set; }
        public string EPAL_ORIGIN_CAT { get; set; }
        public string EPAL_ORIGIN_SUBCAT { get; set; }
        public string APP_ROLENAME { get; set; }
        public string IS_CURRENT { get; set; }
        public string EPAL_STATUS { get; set; }
        public string PROC_CD_STATUS { get; set; }
        public DateTime? EPAL_CURR_REVIEW_DT { get; set; }
        public DateTime? EPAL_PREV_REVIEW_DT { get; set; }
        public string HIERARCHY_CODES_IS_ACTIVE { get; set; }
        public string EPAL_STATUS_MB { get; set; }
        public string INCLUDE_HISTORICAL { get; set; }
        
    }


    public class EPAL_Procedures_V_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_BUS_SEG_DESC { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_ENTITY_DESC { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PLAN_DESC { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_PRODUCT_DESC { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_DESC { get; set; }
        public string PROC_CD { get; set; }
        public string PROC_CD_DESC { get; set; }
        public string PROC_CD_TYPE { get; set; }
        public DateTime? PROC_CD_EFF_DT { get; set; }
        public DateTime? PROC_CD_EXP_DT { get; set; }        
        public string PRIOR_AUTH_AGE_MIN { get; set; }
        public string PRIOR_AUTH_AGE_MAX { get; set; }
        public string DRUG_NM { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public DateTime? EPAL_VER_EXP_DT { get; set; }        
        public string DRUG_RVW_AT_LAUNCH_IND { get; set; }
        public DateTime? OVERALL_EFF_DT { get; set; }
        public DateTime? OVERALL_EXP_DT { get; set; }        
        public string PRIOR_AUTH_IND { get; set; }
        public DateTime? PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? PRIOR_AUTH_EXP_DT { get; set; }        
        public string AUTO_APRVL_IND { get; set; }
        public DateTime? AUTO_APRVL_EFF_DT { get; set; }
        public DateTime? AUTO_APRVL_EXP_DT { get; set; }        
        public string MCARE_SPCL_PRCSNG_IND { get; set; }
        public string MCARE_SPCL_PRCSNG_TYPE { get; set; }
        public DateTime? MCARE_SPCL_PRCSNG_EFF_DT { get; set; }
        public DateTime? MCARE_SPCL_PRCSNG_EXP_DT { get; set; }        
        public string MDCL_NCSSTY_IND { get; set; }
        public string SOS_IND { get; set; }
        public DateTime? SOS_EFF_DT { get; set; }
        public DateTime? SOS_EXP_DT { get; set; }
        public string SOS_DT_Error { get; set; }
        public string LVL_CARE_RVW_RQD_IND { get; set; }
        public string HPBP_ID { get; set; }
        public string STNDRD_SVC_CAT { get; set; }
        public string STNDRD_SVC_SUBCAT { get; set; }
        public string ALTRNT_SVC_CAT { get; set; }
        public string ALTRNT_SVC_SUBCAT { get; set; }
        public string PROC_CD_AHRQ_CAT { get; set; }
        public string PROG_MGD_BY { get; set; }
        public string DELEGATED_UM { get; set; }
        public string INT_EXT_CD { get; set; }
        public string FURTHER_INST { get; set; }
        public string NOTES { get; set; }
        public string EPAL_ORIGIN_SOURCE { get; set; }
        public string EPAL_ORIGIN_TAB { get; set; }
        public string APP_ROLENAME { get; set; }
        public string CHANGE_REQ_ID { get; set; }
        public string CHANGE_DESC { get; set; }
        public string IS_CURRENT { get; set; }
        public string EPAL_STATUS { get; set; }
        public string PROC_CD_STATUS { get; set; }
        public string SOS_TYPE { get; set; }
        public string SOS_SITE_IND { get; set; }
        public string SOS_URG_CAT_MDLTY { get; set; }
        public DateTime? EPAL_CURR_REVIEW_DT { get; set; }
        public DateTime? EPAL_PREV_REVIEW_DT { get; set; }
        public string HIERARCHY_CODES_IS_ACTIVE { get; set; }
        public string EPAL_STATUS_MB { get; set; }

        public string dateErrorString { get; set; }
        public IEnumerable<string> states { get; set; }
        public DateTime? MAX_PRIOR_AUTH_EXP_DT { get; set; }

        /* USER STORY 54535 3/27/2023 MFQ */
        public string PRE_DET_IND { get; set; }
        public DateTime? PRE_DET_EFF_DT { get; set; }
        public DateTime? PRE_DET_EXP_DT { get; set; }

        /* USER STORY 63341 6/21/2023 MFQ */
        public string GOLD_CARD_ELIG_IND { get; set; }
        public string INCLUDE_HISTORICAL { get; set; }
        public string SOS_SITE_AND_SERVICE_VV { get; set; }
        public string SOS_SITE_ONLY_VV { get; set; }
        
        /* USER STORY 93191 3-12-2024 MFQ  */
        public string ADV_NTFCTN_IND { get; set; }
        public DateTime? ADV_NTFCTN_EFF_DT { get; set; }
        public DateTime? ADV_NTFCTN_EXP_DT { get; set; }        
        public DateTime? DRAL_EFF_DT { get; set; }
        public DateTime? DRAL_EXP_DT { get; set; }
        /* END USER STORY 93191 3-12-2024 MFQ  */

        /* USER STORY 95701 MFQ 4-5-2024*/
        public EPALDrivingSourceRangeDto epalDrivingSourceRangeModel { get; set; }

        /*
         * User Story 131025 MFQ 4-11-2025
         * SUSPENSION FIELDS START
         */
        public string SUSP_IND { get; set; }
        public string SUSP_TYPE { get; set; }
        public DateTime? SUSP_EFF_DT { get; set; }
        public DateTime? SUSP_EXP_DT { get; set; }
        public string SUSP_RSN { get; set; }

        /* USER STORY 138187 DYU 10-29-2025*/
        public int IS_DONOR_RECORD { get; set; }
    }

    public class EPALDrivingSourceRangeDto
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EPALDrivingSourceRangeDto Future { get; set; }
    }


    public class EPAL_Procedures_Param_Dto
    {
        public string p_EPAL_HIERARCHY_KEY { get; set; }
        public string p_EPAL_PRODUCT_CD { get; set; }
        public string p_PROC_CD { get; set; }
        public string p_EPAL_PLAN_CD { get; set; }
        public string p_EPAL_BUS_SEG_CD { get; set; }
        public string p_EPAL_FUND_ARNGMNT_CD { get; set; }
        public string p_EPAL_ENTITY_CD { get; set; }
       // public string p_STATE_CD { get; set; }
        public string p_DRUG_NM { get; set; }
        public int p_INCLUDE_HISTORICAL { get; set; }
        public DateTime? p_OVERALL_EFF_DT_From { get; set; }
        public DateTime? p_OVERALL_EFF_DT_To { get; set; }
        public DateTime? p_OVERALL_EXP_DT_From { get; set; }
        public DateTime? p_OVERALL_EXP_DT_To { get; set; }
        public DateTime? p_EPAL_VER_EFF_DT { get; set; }
        public string p_MS_ID { get; set; }
        public string EPALPageView { get; set; }
        public string p_EPAL_STATUS { get; set; }
        public string p_STNDRD_SVC_CAT { get; set; }
        public string p_STNDRD_SVC_SUBCAT { get; set; }
        public string p_ALTRNT_SVC_CAT { get; set; }
        public string p_ALTRNT_SVC_SUBCAT { get; set; }
        public string p_SUSP_IND { get; set; }
        public string p_SUSP_TYPE { get; set; }
        public DateTime? p_SUSP_EFF_DT { get; set; }
        public DateTime? p_SUSP_EXP_DT { get; set; }
        public string p_SUSP_RSN { get; set; }

        public EPAL_Procedures_Param_Dto(){}
        public EPAL_Procedures_Param_Dto(string pims_id, string ms_id)
        {
            string s = pims_id;
            string epal_ver_eff_dt = string.Empty;
            string[] items = s.Split(',');

            if (items.Length > 0)
                pims_id = items[0];

            if (items.Length > 1)
                epal_ver_eff_dt = items[1];

            p_EPAL_HIERARCHY_KEY = pims_id;
            p_EPAL_VER_EFF_DT = string.IsNullOrEmpty(epal_ver_eff_dt) ? null : DateTime.Parse(epal_ver_eff_dt.Replace("-", "/"));
            p_MS_ID = ms_id;    
        }
    }

    public class EPAL_Procedures_AssoCodes_Dto
    {
        public string p_EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? p_EPAL_VER_EFF_DT { get; set; }
        //public string p_DIAG_CD { get; set; }
        //public string p_DIAG_CD_TYPE { get; set; }
        //public string p_REV_CODES { get; set; }
        //public string p_ALLWD_PLC_OF_SVC { get; set; }

    }
    public class EPAL_Procedures_AssoCodes_T_DiagCodes_Dto
    {
        [Required]
        public string DIAG_CD { get; set; }
        [Required]
        public string DIAG_CD_TYPE { get; set; }
    }
    public class EPAL_Procedures_AssoCodes_V_DiagCodes_Dto
    {
        public string DIAG_CD { get; set; }
        public string DIAG_CD_DESC { get; set; }
        public string DIAG_CD_TYPE { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
        public string LIST_NAME { get; set; }
        public string PROG_MGD_BY { get; set; }
    }
    public class EPAL_Procedures_AssoCodes_T_RevCodes_Dto
    {
        public string REV_CD { get; set; }
        public string REV_CD_DESC { get; set; }
    }
    public class EPAL_Procedures_AssoCodes_V_RevCodes_Dto
    {
        public string REV_CD { get; set; }
        public string REV_CD_DESC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }

    public class EPAL_Procedures_AssoCodes_T_AllocatedPlaces_Dto
    {
        public string CODE { get; set; }
        public string ALLWD_PLC_OF_SVC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }
    public class EPAL_Procedures_AssoCodes_V_AllocatedPlaces_Dto
    {
        public string CODE { get; set; }
        public string ALLWD_PLC_OF_SVC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }


    public class EPAL_Procedures_AssoCodes_T_ChangeHistory_Dto
    {
        public string CHANGEBY { get; set; }
        public string CHANGEDATE { get; set; }
        public string CHANGESOURCE { get; set; }
        public string CHANGEDESCRIPTION { get; set; }
        public string ChangeType { get; set; }
    }
    public class EPAL_Procedures_AssoCodes_V_ChangeHistory_Dto
    {
        public string CHANGEBY { get; set; }
        public string CHANGEDATE { get; set; }
        public string CHANGESOURCE { get; set; }
        public string CHANGEDESCRIPTION { get; set; }
        public string ChangeType { get; set; }
    }

    public class EPAL_Procedures_AssoCodes_T_APPL_TO_STATES_Dto
    {
        public string STATE_CD { get; set; }
        public string STATE_MANDATED_IND { get; set; }
        public string INCL_EXCL_CD { get; set; }
        

    }
    public class EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto
    {
        public string STATE_CD { get; set; }
        public string STATE_NAME { get; set; }
        public string STATE_MANDATED_IND { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
        public DateTime? ATS_EFF_DT { get; set; }
        public DateTime? ATS_EXP_DT { get; set; }
        public string ATS_ISSUE_GOV { get; set; }
        public string ATS_PROG_MGD_BY { get; set; }
        public string ATS_ISSUE_GOV_DESC { get; set; }
    }


    public class EPAL_Procedures_AssoCodes_T_PROCS_MODIFIERS_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string EPAL_VER_EFF_DT { get; set; }
        public string MOD_SYS_SEQ { get; set; }
        public string MODIFIER { get; set; }
        public string MOD_DESC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }


    }
    public class EPAL_Procedures_AssoCodes_V_PROCS_MODIFIERS_Dto
    {
        public string MODIFIER { get; set; }
        public string MOD_DESC { get; set; }
        public string INCL_EXCL_CD { get; set; }
        public string INCL_EXCL_CD_DESC { get; set; }
    }

    public class EPAL_Procedures_Codes_Dto
    {
        public string p_PROC_CD { get; set; }

    }


    public class EPAL_PIMSHierarchyCode_T_XwalkByEPALBusSegCD_Dto
    {
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string EPAL_HIERARCHY_STS { get; set; }
    }

    public class EPAL_PIMSHierarchyCodeCombinationExists_Dto
    {
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string IS_EXIST { get; set; }
    }

    public class EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto
    {
        public string EPAL_BUS_SEG_CD { get; set; }
        public string COLUMN_NAME { get; set; }
        public string ACTIVE { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        //public string EPAL_PLAN_CD { get; set; }
        //public string EPAL_PRODUCT_CD { get; set; }
        //public string EPAL_FUND_ARNGMNT_CD { get; set; }
        //public string EPAL_HIERARCHY_STS { get; set; }

        public EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto(){}
        public EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto(string ePAL_BUS_SEG_CD, string cOLUMN_NAME, string ePAL_ENTITY_CD)
        {
            EPAL_BUS_SEG_CD = ePAL_BUS_SEG_CD;
            COLUMN_NAME = cOLUMN_NAME;
            EPAL_ENTITY_CD = ePAL_ENTITY_CD;
        }
    }

    public class EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto
    {
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string EPAL_HIERARCHY_STS { get; set; }
    }

    public class EPAL_Additional_Info_History_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public DateTime? EPAL_VER_EXP_DT { get; set; }
        public string FURTHER_INST { get; set; }
        public string NOTES { get; set; }

    }    

    public class EPAL_Procedures_T_Max_Prior_Auth_Dt_Dto
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string IS_CURRENT { get; set; }
        public DateTime? MAX_PRIOR_AUTH_EXP_DT { get; set; }
    }

    public class EPAL_Procedures_T_Max_PA_PRE_DT
    {
        public string EPAL_HIERARCHY_KEY { get; set; }
        public string IS_CURRENT { get; set; }        
        public DateTime? MAX_EXP_DT { get; set; }
        public DateTime EPAL_VER_EFF_DT { get; set; }
    }

    public class EPAL_Procedures_PIMS_ID_PARAM_Dto
    {
        public string p_EPAL_HIERARCHY_KEY { get; set; }
    }

    public class EPAL_Red_Ret_Param_Dto
    {
        public string p_EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? p_EPAL_VER_EFF_DT { get; set; }
        public string p_FACTOR_TYPE { get; set; }
    }
}
