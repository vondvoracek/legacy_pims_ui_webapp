using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Inv_Gdln_Rules_V_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }        
        public string IQ_CRITERIA { get; set; }
        public string IQ_GDLN_ID { get; set; }
        public string IQ_GDLN_STATUS { get; set; }
        public string IQ_GDLN_NM { get; set; }
        public string IQ_GDLN_PRODUCT_DESC { get; set; }
        public string IQ_GDLN_PRODUCT_NM { get; set; }
        public string IQ_GDLN_DESC { get; set; }
        public string IQ_GDLN_RECOMMENDATION_DESC { get; set; }
        public string IQ_REFERENCE { get; set; }
        public DateTime? IQ_GDLN_REL_DT { get; set; }
        public DateTime? IQ_GDLN_EXP_DT { get; set; }
        public int IQ_GDLN_RULES_SYS_SEQ { get; set; }
        public string IQ_GDLN_VERSION { get; set; }
        public string IQ_GDLN_JRSDCTN { get; set; }
        public string RULE_IMP_TYPE { get; set; }
        public string RULE_IMP_WITH { get; set; }
        public string RULE_TYPE_OUTPAT { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT { get; set; }
        public string RULE_TYPE_RSN_OUTPAT { get; set; }
        public string RULE_TYPE_OUTPAT_FCLTY { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT_FCLTY { get; set; }
        public string RULE_TYPE_RSN_OUTPAT_FCLTY { get; set; }
        public string RULE_TYPE_INPAT { get; set; }
        public string RULE_TYPE_OUTCOME_INPAT { get; set; }
        public string RULE_TYPE_RSN_INPAT { get; set; }
        public string RULE_COMMENTS { get; set; }
        public string RULE_EXCLUSIONS { get; set; }
        public string KL_PLCY_ID { get; set; }
        public string KL_PLCY_NAME { get; set; }
        public string DTQ_APPLIES { get; set; }
        public DateTime? GDLN_ASSOC_EFF_DT { get; set; }
        public DateTime? GDLN_ASSOC_EXP_DT { get; set; }
        public string PageType { get; set; }
        public string PendingRowId { get; set; }
        public string ActiveRowId { get; set; }
        
        // Jurisdiction
        public string JRSDCTN_NM { get; set; }
        public string JRSDCTN_IQ_GDLN_ID { get; set; }
        public string JRSDCTN_IND { get; set; }
        
        // DTQ Fields MFQ 10/2/2024
        public string HOLDING_DTQ { get; set; }
        public string HOLDING_DTQ_VERSION { get; set; }
        public string TGT_DTQ { get; set; }
        public string TGT_DTQ_VERSION { get; set; }
        public string DTQ_RSN { get; set; }

        // Guideline AGE 
        public int GDLN_AGE_MIN { get; set; }
        public int GDLN_AGE_MAX { get; set; }

        //Apply Fields
        public int STATES_APPLY { get; set; }
        public int STATES_APPL { get; set; }
        public int DX_APPLY { get; set; }
        public int POS_APPLY { get; set; }
        public int DTQ_APPLY { get; set; }

        public string DPOC_SOS_PROVIDER_TIN_EXCL { get; set; } //User Story 132092 MFQ 5/14/2025
        //USER STORY 128895 MFQ 5/20/2025
        public string DPOC_RELEASE { get; set; }
        public string DPOC_VER_NUM { get; set; }
        public string PKG_CONFIG_COMMENTS { get; set; } // User Story 138385 MFQ 9/22/2025

        //Medicare Associated Coverage Summary Title and Summary ID USER STORY 137657
        public string MDCR_COVG_SUM_ID { get; set; }
        public string MDCR_COVG_SUM_TITLE { get; set; }

        //User Story 138537 MFQ 12/1/2025
        //public string DPOC_ADDTNL_RQRMNTS { get; set; }

    }

    public class DPOC_Inv_Gdln_Rules_PND_V_Dto
    {
        public string IQ_GDLN_STATUS { get; set; }
        public string IQ_GDLN_ID { get; set; }
        public string PROC_CD { get; set; }
        public string IQ_REFERENCE { get; set; }
        public string IQ_GDLN_PRODUCT_NM { get; set; }
        public string IQ_GDLN_PRODUCT_DESC { get; set; }
        public string IQ_GDLN_VERSION { get; set; }
        public string IQ_GDLN_REL_DT { get; set; }
        public string IQ_GDLN_EXP_DT { get; set; }
        public string IQ_GDLN_NM { get; set; }
        public string IQ_GDLN_DESC { get; set; }
        public string IQ_GDLN_RECOMMENDATION_DESC { get; set; }
        public string IQ_GDLN_JRSDCTN { get; set; }
    }

    public class DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto
    {
        public string iq_gdln_status { get; set; }
        public string iq_gdln_id { get; set; }
        public string proc_cd { get; set; }
        public string iq_reference { get; set; }
        public string iq_gdln_product_nm { get; set; }
        public string iq_gdln_product_desc { get; set; }
        public string iq_gdln_version { get; set; }
        //public DateTime? iq_gdln_rel_dt { get; set; }
        //public DateTime? iq_gdln_exp_dt { get; set; }
        public string iq_gdln_nm { get; set; }
        public string iq_gdln_desc { get; set; }
        public string iq_gdln_recommendation_desc { get; set; }
        public string iq_gdln_jrsdctn { get; set; }
    }


    public class DPOC_Inv_Gdln_Rules_Param_Dto
    {
        public string p_DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? p_DPOC_VER_EFF_DT { get; set; }
        public string p_IQ_GDLN_STATUS { get; set; }
        public string p_DPOC_PACKAGE { get; set; }
        public string p_DPOC_RELEASE { get; set; }
        public int P_IS_HISTORICAL { get; set; }
    }

    public class DPOC_Inv_Gdln_Rules_V_Key
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public string IQ_GDLN_ID { get; set; }
        public string RULE_OUTCOME_OUTPAT { get; set; }
        public string RULE_OUTCOME_OUTPAT_FCLTY { get; set; }
        public string RULE_OUTCOME_INPAT { get; set; }
        public string PageType { get; set; }
    }

    public class GS_Current_Status_Dto
    {
        public string STATUS_CD { get; set; }
        public string STATUS_DESC { get; set; }
    }

    public class DTQ_Applies_Dto
    {
        public string DTQ_CD { get; set; }
        public string DTQ_DESC { get; set; }
    }

    public class InPatientTypeRule
    {
        public string RULE_TYPE_INPAT { get; set; }
        public string RULE_TYPE_OUTCOME_INPAT { get; set; }
    }
    public class OutPatientTypeRule
    {
        public string RULE_TYPE_OUTPAT { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT { get; set; }
    }
    public class OutPatientFacTypeRule
    {
        public string RULE_TYPE_OUTPAT_FCLTY { get; set; }
        public string RULE_TYPE_OUTCOME_OUTPAT_FCLTY { get; set; }
    }
    public class RuleTypeReason
    {
        public string RULE_TYPE_RSN { get; set; }
        public string RULE_TYPE_RSN_DESC { get; set; }
    }

    public class DPOC_Inv_Gdln_Diagnoses_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public string DIAG_IQ_GDLN_ID { get; set; }
        public int GDLN_DIAG_SYS_SEQ { get; set; }
        public string DIAG_INCL_EXCL_CD { get; set; }
        public string LIST_NAME { get; set; }
        public string DIAG_CD { get; set; }        
        public string DIAG_CD_DESC { get; set; }
        public string DIAG_CD_TYPE { get; set; }
        public string DIAG_INCL_EXCL_CD_DESC { get; set; }
        public string LIST_NAME_CODE { get; set; }
        public string DPOC_VER_NUM { get; set; }
        public bool hasChildren { get; set; }
    }

    public class DPOC_Gdln_Param_Dto
    {
        public string p_DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? p_DPOC_VER_EFF_DT { get; set; }
        public string p_IQ_GDLN_ID { get; set; }
        public string p_DPOC_PACKAGE { get; set; }
        public string p_DPOC_RELEASE { get; set; }
        public int p_GDLN_DTQ_SYS_SEQ { get; set; }
        public string p_DPOC_VER_NUM { get; set; }        
    }

    public class DPOC_Guidelines_Dto
    {
        public string proc_cd { get; set; }
        public string iq_reference { get; set; }
        public string iq_gdln_id { get; set; }
        public string iq_gdln_version { get; set; }
    }
}
