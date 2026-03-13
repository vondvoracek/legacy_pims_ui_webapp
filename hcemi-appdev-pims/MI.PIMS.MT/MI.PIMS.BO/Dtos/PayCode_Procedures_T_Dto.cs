using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{


    public class PayCode_Procedures_V_Dto
    {
        public string PAYC_HIERARCHY_KEY { get; set; }
        public string PAYC_BUS_SEG_CD { get; set; }
        public string PAYC_ENTITY_CD { get; set; }
        public string PAYC_PROC_CD { get; set; }
        public DateTime? PAYC_VER_EFF_DT { get; set; }
        public string PAYC_PLAN_CD { get; set; }
        public string PAYC_PRODUCT_CD { get; set; }
        public string PAYC_KL_PCS { get; set; }
        public string PAYC_NDB_PCS { get; set; }
        public string PAYC_NDB_REMARK_CD { get; set; }
        public string PAYC_ICES_IND { get; set; }
        public string PAYC_ICES_EDIT_ACTION { get; set; }
        public string PAYC_ADVN_NOTIF { get; set; }
        public DateTime? PAYC_PRED_EFF_DT { get; set; }
        public DateTime? PAYC_PRED_EXP_DT { get; set; }
        public string PAYC_MCR_ROUTED { get; set; }
        public string PAYC_BIFURCATED { get; set; }
        public string PAYC_NS88_COMPLIANCE { get; set; }
        public string PAYC_ADDITIONAL_EDITS { get; set; }
        public string PAYC_COMMENTS { get; set; }
        public string PAYC_ICES_EDIT_NAME { get; set; }
        public string PAYC_PRED_IND { get; set; }
        public DateTime? PAYC_EFF_DT { get; set; }
        public DateTime? PAYC_EXP_DT { get; set; }
        public string PAYC_FUND_ARNGMNT_CD { get; set; }
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? MAX_EFF_DT { get; set; }
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string PROC_CD { get; set; }
        public string EPAL_STATUS { get; set; }
        public DateTime? EPAL_EFF_DT { get; set; }
        public DateTime? EPAL_EXP_DT { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EXP_DT { get; set; }
        public string EPAL_PRE_DET_STATUS { get; set; }
        public DateTime? EPAL_PRE_DET_EFF_DT { get; set; }
        public DateTime? EPAL_PRE_DET_EXP_DT { get; set; }
        public string EPAL_SOS_IND { get; set; }
        public DateTime? EPAL_SOS_EFF_DT { get; set; }
        public DateTime? EPAL_SOS_EXP_DT { get; set; }
        public string EPAL_DX_IND { get; set; }
        public string PAYC_STATUS { get; set; }
        public string EPAL_ALT_CAT { get; set; }
        public string EPAL_ALT_SUB_CAT { get; set; }
        public bool IS_EDITABLE { get; set; }
        public string LST_UPDT_BY { get; set; }
        public string PAYC_PRE_D { get; set; }
        public string FURTHER_INST { get; set; }
        public string NOTES { get; set; }
        public string CHANGE_REQ_ID { get; set; }
        public string CHANGE_DESC { get; set; }
        public int ROW_NUMBER { get; set; }
        public string PAYC_FURTHER_CONSIDERATIONS { get; set; }
        public string PAYC_NOTES { get; set; }
        public string NOTES_FURTHER_CONSIDERATIONS { get; set; }
        public string NOTES_TYPE { get; set; }
        public string IS_CURRENT { get; set; }
        public string PROC_CD_DESC { get; set; }


    }





    public class PayCode_Procedures_Param_Dto
    {
        public int p_INCLUDE_HISTORICAL { get; set; }

        public string p_PAYC_HIERARCHY_KEY { get; set; }
        public string p_PAYC_STATUS { get; set; }
        public string p_PAYC_KL_PCS { get; set; }
        public string p_PAYC_NDB_PCS { get; set; }


        public string p_PROC_CD { get; set; }
        public string p_ALTRNT_SVC_CAT { get; set; }
        public string p_ALTRNT_SVC_SUBCAT { get; set; }
        public string p_iCES { get; set; }
        public string p_PRIOR_AUTH_STATUS { get; set; }


        public DateTime? p_CURRENT_EFF_DT { get; set; }
        public DateTime? p_CURRENT_EXP_DT { get; set; }
        public DateTime? p_PAYC_VER_EFF_DT { get; set; }
        public string p_PAYC_PLAN_CD { get; set; }
        public string p_PAYC_BUS_SEG_CD { get; set; }
        public string p_PAYC_PRODUCT_CD { get; set; }
        public string p_PAYC_ENTITY_CD { get; set; }
        public string p_PAYC_ICES_EDIT_NAME { get; set; }
        public string p_PAYC_ICES_EDIT_ACTION { get; set; }


        public string p_MS_ID { get; set; }

    }

    public class PayCode_Procedures_T_Dto
    {
        public string PAYC_HIERARCHY_KEY { get; set; }
        public string PAYC_BUS_SEG_CD { get; set; }
        public string PAYC_ENTITY_CD { get; set; }
        public string PAYC_PROC_CD { get; set; }
        public DateTime? PAYC_VER_EFF_DT { get; set; }
        public string PAYC_PLAN_CD { get; set; }
        public string PAYC_PRODUCT_CD { get; set; }
        public string PAYC_KL_PCS { get; set; }
        public string PAYC_NDB_PCS { get; set; }
        public string PAYC_NDB_REMARK_CD { get; set; }
        public string PAYC_ICES_IND { get; set; }
        public string PAYC_ICES_EDIT_ACTION { get; set; }
        public string PAYC_ADVN_NOTIF { get; set; }
        public DateTime? PAYC_PRED_EFF_DT { get; set; }
        public DateTime? PAYC_PRED_EXP_DT { get; set; }
        public string PAYC_MCR_ROUTED { get; set; }
        public string PAYC_BIFURCATED { get; set; }
        public string PAYC_NS88_COMPLIANCE { get; set; }
        public string PAYC_ADDITIONAL_EDITS { get; set; }
        public string PAYC_COMMENTS { get; set; }
        public string PAYC_ICES_EDIT_NAME { get; set; }
        public string PAYC_PRED_IND { get; set; }
        public DateTime? PAYC_EFF_DT { get; set; }
        public DateTime? PAYC_EXP_DT { get; set; }
        public string PAYC_FUND_ARNGMNT_CD { get; set; }
        public string EPAL_HIERARCHY_KEY { get; set; }
        public DateTime? MAX_EFF_DT { get; set; }
        public string EPAL_BUS_SEG_CD { get; set; }
        public string EPAL_ENTITY_CD { get; set; }
        public string EPAL_PLAN_CD { get; set; }
        public string EPAL_PRODUCT_CD { get; set; }
        public string EPAL_FUND_ARNGMNT_CD { get; set; }
        public string PROC_CD { get; set; }
        public string EPAL_STATUS { get; set; }
        public DateTime? EPAL_EFF_DT { get; set; }
        public DateTime? EPAL_EXP_DT { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? EPAL_PRIOR_AUTH_EXP_DT { get; set; }
        public string EPAL_PRE_DET_STATUS { get; set; }
        public DateTime? EPAL_PRE_DET_EFF_DT { get; set; }
        public DateTime? EPAL_PRE_DET_EXP_DT { get; set; }
        public string EPAL_SOS_IND { get; set; }
        public DateTime? EPAL_SOS_EFF_DT { get; set; }
        public DateTime? EPAL_SOS_EXP_DT { get; set; }
        public string EPAL_DX_IND { get; set; }
        public string PAYC_STATUS { get; set; }
        public bool IS_EDITABLE { get; set; }
        public string LST_UPDT_BY { get; set; }
        public string PAYC_PRE_D { get; set; }
        public string FURTHER_INST { get; set; }
        public string NOTES { get; set; }
        public string CHANGE_REQ_ID { get; set; }
        public string CHANGE_DESC { get; set; }
        public string PAYC_FURTHER_CONSIDERATIONS { get; set; }
        public string PAYC_NOTES { get; set; }
        public string NOTES_TYPE { get; set; }
        public string NOTES_FURTHER_CONSIDERATIONS { get; set; }
        public string IS_CURRENT { get; set; }
        public string PROC_CD_DESC { get; set; }
        public string PAGE_TYPE { get; set; }
    }

    public class PayCode_MCR_Routed_Dto
    {
        public string MCR_ROUTED { get; set; }

    }

    public class IsCurrentRecordDto
    {
        public string PIMS_ID { get; set; }
        public DateTime? PIMS_VER_EFF_DT { get; set; }
        public string IS_CURRENT { get; set; }
        public string p_PIMS_ID { get; set; }
        public DateTime? p_PIMS_VER_EFF_DT { get; set; }
        public string p_MODULE_NAME { get; set; }


    }

    public class PayCode_EPAL_Summary_Dto
    {
        public string PAY_CODE { get; set; }
        public string PIMS_EPAL_ID { get; set; }
        public string PRIOR_AUTH_STATUS { get; set; }
        public DateTime? PRIOR_AUTH_EFF_DT { get; set; }
        public DateTime? PRIOR_AUTH_EXP_DT { get; set; }
        public string SOS_APPLIES { get; set; }
        public DateTime? SOS_EFF_DT { get; set; }
        public DateTime? SOS_EXP_DT { get; set; }
        public string ALTRNT_SVC_CAT { get; set; }
        public string ALTRNT_SVC_SUBCAT { get; set; }
        public string DX_INDICATOR { get; set; }
        public DateTime? EPAL_VER_EFF_DT { get; set; }
        public DateTime? EPAL_VER_EXP_DT { get; set; }

    }
    public class PayCode_ChangeHistory_Dto
    {
        public string CHANGEBY { get; set; }
        public string CHANGEDATE { get; set; }
        public string CHANGESOURCE { get; set; }
        public string CHANGEDESCRIPTION { get; set; }
        public string ChangeType { get; set; }
    }
    public class PayCode_PIMSHierarchyCode_V_Xwalk_Dto
    {
        public string PAYC_BUS_SEG_CD { get; set; }
        public string PAYC_ENTITY_CD { get; set; }
        public string PAYC_PLAN_CD { get; set; }
        public string PAYC_PRODUCT_CD { get; set; }
        public string PAYC_FUND_ARNGMNT_CD { get; set; }
        public string PAYC_HIERARCHY_STS { get; set; }
    }

    public class PayCodeFiltersDto
    {
        public string EPAL_BUS_SEG_CD { get; set; }
        public string COLUMN_NAME { get; set; }
        public string ACTIVE { get; set; }
    }

    public class PayCodeHierarchyCodesDto
    {
        public string COLUMN_NAME { get; set; }
    }

    public class PayCode_Ins_Upd_Pkg_Param
    {
        public string p_payc_hierarchy_key { get; set; }
        public string p_payc_bus_seg_cd { get; set; }
        public string p_payc_entity_cd { get; set; }
        public string p_payc_proc_cd { get; set; }
        public string p_payc_plan_cd { get; set; }
        public string p_payc_product_cd { get; set; }
        public string p_payc_fund_arngmnt_cd { get; set; }
        public DateTime? p_payc_eff_dt { get; set; }
        public DateTime? p_payc_exp_dt { get; set; }
        public DateTime? p_payc_pred_eff_dt { get; set; }
        public DateTime? p_payc_pred_exp_dt { get; set; }
        public string p_payc_kl_pcs { get; set; }
        public string p_payc_ndb_pcs { get; set; }
        public string p_payc_ndb_remark_cd { get; set; }
        public string p_payc_ices_edit_name { get; set; }
        public string p_payc_ices_ind { get; set; }
        public string p_payc_pred_ind { get; set; }
        public string p_payc_ices_edit_action { get; set; }
        public string p_payc_advn_notif { get; set; }
        public string p_payc_mcr_routed { get; set; }
        public string p_payc_bifurcated { get; set; }
        public string p_payc_ns88_compliance { get; set; }
        public string p_payc_additional_edits { get; set; }
        public string p_payc_comments { get; set; }
        public string p_further_inst { get; set; }
        public string p_notes { get; set; }
        public string p_user_id { get; set; }
        public string p_change_req_id { get; set; }
        public string p_change_desc { get; set; }
    }

    public class PayCode_Delete_Pkg_Param
    {
        public string p_payc_hierarchy_key { get; set; }
        public DateTime? p_payc_ver_eff_dt { get; set; }
        public string p_user_id { get; set; }
        public string p_change_req_id { get; set; }
        public string p_change_desc { get; set; }
    }

    public class PayCode_Historic_Ins_Upd_Pkg_Param
    {
        public string p_payc_bus_seg_cd { get; set; }
        public string p_payc_entity_cd { get; set; }
        public string p_payc_proc_cd { get; set; }
        public DateTime? p_payc_ver_eff_dt { get; set; }
        public string p_payc_plan_cd { get; set; }
        public string p_payc_product_cd { get; set; }
        public string p_payc_kl_pcs { get; set; }
        public string p_payc_ndb_pcs { get; set; }
        public string p_payc_ndb_remark_cd { get; set; }
        public string p_payc_ices_ind { get; set; }
        public string p_payc_ices_edit_action { get; set; }
        public string p_payc_advn_notif { get; set; }
        public DateTime? p_payc_pred_eff_dt { get; set; }
        public DateTime? p_payc_pred_exp_dt { get; set; }
        public string p_payc_mcr_routed { get; set; }
        public string p_payc_bifurcated { get; set; }
        public string p_payc_ns88_compliance { get; set; }
        public string p_payc_additional_edits { get; set; }
        public string p_payc_comments { get; set; }
        public string p_payc_ices_edit_name { get; set; }
        public string p_payc_pred_ind { get; set; }
        public DateTime? p_payc_eff_dt { get; set; }
        public DateTime? p_payc_exp_dt { get; set; }
        public string p_payc_fund_arngmnt_cd { get; set; }
        public string p_user_id { get; set; }
        public string p_change_req_id { get; set; }
        public string p_change_desc { get; set; }
        public string p_payc_further_considerations { get; set; }
        public string p_payc_notes { get; set; }
    }
}