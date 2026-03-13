
var _DetailHeaderDuplicateRecordController = {
    init: function () {
        _DetailHeaderDuplicateRecordController.bind();
    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })


        $("#chkDefault").click(function () {
            if ($('#chkDefault').is(':checked')) {
                _DetailHeaderDuplicateRecordController.checkbox.disableDrugName();
                var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
                var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtPIMSID_Display").val(pims_id_display);
            }
            else {
                _DetailHeaderDuplicateRecordController.checkbox.enableDrugName();
                var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
                var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtPIMSID_Display").val(pims_id_display);
            }
        });


        $("#btnGotoDetail").click(function () {
            //var pims_id = MIApp.Sanitize.string($("#hidden-EPAL_HIERARCHY_KEY").val()); // fix MFQ 7/17/2023
            var pims_id = $("#hidden-EPAL_HIERARCHY_KEY").val();
            var EPALVersionDt = $("#txtOrigEPALVersionDt").val();

            _DetailHeaderDuplicateRecordController.redirect.redirect_editDetail(pims_id + ',' + kendo.toString(kendo.parseDate(EPALVersionDt), 'MM-dd-yyyy hh:mm:ss tt') );
        });

        $("#txtProcedureCode").on('change', function () {
            $("#txtPIMSID").val(_DetailHeaderDuplicateRecordController.methods.create_pims_id());
        });

        $("#txtDrugName").on('change keyup', function () {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);
        });

        $("#txtDrugName").on('keypress', MIApp.EPAL.validateDrugNameOnTyping);

        _DetailHeaderDuplicateRecordController.checkbox.disableDrugName();

        // Reset pims
        $("#txtPIMSID").val("");

    },
    dropdown: {
        refreshDropDownList: function () {
            var ddlLob = $("#ddlLOB").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan").data('kendoDropDownList');
            var ddlFundingArrangement = $("#ddlFundingArrangement").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity").data('kendoDropDownList');

            ddlLob.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlFundingArrangement.dataSource.read();
            ddlEntity.dataSource.read();

            //$("#ddlEntity").data('kendoDropDownList').unbind('change').bind('change', _DetailHeaderDuplicateRecordController.dropdown.onChange_Entity);
        },

        onChange_LOB: function (e) {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _DetailHeaderDuplicateRecordController.dropdown.refreshDropDownList();

            /* USER STORY 32497 - MFQ 11/2/2022 FIX - Added 'ifp' condition
               If LOB == CNS OR IFP then remove State Info requirement validation */
            if ((e.sender.dataItem().VV_CD.toLowerCase() == 'cns' || e.sender.dataItem().VV_CD.toLowerCase() == 'ifp')) {
                $('#stateInfo-tab').addClass('required');
            } else {
                $('#stateInfo-tab').removeClass('required');
            }

            /* 
            * DevOps BUG 34168 - MFQ 9/19/2022
            * Add State Info button to Add/Edit/Duplicate Screens for State Information tab
            * if LOB = 'CnS' and Entity = 'CnS' 
            *   display the Add State button and allow users to add multiple states
            * else if LOB ='CnS' and Entity not 'CnS' 
            *   and displays and actual state, then do not display add new button.  
            * else if LOB <>'CnS' 
            *   display add new button. 
            */
            if ($('#epal-page-view').val().toLowerCase() == 'duplicaterecorddetail') {
                StateInfoController.methods.renderStateInfoOnDuplicateRecordDetail($("#grid_StateInfo").data("kendoGrid"));
            }
        },

        onChange_Entity: function (e) {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _DetailHeaderDuplicateRecordController.dropdown.refreshDropDownList();

            var ddlEntity = $("#ddlEntity").data('kendoDropDownList');
            var statesArray = $('#ValidStatesList').val().split(',');     
            //&& statesArray.indexOf(ddlEntity.value())

            //e.sender.dataItem().COLUMN_NAME.toLowerCase() != 'cns'
            /* USER STORY 32497 - MFQ 11/2/2022 FIX - Added 'ifp' condition*/
            if (ddlEntity.value().length > 0) {
                if (($("#ddlLOB").data('kendoDropDownList').value().toLowerCase() == 'cns' || $("#ddlLOB").data('kendoDropDownList').value().toLowerCase() == 'ifp') && statesArray.indexOf(ddlEntity.value()) > -1) {
                    $('#stateInfo-tab').addClass('required');
                } else {
                    $('#stateInfo-tab').removeClass('required');
                } 
            }

            /* 
            * DevOps BUG 34168 - MFQ 9/19/2022
            * Add State Info button to Add/Edit/Duplicate Screens for State Information tab
            * if LOB = 'CnS' and Entity = 'CnS' 
            *   display the Add State button and allow users to add multiple states
            * else if LOB ='CnS' and Entity not 'CnS' 
            *   and displays and actual state, then do not display add new button.  
            * else if LOB <>'CnS' 
            *   display add new button. 
            */
            if ($('#epal-page-view').val().toLowerCase() == 'duplicaterecorddetail') {
                StateInfoController.methods.renderStateInfoOnDuplicateRecordDetail($("#grid_StateInfo").data("kendoGrid"));
            }

            StateInfoController.grid.refreshGrid();
        },

        onChange_Product: function () {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _DetailHeaderDuplicateRecordController.dropdown.refreshDropDownList();
        },

        onChange_Plan: function () {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _DetailHeaderDuplicateRecordController.dropdown.refreshDropDownList();
        },

        onChange_FundingArrangement: function () {
            var pims_id = _DetailHeaderDuplicateRecordController.methods.create_pims_id()
            var pims_id_display = _DetailHeaderDuplicateRecordController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _DetailHeaderDuplicateRecordController.dropdown.refreshDropDownList();
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
        },

        param: {
            APP_PRODUCT: function () {
                return {
                    COLUMN_NAME: "EPAL_PRODUCT_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB").val()
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
                    COLUMN_NAME: "EPAL_PLAN_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB").val()
                }
            },
            APP_FUNDING_ARRANGEMENT: function () {
                return {
                    COLUMN_NAME: "EPAL_FUND_ARNGMNT_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB").val()
                }
            },
            APP_ENTITY: function () {
                return {
                    COLUMN_NAME: "EPAL_ENTITY_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB").val()
                }
            },

            All_APP_LOB: function () {
                var param = _DetailHeaderDuplicateRecordController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";
                return param;
            },
            All_APP_ENTITY: function () {
                var param = _DetailHeaderDuplicateRecordController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_ENTITY_CD";
                return param;
            },
            All_APP_PLAN: function () {
                var param = _DetailHeaderDuplicateRecordController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PLAN_CD";
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = _DetailHeaderDuplicateRecordController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PRODUCT_CD";
                return param;
            },
            All_APP_FUNDING_ARRANGEMENT: function () {
                var param = _DetailHeaderDuplicateRecordController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_FUND_ARNGMNT_CD";
                return param;
            },
            All: function () {
                return {
                    EPAL_BUS_SEG_CD: $("#ddlLOB").val(),
                    EPAL_ENTITY_CD: $("#ddlEntity").val(),
                    EPAL_PLAN_CD: $("#ddlPlan").val(),
                    EPAL_PRODUCT_CD: $("#ddlProduct").val(),
                    EPAL_FUND_ARNGMNT_CD: $("#ddlFundingArrangement").val()
                }
            }
        },
    },
    checkbox: {
        disableDrugName: function () {
            $('#txtDrugName').attr('readonly', true);
            $('#txtDrugName').val('NA');

            if ($('#txtDrugName').val()=='NA') {
                $('#lblDrugReviewAtLaunch').removeClass('required') 
            }
        },

        enableDrugName: function () {
            $('#txtDrugName').attr('readonly', false);
            $('#txtDrugName').val('');
            if ($('#txtDrugName').val() == '') {
                $('#lblDrugReviewAtLaunch').addClass('required')     
            }
            $('#txtDrugName').focus();
        },
    },
    methods: {
        create_pims_id: function () {
            var pims_id = ''
            var EPAL_BUS_SEG_CD = $("#ddlLOB").val();
            var EPAL_ENTITY_CD = $("#ddlEntity").val();
            var EPAL_PLAN_CD = $("#ddlPlan").val();
            var EPAL_PRODUCT_CD = $("#ddlProduct").val();
            var EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
            var PROC_CD = $("#txtProcedureCode").val();
            var DRUG_NM = $("#txtDrugName").val();


            if (EPAL_BUS_SEG_CD != '') {
                EPAL_BUS_SEG_CD += "-"
            }
            if (EPAL_ENTITY_CD != '') {
                EPAL_ENTITY_CD += "-"
            }
            if (EPAL_PLAN_CD != '') {
                EPAL_PLAN_CD += "-"
            }
            if (EPAL_PRODUCT_CD != '') {
                EPAL_PRODUCT_CD += "-"
            }
            if (EPAL_FUND_ARNGMNT_CD != '') {
                EPAL_FUND_ARNGMNT_CD += "-"
            }
            if (PROC_CD != '') {
                PROC_CD += "-"
            }

            pims_id = EPAL_BUS_SEG_CD + EPAL_ENTITY_CD + EPAL_PLAN_CD + EPAL_PRODUCT_CD + EPAL_FUND_ARNGMNT_CD + PROC_CD.toUpperCase() + DRUG_NM.toUpperCase().trim();
            return pims_id;
        },
        pims_id_drugname_parse: function (pims_id) {
            if (pims_id.indexOf(" ") > 0) {
                return pims_id = pims_id.substring(0, pims_id.indexOf(' '));
            }

            else {
                return pims_id;
            }
        },
        checkValidNewPIMS_ID: function () {
            var IsExists = "";
            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + '/EPAL/Home/GetPIMS_IDExistStatus',
                data: { p_EPAL_HIERARCHY_KEY: $("#txtPIMSID_Display") == null ? ("#txtPIMSID").val() : $("#txtPIMSID_Display").val() },
                async: false,
                headers: {
                    'Accept': 'application/json'
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (result) {
                    IsExists = result.APP_AS;       
                },
                error: function () {
                    _DetailHeaderDuplicateRecordController.message.showError();
                }
            })
            return IsExists
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
                EPAL_BUS_SEG_CD = $("#ddlLOB").val();
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

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/EPAL/Home/CheckPIMSHierarchyCodeCombinationExists",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                     'RequestVerificationToken': token
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
                    _DetailHeaderDuplicateRecordController.message.showError();
                }
            });

            return isExists;
        },
        validateDropdowns: function () {
            var isValid = true;
            var errorMessage = "";

            if ($("#ddlLOB").val() == 0) {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please select an LOB!");
                errorMessage += "Please select an LOB!\n";
                isValid = false;
            }

            if ($("#ddlEntity").val() == 0) {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please select an Entity!");
                errorMessage += "Please select an Entity!\n";
                isValid = false;
            }

            if ($("#ddlPlan").val() == 0) {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please select a Plan!");
                errorMessage += "Please select a Plan!\n";
                isValid = false;
            }

            if ($("#ddlProduct").val() == 0) {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please select a Product!");
                errorMessage += "Please select a Product!\n";
                isValid = false;
            }

            if ($("#ddlFundingArrangement").val() == 0) {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please select a Funding Arrangement!");
                errorMessage += "Please select a Funding Arrangement!\n";
                isValid = false;
            }

            if ($("#txtDrugName").val() == '') {
                // _DetailHeaderDuplicateRecordController.message.showWarning("Please enter a Drug Name!");
                errorMessage += "Please enter a Drug Name!\n";
                isValid = false;
            }
            
            if (!MIApp.EPAL.invalidDrugNamePattern($("#txtDrugName").val())) {
                errorMessage += "Drug name can only have alpha numeric ~ and _\n";
                isValid = false;
            }

            var IsExists = _DetailHeaderDuplicateRecordController.methods.checkValidNewPIMS_ID();
            if (IsExists == "Exists") {
                // _DetailHeaderDuplicateRecordController.message.showWarning("PIMS ID already exists! Please enter an another PIMS ID.");
                errorMessage += "PIMS ID already exists! Please enter an another PIMS ID.\n";
                isValid = false;
            }                

            // Check valid hierarchy code combination
            if ($("#ddlLOB").val().length > 0 && $("#ddlEntity").val().length > 0 && $("#ddlPlan").val().length > 0 && $("#ddlProduct").val().length > 0 && $("#ddlFundingArrangement").val().length > 0) {
                PIMSHierarchyCodesExist = _DetailHeaderDuplicateRecordController.methods.checkPIMSHierarchyCodeCombinationExists();
                if (PIMSHierarchyCodesExist == false) {
                    errorMessage += "Invalid PIMS ID hierarchy code combination. Please select another value!\n";
                    isValidNewPIMS_ID = false;
                }
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            return result;
        }
        
    },

    redirect: {
        redirect_editDetail: function (pims_id) {
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
        showWarning: function (message) {
            Swal.fire({
                icon: 'warning',
                title: 'Invalid Entry',
                text: message,
                customClass: 'swal-size-sm'
            })
        },
        showError: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Opps, an error occurred! Please contact the system administrators.',
                customClass: 'swal-size-sm'
            })
        }
    }
}

$(document).ready(function () {
    _DetailHeaderDuplicateRecordController.init();
});