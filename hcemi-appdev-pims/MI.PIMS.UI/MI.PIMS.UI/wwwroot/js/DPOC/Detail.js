var form_original_data;
var form_new_data;
var isDirty = false
var current_title = $(document).attr('title');
var viewDetail = current_title.includes("View Detail");
var addDetail = current_title.includes("Add Detail");
var editDetail = current_title.includes("Edit Detail");
var duplicateDetail = current_title.includes("Duplicate Record Detail");

var DetailController = {
    init: function () {
        DetailController.bindEvents();
    },
    bindEvents: function () {
        // Prevent default for elements with class 'noSubmit'
        $('.noSubmit').on('click', e => e.preventDefault());

        // Enable tooltips
        $('[data-toggle="tooltip"]').tooltip();

        // Delete version popup
        $('#deleteVersion').on('click', e => {
            e.preventDefault();
            DeleteVersion.kendoWindow.open();
        });

        // Common handler for redirect buttons
        const handleRedirect = (action) => {
            const pimsId = $('#txtDPOCID').val();
            const versionDate = $('#txtDPOCVersionDt').val();
            const packageVal = $('#ddlDPOCPackage').val();
            const formattedDate = kendo.toString(kendo.parseDate(versionDate), 'MM-dd-yyyy hh:mm:ss tt');
            const param = `${pimsId},${formattedDate},${packageVal}`;
            DetailController.redirect.redirectToDpocDetail(param, action);
        };

        $('#btnDuplicateCopy').on('click', () => handleRedirect('duplicaterecorddetail'));
        $('#btnViewOnly').on('click', () => handleRedirect('viewdetail'));
        $('#btnGoToEdit').on('click', () => handleRedirect('editdetail'));

        // Save button with validation
        $('.btnSave').on('click', e => {
            const validation = DetailController.methods.validation();
            if (validation.isValid) {
                $(window).off('beforeunload');
                MICore.Notification.whileSaving('Saving changes', () => {
                    setTimeout(() => {
                        DetailController.methods.upSert();
                    }, 1000);
                });
            } else {
                const formattedMessage = `<div style='text-align: left;'><ul style='list-style-type: square;'><li>${validation.errorMessage.trim().replace(/\n/g, '</li><li>')}</li></ul></div>`;
                DetailController.message.showWarning(formattedMessage);
                $(`a[href="${validation.activeTabNameAfterValidation}"]`).click();
                e.preventDefault();
            }
        });

        // Reset button
        $('#btnReset').on('click', () => {
            DetailController.message.showReset();
        });

        // Disable Save and Reset buttons initially
        DetailController.dirtyform.toggleButtons(false);

        // Dirty form check on change
        const dirtyCheckForms = ['#editForm', '#addForm', '#duplicateForm', '#grid_guideline_summary_active'];
        dirtyCheckForms.forEach(selector => {
            $(selector).on('change', () => {
                DetailController.dirtyform.dirtycheck();
            });
        });
    },
    dropdown: {
        param: {
            DPOC_ELIGIBLE_IND: function () {
                return {
                    p_VV_SET_NAME: "DPOC_ELIGIBLE_IND",
                    p_BUS_SEG_CD: ""
                }
            },
            DPOC_IMPLEMENTED_IND: function () {
                return {
                    p_VV_SET_NAME: "DPOC_IMPLEMENTED_IND",
                    p_BUS_SEG_CD: ""
                }
            },
            KL_PLCY_NM: function () {

                if (duplicateDetail) {
                    DPOC_BUS_SEG_CD = $("#ddlBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#ddlEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }
                else {
                    DPOC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#txtEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }

                return {
                    p_dpoc_bus_seg_cd: DPOC_BUS_SEG_CD,
                    p_dpoc_entity_cd: DPOC_ENTITY_CD,
                    p_proc_cd: PROC_CD,
                    p_plcy_type_cd: "p_plcy_type_cd"
                }
            },
            DPOC_PACKAGE: function () {
                return {
                    p_column_name: "DPOC_PACKAGE",
                }
            },
            DPOC_PACKAGE_2: function () {
                return {
                    p_VV_SET_NAME: "DPOC_PACKAGE",
                    p_BUS_SEG_CD: $("#txtBusinessSegment").val()
                }
            },
            DPOC_INELIGIBLE_RSN: function () {
                return {
                    p_VV_SET_NAME: "DPOC_INELIGIBLE_RSN",
                    p_BUS_SEG_CD: ""
                }
            },
            DPOC_UNIMPLEMENTED_RSN: function () {
                return {
                    p_VV_SET_NAME: "DPOC_UNIMPLEMENTED_RSN",
                    p_BUS_SEG_CD: ""
                }
            }
        },
        onChangeddlDPOCEligible: function () {
            var $ddlDpocIneligibleRsn = $('#ddlDpocIneligibleRsn').data('kendoDropDownList');
            var $ddlDpocImplemented = $('#ddlDPOCImplemented').data('kendoDropDownList');
            var eligibleValue = $('#ddlDPOCEligible').val();

            if (eligibleValue === 'No') {
                $('.lbl-ddlDpocIneligibleRsn').addClass('required');
                $ddlDpocIneligibleRsn.enable(true);

                // Disable and default DPOC Implemented to "No"
                $ddlDpocImplemented.enable(false);
                $ddlDpocImplemented.value("No"); // <-- Set default value
                $('#lblDPOCImplemented').removeClass('required');

                // User Story 132082 MFQ 5/7/2025
                //$('#lblDPOCStartDate').removeClass('required'); //User Story 137701 MFQ 8/28/2025
            } else {
                $('.lbl-ddlDpocIneligibleRsn').removeClass('required');
                $ddlDpocIneligibleRsn.enable(false);
                $ddlDpocIneligibleRsn.value("");

                // Enable DPOC Implemented for user input
                $ddlDpocImplemented.enable(true);
                $('#lblDPOCImplemented').addClass('required');
                //$('#lblDPOCStartDate').addClass('required'); //User Story 137701 MFQ 8/28/2025
            }
        },
        onChangeddlDPOCImplemented: function () {

            const dpocImplemented = $('#ddlDPOCImplemented').val();
            const notImplementedReason = $('#ddlDPOCNotImplementedReason').val()?.toLowerCase();
            const dpocStartDate = $('#dpDPOCStartDate').val();

            const startDatePicker = $('#dpDPOCStartDate').data("kendoDatePicker");
            const termDatePicker = $('#dpDPOCTermDate').data("kendoDatePicker");
            const notImplementedDropdown = $('#ddlDPOCNotImplementedReason').data('kendoDropDownList');

            if (dpocImplemented === 'No' || dpocImplemented == 'Partial') { //User Story 140403 MFQ 11/18/2025
                //$('#lblDPOCStartDate').removeClass('required'); //User Story 137701 MFQ 8/28/2025
                notImplementedDropdown.enable(true);
                notImplementedDropdown.value('');

                //User Story 137701 MFQ 8/28/2025
                /*
                if (notImplementedReason === 'passthrough') {                    
                    startDatePicker.enable(true);
                    $('#lblDPOCStartDate').addClass('required');

                    if (dpocStartDate !== '') {
                        termDatePicker.enable(true);
                    } else {
                        termDatePicker.enable(false);
                    }
                } else {
                    startDatePicker.enable(false);
                    $('#dpDPOCStartDate').val('');
                    $('#lblDPOCStartDate').removeClass('required');
                    termDatePicker.enable(false);
                }

                $('#dpDPOCTermDate').val('');*/
            } else {
                notImplementedDropdown.enable(false);
                notImplementedDropdown.value('');
                /*
                $('#lblDPOCStartDate').addClass('required');
                startDatePicker.enable(true);

                if (dpocStartDate !== '') {
                    termDatePicker.enable(true);
                } else {
                    termDatePicker.enable(false);
                }*/
            }
        },
        onChangedpDPOCStartDate: function () {
            var dpocStartDateStr = $('#dpDPOCStartDate').val();
            var termDatePicker = $('#dpDPOCTermDate').data("kendoDatePicker");

            var enableTermDate = dpocStartDateStr !== '';

            // Enable or disable the Term Date picker
            termDatePicker.enable(enableTermDate);

            if (enableTermDate) {
                var dpocStartDate = kendo.parseDate(dpocStartDateStr);
                var dpocTermDateStr = $('#dpDPOCTermDate').val();
                var dpocTermDate = kendo.parseDate(dpocTermDateStr);

                // If Term Date is populated and less than Start Date, clear it
                if (dpocTermDate && dpocTermDate < dpocStartDate) {
                    $('#dpDPOCTermDate').val('');
                }

                // Optionally, set min date to enforce selection constraint
                termDatePicker.min(dpocStartDate);
            } else {
                // If disabled, clear the Term Date
                $('#dpDPOCTermDate').val('');
                termDatePicker.min(null); // Reset min constraint
            }
        }
,        
        onChangedddlDPOCNotImplementedReason: function () {
            //User Story 137701 MFQ 8/28/2025
            /*
            const dpocImplemented = $('#ddlDPOCImplemented').val();
            const notImplementedReason = $('#ddlDPOCNotImplementedReason').val()?.toLowerCase();
            const dpocStartDate = $('#dpDPOCStartDate').val();
            const startDatePicker = $('#dpDPOCStartDate').data("kendoDatePicker");
            const termDatePicker = $('#dpDPOCTermDate').data("kendoDatePicker");

            // Handle Start Date field
            if (dpocImplemented !== 'No' || notImplementedReason === 'passthrough') {
                startDatePicker.enable(true);
                $('#lblDPOCStartDate').addClass('required');
            } else {
                startDatePicker.enable(false);
                $('#lblDPOCStartDate').removeClass('required');
                $('#dpDPOCStartDate').val('');
            }

            // Handle Term Date field
            if (
                (dpocImplemented !== 'No' && dpocStartDate !== '') ||
                (dpocImplemented === 'No' && notImplementedReason === 'passthrough' && dpocStartDate !== '')
            ) {
                termDatePicker.enable(true);
            } else {
                termDatePicker.enable(false);
                $('#dpDPOCTermDate').val('');
            }
            */
        }
    },
    multiSelect: {
        param: {
            DPOC_SOS_PROVIDER_TIN_EXCL: function () {
                return {
                    p_VV_SET_NAME: "DPOC_SOS_PROVIDER_TIN_EXCL",
                    p_BUS_SEG_CD: ""
                }
            }
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
        }
    },
    methods: {
        onAssociatedEPALRecord: function (epal_hierarchy_key, epal_ver_eff_dt) {
            var url = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + epal_hierarchy_key + ',' + kendo.toString(kendo.parseDate(epal_ver_eff_dt), 'MM-dd-yyyy hh:mm:ss tt');
            windowPopup('_blank', url);
        },
        validate_change_history: function () {
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

            return result;
        },
        validation: function () {
            var saveValidation = true;
            var validateError = "";
            var activeTabNameAfterValidation = "";

            if (duplicateDetail) { // <-- Run if duplicate screen    
                var validateResultDropdown = _DetailHeaderController.methods.validateDropdowns(); // <-- Run Duplicate PIMS Record Validation 
                saveValidation = validateResultDropdown.isValid;
                validateError += validateResultDropdown.errorMessage ?? "";

                // If duplicated record
                if (!saveValidation) {
                    DetailController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + validateError.replace(/\n$/g, "").replace(/\n/g, "</li><li>") + "</li></ul></div>");

                    return {
                        isValid: false,
                        errorMessage: validateError,
                        activeTabNameAfterValidation: '#DPOCSummary'
                    };
                }
            }

            //Validate DPOC Package
            if ($('#ddlDPOCPackage').val() == '') {
                validateError = "DPOC Package is required!\n";
                activeTabNameAfterValidation = "#DPOCSummary";
                saveValidation = false;
            }

            //Validate DPOC Dates
            if ($('#ddlDPOCEligible').val() == '') {
                validateError += "DPOC Eligible is required in DPOC Summary section!\n";
                activeTabNameAfterValidation = "#DPOCSummary";
                saveValidation = false;
            }

            if ($('.lbl-ddlDpocIneligibleRsn').hasClass('required')) {
                if ($('#ddlDpocIneligibleRsn').data("kendoDropDownList").value() == '') {
                    validateError += "DPOC Not Eligible Reason is required in DPOC Summary section!\n";
                    activeTabNameAfterValidation = "#DPOCSummary";
                    saveValidation = false;
                }
            }

            if ($('#ddlDPOCImplemented').val() == '' && $('#ddlDPOCEligible').val() != 'No') {
                validateError += "DPOC Implemented is required in DPOC Summary section!\n";
                activeTabNameAfterValidation = "#DPOCSummary";
                saveValidation = false;
            }

            var dpDPOCStartDate = $('#dpDPOCStartDate').val();
            var dpDPOCTermDate = $('#dpDPOCTermDate').val();
            var isDPOCStartDateValid = $("#dpDPOCStartDate").val() != "" && Date.parse($("#dpDPOCStartDate").val())
            var isDPOCTermDateValid = $("#dpDPOCTermDate").val() != "" && Date.parse($("#dpDPOCTermDate").val())

            if ($('#lblDPOCStartDate').hasClass('required')) {
                if (dpDPOCStartDate == '' || !Date.parse(dpDPOCStartDate)) {
                    validateError += "DPOC Start Date is required and should be valid in DPOC Summary section!\n";
                    activeTabNameAfterValidation = "#DPOCSummary";
                    saveValidation = false;
                }
            }

            if (isDPOCStartDateValid || isDPOCTermDateValid) {
                if (Date.parse(dpDPOCTermDate) <= Date.parse(dpDPOCStartDate)) {
                    validateError += "DPOC Start Date cannot be greater than DPOC Term Date (DPOC Summary tab)!\n";
                    activeTabNameAfterValidation = "#DPOCSummary";
                    saveValidation = false;
                }
            }

            var guidlineSummaryValidation = DetailController.grid.guideline_summary.validation();
            if ($.trim(guidlineSummaryValidation) != '' && activeTabNameAfterValidation == '') {
                validateError += guidlineSummaryValidation ?? "";
                activeTabNameAfterValidation = "#GuidelineSummary";
                saveValidation = false;
            }

            var guidelineConfigListValidation = DetailController.grid.config.validation(null); //'grid_configHidden'
            if ($.trim(guidelineConfigListValidation) != '' && activeTabNameAfterValidation == '') {
                validateError += guidelineConfigListValidation ?? "";
                activeTabNameAfterValidation = "#GuidelineSummary";
                saveValidation = false;
            }
            var guidelineDiagListValidation = DetailController.grid.diags.validation(null); //'grid_GSDiagListHidden'
            if ($.trim(guidelineDiagListValidation) != '' && activeTabNameAfterValidation == '') {
                validateError += guidelineDiagListValidation ?? "";
                activeTabNameAfterValidation = "#GuidelineSummary";
                saveValidation = false;
            }

            var guidelineStatesValidation = DetailController.grid.states.validation(null); //'grid_GSStateInfoHidden'
            if ($.trim(guidelineStatesValidation) != '' && activeTabNameAfterValidation == '') {
                validateError += guidelineStatesValidation ?? "";
                activeTabNameAfterValidation = "#GuidelineSummary";
                saveValidation = false;
            }

            // validation for Change History
            var validate_Change_history = DetailController.methods.validate_change_history();
            saveValidation = (saveValidation == true) ? validate_Change_history.isValid : false;
            validateError += validate_Change_history.errorMessage ?? "";
            if (!validate_Change_history.isValid && activeTabNameAfterValidation == '')
                activeTabNameAfterValidation = "#changeHistory";

            var result = {
                isValid: saveValidation,
                errorMessage: validateError,
                activeTabNameAfterValidation: activeTabNameAfterValidation
            };

            return result;
        },
        upSert: function () {

            var DPOC_BUS_SEG_CD = "";
            var DPOC_ENTITY_CD = "";
            var DPOC_PLAN_CD = "";
            var DPOC_PRODUCT_CD = "";
            var DPOC_FUND_ARNGMNT_CD = "";
            var PROC_CD = "";
            var DRUG_NM = "";
            var DPOC_PACKAGE = "";
            var DPOC_RELEASE = "";


            if (duplicateDetail) {
                /*==================================
                    Get values from _DetailHeader_DuplicateRecord.cshtml if Duplicating a record. 
                 ==================================*/
                DPOC_BUS_SEG_CD = $("#ddlBusinessSegment").val();
                DPOC_ENTITY_CD = $("#ddlEntity").val();
                DPOC_PLAN_CD = $("#ddlPlan").val();
                DPOC_PRODUCT_CD = $("#ddlProduct").val();
                DPOC_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
                DPOC_PACKAGE = $("#ddlDPOCPackage").val();
                //DPOC_RELEASE = $("#txtDPOCRelease").val();
            }
            else {
                DPOC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                DPOC_ENTITY_CD = $("#txtEntity").val();
                DPOC_PLAN_CD = $("#txtPlan").val();
                DPOC_PRODUCT_CD = $("#txtProduct").val();
                DPOC_FUND_ARNGMNT_CD = $("#txtFunding").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
                DPOC_PACKAGE = $("#ddlDPOCPackage").val();
                //DPOC_RELEASE = $("#txtDPOCRelease").val();
            }

            //Edit PIMS ID to not take any characters after space in drug name. 
            var DPOCID = MIApp.Sanitize.string($("#txtDPOCID").val());
            var DPOCVersionEffDt = MIApp.Sanitize.string(kendo.toString(kendo.parseDate($("#txtDPOCVersionDt").val()), 'MM-dd-yyyy hh:mm:ss tt'));
            var p_DPOC_PACKAGE = MIApp.Sanitize.string($("#ddlDPOCPackage").val());
            if (DPOCID.indexOf(" ") >= 0) {
                DPOCID = DPOCID.substring(0, DPOCID.indexOf(' '))
            }


            function getDelimitedValue(data, key, isDate = false, transformFn = null) {
                if (!(data && typeof data === 'object' && typeof data.length === 'number')) return null;

                const isEmpty = isDate
                    ? data.every(x => x[key] == null)
                    : data.every(x => !x[key]?.toString().trim());

                if (isEmpty && key != 'IQ_GDLN_NM') return null;

                return data.map(x => {
                    let value = x[key];                    
                    if (isDate) {
                        if (key == 'IQ_GDLN_REL_DT' && value == null) { // temp fix 8/5/2025
                            return kendo.toString(kendo.parseDate('1/1/1900'), 'yyyy-MM-dd');
                        }
                        return kendo.toString(kendo.parseDate(value), 'yyyy-MM-dd');
                    }
                    if (transformFn) {
                        return transformFn(value);
                    }
                    if (key == 'IQ_GDLN_NM') {// temp fix 8/5/2025
                        if (value == null || value == '') {
                            return x['IQ_GDLN_ID']
                        }
                    }
                    if (
                        key === 'JRSDCTN_DPOC_RELEASE' ||
                        key === 'JRSDCTN_DPOC_VER_NUM' ||
                        key === 'JRSDCTN_IQ_GDLN_ID' ||
                        key === 'JRSDCTN_NM'
                    ) {
                        var jurisdiction = x['JRSDCTN_NM'];

                        if (jurisdiction != null && jurisdiction !== '' && key !== 'JRSDCTN_NM') {
                            var items = jurisdiction.split(',');
                            var repeatedValues = [];

                            for (var i = 0; i < items.length; i++) {
                                repeatedValues.push(value);
                            }

                            value = repeatedValues.join(',');
                        } else {

                            // Always return value for JRSDCTN_IQ_GDLN_ID even if jurisdiction is null
                            if (key === 'JRSDCTN_IQ_GDLN_ID') {
                                return value;
                            }

                            if (value != null && value !== '') {
                                return value;
                            }
                        }
                    }
                    //AgeMin and AgeMax value update
                    if (key === 'GDLN_AGE_MIN') {
                        var gdln_age_min = normalizeGdlnAgeMin(x['GDLN_AGE_MIN'], x['GDLN_AGE_MAX']);
                        return gdln_age_min;
                    }
                    if (key === 'GDLN_AGE_MAX') {
                        var gdln_age_max = normalizeGdlnAgeMax(x['GDLN_AGE_MIN'], x['GDLN_AGE_MAX']);
                        return gdln_age_max;
                    }

                    return value;
                }).join(',');
            }

            _guideline_summary_data = DetailController.grid.guideline_summary.data;

            //debugger;

            // For config grid
            DetailController.grid.config.data = filterGridDataByGuidelineSummary(
                DetailController.grid.config.data,
                _guideline_summary_data,
                { guidelineIdField: "DTQ_IQ_GDLN_ID" }
            );

            // For diags grid
            DetailController.grid.diags.data = filterGridDataByGuidelineSummary(
                DetailController.grid.diags.data,
                _guideline_summary_data,
                { guidelineIdField: "DIAG_IQ_GDLN_ID" }

            );

            function safeEquals(a, b) {
                return String(a).trim().toUpperCase() === String(b).trim().toUpperCase();
            }

            function filterGridDataByGuidelineSummary(gridDataArray, guidelineSummaryArray, options) {
                var guidelineIdField = options.guidelineIdField || "DTQ_IQ_GDLN_ID"; // default for config

                return gridDataArray.filter(function (child) {
                    return guidelineSummaryArray.some(function (parent) {
                        return safeEquals(child[guidelineIdField], parent.IQ_GDLN_ID) &&
                            safeEquals(child.DPOC_VER_NUM, parent.DPOC_VER_NUM) &&
                            safeEquals(child.DPOC_RELEASE, parent.DPOC_RELEASE);

                    });
                });
            }

            //DetailController.message.showError();
            //return;

            var DPOC_Ins_Upd_Pkg_Param = {
                P_DPOC_BUS_SEG_CD: MIApp.Sanitize.encodeProp(DPOC_BUS_SEG_CD),
                P_DPOC_ENTITY_CD: MIApp.Sanitize.encodeProp(DPOC_ENTITY_CD),
                P_DPOC_PLAN_CD: MIApp.Sanitize.encodeProp(DPOC_PLAN_CD),
                P_DPOC_PRODUCT_CD: MIApp.Sanitize.encodeProp(DPOC_PRODUCT_CD),
                P_DPOC_FUND_ARNGMNT_CD: MIApp.Sanitize.encodeProp(DPOC_FUND_ARNGMNT_CD),
                P_PROC_CD: MIApp.Sanitize.encodeProp(PROC_CD),
                P_DRUG_NM: MIApp.Sanitize.encodeProp(DRUG_NM),
                P_DPOC_PACKAGE: MIApp.Sanitize.encodeProp(DPOC_PACKAGE),
                P_DPOC_EFF_DT: $('#dpDPOCStartDate').val(),
                P_DPOC_EXP_DT: $('#dpDPOCTermDate').val(),
                P_DPOC_ELIGIBLE_IND: MIApp.Sanitize.encodeProp($('#ddlDPOCEligible').val()),
                P_DPOC_INELIGIBLE_RSN: MIApp.Sanitize.encodeProp($('#ddlDpocIneligibleRsn').val()),
                P_DPOC_IMPLEMENTED_IND: MIApp.Sanitize.encodeProp($('#ddlDPOCImplemented').val()),
                
                P_ASSOC_EPAL_HIERARCHY_KEY: MIApp.Sanitize.encodeProp($('#EPAL_HIERARCHY_KEY').val()),
                P_ASSOC_EPAL_VER_EFF_DT: $('#EPAL_VER_EFF_DT').val(),
                P_DPOC_INV_NOTES: MIApp.Sanitize.encodeProp($('#txtNote').val()),

                P_DPOC_ADDTNL_RQRMNTS: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DPOC_ADDTNL_RQRMNTS')), //User Story 138385/138386 MFQ 9/23/2025

                P_DPOC_RELEASE: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'DPOC_RELEASE')),
                P_DPOC_VER_NUM: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'DPOC_VER_NUM')),

                P_IQ_GDLN_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_ID')),
                P_IQ_GDLN_VERSION: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_VERSION')),
                P_IQ_GDLN_NM: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_NM')),

                p_IQ_REFERENCE: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_REFERENCE')),
                P_IQ_GDLN_PRODUCT_NM: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_PRODUCT_NM')),
                P_IQ_GDLN_PRODUCT_DESC: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_PRODUCT_DESC')),
                P_IQ_GDLN_REL_DT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_REL_DT', true)),
                P_IQ_GDLN_EXP_DT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_EXP_DT', true)),

                //User Story 137657 MFQ 8/26/2025
                P_MDCR_COVG_SUM_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'MDCR_COVG_SUM_ID')),
                P_MDCR_COVG_SUM_TITLE: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'MDCR_COVG_SUM_TITLE')),

                P_IQ_GDLN_DESC: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_DESC')),
                P_IQ_GDLN_RECOMMENDATION_DESC: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_RECOMMENDATION_DESC')),
                P_IQ_GDLN_JRSDCTN: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_GDLN_JRSDCTN')),
                P_IQ_CRITERIA: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'IQ_CRITERIA')),

                P_RULE_OUTCOME_OUTPAT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_OUTPAT')),
                P_RULE_OUTCOME_OUTPAT_RSN: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_RSN_OUTPAT')),
                P_RULE_OUTCOME_OUTPAT_FCLTY: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_OUTPAT_FCLTY')),
                P_RULE_OUTCOME_OUTPAT_FCLTY_RSN: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_RSN_OUTPAT_FCLTY')),
                P_RULE_OUTCOME_INPAT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_INPAT')),
                P_RULE_OUTCOME_INPAT_RSN: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_TYPE_RSN_INPAT')),
                P_RULE_IMP_TYPE: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_IMP_TYPE')),
                P_RULE_IMP_WITH: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_IMP_WITH')),
                P_RULE_EXCLUSIONS: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'RULE_EXCLUSIONS')),
                P_GDLN_ASSOC_EFF_DT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'GDLN_ASSOC_EFF_DT', true)),
                P_GDLN_ASSOC_EXP_DT: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'GDLN_ASSOC_EXP_DT', true)),

                P_KL_PLCY_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'KL_PLCY_ID')),
                P_KL_PLCY_NAME: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'KL_PLCY_NAME')),

                P_GDLN_AGE_MIN: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'GDLN_AGE_MIN')),
                P_GDLN_AGE_MAX: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'GDLN_AGE_MAX')),

                //User Story 138385/138386 MFQ 9/23/2025
                P_DPOC_SOS_PROVIDER_TIN_EXCL: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DPOC_SOS_PROVIDER_TIN_EXCL')),
                P_PKG_CONFIG_COMMENTS: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'PKG_CONFIG_COMMENTS')),

                //Child records of guideline - DIAGNOSIS
                P_DIAG_DPOC_RELEASE: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'DPOC_RELEASE')),
                P_DIAG_DPOC_VER_NUM: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'DPOC_VER_NUM')),
                P_DIAG_IQ_GDLN_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'DIAG_IQ_GDLN_ID')),
                P_DIAG_CD: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'DIAG_CD')),
                P_DIAG_INCL_EXCL_CD: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'DIAG_INCL_EXCL_CD')),
                P_LIST_NAME: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.diags.data, 'LIST_NAME')),

                P_JRSDCTN_DPOC_RELEASE: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'JRSDCTN_DPOC_RELEASE')),
                P_JRSDCTN_DPOC_VER_NUM: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'JRSDCTN_DPOC_VER_NUM')),
                P_JRSDCTN_IQ_GDLN_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'JRSDCTN_IQ_GDLN_ID')),
                P_JRSDCTN_NM: MIApp.Sanitize.encodeProp(getDelimitedValue(_guideline_summary_data, 'JRSDCTN_NM')),
                P_JRSDCTN_IND: null,

                //Child records of guideline - CONFIGURATION
                P_DTQ_DPOC_RELEASE: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DPOC_RELEASE')),
                P_DTQ_DPOC_VER_NUM: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DPOC_VER_NUM')),
                P_DTQ_IQ_GDLN_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DTQ_IQ_GDLN_ID')),                
                P_DTQ_STATES_APPL: MIApp.Sanitize.encodeProp(
                    getDelimitedValue(
                        DetailController.grid.config.data,
                        'STATES_APPL',
                        false,
                        val => val.replace(/,/g, '|')
                    )
                ),
                P_DTQ_STATES_INCL_EXCL_CD: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'STATES_INCL_EXCL_CD')),
                P_DTQ_POS_APPL: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'POS_APPL')),
                P_DTQ_POS_INCL_EXCL_CD: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'POS_INCL_EXCL_CD')),
                P_DTQ_NM: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DTQ_NM')),
                P_DTQ_TYPE: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DTQ_TYPE')),
                P_DTQ_RSN: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DTQ_RSN')),
                P_DTQ_ATTACH_RQST_IND: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'DTQ_ATTACH_RQST_IND')),
                P_MED_PLCY_REF_CODE: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'MED_PLCY_REF_CODE')),
                P_GNTC_KL_PLCY_ID: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'GNTC_KL_PLCY_ID')),
                P_GNTC_KL_PLCY_NM: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'GNTC_KL_PLCY_NM')),

                // Additional CONFIGURATION SECTION fields
                P_HOLDING_DTQ: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'HOLDING_DTQ')),
                P_HOLDING_DTQ_VERSION: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'HOLDING_DTQ_VERSION')),
                P_TGT_DTQ: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'TGT_DTQ')),
                P_TGT_DTQ_VERSION: MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'TGT_DTQ_VERSION')),
                P_RULE_COMMENTS: null, // MIApp.Sanitize.encodeProp(getDelimitedValue(DetailController.grid.config.data, 'RULE_COMMENTS')),

                // CHANGE HISTORY SECTION
                P_USER_ID: null,
                P_CHANGE_REQ_ID: MIApp.Sanitize.encodeProp($("#txtChangeSource").val()),
                P_CHANGE_DESC: MIApp.Sanitize.encodeProp($('#txtChangeDesc').val()),
                P_RECORD_ENTRY_METHOD: MIApp.Sanitize.encodeProp(RECORD_ENTRY_METHOD),
                P_UIR_RSN: MIApp.Sanitize.encodeProp(
                    $('#ddlDPOCNotImplementedReason').val() == null || $('#ddlDPOCNotImplementedReason').val() == ''
                        ? ''
                        : $('#ddlDPOCNotImplementedReason').val()
                ),

                DPOC_ID: MIApp.Sanitize.encodeProp(DPOCID),
                DPOC_VER_EFF_DT: DPOCVersionEffDt

            };

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: `${VIRTUAL_DIRECTORY}/DPOC/Home/Upsert`,
                data: DPOC_Ins_Upd_Pkg_Param,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (updateDto) {
                    if (updateDto.StatusID === -1) {
                        MICore.Notification.error(
                            'Error occurred!',
                            'An error occurred while saving the record. Please check with the administrator.',
                            null
                        );
                        return;
                    }

                    Swal.fire({
                        title: 'Success!',
                        text: 'PIMS record has been updated!',
                        icon: 'success',
                        showCancelButton: false,
                        allowOutsideClick: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        if (!result.isConfirmed) return;

                        Swal.fire({
                            title: 'Loading...',
                            text: 'Reloading record, please wait...',
                            allowOutsideClick: false,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();

                                setTimeout(() => {
                                    const isCurrent = $('#isCurrentRecord').val() === 'Y';
                                    const baseUrl = MIApp.Common.ApiEPRepository.get('viewdetail', 'DpocUrls');
                                    const encodedParams = isCurrent
                                        ? MIApp.Sanitize.encode(`${DPOCID},${p_DPOC_PACKAGE}`)
                                        : MIApp.Sanitize.encode(`${DPOCID},${DPOCVersionEffDt},${p_DPOC_PACKAGE}`);

                                    window.location.href = baseUrl.replace('__pims_id__', encodedParams);
                                }, 100);
                            }
                        });
                    });
                },
                error: function () {
                    DetailController.message.showError();
                }
            });
        }
    },
    redirect: {
        redirectToDpocDetail: function (pims_id, routeKey) {
            const redirect = () => {
                Swal.fire({
                    title: 'Redirecting...',
                    text: 'Redirecting to detail page, please wait...',
                    timer: 60000,
                    timerProgressBar: true,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        const url = MIApp.Common.ApiEPRepository
                            .get(routeKey, 'DpocUrls')
                            .replace('__pims_id__', MIApp.Sanitize.encode(pims_id));
                        window.location.href = url;
                    }
                });
            };

            if (isDirty) {
                const confirmLeave = confirm("Changes you made may not be saved.\n\nAre you sure you want to leave?");
                if (!confirmLeave) return;

                $(window).off('beforeunload');
            }

            redirect();
        }
    },
    dirtyform: {
        //dirtycheck: function () {
        //    let formSelector = null;

        //    if (addDetail) {
        //        formSelector = '#addForm';
        //    } else if (editDetail) {
        //        formSelector = '#editForm';
        //    } else if (duplicateDetail) {
        //        formSelector = '#duplicateForm';
        //    }

        //    if (!formSelector) return;

        //    let formNewData = $(formSelector).serialize();

        //    // Append grid data
        //    formNewData += DetailController.dirtyform.get_grid_guideline_summary_active_Data();
        //    formNewData += DetailController.dirtyform.get_grid_config_Data();
        //    formNewData += DetailController.dirtyform.get_grid_diagList_Data();
        //    // formNewData += DetailController.dirtyform.get_grid_state_Data(); // Uncomment if needed

        //    const hasChanged = formNewData !== GridAndFormDirtyChecker.originalFormData;

        //    DetailController.dirtyform.toggleButtons(hasChanged);
        //    isDirty = hasChanged;
        //},
        get_grid_guideline_summary_active_Data: function () {
            var grid_guideline_summary_active = $("#grid_guideline_summary_active").data("kendoGrid").dataSource.data();
            return "&grid_guideline_summary_active=" + JSON.stringify(grid_guideline_summary_active);
        },
        get_grid_config_Data: function () {
            var grid_config = $("#grid_configHidden").data("kendoGrid").dataSource.data();
            return "&grid_config=" + JSON.stringify(grid_config);
        },
        get_grid_diagList_Data: function () {
            var grid_diag_list = $("#grid_GSDiagListHidden").data("kendoGrid").dataSource.data();
            return "&grid_diaglist=" + JSON.stringify(grid_diag_list);
        },
        get_grid_state_Data: function () {
            var grid_state = $("#grid_GSStateInfoHidden").data("kendoGrid").dataSource.data();
            return "&grid_state=" + JSON.stringify(grid_state);
        },
        toggleButtons: function (enable) {
            let formId = null;

            if (addDetail) {
                formId = '#addForm';
            } else if (editDetail) {
                formId = '#editForm';
            } else if (duplicateDetail) {
                formId = '#duplicateForm';
            }

            if (formId) {
                const $form = $(formId);
                const $buttons = $form.find('.btnSave, #btnReset');

                if (enable) {
                    $buttons.removeAttr('disabled');
                } else {
                    $buttons.attr('disabled', true);
                }
            }
        }
    },
    grid: {
        param: function () {
            if (duplicateDetail) {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#txtDPOCPackageOld").val())
                }
            } else {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#ddlDPOCPackage").val())
                }
            }
        },
        config: {
            data: [],
            onDataBound: function (e) {
                //DetailController.grid.config.data = e.sender.dataSource.data();
                onGridDataBound(e);

                if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
                    GridLoadTracker.markLoaded("grid_configHidden");
                }
            },
            validation: function (gridId) {
                const gridData = gridId
                    ? $("#" + gridId).data("kendoGrid").dataSource._data
                    : DetailController.grid.config.data;

                let validationGSSPOSText = '';
                let validationGSSPOSInclExclText = '';

                if (!gridData || gridData.length === 0) return '';

                const isValuesEntered = gridData.every(config =>
                    config.POS_APPL || config.STATES_APPL || config.DTQ_NM || config.TGT_DTQ || config.TGT_DTQ_VERSION || config.HOLDING_DTQ || config.HOLDING_DTQ_VERSION || config.DPOC_SOS_PROVIDER_TIN_EXCL || config.DPOC_ADDTNL_RQRMNTS || config.PKG_CONFIG_COMMENTS
                );
                //|| config.RULE_COMMENTS removed 10/21/2025 MFQ as per new logic -- User Story 138385

                const isPOSSelected = gridData.every(config => config.POS_APPL);
                const isInclExclSelected = gridData.every(config => config.POS_INCL_EXCL_CD);

                if (isPOSSelected && !isInclExclSelected) {
                    validationGSSPOSInclExclText = "Please select Include/Exclude for one of the POS\n";
                }

                if (!isValuesEntered) {
                    validationGSSPOSText = "Please enter/select at least some value for added Configuration for one of the Guidelines\n";
                }

                const seen = new Set();
                let hasDuplicate = false;

                for (const config of gridData) {
                    const key = [
                        config.POS_APPL,
                        config.POS_INCL_EXCL_CD,
                        (config.STATES || '').split(',').map(s => s.trim().toUpperCase()).sort().join(','),
                        config.STATES_INCL_EXCL_CD,
                        config.DTQ_NM,
                        config.TGT_DTQ,
                        config.TGT_DTQ_VERSION,
                        config.HOLDING_DTQ,
                        config.HOLDING_DTQ_VERSION,
                        config.RULE_COMMENTS,
                        config.DPOC_SOS_PROVIDER_TIN_EXCL, // User Story 138385 MFQ 9/22/2025
                        config.DPOC_ADDTNL_RQRMNTS, // User Story 138385 MFQ 9/22/2025
                        config.PKG_CONFIG_COMMENTS // User Story 138385 MFQ 9/22/2025
                    ]
                        .map(val => (val || '').trim().toUpperCase())
                        .join('|');

                    if (seen.has(key)) {
                        hasDuplicate = true;
                        break;
                    }
                    seen.add(key);
                }

                if (hasDuplicate) {
                    //validationGSSPOSText += "Each combination of Configuration must be unique.\n";
                }

                return validationGSSPOSText + validationGSSPOSInclExclText;
            }
        },
        guideline_summary: {
            data: [],
            validation: function (vType) {
                var validationGSSaveText = ''

                var grid_guideline_summary_activeData = $("#grid_guideline_summary_active").data("kendoGrid").dataSource._data;
                jQuery.each(grid_guideline_summary_activeData, function (index, gdl) {

                    validationGSSaveText = DPOC_GuidelineSummaryController.validation(
                        gdl.GDLN_ASSOC_EFF_DT,
                        gdl.GDLN_ASSOC_EXP_DT,
                        gdl.IQ_GDLN_STATUS,
                        gdl.IQ_GDLN_VERSION,
                        gdl.IQ_CRITERIA,
                        gdl.DPOC_RELEASE,
                        gdl.DPOC_VER_NUM);

                });

                return validationGSSaveText;
            }
        },
        diags: {
            data: [],
            onDataBound: function (e) {
                //DetailController.grid.diags.data = e.sender.dataSource.data();
                onGridDataBound(e);

                if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
                    GridLoadTracker.markLoaded("grid_GSDiagListHidden");
                }
            },
            validation: function (gridId) {
                let gridDiagData = gridId
                    ? $("#" + gridId).data("kendoGrid").dataSource._data
                    : DetailController.grid.diags.data;

                let validationText = '';

                if (gridDiagData.length > 0) {
                    if (!gridId) {
                        const groupedDiags = gridDiagData.reduce((acc, diag) => {
                            if (!acc[diag.DIAG_IQ_GDLN_ID]) {
                                acc[diag.DIAG_IQ_GDLN_ID] = [];
                            }
                            acc[diag.DIAG_IQ_GDLN_ID].push(diag);
                            return acc;
                        }, {});

                        const allDiagsByGuideline = Object.values(groupedDiags);

                        allDiagsByGuideline.forEach(diags => {
                            if (validationText === '') {
                                validationText += validateDiags(diags);
                            }
                        });
                    } else {
                        validationText = validateDiags(gridDiagData);
                    }
                }

                return validationText;

                function validateDiags(diags) {
                    let result = '';

                    const isInclExclValid = Object.values(
                        diags.reduce((acc, diag) => {
                            const key = diag.DPOC_VER_NUM;
                            if (!acc[key]) {
                                acc[key] = new Set();
                            }
                            acc[key].add(diag.DIAG_INCL_EXCL_CD);
                            return acc;
                        }, {})
                    ).every(set => set.size === 1);

                    const isDiagSelected = diags.every(diag => diag.DIAG_CD !== '' || diag.LIST_NAME !== '');

                    if (!isInclExclValid) {
                        result += "Please select same Include/Exclude for all Diagnosis codes for same Guideline\n";
                    }

                    if (!isDiagSelected) {
                        result += "Please select Diagnosis list or Diagnosis code in one of the record(s) inserted in Guideline Diagnosis code section!\n";
                    }

                    return result;
                }
            }

        },
        states: {
            data: [],
            onDataBound: function (e) {
                //DetailController.grid.states.data = e.sender.dataSource.data();

                onGridDataBound(e);

                if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
                    GridLoadTracker.markLoaded("grid_GSStateInfoHidden");
                }
            },
            validation: function (gridId, calledFromPopup) {
                let generalValidation = '';
                let stateStartEndText = '';
                let stateDPOCStartEndText = '';
                let stateGdlnStartEndText = '';

                let stateData = gridId
                    ? $("#" + gridId).data("kendoGrid").dataSource._data
                    : DetailController.grid.states.data;

                // Validation for Include/Exclude selection
                if (gridId) {
                    if ($('#ddlStateInclExcl').val() === '' && stateData.length > 0) {
                        generalValidation += "Please select Include/Exclude for all states!\n";
                    }
                } 
                
                // Additional validation if called from popup
                if (calledFromPopup) {
                    for (const state of stateData) {
                        if (!state.STATE_CD && state.STATE_CD == '') {
                            generalValidation = "State cannot be blank, please select state first in the guideline\n";
                            break;
                        }
                    }
                }

                return generalValidation + stateStartEndText + stateDPOCStartEndText + stateGdlnStartEndText;
            }
        },
        dtqs: {
            data: [],
            onDataBound: function (e) {
                DetailController.grid.dtqs.data = this.view();
            },
            validate: function (gridId) {
                let validationTypeText = '';
                let validationReasonText = '';
                let validationTargetText = '';
                let validationHoldingText = '';

                const gridData = $("#" + gridId).data("kendoGrid").dataSource._data;
                const currentPOS = GSSDTQController.methods.getPOS();
                let isPOSMatched = true;

                for (const dtq of gridData) {
                    const posCode = dtq.DTQ_POS_APPL;

                    if (posCode && !currentPOS.some(p => p.PLC_OF_SVC_CD === posCode)) {
                        isPOSMatched = false;
                    }

                    if (dtq.TGT_DTQ?.trim()) {
                        if (!dtq.TGT_DTQ_VERSION?.trim()) {
                            validationTargetText = 'Please select Target Version in one of the record(s) in DTQ Target section!\n';
                        }
                    }

                    if (dtq.HOLDING_DTQ?.trim()) {
                        if (!dtq.HOLDING_DTQ_VERSION?.trim()) {
                            validationHoldingText = 'Please select Holding Version in one of the record(s) in DTQ Holding section!\n';
                        }
                    }
                }

                if (!isPOSMatched) {
                    validationTypeText += 'Guideline Data cannot Save – The Place of Service (POS) selected for the DTQ is not listed on the POS screen. Either select another POS for the DTQ or add the additional POS to the POS Screen!\n';
                }

                return validationTypeText + validationReasonText + validationTargetText + validationHoldingText;
            }
        },
    }
}


