using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO;
using MI.PIMS.BO.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class CategoryUpdateService : ICategoryUpdateService
    {
        private readonly CategoryUpdateRepository _repo;        
        public CategoryUpdateService(CategoryUpdateRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<REF_ALT_CAT_SPLCTY_CMBNTNS_V_Dto>> GetSpecialtyCombination(string p_type, string p_parent_value)
        {
            var data = await _repo.GetSpecialtyCombination(p_type, p_parent_value);
            //if(p_type != "SPECIALTY")
            //    data = data.Select(sp => { sp.CATEGORY = sp.RET_VAL; return sp; } ).ToList();
            return data;
        }
        public async Task<IEnumerable<CategoryUpdate_Dto>> GetCategoryUpdates(CategoryUpdateParam categoryUpdateParam)
        {
            var data = await _repo.GetCategoryUpdates(categoryUpdateParam);
            return data;
        }

        public async Task<int> GetRecordsImpacted(CategoryUpdateParam categoryUpdateParam)
        {
            var data = await _repo.GetRecordsImpacted(categoryUpdateParam);
            return data;
        }

        public async Task<int> GetDuplicateAlternateCatetoriesCount(CategoryUpdateParam categoryUpdateParam)
        {
            var data = await _repo.GetDuplicateAlternateCatetoriesCount(categoryUpdateParam);
            return data;
        }

        public async Task<InsertRetDto> InsertByProcCode(IEnumerable<CategoryInsertByProcCDParam> categoryInsertParams)
        {
            List<CategoryInsertByProcCDParam> insertedCategories = new List<CategoryInsertByProcCDParam>();
            List<CategoryInsertByProcCDParam> errorCategories = new List<CategoryInsertByProcCDParam>();
            List<CategoryInsertByProcCDParam> dupCategories = new List<CategoryInsertByProcCDParam>();
            InsertRetDto insertRetDto = new InsertRetDto();
            

            foreach (var category in categoryInsertParams)
            {
                try
                {
                    var dup = await GetDuplicateAlternateCatetoriesCount(new CategoryUpdateParam
                    {
                        P_PROC_CD = category.P_PROC_CD,                              
                        P_ALTERNATE_CATEGORY = category.P_ALTERNATE_CATEGORY,
                        P_ALTERNATE_SUB_CATEGORY = category.P_ALTERNATE_SUB_CATEGORY
                    });

                    if(dup > 0)
                    {
                        dupCategories.Add(category);
                        continue;
                    }
                    
                    var data = await _repo.InsertByProcCode(category);
                    if(data.O_ALREADY_EXIST_COUNT == 0)
                    {
                        errorCategories.Add(category);
                    }
                    else
                    {
                        insertedCategories.Add(category);
                    }                    
                }
                catch(Exception ex)
                {
                    errorCategories.Add(category);
                }
            }

            insertRetDto.StatusID = 0;
            insertRetDto.Message = Common.Helper.AddMessage;
            insertRetDto.StatusType = RetValStatus.Success.ToString();
            insertRetDto.InsertedRecords = insertedCategories;     
            insertRetDto.ErrorRecords = errorCategories;
            insertRetDto.DupRecords = dupCategories;

            return insertRetDto;
        }

        public async Task<UpdateDto> Insert(CategoryUpdateParam categoryUpdateParam)
        {
            UpdateDto updateDto  = new UpdateDto();

            var data = await _repo.Insert(categoryUpdateParam);

            if(updateDto.StatusID == -1)
            {
                updateDto.StatusID = data;
                updateDto.Message = "Error";
                updateDto.StatusType = RetValStatus.Error.ToString();
            }
            else if (updateDto.StatusID == 2)
            {
                updateDto.StatusID = data;
                updateDto.Message = Common.Helper.AlreadyExistMessage;
                updateDto.StatusType = RetValStatus.Warning.ToString();
            }
            else
            {
                updateDto.StatusID = data;
                updateDto.Message = Common.Helper.AddMessage;
                updateDto.StatusType = RetValStatus.Success.ToString();
            }
            
            
            return updateDto;
        }

        public async Task<UpdateDto> Update(CategoryUpdate_Edit_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto();

            int retVal;

            try
            {
                retVal = await _repo.Update(obj);

                updateTO.StatusID = retVal;
                updateTO.Message = Common.Helper.UpdateMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
                updateTO.ReturnObject = obj;
            }
            catch (Exception ex)
            {
                updateTO.Message = "Error occured while saving the record";
                updateTO.StatusType = RetValStatus.Error.ToString();
                updateTO.ReturnObject = ex;
            }

            return updateTO;
        }

        public async Task<UpdateDto> Delete(CategoryUpdateParam obj)
        {
            UpdateDto updateTO = new UpdateDto();

            int retVal;

            try
            {
                retVal = await _repo.Delete(obj);

                updateTO.StatusID = retVal;
                updateTO.Message = Common.Helper.RecordDeleteMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
                updateTO.ReturnObject = obj;
            }
            catch (Exception ex)
            {
                updateTO.Message = "Error occured while saving the record";
                updateTO.StatusType = RetValStatus.Error.ToString();
                updateTO.ReturnObject = ex;
            }

            return updateTO;
        }

        public async Task<UpdateDto> UpdateAltCat(CategoryUpdate_Edit_Dto obj)
        {
            UpdateDto updateTO = new UpdateDto();

            int retVal;

            try
            {
                retVal = await _repo.UpdateByAltCat(obj);

                updateTO.StatusID = retVal;
                updateTO.Message = Common.Helper.UpdateMessage;
                updateTO.StatusType = RetValStatus.Success.ToString();
                updateTO.ReturnObject = obj;
            }
            catch (Exception ex)
            {
                updateTO.Message = "Error occured while saving the record";
                updateTO.StatusType = RetValStatus.Error.ToString();
                updateTO.ReturnObject = ex;
            }

            return updateTO;
        }

        public async Task<UpdateDto> UpdateSearchResults(IEnumerable<CategoryUpdate_Edit_Dto> categoryUpdate_Edit_Dtos)
        {
            UpdateDto updateTO = new UpdateDto();
            List<CategoryUpdate_Edit_Dto> updatedRecords = new List<CategoryUpdate_Edit_Dto>();
            foreach (CategoryUpdate_Edit_Dto categoryUpdate_Edit in categoryUpdate_Edit_Dtos)
            {

                var recImpacted = await GetRecordsImpacted(new CategoryUpdateParam
                {
                    P_PROC_CD = categoryUpdate_Edit.P_PROC_CD,
                    P_DRUG_NM = categoryUpdate_Edit.P_DRUG_NM,
                    P_ALTERNATE_CATEGORY = categoryUpdate_Edit.P_ALTERNATE_CATEGORY_NEW,
                    P_ALTERNATE_SUB_CATEGORY = categoryUpdate_Edit.P_ALTERNATE_SUB_CATEGORY_NEW
                });

                if (recImpacted > 0)
                {
                    updateTO.StatusID = -999;
                    updateTO.StatusType = RetValStatus.Warning.ToString();
                    updateTO.ReturnObject = recImpacted;
                    updateTO.Message = recImpacted.ToString() + " records associated with one of the Alternate category!";
                    break;
                }
                else
                {
                    var u = await Update(categoryUpdate_Edit);
                    updateTO.StatusID = u.StatusID;
                    updateTO.StatusType = RetValStatus.Success.ToString();
                    updateTO.Message = recImpacted.ToString() + " records associated with one of the Alternate category!";
                }
            }

            return updateTO;
        }

        public async Task<IEnumerable<Admin_Catagory_By_Type_Dto>> GetAdminCategoriesByType(Admin_Catagory_By_Type_Param_Dto admin_Catagory_By_Type_Param_Dto)
        {
            var data = await _repo.GetAdminCategoriesByType(admin_Catagory_By_Type_Param_Dto);
            if(admin_Catagory_By_Type_Param_Dto.P_DRUG_NM != null)
                data = data.Where(sp => sp.DRUG_NM.ToStringNullSafe().Split(',').Contains(admin_Catagory_By_Type_Param_Dto.P_DRUG_NM)).ToList();
            return data;
        }
    }
}
