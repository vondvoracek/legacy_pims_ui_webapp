using MI.PIMS.BL.Common;
using MI.PIMS.BL.Data;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class EPALAltrnt_Svc_CatRepository: DapperPostgresBaseRepository
    {
        private ILoggerService _logger;
        private readonly AppDbContext _context;
        public EPALAltrnt_Svc_CatRepository(Helper helper, ILoggerService logger, AppDbContext context) : base(helper)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateCat(string p_text, string p_column_name, string p_proc_cds)
        {
            var query = _context.dpoc_inventories_v.AsQueryable();

            if (!string.IsNullOrEmpty(p_proc_cds))
            {
                var procCodes = p_proc_cds.Split(',').Select(code => code.Trim()).ToList();
                query = query.Where(p => procCodes.Contains(p.Proc_Cd));
            }

            if (string.IsNullOrEmpty(p_column_name))
                return Enumerable.Empty<Altrnt_Cat_Dto>();

            var loweredText = p_text?.ToLower() ?? "";

            switch (p_column_name)
            {
                case "altrnt_svc_cat":
                    return await query
                        .Where(p => p.Epal_Altrnt_Svc_Cat != null && p.Epal_Altrnt_Svc_Cat.ToLower().Contains(loweredText))
                        .Select(p => new Altrnt_Cat_Dto { altrnt_svc_cat = p.Epal_Altrnt_Svc_Cat })
                        .Distinct()
                        .OrderBy(p => p.altrnt_svc_cat)
                        .ToListAsync();

                case "altrnt_svc_subcat":
                    return await query
                        .Where(p => p.Epal_Altrnt_Svc_Subcat != null && p.Epal_Altrnt_Svc_Subcat.ToLower().Contains(loweredText))
                        .Select(p => new Altrnt_Cat_Dto { altrnt_svc_subcat = p.Epal_Altrnt_Svc_Subcat })
                        .Distinct()
                        .OrderBy(p => p.altrnt_svc_subcat)
                        .ToListAsync();

                // Add more cases as needed

                default:
                    throw new ArgumentException($"Unsupported column name: {p_column_name}");
            }
        }

        public async Task<IEnumerable<Altrnt_Cat_Dto>> GetAlternateSubCat(
            string p_text,
            string p_column_name,
            string p_proc_cds,
            string epal_altrnt_svc_cat)
        {
            var query = _context.dpoc_inventories_v.AsQueryable();

            if (!string.IsNullOrEmpty(p_proc_cds))
            {
                var procCodes = p_proc_cds.Split(',').Select(code => code.Trim()).ToList();
                query = query.Where(p => procCodes.Contains(p.Proc_Cd));
            }

            if (!string.IsNullOrEmpty(epal_altrnt_svc_cat))
            {
                query = query.Where(p => p.Epal_Altrnt_Svc_Cat == epal_altrnt_svc_cat);
            }

            if (!string.IsNullOrEmpty(p_text) && !string.IsNullOrEmpty(p_column_name))
            {
                var loweredText = p_text.ToLower();
                query = query.Where(p => p.Epal_Altrnt_Svc_Subcat.ToLower().Contains(p_text));
            }

            var result = await query
                .Where(p => p.Epal_Altrnt_Svc_Subcat != null)
                .Select(p => new Altrnt_Cat_Dto
                {
                    altrnt_svc_cat = p.Epal_Altrnt_Svc_Cat,
                    altrnt_svc_subcat = p.Epal_Altrnt_Svc_Subcat
                })
                .Distinct()
                .OrderBy(p => p.altrnt_svc_subcat)
                .ToListAsync();

            return result;
        }

    }
}
