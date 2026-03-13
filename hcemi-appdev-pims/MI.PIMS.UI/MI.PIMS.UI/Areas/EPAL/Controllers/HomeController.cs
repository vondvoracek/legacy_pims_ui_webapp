using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BL.Common.Telemetry;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Areas.EPAL.Repositories;
using MI.PIMS.UI.Common;
using MI.PIMS.UI.Models;
using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services.Logging;
using MI.PIMS.UI.Services.PimsStaticDataRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MI.PIMS.UI.Areas.ProcedureCode.Controllers
{

    [Area("EPAL")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TelemetryLogger<HomeController> _telemetry;
        private readonly IEPALProceduresService _service;
        private readonly IRefDiagnosesService _refDiagnosesService;
        private readonly IRefRevenuesService _refRevenuesService;
        private readonly IRefModifiersService _refModifiersService;
        private readonly IUserAccessHistService _userAccessHistService;
        private readonly IRefProceduresService _refProceduresService;
        private readonly IEPALProceduresService _EPALProceduresService;
        private readonly ICnSIFPLogic _cnSIFPLogic;
        private readonly PimsValidValuesManager _pimsValidValuesManager;

        private readonly IPIMSValidValuesService _pimsValidValuesService;
        private readonly IXrefStatusService _xrefStatusService;
        readonly Helper _helper;
        readonly ILoggerService _loggerService;

        public HomeController(IEPALProceduresService service,
                                IRefDiagnosesService refDiagnosesService,
                                Helper helper,
                                IRefRevenuesService refRevenuesService,
                                IRefModifiersService refModifiersService,
                                IUserAccessHistService userAccessHistService,
                                IRefProceduresService refProceduresService,
                                IEPALProceduresService ePALProceduresService,
                                ICnSIFPLogic cnSIFPLogic,
                                IPIMSValidValuesService pimsValidValuesService,
                                IXrefStatusService xrefStatusService,
                                PimsValidValuesManager pimsValidValuesManager,
                                ILoggerService loggerService,
                                TelemetryLogger<HomeController> telemetry)
        {
            _service = service;
            _refDiagnosesService = refDiagnosesService;
            _helper = helper;
            _refRevenuesService = refRevenuesService;
            _refModifiersService = refModifiersService;
            _userAccessHistService = userAccessHistService;
            _refProceduresService = refProceduresService;
            _EPALProceduresService = ePALProceduresService;
            _cnSIFPLogic = cnSIFPLogic;
            _pimsValidValuesService = pimsValidValuesService;
            _xrefStatusService = xrefStatusService;
            _pimsValidValuesManager = pimsValidValuesManager;   
            _loggerService = loggerService;
            _telemetry = telemetry;
        }

        #region Views
        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.EPALReadOnly, 
                        Roles.EPALReadWrite, 
                        Roles.EPALAdmin, 
                        Roles.UMRReadWrite, 
                        Roles.UMRAdmin, 
                        Roles.UMRReadOnly,
                        Roles.CMPReadWrite,
                        Roles.CMPAdmin,
                        Roles.CMPReadOnly,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.DonorRecordsReadOnly,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin,
                        Roles.ICHRAReadOnly
                        )]
        public async Task<IActionResult> Index()
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL Search", "EPAL/Home/Index", "EPAL/Home/Index", 1);

            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.EPALReadWrite)]
        public async Task<IActionResult> BulkInsert()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, Roles.EPALReadWrite)]
        public async Task<IActionResult> BulkUpdate()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> UMRForbidden()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> CMPForbidden()
        {
            return await Task.FromResult(View());
        }
        public async Task<IActionResult> EPALForbidden()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> NonUMRForbidden()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> NonCMPForbidden()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> DonorRecordsForbidden()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> ICHRAForbidden()
        {
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.EPALReadWrite, 
                        Roles.EPALAdmin, 
                        Roles.UMRReadWrite, 
                        Roles.UMRAdmin,
                        Roles.CMPReadWrite,
                        Roles.CMPAdmin,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin
                        )]
        public async Task<IActionResult> AddNew()
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL Add New", "EPAL/Home/AddNew", "EPAL/Home/AddNew", 4);
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.EPALReadOnly, 
                        Roles.EPALReadWrite, 
                        Roles.UMRReadOnly
                        )]
        public async Task<IActionResult> Detail()
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL", "EPAL/Home/Detail", "EPAL/Home/Detail", 7);
            return await Task.FromResult(View());
        }

        [AuthorizeRoles(Roles.SuperAdmin,
                        Roles.EPALReadWrite,
                        Roles.EPALAdmin,
                        Roles.UMRAdmin,
                        Roles.UMRReadWrite,
                        Roles.CMPAdmin,
                        Roles.CMPReadWrite,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin
                        )]
        [Route("EPAL/Home/EditDetail/{pims_id}")]
        public async Task<IActionResult> EditDetail(string pims_id)
        {
            var aiEventName = Helper.GetAppInsightCustomEventName(RouteData, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            _telemetry.TrackEvent(aiEventName, new LogInfo
            {
                Operation = $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}",
                UserId = _helper.MS_ID,
                AdditionalInfo = pims_id
            });

            // Log user activity
            //await _userAccessHistRepository.Add("EPAL", "EPAL/Home/EditDetail", "EPAL/Home/EditDetail?pims_id=" + pims_id, 7);

            pims_id = HttpUtility.HtmlDecode(pims_id);

            string s = pims_id;
            string epal_ver_eff_dt;
            string pims_id_eff_dt;
            //string is_current;
            string[] items = s.Split(',');
            ViewBag.EPALPageView = EPALPageView.EditDetail.ToString();

            EPAL_Procedures_V_Dto data;

            if (items.Length > 1)
            {
                pims_id = items[0];
                epal_ver_eff_dt = items[1];
                //is_current = items[2];
                pims_id_eff_dt = pims_id + "," + epal_ver_eff_dt;
                data = await _service.GetEPALProcedureByPIMS_ID(new EPAL_Procedures_Param_Dto(pims_id + "," + epal_ver_eff_dt, _helper.MS_ID));
                //ViewBag.MAX_EXP_DT = await _repo.MaxInActivePrioAuthOrPredetExpDate(pims_id);
            }
            else
            {
                data = await _service.GetEPALProcedureCurrVerByPIMS_ID(new EPAL_Procedures_Param_Dto(pims_id, _helper.MS_ID));
                pims_id_eff_dt = pims_id;
            }

            if (data.EPAL_HIERARCHY_KEY != null)
            {
                var procsSOS = await _service.GetPIMSProcsSOS(new EPAL_Procedures_Param_Dto(pims_id_eff_dt, _helper.MS_ID));
                if (procsSOS != null)
                {
                    data.SOS_URG_CAT_MDLTY = procsSOS.SOS_URG_CAT_MDLTY;
                    data.SOS_TYPE = procsSOS.SOS_TYPE;
                    data.SOS_SITE_IND = procsSOS.SOS_SITE_IND;
                }

                var SOS_SITE_AND_SERVICE_VV = await _pimsValidValuesService.GetSOSSiteAndService();
                data.SOS_SITE_AND_SERVICE_VV = SOS_SITE_AND_SERVICE_VV.VV_CD_DESC;

                var SOS_SITE_ONLY_VV = await _pimsValidValuesService.GetSOSSiteOnly();
                data.SOS_SITE_ONLY_VV = SOS_SITE_ONLY_VV.VV_CD_DESC;

                // USER STORY 47808
                var max_Prior_Auth_Dt_Dto = await _EPALProceduresService.GetEPALProcedureTMaxPriorAuthDt(new EPAL_Procedures_Param_Dto { p_EPAL_HIERARCHY_KEY = data.EPAL_HIERARCHY_KEY });
                if (max_Prior_Auth_Dt_Dto.MAX_PRIOR_AUTH_EXP_DT != null)
                    data.MAX_PRIOR_AUTH_EXP_DT = max_Prior_Auth_Dt_Dto.MAX_PRIOR_AUTH_EXP_DT.Value.AddDays(1);
                else
                    data.MAX_PRIOR_AUTH_EXP_DT = null;
            }

            // USER STORY 95701 MFQ 4-5-2024
            List<EPALDrivingSourceRangeDto> epalDrivingSourceRange = new();

            if (data.PRIOR_AUTH_EFF_DT != null) {
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.PA.GetEnumDescription(), StartDate = Convert.ToDateTime(data.PRIOR_AUTH_EFF_DT), EndDate = data.PRIOR_AUTH_EXP_DT == null ? Convert.ToDateTime("12/31/2999"): Convert.ToDateTime(data.PRIOR_AUTH_EXP_DT) } );                
            }
            if (data.PRE_DET_EFF_DT != null){
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.Pred.GetEnumDescription(), StartDate = Convert.ToDateTime(data.PRE_DET_EFF_DT), EndDate = data.PRIOR_AUTH_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.PRE_DET_EXP_DT) });
            }
            if (data.ADV_NTFCTN_EFF_DT != null){
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.Adv.GetEnumDescription(), StartDate = Convert.ToDateTime(data.ADV_NTFCTN_EFF_DT), EndDate = data.PRIOR_AUTH_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.ADV_NTFCTN_EXP_DT) });
            }
            if (data.DRAL_EFF_DT != null){
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.DRAL.GetEnumDescription(), StartDate = Convert.ToDateTime(data.DRAL_EFF_DT), EndDate = data.PRIOR_AUTH_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.DRAL_EXP_DT) });
            }
            if (data.AUTO_APRVL_EFF_DT != null)
            {
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.AA.GetEnumDescription(), StartDate = Convert.ToDateTime(data.AUTO_APRVL_EFF_DT), EndDate = data.AUTO_APRVL_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.AUTO_APRVL_EXP_DT) });
            }
            if (data.MCARE_SPCL_PRCSNG_EFF_DT != null)
            {
                epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.MSP.GetEnumDescription(), StartDate = Convert.ToDateTime(data.MCARE_SPCL_PRCSNG_EFF_DT), EndDate = data.MCARE_SPCL_PRCSNG_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.MCARE_SPCL_PRCSNG_EXP_DT) });
            }
            data.epalDrivingSourceRangeModel = GetDrivingStatusDateRangeIncludingToday(epalDrivingSourceRange, DateTime.Today);

            if(data.epalDrivingSourceRangeModel != null)
            {
                if (data.epalDrivingSourceRangeModel.Name == EPALDrivingStatusEnum.PA.GetEnumDescription() && data.SOS_EFF_DT != null)
                {
                    data.epalDrivingSourceRangeModel.Name = EPALDrivingStatusEnum.PASOS.GetEnumDescription();
                    data.epalDrivingSourceRangeModel.DisplayName = EPALDrivingStatusEnum.PASOS.GetEnumDescription();
                }
                else if (data.epalDrivingSourceRangeModel.Name == EPALDrivingStatusEnum.Adv.GetEnumDescription() && data.SOS_EFF_DT != null)
                {
                    data.epalDrivingSourceRangeModel.Name = EPALDrivingStatusEnum.AdvSOS.GetEnumDescription();
                    data.epalDrivingSourceRangeModel.DisplayName = EPALDrivingStatusEnum.AdvSOS.GetEnumDescription();
                }
            }
            // END USER STORY 95701 MFQ 4-5-2024            

            //BUG 36149 MFQ 10/7/2022
            data = await validateDates(data);

            // get list of all valid states from db to use in UI
            data.states = await getValidStates();

            // If data is curropted (dates) then redirect to view only mode.
            if (!String.IsNullOrEmpty(data.dateErrorString))
            {
                return RedirectToAction("ViewDetail", "Home", new { pims_id = pims_id, Area = "EPAL" });
            }

            if (data.EPAL_HIERARCHY_KEY != null)
            {
                EPAL_Procedures_Param_Dto obj = new EPAL_Procedures_Param_Dto() { p_EPAL_HIERARCHY_KEY = pims_id };
                var additinalInfoData = await _service.GetPIMSAdditionalInfoHistory(obj);

                DIAG_LIST_NAME_CNT_Dto obj2 = new DIAG_LIST_NAME_CNT_Dto();
                EPAL_Procedures_Param_Dto param = new EPAL_Procedures_Param_Dto() { p_EPAL_HIERARCHY_KEY = data.EPAL_HIERARCHY_KEY, p_EPAL_VER_EFF_DT = data.EPAL_VER_EFF_DT };

                obj2 = await _refDiagnosesService.GetPIMS_DiagCount(param);

                ViewBag.Note = MI.PIMS.UI.Common.Helper.GetPIMSAdditionalInfoHistory(additinalInfoData, "Note");
                ViewBag.Future = MI.PIMS.UI.Common.Helper.GetPIMSAdditionalInfoHistory(additinalInfoData, "Future");
                ViewBag.EditRecord = "Y";
                ViewBag.DiagListCount = obj2.LIST_NAME_CNT;


                //=================================================================================
                // DY: 10/29/2025: User Story 138187 - Check if Donar Record 
                //=================================================================================
                EPAL_Procedures_V_Dto CheckDonarRecord;
                CheckDonarRecord = await _service.CheckIsDonorRecord(pims_id);
                
                if (CheckDonarRecord.IS_DONOR_RECORD > 0)
                {
                    data.IS_DONOR_RECORD = 1;
                }
                else {
                    data.IS_DONOR_RECORD = 0;
                }

                //=================================================================================
                // DY: 01/28/2024 - Only allow Super Admins or EPAL Admins to see the pencil icon. 
                // DY: 05/07/2024: Show Historical Update button if user is CMP Admin
                // DY: 10/27/2025: Show Historical Update button if user is Donar/ICHRA Admin
                //=================================================================================
                if (data.IS_CURRENT == "N"
                        && (User.IsInRole("1")          //  1  - Super Admin
                            || User.IsInRole("4")       //  4  - EPAL Admin
                            || User.IsInRole("20")      //  20 - CMP Admin
                            || User.IsInRole("24")      //  24 - Donor Records Admin
                            || User.IsInRole("28"))     //  28 - ICHRA Admin
                            )
                {
                    ViewBag.ShowHistoricalUpdateButton = true;
                }
                else
                {
                    ViewBag.ShowHistoricalUpdateButton = false;
                }

                //=================================================================================
                // DY: 01/28/2024 - Only allow Super Admins or EPAL Admins to see the pencil icon. 
                // DY: 05/07/2024: Show Delete button if user is CMP Admin
                // DY: 10/27/2025: Show Delete button if user is Donar/ICHRA Admin
                //=================================================================================
                if (User.IsInRole("1")          // 1  - Super Admin
                    || User.IsInRole("4")       // 4  - EPAL Admin
                    || User.IsInRole("20")      // 20 - CMP Admin
                    || User.IsInRole("24")      // 24 - Donor Records Admin
                    || User.IsInRole("28")      // 28 - ICHRA Admin
                    )  
                {
                    ViewBag.ShowDeleteButton = true;
                }
                else
                {
                    ViewBag.ShowDeleteButton = false;
                }

                if (data.EPAL_ENTITY_CD == "UMR")
                {
                    //=================================================================================
                    // All UMR roles or Super Admin Roles allowed. 
                    //=================================================================================
                    if (User.IsInRole("11")         // 11 - UMR Read Only
                        || User.IsInRole("12")      // 12 - UMR Read Write
                        || User.IsInRole("13")      // 13 - UMR Admin
                        || User.IsInRole("1")       // 1  - Super Admin
                    )
                    {
                        if (User.IsInRole("12")     // 12 - UMR Read Write 
                            || User.IsInRole("13")  // 13 - UMR Admin
                            || User.IsInRole("1"))  // 1  - Super Admin
                        {
                            ViewBag.UMR_Edit = true;
                            return View(data);
                        }
                        else
                        {
                            return View("UMRForbidden");
                        }
                    }
                    else
                    {
                        return View("UMRForbidden");
                    }
                }


                else if (data.EPAL_PRODUCT_CD == "ICHRA")
                {
                    //=================================================================================
                    // DY: 10/27/2025: All ICHRA roles or Super Admin Roles allowed. 
                    //=================================================================================
                    if (User.IsInRole("26")         // 26 - ICHRA Read Only
                        || User.IsInRole("27")      // 27 - ICHRA Read Write
                        || User.IsInRole("28")      // 28 - ICHRA Admin
                        || User.IsInRole("1")       // 1  - Super Admin
                    )
                    {
                        if (User.IsInRole("27")     // 27 - ICHRA Read Write 
                            || User.IsInRole("28")  // 28 - ICHRA Admin
                            || User.IsInRole("1"))  // 1  - Super Admin
                        {
                            ViewBag.ICHRA_Edit = true;
                            return View(data);
                        }
                        else
                        {
                            return View("ICHRAForbidden");
                        }
                    }
                    else
                    {
                        return View("ICHRAForbidden");
                    }
                }

                else if (data.IS_DONOR_RECORD == 1)
                {
                    //=================================================================================
                    // DY: 10/27/2025: All Donor roles or Super Admin Roles allowed. 
                    //=================================================================================
                    if (User.IsInRole("22")         // 22 - Donor Records Read Only
                        || User.IsInRole("23")      // 23 - Donor Records Read Write
                        || User.IsInRole("24")      // 24 - Donor Records Admin
                        || User.IsInRole("1")       // 1  - Super Admin
                    )
                    {
                        if (User.IsInRole("23")     // 23 - Donor Records Read Write
                            || User.IsInRole("24")  // 24 - Donor Records Admin
                            || User.IsInRole("1"))  // 1  - Super Admin
                        {
                            ViewBag.DonorRecords_Edit = true;
                            return View(data);
                        }
                        else
                        {
                            return View("DonorRecordsForbidden");
                        }
                    }
                    else
                    {
                        return View("DonorRecordsForbidden");
                    }
                }

                else if (data.EPAL_ENTITY_CD != "UMR")
                {
                    if (data.EPAL_BUS_SEG_CD.ToUpper().Contains("_CMP") 
                        || data.EPAL_BUS_SEG_CD.ToUpper().Contains("CMP_")
                        )
                    {
                        if (User.IsInRole("19")     // 19 - CMP Read Write
                            || User.IsInRole("20")  // 20 - CMP Admin
                            || User.IsInRole("1")   // 1 - Super Admin
                            )
                        {
                            return View(data);
                        }
                        else
                        {
                            return View("CMPForbidden");
                        }
                    }
                    else 
                    {
                        // EPAL Admin, ReadWrite roles or Super Admin Roles allowed. 
                        if (User.IsInRole("3")          // 3 - EPAL Read Write    
                            || User.IsInRole("4")       // 4 - EPAL Admin
                            || User.IsInRole("1")       // 1 - Super Admin
                            )
                        {
                            ViewBag.EPAL_Edit = true;
                            return View(data);
                        }
                        else
                        {                             
                            if (User.IsInRole("18")         // 18 - CMP Read Only
                                  || User.IsInRole("19")    // 19 - CMP Read Write
                                  || User.IsInRole("20")    // 20 - CMP Admin
                                  || User.IsInRole("21")    // 21 - CMP Reports Only
                                )
                            {
                                return View("NonCMPForbidden");
                            }

                            else if (User.IsInRole("11")    // 11 - UMR Read Only
                              || User.IsInRole("12")        // 12 - UMR Read Write
                              || User.IsInRole("13"))       // 13 - UMR Admin
                            {
                                return View("NonUMRForbidden");
                            }

                            else {
                                return View(data);
                            }
                        }
                    }


                }

                else
                {
                    return View(data);
                }


            }
            else
            {
                return RedirectToAction("RecordNotFound", "ErrorHandler");
                // return View();
            }
        }

        /// <summary>
        /// //BUG 36149 MFQ 10/7/2022
        /// </summary>
        /// <param name="epal"></param>
        /// <returns></returns>
        private async Task<EPAL_Procedures_V_Dto> validateDates(EPAL_Procedures_V_Dto epal)
        {
            epal.dateErrorString = string.Empty;

            if (epal.PROC_CD_EFF_DT > epal.PROC_CD_EXP_DT)
            {
                epal.dateErrorString = "";
            }

            if (epal.EPAL_VER_EFF_DT > epal.EPAL_VER_EXP_DT)
            {
                epal.dateErrorString += "EPAL Version Exp Date cannot be less than EPAL Version Eff Date!\n";
            }

            if (epal.OVERALL_EFF_DT > epal.OVERALL_EXP_DT)
            {
                epal.dateErrorString += ""; //"Overall Exp Date cannot be less than Overall Eff Date!\n";
            }

            if (epal.PRIOR_AUTH_EFF_DT > epal.PRIOR_AUTH_EXP_DT)
            {
                epal.dateErrorString += "PA Expiration Date cannot be less than PA Effective Date!\n";
            }

            if (epal.AUTO_APRVL_EFF_DT > epal.AUTO_APRVL_EXP_DT)
            {
                epal.dateErrorString += "AA Expiration Date cannot be less than AA Effective Date!\n";
            }

            if (epal.MCARE_SPCL_PRCSNG_EFF_DT > epal.MCARE_SPCL_PRCSNG_EXP_DT)
            {
                epal.dateErrorString += "MSP Expiration Date cannot be less than MSP Effective Date!\n";
            }

            if (epal.SOS_EFF_DT > epal.SOS_EXP_DT)
            {
                epal.dateErrorString += "SOS Expiration Date cannot be less than SOS Effective Date!\n";
            }

            if (!string.IsNullOrEmpty(epal.dateErrorString))
            {
                epal.dateErrorString += "\nYou have been redirected to View mode, Please ask Administrator for data correction!";
            }

            epal.dateErrorString = epal.dateErrorString.Trim();

            return await Task.FromResult(epal);
        }

        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.EPALReadWrite, 
                        Roles.EPALAdmin,
                        Roles.UMRAdmin,
                        Roles.UMRReadWrite,
                        Roles.CMPAdmin,
                        Roles.CMPReadWrite,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin
            )]
        [Route("EPAL/Home/AddDetail/{pims_id}")]
        public async Task<IActionResult> AddDetail(string pims_id)
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL", "EPAL/Home/AddDetail?pims_id=" + pims_id, "EPAL/Home/AddDetail", 4);

            pims_id = HttpUtility.HtmlDecode(pims_id);
            string s = pims_id;
            string[] items = s.Split('-');
            ViewBag.EPALPageView = EPALPageView.AddDetail.ToString();

            EPAL_Procedures_V_Dto data = new EPAL_Procedures_V_Dto();
            {
                data.EPAL_BUS_SEG_CD = items[0];
                data.EPAL_ENTITY_CD = items[1];
                data.EPAL_PLAN_CD = items[2];
                data.EPAL_PRODUCT_CD = items[3];
                data.EPAL_FUND_ARNGMNT_CD = items[4];
                data.PROC_CD = items[5];
                data.DRUG_NM = items[6];
                data.EPAL_HIERARCHY_KEY = pims_id.Contains(" ") ? pims_id.Substring(0, pims_id.IndexOf(" ")) : pims_id;
                data.EPAL_STATUS = "";
            };


            //=================================================================================
            // DY: 11/06/2025: User Story 138187 - Check if Donar Record 
            //=================================================================================
            EPAL_Procedures_V_Dto CheckDonarRecord;
            CheckDonarRecord = await _service.CheckIsDonorRecord(pims_id);

            if (CheckDonarRecord.IS_DONOR_RECORD > 0)
            {
                data.IS_DONOR_RECORD = 1;
            }
            else
            {
                data.IS_DONOR_RECORD = 0;
            }


            // ------> USER STORY 33957 MFQ 9/23/2022
            var categories = await _EPALProceduresService.GetPIMSEPALCategories(new EPAL_Catagories_Dto { I_EPAL_HIERARCHY_KEY = data.EPAL_HIERARCHY_KEY });
            if (categories != null)
            {
                data.ALTRNT_SVC_CAT = categories.O_ALTERNATE_CATEGORY;
                data.ALTRNT_SVC_SUBCAT = categories.O_ALTERNATE_SUB_CATEGORY;
                data.STNDRD_SVC_CAT = categories.O_STANDARD_CATEGORY;
                data.STNDRD_SVC_SUBCAT = categories.O_STANDARD_SUB_CATEGORY;
            }
            // ------> END USER STORY 33957 MFQ 9/23/2022

            ViewBag.ShowHistoricalUpdateButton = false;
            ViewBag.ShowDeleteButton = false;
            ViewBag.ShowDuplicateButton = false;
            ViewBag.AddRecord = "Y";

            // get list of all valid states from db to use in UI
            data.states = await getValidStates();


            if (data.IS_DONOR_RECORD == 1)
            {
                //=================================================================================
                // DY: 11/06/2025: All Donor roles or Super Admin Roles allowed. 
                //=================================================================================
                if (User.IsInRole("22")         // 22 - Donor Records Read Only
                    || User.IsInRole("23")      // 23 - Donor Records Read Write
                    || User.IsInRole("24")      // 24 - Donor Records Admin
                    || User.IsInRole("1")       // 1  - Super Admin
                )
                {
                    if (User.IsInRole("23")     // 23 - Donor Records Read Write
                        || User.IsInRole("24")  // 24 - Donor Records Admin
                        || User.IsInRole("1"))  // 1  - Super Admin
                    {
                        //ViewBag.DonorRecords_Edit = true;
                        //return View(data);
                        return await Task.Run(() => View(data));
                    }
                    else
                    {
                        return View("DonorRecordsForbidden");
                    }
                }
                else
                {
                    return View("DonorRecordsForbidden");
                }
            }

            else if (data.EPAL_PRODUCT_CD == "ICHRA")
            {
                //=================================================================================
                // DY: 11/06/2025: All ICHRA roles or Super Admin Roles allowed. 
                //=================================================================================
                if (User.IsInRole("26")         // 26 - ICHRA Read Only
                    || User.IsInRole("27")      // 27 - ICHRA Read Write
                    || User.IsInRole("28")      // 28 - ICHRA Admin
                    || User.IsInRole("1")       // 1  - Super Admin
                )
                {
                    if (User.IsInRole("27")     // 27 - ICHRA Read Write 
                        || User.IsInRole("28")  // 28 - ICHRA Admin
                        || User.IsInRole("1"))  // 1  - Super Admin
                    {
                        ViewBag.ICHRA_Edit = true;
                        return View(data);
                    }
                    else
                    {
                        return View("ICHRAForbidden");
                    }
                }
                else
                {
                    return View("ICHRAForbidden");
                }
            }

            else {
                return await Task.Run(() => View(data));
            }
                



        }

        [AuthorizeRoles(Roles.SuperAdmin,
                        Roles.EPALReadOnly,
                        Roles.EPALReadWrite,
                        Roles.EPALAdmin,
                        Roles.DPOCReadOnly,
                        Roles.PayCodeReadOnly,
                        Roles.UMRReadOnly,
                        Roles.UMRAdmin,
                        Roles.UMRReadWrite,
                        Roles.CMPReadOnly,
                        Roles.CMPAdmin,
                        Roles.CMPReadWrite,
                        Roles.DonorRecordsReadOnly,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.ICHRAReadOnly,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin
                        )]
        [Route("EPAL/Home/ViewDetail/{pims_id}")]
        public async Task<IActionResult> ViewDetail(string pims_id)
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL", "EPAL/Home/ViewDetail?pims_id=" + pims_id, "EPAL/Home/ViewDetail", 4);
            pims_id = HttpUtility.HtmlDecode(pims_id);
            string s = pims_id;
            string originalPimsId = pims_id;
            string epal_ver_eff_dt;
            string pims_id_eff_dt;
            //string is_current;
            string[] items1 = s.Split(',');

            EPAL_Procedures_V_Dto data;
            if (items1.Length > 1)
            {
                pims_id = items1[0];
                epal_ver_eff_dt = items1[1];
                pims_id_eff_dt = pims_id + "," + epal_ver_eff_dt;
                data = await _service.GetEPALProcedureByPIMS_ID(new EPAL_Procedures_Param_Dto(pims_id + "," + epal_ver_eff_dt, _helper.MS_ID));
            }

            else
            {
                data = await _service.GetEPALProcedureCurrVerByPIMS_ID(new EPAL_Procedures_Param_Dto(pims_id, _helper.MS_ID));
                pims_id_eff_dt = pims_id;
            }



            if (data.EPAL_HIERARCHY_KEY != null)
            {
                string pimsIDAdditionalInfo = "";

                if (pims_id.Contains(","))
                {
                    string[] items = pims_id.Split(',');
                    pimsIDAdditionalInfo = items[0];
                }
                else
                {
                    pimsIDAdditionalInfo = pims_id;
                }


                // USER STORY 95701 MFQ 4-5-2024
                List<EPALDrivingSourceRangeDto> epalDrivingSourceRange = new();
                if (data.PRIOR_AUTH_EFF_DT != null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.PA.GetEnumDescription(), StartDate = Convert.ToDateTime(data.PRIOR_AUTH_EFF_DT), EndDate = data.PRIOR_AUTH_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.PRIOR_AUTH_EXP_DT) });
                }
                if (data.PRE_DET_EFF_DT != null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.Pred.GetEnumDescription(), StartDate = Convert.ToDateTime(data.PRE_DET_EFF_DT), EndDate = data.PRE_DET_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.PRE_DET_EXP_DT) });
                }
                if (data.ADV_NTFCTN_EFF_DT != null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.Adv.GetEnumDescription(), StartDate = Convert.ToDateTime(data.ADV_NTFCTN_EFF_DT), EndDate = data.ADV_NTFCTN_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.ADV_NTFCTN_EXP_DT) });
                }
                if (data.DRAL_EFF_DT != null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.DRAL.GetEnumDescription(), StartDate = Convert.ToDateTime(data.DRAL_EFF_DT), EndDate = data.DRAL_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.DRAL_EXP_DT) });
                }
                if (data.AUTO_APRVL_EFF_DT != null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.AA.GetEnumDescription(), StartDate = Convert.ToDateTime(data.AUTO_APRVL_EFF_DT), EndDate = data.AUTO_APRVL_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.AUTO_APRVL_EXP_DT) });
                }
                if (data.MCARE_SPCL_PRCSNG_EFF_DT!= null)
                {
                    epalDrivingSourceRange.Add(new EPALDrivingSourceRangeDto() { Name = EPALDrivingStatusEnum.MSP.GetEnumDescription(), StartDate = Convert.ToDateTime(data.MCARE_SPCL_PRCSNG_EFF_DT), EndDate = data.MCARE_SPCL_PRCSNG_EXP_DT == null ? Convert.ToDateTime("12/31/2999") : Convert.ToDateTime(data.MCARE_SPCL_PRCSNG_EXP_DT) });
                }
                data.epalDrivingSourceRangeModel = GetDrivingStatusDateRangeIncludingToday(epalDrivingSourceRange, DateTime.Today);

                if (data.epalDrivingSourceRangeModel !=null) 
                {
                    if (data.epalDrivingSourceRangeModel.Name == EPALDrivingStatusEnum.PA.GetEnumDescription() && data.SOS_EFF_DT != null)
                    {
                        data.epalDrivingSourceRangeModel.Name = EPALDrivingStatusEnum.PASOS.GetEnumDescription();
                    }
                }

                // END USER STORY 95701 MFQ 4-5-2024

                var SOS_SITE_AND_SERVICE_VV = await _pimsValidValuesService.GetSOSSiteAndService();
                data.SOS_SITE_AND_SERVICE_VV = SOS_SITE_AND_SERVICE_VV.VV_CD_DESC;

                var SOS_SITE_ONLY_VV = await _pimsValidValuesService.GetSOSSiteOnly();
                data.SOS_SITE_ONLY_VV = SOS_SITE_ONLY_VV.VV_CD_DESC;


                EPAL_Procedures_Param_Dto obj = new EPAL_Procedures_Param_Dto() { p_EPAL_HIERARCHY_KEY = pimsIDAdditionalInfo };
                var additinalInfoData = await _service.GetPIMSAdditionalInfoHistory(obj);

                ViewBag.Note = MI.PIMS.UI.Common.Helper.GetPIMSAdditionalInfoHistory(additinalInfoData, "Note");
                ViewBag.Future = MI.PIMS.UI.Common.Helper.GetPIMSAdditionalInfoHistory(additinalInfoData, "Future");


                //=================================================================================
                // DY: 10/29/2025: User Story 138187 - Check if Donar Record 
                //=================================================================================
                EPAL_Procedures_V_Dto CheckDonarRecord;
                CheckDonarRecord = await _service.CheckIsDonorRecord(pims_id);

                if (CheckDonarRecord.IS_DONOR_RECORD > 0)
                {
                    data.IS_DONOR_RECORD = 1;
                }
                else
                {
                    data.IS_DONOR_RECORD = 0;
                }


                if (User.IsInRole("1")          //1 - Super Admin
                    || User.IsInRole("3")       //3 - EPAL Read Write                                               
                    || User.IsInRole("4")       //4 - EPAL Admin
                    || User.IsInRole("12")      //12 - UMR Read Write
                    || User.IsInRole("13")      //13 - UMR Admin
                    || User.IsInRole("19")      //19 - CMP Read Write
                    || User.IsInRole("20")      //20 - CMP Admin
                    || User.IsInRole("23")      //23 - Donor Records Read Write
                    || User.IsInRole("24")      //24 - Donor Records Admin
                    || User.IsInRole("27")      //27 - ICHRA Read Write
                    || User.IsInRole("28")      //28 - ICHRA Admin
                    )
                {

                    /*=============================================
                        EPAL Read/Write, EPAL Admin, Super Admin
                    ==============================================*/
                    if (User.IsInRole("3")      //3-EPAL Read/Write
                        || User.IsInRole("4")   //4-EPAL Admin
                        || User.IsInRole("1"))  //1-Super Admin
                    {
                        ViewBag.EPAL_Edit = true;
                    }
                    else
                    {
                        ViewBag.EPAL_Edit = false;
                    }


                    /*=============================================
                        UMR Read/Write, UMR Admin, Super Admin
                    ==============================================*/
                    if (User.IsInRole("12")     //12-UMR Read/Write
                        || User.IsInRole("13")  //13-UMR Admin
                        || User.IsInRole("1")   //1-Super Admin
                        ) 
                    {
                        ViewBag.UMR_Edit = true;
                    }
                    else
                    {
                        ViewBag.UMR_Edit = false;
                    }


                    /*=============================================
                        CMP Read/Write, CMP Admin, Super Admin
                    ==============================================*/
                    if (User.IsInRole("19")     //19-CMP Read/Write
                        || User.IsInRole("20")  //20-CMP Admin
                        || User.IsInRole("1")   //1-Super Admin
                        )  
                    {
                        ViewBag.CMP_Edit = true;
                    }
                    else
                    {
                        ViewBag.CMP_Edit = false;
                    }

                    //User Story 138187: PIMS ADMIN Functionality - New Security Role for Donor Records
                    /*=============================================
                        DonorRecords Read/Write, DonorRecords Admin, Super Admin
                    ==============================================*/
                    if (User.IsInRole("23")     //23 - Donor Records Read Write
                        || User.IsInRole("24")  //24 - Donor Records Admin
                        || User.IsInRole("1")   //1 - Super Admin
                        )
                    {
                        ViewBag.DonorRecords_Edit = true;
                    }
                    else
                    {
                        ViewBag.DonorRecords_Edit = false;
                    }

                    //User Story 138188: PIMS ADMIN Functionality - New Security Role for New ICHRA records
                    /*=============================================
                        ICHRA Read/Write, ICHRA Admin, Super Admin                       
                    ==============================================*/
                    if (User.IsInRole("27")     //27-ICHRA Read/Write
                        || User.IsInRole("28")  //28-ICHRA Admin
                        || User.IsInRole("1"))  //1-Super Admin
                    {
                        ViewBag.ICHRA_Edit = true;
                    }
                    else
                    {
                        ViewBag.ICHRA_Edit = false;
                    }

                    ViewBag.ShowEditButton = true;
                }

                // For historical EPAL record
                if (items1.Length > 1)
                {
                    pims_id = items1[0];
                    epal_ver_eff_dt = items1[1];

                    var procsSOS = await _service.GetPIMSProcsSOS(new EPAL_Procedures_Param_Dto(pims_id + "," + epal_ver_eff_dt, _helper.MS_ID));
                    if (procsSOS != null)
                    {
                        data.SOS_EFF_DT = procsSOS.SOS_EFF_DT;
                        data.SOS_EXP_DT = procsSOS.SOS_EXP_DT;
                        data.SOS_URG_CAT_MDLTY = procsSOS.SOS_URG_CAT_MDLTY;
                        data.SOS_TYPE = procsSOS.SOS_TYPE;
                        data.SOS_SITE_IND = procsSOS.SOS_SITE_IND;
                    }
                }
                else
                {
                    // For current EPAL record
                    var procsSOS = await _service.GetPIMSProcsSOS(new EPAL_Procedures_Param_Dto(pims_id, _helper.MS_ID));
                    if (procsSOS != null)
                    {
                        data.SOS_EFF_DT = procsSOS.SOS_EFF_DT;
                        data.SOS_EXP_DT = procsSOS.SOS_EXP_DT;
                        data.SOS_URG_CAT_MDLTY = procsSOS.SOS_URG_CAT_MDLTY;
                        data.SOS_TYPE = procsSOS.SOS_TYPE;
                        data.SOS_SITE_IND = procsSOS.SOS_SITE_IND;
                    }
                }

                data = await validateDates(data);
                /*=============================================
                // DY - 01/23/2024: Only show Historical Update button if user is SuperAdmin or EPAL Admin
                // DY - 05/07/2024: Show Historical Update button if user is CMP Admin
                // DY - 10/28/2025: Show Historical Update button if user is Donor Records and ICHRA Admin
                =============================================*/
                if (data.IS_CURRENT == "N" && 
                    (User.IsInRole("1")         //1 - Super Admin
                    || User.IsInRole("4")       //4 - EPAL Admin
                    || User.IsInRole("20")      //20 - CMP Admin
                    || User.IsInRole("24")      //24 - Donor Records Admin
                    || User.IsInRole("28")      //28 - ICHRA Admin
                    ))
                {
                    ViewBag.ShowHistoricalUpdateButton = true;
                }
                else
                {
                    ViewBag.ShowHistoricalUpdateButton = false;
                }



                if (data.EPAL_ENTITY_CD == "UMR")
                {
                    // All UMR roles or Super Admin Roles allowed. 
                    if (User.IsInRole("11")         //11 - UMR Read Only
                        || User.IsInRole("12")      //12 - UMR Read Write     
                        || User.IsInRole("13")      //13 - UMR Admin
                        || User.IsInRole("1")       //1 - Super Admin
                        )
                    {
                        return View(data);
                    }
                    else
                    {
                        return View("UMRForbidden");
                    }
                }


                /*============================================================================
                  1. User Story 138187: PIMS ADMIN Functionality - New Security Role for Donor Records
                ============================================================================*/
                else if (data.IS_DONOR_RECORD == 1)
                {
                    // All ICHRA roles or Super Admin Roles allowed. 
                    if (User.IsInRole("22")         // 22 - Donor Records Read Only
                        || User.IsInRole("23")      // 23 - Donor Records Read Write   
                        || User.IsInRole("24")      // 24 - Donor Records Admin
                        || User.IsInRole("1")       // 1 - Super Admin
                        )
                    {
                        return View(data);
                    }
                    else
                    {
                        return View("DonorRecordsForbidden");
                    }
                }


                /*============================================================================
                  2. User Story 138188: PIMS ADMIN Functionality - New Security Role for New ICHRA records
                ============================================================================*/
                else if (data.EPAL_PRODUCT_CD == "ICHRA")
                {
                    // All ICHRA roles or Super Admin Roles allowed. 
                    if (User.IsInRole("26")         //26 - ICHRA Read Only
                        || User.IsInRole("27")      //27 - ICHRA Read Write     
                        || User.IsInRole("28")      //28 - ICHRA Admin
                        || User.IsInRole("1")       //1 - Super Admin
                        )
                    {
                        return View(data);
                    }
                    else
                    {
                        return View("ICHRAForbidden");
                    }
                }



                else if (data.EPAL_ENTITY_CD != "UMR")
                {
                    if (data.EPAL_BUS_SEG_CD.ToUpper().Contains("_CMP") || data.EPAL_BUS_SEG_CD.ToUpper().Contains("CMP_"))
                        {
                        if (User.IsInRole("18")     //18 - CMP Read Only
                            || User.IsInRole("19")  //19 - CMP Read Write 
                            || User.IsInRole("20")  //20 - CMP Admin
                            || User.IsInRole("1")   //1 - Super Admin
                            )
                        {
                            return View(data);
                        }
                        else
                        {
                            return View("CMPForbidden");
                        }

                    }
                    else {
                        // All EPAL roles or Super Admin Roles allowed. 
                        if (User.IsInRole("2")      //2 - EPAL Read Only
                            || User.IsInRole("3")   //3 - EPAL Read Write
                            || User.IsInRole("4")   //4 - EPAL Admin
                            || User.IsInRole("1")   //1 - Super Admin
                            )
                        {
                            return View(data);
                        }
                        else
                        {
                            if (User.IsInRole("18")     //18 - CMP Read Only
                                || User.IsInRole("19")  //19 - CMP Read Write
                                || User.IsInRole("20")  //20 - CMP Admin
                                || User.IsInRole("21")  //21 - CMP Reports Only
                                )
                                {
                                    return View("NonCMPForbidden");
                                }

                                else if (User.IsInRole("11")    //11 - UMR Read Only
                                    || User.IsInRole("12")      //12 - UMR Read Write
                                    || User.IsInRole("13")      //13 - UMR Admin
                                    )
                                {
                                    return View("NonUMRForbidden");
                                }

                                else
                                {
                                    return View(data);
                                }
                        }
                    }
                }


                else
                {
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("RecordNotFound", "ErrorHandler", new { Area = "", ErrorViewModel = new ErrorViewModel() { RequestId = "404", Message = $"PIMS ID:{originalPimsId}" }});
            }
        }

        [AuthorizeRoles(Roles.SuperAdmin, 
                        Roles.EPALReadWrite, 
                        Roles.EPALAdmin, 
                        Roles.UMRAdmin, 
                        Roles.UMRReadWrite,
                        Roles.CMPAdmin,
                        Roles.CMPReadWrite,
                        Roles.DonorRecordsReadWrite,
                        Roles.DonorRecordsAdmin,
                        Roles.ICHRAReadWrite,
                        Roles.ICHRAAdmin
                        )]
        [Route("EPAL/Home/DuplicateRecordDetail/{pims_id}")]
        public async Task<IActionResult> DuplicateRecordDetail(string pims_id)
        {
            // Log user activity
            //await _userAccessHistRepository.Add("EPAL", "EPAL/Home/DuplicateRecordDetail?pims_id=" + pims_id, "EPAL/Home/DuplicateRecordDetail", 4);
            pims_id = HttpUtility.HtmlDecode(pims_id);
            ViewBag.DuplicateRecord = "Y";
            EPAL_Procedures_V_Dto data;
            data = await _service.GetEPALProcedureCurrVerByPIMS_ID(new EPAL_Procedures_Param_Dto(pims_id, _helper.MS_ID));

            data.states = await getValidStates();

            if (data.EPAL_HIERARCHY_KEY != null)
            {
                ViewBag.orig_EPAL_HIERARCHY_KEY = data.EPAL_HIERARCHY_KEY;
                ViewBag.orig_EPAL_VER_EFF_DT = data.EPAL_VER_EFF_DT;
                ViewBag.orig_EPAL_BUS_SEG_CD = data.EPAL_BUS_SEG_CD;
                ViewBag.orig_EPAL_ENTITY_CD = data.EPAL_ENTITY_CD;

                //BUG 36149 MFQ 10/7/2022
                data = await validateDates(data);

                // If data is curropted (dates) then redirect to view only mode.
                if (!String.IsNullOrEmpty(data.dateErrorString))
                {
                    return RedirectToAction("ViewDetail", "Home", new { pims_id = pims_id, Area = "EPAL" });
                }

                return View(data);
            }
            else
            {
                return RedirectToAction("RecordNotFound", "ErrorHandler");
            }

        }

        #endregion

        #region KendoGrids

        public async Task<ActionResult> GetEPALProceduresSearch([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            var aiEventName = Helper.GetAppInsightCustomEventName(RouteData, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            _telemetry.TrackEvent(aiEventName, new LogInfo
            {
                Operation = $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}",
                UserId = _helper.MS_ID,
                AdditionalInfo = param
            });

            var data = await _service.GetEPALProceduresSearch(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetEPALProceduresHist([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            var data = await _service.GetEPALProceduresHist(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public async Task<ActionResult> GetProgramManagedByPIMSID([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            param = _helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param);
            IEnumerable<EPAL_Procs_Prog_Mgd_By_V> prog_Mgd_By_Vs = null;
            if (param.EPALPageView == EPALPageView.AddNew.ToStringNullSafe().ToLower() ||
                param.EPALPageView == EPALPageView.EditDetail.ToStringNullSafe().ToLower() ||
                param.EPALPageView == EPALPageView.AddDetail.ToStringNullSafe().ToLower() ||
                param.EPALPageView == EPALPageView.DuplicateRecordDetail.ToStringNullSafe().ToLower() ||
                param.EPALPageView == EPALPageView.ViewDetail.ToStringNullSafe().ToLower())
            {
                prog_Mgd_By_Vs = await _EPALProceduresService.GetProgMgdByPIMSID(param);
            }
            else if (_cnSIFPLogic.IsLOBCnSIFP(param.p_EPAL_BUS_SEG_CD) && param.EPALPageView == EPALPageView.DuplicateRecordDetail.ToStringNullSafe().ToLower())
            {
                prog_Mgd_By_Vs = await _EPALProceduresService.GetProgMgdByPIMSID(param);
            }

            if (prog_Mgd_By_Vs == null)
            {
                return new StatusCodeResult(200);
            }

            return Json(prog_Mgd_By_Vs.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetRetFactorsByPIMSID([DataSourceRequest] DataSourceRequest request, EPAL_Red_Ret_Param_Dto param)
        {
            var data = await _EPALProceduresService.GetRetFactorsByPIMSID(_helper.DecodeSanitizedFields<EPAL_Red_Ret_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetRedFactorsByPIMSID([DataSourceRequest] DataSourceRequest request, EPAL_Red_Ret_Param_Dto param)
        {
            var data = await _EPALProceduresService.GetRedFactorsByPIMSID(_helper.DecodeSanitizedFields<EPAL_Red_Ret_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }
        #endregion

        #region AssoccCodes        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DiagCodes_Create([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_AssoCodes_T_DiagCodes_Dto diagCodes_Dto)
        {
            var results = new List<EPAL_Procedures_AssoCodes_T_DiagCodes_Dto>();

            if (diagCodes_Dto != null && ModelState.IsValid)
            {
                //Service to add
                results.Add(diagCodes_Dto);
            }

            return await Task.FromResult(Json(results.ToDataSourceResult(request, ModelState)));
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<ActionResult> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            //var retVal = await _pimsValidValuesManager.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
            var retVal = await _pimsValidValuesService.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetPIMSValidValuesVVCdOnly(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            //var retVal = await _pimsValidValuesManager.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
            var retVal = await _pimsValidValuesService.GetPIMSValidValues_VVCdOnly(p_VV_SET_NAME, p_BUS_SEG_CD);            
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetPIMSHierarchyCodes(string EPAL_BUS_SEG_CD, string COLUMN_NAME)
        {
            var retVal = await _service.GetPIMSHierarchyCodesXwalkByEPALBusSegCD(new EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto(EPAL_BUS_SEG_CD, COLUMN_NAME, string.Empty));
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPIMSHierarchyCodesXwalk(string EPAL_BUS_SEG_CD, string EPAL_ENTITY_CD, string EPAL_PLAN_CD, string EPAL_PRODUCT_CD, string EPAL_FUND_ARNGMNT_CD, string COLUMN_NAME)
        {
            var retVal = await _service.GetAllPIMSHierarchyCodesXwalk();
            JsonResult toReturn = null;

            List<EPAL_PIMSHierarchyCode_V_Xwalk_All_Dto> result = retVal.ToList();

            var epalBusSegCdList = string.IsNullOrEmpty(EPAL_BUS_SEG_CD) ? new string[] { } : EPAL_BUS_SEG_CD.Split(',');
            var epalEntityCdList = string.IsNullOrEmpty(EPAL_ENTITY_CD) ? new string[] { } : EPAL_ENTITY_CD.Split(',');
            var epalPlanCdList = string.IsNullOrEmpty(EPAL_PLAN_CD) ? new string[] { } : EPAL_PLAN_CD.Split(',');
            var epalProductCdList = string.IsNullOrEmpty(EPAL_PRODUCT_CD) ? new string[] { } : EPAL_PRODUCT_CD.Split(',');
            var epalFundArngmntCdList = string.IsNullOrEmpty(EPAL_FUND_ARNGMNT_CD) ? new string[] { } : EPAL_FUND_ARNGMNT_CD.Split(',');

            // Bug 129889 replace 'Contains' with 'Equals'
            var toDropdown = result.Where(o => o.EPAL_HIERARCHY_STS == "Active"
                    && (string.IsNullOrEmpty(EPAL_BUS_SEG_CD) || epalBusSegCdList.Contains(o.EPAL_BUS_SEG_CD, StringComparer.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(EPAL_ENTITY_CD) || epalEntityCdList.Contains(o.EPAL_ENTITY_CD, StringComparer.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(EPAL_PLAN_CD) || epalPlanCdList.Contains(o.EPAL_PLAN_CD, StringComparer.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(EPAL_PRODUCT_CD) || epalProductCdList.Contains(o.EPAL_PRODUCT_CD, StringComparer.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(EPAL_FUND_ARNGMNT_CD) || epalFundArngmntCdList.Contains(o.EPAL_FUND_ARNGMNT_CD, StringComparer.OrdinalIgnoreCase))
                    );

            if (COLUMN_NAME == "BUS_SEG_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { VV_CD = o.EPAL_BUS_SEG_CD }).Distinct().OrderBy(o => o.VV_CD));
            }
            else if (COLUMN_NAME == "EPAL_ENTITY_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.EPAL_ENTITY_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }
            else if (COLUMN_NAME == "EPAL_PLAN_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.EPAL_PLAN_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }
            else if (COLUMN_NAME == "EPAL_PRODUCT_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.EPAL_PRODUCT_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }
            else if (COLUMN_NAME == "EPAL_FUND_ARNGMNT_CD")
            {
                toReturn = Json(toDropdown.Select(o => new { COLUMN_NAME = o.EPAL_FUND_ARNGMNT_CD }).Distinct().OrderBy(o => o.COLUMN_NAME));
            }

            return toReturn;
        }

        [HttpGet]
        public async Task<ActionResult> GetStateCDs()
        {
            var retVal = await _pimsValidValuesService.GetStateCDs();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetStateMandatedInds()
        {
            var retVal = await _pimsValidValuesService.GetStateMandatedInds();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetInclExclCDs()
        {
            var retVal = await _pimsValidValuesService.GetInclExclCDs();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllwdPlcOfSvcs()
        {
            var retVal = await _pimsValidValuesService.GetAllwdPlcOfSvcs();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetEPALProcedures(EPAL_Procedures_Param_Dto param)
        {
            var retVal = await _service.GetEPALProceduresSearch(param);
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckPIMSHierarchyCodeCombinationExists(EPAL_PIMSHierarchyCodeCombinationExists_Dto param)
        {
            var retVal = await _service.CheckPIMSHierarchyCodeCombinationExists(param);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetState()
        {
            var retVal = await _pimsValidValuesService.GetStateCDs();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetStatus()
        {
            var retVal = await _xrefStatusService.GetPIMSXrefStatus();
            return Json(retVal);
        }


        [HttpGet]
        public async Task<ActionResult> GetPIMS_DiagnosisList()
        {
            var retVal = await _refDiagnosesService.GetPIMS_DiagnosisList();
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetEPALProcStatus(EPAL_Procedures_Codes_Dto param)
        {
            var retVal = await _service.GetEPALProcStatus(param);
            return Json(retVal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEpal(EPAL_Ins_Upd_Pkg_Param obj)
        {
            obj.P_USER_ID = _helper.MS_ID;
            var data = await _service.EPAL_DELETE_DRIVER_PRC(obj);

            // Log user activity
            await _userAccessHistService.Add("EPAL", "EPAL/Home/EPAL_DELETE_DRIVER_PRC?epalPageView=" + obj.EPALPageView, JsonConvert.SerializeObject(obj), 7);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInsert(EPAL_Ins_Upd_Pkg_Param obj)
        {
            if (!ModelState.IsValid)
            {                
                _loggerService.Error("Invalid UpdateInsert ModelState for EPAL Hierarchy Key: " + obj.P_EPAL_HIERARCHY_KEY);
                return Json(new UpdateDto() { StatusID = -1, Message = "Record submitted is from other source." }, new JsonSerializerSettings());
            }
            obj.P_PROC_CD = _helper.DecodeBase64AndUri(obj.P_PROC_CD);
            obj.P_EPAL_PRODUCT_CD = _helper.DecodeBase64AndUri(obj.P_EPAL_PRODUCT_CD);
            obj.P_USER_ID = _helper.MS_ID;
            obj.P_FURTHER_INST = _helper.DecodeBase64AndUri(obj.P_FURTHER_INST.ToStringNullSafe());
            obj.P_NOTES = _helper.DecodeBase64AndUri(obj.P_NOTES.ToStringNullSafe());
            obj.P_CHANGE_REQ_ID = _helper.DecodeBase64AndUri(obj.P_CHANGE_REQ_ID.ToStringNullSafe());
            obj.P_CHANGE_DESC = _helper.DecodeBase64AndUri(obj.P_CHANGE_DESC.ToStringNullSafe());

            obj.P_ATS_EFF_DT = _helper.DecodeBase64AndUri(obj.P_ATS_EFF_DT.ToStringNullSafe());
            obj.P_ATS_EXP_DT = _helper.DecodeBase64AndUri(obj.P_ATS_EXP_DT.ToStringNullSafe());
            obj.P_FACTOR_EFF_DT = _helper.DecodeBase64AndUri(obj.P_FACTOR_EFF_DT.ToStringNullSafe());
            obj.P_FACTOR_EXP_DT = _helper.DecodeBase64AndUri(obj.P_FACTOR_EXP_DT.ToStringNullSafe());

            var data = await _service.EPAL_INS_UPD_DRIVER(obj);

            // Log user activity
            await _userAccessHistService.Add("EPAL", "EPAL/Home/EPAL_INS_UPD_DRIVER?epalPageView=" + obj.EPALPageView, JsonConvert.SerializeObject(obj), 7);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInsertHistoric(EPAL_Ins_Upd_Pkg_Param obj)
        {
            obj.P_USER_ID = _helper.MS_ID;
            obj.P_PROC_CD = _helper.DecodeBase64AndUri(obj.P_PROC_CD);
            obj.P_EPAL_PRODUCT_CD = _helper.DecodeBase64AndUri(obj.P_EPAL_PRODUCT_CD);
            obj.P_FURTHER_INST = _helper.DecodeBase64AndUri(obj.P_FURTHER_INST.ToStringNullSafe());
            obj.P_NOTES =   _helper.DecodeBase64AndUri(obj.P_NOTES.ToStringNullSafe());
            obj.P_CHANGE_REQ_ID = _helper.DecodeBase64AndUri(obj.P_CHANGE_REQ_ID.ToStringNullSafe());
            obj.P_CHANGE_DESC = _helper.DecodeBase64AndUri(obj.P_CHANGE_DESC.ToStringNullSafe());

            var data = await _service.EPAL_HISTORIC_INS_UPD_DRIVER(obj);

            // Log user activity
            await _userAccessHistService.Add("EPAL", "EPAL/Home/EPAL_HISTORIC_INS_UPD_DRIVER?epalPageView=" + obj.EPALPageView, JsonConvert.SerializeObject(obj), 7);

            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

        //Here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetEPALProceduresModifiers([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            var retVal = await _service.GetEPALProceduresModifiers(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(retVal.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDiagCodes([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _service.GetEPALProcedureDGCodesByPIMS_ID(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }


        public async Task<ActionResult> GetDiagnosisCurrVerVList([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _refDiagnosesService.GetPIMS_Diagnosis_Curr_Ver_V_List(param);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDiagnosisVList([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _refDiagnosesService.GetPIMS_Diagnosis_V_List(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetRevCodes([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _service.GetEPALProcedureRevCodesByPIMS_ID(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetAllocatedPlaces([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _service.GetEPALProcedureAllowedPlaceByPIMS_ID(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ChangeHistory([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            //param.p_EPAL_HIERARCHY_KEY = "EnI-ALS-COM-ALL-ALL-J0741-CABENUVA";
            var data = await _service.GetEPALProcedureChangeHistoryByPIMS_ID(_helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param));
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> StateInfo([DataSourceRequest] DataSourceRequest request, EPAL_Procedures_Param_Dto param)
        {
            param = _helper.DecodeSanitizedFields<EPAL_Procedures_Param_Dto>(param);
            IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto> data = null;

            if (param.EPALPageView.ToStringNullSafe().ToLower() == EPALPageView.EditDetail.ToStringNullSafe().ToLower() || param.EPALPageView.ToStringNullSafe().ToLower() == EPALPageView.ViewDetail.ToStringNullSafe().ToLower())
            {
                data = await _service.GetEPALProcedureAPPlTOSTATESByPIMS_ID(param);

                if (data.Count() == 0)
                {
                    data = await SetStateIfEmpty(param.p_EPAL_ENTITY_CD);
                }
            }
            else if (param.EPALPageView.ToStringNullSafe().ToLower() == EPALPageView.DuplicateRecordDetail.ToStringNullSafe().ToLower() && param.p_EPAL_HIERARCHY_KEY != null)
            {
                if (_cnSIFPLogic.IsLOBCnSIFP(param.p_EPAL_BUS_SEG_CD))
                {
                    List<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto> stateList = new List<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>();

                    var state = await SetStateIfEmpty(param.p_EPAL_ENTITY_CD);
                    if (state != null) stateList.AddRange(state);

                    if (stateList.Count() == 1)
                    {
                        if (state != null) data = stateList;
                    }
                }
                else
                {
                    data = await _service.GetEPALProcedureAPPlTOSTATESByPIMS_ID(param);
                }
            }
            else if (param.EPALPageView.ToStringNullSafe().ToLower() == EPALPageView.AddDetail.ToStringNullSafe().ToLower())
            {
                // Check if param.p_EPAL_ENTITY_CD is a VALID state from _repo.GetStateCDs()
                data = await SetStateIfEmpty(param.p_EPAL_ENTITY_CD);
            }

            if (data == null)
            {
                return new StatusCodeResult(200);
            }

            return Json(data.ToDataSourceResult(request));
        }

        private async Task<IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>> SetStateIfEmpty(string p_EPAL_ENTITY_CD)
        {
            IEnumerable<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto> data = null;
            foreach (var state in await _pimsValidValuesService.GetStateCDs())
            {
                if (state.STATE_CD == p_EPAL_ENTITY_CD)
                {

                    data = new List<EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto>()
                        {
                            new EPAL_Procedures_AssoCodes_V_APPL_TO_STATES_Dto()
                            {
                                STATE_CD = p_EPAL_ENTITY_CD,
                                STATE_NAME = state.DESC,
                                INCL_EXCL_CD = "INCL",
                                INCL_EXCL_CD_DESC = "Include"
                            }
                        };
                    break;

                }
            }

            return data;
        }

        public async Task<ActionResult> GetRefModifiersByFilter(string text)
        {
            var data = await _refModifiersService.GetRefModifiersByFilter(text);
            return Json(data);
        }

        public async Task<ActionResult> GetPIMS_IDExistStatus(EPAL_Procedures_Param_Dto param)
        {
            var retVal = await _service.GetPIMS_IDExistStatus(param);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<JsonResult> GetRefDiagnosesByFilter(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Json("");
            }

            var retVal = await _refDiagnosesService.GetRefDiagnosesByFilter(text);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<JsonResult> GetRefRevenuesByFilter(string text)
        {
            //if (string.IsNullOrEmpty(text))
            //{
            //    return Json("");
            //}

            var retVal = await _refRevenuesService.GetRefRevenuesByFilter(text);
            return Json(retVal);
        }

        [HttpGet]
        public async Task<JsonResult> GetRefProceduresByFilter(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Json("");
            }

            var retVal = await _refProceduresService.GetRefProceduresByFilter(text);
            return Json(retVal);
        }

        public async Task<IEnumerable<string>> getValidStates()
        {
            var state_cd_dto = await _pimsValidValuesService.GetStateCDs();
            var states = state_cd_dto.Select(s => s.STATE_CD).ToList();
            return states;
        }

        [HttpGet]
        public async Task<ActionResult> GetDelegateUms()
        {
            var retVal = await _pimsValidValuesService.GetDelegateUms();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetProgMgdBy()
        {
            var retVal = await _pimsValidValuesService.GetProgMgdBy();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetDXInds()
        {
            var retVal = await _pimsValidValuesService.GetDXInds();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetStAppInds()
        {
            var retVal = await _pimsValidValuesService.GetStAppInds();
            return Json(retVal);
        }

        [HttpGet]
        public async Task<ActionResult> GetIntExtCDs()
        {
            var retVal = await _pimsValidValuesService.GetIntExtCDs();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<JsonResult> GetPIMSEPALCategoriesByType(string text, string P_CATEGORY_TYPE, string P_PARENT_CATEGORY)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Json("");
            }

            var retVal = await _EPALProceduresService.GetPIMSEPALCategoriesByType(new EPAL_Catagory_By_Type_Param_Dto { P_TEXT = text, P_CATEGORY_TYPE = P_CATEGORY_TYPE, P_PARENT_CATEGORY = P_PARENT_CATEGORY });
            return Json(retVal);
        }
        [HttpGet]
        public async Task<JsonResult> GetPIMSEPALCategoriesByProcCDDrugNM(string P_DRUG_NM, string P_PROC_CD, string P_ALTERNATE_CATEGORY)
        {
            var retVal = await _EPALProceduresService.GetPIMSEPALCategoriesByProcCDDrugNM(new EPAL_Catagory_By_ProcCDDrugNM_Param_Dto { P_DRUG_NM = P_DRUG_NM, P_PROC_CD = P_PROC_CD, P_ALTERNATE_CATEGORY = P_ALTERNATE_CATEGORY });
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetRetentionFactors()
        {
            var retVal = await _pimsValidValuesService.GetRetentionFactors();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetReductionFactors()
        {
            var retVal = await _pimsValidValuesService.GetReductionFactors();
            return Json(retVal);
        }
        [HttpGet]
        public async Task<ActionResult> GetStateIssueGov()
        {
            var retVal = await _pimsValidValuesService.GetStateIssueGov();
            return Json(retVal);
        }
        #endregion

        public static EPALDrivingSourceRangeDto GetDrivingStatusDateRangeIncludingToday(List<EPALDrivingSourceRangeDto> dateRanges, DateTime today)
        {
            EPALDrivingSourceRangeDto activeDrivingSourceDatePair = null;

            foreach (var range in dateRanges)
            {
                if (range.StartDate <= today && range.EndDate >= today)
                {
                    activeDrivingSourceDatePair = range;
                    break;
                }
            }

            dateRanges.ForEach(e => e.EndDate = (e.EndDate == new DateTime(0) ? DateTime.Parse("1/1/2999") : e.EndDate));

            dateRanges = dateRanges.OrderBy(x => x.EndDate).ThenBy(z => z.StartDate).ToList();

            DateTime latestEndDate = new DateTime(0);

            // If there's not any date pair that has today's date, then get the max of these dates
            //if (activeDrivingSourceDatePair == null)
            //{
            foreach (var range in dateRanges)
            {
                if(range.EndDate.ToString() == "1/1/0001 12:00:00 AM")
                {
                    range.EndDate = Convert.ToDateTime("12/31/2999");
                }

                if (range.EndDate > latestEndDate)
                {
                    latestEndDate = range.EndDate;
                    //activeDrivingSourceDatePair = range;

                    if (activeDrivingSourceDatePair == null)
                    {
                        activeDrivingSourceDatePair = range;
                    }
                    else
                    {
                        if(range.StartDate > DateTime.Today)
                            activeDrivingSourceDatePair.Future = range;
                        else
                            activeDrivingSourceDatePair = range;
                    }
                }
            }
            //}
            if( activeDrivingSourceDatePair != null)
            {
                if(activeDrivingSourceDatePair.Future != null)
                {
                    activeDrivingSourceDatePair.DisplayName = activeDrivingSourceDatePair.Name == activeDrivingSourceDatePair.Future.Name ? activeDrivingSourceDatePair.Name : activeDrivingSourceDatePair?.Name + " -> "+ activeDrivingSourceDatePair?.Future?.Name;
                }else
                {
                    activeDrivingSourceDatePair.DisplayName = activeDrivingSourceDatePair.Name;
                }
            }

            return activeDrivingSourceDatePair;
        }
    }
}
