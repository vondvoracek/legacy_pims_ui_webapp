using MI.PIMS.BL.Common;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class UserAccessHistService : IUserAccessHistService
    {
        private readonly IUserAccessHistRepository _userAccessHistRepository;
        public UserAccessHistService(IUserAccessHistRepository userAccessHistRepository)
        {
            _userAccessHistRepository = userAccessHistRepository;
        }
        public async Task<UpdateDto> Add(UserAccess_Hist_Dto obj)
        {
            var retVal = await _userAccessHistRepository.Add(obj);
            UpdateDto updateDto = new UpdateDto
            {
                StatusID = retVal,
                StatusType = RetValStatus.Success.ToString()
            };

            return updateDto;
        }

        public async Task<UpdateDto> Add(string module_name, string useraction, string userselection, int page_id, string lst_updt_by = "")
        {
            UserAccess_Hist_Dto userAccess_Hist_Dto = new UserAccess_Hist_Dto
            {
                LST_UPDT_BY = lst_updt_by,
                MODULE_NAME = module_name,
                USERACTION = useraction,
                USERSELECTION = userselection,
                PAGE_ID = page_id
            };
            UpdateDto task;
            try
            {
                task = await Add(userAccess_Hist_Dto);
            }
            catch (Exception ex)
            {
                task = new UpdateDto();
            }
            return task;
        }
    }
}
