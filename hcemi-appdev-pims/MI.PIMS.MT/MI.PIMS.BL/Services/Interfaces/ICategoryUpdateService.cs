using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface ICategoryUpdateService
    {
        Task<IEnumerable<REF_ALT_CAT_SPLCTY_CMBNTNS_V_Dto>> GetSpecialtyCombination(string p_type, string p_parent_value);
        Task<IEnumerable<CategoryUpdate_Dto>> GetCategoryUpdates(CategoryUpdateParam categoryUpdateParam);
        Task<UpdateDto> Update(CategoryUpdate_Edit_Dto obj);
        Task<int> GetRecordsImpacted(CategoryUpdateParam categoryUpdateParam);
        Task<UpdateDto> UpdateSearchResults(IEnumerable<CategoryUpdate_Edit_Dto> categoryUpdate_Edit_Dtos);
        Task<int> GetDuplicateAlternateCatetoriesCount(CategoryUpdateParam categoryUpdateParam);
        Task<InsertRetDto> InsertByProcCode(IEnumerable<CategoryInsertByProcCDParam> categoryInsertParam);
        Task<UpdateDto> UpdateAltCat(CategoryUpdate_Edit_Dto obj);
        Task<UpdateDto> Insert(CategoryUpdateParam categoryUpdateParam);
        Task<UpdateDto> Delete(CategoryUpdateParam obj);
        Task<IEnumerable<Admin_Catagory_By_Type_Dto>> GetAdminCategoriesByType(Admin_Catagory_By_Type_Param_Dto admin_Catagory_By_Type_Param_Dto);
    }
}
