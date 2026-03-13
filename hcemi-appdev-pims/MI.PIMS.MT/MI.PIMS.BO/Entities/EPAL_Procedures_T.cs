using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Entities
{

    [Table("epal_procedures_t", Schema = "pims_user")]
    public class EPAL_Procedures_T
    {
        [Key, Column("epal_hierarchy_key", Order = 0)]
        [MaxLength(50)]
        public string EPAL_Hierarchy_Key { get; set; }

        [Key, Column("epal_ver_eff_dt", Order = 1)]
        public DateTime EPAL_Ver_Eff_Dt { get; set; }

        [Required, Column("epal_bus_seg_cd")]
        [MaxLength(10)]
        public string EPAL_Bus_Seg_Cd { get; set; }

        [Required, Column("epal_entity_cd")]
        [MaxLength(20)]
        public string EPAL_Entity_Cd { get; set; }

        [Required, Column("epal_plan_cd")]
        [MaxLength(10)]
        public string EPAL_Plan_Cd { get; set; }

        [Required, Column("epal_product_cd")]
        [MaxLength(20)]
        public string EPAL_Product_Cd { get; set; }

        [Required, Column("epal_fund_arngmnt_cd")]
        [MaxLength(3)]
        public string EPAL_Fund_Arngmnt_Cd { get; set; }

        [Required, Column("proc_cd")]
        [MaxLength(20)]
        public string Proc_Cd { get; set; }

        [Column("drug_nm")]
        [MaxLength(100)]
        public string Drug_Nm { get; set; }

        [Column("drug_rvw_at_launch_ind")]
        [MaxLength(3)]
        public string Drug_Rvw_At_Launch_Ind { get; set; }

        [Column("prior_auth_eff_dt")]
        public DateTime? Prior_Auth_Eff_Dt { get; set; }

        [Column("prior_auth_exp_dt")]
        public DateTime? Prior_Auth_Exp_Dt { get; set; }

        [Column("auto_aprvl_eff_dt")]
        public DateTime? Auto_Aprvl_Eff_Dt { get; set; }

        [Column("auto_aprvl_exp_dt")]
        public DateTime? Auto_Aprvl_Exp_Dt { get; set; }

        [Column("mcare_spcl_prcsng_type")]
        [MaxLength(100)]
        public string Mcare_Spcl_Prcsng_Type { get; set; }

        [Column("mcare_spcl_prcsng_eff_dt")]
        public DateTime? Mcare_Spcl_Prcsng_Eff_Dt { get; set; }

        [Column("mcare_spcl_prcsng_exp_dt")]
        public DateTime? Mcare_Spcl_Prcsng_Exp_Dt { get; set; }

        [Column("sos_ind")]
        [MaxLength(3)]
        public string Sos_Ind { get; set; }

        [Column("altrnt_svc_cat")]
        [MaxLength(100)]
        public string Altrnt_Svc_Cat { get; set; }

        [Column("altrnt_svc_subcat")]
        [MaxLength(100)]
        public string Altrnt_Svc_Subcat { get; set; }

        [Column("further_inst")]
        [MaxLength(500)]
        public string Further_Inst { get; set; }

        [Column("notes")]
        [MaxLength(500)]
        public string Notes { get; set; }

        [Column("prior_auth_age_min")]
        public double? Prior_Auth_Age_Min { get; set; }

        [Column("prior_auth_age_max")]
        public double? Prior_Auth_Age_Max { get; set; }

        [Column("epal_origin_cat")]
        [MaxLength(100)]
        public string EPAL_Origin_Cat { get; set; }

        [Column("epal_origin_subcat")]
        [MaxLength(100)]
        public string EPAL_Origin_Subcat { get; set; }

        [Column("pre_det_eff_dt")]
        public DateTime? Pre_Det_Eff_Dt { get; set; }

        [Column("pre_det_exp_dt")]
        public DateTime? Pre_Det_Exp_Dt { get; set; }

        [Column("payc_hierarchy_key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(88)]
        public string Payc_Hierarchy_Key { get; set; }

        [Column("dral_eff_dt")]
        public DateTime? Dral_Eff_Dt { get; set; }

        [Column("dral_exp_dt")]
        public DateTime? Dral_Exp_Dt { get; set; }

        [Column("adv_ntfctn_eff_dt")]
        public DateTime? Adv_Ntfctn_Eff_Dt { get; set; }

        [Column("adv_ntfctn_exp_dt")]
        public DateTime? Adv_Ntfctn_Exp_Dt { get; set; }
    }
}
