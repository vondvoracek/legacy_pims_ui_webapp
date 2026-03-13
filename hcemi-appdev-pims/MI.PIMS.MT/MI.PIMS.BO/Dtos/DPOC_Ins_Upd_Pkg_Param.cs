using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    public class DPOC_Ins_Upd_Pkg_Param
    {
        public string P_DPOC_BUS_SEG_CD { get; set; }
        public string P_DPOC_ENTITY_CD { get; set; }
        public string P_DPOC_PLAN_CD { get; set; }
        public string P_DPOC_PRODUCT_CD { get; set; }
        public string P_DPOC_FUND_ARNGMNT_CD { get; set; }
        public string P_PROC_CD { get; set; }
        public string P_DRUG_NM { get; set; }
        public string P_DPOC_PACKAGE { get; set; }
        public DateTime? P_DPOC_EFF_DT { get; set; }
        public DateTime? P_DPOC_EXP_DT { get; set; }
        public string P_DPOC_ELIGIBLE_IND { get; set; }
        public string P_DPOC_INELIGIBLE_RSN { get; set; }
        public string P_DPOC_IMPLEMENTED_IND { get; set; }

        //User Story 137657 MFQ 8/26/2025

        //public string P_KL_PLCY_POLICY_ID { get; set; }
        //public string P_KL_PLCY_NM { get; set; }

        public string P_DPOC_INV_NOTES { get; set; }
        public string P_ASSOC_EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? P_ASSOC_EPAL_VER_EFF_DT { get; set; }
        public string P_DPOC_ADDTNL_RQRMNTS { get; set; }
        public string P_USER_ID { get; set; }
        public string P_CHANGE_REQ_ID { get; set; }
        public string P_CHANGE_DESC { get; set; }
        public string P_RECORD_ENTRY_METHOD { get; set; }
        public string P_UIR_RSN { get; set; }
        public string P_DPOC_RELEASE { get; set; }
        public string P_DPOC_VER_NUM { get; set; }
        public string P_IQ_GDLN_ID { get; set; }
        public string P_IQ_GDLN_VERSION { get; set; }
        public string P_IQ_GDLN_NM { get; set; }
        public string P_IQ_REFERENCE { get; set; }
        public string P_IQ_GDLN_PRODUCT_NM { get; set; }
        public string P_IQ_GDLN_PRODUCT_DESC { get; set; }
        public string P_IQ_GDLN_REL_DT { get; set; }
        public string P_IQ_GDLN_EXP_DT { get; set; }
        public string P_IQ_GDLN_DESC { get; set; }
        public string P_IQ_GDLN_RECOMMENDATION_DESC { get; set; }
        public string P_IQ_GDLN_JRSDCTN { get; set; }
        public string P_IQ_CRITERIA { get; set; }
        public string P_RULE_OUTCOME_OUTPAT { get; set; }
        public string P_RULE_OUTCOME_OUTPAT_RSN { get; set; }
        public string P_RULE_OUTCOME_OUTPAT_FCLTY { get; set; }
        public string P_RULE_OUTCOME_OUTPAT_FCLTY_RSN { get; set; }
        public string P_RULE_OUTCOME_INPAT { get; set; }
        public string P_RULE_OUTCOME_INPAT_RSN { get; set; }
        public string P_RULE_IMP_TYPE { get; set; }
        public string P_RULE_IMP_WITH { get; set; }
        public string P_RULE_EXCLUSIONS { get; set; }
        public string P_GDLN_ASSOC_EFF_DT { get; set; }
        public string P_GDLN_ASSOC_EXP_DT { get; set; }
        public string P_KL_PLCY_ID { get; set; }
        public string P_KL_PLCY_NAME { get; set; }
        public string P_GDLN_AGE_MIN { get; set; }
        public string P_GDLN_AGE_MAX { get; set; }
        public string P_DPOC_SOS_PROVIDER_TIN_EXCL { get; set; }
        public string P_PKG_CONFIG_COMMENTS { get; set; }
        //User Story 137657 MFQ 8/26/2025
        public string P_MDCR_COVG_SUM_ID { get; set; }
        public string P_MDCR_COVG_SUM_TITLE { get; set; }
        public string P_DIAG_DPOC_RELEASE { get; set; }
        public string P_DIAG_DPOC_VER_NUM { get; set; }
        public string P_DIAG_IQ_GDLN_ID { get; set; }
        public string P_DIAG_CD { get; set; }
        public string P_DIAG_INCL_EXCL_CD { get; set; }
        public string P_LIST_NAME { get; set; }
        public string P_JRSDCTN_DPOC_RELEASE { get; set; }
        public string P_JRSDCTN_DPOC_VER_NUM { get; set; }
        public string P_JRSDCTN_IQ_GDLN_ID { get; set; }
        public string P_JRSDCTN_NM { get; set; }
        public string P_JRSDCTN_IND { get; set; }
        public string P_RCMNDTN_DPOC_RELEASE { get; set; }
        public string P_RCMNDTN_DPOC_VER_NUM { get; set; }
        public string P_RCMNDTN_IQ_GDLN_ID { get; set; }
        public string P_IQ_GDLN_RCMNDTN_ID { get; set; }
        public string P_IQ_GDLN_RCMNDTN_DESC { get; set; }
        public string P_IQ_GDLN_RCMNDTN_RCHBL_IND { get; set; }
        public string P_DTQ_DPOC_RELEASE { get; set; }
        public string P_DTQ_DPOC_VER_NUM { get; set; }
        public string P_DTQ_IQ_GDLN_ID { get; set; }
        public string P_DTQ_STATES_APPL { get; set; }
        public string P_DTQ_STATES_INCL_EXCL_CD { get; set; }
        public string P_DTQ_POS_APPL { get; set; }
        public string P_DTQ_POS_INCL_EXCL_CD { get; set; }
        public string P_DTQ_NM { get; set; }
        public string P_DTQ_TYPE { get; set; }
        public string P_DTQ_RSN { get; set; }
        public string P_DTQ_ATTACH_RQST_IND { get; set; }
        public string P_MED_PLCY_REF_CODE { get; set; }
        public string P_GNTC_KL_PLCY_ID { get; set; }
        public string P_GNTC_KL_PLCY_NM { get; set; }
        public string P_HOLDING_DTQ { get; set; }
        public string P_HOLDING_DTQ_VERSION { get; set; }
        public string P_TGT_DTQ { get; set; }
        public string P_TGT_DTQ_VERSION { get; set; }
        public string P_RULE_COMMENTS { get; set; }


        public string DPOCPageView { get; set; }
        public string DPOC_ID { get; set; }
        public string DPOC_VER_EFF_DT { get; set; }
    }

    public class DPOC_Delete_Pkg_Param
    {
        public string P_DPOC_HIERARCHY_KEY { get; set; }
        public DateTime P_DPOC_VER_EFF_DT { get; set; }
        public string P_DPOC_PACKAGE { get; set; }
        public string P_USER_ID { get; set; }
        public string P_CHANGE_REQ_ID { get; set; }
        public string P_CHANGE_DESC { get; set; }
        public string P_RECORD_ENTRY_METHOD { get; set; }
    }
}
