var form_original_data
var form_new_data
var isDirty = false
var current_title = $(document).attr('title');
var viewDetail = current_title.includes("View Detail");
var addDetail = current_title.includes("Add Detail");
var editDetail = current_title.includes("Edit Detail");
var duplicateDetail = current_title.includes("Duplicate Record Detail");
var lblCodeInactive = $("#lblCodeInactive").html();

var drivingStatus = {
    PA: 'PA',
    PASOS: 'PASOS',
    PAAA: 'PAAA',
    Pred: 'Pred',
    Adv: 'Adv',
    AdvSOS: 'AdvSOS',
    DRAL: 'DRAL',
    AA: 'AA',
    MSP: 'MSP',
    ALL: 'ALL',
    NONE: 'NONE',
    SOS: 'SOS',
    PAS: 'PAS',
    PredS: 'PredS',
    DRALS: 'DRALS',
    AdvS: 'AdvS',
    AAS: 'AAS',
    SOSS: 'SOSS',
    MSPS: 'MSPS'
}

var DetailController = {

    init: function () {
        DetailController.bind();
    },
    bind: function () {

        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        function SOSEnable() {
            $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(true);
            $("#dpSOSExpirationDate").data("kendoDatePicker").enable(true);
            if ($("#dpSOSEffectiveDate").val() != '') {
                $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('disabled', false);
                });
            } else {
                $("#ddlURGCategory").data("kendoDropDownList").enable(false);
                $("#ddlSOSTypeReview").data("kendoDropDownList").enable(false);
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('disabled', false);
                });
            }
        }

        function SOSDisable() {
            $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(false);
            $("#dpSOSExpirationDate").data("kendoDatePicker").enable(false);
            $("#ddlURGCategory").data("kendoDropDownList").enable(false);
            $("#ddlSOSTypeReview").data("kendoDropDownList").enable(false);
            $("input[name='sossite']").each(function (i) {
                $(this).attr('disabled', true);
            });
        }

        $("#btnDuplicateCopy").click(function () {
            //var pims_id = MIApp.Sanitize.string($("#txtPIMSID").val()); // fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/85 MFQ 7/17/2023
            var pims_id = $("#txtPIMSID").val();
            DetailController.redirect.redirect_duplicateRecordDetail(pims_id);
        });

        $("#btnViewOnly").click(function () {
            //var pims_id = MIApp.Sanitize.string($("#txtPIMSID").val()); // fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/87 MFQ 7/17/2023
            var pims_id = $("#txtPIMSID").val();
            var EPALVersionDt = $("#txtEPALVersionDt").val();
            DetailController.redirect.redirect_viewDetail(pims_id + ',' + kendo.toString(kendo.parseDate(EPALVersionDt), 'MM-dd-yyyy hh:mm:ss tt'));
        });

        // If SOS Indicator is YES and SOS Effective date is NOT NULL then enable required field
        $('#txtSiteofServiceApplies').change(function () {
            if ($(this).val() == 'Yes') {
                if ($("#dpSOSEffectiveDate").val() != '') {
                    $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                    $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                    $("input[name='sossite']").each(function (i) {
                        $(this).attr('disabled', false);
                    });
                }                     
            } 

            if ($('#txtPriorAuth').val() == 'Yes' || $('#txtAdvNotification').val() == 'Yes') {
                //USER STORY 95312 MFQ 4-2-2024
                DetailController.drivingStatus.activate();
            }
        });
        
        $('#txtPriorAuth').change(function () {
            if ($(this).val() == 'Yes') {
                $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(true);
                $("#dpSOSExpirationDate").data("kendoDatePicker").enable(true);
                if ($("#dpSOSEffectiveDate").val() != '') {
                    $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                    $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                    $("input[name='sossite']").each(function (i) {
                        $(this).attr('disabled', false);
                    });
                } 
                DetailController.drivingStatus.activate();
            } else {
                // BUG 35142 MFQ 9/29/2022
                //DetailController.methods.SOS.revertBackToOriginalValues();
                // END of BUG 35142
                DetailController.drivingStatus.activate();
                // Disable all other SOS fields
                if (DetailController.drivingStatus?.name != drivingStatus.Adv &&
                    DetailController.drivingStatus?.name != drivingStatus.AdvSOS &&
                    DetailController.drivingStatus?.name != drivingStatus.PA &&
                    DetailController.drivingStatus?.name != drivingStatus.PASOS ||
                    DetailController.drivingStatus?.name == undefined
                ) {
                    SOSDisable();
                }
            }
                        
        });
 
        $('#dpPAEffectiveDate').on('change', function () {
            var PAEffectiveDate = $("#dpPAEffectiveDate").val();
            var PAExpirationDate = $("#dpPAExpirationDate").val();

            if (PAEffectiveDate != "" && PAExpirationDate == "" && Date.parse(PAEffectiveDate) <= Date.parse(TODAY_DATE)) {
                $("#txtPriorAuth").val('Yes').trigger('change');
                $("#programManagedBy-tab").addClass('required');
                DetailController.datePicker.enabledPAExpireDate();
            }
            else if (PAEffectiveDate == "") {
                $("#programManagedBy-tab").removeClass('required');
                $("#txtPriorAuth").val('No').trigger('change');
                DetailController.datePicker.disablePAExpireDate();
            } else if (PAEffectiveDate != "") {
                DetailController.datePicker.enabledPAExpireDate();
            }     

            DetailController.drivingStatus.activate();
        });

        $('#dpPAExpirationDate').on('change', function () {
            var PAEffectiveDate = $("#dpPAEffectiveDate").val();
            var PAExpirationDate = $("#dpPAExpirationDate").val();

            if (PAEffectiveDate && PAEffectiveDate != '' && PAExpirationDate && PAExpirationDate != '') {
                if (Date.parse(PAEffectiveDate) > Date.parse(PAExpirationDate)) {
                    $("#dpPAExpirationDate").val('');
                    DetailController.message.showWarning('PA Exp Date must be equal to or later than the PA Effective Date');
                    return
                }
            }
            if (PAEffectiveDate != "" && PAExpirationDate == "") {
                $("#txtPriorAuth").val('Yes').trigger('change');
                $("#programManagedBy-tab").addClass('required');
            }
            else if (Date.parse(PAExpirationDate) > Date.parse(TODAY_DATE)) {
                $("#txtPriorAuth").val('Yes').trigger('change');
                $("#programManagedBy-tab").addClass('required');                
            }
            else if (Date.parse(PAExpirationDate) <= Date.parse(TODAY_DATE)) {                
                $("#programManagedBy-tab").removeClass('required');
                $("#txtPriorAuth").val('No').trigger('change');
            }

            if (DetailController.drivingStatus.renderCurrent()?.name == drivingStatus.PA) {
                ProgMgdByController.grid.updatePMBExpDate($("#dpPAExpirationDate").val(), $("#txtPriorAuth").val());
            }
            StateInfoController.grid.updateStateDate($("#dpPAExpirationDate").val()); // USER STORY 94391 MFQ 3-25-2024
        });

        //USER STORY 54535 MFQ 3/31/2023
        /*Pre-Determination Section*/ 
        $('#dpPreDeterminationEffDate').on('change', function () {
            var preDetEffDate = $("#dpPreDeterminationEffDate").val();
            var preDetExpDate = $.trim($("#dpPreDeterminationExpDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpPreDeterminationExpDate").val());

            if (Date.parse(TODAY_DATE) >= Date.parse(preDetEffDate) && Date.parse(TODAY_DATE) <= Date.parse(preDetExpDate)) {
                $("#txtPreDetermination").val('Yes').trigger('change');    
                $("#programManagedBy-tab").addClass('required');
            } else {
                $("#txtPreDetermination").val('No').trigger('change');
                $("#programManagedBy-tab").removeClass('required');
            }

            if (preDetEffDate != '')
                DetailController.datePicker.enablePreDetermineExpireDate();
            else 
                DetailController.datePicker.disablePreDetermineExpireDate();

            //USER STORY 95312 MFQ 4-1-2024
            DetailController.drivingStatus.activate();
        });

        $('#dpPreDeterminationExpDate').on('change', function () {
            var preDetEffDate = $("#dpPreDeterminationEffDate").val();
            var preDetExpDate = $.trim($("#dpPreDeterminationExpDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpPreDeterminationExpDate").val());

            if (preDetEffDate && preDetEffDate != '' && preDetExpDate && preDetExpDate != '') {
                if (Date.parse(preDetEffDate) > Date.parse(preDetExpDate)) {
                    $("#dpPreDeterminationExpDate").val('');
                    DetailController.message.showWarning('Pre-Determination Expiration Date must be equal to or later than the Pre-Determination Effective Date');
                    return
                }
            }

            if (Date.parse(TODAY_DATE) >= Date.parse(preDetEffDate) && Date.parse(TODAY_DATE) <= Date.parse(preDetExpDate)) {
                $("#txtPreDetermination").val('Yes').trigger('change');
                //USER STORY 95312 MFQ 4-1-2024
                DetailController.drivingStatus.activate();
                $("#programManagedBy-tab").addClass('required');
            } else {
                $("#txtPreDetermination").val('No').trigger('change');
                //USER STORY 95312 MFQ 4-1-2024                
                DetailController.drivingStatus.activate(); 
                $("#programManagedBy-tab").removeClass('required');
            }

            if (DetailController.drivingStatus.renderCurrent()?.name == drivingStatus.Pred) {
                ProgMgdByController.grid.updatePMBExpDate($("#dpPreDeterminationExpDate").val(), $("#txtPreDetermination").val())
            }
        });
        /*End of Pre-Determination Section*/

        // USER STORY 95313 MFQ 4-1-2024
        /*Advanced Notification Section*/
        $('#txtAdvNotification').change(function () {
            if ($(this).val() == 'Yes') {
                $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(true);
                $("#dpSOSExpirationDate").data("kendoDatePicker").enable(true);
                if ($("#dpSOSEffectiveDate").val() != '') {
                    $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                    $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                    $("input[name='sossite']").each(function (i) {
                        $(this).attr('disabled', false);
                    });
                }
                DetailController.drivingStatus.activate();
            } else {
                // BUG 35142 MFQ 9/29/2022
                //DetailController.methods.SOS.revertBackToOriginalValues();
                // END of BUG 35142
                DetailController.drivingStatus.activate();
                // Disable all other SOS fields
                if (DetailController.drivingStatus?.name != drivingStatus.Adv &&
                    DetailController.drivingStatus?.name != drivingStatus.AdvSOS &&
                    DetailController.drivingStatus?.name != drivingStatus.PA &&
                    DetailController.drivingStatus?.name != drivingStatus.PASOS ||
                    DetailController.drivingStatus?.name == undefined
                ) {
                    SOSDisable();
                }
            }            
        });


        $('#dpAdvNotificationEffDate').on('change', function () {
            dpAdvNotDatesChange();

            var effDate = $("#dpAdvNotificationEffDate").val();
            if (effDate != '')
                DetailController.datePicker.enableAdvancedNotificationExpireDate();
            else
                DetailController.datePicker.disableAdvancedNotificationExpireDate();
        });

        $('#dpAdvNotificationExpDate').on('change', function () {
            var effDate = $("#dpAdvNotificationEffDate").val();
            var expDate = $.trim($("#dpAdvNotificationExpDate").val())

            if (effDate && effDate != '' && expDate && expDate != '') {
                if (Date.parse(effDate) > Date.parse(expDate)) {
                    $("#dpAdvNotificationExpDate").val('');
                    DetailController.message.showWarning('Advanced Notification Exp Date must be equal to or later than the Advanced Notification Effective Date');
                    return
                }
            }

            if (DetailController.drivingStatus.renderCurrent()?.name == drivingStatus.Adv) {
                ProgMgdByController.grid.updatePMBExpDate($("#dpAdvNotificationExpDate").val(), $("#txtAdvNotification").val());
            }            
        });

        function dpAdvNotDatesChange() {
            var advNotEffDate = $("#dpAdvNotificationEffDate").val();
            var advNotExpDate = $.trim($("#dpAdvNotificationExpDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpAdvNotificationExpDate").val());
            DetailController.drivingStatus.renderCurrent();

            if (Date.parse(TODAY_DATE) >= Date.parse(advNotEffDate) && Date.parse(TODAY_DATE) <= Date.parse(advNotExpDate)) {
                $("#txtAdvNotification").val('Yes').trigger('change');
                //USER STORY 95312 MFQ 4-1-2024
                DetailController.drivingStatus.activate(DetailController.drivingStatus.name); 
                $("#programManagedBy-tab").addClass('required');
            } else {
                $("#txtAdvNotification").val('No').trigger('change');
                //USER STORY 95312 MFQ 4-1-2024
                DetailController.drivingStatus.activate(DetailController.drivingStatus.name);
                $("#programManagedBy-tab").removeClass('required');
            }

            // Disable all other SOS fields
            if (DetailController.drivingStatus.name != drivingStatus.Adv && DetailController.drivingStatus.name != drivingStatus.AdvSOS && DetailController.drivingStatus.name != drivingStatus.PA && DetailController.drivingStatus.name != drivingStatus.PASOS) {
                SOSDisable();
            } else {
                SOSEnable();
            }

        }
        /*End of Advanced Notification Section*/

        // USER STORY 95315 MFQ 4-1-2024
        /*DRAL Section*/
        $('#dpDRALEffDate').on('change', function () {
            dpDRALDatesChange();

            var effDate = $("#dpDRALEffDate").val();
            if (effDate != '')
                DetailController.datePicker.enableDRALExpireDate();
            else
                DetailController.datePicker.disableDRALExpireDate();
        });

        $('#dpDRALExpDate').on('change', function () {
            var effDate = $("#dpDRALEffDate").val();
            var expDate = $.trim($("#dpDRALExpDate").val())

            if (effDate && effDate != '' && expDate && expDate != '') {
                if (Date.parse(effDate) > Date.parse(expDate)) {
                    $("#dpDRALExpDate").val('');
                    DetailController.message.showWarning('DRAL Expiration Date must be equal to or later than the DRAL Effective Date');
                    return
                }
            }
            dpDRALDatesChange();
            if (DetailController.drivingStatus.renderCurrent().name == drivingStatus.DRAL) {
                ProgMgdByController.grid.updatePMBExpDate($("#dpDRALExpDate").val(), $("#txtDrugReviewAtLaunch").val());
            }
        });

        $('#dpSOSEffectiveDate').on('change', function () {
            var effDate = $("#dpSOSEffectiveDate").val();
            if (effDate != '')
                DetailController.datePicker.enableSOSExpireDate();
            else
                DetailController.datePicker.disableSOSExpireDate();
        });

        $('#dpSOSExpirationDate').on('change', function () {
            var effDate = $("#dpSOSEffectiveDate").val();
            var expDate = $.trim($("#dpSOSExpirationDate").val())

            if (effDate && effDate != '' && expDate && expDate != '') {
                if (Date.parse(effDate) > Date.parse(expDate)) {
                    $("#dpSOSExpirationDate").val('');
                    DetailController.message.showWarning('SOS Expiration Date must be equal to or later than the SOS Effective Date');
                    return
                }
            }
        });

        function dpDRALDatesChange() {
            var dpDRALEffDate = $("#dpDRALEffDate").val();
            var dpDRALExpDate = $.trim($("#dpDRALExpDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpDRALExpDate").val());

            if (Date.parse(TODAY_DATE) >= Date.parse(dpDRALEffDate) && Date.parse(TODAY_DATE) <= Date.parse(dpDRALExpDate)) {
                $("#txtDrugReviewAtLaunch").val('Yes').trigger('change');
                //USER STORY 95312 MFQ 4-1-2024
                DetailController.drivingStatus.activate(); 
                $("#programManagedBy-tab").addClass('required');
            } else {
                $("#txtDrugReviewAtLaunch").val('No').trigger('change');
                $("#programManagedBy-tab").removeClass('required');
                //USER STORY 95312 MFQ 4-1-2024
                DetailController.drivingStatus.activate();
            }
        }
        /*End of DRAL Section*/

        // USER STORY 95315 MFQ 4-4-2024
        /*Auto Approval Section*/
        $('#dpAAEffectiveDate').on('change', function () {
            dpAADatesChange();
            var effDate = $("#dpAAEffectiveDate").val();
            if (effDate != '')
                DetailController.datePicker.enableAAExpireDate();
            else
                DetailController.datePicker.disableAAExpireDate();
        });

        $('#dpAAExpirationDate').on('change', function () {
            var effDate = $("#dpAAEffectiveDate").val();
            var expDate = $.trim($("#dpAAExpirationDate").val())

            if (effDate && effDate != '' && expDate && expDate != '') {
                if (Date.parse(effDate) > Date.parse(expDate)) {
                    $("#dpAAExpirationDate").val('');
                    DetailController.message.showWarning('AA Expiration Date must be equal to or later than the AA Effective Date');
                    return
                }
            }
            dpAADatesChange();
        });        

        function dpAADatesChange() {

            DetailController.drivingStatus.renderCurrent();

            var dpAAEffectiveDate = $("#dpAAEffectiveDate").val();
            var dpAAExpirationDate = $.trim($("#dpAAExpirationDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpAAExpirationDate").val());

            if (Date.parse(TODAY_DATE) >= Date.parse(dpAAEffectiveDate) && Date.parse(TODAY_DATE) <= Date.parse(dpAAExpirationDate)) {
                $("#txtAutoApproval").val('Yes').trigger('change');
            } else {
                $("#txtAutoApproval").val('No').trigger('change');                
            }
            //USER STORY 95312 MFQ 4-1-2024
            //Set Active status                
            DetailController.drivingStatus.activate();

        }
        /*End of Auto Approval Section*/

        // USER STORY 95626 MFQ 4-4-2024
        /*MSP Section*/
        $('#dpMSPEffectiveDate').on('change', function () {
            dpMSPDatesChange();

            var effDate = $("#dpMSPEffectiveDate").val();
            if (effDate != '')
                DetailController.datePicker.enableMSPExpireDate();
            else
                DetailController.datePicker.disableMSPExpireDate();
        });

        $('#dpMSPExpirationDate').on('change', function () {
            var effDate = $("#dpMSPEffectiveDate").val();
            var expDate = $.trim($("#dpMSPExpirationDate").val())

            if (effDate && effDate != '' && expDate && expDate != '') {
                if (Date.parse(effDate) > Date.parse(expDate)) {
                    $("#dpMSPExpirationDate").val('');
                    DetailController.message.showWarning('MSP Expiration Date must be equal to or later than the MSP Effective Date');
                    return
                }
            }
            dpMSPDatesChange();
        });

        function dpMSPDatesChange() {
            var dpMSPEffectiveDate = $("#dpMSPEffectiveDate").val();
            var dpMSPExpirationDate = $.trim($("#dpMSPExpirationDate").val()) == '' ? kendo.parseDate('12/31/2999') : $.trim($("#dpMSPExpirationDate").val());

            if (Date.parse(TODAY_DATE) >= Date.parse(dpMSPEffectiveDate) && Date.parse(TODAY_DATE) <= Date.parse(dpMSPExpirationDate)) {
                $("#txtMedicareSpecialProcessing").val('Yes').trigger('change');
                DetailController.drivingStatus.activate();
            } else {
                $("#txtMedicareSpecialProcessing").val('No').trigger('change');
                DetailController.drivingStatus.activate()
            }
        }
        /*End of MSP Section*/

        $("#btnSave, #btnSaveHistoric").on('click', function () {
            var saveValidation = true;
            var validateError = "";
            var activeTabNameAfterValidation = "";

            if ($("#duplicate-record").val() == "Y") { // <-- Run if duplicate screen    
                var validateResultDropdown = _DetailHeaderDuplicateRecordController.methods.validateDropdowns(); // <-- Run Duplicate PIMS Record Validation 
                saveValidation = validateResultDropdown.isValid;
                validateError += validateResultDropdown.errorMessage ?? "";

                // If duplicated record
                if (!saveValidation) {
                    DetailController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + validateError.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>");
                
                    return;
                }
            }                       

            var bypass_invalid_proc_code = false;
            // USER STORY 94391 MFQ 3-21-2024 Procedure Code Status Deleted or Invalid then warn user
            if ($('.procedure-code-status').hasClass('label-tag-red')) {
                MICore.Notification.question('Invalid Procedure code', 'Procedure Code is invalid or deleted. Do you want to proceed with edit?', 'Yes', 'No', function () {
                    restOfTheValidation();
                });
            } else {
                restOfTheValidation();
            }
            
            function restOfTheValidation() {

                //Get and set current EPAL Status for validations
                var drivingStatusDateRange = DetailController.drivingStatus.renderCurrent();

                // USER STORY 93759 MFQ 3-20-2024 Site of Service, Site of Service (SOS) Office Based Program, Site of Service (SOS) Outpatient Hospital
                if ($('#txtSiteofServiceApplies').val() == 'Yes' &&
                    $("input[name='sossite']:checked").val() == 'Site Only' &&                    
                    // 'Site of Service' && 'Site of Service (SOS) Office Based Program' && 'Site of Service (SOS) Outpatient Hospital'
                    $('#ddlAlternateCategory').val().indexOf(drivingStatus.AdvSOS) < 0 && 
                    DetailController.drivingStatus.name.indexOf(drivingStatus.PASOS) > -1
                    ) {
                        var isStandardCat = $('#txtStandardCategory').val().trim() != '';

                        if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
                            validateError += "Record cannot save - Alternate Category does not match current status of record!\n";
                            saveValidation = false;
                        }
                } else {
                    //Validation for Alternate Category
                    if ($('#ddlAlternateCategory').val().indexOf(DetailController.drivingStatus.name) < 0 && DetailController.drivingStatus.name != undefined) { // fix bug when no current status selected MFQ 7-29-2024
                        var isStandardCat = $('#txtStandardCategory').val().trim() != '';
                        if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
                            validateError += "Record cannot save - Alternate Category does not match current status of record!\n";
                            saveValidation = false;
                        }
                    }
                }

                var validateResultPASummary = DetailController.validation.validate_PASummary(); // <-- Run PA Summary Validation 
                saveValidation = (saveValidation == true) ? validateResultPASummary.isValid : false;
                validateError += validateResultPASummary.errorMessage ?? "";

                if (activeTabNameAfterValidation == "" && !validateResultPASummary.isValid) {
                    activeTabNameAfterValidation = "#EPALSummary";
                }

                // validation for SOS
                var validateResultSOS = DetailController.validation.validate_SOS();
                saveValidation = (saveValidation == true) ? validateResultSOS.isValid : false;
                validateError += validateResultSOS.errorMessage ?? "";
                if (!validateResultSOS.isValid) {
                    activeTabNameAfterValidation = validateResultSOS.activeTabNameAfterValidation;
                }

                // validation for AA
                var validateResultAA = DetailController.validation.validate_AA_Prev_Review();
                saveValidation = (saveValidation == true) ? validateResultAA.isValid : false;
                validateError += validateResultAA.errorMessage ?? "";

                // validation for MSP
                var validateResultMedSpeProc = DetailController.validation.validate_MedSpeProc();
                saveValidation = (saveValidation == true) ? validateResultMedSpeProc.isValid : false;
                validateError += validateResultMedSpeProc.errorMessage ?? "";

                if (activeTabNameAfterValidation == "" && (!validateResultSOS.isValid || !validateResultAA.isValid || !validateResultMedSpeProc.isValid)) {
                    activeTabNameAfterValidation = "#EPALReviewIndicators";
                }

                // Validation for Associated Codes
                //Get all serialized data from all grids of Assoc Code and State Info tabs            
                var associateCodesValidation = DiagCodesController.data.init();
                if (activeTabNameAfterValidation == "" && $.trim(associateCodesValidation) != '') {
                    activeTabNameAfterValidation = "#assocCodesPolicy";

                    $("#associated-codes-tabStrip").kendoTabStrip().data("kendoTabStrip").select(0);
                }

                associateCodesValidation += DiagListController.data.init();
                associateCodesValidation += RevCodesController.data.init();
                associateCodesValidation += AllocatedPlacesController.data.init();
                associateCodesValidation += ModifiersController.data.init();

                saveValidation = (saveValidation == true) ? ($.trim(associateCodesValidation) == '' ? true : false) : false;
                validateError += associateCodesValidation ?? "";

                // Retention and Reduction Factor Validation
                var validateRetRedFactor = _RetRedFactorController.data.init();
                saveValidation = saveValidation ? validateRetRedFactor.isValid : false;
                validateError += validateRetRedFactor.errorMessage ?? "";


                // PROG MGD BY VALIDATION
                var validateResultProgMgdBy = ProgMgdByController.validation();
                saveValidation = (saveValidation == true) ? validateResultProgMgdBy.isValid : false;
                validateError += validateResultProgMgdBy.errorMessage ?? "";

                // STATE INFO VALIDATION
                //If previous validation is passed then go to State Info validation
                var validateResultStateInfo = StateInfoController.validation(DetailController.drivingStatus.name, drivingStatusDateRange);
                saveValidation = (saveValidation == true) ? validateResultStateInfo.isValid : false;
                validateError += validateResultStateInfo.errorMessage ?? "";

                if (activeTabNameAfterValidation == "" && !validateResultStateInfo.isValid) {
                    // $('a[href="#stateInfo"]').click();
                    activeTabNameAfterValidation = "#stateInfo";
                }

                // validation for Change History
                var validate_ChgHist = DetailController.validation.validate_ChgHist();
                saveValidation = (saveValidation == true) ? validate_ChgHist.isValid : false;
                validateError += validate_ChgHist.errorMessage ?? "";

                if (activeTabNameAfterValidation == "" && !validate_ChgHist.isValid) {
                    // DetailController.message.showWarning("<pre>" + validateError + "</pre>");
                    // $('a[href="#changeHistory"]').click();
                    activeTabNameAfterValidation = "#changeHistory";
                }

                // If validation is passed
                if (saveValidation) {
                    $(window).unbind('beforeunload');
                    MICore.Notification.whileSaving('Saving changes', function () {
                        setTimeout(function () {
                            $(function () {
                                var showHistoricalUpdateButton = $("#showHistoricalUpdateButton").val();
                                if (showHistoricalUpdateButton =="True") {
                                    DetailController.methods.updateInsertHistoric();
                                }
                                else {
                                    DetailController.methods.updateInsert();
                                }
                            
                            });
                        }, 1000)
                    });
                }
                else {
                    DetailController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + validateError.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>");
                    $('a[href="' + activeTabNameAfterValidation + '"]').click();
                }
            }
        });

        $("#btnGoToEdit").click(function () {
            /*var pims_id = MIApp.Sanitize.string($("#txtPIMSID").val()); // fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/85 MFQ 7/17/2023*/
            var pims_id = $("#txtPIMSID").val();
            var EPALVersionDt = $("#txtEPALVersionDt").val();
            DetailController.redirect.redirect_editRecordDetail(pims_id + ',' + kendo.toString(kendo.parseDate(EPALVersionDt), 'MM-dd-yyyy hh:mm:ss tt'));
        });

        $("#btnReset").click(function () {
            DetailController.message.showReset();    
        });

        $("#btnDeleteRecord").on('click', function () {
            // If validation is passed
            if ($("#txtCRNumber").val() != "" && $("#txtDeleleReason").val() != "") {
                $(window).unbind('beforeunload');
                MICore.Notification.whileSaving('Saving changes', function () {
                    setTimeout(function () {
                        $(function () {
                            DetailController.methods.delete();
                        });
                    }, 1000)
                });
            }            
        });

        /*========================
           Disable Save and Reset Buttons
        ========================*/
        DetailController.dirtyform.disablebuttons();

        /*========================
            Dirty form check hack on change
        ========================*/
        $('#editForm').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#addForm').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#duplicateForm').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#grid_StateInfo').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#grid_DiagCodes').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#grid_RevCodes').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#grid_AllocatedPlaces').change(function () {
            DetailController.dirtyform.dirtycheck();
        });

        $('#grid_Modifiers').change(function () {
            DetailController.dirtyform.dirtycheck();
        });      

        setTimeout(function () {
            DetailController.drivingStatus.renderCurrent();
        }, 500);                

        DetailController.methods.prgMgdByRequired();

        function renderSuspensionFieldsEnDisable() {
            if ($('#txtSuspensionInd').val() != 'Yes') return;

            switch ($("#ddlSUSPType").val()) {
                case "PA":
                    $("#dpPAEffectiveDate").prop('disabled', true);
                    $("#dpPAExpirationDate").prop('disabled', true);
                    break;
                case "PRE-D":
                    $("#dpPreDeterminationEffDate").prop('disabled', true);
                    $("#dpPreDeterminationExpDate").prop('disabled', true);
                    break;
                case "DRAL":
                    $("#dpDRALEffDate").prop('disabled', true);
                    $("#dpDRALExpDate").prop('disabled', true);
                    break;
                case "AN":
                    $("#dpAdvNotificationEffDate").prop('disabled', true);
                    $("#dpAdvNotificationExpDate").prop('disabled', true);
                    break;
                case "AA":
                    $("#dpAAEffectiveDate").prop('disabled', true);
                    $("#dpAAExpirationDate").prop('disabled', true);
                    break;
                case "MSP":
                    $("#dpMSPEffectiveDate").prop('disabled', true);
                    $("#dpMSPExpirationDate").prop('disabled', true);
                    break;
                default:
                    break;
            }
        }

        renderSuspensionFieldsEnDisable();
    },

    datePicker: {
        enabledPAExpireDate: function () {
            var datepicker = $("#dpPAExpirationDate").data("kendoDatePicker");            
            datepicker.enable(true);
        },
        disablePAExpireDate: function () {
            var datepicker = $("#dpPAExpirationDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enablePreDetermineExpireDate: function () {
            var datepicker = $("#dpPreDeterminationExpDate").data("kendoDatePicker");
            datepicker.enable(true);
        },
        disablePreDetermineExpireDate: function () {
            var datepicker = $("#dpPreDeterminationExpDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enableAdvancedNotificationExpireDate: function () {
            var datepicker = $("#dpAdvNotificationExpDate").data("kendoDatePicker");
            datepicker.enable(true);
        },
        disableAdvancedNotificationExpireDate: function () {
            var datepicker = $("#dpAdvNotificationExpDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enableSOSExpireDate: function () {
            var datepicker = $("#dpSOSExpirationDate").data("kendoDatePicker");
            datepicker.enable(true);
        },
        disableSOSExpireDate: function () {
            var datepicker = $("#dpSOSExpirationDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enableDRALExpireDate: function () {
            var datepicker = $("#dpDRALExpDate").data("kendoDatePicker");
            datepicker.enable(true);
        },
        disableDRALExpireDate: function () {
            var datepicker = $("#dpDRALExpDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enableAAExpireDate: function () {
            var datepicker = $("#dpAAExpirationDate").data("kendoDatePicker");
            datepicker.enable(true);
        },
        disableAAExpireDate: function () {
            var datepicker = $("#dpAAExpirationDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);
        },

        enableMSPExpireDate: function () {
            var datepicker = $("#dpMSPExpirationDate").data("kendoDatePicker");
            if (datepicker != undefined)
                datepicker.enable(true);

            //Bug 125193 MFQ 2/18/2025
            $("#ddlTypeofSpecialProcessing").data("kendoDropDownList").enable(true);
        },
        disableMSPExpireDate: function () {
            var datepicker = $("#dpMSPExpirationDate").data("kendoDatePicker");
            datepicker.value('')
            datepicker.enable(false);

            //Bug 125193 MFQ 2/18/2025
            $("#ddlTypeofSpecialProcessing").data("kendoDropDownList").enable(false);
            $("#ddlTypeofSpecialProcessing").data("kendoDropDownList").value('');
        },
    },
    dropdown: {
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
                    P_DRUG_NM: DetailController.dropdown.param.getDrugName(),
                    P_PROC_CD: $('#txtProcedureCode').val(),
                    P_ALTERNATE_CATEGORY: null
                };
            },
            ALTRNT_SVC_SUBCAT: function () {
                return {
                    P_DRUG_NM: DetailController.dropdown.param.getDrugName(),
                    P_PROC_CD: $('#txtProcedureCode').val(),
                    P_ALTERNATE_CATEGORY: $("#ddlAlternateCategory").data("kendoDropDownList").text()
                };
            },
            getDrugName: function () {
                var drug_no = $('#txtDrugName').val();
                if (drug_no.indexOf('~') > -1) {
                    drug_no = '~' + drug_no.split('~').slice(1);
                }
                return drug_no;
            },
            SUSP_IND: function () {
                return {
                    p_VV_SET_NAME: "SUSP_IND",
                    p_BUS_SEG_CD: null
                }
            },
            SUSP_TYPE: function () {
                return {
                    p_VV_SET_NAME: "SUSP_TYPE",
                    p_BUS_SEG_CD: null
                }
            }
        },
        //USER STORY 34764 Comment out MFQ 9/23/2022
        /*SOS_INDChange: function (e) {
            if ($("#ddlSiteofServiceApplies").data('kendoDropDownList').value().toUpperCase() == 'YES') {
                $('.label-sos-effective-date').addClass('required');

                // Enable SOSEffectiveDate
                $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(true);

                if ($("#dpSOSEffectiveDate").val() != "") {// Enable all other SOS fields
                    $("#dpSOSExpirationDate").data("kendoDatePicker").enable(true);
                    $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                    $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                    $("input[name='sossite']").each(function (i) {
                        $(this).attr('disabled', false);
                    })
                }
            } else {
                $('.label-sos-effective-date').removeClass('required');

                // Clear all othere SOS fields
                $("#dpSOSEffectiveDate").data("kendoDatePicker").value(null);
                $("#dpSOSExpirationDate").data("kendoDatePicker").value(null);
                $("#ddlURGCategory").data("kendoDropDownList").select(0);
                $("#ddlSOSTypeReview").data("kendoDropDownList").select(0);
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('checked', false);
                })

                // Disable all other SOS fields
                $("#dpSOSEffectiveDate").data("kendoDatePicker").enable(false);
                $("#dpSOSExpirationDate").data("kendoDatePicker").enable(false);
                $("#ddlURGCategory").data("kendoDropDownList").enable(false);
                $("#ddlSOSTypeReview").data("kendoDropDownList").enable(false);
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('disabled', 'disabled');
                })
            }
        },*/
        SOS_INDDataBound: function (e) {
            DetailController.dropdown.SOS_INDChange();
        },
        ALTRNT_SVC_CAT_Change: function (e) {
            if ($("#ddlAlternateCategory").data("kendoDropDownList").text() == '' || $("#ddlAlternateCategory").data("kendoDropDownList").text() == '---Select Category---')
                $('#ddlAlternateSubCategory').data("kendoDropDownList").enable(false);
            else
                $('#ddlAlternateSubCategory').data("kendoDropDownList").enable(true);
        },
        ALTRNT_SVC_SUBCAT_Change: function () {
            /*if ($('#txtDrugName').val().indexOf('~') > -1 && !addDetail) {
                $('#ddlAlternateSubCategory').data("kendoDropDownList").enable(false);
            }*/
        },
        ALTRNT_SVC_SUBCAT_DataBound: function () {
            // Setting value to cascaded dropdown with default value at the load of the page
            if ($('#ddlAlternateSubCategoryFirstLoadValue').val().length > 0) {
                $('#ddlAlternateSubCategory').data("kendoDropDownList").value($('#ddlAlternateSubCategoryFirstLoadValue').val());
                $('#ddlAlternateSubCategoryFirstLoadValue').val('');
            }
        },
        ALTRNT_SVC_CATDataBound: function (e) {            

            /**
             * This is implemented to search ALT CATEGORY regardless of Case sensitiveness.
             */
            var i = 1;
            $($("#ddlAlternateCategory").data("kendoDropDownList").dataItems()).each(function () {
                if (this.CATEGORY.toLowerCase() == $('#ddlAlternateCategory_hidden').val().toLowerCase()) {
                    $("#ddlAlternateCategory").data("kendoDropDownList").select(i);
                }
                i++;
            });

        }
    },
    dirtyform: {
        dirtycheck: function () {

            if (addDetail) {
                form_new_data = $('#addForm').serialize();
            }
            else if (editDetail) {
                form_new_data = $('#editForm').serialize();
            }
            else if (duplicateDetail) {
                form_new_data = $('#duplicateForm').serialize();
            }

            form_new_data += DetailController.dirtyform.get_grid_ProgMgdBy_Data();
            form_new_data += DetailController.dirtyform.get_grid_StateInfo_Data();
            form_new_data += DetailController.dirtyform.get_grid_DiagCodes_Data();
            form_new_data += DetailController.dirtyform.get_grid_RevCodes_Data();
            form_new_data += DetailController.dirtyform.get_grid_AllocatedPlaces_Data();
            form_new_data += DetailController.dirtyform.get_grid_Modifiers_Data();
            form_original_data += DetailController.dirtyform.get_grid_Retention_Data();
            form_original_data += DetailController.dirtyform.get_grid_Reduction_Data();

            if (form_new_data != form_original_data) {
                DetailController.dirtyform.enablebuttons();
                isDirty = true;
            }
            else if (form_new_data === form_original_data) {
                DetailController.dirtyform.disablebuttons();
                isDirty = false;
            }        
        },

        get_grid_ProgMgdBy_Data: function () {
            var grid_ProgMgdBy = $("#grid_ProgMgdBy").data("kendoGrid").dataSource.data();
            return "&grid_ProgMgdBy=" + JSON.stringify(grid_ProgMgdBy);
        },

        get_grid_StateInfo_Data: function () {
            var grid_StateInfo = $("#grid_StateInfo").data("kendoGrid").dataSource.data();
            return "&grid_StateInfo=" + JSON.stringify(grid_StateInfo);
        },

        get_grid_DiagCodes_Data: function () {
            var grid_DiagCodes = $("#grid_DiagCodes").data("kendoGrid").dataSource.data();
            return "&grid_DiagCodes=" + JSON.stringify(grid_DiagCodes);
        },

        get_grid_RevCodes_Data: function () {
            var grid_RevCodes = $("#grid_RevCodes").data("kendoGrid").dataSource.data();
            return "&grid_RevCodes=" + JSON.stringify(grid_RevCodes);
        },

        get_grid_AllocatedPlaces_Data: function () {
            var grid_AllocatedPlaces = $("#grid_AllocatedPlaces").data("kendoGrid").dataSource.data();
            return "&grid_AllocatedPlaces=" + JSON.stringify(grid_AllocatedPlaces);
        },

        get_grid_Modifiers_Data: function () {
            var grid_Modifiers = $("#grid_Modifiers").data("kendoGrid").dataSource.data();
            return "&grid_Modifiers=" + JSON.stringify(grid_Modifiers);
        },

        get_grid_Retention_Data: function () {
            var grid_Retention = $("#grid_Retention").data("kendoGrid").dataSource.data();
            return "&grid_Retention=" + JSON.stringify(grid_Retention);
        },

        get_grid_Reduction_Data: function () {
            var grid_Reduction = $("#grid_Reduction").data("kendoGrid").dataSource.data();
            return "&grid_Reduction=" + JSON.stringify(grid_Reduction);
        },

        disablebuttons: function () {
            if (addDetail) {
                $('#addForm').find('#btnSave').attr('disabled', 'disabled');
                $('#addForm').find('#btnReset').attr('disabled', 'disabled');
            }
            else if (editDetail) {
                $('#editForm').find('#btnSave').attr('disabled', 'disabled');
                $('#editForm').find('#btnHistoricalSave').attr('disabled', 'disabled');            
                $('#editForm').find('#btnReset').attr('disabled', 'disabled');
            }
            else if (duplicateDetail) {
                $('#duplicateForm').find('#btnSave').attr('disabled', 'disabled');
                $('#duplicateForm').find('#btnReset').attr('disabled', 'disabled');
            }
        },

        enablebuttons: function () {
            if (addDetail) {
                $('#addForm').find('#btnSave').removeAttr('disabled', 'disabled');
                $('#addForm').find('#btnReset').removeAttr('disabled', 'disabled');
            }
            else if (editDetail) {
                $('#editForm').find('#btnSave').removeAttr('disabled', 'disabled');
                $('#editForm').find('#btnHistoricalSave').removeAttr('disabled', 'disabled');   
                $('#editForm').find('#btnReset').removeAttr('disabled', 'disabled');
            }
            else if (duplicateDetail) {
                $('#duplicateForm').find('#btnSave').removeAttr('disabled', 'disabled');
                $('#duplicateForm').find('#btnReset').removeAttr('disabled', 'disabled');
            }
        }
    },
    methods: {
        delete: function () {    
            var PIMS_ID = MIApp.Sanitize.string($("#txtPIMSID").val());
            if (PIMS_ID.indexOf(" ") >= 0) {
                PIMS_ID = PIMS_ID.substring(0, PIMS_ID.indexOf(' '))
            }
            var obj = {
                "P_EPAL_HIERARCHY_KEY": PIMS_ID,
                "P_CHANGE_REQ_ID": $("#txtCRNumber").val(),
                "P_CHANGE_DESC": $("#txtDeleleReason").val(),
                "P_EPAL_VER_EFF_DT": $("#txtEPALVersionDt").val()
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/EPAL/Home/DeleteEpal",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function () {
                    Swal.fire({
                        title: 'Success!',
                        text: 'PIMS record has been deleted!',
                        icon: 'success',
                        showCancelButton: false,
                        allowOutsideClick: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            Swal.fire({
                                title: 'Loading..',
                                text: 'Redirecting, please wait..',
                                timerProgressBar: true,
                                //timer: 5000,
                                timer: 60000,
                                allowOutsideClick: false,
                                willOpen: () => {
                                    Swal.showLoading();
                                    window.location.href = VIRTUAL_DIRECTORY + "/EPAL" ;
                                }
                            })
                        }


                    })
                },
                error: function () {
                    DetailController.message.showError();
                }
            })
        },
        updateInsert: function () {
            var EPAL_BUS_SEG_CD = "";
            var EPAL_ENTITY_CD = "";
            var EPAL_PLAN_CD = "";
            var EPAL_PRODUCT_CD = "";
            var EPAL_FUND_ARNGMNT_CD = "";
            var PROC_CD = "";
            var DRUG_NM = "";


            if ($("#duplicate-record").val() == "Y") {
                /*==================================
                    Get values from _DetailHeader_DuplicateRecord.cshtml if Duplicating a record. 
                 ==================================*/
                EPAL_BUS_SEG_CD = $("#ddlLOB").val();
                EPAL_ENTITY_CD = $("#ddlEntity").val();
                EPAL_PLAN_CD = $("#ddlPlan").val();
                EPAL_PRODUCT_CD = $("#ddlProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
            }


            else {
                EPAL_BUS_SEG_CD = $("#txtBusinessSegment").val();
                EPAL_ENTITY_CD = $("#txtEntity").val();
                EPAL_PLAN_CD = $("#txtPlan").val();
                EPAL_PRODUCT_CD = $("#txtProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#txtFunding").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
            }
                

            //Added EPAL Review Logic. 
            var DateOfPreviousReview_Before = $("#txtDateOfRecentReview_Before").val();
            var P_EPAL_PREV_REVIEW_DT = $("#txtDateOfPreviousReview").val();
            var P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();

            if (DateOfPreviousReview_Before != P_EPAL_CURR_REVIEW_DT) {
                P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();
                P_EPAL_PREV_REVIEW_DT = DateOfPreviousReview_Before;
            }
            else if (DateOfPreviousReview_Before == P_EPAL_CURR_REVIEW_DT) {
                P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();
                P_EPAL_PREV_REVIEW_DT = $("#txtDateOfPreviousReview").val();
            }
            
            //console.log(DiagListController.grid.row_count())

            //Edit PIMS ID to not take any characters after space in drug name. 
            var PIMS_ID = MIApp.Sanitize.string($("#txtPIMSID").val());
            if (PIMS_ID.indexOf(" ") >= 0) {
                PIMS_ID = PIMS_ID.substring(0, PIMS_ID.indexOf(' '))
            }

            var obj = {
                "P_EPAL_BUS_SEG_CD": EPAL_BUS_SEG_CD,
                "P_EPAL_ENTITY_CD": EPAL_ENTITY_CD,
                "P_EPAL_PLAN_CD": EPAL_PLAN_CD,
                "P_EPAL_PRODUCT_CD": MIApp.Sanitize.encodeProp(EPAL_PRODUCT_CD),
                "P_EPAL_FUND_ARNGMNT_CD": EPAL_FUND_ARNGMNT_CD,
                "P_PROC_CD": MIApp.Sanitize.encodeProp(PROC_CD),
                "P_DRUG_NM": DRUG_NM.toUpperCase().trim(),
                "P_DRUG_RVW_AT_LAUNCH_IND": $("#txtDrugReviewAtLaunch").val(),
                "P_PRIOR_AUTH_EFF_DT": $("#dpPAEffectiveDate").val(),
                "P_PRIOR_AUTH_EXP_DT": $("#dpPAExpirationDate").val(),
                "P_PRIOR_AUTH_AGE_MIN": $("#ddlPAMinAge").val() == '' ? 0 : $("#ddlPAMinAge").val(),
                "P_PRIOR_AUTH_AGE_MAX": $("#ddlPAMaxAge").val() == '' ? 0 : $("#ddlPAMaxAge").val(),
                "P_AUTO_APRVL_EFF_DT": $("#dpAAEffectiveDate").val(),
                "P_AUTO_APRVL_EXP_DT": $("#dpAAExpirationDate").val(),
                "P_MCARE_SPCL_PRCSNG_TYPE": $("#ddlTypeofSpecialProcessing").val(),
                "P_MCARE_SPCL_PRCSNG_EFF_DT": $("#dpMSPEffectiveDate").val(),
                "P_MCARE_SPCL_PRCSNG_EXP_DT": $("#dpMSPExpirationDate").val(),
                "P_SOS_IND": $("#txtSiteofServiceApplies").val(),
                "P_SVC_CAT": $("#ddlCategory").val(),
                "P_SVC_SUBCAT": $("#ddlSubCategory").val(),
                "P_HCE_REP_CAT": $("#ddlHCEMICategory").val(),
                "P_PROG_MGD_BY": $("#ddlProgramManagedBy").val(),
                "P_DELEGATED_UM": $("#ddlDelegatedUM").val(),
                "P_FURTHER_INST": MIApp.Sanitize.encodeProp($("#txtFurtherConsideration").val()),
                "P_NOTES": MIApp.Sanitize.encodeProp($("#txtNote").val()),
                "P_USER_ID": null,
                "P_CHANGE_REQ_ID": MIApp.Sanitize.encodeProp($("#txtChangeSource").val()),
                "P_CHANGE_DESC": MIApp.Sanitize.encodeProp($("#txtChangeDesc").val()),
                "P_STATE_CD": StateInfoController.data.STATE_CD,
                "P_STATE_MANDATED_IND": StateInfoController.data.STATE_MANDATED_IND,
                "P_ATS_INCL_EXCL_CD": StateInfoController.data.INCL_EXCL_CD,                
                "P_ATS_EFF_DT": MIApp.Sanitize.encodeProp(StateInfoController.data.ATS_EFF_DT),
                "P_ATS_EXP_DT": MIApp.Sanitize.encodeProp(StateInfoController.data.ATS_EXP_DT),
                "P_ATS_ISSUE_GOV": StateInfoController.data.ATS_ISSUE_GOV,
                "P_ATS_PROG_MGD_BY": StateInfoController.data.ATS_PROG_MGD_BY,
                /*      "P_DIAG_CD": DiagCodesController.data.DIAG_CD.startsWith(",") ? DiagCodesController.data.DIAG_CD.substring(1): DiagCodesController.data.DIAG_CD,*/
                "P_DIAG_CD": DiagListController.grid.row_count() == 0 && $("#txtDiagListCount").val() > 0 ? null : DiagCodesController.data.DIAG_CD.startsWith(",") ? DiagCodesController.data.DIAG_CD.substring(1) : DiagCodesController.data.DIAG_CD,
                "P_DIAG_INCL_EXCL_CD": DiagListController.data.INCL_EXCL_CD == "" ? DiagCodesController.data.INCL_EXCL_CD : DiagListController.data.INCL_EXCL_CD + "," + DiagCodesController.data.INCL_EXCL_CD,
                "P_DIAG_PROG_MGD_BY": DiagListController.data.PROG_MGD_BY == "" ? DiagCodesController.data.PROG_MGD_BY : DiagListController.data.PROG_MGD_BY + "," + DiagCodesController.data.PROG_MGD_BY, 
                "P_MODIFIER": ModifiersController.data.MODIFIER.startsWith(",") ? ModifiersController.data.MODIFIER.substring(1) : ModifiersController.data.MODIFIER,
                "P_MOD_INCL_EXCL_CD": ModifiersController.data.INCL_EXCL_CD,
                "P_PLC_OF_SVC_CD": AllocatedPlacesController.data.ALLWD_PLC_OF_SVC.startsWith(",") ? AllocatedPlacesController.data.ALLWD_PLC_OF_SVC.substring(1) : AllocatedPlacesController.data.ALLWD_PLC_OF_SVC,
                "P_POS_INCL_EXCL_CD": AllocatedPlacesController.data.INCL_EXCL_CD,
                "P_REV_CD": RevCodesController.data.REV_CD.startsWith(",") ? RevCodesController.data.REV_CD.substring(1) : RevCodesController.data.REV_CD,
                "P_REV_INCL_EXCL_CD": RevCodesController.data.INCL_EXCL_CD,
                "P_EPAL_CURR_REVIEW_DT": P_EPAL_CURR_REVIEW_DT,
                "P_EPAL_PREV_REVIEW_DT": P_EPAL_PREV_REVIEW_DT,
                "P_SOS_EFF_DT": $("#dpSOSEffectiveDate").val(),
                "P_SOS_EXP_DT": $("#dpSOSExpirationDate").val(),
                "P_SOS_TYPE": $("#ddlSOSTypeReview").val(),
                "P_SOS_SITE_IND": $('input[name=sossite]:checked').val(),
                "P_SOS_URG_CAT_MDLTY": $("#ddlURGCategory").val(),
                "P_PMB_EFF_DT": ProgMgdByController.data.PMB_EFF_DT,
                "P_PMB_EXP_DT": ProgMgdByController.data.PMB_EXP_DT,
                "P_PMB_PROG_MGD_BY": ProgMgdByController.data.PROG_MGD_BY,
                "P_LIST_NAME": DiagListController.data.DIAG_LIST,
                "P_PMB_DELEGATED_UM": ProgMgdByController.data.DELEGATED_UM,
                "P_PMB_BASED_ON_DX_IND": ProgMgdByController.data.PMB_BASED_ON_DX_IND,
                "P_PMB_BASED_ON_ST_APP_IND": ProgMgdByController.data.PMB_BASED_ON_ST_APP_IND,
                "P_PMB_BASED_ON_AGE_MIN": ProgMgdByController.data.PMB_BASED_ON_AGE_MIN == '' ? 0 : ProgMgdByController.data.PMB_BASED_ON_AGE_MIN,
                "P_PMB_BASED_ON_AGE_MAX": ProgMgdByController.data.PMB_BASED_ON_AGE_MAX == '' ? 0 : ProgMgdByController.data.PMB_BASED_ON_AGE_MAX,
                "P_EPAL_HIERARCHY_KEY": PIMS_ID,
                "EPALPageView": $('#epal-page-view').val(),
                "P_PRE_DET_EFF_DT": $('#dpPreDeterminationEffDate').val(), // USER STORY 54535 MFQ 03/31/2023
                "P_PRE_DET_EXP_DT": $('#dpPreDeterminationExpDate').val(), // USER STORY 54535 MFQ 03/31/2023
                "P_ALTRNT_SVC_CAT": $("#ddlAlternateCategory").data("kendoDropDownList").text() == '---Select Category---' ? '' : $("#ddlAlternateCategory").data("kendoDropDownList").text(),   // USER STORY 56704 MFQ 04/20/2023
                "P_ALTRNT_SVC_SUBCAT": $('#ddlAlternateSubCategory').val(),    // USER STORY 56704 MFQ 04/20/2023 
                "P_FACTOR_TYPE": _RetRedFactorController.data.FACTOR_TYPE,
                "P_FACTOR_NM": _RetRedFactorController.data.FACTOR_NM,
                "P_FACTOR_EFF_DT": MIApp.Sanitize.encodeProp(_RetRedFactorController.data.FACTOR_EFF_DT),
                "P_FACTOR_EXP_DT": MIApp.Sanitize.encodeProp(_RetRedFactorController.data.FACTOR_EXP_DT),
                "P_FACTOR_NOTES": _RetRedFactorController.data.FACTOR_NOTES,
                "P_ADV_NTFCTN_EFF_DT": $('#dpAdvNotificationEffDate').val(),
                "P_ADV_NTFCTN_EXP_DT": $('#dpAdvNotificationExpDate').val(),
                "P_DRAL_EFF_DT": $('#dpDRALEffDate').val(),
                "P_DRAL_EXP_DT": $('#dpDRALExpDate').val()
            }

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/EPAL/Home/UpdateInsert",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    "RequestVerificationToken": $("#RequestVerificationToken").val()
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (updateTO) {
                    if (updateTO.StatusID == 2) {
                        Swal.fire({
                            title: 'Partially updated!',
                            text: 'Record partially updated, please check with administrator for detail',
                            icon: 'warning',
                            showCancelButton: false,
                            allowOutsideClick: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                Swal.fire({
                                    title: 'Loading..',
                                    text: 'Reloading record, please wait..',
                                    timerProgressBar: true,
                                    //timer: 5000,
                                    timer: 60000,
                                    allowOutsideClick: false,
                                    willOpen: () => {
                                        Swal.showLoading();
                                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + PIMS_ID;
                                    }
                                })
                            }
                        });
                    } else if (updateTO.StatusID == 1) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'PIMS record has been updated!',
                            icon: 'success',
                            showCancelButton: false,
                            allowOutsideClick: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                Swal.fire({
                                    title: 'Loading..',
                                    text: 'Reloading record, please wait..',
                                    timerProgressBar: true,
                                    //timer: 5000,
                                    timer: 60000,
                                    allowOutsideClick: false,
                                    willOpen: () => {
                                        Swal.showLoading();
                                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + PIMS_ID;
                                    }
                                })
                            }
                        });
                    } else {
                        //MICore.Notification.error('Error occured!', 'Error occured while saving the record, please check with administrator!', null);
                        MICore.Notification.error('Error occured!', 'Error occured while saving the record, please check with administrator! \n' + (updateTO.Message || ''), null);
                    }
                },
                error: function () {
                    DetailController.message.showError();
                }
            })
        },
        updateInsertHistoric: function () {
            var EPAL_BUS_SEG_CD = "";
            var EPAL_ENTITY_CD = "";
            var EPAL_PLAN_CD = "";
            var EPAL_PRODUCT_CD = "";
            var EPAL_FUND_ARNGMNT_CD = "";
            var PROC_CD = "";
            var DRUG_NM = "";


            if ($("#duplicate-record").val() == "Y") {
                /*==================================
                    Get values from _DetailHeader_DuplicateRecord.cshtml if Duplicating a record. 
                 ==================================*/
                EPAL_BUS_SEG_CD = $("#ddlLOB").val();
                EPAL_ENTITY_CD = $("#ddlEntity").val();
                EPAL_PLAN_CD = $("#ddlPlan").val();
                EPAL_PRODUCT_CD = $("#ddlProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
            }


            else {
                EPAL_BUS_SEG_CD = $("#txtBusinessSegment").val();
                EPAL_ENTITY_CD = $("#txtEntity").val();
                EPAL_PLAN_CD = $("#txtPlan").val();
                EPAL_PRODUCT_CD = $("#txtProduct").val();
                EPAL_FUND_ARNGMNT_CD = $("#txtFunding").val();
                PROC_CD = $("#txtProcedureCode").val();
                DRUG_NM = $("#txtDrugName").val();
            }


            //Added EPAL Review Logic. 
            var DateOfPreviousReview_Before = $("#txtDateOfRecentReview_Before").val();
            var P_EPAL_PREV_REVIEW_DT = $("#txtDateOfPreviousReview").val();
            var P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();

            if (DateOfPreviousReview_Before != P_EPAL_CURR_REVIEW_DT) {
                P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();
                P_EPAL_PREV_REVIEW_DT = DateOfPreviousReview_Before;
            }
            else if (DateOfPreviousReview_Before == P_EPAL_CURR_REVIEW_DT) {
                P_EPAL_CURR_REVIEW_DT = $("#dpDateOfRecentReview").val();
                P_EPAL_PREV_REVIEW_DT = $("#txtDateOfPreviousReview").val();
            }

            console.log(DiagListController.grid.row_count())

            //Edit PIMS ID to not take any characters after space in drug name. 
            var PIMS_ID = MIApp.Sanitize.string($("#txtPIMSID").val());
            if (PIMS_ID.indexOf(" ") >= 0) {
                PIMS_ID = PIMS_ID.substring(0, PIMS_ID.indexOf(' '))
            }

            var obj = {
                "P_EPAL_BUS_SEG_CD": EPAL_BUS_SEG_CD,
                "P_EPAL_ENTITY_CD": EPAL_ENTITY_CD,
                "P_EPAL_PLAN_CD": EPAL_PLAN_CD,
                "P_EPAL_PRODUCT_CD": MIApp.Sanitize.encodeProp(EPAL_PRODUCT_CD),
                "P_EPAL_FUND_ARNGMNT_CD": EPAL_FUND_ARNGMNT_CD,
                "P_PROC_CD": MIApp.Sanitize.encodeProp(PROC_CD),
                "P_DRUG_NM": DRUG_NM.toUpperCase().trim(),
                "P_DRUG_RVW_AT_LAUNCH_IND": $("#txtDrugReviewAtLaunch").val(),
                "P_PRIOR_AUTH_EFF_DT": $("#dpPAEffectiveDate").val(),
                "P_PRIOR_AUTH_EXP_DT": $("#dpPAExpirationDate").val(),
                "P_PRIOR_AUTH_AGE_MIN": $("#ddlPAMinAge").val(),
                "P_PRIOR_AUTH_AGE_MAX": $("#ddlPAMaxAge").val(),
                "P_AUTO_APRVL_EFF_DT": $("#dpAAEffectiveDate").val(),
                "P_AUTO_APRVL_EXP_DT": $("#dpAAExpirationDate").val(),
                "P_MCARE_SPCL_PRCSNG_TYPE": $("#ddlTypeofSpecialProcessing").val(),
                "P_MCARE_SPCL_PRCSNG_EFF_DT": $("#dpMSPEffectiveDate").val(),
                "P_MCARE_SPCL_PRCSNG_EXP_DT": $("#dpMSPExpirationDate").val(),
                "P_SOS_IND": $("#txtSiteofServiceApplies").val(),
                "P_SVC_CAT": $("#ddlCategory").val(),
                "P_SVC_SUBCAT": $("#ddlSubCategory").val(),
                "P_HCE_REP_CAT": $("#ddlHCEMICategory").val(),
                "P_PROG_MGD_BY": $("#ddlProgramManagedBy").val(),
                "P_DELEGATED_UM": $("#ddlDelegatedUM").val(),
                "P_FURTHER_INST": MIApp.Sanitize.encodeProp($("#txtFurtherConsideration").val()),
                "P_NOTES": MIApp.Sanitize.encodeProp($("#txtNote").val()),
                "P_USER_ID": null,
                "P_CHANGE_REQ_ID": MIApp.Sanitize.encodeProp($("#txtChangeSource").val()),
                "P_CHANGE_DESC": MIApp.Sanitize.encodeProp($("#txtChangeDesc").val()),
                "P_STATE_CD": StateInfoController.data.STATE_CD,
                "P_STATE_MANDATED_IND": StateInfoController.data.STATE_MANDATED_IND,
                "P_ATS_INCL_EXCL_CD": StateInfoController.data.INCL_EXCL_CD,
                "P_ATS_EFF_DT": StateInfoController.data.ATS_EFF_DT,
                "P_ATS_EXP_DT": StateInfoController.data.ATS_EXP_DT,
                "P_ATS_ISSUE_GOV": StateInfoController.data.ATS_ISSUE_GOV,
                "P_ATS_PROG_MGD_BY": StateInfoController.data.ATS_PROG_MGD_BY,
                /*      "P_DIAG_CD": DiagCodesController.data.DIAG_CD.startsWith(",") ? DiagCodesController.data.DIAG_CD.substring(1): DiagCodesController.data.DIAG_CD,*/
                "P_DIAG_CD": DiagListController.grid.row_count() == 0 && $("#txtDiagListCount").val() > 0 ? null : DiagCodesController.data.DIAG_CD.startsWith(",") ? DiagCodesController.data.DIAG_CD.substring(1) : DiagCodesController.data.DIAG_CD,
                "P_DIAG_INCL_EXCL_CD": DiagListController.data.INCL_EXCL_CD == "" ? DiagCodesController.data.INCL_EXCL_CD : DiagListController.data.INCL_EXCL_CD + "," + DiagCodesController.data.INCL_EXCL_CD,
                "P_DIAG_PROG_MGD_BY": DiagListController.data.PROG_MGD_BY == "" ? DiagCodesController.data.PROG_MGD_BY : DiagListController.data.PROG_MGD_BY + "," + DiagCodesController.data.PROG_MGD_BY,
                "P_MODIFIER": ModifiersController.data.MODIFIER.startsWith(",") ? ModifiersController.data.MODIFIER.substring(1) : ModifiersController.data.MODIFIER,
                "P_MOD_INCL_EXCL_CD": ModifiersController.data.INCL_EXCL_CD,
                "P_PLC_OF_SVC_CD": AllocatedPlacesController.data.ALLWD_PLC_OF_SVC.startsWith(",") ? AllocatedPlacesController.data.ALLWD_PLC_OF_SVC.substring(1) : AllocatedPlacesController.data.ALLWD_PLC_OF_SVC,
                "P_POS_INCL_EXCL_CD": AllocatedPlacesController.data.INCL_EXCL_CD,
                "P_REV_CD": RevCodesController.data.REV_CD.startsWith(",") ? RevCodesController.data.REV_CD.substring(1) : RevCodesController.data.REV_CD,
                "P_REV_INCL_EXCL_CD": RevCodesController.data.INCL_EXCL_CD,
                "P_EPAL_CURR_REVIEW_DT": P_EPAL_CURR_REVIEW_DT,
                "P_EPAL_PREV_REVIEW_DT": P_EPAL_PREV_REVIEW_DT,
                "P_SOS_EFF_DT": $("#dpSOSEffectiveDate").val(),
                "P_SOS_EXP_DT": $("#dpSOSExpirationDate").val(),
                "P_SOS_TYPE": $("#ddlSOSTypeReview").val(),
                "P_SOS_SITE_IND": $('input[name=sossite]:checked').val(),
                "P_SOS_URG_CAT_MDLTY": $("#ddlURGCategory").val(),
                "P_PMB_EFF_DT": ProgMgdByController.data.PMB_EFF_DT,
                "P_PMB_EXP_DT": ProgMgdByController.data.PMB_EXP_DT,
                "P_PMB_PROG_MGD_BY": ProgMgdByController.data.PROG_MGD_BY,
                "P_LIST_NAME": DiagListController.data.DIAG_LIST,
                "P_PMB_DELEGATED_UM": ProgMgdByController.data.DELEGATED_UM,
                "P_PMB_BASED_ON_DX_IND": ProgMgdByController.data.PMB_BASED_ON_DX_IND,
                "P_PMB_BASED_ON_ST_APP_IND": ProgMgdByController.data.PMB_BASED_ON_ST_APP_IND,
                "P_PMB_BASED_ON_AGE_MIN": ProgMgdByController.data.PMB_BASED_ON_AGE_MIN,
                "P_PMB_BASED_ON_AGE_MAX": ProgMgdByController.data.PMB_BASED_ON_AGE_MAX,
                "P_EPAL_HIERARCHY_KEY": PIMS_ID,
                "EPALPageView": $('#epal-page-view').val(),
                "P_PRE_DET_EFF_DT": $('#dpPreDeterminationEffDate').val(), // USER STORY 54535 MFQ 03/31/2023
                "P_PRE_DET_EXP_DT": $('#dpPreDeterminationExpDate').val(), // USER STORY 54535 MFQ 03/31/2023
                "P_ALTRNT_SVC_CAT": $("#ddlAlternateCategory").data("kendoDropDownList").text() == '---Select Category---' ? '' : $("#ddlAlternateCategory").data("kendoDropDownList").text(),   // User Story 129512 MFQ 3/7/2025, USER STORY 56704 MFQ 04/20/2023
                "P_ALTRNT_SVC_SUBCAT": $('#ddlAlternateSubCategory').val(),    // USER STORY 56704 MFQ 04/20/2023 
                "P_FACTOR_TYPE": _RetRedFactorController.data.FACTOR_TYPE,
                "P_FACTOR_NM": _RetRedFactorController.data.FACTOR_NM,
                "P_FACTOR_EFF_DT": _RetRedFactorController.data.FACTOR_EFF_DT,
                "P_FACTOR_EXP_DT": _RetRedFactorController.data.FACTOR_EXP_DT,
                "P_FACTOR_NOTES": _RetRedFactorController.data.FACTOR_NOTES,
                "P_EPAL_VER_EFF_DT": $("#txtEPALVersionDt").val(),
                "P_ADV_NTFCTN_EFF_DT": $('#dpAdvNotificationEffDate').val(),
                "P_ADV_NTFCTN_EXP_DT": $('#dpAdvNotificationExpDate').val(),
                "P_DRAL_EFF_DT": $('#dpDRALEffDate').val(),
                "P_DRAL_EXP_DT": $('#dpDRALExpDate').val()
            }

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/EPAL/Home/UpdateInsertHistoric",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    "RequestVerificationToken": $("#RequestVerificationToken").val()
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (updateTO) {
                    if (updateTO.StatusID == -2) {
                        Swal.fire({
                            title: 'Partially updated!',
                            text: 'Record partially updated, please check with administrator for detail',
                            icon: 'warning',
                            showCancelButton: false,
                            allowOutsideClick: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                Swal.fire({
                                    title: 'Loading..',
                                    text: 'Reloading record, please wait..',
                                    timerProgressBar: true,
                                    //timer: 5000,
                                    timer: 60000,
                                    allowOutsideClick: false,
                                    willOpen: () => {
                                        Swal.showLoading();
                                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + PIMS_ID + "," + kendo.toString(kendo.parseDate($("#txtEPALVersionDt").val()), 'MM-dd-yyyy hh:mm:ss tt');
                                    }
                                })
                            }
                        });
                        
                    } else if (updateTO.StatusID == 1) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'PIMS record has been updated!',
                            icon: 'success',
                            showCancelButton: false,
                            allowOutsideClick: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                Swal.fire({
                                    title: 'Loading..',
                                    text: 'Reloading record, please wait..',
                                    timerProgressBar: true,
                                    //timer: 5000,
                                    timer: 60000,
                                    allowOutsideClick: false,
                                    willOpen: () => {
                                        Swal.showLoading();
                                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + PIMS_ID + "," + kendo.toString(kendo.parseDate($("#txtEPALVersionDt").val()), 'MM-dd-yyyy hh:mm:ss tt');
                                    }
                                })
                            }
                        });
                    } else{
                        //MICore.Notification.error('Error occured!', 'Error occured while saving the record, please check with administrator!', null);
                        MICore.Notification.error('Error occured!', 'Error occured while saving the record, please check with administrator! \n' + (updateTO.Message || ''), null);
                    }
                },
                error: function () {
                    DetailController.message.showError();
                }
            })
        },
        focusPASummaryTab: function () {
            $('.tab-pane,.nav-link').removeClass('active show');
            $('#EPALSummary-tab').addClass('active');
            $('#EPALSummary').addClass('active show');
        },
        focusReviewIndicatorTab: function () {
            $('.tab-pane,.nav-link').removeClass('active show');
            $('#EPALReviewIndicators-tab').addClass('active');
            $('#EPALReviewIndicators').addClass('active show');
        },
        SOSEffectiveDateChange: function () {            
            if ($("#dpSOSEffectiveDate").val() != "") {// Enable all other SOS fields //USER STORY 34764 Comment out MFQ 9/23/2022 --> && $("#txtSiteofServiceApplies").val().toUpperCase() == 'YES'
                $("#txtSiteofServiceApplies").val('Yes').change();
                $("#dpSOSExpirationDate").data("kendoDatePicker").enable(true);
                $("#ddlURGCategory").data("kendoDropDownList").enable(true);
                $("#ddlSOSTypeReview").data("kendoDropDownList").enable(true);
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('disabled', false);
                });
                $('.sos-required-label').addClass('required'); //USER STORY 34764 Comment out MFQ 9/23/2022                
            } else {
                $("#txtSiteofServiceApplies").val('No').change();
                $("#ddlURGCategory").data("kendoDropDownList").enable(false);
                $("#ddlURGCategory").data("kendoDropDownList").value('');
                $("#ddlSOSTypeReview").data("kendoDropDownList").enable(false);
                $("#ddlSOSTypeReview").data("kendoDropDownList").value('');
                $("input[name='sossite']").each(function (i) {
                    $(this).attr('disabled', true);
                });
                $("[name='sossite']").prop('checked', false);
                $('.sos-required-label').removeClass('required'); //USER STORY 34764 Comment out MFQ 9/23/2022
            }
            DetailController.drivingStatus.renderCurrent(); // 
        },
        SOSExpirationDateChange: function () {
            var SOSEffectiveDate = $("#dpSOSEffectiveDate").val();
            var SOSExpirationDate = $("#dpSOSExpirationDate").val();

            if (SOSEffectiveDate != "" && SOSExpirationDate == "") {
                $("#txtSiteofServiceApplies").val('Yes').change();
            }

            else if (Date.parse(SOSExpirationDate) > Date.parse(TODAY_DATE)) {
                $("#txtSiteofServiceApplies").val('Yes').change();                
            }

            else if (Date.parse(SOSExpirationDate) <= Date.parse(TODAY_DATE)) {                
                $("#txtSiteofServiceApplies").val('No').change();
            }
            
            DetailController.drivingStatus.activate();
        },
        MSPEffectiveDateChange: function () {
            if ($("#dpMSPEffectiveDate").val() != "") {//USER STORY 34762 Comment out MFQ 9/23/2022                 
                $('#lblTypeofSpecialProcessing').addClass('required'); //USER STORY 34762 Comment out MFQ 9/23/2022
            } else {                
                $('#lblTypeofSpecialProcessing').removeClass('required'); //USER STORY 34762 Comment out MFQ 9/23/2022
            }
        }, 
        SOS: {
            changed: function () {
                var isSOSChanged =
                    convertDateMMddyyyy($('#dpSOSEffectiveDate').attr('ovalue')) != convertDateMMddyyyy($('#dpSOSEffectiveDate').val()) ||
                    convertDateMMddyyyy($('#dpSOSExpirationDate').attr('ovalue')) != convertDateMMddyyyy($('#dpSOSExpirationDate').val()) ||
                    $('#ddlURGCategory').attr('ovalue') != $('#ddlURGCategory').val() ||
                    $('#ddlSOSTypeReview').attr('ovalue') != $('#ddlSOSTypeReview').val() ||
                    ($('input[name=sossite]:checked').val() == undefined ? '' : $('input[name=sossite]:checked').val()) != $('#sossite-ovalue').val();

                if ($('#txtPriorAuth').val() == "No" && isSOSChanged) {
                    return true;
                } else {
                    return false;
                }
            },
            compulsoryFieldsMissing: function () {
                if ($('#dpSOSEffectiveDate').val() == '' ||
                    $('#dpSOSExpirationDate').val() == '' ||
                    $('#ddlURGCategory').val() == '' ||
                    $('#ddlSOSTypeReview').val() == '' ||
                    $('input[name=sossite]:checked').val() == '') {
                    return true;
                } else {
                    return false;
                }
            },
            revertBackToOriginalValues: function () {
                var dpSOSEffectiveDate_ovalue = convertDateMMddyyyy($('#dpSOSEffectiveDate').attr('ovalue'));
                var dpSOSExpirationDate_ovalue = convertDateMMddyyyy($('#dpSOSExpirationDate').attr('ovalue'));

                $('#txtSiteofServiceApplies').val($('#txtSiteofServiceApplies').attr('ovalue'));
                $('#dpSOSEffectiveDate').val(dpSOSEffectiveDate_ovalue);
                $('#dpSOSExpirationDate').val(dpSOSExpirationDate_ovalue);
                $('#ddlURGCategory').data("kendoDropDownList").value($('#ddlURGCategory').attr('ovalue'));
                $("#ddlSOSTypeReview").data("kendoDropDownList").value($('#ddlSOSTypeReview').attr('ovalue'));                
                $("input[name='sossite']").each(function (i) {
                    if ($(this).val() == $('#sossite-ovalue').val()) {
                        $(this).prop('checked', true);
                    } else {
                        $(this).prop('checked', false);
                    }
                })
            }
        },
        prgMgdByRequired: function () {
            if ($("#txtPriorAuth").val() == "Yes") {
                $("#programManagedBy-tab").addClass('required');
            }
            else if ($("#txtPriorAuth").val() == "No") {
                $("#programManagedBy-tab").removeClass('required');
            }
        }
    },
    redirect: {
        redirect_duplicateRecordDetail: function (pims_id) {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to leave?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Redirecting..',
                        text: 'Redirecting to detail page, please wait..',
                        //timer: 5000,
                        timer: 60000,
                        timerProgressBar: true,
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();
                            window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/DuplicateRecordDetail/" + encodeURIComponent(pims_id);
                        }
                    })
                }
            }
            else if (!isDirty) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to detail page, please wait..',
                    //timer: 5000,
                    timer: 60000,
                    timerProgressBar: true,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/DuplicateRecordDetail/" + encodeURIComponent(pims_id);
                    }
                })
            }
        },
        redirect_editRecordDetail: function (pims_id) {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to leave?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Redirecting..',
                        text: 'Redirecting to detail page, please wait..',
                        timer: 5000,
                        timerProgressBar: true,
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();                 
                            window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);
                        }
                    })
                }
            }
            else if (!isDirty) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to detail page, please wait..',
                    timerProgressBar: true,
                    //timer: 5000,
                    timer: 60000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);
                    }
                })
            }

        },
        redirect_viewDetail: function (pims_id) {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to leave?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Redirecting..',
                        text: 'Redirecting to detail page, please wait..',
                        timerProgressBar: true,
                        //timer: 5000,
                        timer: 60000,
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();
                            window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                        }
                    })
                }
            }
            else if (!isDirty) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to detail page, please wait..',
                    timerProgressBar: true,
                    //timer: 5000,
                    timer: 60000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                    }
                })
            }
        },
    },
    validation: {
        validate_PASummary: function () {
            var isValid = true;
            var errorMessage = "";
            var priorAuth = $("#txtPriorAuth").val();
            var dpPAEffectiveDate = Date.parse($("#dpPAEffectiveDate").val());
            var dpPAExpirationDate = $.trim($("#dpPAExpirationDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpPAExpirationDate").val());
            var dpPAExpirationDateOriginal = $.trim($("#dpPAExpirationDateOriginal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpPAExpirationDateOriginal").val());
            var dpPreDeterminationEffDate = Date.parse($("#dpPreDeterminationEffDate").val());
            var dpPreDeterminationExpDate = $.trim($("#dpPreDeterminationExpDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpPreDeterminationExpDate").val());
            var dpPreDeterminationExpDateOriginal = $.trim($("#dpPreDeterminationExpDateOriginal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpPreDeterminationExpDateOriginal").val());
            var ddlPAMinAge = parseInt($("#ddlPAMinAge").val());
            var ddlPAMaxAge = parseInt($("#ddlPAMaxAge").val());
            var isPAEffValid = true;
            var isPAExpValid = true;
            var isPDetEffValid = true;
            var isPDetExpValid = true;
            var isAdvNotEffValid = true;
            var isAdvNotExpValid = true;
            var isDRALEffValid = true;
            var isDRALExpValid = true;            
            var isMSPEffValid = true;

            // USER STORY 94398 MFQ 3-21-2024
            var dpAdvNotificationEffDate = Date.parse($("#dpAdvNotificationEffDate").val());
            var dpAdvNotificationExpDate = $.trim($("#dpAdvNotificationEffDate").val()) != '' ? ($.trim($("#dpAdvNotificationExpDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpAdvNotificationExpDate").val())) : null;
            var dpAdvNotificationExpDateOriginal = $.trim($("#dpAdvNotificationEffDate").val()) != '' ? ($.trim($("#dpAdvNotificationExpDateOriginal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpAdvNotificationExpDateOriginal").val())) : null;

            var dpDRALEffDate = Date.parse($("#dpDRALEffDate").val());
            var dpDRALExpDate = $.trim($("#dpDRALEffDate").val()) != '' ? ($.trim($("#dpDRALExpDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpDRALExpDate").val())) : null;
            var dpDRALExpDateOriginal = $.trim($("#dpDRALEffDate").val()) != '' ? ($.trim($("#dpDRALExpDateOrignal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpDRALExpDateOrignal").val())) : null;

            var dpMSPEffectiveDate = Date.parse($("#dpMSPEffectiveDate").val());
            var dpMSPExpirationDate = $.trim($("#dpMSPEffectiveDate").val()) != '' ? ($.trim($("#dpMSPExpirationDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpMSPExpirationDate").val())) : null;
            var dpMSPExpirationDateOriginal = $.trim($("#dpMSPEffectiveDate").val()) != '' ? ($.trim($("#dpMSPExpirationDateOrignal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpMSPExpirationDateOrignal").val())) : null;

            var dpSOSEffectiveDate = $.trim($("#dpSOSEffectiveDate").val());
            var dpSOSExpirationDate = $.trim($("#dpSOSExpirationDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpSOSExpirationDate").val());
            var dpSOSExpirationDateOrignal = $.trim($("#dpSOSExpirationDateOrignal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpSOSExpirationDateOrignal").val());

            var dpAAEffectiveDate = Date.parse($("#dpAAEffectiveDate").val());
            var dpAAExpirationDate = $.trim($("#dpAAEffectiveDate").val()) != '' ? ($.trim($("#dpAAExpirationDate").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpAAExpirationDate").val())) : null;
            var dpAAExpirationDateOriginal = $.trim($("#dpAAEffectiveDate").val()) != '' ? ($.trim($("#dpAAExpirationDateOrignal").val()) == "" ? Date.parse("12/31/2999") : Date.parse($("#dpAAExpirationDateOrignal").val())) : null;            
            

            // BUG 97752 MFQ 4/26/2024
            // ISSUE 102515 MFQ 6/24/2024

            var isStandardCat = $('#txtStandardCategory').val().trim() != '';

            if ($('#txtStandardCategory').val().trim() == '' && $("#ddlAlternateCategory").data("kendoDropDownList").value() == '') {
                errorMessage += "Record cannot save: Version must have at least one of the following populated:<br/> 1. HCEMI Reporting Category<br/> 2. Alternate Category\n";
                isValid = false;
            }
            // END BUG 97752 MFQ 4/26/2024

            // TASK 102519 MFQ 6/24/2024
            var $ddlAlternateCategory = $("#ddlAlternateCategory").data("kendoDropDownList");
            if ($ddlAlternateCategory.dataSource.data().length == 0) {
                errorMessage += "Record cannot save: Alternate Category is not configured to this procedure code.<br/>Please  update the Alternate Category Crosswalk\n";
                isValid = false;
            }
            // END TASK 102519 MFQ 6/24/2024

            //MFQCOMMENT
            //if (
            //    priorAuth == 'Yes' &&
            //    $("#ddlAlternateCategory").data("kendoDropDownList").value() != drivingStatus.PA &&
            //    $("#ddlAlternateCategory").data("kendoDropDownList").value().indexOf(drivingStatus.PAAA) < 0 &&
            //    $("#ddlAlternateCategory").data("kendoDropDownList").value().indexOf(drivingStatus.PASOS) < 0
            //) {
            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record (EPAL Summary tab)!\n";
            //        isValid = false;
            //    }
            //} else if (priorAuth == 'Yes' &&
            //    DetailController.drivingStatus?.name == drivingStatus.PASOS &&
            //    $("#ddlAlternateCategory").data("kendoDropDownList").value().indexOf(drivingStatus.PASOS) < 0) {

            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record (EPAL Summary tab)!\n";
            //        isValid = false;
            //    }
            //} else if (priorAuth == 'Yes' &&
            //    (
            //        $("#ddlAlternateCategory").data("kendoDropDownList").value().indexOf(drivingStatus.PASOS) >= 0 ||
            //        $("#ddlAlternateCategory").data("kendoDropDownList").value().indexOf(drivingStatus.AdvSOS) >= 0
            //    ) &&
            //    dpSOSEffectiveDate == '') {

            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record, Please enter SOS information (EPAL Summary tab)!\n";
            //        isValid = false;
            //    }
            //} 

            if (priorAuth == "Yes" && (dpPAEffectiveDate == "" || !kendo.parseDate($("#dpPAEffectiveDate").val()))) {
                // DetailController.message.showWarning("Please select a PA Effective Date in the EPAL Summary tab!");
                errorMessage += "Please select a PA Effective Date (EPAL Summary tab)!\n";
                isValid = false;
            }

            if ($("#dpPAEffectiveDate").val() != "" && !kendo.parseDate($("#dpPAEffectiveDate").val())) {
                errorMessage += "PA Effective Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isPAEffValid = false;
            }

            if ($("#dpPAExpirationDate").val() != "" && !kendo.parseDate($("#dpPAExpirationDate").val()) ) {
                errorMessage += "PA Expiration Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isPAExpValid = false;
            }

            if (isPAEffValid && isPAExpValid) {                
                if (dpPAEffectiveDate > dpPAExpirationDate) {
                    errorMessage += "PA Effective Date cannot be greater than PA Expiration Date (EPAL Summary tab)!\n";
                    isValid = false;
                }
                /*else if (dpPAEffectiveDate == dpPAExpirationDate) {
                    errorMessage += "PA Expiration Date must be greater than PA Effective Date (EPAL Summary tab)!\n";
                    isValid = false;
                }*/               
            }     

            // USER STORY 54535 MFQ
            if ($("#dpPreDeterminationEffDate").val() != "" && !kendo.parseDate($("#dpPreDeterminationEffDate").val())) {
                errorMessage += "Pre Determination Effective Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isPDetEffValid = false;
            }

            if ($("#dpPreDeterminationExpDate").val() != "" && !kendo.parseDate($("#dpPreDeterminationExpDate").val())) {
                errorMessage += "Pre Determination Expiration Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isPDetExpValid = false;
            }

            if (isPDetEffValid && isPDetExpValid) {                
                if (dpPreDeterminationEffDate > dpPreDeterminationExpDate) {
                    errorMessage += "Pre Determination Effective Date cannot be greater than Pre Determination Expiration Date (EPAL Summary tab)!\n";
                    isValid = false;
                }/*
                else if (dpPreDeterminationEffDate == dpPreDeterminationExpDate) {
                    errorMessage += "Pre Determination Expiration Date must be greater than Pre Determination Effective Date (EPAL Summary tab)!\n";
                    isValid = false;
                }*/
            }   

            //Advanced Notification Dates basic Validations
            if ($("#dpAdvNotificationEffDate").val() != "" && !kendo.parseDate($("#dpAdvNotificationEffDate").val())) {
                errorMessage += "Advanced Notification Effective Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isAdvNotEffValid = false;
            }

            if ($("#dpAdvNotificationExpDate").val() != "" && !kendo.parseDate($("#dpAdvNotificationExpDate").val())) {
                errorMessage += "Advanced Notification Expiration Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isAdvNotExpValid = false;
            }

            if (isAdvNotEffValid && isAdvNotExpValid) {
                if (dpAdvNotificationEffDate > dpAdvNotificationExpDate) {
                    errorMessage += "Advanced Notification Effective Date cannot be greater than it's Expiration Date (EPAL Summary tab)!\n";
                    isValid = false;
                }
                /*
                else if (dpAdvNotificationEffDate == dpAdvNotificationExpDate) {
                    errorMessage += "Advanced Notification Expiration Date must be greater than it's Effective Date (EPAL Summary tab)!\n";
                    isValid = false;
                }*/
            }

            //DRAL Dates basic Validations
            if ($("#dpDRALEffDate").val() != "" && !kendo.parseDate($("#dpDRALEffDate").val())) {
                errorMessage += "Drug Review At Launch Effective Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isDRALEffValid = false;
            }

            if ($("#dpDRALEffDate").val() != "" && !kendo.parseDate($("#dpDRALEffDate").val())) {
                errorMessage += "Drug Review At Launch Expiration Date is not correct (EPAL Summary tab)!\n";
                isValid = false;
                isDRALExpValid = false;
            }

            if (isDRALEffValid && isDRALExpValid) {
                if (dpDRALEffDate > dpDRALExpDate) {
                    errorMessage += "Drug Review At Launch Effective Date cannot be greater than it's Expiration Date (EPAL Summary tab)!\n";
                    isValid = false;
                }
                /*
                else if (dpDRALEffDate == dpDRALExpDate) {
                    errorMessage += "Drug Review At Launch Expiration Date must be greater than it's Effective Date (EPAL Summary tab)!\n";
                    isValid = false;
                }*/
            }

            var retRes = PADatesValidations(dpPAEffectiveDate, dpPAExpirationDate, dpPreDeterminationEffDate, dpPreDeterminationExpDate, dpAdvNotificationEffDate, dpAdvNotificationExpDate, dpDRALEffDate, dpDRALExpDate,  dpAAEffectiveDate, dpAAExpirationDate, dpMSPEffectiveDate, dpMSPExpirationDate,isPAEffValid, isPDetEffValid, errorMessage, isValid);
            errorMessage = retRes.errorMessage;
            isValid = retRes.isValid;

            retRes = PreDetDatesValidations(dpPreDeterminationEffDate, dpPreDeterminationExpDate, dpAdvNotificationEffDate, dpAdvNotificationExpDate, dpDRALEffDate, dpDRALExpDate, dpAAEffectiveDate, dpAAExpirationDate, dpMSPEffectiveDate, dpMSPExpirationDate, isPDetEffValid, errorMessage, isValid);
            errorMessage = retRes.errorMessage;
            isValid = retRes.isValid;

            retRes = AdvNotificationDatesValidations(dpAdvNotificationEffDate, dpAdvNotificationExpDate, dpDRALEffDate, dpDRALExpDate, dpAAEffectiveDate, dpAAExpirationDate, dpMSPEffectiveDate, dpMSPExpirationDate, isAdvNotEffValid, errorMessage, isValid);
            errorMessage = retRes.errorMessage;
            isValid = retRes.isValid;

            retRes = MSPDatesValidations(dpMSPEffectiveDate, dpMSPExpirationDate, dpDRALEffDate, dpDRALExpDate, dpAAEffectiveDate, dpAAExpirationDate, isMSPEffValid, errorMessage, isValid);
            errorMessage = retRes.errorMessage;
            isValid = retRes.isValid;

            retRes = DRALDatesValidations(dpDRALEffDate, dpDRALExpDate, dpAAEffectiveDate, dpAAExpirationDate, isDRALEffValid, errorMessage, isValid);
            errorMessage = retRes.errorMessage;
            isValid = retRes.isValid;

            /* Removed as it becomes changeable field now
            if (txtDrugName != "NA" && txtDrugReviewAtLaunch == "") {                
                errorMessage += "Please select a Drug Review At Launch (EPAL Summary tab)!\n";
                isValid = false;
            }*/
            
            if (parseInt(ddlPAMinAge) > parseInt(ddlPAMaxAge)) {                
                errorMessage += "EPAL Requirement Min Age can not be greater than PA Max Age (EPAL Summary tab)!\n";
                isValid = false;
            }

            if (diagList_Isvalid == false)
            {
                errorMessage += "Diagnosis list name already exists with selected include/exclude values. Please update include/exclude values!\n";
                isValid = false;
            }

            if (DiagListController.grid.check_diagList_prgmgby() == false)
            {
                errorMessage += "Diagnosis list requires a program managed by!\n";
                isValid = false;
            }

            // Get current status of the record
            DetailController.drivingStatus.renderCurrent();            

            if (
                    DetailController.drivingStatus.name == drivingStatus.PA ||
                    DetailController.drivingStatus.name == drivingStatus.PAAA ||
                    DetailController.drivingStatus.name == drivingStatus.PASOS ||
                    DetailController.drivingStatus.name == drivingStatus.Pred ||
                    DetailController.drivingStatus.name == drivingStatus.Adv || 
                    DetailController.drivingStatus.name == drivingStatus.AdvSOS                
               ) {

                if (ProgMgdByController.grid.row_count() == 0) {
                    var activeDrivingRecName = getEPALStatusName(DetailController.drivingStatus.name);
                    errorMessage += activeDrivingRecName + " record requires a program managed by!\n";
                    isValid = false;
                }
            }    

            // USER STORY 93759 MFQ 3-20-2024  //'Pre-D - Clinical Review Required' && 'Injectable Meds Pre-D'
            //if ($('#txtPreDetermination').val() == 'Yes' && ($("#ddlAlternateCategory").data("kendoDropDownList").value() != drivingStatus.Pred)) {                
            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record!\n";
            //        isValid = false;
            //    }
            //}

            // This condition restrict user to change alternate category and set to pred even the pre-det is In-active
            /*if ($('#txtPreDetermination').val() == 'No' && !isNaN(dpPreDeterminationEffDate) &&
                $("#ddlAlternateCategory").data("kendoDropDownList").text() != $('#ddlAlternateCategoryOrignal').val() &&
                $("#ddlAlternateCategory").data("kendoDropDownList").value() == drivingStatus.Pred &&
                $.trim($("#dpPreDeterminationExpDate").val()) != ""
            ) {
                errorMessage += "Record cannot save - Alternate Category cannot be changed for inactive Pre-determination!\n";
                isValid = false;
            }*/

            //USER STORY 93759 MFQ 3-15-2024
            if ($('#ddlAlternateCategoryOrignal').val() != $("#ddlAlternateCategory").data("kendoDropDownList").text() && $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
                if (dpPAExpirationDate != dpPAExpirationDateOriginal ||
                    dpPreDeterminationExpDate != dpPreDeterminationExpDateOriginal ||
                    dpAdvNotificationExpDate != dpAdvNotificationExpDateOriginal ||
                    dpDRALExpDate != dpDRALExpDateOriginal ||
                    dpMSPExpirationDate != dpMSPExpirationDateOriginal ||
                    dpAAExpirationDate != dpAAExpirationDateOriginal ||
                    dpSOSExpirationDate != dpSOSExpirationDateOrignal) {

                    errorMessage += " Record cannot save - Alternate Category cannot change when expiring a record!\n";
                    isValid = false;
                }
            }
            if ($('#ddlAlternateSubCategoryOrignal').val() != $('#ddlAlternateSubCategory').val()) { //USER STORY 93759 MFQ 3-20-2024
                if (dpPAExpirationDate != dpPAExpirationDateOriginal ||
                    dpPreDeterminationExpDate != dpPreDeterminationExpDateOriginal ||
                    dpAdvNotificationExpDate != dpAdvNotificationExpDateOriginal ||
                    dpDRALExpDate != dpDRALExpDateOriginal ||
                    dpMSPExpirationDate != dpMSPExpirationDateOriginal ||
                    dpAAExpirationDate != dpAAExpirationDateOriginal ||
                    dpSOSExpirationDate != dpSOSExpirationDateOrignal) {

                    errorMessage += " Record cannot save - Alternate Subcategory cannot change when expiring a record!\n";
                    isValid = false;
                }
            }

            // This condition restrict user to change alternate category and set to pred even the pre-det is In-active
            /*if ($('#txtDrugReviewAtLaunch').val() == 'No' && !isNaN(dpDRALEffDate) &&
                ($("#ddlAlternateCategory").data("kendoDropDownList").text() != $('#ddlAlternateCategoryOrignal').val() && $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') &&
                $("#ddlAlternateCategory").data("kendoDropDownList").value() == drivingStatus.DRAL && 
                $.trim($("#dpDRALExpDate").val()) != ""
            ) {
                errorMessage += "Record cannot save - Alternate Category cannot be changed for inactive Drug Review At Launch!\n";
                isValid = false;
            }*/
            /*
            if (ProgMgdByController.grid.row_count() == 0 && $("#txtPreDetermination").val() == "Yes" && ($("#txtPriorAuth").val() == "No" || $.trim($("#txtPriorAuth").val()) == "")) {
                errorMessage += "Pre Determination record requires a program managed by!\n";
                isValid = false;
            } */


            //Focus on PASummary tab after validation popup

            // DetailController.methods.focusPASummaryTab();            

            function PADatesValidations(dpPAEffectiveDate,
                dpPAExpirationDate,
                dpPreDeterminationEffDate,
                dpPreDeterminationExpDate,                
                dpAdvNotificationEffDate,
                dpAdvNotificationExpDate,
                dpDRALEffDate,
                dpDRALExpDate,
                dpAAEffectiveDate,
                dpAAExpirationDate,
                dpMSPEffectiveDate,
                dpMSPExpirationDate,      
                isPAEffValid,
                isPDetEffValid,
                errorMessage,
                isValid
            ) {

                // PA and Pre Det date overlap logic
                if (dpPAEffectiveDate <= dpPreDeterminationExpDate && dpPAExpirationDate >= dpPreDeterminationEffDate) {
                    errorMessage += "PA and Pre Determination dates must not overlap each other (EPAL Summary tab)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPAEffectiveDate) != '' && $.trim(dpPAExpirationDate) != '') {
                    if (dpPreDeterminationEffDate <= dpPAExpirationDate && dpPreDeterminationEffDate >= dpPAEffectiveDate) {
                        errorMessage += "Pre Determination Effective date must be 1 day greater than PA Effective Expiration date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpPreDeterminationEffDate) != '' && $.trim(dpPreDeterminationExpDate) != '') {
                        if (dpPAEffectiveDate <= dpPreDeterminationExpDate && dpPAEffectiveDate >= dpPreDeterminationEffDate) {
                            errorMessage += "PA Effective date must be 1 day greater than Pre Determination Expiration date (EPAL Summary tab)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPAEffValid && isPDetEffValid) { // IF PA EFF and PRE DET EFF Dates are only provided with NO EXP 
                    if ($.trim($('#dpPAEffectiveDate').val()).length > 0 && $.trim($('#dpPreDeterminationEffDate').val()).length > 0 && $.trim($('#dpPAExpirationDate').val()) == '' && $.trim($('#dpPreDeterminationExpDate').val()) == '') {
                        errorMessage += "Pre-Determination cannot be in effect at the same time as a Prior auth date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - PA and Advance Notification date overlap logic - MFQ 3-21-2024 
                if (dpPAEffectiveDate <= dpAdvNotificationExpDate && dpPAExpirationDate >= dpAdvNotificationEffDate) {
                    errorMessage += "PA and Advance Notification dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPAEffectiveDate) != '' && $.trim(dpPAExpirationDate) != '') {
                    if (dpAdvNotificationEffDate <= dpPAExpirationDate && dpAdvNotificationEffDate >= dpPAEffectiveDate) {
                        errorMessage += "Advance Notification Effective date must be 1 day greater than PA Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAdvNotificationEffDate) != '' && $.trim(dpAdvNotificationExpDate) != '') {
                        if (dpPAEffectiveDate <= dpAdvNotificationExpDate && dpPAEffectiveDate >= dpAdvNotificationEffDate) {
                            errorMessage += "PA Effective date must be 1 day greater than Advance Notification Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPAEffValid && kendo.parseDate($("#dpAdvNotificationEffDate").val())) { // IF PA EFF and Advanced Notification EFF Dates are only provided with NO EXP 
                    if ($.trim($('#dpPAEffectiveDate').val()).length > 0 && $.trim($('#dpAdvNotificationEffDate').val()).length > 0 && $.trim($('#dpPAExpirationDate').val()) == '' && $.trim($('#dpAdvNotificationExpDate').val()) == '') {
                        errorMessage += "Advanced Notification cannot be in effect at the same time as a Prior auth date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - PA and DRAL date overlap logic - MFQ 3-21-2024 dpDRALEffDate, dpDRALExpDate
                if (dpPAEffectiveDate <= dpDRALExpDate && dpPAExpirationDate >= dpDRALEffDate) {
                    errorMessage += "PA and Drug Review At Launch dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPAEffectiveDate) != '' && $.trim(dpPAExpirationDate) != '') {
                    if (dpDRALEffDate <= dpPAExpirationDate && dpDRALEffDate >= dpPAEffectiveDate) {
                        errorMessage += "Drug Review At Launch Effective date must be 1 day greater than PA Effective Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpDRALEffDate) != '' && $.trim(dpDRALExpDate) != '') {
                        if (dpPAEffectiveDate <= dpDRALExpDate && dpPAEffectiveDate >= dpDRALEffDate) {
                            errorMessage += "PA Effective date must be 1 day greater than Drug Review At Launch Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPAEffValid && kendo.parseDate($("#dpDRALEffDate").val())) { // IF PA EFF and DRAL EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpPAEffectiveDate').val()).length > 0 && $.trim($('#dpDRALEffDate').val()).length > 0 && $.trim($('#dpPAExpirationDate').val()) == '' && $.trim($('#dpDRALExpDate').val()) == '') {
                        errorMessage += "Drug Review At Launch cannot be in effect at the same time as a Prior auth date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                //// User Story 129227 Temporary validation removal until data is cleared

                // USER STORY 94398 - PA and Medicare Special Processing date overlap logic - MFQ 3-21-2024
                /*if (dpPAEffectiveDate <= dpMSPExpirationDate && dpPAExpirationDate >= dpMSPEffectiveDate) {
                    errorMessage += "PA and Medicare Special Processing dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else 
                if ($.trim(dpPAEffectiveDate) != '' && $.trim(dpPAExpirationDate) != '') {
                    if (dpMSPEffectiveDate <= dpPAExpirationDate && dpMSPEffectiveDate >= dpPAEffectiveDate) {
                        errorMessage += "Medicare Special Processing date must be 1 day greater than PA Effective Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpMSPEffectiveDate) != '' && $.trim(dpMSPExpirationDate) != '') {
                        if (dpPAEffectiveDate <= dpMSPExpirationDate && dpPAEffectiveDate >= dpMSPEffectiveDate) {
                            errorMessage += "PA Effective date must be 1 day greater than Medicare Special Processing Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPAEffValid && kendo.parseDate($("#dpMSPEffectiveDate").val())) { // IF PA EFF and MSP EFF Dates are only provided with NO EXP 
                    if ($.trim($('#dpPAEffectiveDate').val()).length > 0 && $.trim($('#dpMSPEffectiveDate').val()).length > 0 && $.trim($('#dpPAExpirationDate').val()) == '' && $.trim($('#dpMSPExpirationDate').val()) == '') {
                        errorMessage += "Medicare Special Processing cannot be in effect at the same time as a Prior auth date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }*/
                //// END User Story 129227 Temporary validation removal until data is cleared

                return result = {
                    isValid: isValid,
                    errorMessage: errorMessage
                };
            }

            function PreDetDatesValidations(
                dpPreDeterminationEffDate,
                dpPreDeterminationExpDate,
                dpAdvNotificationEffDate,
                dpAdvNotificationExpDate,
                dpDRALEffDate,
                dpDRALExpDate,
                dpAAEffectiveDate,
                dpAAExpirationDate,
                dpMSPEffectiveDate,
                dpMSPExpirationDate,                
                isPDetEffValid,
                errorMessage,
                isValid
            ) {

                // USER STORY 94398 - Pre Det and Advance Notification date overlap logic - MFQ 3-21-2024 
                if (dpPreDeterminationEffDate <= dpAdvNotificationExpDate && dpPreDeterminationExpDate >= dpAdvNotificationEffDate) {
                    errorMessage += "Pre Determination and Advance Notification dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPreDeterminationEffDate) != '' && $.trim(dpPreDeterminationExpDate) != '') {
                    if (dpAdvNotificationEffDate <= dpPreDeterminationExpDate && dpAdvNotificationEffDate >= dpPreDeterminationEffDate) {
                        errorMessage += "Advance Notification Effective date must be 1 day greater than Pre Determination Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpPreDeterminationEffDate <= dpAdvNotificationExpDate && dpPreDeterminationEffDate >= dpAdvNotificationEffDate) {
                            errorMessage += "Pre Determination Effective date must be 1 day greater than Advance Notification Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPDetEffValid && kendo.parseDate($("#dpAdvNotificationEffDate").val())) { // IF PA EFF and Advanced Notification EFF Dates are only provided with NO EXP 
                    if ($.trim($('#dpPreDeterminationEffDate').val()).length > 0 && $.trim($('#dpAdvNotificationEffDate').val()).length > 0 && $.trim($('#dpPreDeterminationExpDate').val()) == '' && $.trim($('#dpAdvNotificationExpDate').val()) == '') {
                        errorMessage += "Advanced Notification cannot be in effect at the same time as a Pre Determination date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - PA and DRAL date overlap logic - MFQ 3-21-2024 dpDRALEffDate, dpDRALExpDate
                if (dpPreDeterminationEffDate <= dpDRALExpDate && dpPreDeterminationExpDate >= dpDRALEffDate && !isNaN(dpPreDeterminationEffDate)) {
                    errorMessage += "Pre Determination and Drug Review At Launch dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPreDeterminationEffDate) != '' && $.trim(dpPreDeterminationExpDate) != '') {
                    if (dpDRALEffDate <= dpPreDeterminationExpDate && dpDRALEffDate >= dpPreDeterminationEffDate) {
                        errorMessage += "Drug Review At Launch Effective date must be 1 day greater than Pre Determination Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpPreDeterminationEffDate <= dpDRALExpDate && dpPreDeterminationEffDate >= dpDRALEffDate) {
                            errorMessage += "Pre Determination Effective date must be 1 day greater than Drug Review At Launch Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPDetEffValid && kendo.parseDate($("#dpDRALEffDate").val())) { // IF PA EFF and DRAL EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpPreDeterminationEffDate').val()).length > 0 && $.trim($('#dpDRALEffDate').val()).length > 0 && $.trim($('#dpPreDeterminationExpDate').val()) == '' && $.trim($('#dpDRALExpDate').val()) == '') {
                        errorMessage += "Drug Review At Launch cannot be in effect at the same time as a Pre Determination date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }


                // USER STORY 94398 - Pre Determination and Auto Approval date overlap logic - MFQ 3-21-2024
                if (dpPreDeterminationEffDate <= dpAAExpirationDate && dpPreDeterminationExpDate >= dpAAEffectiveDate) {
                    errorMessage += "Pre Determination and Auto Approval dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPreDeterminationEffDate) != '' && $.trim(dpPreDeterminationExpDate) != '') {
                    if (dpAAEffectiveDate <= dpPreDeterminationExpDate && dpAAEffectiveDate >= dpPreDeterminationEffDate) {
                        errorMessage += "Auto Approval Effective date must be 1 day greater than Pre Determination Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpPreDeterminationEffDate <= dpAAExpirationDate && dpPreDeterminationEffDate >= dpAAEffectiveDate) {
                            errorMessage += "Pre Determination Effective date must be 1 day greater than Auto Approval Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPDetEffValid && kendo.parseDate($("#dpAAEffectiveDate").val())) { // IF Pre Determination EFF and AA EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpPreDeterminationEffDate').val()).length > 0 && $.trim($('#dpAAEffectiveDate').val()).length > 0 && $.trim($('#dpPreDeterminationExpDate').val()) == '' && $.trim($('#dpAAExpirationDate').val()) == '') {
                        errorMessage += "Auto Approval cannot be in effect at the same time as a Pre Determination date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - Pre Determination and Medicare Special Processing date overlap logic - MFQ 3-21-2024
                if (dpPreDeterminationEffDate <= dpMSPExpirationDate && dpPAExpirationDate >= dpMSPEffectiveDate) {
                    errorMessage += "Pre Determination and Medicare Special Processing dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpPreDeterminationEffDate) != '' && $.trim(dpPreDeterminationExpDate) != '') {
                    if (dpMSPEffectiveDate <= dpPreDeterminationExpDate && dpMSPEffectiveDate >= dpPreDeterminationEffDate) {
                        errorMessage += "Medicare Special Processing date must be 1 day greater than Pre Determination Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpMSPEffectiveDate) != '' && $.trim(dpMSPExpirationDate) != '') {
                        if (dpPreDeterminationEffDate <= dpMSPExpirationDate && dpPreDeterminationEffDate >= dpMSPEffectiveDate) {
                            errorMessage += "Pre Determination Effective date must be 1 day greater than Medicare Special Processing Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isPDetEffValid && kendo.parseDate($("#dpMSPEffectiveDate").val())) { // IF Pre Determination EFF and MSP EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpPreDeterminationEffDate').val()).length > 0 && $.trim($('#dpMSPEffectiveDate').val()).length > 0 && $.trim($('#dpPreDeterminationExpDate').val()) == '' && $.trim($('#dpMSPExpirationDate').val()) == '') {
                        errorMessage += "Medicare Special Processing cannot be in effect at the same time as a Pre determination (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }


                return result = {
                    isValid: isValid,
                    errorMessage: errorMessage
                };
            }

            function AdvNotificationDatesValidations(                
                dpAdvNotificationEffDate,
                dpAdvNotificationExpDate,
                dpDRALEffDate,
                dpDRALExpDate,
                dpAAEffectiveDate,
                dpAAExpirationDate,                
                dpMSPEffectiveDate,
                dpMSPExpirationDate,
                isAdvNotEffValid,
                errorMessage,
                isValid
            ) {

                // USER STORY 94398 - Adv Notification and DRAL date overlap logic - MFQ 3-21-2024 dpDRALEffDate, dpDRALExpDate
                if (dpAdvNotificationEffDate <= dpDRALExpDate && dpAdvNotificationExpDate >= dpDRALEffDate) {
                    errorMessage += "Advanced Notification and Drug Review At Launch dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpAdvNotificationEffDate) != '' && $.trim(dpAdvNotificationExpDate) != '') {
                    if (dpDRALEffDate <= dpAdvNotificationExpDate && dpDRALEffDate >= dpAdvNotificationExpDate) {
                        errorMessage += "Drug Review At Launch Effective date must be 1 day greater than Advanced Notification Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpDRALEffDate) != '' && $.trim(dpDRALExpDate) != '') {
                        if (dpAdvNotificationExpDate <= dpDRALExpDate && dpAdvNotificationExpDate >= dpDRALEffDate) {
                            errorMessage += "Advanced Notification Effective date must be 1 day greater than Drug Review At Launch Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isAdvNotEffValid && kendo.parseDate($("#dpDRALEffDate").val())) { // IF Adv Eff and DRAL EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpAdvNotificationExpDate').val()).length > 0 && $.trim($('#dpDRALEffDate').val()).length > 0 && $.trim($('#dpAdvNotificationExpDate').val()) == '' && $.trim($('#dpDRALExpDate').val()) == '') {
                        errorMessage += "Drug Review At Launch cannot be in effect at the same time as a Advanced Notification date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - Adv Notification and Auto Approval date overlap logic - MFQ 3-21-2024
                if (dpAdvNotificationExpDate <= dpAAExpirationDate && dpAdvNotificationExpDate >= dpAAEffectiveDate) {
                    errorMessage += "Advanced Notification and Auto Approval dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpAdvNotificationEffDate) != '' && $.trim(dpAdvNotificationExpDate) != '') {
                    if (dpAAEffectiveDate <= dpAdvNotificationExpDate && dpAAEffectiveDate >= dpAdvNotificationExpDate) {
                        errorMessage += "Auto Approval Effective date must be 1 day greater than Advanced Notification Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpAdvNotificationExpDate <= dpAAExpirationDate && dpAdvNotificationExpDate >= dpAAEffectiveDate) {
                            errorMessage += "Advanced Notification Effective date must be 1 day greater than Auto Approval Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isAdvNotEffValid && kendo.parseDate($("#dpAAEffectiveDate").val())) { // IF Advanced Notification EFF and AA EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpAdvNotificationExpDate').val()).length > 0 && $.trim($('#dpAAEffectiveDate').val()).length > 0 && $.trim($('#dpAdvNotificationExpDate').val()) == '' && $.trim($('#dpAAExpirationDate').val()) == '') {
                        errorMessage += "Auto Approval cannot be in effect at the same time as a Advanced Notification date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - Adv Notification and Medicare Special Processing date overlap logic - MFQ 3-21-2024
                if (dpAdvNotificationExpDate <= dpMSPExpirationDate && dpAdvNotificationExpDate >= dpMSPEffectiveDate) {
                    errorMessage += "Advanced Notification and Medicare Special Processing dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpAdvNotificationEffDate) != '' && $.trim(dpAdvNotificationExpDate) != '') {
                    if (dpMSPEffectiveDate <= dpAdvNotificationExpDate && dpMSPEffectiveDate >= dpAdvNotificationExpDate) {
                        errorMessage += "Medicare Special Processing date must be 1 day greater than Advanced Notification Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpMSPEffectiveDate) != '' && $.trim(dpMSPExpirationDate) != '') {
                        if (dpAdvNotificationExpDate <= dpMSPExpirationDate && dpAdvNotificationExpDate >= dpMSPEffectiveDate) {
                            errorMessage += "Advanced Notification Effective date must be 1 day greater than Medicare Special Processing Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }                
                if (isAdvNotEffValid && kendo.parseDate($("#dpMSPEffectiveDate").val())) { // IF Advanced Notification EFF and MSP EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpAdvNotificationExpDate').val()).length > 0 && $.trim($('#dpMSPEffectiveDate').val()).length > 0 && $.trim($('#dpAdvNotificationExpDate').val()) == '' && $.trim($('#dpMSPExpirationDate').val()) == '') {
                        errorMessage += "Medicare Special Processing cannot be in effect at the same time as a Advanced Notification date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }


                return result = {
                    isValid: isValid,
                    errorMessage: errorMessage
                };
            }

            function MSPDatesValidations(                
                dpMSPEffectiveDate,
                dpMSPExpirationDate,
                dpDRALEffDate,
                dpDRALExpDate,
                dpAAEffectiveDate,
                dpAAExpirationDate,                
                isMSPEffValid,
                errorMessage,
                isValid
            ) {

                // USER STORY 94398 - MSP and DRAL date overlap logic - MFQ 3-21-2024 dpDRALEffDate, dpDRALExpDate
                if (dpMSPEffectiveDate <= dpDRALExpDate && dpMSPExpirationDate >= dpDRALEffDate) {
                    errorMessage += "Medicare Special Processing and Drug Review At Launch dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpMSPEffectiveDate) != '' && $.trim(dpMSPExpirationDate) != '') {
                    if (dpDRALEffDate <= dpMSPExpirationDate && dpDRALEffDate >= dpMSPEffectiveDate) {
                        errorMessage += "Drug Review At Launch Effective date must be 1 day greater than Medicare Special Processing Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpDRALEffDate) != '' && $.trim(dpDRALExpDate) != '') {
                        if (dpMSPEffectiveDate <= dpDRALExpDate && dpMSPEffectiveDate >= dpDRALEffDate) {
                            errorMessage += "Medicare Special Processing Effective date must be 1 day greater than Drug Review At Launch Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }
                if (isMSPEffValid && kendo.parseDate($("#dpDRALEffDate").val())) { // IF Adv Eff and DRAL EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpMSPExpirationDate').val()).length > 0 && $.trim($('#dpDRALEffDate').val()).length > 0 && $.trim($('#dpMSPExpirationDate').val()) == '' && $.trim($('#dpDRALExpDate').val()) == '') {
                        errorMessage += "Drug Review At Launch cannot be in effect at the same time as a Medicare Special Processing date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                // USER STORY 94398 - Adv Notification and Auto Approval date overlap logic - MFQ 3-21-2024
                if (dpMSPEffectiveDate <= dpAAExpirationDate && dpMSPExpirationDate >= dpAAEffectiveDate) {
                    errorMessage += "Medicare Special Processing and Auto Approval dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpMSPEffectiveDate) != '' && $.trim(dpMSPExpirationDate) != '') {
                    if (dpAAEffectiveDate <= dpMSPExpirationDate && dpAAEffectiveDate >= dpMSPEffectiveDate) {
                        errorMessage += "Auto Approval Effective date must be 1 day greater than Medicare Special Processing Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpMSPEffectiveDate <= dpAAExpirationDate && dpMSPEffectiveDate >= dpAAEffectiveDate) {
                            errorMessage += "Medicare Special Processing Effective date must be 1 day greater than Auto Approval Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }
                if (isMSPEffValid && kendo.parseDate($("#dpAAEffectiveDate").val())) { // IF Advanced Notification EFF and AA EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpMSPExpirationDate').val()).length > 0 && $.trim($('#dpAAEffectiveDate').val()).length > 0 && $.trim($('#dpMSPExpirationDate').val()) == '' && $.trim($('#dpAAExpirationDate').val()) == '') {
                        errorMessage += "Auto Approval cannot be in effect at the same time as a Medicare Special Processing date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                return result = {
                    isValid: isValid,
                    errorMessage: errorMessage
                };
            }

            function DRALDatesValidations(                
                dpDRALEffDate,
                dpDRALExpDate,
                dpAAEffectiveDate,
                dpAAExpirationDate,
                isDRALEffValid,
                errorMessage,
                isValid
            ) {

                // USER STORY 94398 - Adv Notification and Auto Approval date overlap logic - MFQ 3-21-2024
                if (dpDRALEffDate <= dpAAExpirationDate && dpDRALExpDate >= dpAAEffectiveDate) {
                    errorMessage += "Drug Review At Launch and Auto Approval dates must not overlap each other (EPAL Summary tab / EPAL Review Indicators)!\n";
                    isValid = false;
                }
                else if ($.trim(dpDRALEffDate) != '' && $.trim(dpDRALExpDate) != '') {
                    if (dpAAEffectiveDate <= dpDRALExpDate && dpAAEffectiveDate >= dpDRALExpDate) {
                        errorMessage += "Auto Approval Effective date must be 1 day greater than Drug Review At Launch Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                        isValid = false;
                    }
                    if ($.trim(dpAAEffectiveDate) != '' && $.trim(dpAAExpirationDate) != '') {
                        if (dpDRALExpDate <= dpAAExpirationDate && dpDRALExpDate >= dpAAEffectiveDate) {
                            errorMessage += "Drug Review At Launch Effective date must be 1 day greater than Auto Approval Expiration date (EPAL Summary tab / EPAL Review Indicators)!\n";
                            isValid = false;
                        }
                    }
                }
                if (isDRALEffValid && kendo.parseDate($("#dpAAEffectiveDate").val())) { // IF Advanced Notification EFF and AA EFF Dates are only provided with NO EXP
                    if ($.trim($('#dpDRALExpDate').val()).length > 0 && $.trim($('#dpAAEffectiveDate').val()).length > 0 && $.trim($('#dpDRALExpDate').val()) == '' && $.trim($('#dpAAExpirationDate').val()) == '') {
                        errorMessage += "Auto Approval cannot be in effect at the same time as a Drug Review At Launch date (EPAL Summary tab)!\n";
                        isValid = false;
                    }
                }

                return result = {
                    isValid: isValid,
                    errorMessage: errorMessage
                };
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            return result;
        },
        validate_SOS: function () {
            var isValid = true;
            var errorMessage = "";
            var activeTabNameAfterValidation = "#EPALReviewIndicators";
            var isEffValid = false;
            var isExpValid = false;
            var txtPriorAuth                = $("#txtPriorAuth");
            var txtSiteofServiceApplies     = $("#txtSiteofServiceApplies");
            var dpPAEffectiveDate           = $("#dpPAEffectiveDate");
            var dpPAExpirationDate          = $("#dpPAExpirationDate");
            var dpSOSEffectiveDate          = $('#dpSOSEffectiveDate');   
            var dpSOSExpirationDate         = $("#dpSOSExpirationDate");
            var ddlURGCategory              = $('#ddlURGCategory');
            var ddlSOSTypeReview            = $('#ddlSOSTypeReview');
            var dpSOSExpirationDate         = $("#dpSOSExpirationDate");  
            var txtAdvNotification          = $('#txtAdvNotification');
            var dpAdvNotificationEffDate    = $('#dpAdvNotificationEffDate');
            var dpAdvNotificationExpDate    = $('#dpAdvNotificationExpDate');

            function getSOSMaxDatePairType(dpPAEffectiveDate, dpPAExpirationDate , dpAdvNotificationEffDate, dpAdvNotificationExpDate) {
                var activeService = dpPAEffectiveDate.val() != '' && dpAdvNotificationEffDate.val() != '' ? drivingStatus.ALL : (dpPAEffectiveDate.val() != '' ? drivingStatus.PA : (dpAdvNotificationEffDate.val() != '' ? drivingStatus.Adv : drivingStatus.NONE));
                if (activeService == drivingStatus.ALL) {
                    var paExp = $.trim(dpPAExpirationDate.val()) == '' ? '12/31/2024' : dpPAExpirationDate.val();

                    if (Date.parse(paExp) > Date.parse(dpAdvNotificationExpDate.val())) {
                        return drivingStatus.PA;
                    } else {
                        return drivingStatus.Adv;
                    }
                } 

                return activeService;
            }

            // USER STORY 93759 MFQ 3-20-2024 
            var _drivingStatus = getSOSMaxDatePairType(dpPAEffectiveDate, dpPAExpirationDate, dpAdvNotificationEffDate, dpAdvNotificationExpDate);

            if (_drivingStatus == drivingStatus.Adv && $.trim(dpSOSEffectiveDate.val()) != '') {
                if (DetailController.methods.SOS.changed() && txtAdvNotification.val() == "No" && Date.parse(dpAdvNotificationExpDate.val()) < Date.parse(dpSOSEffectiveDate.val()) && (dpSOSEffectiveDate.val() != "" || !kendo.parseDate(dpSOSEffectiveDate.val()))) {
                    errorMessage = "Advanced Notification is not Active, SOS should be not be entered (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                    activeTabNameAfterValidation = "#EPALReviewIndicators";
                } else {

                    if (Date.parse(dpSOSEffectiveDate.val()) < Date.parse(dpAdvNotificationEffDate.val())) {
                        errorMessage += "SOS Effective Date cannot be less than Advanced Notification Effective date (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if (Date.parse(dpSOSEffectiveDate.val()) > Date.parse(dpAdvNotificationExpDate.val())) {
                        errorMessage += "SOS Effective Date cannot be greater than Advanced Notification Expiration date (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if (Date.parse(dpSOSExpirationDate.val() || new Date("12/31/2999")) < Date.parse(dpAdvNotificationEffDate.val())) {
                        errorMessage += "SOS Expiration Date cannot be less than Advanced Notification Effective date (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if ($('#txtAdvNotification').val() =='Yes' && Date.parse(dpSOSExpirationDate.val() || new Date("12/31/2999")) > Date.parse(dpAdvNotificationExpDate.val())) {
                        errorMessage += "SOS Expiration Date cannot be greater than Advanced Notification date(EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    var resultSoSValidations = sosValidations(dpSOSEffectiveDate, dpSOSExpirationDate, errorMessage, isValid, isExpValid);
                    errorMessage = resultSoSValidations.errorMessage;
                    isValid = resultSoSValidations.isValid;
                }
            } else if (_drivingStatus == drivingStatus.PA && $.trim(dpSOSEffectiveDate.val()) != '') {
                if (DetailController.methods.SOS.changed() && txtPriorAuth.val() == "No" && Date.parse(dpPAExpirationDate.val()) < Date.parse(dpSOSEffectiveDate.val()) && (dpSOSEffectiveDate.val() != "" || !kendo.parseDate(dpSOSEffectiveDate.val()))) {
                    errorMessage = "PRIOR AUTH is not Active, SOS should be not be entered (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                    activeTabNameAfterValidation = "#EPALReviewIndicators";
                } else {

                    if (Date.parse(dpSOSEffectiveDate.val()) < Date.parse(dpPAEffectiveDate.val())) {
                        errorMessage += "SOS Effective Date cannot be less than PA Effective date(EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if (Date.parse(dpSOSEffectiveDate.val()) > Date.parse(dpPAExpirationDate.val())) {
                        errorMessage += "SOS Effective Date cannot be greater than PA Expiration date(EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if (Date.parse(dpSOSExpirationDate.val() || new Date("12/31/2999")) < Date.parse(dpPAEffectiveDate.val())) {
                        errorMessage += "SOS Expiration Date cannot be less than PA Effective date(EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }

                    if ($('#txtPriorAuth').val() == 'Yes' && Date.parse(dpSOSExpirationDate.val() || new Date("12/31/2999")) > Date.parse(dpPAExpirationDate.val())) {
                        errorMessage += "SOS Expiration Date cannot be greater than PA Expiration date(EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }


                    var resultSoSValidations = sosValidations(dpSOSEffectiveDate, dpSOSExpirationDate, errorMessage, isValid, isExpValid);
                    errorMessage = resultSoSValidations.errorMessage;
                    isValid = resultSoSValidations.isValid;
                }
            }                        

            if (isEffValid && isExpValid) {
                var dSOSExpire = Date.parse(dpSOSExpirationDate.val());
                var dSOSEffective = Date.parse(dpSOSEffectiveDate.val());
                if (dSOSExpire < dSOSEffective) {
                    errorMessage += "The SOS expiration date must be at least one day later then the Effective date (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
            }

            function sosValidations(dpSOSEffectiveDate, dpSOSExpirationDate, errorMessage, isValid, isExpValid) {
                if (dpSOSEffectiveDate.val() != "") {
                    var effDate = kendo.parseDate(dpSOSEffectiveDate.val());
                    // Check if date parse was successful
                    if (!effDate) {
                        errorMessage += "SOS Effective Date is not correct (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }
                    else {
                        isEffValid = true;
                    }

                    if (ddlURGCategory.val() == '') {
                        errorMessage += "URG Category is required!\n";
                        isValid = false;
                    }
                    if (ddlSOSTypeReview.val() == '') {
                        errorMessage += "SOS Type Review is required!\n";
                        isValid = false;
                    }

                    if (!$('input[name=sossite]:checked').length) {
                        errorMessage += "SOS Site Indicator is required!\n";
                        isValid = false;
                    }
                }

                if (dpSOSExpirationDate.val() != "") {
                    var expDate = kendo.parseDate(dpSOSExpirationDate.val());
                    // Check if date parse was successful
                    if (!expDate) {
                        errorMessage += "SOS Expiration Date is not correct (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }
                    else {
                        isExpValid = true;
                    }
                }

                // TASK 97578 MFQ 4/24/2024
                if (dpSOSEffectiveDate.val() != "" && dpSOSExpirationDate.val() != "") {
                    var sosEff = new Date(dpSOSEffectiveDate.val());
                    var sosExp = new Date(dpSOSExpirationDate.val());

                    if (sosEff == sosExp) {
                        errorMessage += "SOS Effective Date cannot be equal to SOS Expiration Date (EPAL Review Indicators Tab)!\n";
                        isValid = false;
                    }
                }

                if (dpSOSEffectiveDate.val() != "" && $('#dpAAEffectiveDate').val() != "") {
                    errorMessage += "SOS and Auto Approval cannot be added at the same time in the record (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }

                return {
                    isValid: isValid,
                    errorMessage: errorMessage                    
                };
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage,
                activeTabNameAfterValidation: activeTabNameAfterValidation
            };

            return result;
        },
        validate_AA_Prev_Review: function () {
            var isValid = true;
            var errorMessage = "";
            var isEffValid = false;
            var isExpValid = false;

            // USER STORY 93759 MFQ 3-20-2024 MFQCOMMENT
            //if (DetailController.drivingStatus.name == drivingStatus.AA && $('#ddlAlternateCategory').val() != drivingStatus.AA) {
            //    var isStandardCat = $('#txtStandardCategory').val().trim() != '';
            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record!\n";
            //        isValid = false;
            //    }
            //}

            if ($("#dpAAEffectiveDate").val() != "") {
                var effDate = kendo.parseDate($("#dpAAEffectiveDate").val());
                // Check if date parse was successful
                if (!effDate) {
                    errorMessage += "AA Effective Date is not correct (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
                else {
                    isEffValid = true;
                }
            }

            if ($("#dpAAExpirationDate").val() != "") {
                var expDate = kendo.parseDate($("#dpAAExpirationDate").val());
                // Check if date parse was successful
                if (!expDate) {
                    errorMessage += "AA Expiration Date is not correct (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
                else {
                    isExpValid = true;
                }
            }

            if (isEffValid && isExpValid) {
                var dAAExpire = Date.parse($("#dpAAExpirationDate").val());
                var dAAEffective = Date.parse($("#dpAAEffectiveDate").val());
                if (dAAExpire < dAAEffective) { //removed = from date comparison MFQ 9/11/2024
                    errorMessage += "The AA expiration date must be at least one day later then the Effective date (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
            }

            if ($("#dpDateOfRecentReview").val() != "") {
                var rrDate = kendo.parseDate($("#dpDateOfRecentReview").val());
                // Check if date parse was successful
                if (!rrDate) {
                    errorMessage += "Date of Most Recent Review is not correct (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
                else if (Date.parse($("#dpDateOfRecentReview").val()) > Date.parse(new Date())) {
                    errorMessage += "Date of Most Recent Review can not be future date (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            return result;
        },
        validate_MedSpeProc: function () {
            var isValid = true;
            var errorMessage = "";
            var isEffValid = false;
            var isExpValid = false;

            // USER STORY 93759 MFQ 3-20-2024 MFQCOMMENT
            //if (DetailController.drivingStatus.name == drivingStatus.MSP && $('#ddlAlternateCategory').val() != drivingStatus.MSP) {
            //    var isStandardCat = $('#txtStandardCategory').val().trim() != '';
            //    if (!isStandardCat || $("#ddlAlternateCategory").data("kendoDropDownList").text() != '---Select Category---') {
            //        errorMessage += "Record cannot save - Alternate Category does not match current status of record!\n";
            //        isValid = false;
            //    }
            //}

            if ($("#dpMSPEffectiveDate").val() != "") {
                var effDate = kendo.parseDate($("#dpMSPEffectiveDate").val());
                // Check if date parse was successful
                if (!effDate) {
                    errorMessage += "MSP Effective Date is not correct (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
                else {
                    isEffValid = true;
                }
            }

            if ($('#lblTypeofSpecialProcessing').hasClass('required')) {
                if ($("#ddlTypeofSpecialProcessing").val() == "") {
                    errorMessage += "Please select a Type of Special Processing (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
            }

            //if ($("#txtMedicareSpecialProcessing").val() == "Yes") {
            //    if ($("#dpMSPEffectiveDate").val() == "") {
            //        errorMessage += "Please pick a MSP Effective Date because MSP is Yes (EPAL Review Indicators Tab)!\n";
            //        isValid = false;
            //    }

            //    if ($("#ddlTypeofSpecialProcessing").val() == "") {
            //        errorMessage += "Please select a Type of Special Processing (EPAL Review Indicators Tab)!\n";
            //        isValid = false;
            //    }
            //}

            if ($("#dpMSPExpirationDate").val() != "") {
                var expDate = kendo.parseDate($("#dpMSPExpirationDate").val());
                // Check if date parse was successful
                if (!expDate) {
                    errorMessage += "MSP Expiration Date is not correct (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
                else {
                    isExpValid = true;
                }
            }

            if (isEffValid && isExpValid) {
                var dMSPExpire = Date.parse($("#dpMSPExpirationDate").val());
                var dMSPEffective = Date.parse($("#dpMSPEffectiveDate").val());
                if (dMSPExpire < dMSPEffective) { //removed = from date comparison MFQ 9/11/2024
                    errorMessage += "The MSP expiration date must be at least one day later then the Effective date (EPAL Review Indicators Tab)!\n";
                    isValid = false;
                }
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
        }
    },
    drivingStatus: {
        name: '',
        byMaxDate: {
            name: ''
        },
        activate: function () {
            var currentStatus = DetailController.drivingStatus.renderCurrent();
            var futureStatus = currentStatus?.future; //BUG 127081 MFQ 2/21/2025
            if (futureStatus == null) {
                $('.driving-status-badge').addClass('show').removeClass('hide').text(getEPALStatusName(currentStatus?.name)); //USER STORY 95312 MFQ 4-1-2024    
            } else {
                currentStatus = getEPALStatusName(currentStatus?.name);
                futureStatus = getEPALStatusName(futureStatus?.name);
                currentStatus = currentStatus == futureStatus ? currentStatus : currentStatus + ' -> ' + futureStatus;
                $('.driving-status-badge').addClass('show').removeClass('hide').text(currentStatus); //USER STORY 95312 MFQ 4-1-2024    
            }            
        },
        inActive: function (status) {
            if ($('.driving-status-badge').text() == status)
                $('.driving-status-badge').addClass('hide').removeClass('show').text(''); //USER STORY 95312 MFQ 4-1-2024
        },
        renderCurrent: function (dateRange) {
            var activeDateRange = getDrivingStatusDateRangeIncludingToday((dateRange == null || dateRange == undefined ? 'all' : dateRange), null);
            DetailController.drivingStatus.name = activeDateRange?.future != null ? activeDateRange?.future.name : activeDateRange?.name;

            return activeDateRange;
        }
    }
};

$(document).ready(function () {
    DetailController.init();

    function showOverlay() {
        $("#myOverlay").removeClass('hidden');
        $("#myOverlay").addClass('overlay');
    }

    function hideOverlay() {
        $("#myOverlay").removeClass('overlay');
        $("#myOverlay").addClass('hidden');
    }

    $(window).bind('beforeunload', function () {
        if (isDirty) {
            return 'please save your setting before leaving the page.';     
        }     
    });

    Swal.fire({
        title: 'Loading..',
        text: 'Loading record, please wait..',
        timerProgressBar: true,
        timer: 5000,
        //timer: 60000, 
        allowOutsideClick: false,
        willOpen: () => {
            Swal.showLoading();

            setTimeout(function () {
                $(function () {
                    if (addDetail) {
                        form_original_data = $('#addForm').serialize();
                    }
                    else if (editDetail) {
                        form_original_data = $('#editForm').serialize();
                    }
                    else if (duplicateDetail) {
                        form_original_data = $('#duplicateForm').serialize();
                    }

                    form_original_data += DetailController.dirtyform.get_grid_StateInfo_Data();
                    form_original_data += DetailController.dirtyform.get_grid_DiagCodes_Data();
                    form_original_data += DetailController.dirtyform.get_grid_RevCodes_Data();
                    form_original_data += DetailController.dirtyform.get_grid_AllocatedPlaces_Data();
                    form_original_data += DetailController.dirtyform.get_grid_Modifiers_Data();
                    form_original_data += DetailController.dirtyform.get_grid_Retention_Data();
                    form_original_data += DetailController.dirtyform.get_grid_Reduction_Data();
                    
                    Swal.close();


                    /*if (!viewDetail) {
                        DetailController.dropdown.AutoApproval_Change();
                        DetailController.dropdown.MedicareSpecialProcessing_Change();
                    }*/

                });
            }, 3000); // <-- Change timeout this to 5000 when running on LocalHost for dirtyform check, else set to 3000 for web servers.
        }
    });    
});



