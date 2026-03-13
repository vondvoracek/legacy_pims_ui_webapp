using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Entities
{
    [Table("dpoc_inventories_v", Schema = "pims_user")]
    public class DPOC_Inventories_V
    {

        [Column("dpoc_hierarchy_key")]
        public string Dpoc_Hierarchy_Key { get; set; }

        [Column("dpoc_bus_seg_cd")]
        public string Dpoc_Bus_Seg_Cd { get; set; }

        [Column("dpoc_bus_seg_desc")]
        public string Dpoc_Bus_Seg_Desc { get; set; }

        [Column("dpoc_entity_cd")]
        public string Dpoc_Entity_Cd { get; set; }

        [Column("dpoc_entity_desc")]
        public string Dpoc_Entity_Desc { get; set; }

        [Column("dpoc_plan_cd")]
        public string Dpoc_Plan_Cd { get; set; }

        [Column("dpoc_plan_desc")]
        public string Dpoc_Plan_Desc { get; set; }

        [Column("dpoc_product_cd")]
        public string Dpoc_Product_Cd { get; set; }

        [Column("dpoc_product_desc")]
        public string Dpoc_Product_Desc { get; set; }

        [Column("dpoc_fund_arngmnt_cd")]
        public string Dpoc_Fund_Arngmnt_Cd { get; set; }

        [Column("dpoc_fund_arngmnt_desc")]
        public string Dpoc_Fund_Arngmnt_Desc { get; set; }

        [Column("proc_cd")]
        public string Proc_Cd { get; set; }

        [Column("drug_nm")]
        public string Drug_Nm { get; set; }

        [Column("dpoc_ver_eff_dt")]
        public DateTime? Dpoc_Ver_Eff_Dt { get; set; }

        [Column("dpoc_ver_exp_dt")]
        public DateTime? Dpoc_Ver_Exp_Dt { get; set; }

        [Column("dpoc_ver_num")]
        public int Dpoc_Ver_Num { get; set; }

        [Column("dpoc_package")]
        public string Dpoc_Package { get; set; }

        [Column("dpoc_release")]
        public string Dpoc_Release { get; set; }

        [Column("dpoc_eff_dt")]
        public DateTime? Dpoc_Eff_Dt { get; set; }

        [Column("dpoc_exp_dt")]
        public DateTime? Dpoc_Exp_Dt { get; set; }

        [Column("dpoc_status")]
        public string Dpoc_Status { get; set; }

        [Column("dpoc_eligible_ind")]
        public string Dpoc_Eligible_Ind { get; set; }

        [Column("dpoc_ineligible_rsn")]
        public string Dpoc_Ineligible_Rsn { get; set; }

        [Column("dpoc_implemented_ind")]
        public string Dpoc_Implemented_Ind { get; set; }

        [Column("dtq_applies")]
        public string Dtq_Applies { get; set; }

        [Column("dpoc_inv_notes")]
        public string Dpoc_Inv_Notes { get; set; }

        [Column("dpoc_addtnl_rqrmnts")]
        public string Dpoc_Addtnl_Rqrmnts { get; set; }

        [Column("dpoc_sos_provider_tin_excl")]
        public string Dpoc_Sos_Provider_Tin_Excl { get; set; }

        [Column("kl_plcy_policy_id")]
        public string Kl_Plcy_Policy_Id { get; set; }

        [Column("kl_plcy_nm")]
        public string Kl_Plcy_Nm { get; set; }

        [Column("epal_hierarchy_key")]
        public string Epal_Hierarchy_Key { get; set; }

        [Column("epal_ver_eff_dt")]
        public DateTime? Epal_Ver_Eff_Dt { get; set; }

        [Column("epal_ver_exp_dt")]
        public DateTime? Epal_Ver_Exp_Dt { get; set; }

        [Column("epal_stndrd_svc_cat")]
        public string Epal_Stndrd_Svc_Cat { get; set; }

        [Column("epal_stndrd_svc_subcat")]
        public string Epal_Stndrd_Svc_Subcat { get; set; }

        [Column("epal_altrnt_svc_cat")]
        public string Epal_Altrnt_Svc_Cat { get; set; }

        [Column("epal_altrnt_svc_subcat")]
        public string Epal_Altrnt_Svc_Subcat { get; set; }

        [Column("epal_svc_cat")]
        public string Epal_Svc_Cat { get; set; }

        [Column("epal_svc_subcat")]
        public string Epal_Svc_Subcat { get; set; }

        [Column("epal_proc_cd_type")]
        public string Epal_Proc_Cd_Type { get; set; }

        [Column("epal_proc_cd_desc")]
        public string Epal_Proc_Cd_Desc { get; set; }

        [Column("epal_proc_cd_status")]
        public string Epal_Proc_Cd_Status { get; set; }

        [Column("epal_prior_auth_eff_dt")]
        public DateTime? Epal_Prior_Auth_Eff_Dt { get; set; }

        [Column("epal_prior_auth_exp_dt")]
        public DateTime? Epal_Prior_Auth_Exp_Dt { get; set; }

        [Column("epal_sos_ind")]
        public string Epal_Sos_Ind { get; set; }

        [Column("epal_sos_eff_dt")]
        public DateTime? Epal_Sos_Eff_Dt { get; set; }

        [Column("epal_sos_exp_dt")]
        public DateTime? Epal_Sos_Exp_Dt { get; set; }

        [Column("prog_mgd_by")]
        public string Prog_Mgd_By { get; set; }

    }


    public class DPOC_Inv_Gdln_Rules_T
    {
        public string dpoc_hierarchy_key { get; set; }
        public DateTime? dpoc_ver_eff_dt { get; set; }
        public string dpoc_package { get; set; }
        public string dpoc_release { get; set; }
        public long? iq_gdln_rules_sys_seq { get; set; }
        public string iq_gdln_id { get; set; }
        public string iq_gdln_version { get; set; }
        public string iq_gdln_nm { get; set; }
        public string iq_criteria { get; set; }
        public string rule_outcome_outpat { get; set; }
        public string rule_outcome_outpat_rsn { get; set; }
        public string rule_outcome_outpat_fclty { get; set; }
        public string rule_outcome_outpat_fclty_rsn { get; set; }
        public string rule_outcome_inpat { get; set; }
        public string rule_outcome_inpat_rsn { get; set; }
        public string rule_imp_type { get; set; }
        public string rule_imp_with { get; set; }
        public string rule_exclusions { get; set; }
        public DateTime? gdln_assoc_eff_dt { get; set; }
        public DateTime? gdln_assoc_exp_dt { get; set; }
        public string kl_plcy_policy_id { get; set; }
        public string kl_plcy_nm { get; set; }
        public string iq_reference { get; set; }
        public string iq_gdln_product_nm { get; set; }
        public string iq_gdln_product_desc { get; set; }
        public DateTime? iq_gdln_rel_dt { get; set; }
        public DateTime? iq_gdln_exp_dt { get; set; }
        public string iq_gdln_desc { get; set; }
        public string iq_gdln_recommendation_desc { get; set; }
        public string iq_gdln_jrsdctn { get; set; }
        public long? gdln_age_min { get; set; }
        public long? gdln_age_max { get; set; }
        public string dpoc_sos_provider_tin_excl { get; set; }
        public string pkg_config_comments { get; set; }
        public string dpoc_ver_num { get; set; }
        public string mdcr_covg_sum_id { get; set; }
        public string mdcr_covg_sum_title { get; set; }
    }
}