const DeleteVersion = {
    init: function () {
        this.cacheElements();
        this.initWindow();
        this.bindEvents();
    },

    cacheElements: function () {
        this.$window = $("#deleteVersionWindow");
        this.$crNumber = $("#CRNumber");
        this.$deleteReason = $("#DeleteReason");
        this.$okButton = $("#deleteVersionOK");
        this.$cancelButton = $("#deleteVersionCancel");
    },

    initWindow: function () {
        this.kendoWindow = this.$window.kendoWindow({
            title: "Delete Version",
            modal: true,
            visible: false,
            width: "350px",
            position: {
                top: 100,
                left: "40%"
            }
        }).data("kendoWindow");
    },

    validate: function () {
        const crNumber = this.$crNumber.val().trim();
        const deleteReason = this.$deleteReason.val().trim();

        if (!crNumber || !deleteReason) {
            Swal.fire({
                icon: 'error',
                title: 'Validation Error',
                text: 'Both CR Number and Delete Reason are required.'
            });
            return false;
        }
        return true;
    },

    submit: function () {
        const obj = {
            "P_DPOC_HIERARCHY_KEY": MIApp.Sanitize.encodeProp(p_DPOC_HIERARCHY_KEY),
            "P_CHANGE_REQ_ID": MIApp.Sanitize.encodeProp($("#txtCRNumber").val()),
            "P_CHANGE_DESC": MIApp.Sanitize.encodeProp($("#txtDeleteReason").val()),
            "P_DPOC_VER_EFF_DT": p_DPOC_VER_EFF_DT,
            "P_DPOC_PACKAGE": MIApp.Sanitize.encodeProp(p_DPOC_PACKAGE)
        };

        var token = $('meta[name="request-verification-token"]')[0].content;

        $.ajax({
            type: "POST",
            url: this.$window.data('deleteversionurl'),
            headers: {
                 'Accept': 'application/json',
                'RequestVerificationToken': token
            },
            data: obj,
            success: (ret) => {
                Swal.fire({
                    title: 'Success!',
                    text: 'DPOC version has been deleted!',
                    icon: 'success',
                    confirmButtonText: 'OK',
                    customClass: 'swal-size-sm'
                }).then(() => {
                    window.location.href = $(".search-section").data('searchurl');
                });
            },
            error: (e) => {
                MICore.Notification.error('Error occurred', e.responseJSON?.Message || 'Unknown error', null);
            }
        });
    },

    bindEvents: function () {
        this.$okButton.on('click', () => {
            if (this.validate()) {
                this.submit();
            }
        });

        this.$cancelButton.on('click', () => {
            this.kendoWindow.close();
        });
    }
};

