using Kendo.Mvc.UI;
using MI.PIMS.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using MI.PIMS.UI.Common;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Services.MIAuthenticate;
using MI.PIMS.BL.Services;
using Newtonsoft.Json;
using MI.PIMS.BO.Entities;
using MI.PIMS.UI.Models;
using System.Linq;
using MI.PIMS.UI.Areas.EPAL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace MI.PIMS.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        readonly IUserInfoService _userInfoService;
        readonly Helper _helper;
        readonly ICacheRepository _cacheRepository;
        readonly IReportsService _reportsService;
        private readonly IUserAccessHistService _userAccessHistService;
        private readonly ICategoryUpdateService _categoryUpdateService;
        private readonly IEPALProceduresService _ePALProceduresService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        //private readonly IGraphHandlerService _graphHandlerService;
        public HomeController(IUserInfoService userInfoService, 
                            Helper helper, 
                            ICacheRepository cacheRepository,
                            IReportsService reportsRepository,
                            IUserAccessHistService userAccessHistService,
                            ICategoryUpdateService categoryUpdateService,
                            IEPALProceduresService ePALProceduresService,
                            IActiveDirectoryService activeDirectoryService
                            //IGraphHandlerService graphHandlerService
                        )
        {
            _userInfoService = userInfoService;
            _helper = helper;
            _cacheRepository = cacheRepository;
            _reportsService = reportsRepository;
            _userAccessHistService = userAccessHistService;
            _categoryUpdateService = categoryUpdateService;
            _ePALProceduresService = ePALProceduresService;
            _activeDirectoryService = activeDirectoryService;
            //_graphHandlerService = graphHandlerService;
        }

        #region Views

        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.DPOCAdmin, 
                        Roles.EPALAdmin, 
                        Roles.PayCodeAdmin,
                        Roles.CMPAdmin
                        )]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        //[AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> JurisdictionUpdate()
        {
            return await Task.FromResult(View());
        }

        //[AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> ValidValuesChanges()
        {
            return await Task.FromResult(View());
        }

        //[AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> CategoryUpdate() 
        {
            List<PageApiInfo> _pageApiInfos = new List<PageApiInfo>();
            _pageApiInfos.AddRange(new List<PageApiInfo>()
                {
                    new PageApiInfo() { Url = Url.Action("GetRecordsImpacted", "Home", new { Area = "Admin"  }) , Type = "admin-get-records-impacted-url" },
                    new PageApiInfo() { Url = Url.Action("UpdateCategory", "Home", new { Area = "Admin"  }) , Type = "admin-update-cat-url" },
                    new PageApiInfo() { Url = Url.Action("UpdateSearchCategoryResults", "Home", new { Area = "Admin"  }) , Type = "admin-update-search-category-results-url" },
                    new PageApiInfo() { Url = Url.Action("GetDuplicateAlternateCatetoriesCount", "Home", new { Area = "Admin"  }) , Type = "admin-get-dup-alt-cat-count-url" },
                    new PageApiInfo() { Url = Url.Action("InsertByProcCd", "Home", new { Area = "Admin"  }) , Type = "admin-insert-cat-by-proccd-url" },
                    new PageApiInfo() { Url = Url.Action("UpdateAltCat", "Home", new { Area = "Admin"  }) , Type = "admin-update-alt-cat-url" },
                    new PageApiInfo() { Url = Url.Action("GetSpecialtyCombination", "Home", new { Area = "Admin"  }) , Type = "admin-get-specialty-combination-url" },
                    new PageApiInfo() { Url = Url.Action("Insert", "Home", new { Area = "Admin"  }) , Type = "admin-insert-alt-cat-url" },
                    new PageApiInfo() { Url = Url.Action("Delete", "Home", new { Area = "Admin"  }) , Type = "admin-delete-alt-cat-url" }
                });

            ViewBag.CategoryUpdateUrls = JsonConvert.SerializeObject(_pageApiInfos);

            return await Task.FromResult(View());
        }

        //[AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> DiagnosisChanges()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> ManageReports()
        {
            return await Task.FromResult(View());
        }

        /// <summary>
        /// Add/Update IQ Guidelines
        /// Note: User Story 137441 - MFQ 11-5-2025
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(Roles.SuperAdmin)]
        public async Task<IActionResult> ManageGuidelines()
        {
            return await Task.FromResult(View());
        }

        #endregion

        #region Methods
        [HttpGet]
        public async Task<ActionResult> GetSpecialtyCombination(string p_type, string p_parent_value)
        {
            var retVal = await _categoryUpdateService.GetSpecialtyCombination(p_type, p_parent_value);
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserInfo(UserInfo_AddDto obj)
        {
            obj.P_LST_UPDT_BY = _helper.MS_ID;
            var data = await _userInfoService.Add(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/AddUserInfo", JsonConvert.SerializeObject(obj), 3);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser (DeleteUserInfoTParam_Dto obj)
        {
            var data = await _userInfoService.Delete(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/DeleteUser", JsonConvert.SerializeObject(obj), 3);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(AppRoleUserAssign_Dto obj)
        {
            obj.p_LST_UPDT_BY = _helper.MS_ID;

            var  data = await _userInfoService.AddRoleUserAssign(obj);
            
            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/UpdateRole", JsonConvert.SerializeObject(obj), 3);

            /*  StatusID == -1 means record has been saved, then update the cache of the user*/
            if (data.StatusID == -1)
            {
                var userInfoDto = await _userInfoService.Get(_helper.MS_ID);
                _cacheRepository.Set(obj.p_MS_ID, userInfoDto);

                _cacheRepository.Remove("MenuAccessDto", obj.p_MS_ID);
                _cacheRepository.RemoveGlobal(obj.p_MS_ID);                
            }

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRptUrl(ReportsDto obj)
        {
            obj.LST_UPDT_BY = _helper.MS_ID;
            var data = await _reportsService.UpdateReportLinks(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/UpdateReports", JsonConvert.SerializeObject(obj), 3);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePIMSUserStatus(TogglePIMSUserStatusParam_Dto obj)
        {
            obj.p_LST_UPDT_BY = _helper.MS_ID;
            var data = await _userInfoService.TogglePIMSUserStatus(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/TogglePIMSUserStatus", JsonConvert.SerializeObject(obj), 3);

            /*  record has been saved, then update the cache of the user*/

            var userInfoDto = await _userInfoService.Get(_helper.MS_ID);
#if !DEBUG
            _cacheRepository.Set(obj.p_MS_ID, userInfoDto);
            _cacheRepository.Remove("MenuAccessDto", obj.p_MS_ID.ToLower());
            _cacheRepository.RemoveGlobal(obj.p_MS_ID.ToLower());
#endif
            return Json(data, new JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetRecordsImpacted(CategoryUpdateParam categoryUpdateParam)
        {
            var recordsImacted = await _categoryUpdateService.GetRecordsImpacted(categoryUpdateParam);
            return Json(recordsImacted);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryUpdate_Edit_Dto obj)
        {
            obj.P_LST_UPDT_BY = _helper.MS_ID;
            var data = await _categoryUpdateService.Update(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/UpdateCategory", JsonConvert.SerializeObject(obj), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAltCat(CategoryUpdate_Edit_Dto obj)
        {
            obj.P_LST_UPDT_BY = _helper.MS_ID;
            var data = await _categoryUpdateService.UpdateAltCat(obj);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/UpdateAltCat", JsonConvert.SerializeObject(obj), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSearchCategoryResults(IEnumerable<CategoryUpdate_Edit_Dto> categoryUpdate_Edit_Dtos)
        {            
            var data = await _categoryUpdateService.UpdateSearchResults(categoryUpdate_Edit_Dtos);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/UpdateSearchCategoryResults", JsonConvert.SerializeObject(categoryUpdate_Edit_Dtos), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDuplicateAlternateCatetoriesCount(CategoryUpdateParam categoryUpdateParam)
        {
            var data = await _categoryUpdateService.GetDuplicateAlternateCatetoriesCount(categoryUpdateParam);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/GetDuplicateAlternateCatetoriesCount", JsonConvert.SerializeObject(categoryUpdateParam), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertByProcCd([FromBody] IEnumerable<CategoryInsertByProcCDParam> categoryInsertByProcCDParams)
        {
            var data = await _categoryUpdateService.InsertByProcCode(categoryInsertByProcCDParams);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/Insert", JsonConvert.SerializeObject(categoryInsertByProcCDParams), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(CategoryUpdateParam categoryUpdateParam)
        {
            categoryUpdateParam.P_LST_UPDT_BY = _helper.MS_ID;
            var data = await _categoryUpdateService.Insert(categoryUpdateParam);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/Insert", JsonConvert.SerializeObject(categoryUpdateParam), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryUpdateParam categoryUpdateParam)
        {
            categoryUpdateParam.P_LST_UPDT_BY = _helper.MS_ID;
            var data = await _categoryUpdateService.Delete(categoryUpdateParam);

            // Log user activity
            await _userAccessHistService.Add("Admin", "Admin/Home/CategoryUpdate/Delete", JsonConvert.SerializeObject(categoryUpdateParam), 10);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpGet]
        public async Task<JsonResult> GetAdminCategoriesByType(string text, string P_CATEGORY_TYPE, string P_PARENT_CATEGORY, string P_DRUG_NM)
        {
            var retVal = await _categoryUpdateService.GetAdminCategoriesByType(new Admin_Catagory_By_Type_Param_Dto { P_TEXT = text, P_CATEGORY_TYPE = P_CATEGORY_TYPE, P_PARENT_CATEGORY = P_PARENT_CATEGORY, P_DRUG_NM = P_DRUG_NM });
            return Json(retVal);
        }
#endregion

        #region KendoGrids
        public async Task<JsonResult> GetUsers([DataSourceRequest] DataSourceRequest request, UserSearchParam param)
        {
            var data = await _userInfoService.GetUsers(param.MS_ID, param.Fname, param.Lname, param.App_Role_ID, param.Active, param.PIMS_user);
            return Json(data.ToDataSourceResult(request));
        }



        public async Task<ActionResult> GetActiveDirectoryUser([DataSourceRequest] DataSourceRequest request, UserSearchParam param)
        {
            var data = await _activeDirectoryService.GetActiveDirectoryUser(param.MS_ID, param.Fname, param.Lname);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetActiveDirectoryUser2([DataSourceRequest] DataSourceRequest request, UserSearchParam param)
        {
            var data = await _userInfoService.GetETL_ADExportUsersByParam(param.MS_ID, param.Fname, param.Lname);
            return Json(data.ToDataSourceResult(request));
        }


        public async Task<ActionResult> GetRoleUserAssign([DataSourceRequest] DataSourceRequest request, UserSearchParam param)
        {
            var data = await _userInfoService.GetRoleUserAssignByMSID(param.MS_ID);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetReportLinks([DataSourceRequest] DataSourceRequest request)
        {
            var data = await _reportsService.GetReportLinks();
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetCategoryUpdates([DataSourceRequest] DataSourceRequest request, CategoryUpdateParam categoryUpdateParam)
        {
            var data = await _categoryUpdateService.GetCategoryUpdates(categoryUpdateParam);
            return Json(data.ToDataSourceResult(request));
        }
        #endregion
    }
}
