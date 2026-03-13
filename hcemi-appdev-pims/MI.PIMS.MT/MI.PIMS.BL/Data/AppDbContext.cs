using MI.PIMS.BO.Dtos;
using MI.PIMS.BO.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace MI.PIMS.BL.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        //Tables
        public DbSet<DPOC_Inventories_T> dpoc_inventories_t { get; set; }
        public DbSet<EPAL_Procedures_T> epal_procedures_t { get; set; }

        //Views
        public DbSet<DPOC_Inventories_V> dpoc_inventories_v { get; set; }
        public DbSet<PAYC_Procs_Curr_Ver_Eff_Dt_V> payc_procs_curr_ver_eff_dt_v { get; set; }
        public DbSet<DPOC_HIERARCHY_CODES_XWALK_V_Dto> dpoc_hierarchy_codes_xwalk_v { get; set; }
        public DbSet<DPOC_Inv_Gdln_Rules_Act_Ret_V> dpoc_inv_gdln_rules_act_ret_v { get; set; }
        public DbSet<DPOC_REF_ALL_DIAG_CD_LISTS_V> ref_all_diag_cd_lists_v { get; set; }
        public DbSet<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto> dpoc_inv_gdln_rules_pnd_v { get; set; }
        public DbSet<Ref_Procedures_V_Dto> ref_procedures_v { get; set; } //User Story 137441 MFQ 11/13/2025
        public DbSet<Ref_Iq_Manual_Guidelines_T> ref_iq_manual_guidelines_t { get; set; } // User Story 137441 MFQ 11/17/2025
        public DbSet<DPOC_Inv_Gdln_Rules_T> dpoc_inv_gdln_rules_t { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the view
            modelBuilder.Entity<DPOC_Inventories_V>()
                .HasNoKey()
                .ToView("dpoc_inventories_v", "pims_user");

            // Map the view
            modelBuilder.Entity<PAYC_Procs_Curr_Ver_Eff_Dt_V>()
                .HasNoKey()
                .ToView("payc_procs_curr_ver_eff_dt_v", "pims_user");

            // Map the view
            modelBuilder.Entity<DPOC_HIERARCHY_CODES_XWALK_V_Dto>()
                .HasNoKey()
                .ToView("dpoc_hierarchy_codes_xwalk_v", "pims_user");

            // Map the view
            modelBuilder.Entity<DPOC_Inv_Gdln_Rules_Act_Ret_V>()
                .HasNoKey()
                .ToView("dpoc_inv_gdln_rules_act_ret_v", "pims_user");

            // Map the view // User Story 137441 MFQ 11/13/2025
            modelBuilder.Entity<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>()
                .HasNoKey()
                .ToView("dpoc_inv_gdln_rules_pnd_v", "pims_user");

            // Map the view
            modelBuilder.Entity<DPOC_REF_ALL_DIAG_CD_LISTS_V>()
                .HasNoKey()
                .ToView("ref_all_diag_cd_lists_v", "pims_user");

            //Tables
            modelBuilder.Entity<DPOC_Inventories_T>(entity =>
            {
                entity.ToTable("dpoc_inventories_t", "pims_user");

                // Composite primary key
                entity.HasKey(e => new { e.DPOC_Hierarchy_Key, e.DPOC_Ver_Eff_Dt, e.DPOC_Package });
            });

            modelBuilder.Entity<EPAL_Procedures_T>(entity =>
            {
                entity.ToTable("epal_procedures_t", "pims_user");

                // Composite primary key
                entity.HasKey(e => new { e.EPAL_Hierarchy_Key, e.EPAL_Ver_Eff_Dt });
            });

            // Map the view //User Story 137441 MFQ 11/13/2025
            modelBuilder.Entity<Ref_Procedures_V_Dto>()
                .HasNoKey()
                .ToView("ref_procedures_v", "pims_user");


            modelBuilder.Entity<Ref_Iq_Manual_Guidelines_T>(entity =>
            {
                entity.ToTable("ref_iq_manual_guidelines_t", "pims_user");

                entity.HasKey(e => new { e.iq_gdln_id, e.iq_gdln_proc_cd });
            });

            modelBuilder.Entity<DPOC_Inv_Gdln_Rules_T>(entity =>
            {
                entity.ToTable("dpoc_inv_gdln_rules_t", "pims_user");

                entity.HasKey(e => new { e.dpoc_hierarchy_key, e.dpoc_ver_eff_dt, e.dpoc_package, e.dpoc_release, e.iq_gdln_id, e.iq_gdln_rules_sys_seq, e.dpoc_ver_num });
            });
        }

    }
}
