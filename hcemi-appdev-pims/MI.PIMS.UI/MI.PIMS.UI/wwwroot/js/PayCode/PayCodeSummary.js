Date.prototype.isValid = function () {
    // An invalid date object returns NaN for getTime() and NaN is the only
    // object not strictly equal to itself.
    return this.getTime() === this.getTime();
};
var PayCodeSummaryController = {    
    init: function () {
        PayCodeSummaryController.bind();
    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });
        setTimeout(function () {
            $(function () {
                // PayCodeSummaryController.multiSelect.loadMultiSelect();
                form_original_data = $('#editForm').serialize();
            });
        }, 3000);

        $("#btnGotoDetail").click(function () {
            var pims_id = encodeURI($("#hidden-PAYC_HIERARCHY_KEY").val()); 
            var payCodeVersionDt = $("#txtOrigEPALVersionDt").val();
            PayCodeSummaryController.methods.redirect_PayCode(pims_id);
        });

        $("#btnSave").on("click", function () {
            PayCodeSummaryController.methods.savePayCode();
        });

        $("#btnSaveHistoric").on("click", function () {
            PayCodeSummaryController.methods.saveHistoricalPayCode();
        });

        $("#btnReset").click(function () {
            PayCodeSummaryController.message.showReset();
        });
        $("#btnGoToEdit").click(function () {
            PayCodeSummaryController.methods.redirect_Detail("Edit");
        });
        $("#btnViewOnly").click(function () {
            PayCodeSummaryController.methods.redirect_Detail("View");
        });
        $("#btnDuplicateCopy").click(function () {
            PayCodeSummaryController.methods.redirect_Detail("DuplicateRecord");
        });
        $('#editForm').change(function () {
            PayCodeSummaryController.dirtyform.dirtycheck();
        });

        $("#btnDeleteRecord").on('click', function () {

      /*      alert('testing..')*/
            // If validation is passed
            if ($("#txtCRNumber").val() != "" && $("#txtDeleleReason").val() != "") {
                $(window).unbind('beforeunload');
                MICore.Notification.whileSaving('Saving changes', function () {
                    setTimeout(function () {
                        $(function () {
                            PayCodeSummaryController.methods.delete();
                        });
                    }, 1000)
                });
            }
        });
    },
    multiSelect: {       
        param: {
            KLPCS: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_KL_PCS'
                }
            },
            NDBPCS: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_NDB_PCS'
                }
            },
            NDBRemarkCode: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_NDB_REMARK_CD'
                }
            },
            ClinicaliCES: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_IND'
                }
            },
            ClinicaliCESName: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_NAME'
                }
            },
            ClinicaliCESName2: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_NAME',
                    p_BUS_SEG_CD: $("#txtBusinessSegment").val()
                }
            },
            ClinicaliCESAction: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_EDIT_ACTION'
                }
            },
            AdvancedNotificationRequired: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ADVN_NOTIF'
                }
            },
            RoutedMCR : function () {
                return {
                    p_VV_SET_NAME: 'PAYC_MCR_ROUTED'
                }
            },
            BifurcatedProcess: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_BIFURCATED'
                }
            },
            NS88Compliance: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_NS88_COMPLIANCE'
                }
            },
            AdditionalEdits: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ADDITIONAL_EDITS'
                }
            }
        },
        loadMultiSelect: function () {
            //PayCodeSummaryController.multiSelect.msRoutedMCR();
        },
        onChange_RoutedMCR: function () {
            $("#txtRoutedMCR").val('');
            $("#txtRoutedMCR").val($("#msRoutedMCR").val());
        },
        msRoutedMCR: function () {
            var multiselect = $("#msRoutedMCR").data("kendoMultiSelect");
            var values = $("#txtRoutedMCR").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        }
    },
    dropdown: {
        refreshDropDownList: function () {
            var ddlLob = $("#ddlLOB").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity").data('kendoDropDownList');
            ddlLob.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlEntity.dataSource.read();
        },

        onChange_LOB: function () {
            PayCodeSummaryController.methods.create_pims_id()
            PayCodeSummaryController.dropdown.refreshDropDownList();
        },

        onChange_Entity: function () {
            PayCodeSummaryController.methods.create_pims_id()
            PayCodeSummaryController.dropdown.refreshDropDownList();
        },

        onChange_Plan: function () {
            PayCodeSummaryController.methods.create_pims_id()
            PayCodeSummaryController.dropdown.refreshDropDownList();
        },

        onChange_Product: function () {
            PayCodeSummaryController.methods.create_pims_id()
            PayCodeSummaryController.dropdown.refreshDropDownList();
        },
        onChange_txtProcedureCode_AddNew: function () {
            PayCodeSummaryController.methods.create_pims_id()
        },
        onChangeKLPCS: function () {
            if ($('#ddKLPCS').val().length > 0 && $('#PCS_PAYC_BUS_SEG_CD').val() == 'EnI') {
                $('.ddNDBPCS_label').addClass('required');
            } else {
                $('.ddNDBPCS_label').removeClass('required');
            }
        },
        onChange_ClinicaliCES: function () {
            if ($('#ddClinicaliCES').val().toUpperCase() == 'YES') {                
                $("#ddClinicaliCESName").data('kendoMultiSelect').enable(true);                
                $("#ddClinicaliCESAction").data('kendoDropDownList').enable(true);                

                $('.ddClinicaliCESName_label').addClass('required');
                $('.ddClinicaliCESAction_label').addClass('required');                
            } else {
                $("#ddClinicaliCESName").data('kendoMultiSelect').value('');
                $("#ddClinicaliCESAction").data('kendoDropDownList').value('');
                $("#ddClinicaliCESName").data('kendoMultiSelect').enable(false);
                $("#ddClinicaliCESAction").data('kendoDropDownList').enable(false);

                $('.ddClinicaliCESName_label').removeClass('required');
                $('.ddClinicaliCESAction_label').removeClass('required');
            }
        }, 
        param: {
            All_APP_LOB: function () {
                var param = PayCodeSummaryController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";
                return param;
            },
            All_APP_ENTITY: function () {
                var param = PayCodeSummaryController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_ENTITY_CD";
                return param;
            },
            All_APP_PLAN: function () {
                var param = PayCodeSummaryController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_PLAN_CD";
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = PayCodeSummaryController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_PRODUCT_CD";
                return param;
            },
            All: function () {
                return {
                    PAYC_BUS_SEG_CD: $("#ddlLOB").val(),
                    PAYC_ENTITY_CD: $("#ddlEntity").val(),
                    PAYC_PLAN_CD: $("#ddlPlan").val(),
                    PAYC_PRODUCT_CD: $("#ddlProduct").val()
                }
            }
        },
    },
    grid: {
        param: function () {
            var paramObj = {
                p_PAYC_HIERARCHY_KEY: $.trim($("#txtPIMSID").val())
            }
            return paramObj;
        }
    },
    dirtyform: {
        dirtycheck: function () {
            form_new_data = $('#editForm').serialize();
            if (form_new_data != form_original_data) {
                PayCodeSummaryController.dirtyform.enablebuttons();
                isDirty = true;
            }
            else if (form_new_data === form_original_data) {
                PayCodeSummaryController.dirtyform.disablebuttons();
                isDirty = false;
            }
        },
        disablebuttons: function () {
            $('#editForm').find('#btnSave').attr('disabled', 'disabled');
            $('#editForm').find('#btnReset').attr('disabled', 'disabled');
        },
        enablebuttons: function () {
            $('#editForm').find('#btnSave').removeAttr('disabled', 'disabled');
            $('#editForm').find('#btnReset').removeAttr('disabled', 'disabled');
        }
    },
    methods: {
        redirect: function (pims_epal_id) {
            if (pims_epal_id != 'null') {
                const appInsightsClient = new TelemetryLogger();
                if (appInsightsClient.isAppInsightsEnabled("Client.PayCodeSearch")) {
                    const logProperties = new LogInfo("Search.redirect", MS_ID, pims_id);
                    appInsightsClient.trackEvent('Client.PayCodeSearch.ViewDetail', logProperties);
                }

                var URL = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + pims_epal_id;
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to detail page, please wait..',
                    timerProgressBar: true,
                    timer: 50000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = URL;
                    }
                })
            }

        },

        redirect_PayCode: function (pims_epal_id) {
            var payCodeVersionDt = $("#txtPayCodeVersionEffectiveDate").val()

            if (payCodeVersionDt == "" || payCodeVersionDt == undefined) {
                var pims_id = pims_epal_id
            }
            else if (payCodeVersionDt != "") {
                var pims_id = pims_epal_id + ',' + kendo.toString(kendo.parseDate(payCodeVersionDt), 'MM-dd-yyyy hh:mm:ss tt')
            }
            var URL = VIRTUAL_DIRECTORY + "/PayCodes/Home/ViewDetail/" + pims_id;

            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 50000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = URL;
                }
            })
        },


        redirect_Detail: function (page) {
            var payCodeVersionDt = $("#txtPayCodeVersionEffectiveDate").val() 
            var pims_id = $("#txtPIMSID").val() + ',' + kendo.toString(kendo.parseDate(payCodeVersionDt), 'MM-dd-yyyy hh:mm:ss tt')
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 50000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    // fix for https://github.com/uhc-actuarial/hcemi-appdev-pims/security/code-scanning/17 MFQ 8/22/2024
                    window.location.href = VIRTUAL_DIRECTORY + "/PayCodes/Home/" + page + "Detail/" + MIApp.Sanitize.encode(pims_id);
                }
            })
        },
        create_pims_id: function () {
            var pims_id = '-'
            var PAYC_BUS_SEG_CD = $("#ddlLOB").val();
            var PAYC_ENTITY_CD = $("#ddlEntity").val();
            var PAYC_PLAN_CD = $("#ddlPlan").val();
            var PAYC_PRODUCT_CD = $("#ddlProduct").val();
            var PAYC_FUNDING = "FI-";
            var PROC_CD = $("#txtProcedureCode").val();

            if (PAYC_BUS_SEG_CD != '') {
                PAYC_BUS_SEG_CD += "-"
                $("#txtBusinessSegment").val(PAYC_BUS_SEG_CD);
            }
            else $("#txtBusinessSegment").val("");
            if (PAYC_ENTITY_CD != '') {
                PAYC_ENTITY_CD += "-"
                $("#txtEntity").val(PAYC_ENTITY_CD);
            }
            else $("#txtEntity").val("");
            if (PAYC_PLAN_CD != '') {
                PAYC_PLAN_CD += "-"
                $("#txtPlan").val(PAYC_PLAN_CD);
            }
            else $("#txtPlan").val('');
            if (PAYC_PRODUCT_CD != '') {
                PAYC_PRODUCT_CD += "-"
                $("#txtProduct").val(PAYC_PRODUCT_CD);
            }
            else $("#txtProduct").val('');
            if (PROC_CD != '') {
                PROC_CD += "-"
            }

            pims_id = PAYC_BUS_SEG_CD + PAYC_ENTITY_CD + PAYC_PLAN_CD + PAYC_PRODUCT_CD + PAYC_FUNDING + PROC_CD.toUpperCase();
            $("#txtPIMSID").val(pims_id.substr(0, pims_id.length - 1));
            return pims_id;
        },
        validation: function () {
            var isPACEffValid = true;
            var isPACExpValid = true;
            var isValid = true;
            var errorMessage = "";
            var activeTabNameAfterValidation = null;
            var payCodeEffectiveDate = new Date($("#dpPayCodeEffectiveDate").val());
            var payCodeExpirationDate = $.trim($("#dpPayCodeExpirationDate").val()) == "" ? Date.parse("01/01/2999") : new Date($("#dpPayCodeExpirationDate").val());
            var payCodeExpirationDateCurrent = new Date($("#txtPayCodeVersionEffectiveDate").val());   
            //payCodeEffectiveDate.setDate(payCodeEffectiveDate.getDate() + 1);

            // BUG 73158 FIX MFQ 9/18/2023
            /*if (payCodeExpirationDateCurrent.isValid() && payCodeEffectiveDate.isValid() && payCodeEffectiveDate < payCodeExpirationDateCurrent) {
                errorMessage += "The new effective date must be at least one day later than the old records expiration date!\n"
                activeTabNameAfterValidation = "#payCodeSummary";
                isValid = false;
            }*/

            //BUG 74469 Add validation for dpPayCodeEffectiveDate mandatory
            if (payCodeEffectiveDate == "" || !kendo.parseDate($('#dpPayCodeEffectiveDate').val())) {
                errorMessage += "Please select a Pay Code Effective Date (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
                isPACEffValid = false;
            }

            if ($('#dpPayCodeExpirationDate').val() != "" && !kendo.parseDate($('#dpPayCodeExpirationDate').val())) {
                errorMessage += "Pay Code Expiration Date is not correct (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
                isPACExpValid = false;
            }

            //BUG 74471 Pay code expiration date should be equals or greater than Pay code Effective date
            if (payCodeExpirationDate < payCodeEffectiveDate) {
                errorMessage += "Pay Code Expiration Date cannot be less than Pay Code Effective Date (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            }

            if ($('.ddKLPCS_label').hasClass('required') && $('#ddKLPCS').val() == '') {
                errorMessage += "Please select KL PCS (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            }

            if ($('.ddNDBPCS_label').hasClass('required') && $('#ddNDBPCS').val() == '') {
                errorMessage += "Please select NDB PCS (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            }
            
            if ($('.ddClinicaliCES_label').hasClass('required') && $('#ddClinicaliCES').val() == '') {
                errorMessage += "Please select Clinical iCES/CES (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            }

            if ($('.ddClinicaliCESName_label').hasClass('required') && $('#ddClinicaliCESName').val() == '') {
                errorMessage += "Please select Clinical iCES/CES Name (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            };
            if ($('.ddClinicaliCESAction_label').hasClass('required') && $('#ddClinicaliCESAction').val() == '') {
                errorMessage += "Please select Clinical iCES/CES Action (Pay Code Summary tab)!\n";
                activeTabNameAfterValidation = "#PayCodeSummary";
                isValid = false;
            }

            if ($("#changeHistory").length) {
                var text = "";
                text = $("#txtChangeSource").val() ?? "";
                if (text.trim() == "") {
                    errorMessage += "Please input Change Source (Change History tab)!\n";
                    activeTabNameAfterValidation ?? (activeTabNameAfterValidation = "#changeHistory");
                    isValid = false;
                }
                else {
                    var regex = new RegExp("®|\'");
                    if ((regex.test(text))) {
                        errorMessage += "Change Source can not have @ or \' (Change History tab)!\n";
                        activeTabNameAfterValidation ?? (activeTabNameAfterValidation = "#changeHistory")
                        isValid = false;
                    }
                }

                text2 = $("#txtChangeDesc").val() ?? "";
                if (text2.trim() == "") {
                    errorMessage += "Please input Change Description (Change History tab)!\n";
                    activeTabNameAfterValidation ?? (activeTabNameAfterValidation = "#changeHistory")
                    isValid = false;
                }
                else {
                    var regex = new RegExp("®|\'");
                    if ((regex.test(text2))) {
                        errorMessage += "Change Description can not have @ or \' (Change History tab)!\n";
                        activeTabNameAfterValidation ?? (activeTabNameAfterValidation = "#changeHistory")
                        isValid = false;
                    }
                }
            }
            return {
                isValid: isValid,
                errorMessage: errorMessage,
                tab: activeTabNameAfterValidation == null ? '' : activeTabNameAfterValidation
            };
        },
        savePayCode: function () {
            var valid = PayCodeSummaryController.methods.validation();
            if (!valid.isValid) {
                PayCodeSummaryController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + valid.errorMessage.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>", "Invalid Entry");
                $('a[href="' + valid.tab + '"]').click();
                return
            }
            if (!$("#changeHistory").length) {
                var PAYC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                var PAYC_ENTITY_CD = $("#txtEntity").val();
                var PAYC_PLAN_CD = $("#txtPlan").val();
                var PAYC_PRODUCT_CD = $("#txtProduct").val();
                // Check null or empty string
                if (
                    PAYC_BUS_SEG_CD?.trim().length > 0 &&
                    PAYC_ENTITY_CD?.trim().length > 0 &&
                    PAYC_PLAN_CD?.trim().length > 0 &&
                    PAYC_PRODUCT_CD?.trim().length > 0 
                ) {
                    $.ajax({
                        type: "GET",
                        url: VIRTUAL_DIRECTORY + '/PayCodes/Home/CheckValidNewPIMSID',
                        data: { pims_id: $("#txtPIMSID").val() },
                        async: false,
                        headers: {
                            'Accept': 'application/json'
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        success: function (result) {
                            if (result.PAYC_HIERARCHY_KEY != null) {
                                PayCodeSummaryController.message.showWarning("Pay Code ID already exists! Please enter an another Pay Code ID.\n", 'Invalid Entry');
                            }
                            else {
                                PayCodeSummaryController.methods.doSavePayCode();
                            } 

                        },
                        error: function () {
                            PayCodeSummaryController.message.showError();
                        }
                    })
                }
                else {
                    isValidNewPIMS_ID = false;
                    PayCodeSummaryController.message.showWarning("Please fill in required fields!", "Required fields")
                }                
            }
            else {
                    PayCodeSummaryController.methods.doSavePayCode();
            }
        },
        saveHistoricalPayCode: function () {
            var valid = PayCodeSummaryController.methods.validation();
            if (!valid.isValid) {
                PayCodeSummaryController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + valid.errorMessage.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>", "Invalid Entry");
                $('a[href="' + valid.tab + '"]').click();
                return
            }
            if (!$("#changeHistory").length) {
                var PAYC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                var PAYC_ENTITY_CD = $("#txtEntity").val();
                var PAYC_PLAN_CD = $("#txtPlan").val();
                var PAYC_PRODUCT_CD = $("#txtProduct").val();
                // Check null or empty string
                if (
                    PAYC_BUS_SEG_CD?.trim().length > 0 &&
                    PAYC_ENTITY_CD?.trim().length > 0 &&
                    PAYC_PLAN_CD?.trim().length > 0 &&
                    PAYC_PRODUCT_CD?.trim().length > 0
                ) {
                    $.ajax({
                        type: "GET",
                        url: VIRTUAL_DIRECTORY + '/PayCodes/Home/CheckValidNewPIMSID',
                        data: { pims_id: $("#txtPIMSID").val() },
                        async: false,
                        headers: {
                            'Accept': 'application/json'
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        success: function (result) {
                            if (result.PAYC_HIERARCHY_KEY != null) {
                                PayCodeSummaryController.message.showWarning("Pay Code ID already exists! Please enter an another Pay Code ID.\n", 'Invalid Entry');
                            }
                            else {
                                PayCodeSummaryController.methods.doSaveHistoricPayCode();
                            }

                        },
                        error: function () {
                            PayCodeSummaryController.message.showError();
                        }
                    })
                }
                else {
                    isValidNewPIMS_ID = false;
                    PayCodeSummaryController.message.showWarning("Please fill in required fields!", "Required fields")
                }
            }
            else {
                PayCodeSummaryController.methods.doSaveHistoricPayCode();
            }
        },
        delete: function () {
            var PIMS_ID = MIApp.Sanitize.string($("#txtPIMSID").val());
            if (PIMS_ID.indexOf(" ") >= 0) {
                PIMS_ID = PIMS_ID.substring(0, PIMS_ID.indexOf(' '))
            }
            var obj = {
                "PAYC_HIERARCHY_KEY": PIMS_ID,
                "CHANGE_REQ_ID": $("#txtCRNumber").val(),
                "CHANGE_DESC": $("#txtDeleleReason").val(),
                "PAYC_VER_EFF_DT": $("#txtPayCodeVersionEffectiveDate").val()
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/PayCodes/Home/DeletePayCodeProcedure",
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
                                    window.location.href = VIRTUAL_DIRECTORY + "/PayCodes";
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
        doSavePayCode: function () {
            var PAYC_BUS_SEG_CD = "";
            var PAYC_ENTITY_CD = "";
            var PAYC_PLAN_CD = "";
            var PAYC_PRODUCT_CD = "";
            var PAYC_FUND_ARNGMNT_CD = "";
            var PROC_CD = "";
            PAYC_BUS_SEG_CD = $("#txtBusinessSegment").val();
            PAYC_ENTITY_CD = $("#txtEntity").val();
            PAYC_PLAN_CD = $("#txtPlan").val();
            PAYC_PRODUCT_CD = $("#txtProduct").val();
            PAYC_FUND_ARNGMNT_CD = "FI";
            PROC_CD = $("#txtProcedureCode").val();
            var obj = {
                "PAYC_BUS_SEG_CD": PAYC_BUS_SEG_CD.replace("-",""),
                "PAYC_ENTITY_CD": PAYC_ENTITY_CD.replace("-", ""),
                "PAYC_PLAN_CD": PAYC_PLAN_CD.replace("-", ""),
                "PAYC_PRODUCT_CD": PAYC_PRODUCT_CD.replace("-", ""),
                "PAYC_PROC_CD": PROC_CD,
                "PAYC_FUND_ARNGMNT_CD": PAYC_FUND_ARNGMNT_CD,
                "PAYC_HIERARCHY_KEY": $("#txtPIMSID").val(),
                "FURTHER_INST": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtFurtherConsideration").val())),
                "PAYC_NOTES": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtNote").val())),
                "CHANGE_REQ_ID": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtChangeSource").val())),
                "CHANGE_DESC": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtChangeDesc").val())),
                "PAYC_KL_PCS": $("#ddKLPCS").val(),
                "PAYC_NDB_PCS": $("#ddNDBPCS").val(),
                "PAYC_NDB_REMARK_CD": $("#ddNDBRemarkCode").val(),
                "PAYC_ICES_IND": $("#ddClinicaliCES").val(),
                "PAYC_ICES_EDIT_NAME": $("#ddClinicaliCESName").val() == null ? '' : $("#ddClinicaliCESName").val().join(','),
                "PAYC_ICES_EDIT_ACTION": $("#ddClinicaliCESAction").val(),
                "PAYC_ADVN_NOTIF": $("#ddAdvancedNotificationRequired").val(),
                "PAYC_MCR_ROUTED": $("#txtRoutedMCR").val(),
                "PAYC_BIFURCATED": $("#ddBifurcatedProcess").val(),
                "PAYC_NS88_COMPLIANCE": $("#ddNS88Compliance").val(),                
                "PAYC_ADDITIONAL_EDITS": $("#ddAdditionalEdits").val() == null ? '' : $("#ddAdditionalEdits").val().join(','), //INC41991865 MFQ 3/11/2025
                "PAYC_COMMENTS": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtComments").val())),
                "PAYC_EFF_DT": $.trim($("#dpPayCodeEffectiveDate").val()),
                "PAYC_EXP_DT": $.trim($("#dpPayCodeExpirationDate").val()),
                "PAYC_PRED_EFF_DT": $.trim($("#dpPayCodePreDeterminationEffDate").val()),
                "PAYC_PRED_EXP_DT": $.trim($("#dpPayCodePreDeterminationExpDate").val()),
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/PayCodes/Home/UpdatePayCodeProcedure",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (updateDto) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'PIMS record has been updated!',
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        isDirty = false;
                        if (!$("#changeHistory").length) {
                            PayCodeSummaryController.methods.redirect_Detail("Edit");
                        }
                        else { 
                            Swal.fire({
                                title: 'Loading..',
                                text: 'Refresh record, please wait..',
                                timerProgressBar: true,
                                timer: 20000,
                                allowOutsideClick: false,
                                willOpen: () => {
                                    Swal.showLoading();

                                    window.location.href = VIRTUAL_DIRECTORY + "/PayCodes/Home/ViewDetail/" + MIApp.Sanitize.encode($("#txtPIMSID").val()) + ',' + kendo.toString(kendo.parseDate(updateDto.ReturnObject), 'MM-dd-yyyy hh:mm:ss tt');
                                }
                            })
                        }
                    })
                },
                error: function () {
                    PayCodeSummaryController.message.showError();
                }
            })
        },
        doSaveHistoricPayCode: function ()
        {
            var PAYC_BUS_SEG_CD = "";
            var PAYC_ENTITY_CD = "";
            var PAYC_PLAN_CD = "";
            var PAYC_PRODUCT_CD = "";
            var PAYC_FUND_ARNGMNT_CD = "";
            var PROC_CD = "";
            PAYC_BUS_SEG_CD = $("#txtBusinessSegment").val();
            PAYC_ENTITY_CD = $("#txtEntity").val();
            PAYC_PLAN_CD = $("#txtPlan").val();
            PAYC_PRODUCT_CD = $("#txtProduct").val();
            PAYC_FUND_ARNGMNT_CD = "FI";
            PROC_CD = $("#txtProcedureCode").val();
            PAYC_VER_EFF_DT = $("#txtPayCodeVersionEffectiveDate").val()
            var obj = {
                "PAYC_BUS_SEG_CD": PAYC_BUS_SEG_CD.replace("-", ""),
                "PAYC_ENTITY_CD": PAYC_ENTITY_CD.replace("-", ""),
                "PAYC_PRODUCT_CD": PAYC_PRODUCT_CD.replace("-", ""),
                "PAYC_VER_EFF_DT": PAYC_VER_EFF_DT, 
                "PAYC_PLAN_CD": PAYC_PLAN_CD.replace("-", ""),
                "PAYC_PROC_CD": PROC_CD,
                "PAYC_KL_PCS": $("#ddKLPCS").val(),
                "PAYC_NDB_PCS": $("#ddNDBPCS").val(),
                "PAYC_NDB_REMARK_CD": $("#ddNDBRemarkCode").val(),
                "PAYC_ICES_IND": $("#ddClinicaliCES").val(),
                "PAYC_ICES_EDIT_ACTION": $("#ddClinicaliCESAction").val(),
                "PAYC_ADVN_NOTIF": $("#ddAdvancedNotificationRequired").val(),
                "PAYC_PRED_EFF_DT": $.trim($("#dpPayCodePreDeterminationEffDate").val()),
                "PAYC_PRED_EXP_DT": $.trim($("#dpPayCodePreDeterminationExpDate").val()),
                "PAYC_MCR_ROUTED": $("#txtRoutedMCR").val(),
                "PAYC_BIFURCATED": $("#ddBifurcatedProcess").val(),
                "PAYC_NS88_COMPLIANCE": $("#ddNS88Compliance").val(),
                "PAYC_ADDITIONAL_EDITS": $("#ddAdditionalEdits").val() == null ? '' : $("#ddAdditionalEdits").val().join(','),
                "PAYC_COMMENTS": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtComments").val())),
                "PAYC_ICES_EDIT_NAME": $("#ddClinicaliCESName").val() == null ? '' : $("#ddClinicaliCESName").val().join(','),
                "PAYC_PRED_IND": null,
                "USER_ID": null,
                "PAYC_EFF_DT": $.trim($("#dpPayCodeEffectiveDate").val()),
                "PAYC_EXP_DT": $.trim($("#dpPayCodeExpirationDate").val()),
                "PAYC_FUND_ARNGMNT_CD": PAYC_FUND_ARNGMNT_CD,
                "CHANGE_REQ_ID": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtChangeSource").val())),
                "CHANGE_DESC": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtChangeDesc").val())),
                "PAYC_FURTHER_CONSIDERATIONS": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtFurtherConsideration").val())),
                "PAYC_NOTES": MIApp.Sanitize.toBase64(encodeURIComponent($("#txtNote").val()))                
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/PayCodes/Home/HistoricalUpdatePayCodeProcedure",
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
                        text: 'PIMS record has been updated!',
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        isDirty = false;
                        if (!$("#changeHistory").length) {
                            PayCodeSummaryController.methods.redirect_Detail("Edit");
                        }
                        else {
                            Swal.fire({
                                title: 'Loading..',
                                text: 'Refresh record, please wait..',
                                timerProgressBar: true,
                                timer: 20000,
                                allowOutsideClick: false,
                                willOpen: () => {
                                    Swal.showLoading();
                                    // MFQ 8/22/2024 vulnerability fix for https://github.com/uhc-actuarial/hcemi-appdev-pims/security/code-scanning/19
                                    window.location.href = VIRTUAL_DIRECTORY + "/PayCodes/Home/ViewDetail/" + MIApp.Sanitize.encode($.find("#txtPIMSID")[0].value + ',' + kendo.toString(kendo.parseDate(PAYC_VER_EFF_DT), 'MM-dd-yyyy hh:mm:ss tt'));
                                }
                            })
                        }
                    })
                },
                error: function () {
                    PayCodeSummaryController.message.showError();
                }
            })
        }
    },
    message: {
        showSuccess: function () {
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Something Success!',
                customClass: 'swal-size-sm'
            })
        },
        showWarning: function (message,title) {
            Swal.fire({
                icon: 'warning',
                title: title,
                html: message,
                customClass: 'swal-size-sm',
                width: '750px'
            })
        },
        showError: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
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
var form_original_data
var form_new_data
var isDirty = false
$(document).ready(function () {
    PayCodeSummaryController.init(); 
    $(window).bind('beforeunload', function () {
        if (isDirty) {
            return 'please save your setting before leaving the page.';
        }
    });

});