$(function () {
    DeleteVersion.init();
});


function loadAllFormdataToTracker() {

    if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
        GridLoadTracker.onAllLoaded(() => {

            const formType = addDetail
                ? '#addForm'
                : editDetail
                    ? '#editForm'
                    : duplicateDetail
                        ? '#duplicateForm'
                        : null;

            GridAndFormDirtyChecker.init({
                forms: [formType], //if there are multiple forms, then pass in as array
                getGridData: () => ({
                    config: DetailController.grid.config.data,
                    guideline_summary: DetailController.grid.guideline_summary.data,
                    diags: DetailController.grid.diags.data,
                    states: DetailController.grid.states.data
                })
            });
            $('.loading-header').hide();
            hideGridLoading('grid_guideline_summary_active');
        });
    }

    // Hook into your dirtycheck logic
    DetailController.dirtyform.dirtycheck = function () {
        GridAndFormDirtyChecker.dirtycheck(DetailController.dirtyform.toggleButtons);
    };

    // Focus dropdown if editing
    if (editDetail) {
        $('#ddlDPOCEligible').data('kendoDropDownList').focus();
    }
}

function returnNullIfAllNull(arr) {
    return arr.every(element => element === null) ? null : arr;
}

function onGridDataBound(e) {

    var grid = e.sender;
    var gridId = grid.element.attr("id");
    var data = grid.dataSource.view();

    switch (gridId) {
        case "grid_configHidden":

            DetailController.grid.config.data = data.map(function (dtq) {
                return {
                    RowNumber: dtq.RowNumber,
                    GDLN_DTQ_SYS_SEQ: dtq.GDLN_DTQ_SYS_SEQ,
                    DTQ_IQ_GDLN_ID: dtq.DTQ_IQ_GDLN_ID,
                    POS_APPL: dtq.POS_APPL,
                    POS_INCL_EXCL_CD: dtq.POS_INCL_EXCL_CD,
                    POS_INCL_EXCL_DESC: dtq.POS_INCL_EXCL_DESC,
                    STATES_APPL: dtq.STATES_APPL,
                    STATES_INCL_EXCL_CD: dtq.STATES_INCL_EXCL_CD,
                    STATES_INCL_EXCL_DESC: dtq.STATES_INCL_EXCL_DESC,
                    DTQ_NM: dtq.DTQ_NM,
                    TGT_DTQ: dtq.TGT_DTQ,
                    TGT_DTQ_VERSION: dtq.TGT_DTQ_VERSION,
                    HOLDING_DTQ: dtq.HOLDING_DTQ,
                    HOLDING_DTQ_VERSION: dtq.HOLDING_DTQ_VERSION,
                    RULE_COMMENTS: dtq.RULE_COMMENTS,
                    DPOC_RELEASE: dtq.DPOC_RELEASE,
                    DPOC_VER_NUM: dtq.DPOC_VER_NUM,
                    DPOC_SOS_PROVIDER_TIN_EXCL: dtq.DPOC_SOS_PROVIDER_TIN_EXCL, // User Story 138385 MFQ 9/22/2025
                    DPOC_ADDTNL_RQRMNTS: dtq.DPOC_ADDTNL_RQRMNTS, // User Story 138385 MFQ 9/22/2025
                    PKG_CONFIG_COMMENTS: dtq.PKG_CONFIG_COMMENTS // User Story 138385 MFQ 9/22/2025
                };
            });
            break;
        case "grid_guideline_summary_active":
            DetailController.grid.guideline_summary.data = columnData;
            break;
        case "grid_GSDiagListHidden":

            DetailController.grid.diags.data = data.map(function (diag) {
                return {
                    DIAG_IQ_GDLN_ID: diag.DIAG_IQ_GDLN_ID,
                    DIAG_CD: diag.DIAG_CD,
                    DIAG_INCL_EXCL_CD: diag.DIAG_INCL_EXCL_CD,
                    DIAG_INCL_EXCL_CD_DESC: diag.DIAG_INCL_EXCL_CD_DESC,
                    LIST_NAME: diag.LIST_NAME,
                    DPOC_RELEASE: diag.DPOC_RELEASE,
                    DPOC_VER_NUM: diag.DPOC_VER_NUM
                };
            });

            break;
        case "grid_GSStateInfoHidden":

            DetailController.grid.states.data = data.map(function (state) {
                return {
                    ATS_IQ_GDLN_ID: state.ATS_IQ_GDLN_ID,
                    GDLN_DTQ_SYS_SEQ: state.GDLN_DTQ_SYS_SEQ,
                    STATE_CD: state.STATE_CD,
                    STATE_NAME: state.STATE_NAME,
                    ATS_INCL_EXCL_CD: state.ATS_INCL_EXCL_CD,
                    ATS_INCL_EXCL_CD_DESC: state.ATS_INCL_EXCL_CD_DESC,
                    DPOC_RELEASE: state.DPOC_RELEASE,
                    DPOC_VER_NUM: state.DPOC_VER_NUM
                };
            });

            break;
    }
}
function showGridLoading(gridId) {
    var grid = $("#" + gridId);
    if (grid.find(".kendo-grid-loading").length === 0) {
        var loadingDiv = $("<div class='kendo-grid-loading'>Loading...</div>");
        grid.css("position", "relative").append(loadingDiv);
    }
}

