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
    public class DPOCHierarchyCodesXWalkRepository: DapperPostgresBaseRepository
    {
        private readonly ILoggerService _logger;
        private readonly Helper _helper;
        private readonly AppDbContext _context;
        public DPOCHierarchyCodesXWalkRepository(Helper helper, ILoggerService logger, AppDbContext context) : base(helper)
        {
            _helper = helper;   
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<DPOC_HIERARCHY_CODES_XWALK_V_Dto>> GetAll()
        {
            var data = await _context.dpoc_hierarchy_codes_xwalk_v.ToListAsync();            
            return data;
        }
    }
}
