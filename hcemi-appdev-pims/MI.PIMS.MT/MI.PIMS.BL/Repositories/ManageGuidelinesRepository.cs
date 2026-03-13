using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class ManageGuidelinesRepository
    {
        private readonly AppDbContext _context;

        public ManageGuidelinesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>> GetGuidelinesByParams(
    string proc_cd, string iq_reference, string iq_gdln_id, string iq_gdln_version)
        {
            if (string.IsNullOrWhiteSpace(proc_cd) &&
                string.IsNullOrWhiteSpace(iq_reference) &&
                string.IsNullOrWhiteSpace(iq_gdln_id) &&
                string.IsNullOrWhiteSpace(iq_gdln_version))
            {
                return Enumerable.Empty<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>();
            }

            // Start with base query
            IQueryable<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto> query = null;

            if (!string.IsNullOrWhiteSpace(proc_cd))
            {
                var procCodes = proc_cd.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(x => x.Trim())
                                       .ToList();

                var validCodes = await _context.ref_procedures_v
                    .Where(r => procCodes.Contains(r.proc_cd) &&
                                r.proc_cd_exp_dt == null &&
                                (r.proc_cd_type == "CPT" || r.proc_cd_type == "HCPCS"))
                    .Select(r => r.proc_cd)
                    .ToListAsync();

                if (!validCodes.Any())
                    return Enumerable.Empty<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>();

                query = _context.dpoc_inv_gdln_rules_pnd_v
                    .Where(x => validCodes.Contains(x.proc_cd))
                    .Select(x => new DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto
                    {
                        proc_cd = x.proc_cd,
                        iq_reference = x.iq_reference,
                        iq_gdln_id = x.iq_gdln_id,
                        iq_gdln_version = x.iq_gdln_version
                    });
            }
            else
            {
                query = _context.dpoc_inv_gdln_rules_pnd_v
                    .Select(x => new DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto
                    {
                        proc_cd = x.proc_cd,
                        iq_reference = x.iq_reference,
                        iq_gdln_id = x.iq_gdln_id,
                        iq_gdln_version = x.iq_gdln_version
                    });
            }

            // Apply additional filters
            if (!string.IsNullOrWhiteSpace(iq_reference))
                query = query.Where(x => x.iq_reference.Contains(iq_reference));

            if (!string.IsNullOrWhiteSpace(iq_gdln_id))
                query = query.Where(x => x.iq_gdln_id.Contains(iq_gdln_id));

            if (!string.IsNullOrWhiteSpace(iq_gdln_version))
                query = query.Where(x => x.iq_gdln_version.Contains(iq_gdln_version));

            // Apply Distinct on required fields
            var result = await query
                .GroupBy(x => new { x.proc_cd, x.iq_reference, x.iq_gdln_id, x.iq_gdln_version })
                .Select(g => g.First()) // Take one per group
                .ToListAsync();

            return result;
        }


        public async Task<List<Ref_Procedures_V_Dto>> GetActiveProcedureCodes(string proc_cds)
        {
            var data = await _context.ref_procedures_v
                .Where(p => p.proc_cd_exp_dt == null &&
                            p.proc_cd.Contains(proc_cds) && 
                            (p.proc_cd_type == "CPT" || p.proc_cd_type == "HCPCS"))
                .Select(p => new Ref_Procedures_V_Dto
                {
                    proc_cd = p.proc_cd,
                    proc_cd_desc = p.proc_cd_desc
                })
                .OrderBy(p => p.proc_cd)
                .ToListAsync();

            return data;
        }
    }


    public interface IManualGuidelineRepository
    {
        Task<bool> ExistsAsync(string iq_gdln_id, string iq_gdln_proc_cd);
        Task AddAsync(Ref_Iq_Manual_Guidelines_T guideline);
        Task<bool> ExistsInRefProceduresAsync(string procCd);
    }

    public class ManualGuidelineRepository: IManualGuidelineRepository
    {
        private readonly AppDbContext _context;

        public ManualGuidelineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string iq_gdln_id, string iq_gdln_proc_cd)
        {
            return await _context.ref_iq_manual_guidelines_t
                .AnyAsync(x => x.iq_gdln_id == iq_gdln_id && x.iq_gdln_proc_cd == iq_gdln_proc_cd);
        }

        public async Task AddAsync(Ref_Iq_Manual_Guidelines_T guideline)
        {
            _context.ref_iq_manual_guidelines_t.Add(guideline);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsInRefProceduresAsync(string procCd)
        {
            return await _context.ref_procedures_v.AnyAsync(r => r.proc_cd == procCd && (r.proc_cd_type == "CPT" || r.proc_cd_type == "HCPCS"));
        }

    }

}
