using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Entities
{


    [Table("dpoc_inv_gdln_rules_act_ret_v", Schema = "pims_user")]
    public class DPOC_Inv_Gdln_Rules_Act_Ret_V
    {
        [Column("dpoc_hierarchy_key")]
        public string Dpoc_Hierarchy_Key { get; set; }

        [Column("dpoc_bus_seg_cd")]
        public string Dpoc_Bus_Seg_Cd { get; set; }

        [Column("dpoc_entity_cd")]
        public string Dpoc_Entity_Cd { get; set; }

        [Column("dpoc_plan_cd")]
        public string Dpoc_Plan_Cd { get; set; }

        [Column("dpoc_product_cd")]
        public string Dpoc_Product_Cd { get; set; }

        [Column("dpoc_fund_arngmnt_cd")]
        public string Dpoc_Fund_Arngmnt_Cd { get; set; }

        [Column("dpoc_proc_cd")]
        public string Dpoc_Proc_Cd { get; set; }

        [Column("dpoc_drug_node")]
        public string Dpoc_Drug_Node { get; set; }

        [Column("dpoc_ver_eff_dt")]
        public DateTime Dpoc_Ver_Eff_Dt { get; set; }

        [Column("dpoc_package")]
        public string Dpoc_Package { get; set; }

        [Column("dpoc_release")]
        public string Dpoc_Release { get; set; }

        [Column("dpoc_ver_num")]
        public string Dpoc_Ver_Num { get; set; }

        [Column("iq_gdln_rules_sys_seq")]
        public long? Iq_Gdln_Rules_Sys_Seq { get; set; }

        [Column("iq_gdln_status")]
        public string Iq_Gdln_Status { get; set; }

        [Column("gdln_assoc_eff_dt")]
        public DateTime? Gdln_Assoc_Eff_Dt { get; set; }

        [Column("gdln_assoc_exp_dt")]
        public DateTime? Gdln_Assoc_Exp_Dt { get; set; }

        [Column("iq_gdln_id")]
        public string Iq_Gdln_Id { get; set; }

        [Column("iq_reference")]
        public string Iq_Reference { get; set; }

        [Column("iq_gdln_product_nm")]
        public string Iq_Gdln_Product_Nm { get; set; }

        [Column("iq_gdln_product_desc")]
        public string Iq_Gdln_Product_Desc { get; set; }

        [Column("iq_gdln_version")]
        public string Iq_Gdln_Version { get; set; }

        [Column("iq_gdln_rel_dt")]
        public DateTime? Iq_Gdln_Rel_Dt { get; set; }

        [Column("iq_gdln_exp_dt")]
        public DateTime? Iq_Gdln_Exp_Dt { get; set; }

        [Column("iq_gdln_nm")]
        public string Iq_Gdln_Nm { get; set; }

        [Column("iq_gdln_desc")]
        public string Iq_Gdln_Desc { get; set; }

        [Column("iq_gdln_recommendation_desc")]
        public string Iq_Gdln_Recommendation_Desc { get; set; }

        [Column("iq_gdln_jrsdctn")]
        public string Iq_Gdln_Jrsdctn { get; set; }

        [Column("iq_criteria")]
        public string Iq_Criteria { get; set; }

        [Column("rule_type_outpat")]
        public string Rule_Type_Outpat { get; set; }

        [Column("rule_type_outcome_outpat")]
        public string Rule_Type_Outcome_Outpat { get; set; }

        [Column("rule_type_rsn_outpat")]
        public string Rule_Type_Rsn_Outpat { get; set; }

        [Column("rule_type_outpat_fclty")]
        public string Rule_Type_Outpat_Fclty { get; set; }

        [Column("rule_type_outcome_outpat_fclty")]
        public string Rule_Type_Outcome_Outpat_Fclty { get; set; }

        [Column("rule_type_rsn_outpat_fclty")]
        public string Rule_Type_Rsn_Outpat_Fclty { get; set; }

        [Column("rule_type_inpat")]
        public string Rule_Type_Inpat { get; set; }

        [Column("rule_type_outcome_inpat")]
        public string Rule_Type_Outcome_Inpat { get; set; }

        [Column("rule_type_rsn_inpat")]
        public string Rule_Type_Rsn_Inpat { get; set; }

        [Column("rule_exclusions")]
        public string Rule_Exclusions { get; set; }

        [Column("rule_imp_type")]
        public string Rule_Imp_Type { get; set; }

        [Column("rule_imp_with")]
        public string Rule_Imp_With { get; set; }

        [Column("kl_plcy_policy_id")]
        public string Kl_Plcy_Policy_Id { get; set; }

        [Column("kl_plcy_nm")]
        public string Kl_Plcy_Nm { get; set; }

        [Column("gdln_age_min")]
        public long Gdln_Age_Min { get; set; }

        [Column("gdln_age_max")]
        public long Gdln_Age_Max { get; set; }

        [Column("dpoc_sos_provider_tin_excl")]
        public string Dpoc_Sos_Provider_Tin_Excl { get; set; }

        [Column("pkg_config_comments")]
        public string Pkg_Config_Comments { get; set; }
    }


}
