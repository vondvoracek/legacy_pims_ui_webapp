using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Entities
{

    [Table("dpoc_inventories_t", Schema = "pims_user")]

    public class DPOC_Inventories_T
    {
        [Key, Column("dpoc_hierarchy_key", Order = 0)]
        [MaxLength(100)]
        public string DPOC_Hierarchy_Key { get; set; }

        [Key, Column("dpoc_ver_eff_dt", Order = 1)]
        public DateTime DPOC_Ver_Eff_Dt { get; set; }

        [Key, Column("dpoc_package", Order = 2)]
        [MaxLength(100)]
        public string DPOC_Package { get; set; }

        [Required, Column("dpoc_bus_seg_cd")]
        [MaxLength(10)]
        public string DPOC_Bus_Seg_Cd { get; set; }

        [Required, Column("dpoc_entity_cd")]
        [MaxLength(20)]
        public string DPOC_Entity_Cd { get; set; }

        [Required, Column("dpoc_plan_cd")]
        [MaxLength(10)]
        public string DPOC_Plan_Cd { get; set; }

        [Required, Column("dpoc_product_cd")]
        [MaxLength(20)]
        public string DPOC_Product_Cd { get; set; }

        [Required, Column("dpoc_fund_arngmnt_cd")]
        [MaxLength(3)]
        public string DPOC_Fund_Arngmnt_Cd { get; set; }

        [Required, Column("proc_cd")]
        [MaxLength(20)]
        public string Proc_Cd { get; set; }

        [Column("drug_nm")]
        [MaxLength(100)]
        public string? Drug_Nm { get; set; }

        [Column("dpoc_release")]
        [MaxLength(20)]
        public string? DPOC_Release { get; set; }

        [Column("dpoc_eff_dt")]
        public DateTime? DPOC_Eff_Dt { get; set; }

        [Column("dpoc_exp_dt")]
        public DateTime? DPOC_Exp_Dt { get; set; }

        [Column("dpoc_eligible_ind")]
        [MaxLength(3)]
        public string? DPOC_Eligible_Ind { get; set; }

        [Column("dpoc_ineligible_rsn")]
        [MaxLength(100)]
        public string? DPOC_Ineligible_Rsn { get; set; }

        [Column("dpoc_implemented_ind")]
        [MaxLength(3)]
        public string? DPOC_Implemented_Ind { get; set; }

        [Column("kl_plcy_policy_id")]
        [MaxLength(20)]
        public string? Kl_Plcy_Policy_Id { get; set; }

        [Column("kl_plcy_nm")]
        [MaxLength(160)]
        public string? Kl_Plcy_Nm { get; set; }

        [Column("dpoc_inv_notes")]
        [MaxLength(500)]
        public string? DPOC_Inv_Notes { get; set; }

        [Column("assoc_epal_hierarchy_key")]
        [MaxLength(50)]
        public string? Assoc_Epal_Hierarchy_Key { get; set; }

        [Column("assoc_epal_ver_eff_dt")]
        public DateTime? Assoc_Epal_Ver_Eff_Dt { get; set; }

        [Column("dpoc_addtnl_rqrmnts")]
        [MaxLength(500)]
        public string? DPOC_Addtnl_Rqrmnts { get; set; }

        [Column("dpoc_sos_provider_tin_excl")]
        [MaxLength(50)]
        public string? DPOC_Sos_Provider_Tin_Excl { get; set; }
    }

}
