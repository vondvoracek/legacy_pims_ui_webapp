var isDirty = false
var current_title = $(document).attr('title');
var viewDetail = current_title.includes("View Detail");
var addDetail = current_title.includes("Add Detail");
var editDetail = current_title.includes("Edit Detail");
var duplicateDetail = current_title.includes("Duplicate Record Detail");
var lblCodeInactive = $("#lblCodeInactive").html();

var _DetailHeaderController = {

    init: function () {
        _DetailHeaderController.bind();
    },
    bind: function () {
        $("#chkDefault").click(function () {
            if ($('#chkDefault').is(':checked')) {
                MIApp.DPOC.disableDrugName();
                var pims_id = MIApp.DPOC.create_pims_id();
                var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtDPOCID").val(pims_id_display);
            }
            else {
                MIApp.DPOC.enableDrugName();
                var pims_id = MIApp.DPOC.create_pims_id();
                var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtDPOCID").val(pims_id_display);
            }
        });  

        $("#txtDrugName").on('change keyup', function () {
            var pims_id = MIApp.DPOC.create_pims_id();
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);
        });

        $("#btnGotoDetail").click(function () {
            var pims_id = MIApp.Sanitize.string($("#txtPIMSID").val()); // fix MFQ 7/17/2023
            var DPOCVersionDt = $("#txtDPOCVersionDt").val();
            if (duplicateDetail) {
                var DPOCPackage = $("#txtDPOCPackageOld").val();
                //var DPOCRelease = $("#txtDPOCReleaseOld").val();
            }
            else {
                var DPOCPackage = $("#ddlDPOCPackage").val();
                //var DPOCRelease = $("#txtDPOCRelease").val();
            }
            pims_id = pims_id + ',' + kendo.toString(kendo.parseDate(DPOCVersionDt), 'MM-dd-yyyy hh:mm:ss tt') + ',' + DPOCPackage;
            DetailController.redirect.redirectToDpocDetail(pims_id, 'viewdetail');
        });
    },

    dropdown: {
        refreshDropDownList: function () {
            var ddlBusinessSegment = $("#ddlBusinessSegment").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan").data('kendoDropDownList');
            var ddlFundingArrangement = $("#ddlFundingArrangement").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity").data('kendoDropDownList');
            var ddlPackage = $("#ddlDPOCPackage").data('kendoDropDownList');

            ddlBusinessSegment.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlFundingArrangement.dataSource.read();
            ddlEntity.dataSource.read();
            ddlPackage.dataSource.read();
        },
        param: {
            APP_PRODUCT: function () {
                return {
                    p_VV_SET_NAME: "PA_PROC_PRODUCT_CD",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            APP_LOB: function () {
                return {
                    p_VV_SET_NAME: "BUS_SEG_CD",
                    p_BUS_SEG_CD: ""
                }
            },
            APP_PLAN: function () {
                return {
                    p_VV_SET_NAME: "PLAN_CD",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            APP_FUNDING_ARRANGEMENT: function () {
                return {
                    p_VV_SET_NAME: "PA_PROC_FUND_ARNGMNT_CD",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            APP_ENTITY: function () {
                return {
                    p_VV_SET_NAME: "ENTITY_CD",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            SVC_CAT: function () {
                return {
                    p_VV_SET_NAME: "SVC_CAT",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            SVC_SUBCAT: function () {
                return {
                    p_VV_SET_NAME: "SVC_SUBCAT",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            PROG_MGD_BY: function () {
                return {
                    p_VV_SET_NAME: "PROG_MGD_BY",
                    p_BUS_SEG_CD: "ALL"
                }
            },
            DRUG_RVW_AT_LAUNCH_IND: function () {
                return {
                    p_VV_SET_NAME: "DRUG_RVW_AT_LAUNCH_IND",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            SOS_IND: function () {
                return {
                    p_VV_SET_NAME: "SOS_IND",
                    p_BUS_SEG_CD: "ALL"
                }
            },
            SOS_URG_CAT_MDLTY: function () {
                return {
                    p_VV_SET_NAME: "SOS_URG_CAT_MDLTY",
                    p_BUS_SEG_CD: "ALL"
                }
            },
            SOS_TYPE: function () {
                return {
                    p_VV_SET_NAME: "SOS_TYPE",
                    p_BUS_SEG_CD: "ALL"
                }
            },
            SOS_SITE_IND: function () {
                return {
                    p_VV_SET_NAME: "SOS_SITE_IND",
                    p_BUS_SEG_CD: "ALL"
                }
            },
            DELEGATED_UM: function () {
                return {
                    p_VV_SET_NAME: "DELEGATED_UM",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            AUTO_APRVL_IND: function () {
                return {
                    p_VV_SET_NAME: "AUTO_APRVL_IND",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            MCARE_SPCL_PRCSNG_IND: function () {
                return {
                    p_VV_SET_NAME: "MCARE_SPCL_PRCSNG_IND",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            MCARE_SPCL_PRCSNG_TYPE: function () {
                return {
                    p_VV_SET_NAME: "MCARE_SPCL_PRCSNG_TYPE",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            LVL_CARE_RVW_RQD_IND: function () {
                return {
                    p_VV_SET_NAME: "LVL_CARE_RVW_RQD_IND",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            MDCL_NCSSTY_IND: function () {
                return {
                    p_VV_SET_NAME: "MDCL_NCSSTY_IND",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            ALTRNT_SVC_CAT: function () {
                return {
                    P_DRUG_NM: _DetailHeaderController.dropdown.param.getDrugName(),
                    P_PROC_CD: $('#txtProcedureCode').val(),
                    P_ALTERNATE_CATEGORY: null
                };
            },
            ALTRNT_SVC_SUBCAT: function () {
                return {
                    P_DRUG_NM: _DetailHeaderController.dropdown.param.getDrugName(),
                    P_PROC_CD: $('#txtProcedureCode').val(),
                    P_ALTERNATE_CATEGORY: $('#ddlAlternateCategory').val()
                };
            },
            getDrugName: function () {
                var drug_no = $('#txtDrugName').val();
                if (drug_no.indexOf('~') > -1) {
                    drug_no = '~' + drug_no.split('~').slice(1);
                }
                return drug_no;
            },
            All_APP_BUSINESS_SEGMENT: function () {
                var param = _DetailHeaderController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";
                return param;
            },
            All_APP_ENTITY: function () {
                var param = _DetailHeaderController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_ENTITY_CD";
                return param;
            },
            All_APP_PLAN: function () {
                var param = _DetailHeaderController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PLAN_CD";
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = _DetailHeaderController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PRODUCT_CD";
                return param;
            },
            All_APP_FUNDING_ARRANGEMENT: function () {
                var param = _DetailHeaderController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_FUND_ARNGMNT_CD";
                return param;
            },
            All: function () {
                return {
                    EPAL_BUS_SEG_CD: $("#ddlBusinessSegment").val(),
                    EPAL_ENTITY_CD: $("#ddlEntity").val(),
                    EPAL_PLAN_CD: $("#ddlPlan").val(),
                    EPAL_PRODUCT_CD: $("#ddlProduct").val(),
                    EPAL_FUND_ARNGMNT_CD: $("#ddlFundingArrangement").val()
                }
            },
            DPOC_PACKAGE: function () {
                return {
                    p_VV_SET_NAME: "DPOC_PACKAGE",
                    p_BUS_SEG_CD: $("#ddlBusinessSegment").val()
                }
            }
        },
        onChange_BusinessSegment: function (e) {
            var pims_id = MIApp.DPOC.create_pims_id();
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);

            _DetailHeaderController.dropdown.refreshDropDownList();
        },
        onChange_Entity: function (e) {
            var pims_id = MIApp.DPOC.create_pims_id()
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);

            _DetailHeaderController.dropdown.refreshDropDownList();
        },
        onChange_Product: function () {
            var pims_id = MIApp.DPOC.create_pims_id()
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);

            _DetailHeaderController.dropdown.refreshDropDownList();
        },
        onChange_Plan: function () {
            var pims_id = MIApp.DPOC.create_pims_id();
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id);
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);

            _DetailHeaderController.dropdown.refreshDropDownList();
        },
        onChange_FundingArrangement: function () {
            var pims_id = MIApp.DPOC.create_pims_id();
            var pims_id_display = _DetailHeaderController.methods.pims_id_drugname_parse(pims_id);
            $("#txtPIMSID").val(pims_id);
            $("#txtDPOCID").val(pims_id_display);

            _DetailHeaderController.dropdown.refreshDropDownList();
        },
        onOpen_One: function (e) {
            var value = $("#" + this.element.attr("id")).val();
            if ($("#" + this.element.attr("id")).data("kendoDropDownList")) {
                $("#" + this.element.attr("id")).data("kendoDropDownList").value(-1);
                $("#" + this.element.attr("id")).data('kendoDropDownList').dataSource.read();
                if (value != "") {
                    $("#" + this.element.attr("id")).data("kendoDropDownList").value(value);
                }
            }
        }        
    },
    methods: {
        pims_id_drugname_parse: function (pims_id) {
            if (pims_id.indexOf(" ") > 0) {
                return pims_id = pims_id.substring(0, pims_id.indexOf(' '));
            }

            else {
                return pims_id;
            }
        },
        checkValidNewPIMS_ID: function () {
            var existStatus = "";
            $.ajax({
                type: "GET",
                url: VIRTUAL_DIRECTORY + '/DPOC/Home/GetDPOCIDExistStatus?dpoc_hierarchy_key=' +
                    ($("#txtDPOCID") == null ? ("#txtPIMSID").val() : $("#txtDPOCID").val()) + 
                    '&dpoc_package=' + $('#ddlDPOCPackage').val(),
                async: false,
                headers: {
                    'Accept': 'application/json'
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (result) {
                    existStatus = result;
                },
                error: function (e) {
                    _DetailHeaderController.message.showError(e.statusText);
                }
            })
            return existStatus
        },
        checkPIMSHierarchyCodeCombinationExists: function () {
            var EPAL_BUS_SEG_CD = "";
            var EPAL_ENTITY_CD = "";
            var EPAL_PLAN_CD = "";
            var EPAL_PRODUCT_CD = "";
            var EPAL_FUND_ARNGMNT_CD = "";
            var isExists = true;

            if ($("#duplicate-record").val() == "Y") {
                /*==================================
                    Get values from _DetailHeader_DuplicateRecord.cshtml if Duplicating a record. 
                 ==================================*/
                EPAL_BUS_SEG_CD = $("#ddlBusinessSegment").val();
                EPAL_ENTITY_CD = $("#ddlEntity").val();
                EPAL_PLAN_CD = $("#ddlPlan").val();
                EPAL_PRODUCT_CD = $("#ddlProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
            }

            else {
                EPAL_BUS_SEG_CD = $("#txtBusinessSegment").val();
                EPAL_ENTITY_CD = $("#txtEntity").val();
                EPAL_PLAN_CD = $("#txtPlan").val();
                EPAL_PRODUCT_CD = $("#txtProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#txtFunding").val();
            }

            var obj = {
                "EPAL_BUS_SEG_CD": EPAL_BUS_SEG_CD,
                "EPAL_ENTITY_CD": EPAL_ENTITY_CD,
                "EPAL_PLAN_CD": EPAL_PLAN_CD,
                "EPAL_PRODUCT_CD": EPAL_PRODUCT_CD,
                "EPAL_FUND_ARNGMNT_CD": EPAL_FUND_ARNGMNT_CD
            }

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/EPAL/Home/CheckPIMSHierarchyCodeCombinationExists",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json'
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (result) {
                    if (result.IS_EXIST == "1") {
                        isExists = true
                    }
                    else {
                        isExists = false
                    }

                },
                error: function () {
                    _DetailHeaderController.message.showError();
                }
            });

            return isExists;
        },
        validateDropdowns: function () {
            var isValid = true;
            var errorMessage = "";

            if ($("#ddlBusinessSegment").val() == 0) {
                errorMessage += "Please select an LOB!\n";
                isValid = false;
            }

            if ($("#ddlEntity").val() == 0) {
                errorMessage += "Please select an Entity!\n";
                isValid = false;
            }

            if ($("#ddlPlan").val() == 0) {
                errorMessage += "Please select a Plan!\n";
                isValid = false;
            }

            if ($("#ddlProduct").val() == 0) {
                errorMessage += "Please select a Product!\n";
                isValid = false;
            }

            if ($("#ddlFundingArrangement").val() == 0) {
                errorMessage += "Please select a Funding Arrangement!\n";
                isValid = false;
            }

            if ($("#txtDrugName").val() == '' && !$("#txtDrugName").is('[readonly]')) {
                errorMessage += "Please enter a Drug Name!\n";
                isValid = false;
            }

            if ($("#txtDPOCPackage").val() == '') {
                errorMessage += "Please select a Package!\n";
                isValid = false;
            }

            var existStatus = _DetailHeaderController.methods.checkValidNewPIMS_ID();
            if (existStatus == "Exists") {
                errorMessage += "DPOC ID already exists! Please enter an another DPOC ID.\n";
                isValid = false;
            }

            // Check valid hierarchy code combination
            PIMSHierarchyCodesExist = _DetailHeaderController.methods.checkPIMSHierarchyCodeCombinationExists();
            if (PIMSHierarchyCodesExist == false) {
                errorMessage += "Invalid PIMS ID hierarchy code combination. Please select another value!\n";
                isValidNewPIMS_ID = false;
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            return result;
        }
    },
    validation: {
        validate_PASummary: function () {
            if (DiagListController.grid.check_diagList_prgmgby() == false) {
                errorMessage += "Diagnosis list requires a program managed by!\n";
                isValid = false;
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            return result;
        },        
        validate_ChgHist: function () {
            var isValid = true;
            var errorMessage = "";
            var text = "";

            text = $("#txtChangeSource").val() ?? "";
            if (text.trim() == "") {
                errorMessage = "Please input Change Source (Change History tab)!\n";
                isValid = false;
            }
            else {
                var regex = new RegExp("®|\'");
                if ((regex.test(text))) {
                    errorMessage += "Change Source can not have @ or \' (Change History tab)!\n";
                    isValid = false;
                }
            }

            text2 = $("#txtChangeDesc").val() ?? "";
            if (text2.trim() == "") {
                errorMessage += "Please input Change Description (Change History tab)!\n";
                isValid = false;
            }
            else {
                var regex = new RegExp("®|\'");
                if ((regex.test(text2))) {
                    errorMessage += "Change Description can not have @ or \' (Change History tab)!\n";
                    isValid = false;
                }
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            //if (result.isValid == false) {
            //    _DetailHeaderController.methods.focusReviewIndicatorTab();
            //}

            return result;
        }
    },
    message: {
        showSuccess: function () {
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Something Success!',
                allowOutsideClick: false,
                customClass: 'swal-size-sm'
            })
        },
        showWarning: function (message) {
            Swal.fire({
                html: message,
                icon: 'warning',
                title: 'Invalid Entry',
                allowOutsideClick: false,
                //text: message,
                customClass: 'swal-size-sm',
                width: '750px'
            })
        },
        showError: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                allowOutsideClick: false,
                text: 'Opps, an error occurred! Please contact the system administrators.',
                customClass: 'swal-size-sm'
            })
        },
        showReset: function () {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to reset?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Resetting changes..',
                        text: 'Reloading record, please wait..',
                        timerProgressBar: true,
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();
                            location.reload();
                        }
                    })
                }
            }

            else if (!isDirty) {
                Swal.fire({
                    title: 'Resetting changes..',
                    text: 'Reloading record, please wait..',
                    timerProgressBar: true,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        location.reload();
                    }
                })
            }
        },

    }
};

$(document).ready(function () {
    _DetailHeaderController.init();
});