function hideGridLoading(gridId) {
    $("#" + gridId).find(".kendo-grid-loading").remove();
}


$(document).ready(function () {

    if (duplicateDetail) {
        p_DPOC_HIERARCHY_KEY = MIApp.Sanitize.string($('#txtPIMSID').val());
        p_DPOC_VER_EFF_DT = MIApp.Sanitize.string($('#txtDPOCVersionDt').val());
        p_DPOC_PACKAGE = MIApp.Sanitize.string($("#txtDPOCPackageOld").val());

    }
    else {
        p_DPOC_HIERARCHY_KEY = MIApp.Sanitize.string($('#txtPIMSID').val());
        p_DPOC_VER_EFF_DT = MIApp.Sanitize.string($('#txtDPOCVersionDt').val());
        p_DPOC_PACKAGE = MIApp.Sanitize.string($("#ddlDPOCPackage").val());
    }

    DetailController.init();

    $(window).bind('beforeunload', function () {
        if (isDirty) {
            return 'please save your setting before leaving the page.';
        }
    });

    setTimeout(() => {

        if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
            $('.loading-header').show();
        } else {
            $('.loading-header').hide();
        }
        
        // Show loading indicator
        showGridLoading('grid_guideline_summary_active');

        loadAllFormdataToTracker();

    }, 500); // Use 5000ms for localhost testing if needed

});
