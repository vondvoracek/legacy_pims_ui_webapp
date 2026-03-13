
var _AddNewDetailController = {
    forceHidden: false,
    init: function () {
        _AddNewDetailController.bind();
    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        $('#addNewModal').on('shown.bs.modal', function () {
            $(document).off('focusin.modal');
        });

        $('#validate-add-new-record-modal').on('shown.bs.modal', function () {
            $(document).off('focusin.modal');
        });

        $('.validate-add-new-record-modal-close').click(function () {
            if (MIApp.Common.Enum.screenType == MIApp.Common.Enum.screenTypeValues.search && _AddNewDetailController.forceHidden) {
                _AddNewDetailController.forceHidden = false;
                $('#addNewModal').modal('show');
            }
            $('#validate-add-new-record-modal').modal('hide');
        });

        $("#chkDefault").click(function () {
            if ($('#chkDefault').is(':checked')) {
                _AddNewDetailController.checkbox.disableDrugName();
                var pims_id = _AddNewDetailController.methods.create_pims_id()
                var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtPIMSID_Display").val(pims_id_display);
            }
            else {
                _AddNewDetailController.checkbox.enableDrugName();
                var pims_id = _AddNewDetailController.methods.create_pims_id()
                var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
                $("#txtPIMSID").val(pims_id);
                $("#txtPIMSID_Display").val(pims_id_display);
            }
        });

        $(".resetAddNew").click(function () {
            $('#addNewForm').trigger("reset");
            _AddNewDetailController.checkbox.disableDrugName();
        });

        $(".validate-add-new-record-modal-close").click(function () {
            $('#addNewModal').modal('show');
        })


        $('#btnValidate, #btnValidatePopup').click(function () {
            var isValidNewPIMS_ID = _AddNewDetailController.methods.HandlePIMSValidation();

            if (isValidNewPIMS_ID == true) {
                //var pims_id = _AddNewDetailController.methods.create_pims_id();
                //_AddNewDetailController.methods.validate.addDetail(pims_id);
            }
        })

        $("#txtDrugName_AddNew").on('change keyup', function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);
        });

        $("#txtDrugName_AddNew").on('keypress', MIApp.EPAL.validateDrugNameOnTyping);

        $('.update-existing').click(function () {
            var pims_id = MIApp.Sanitize.string($("#txtPIMSID_Display").val());// fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/75 MFQ 7/17/2023
            _AddNewDetailController.methods.redirect_editDetail(pims_id);
        });

        $('.duplicate').click(function () {
            var pims_id = MIApp.Sanitize.string($("#txtPIMSID_Display").val()); // fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/76 MFQ 7/17/2023
            _AddNewDetailController.methods.redirect_duplicateDetail(pims_id);
        });

        $('.view-existing').click(function () {
            var pims_id = MIApp.Sanitize.string($("#txtPIMSID_Display").val());// fix MFQ 7/17/2023
            _AddNewDetailController.methods.redirect_viewDetail(pims_id);
        });
        _AddNewDetailController.checkbox.disableDrugName();

        $('#form').on('shown.bs.modal', function () { $(document).off('focusin.modal'); });
    },
    dropdown: {       
        refreshDropDownList: function () {
            var ddlLob = $("#ddlLOB_AddNew").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct_AddNew").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan_AddNew").data('kendoDropDownList');
            var ddlFundingArrangement = $("#ddlFundingArrangement_AddNew").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity_AddNew").data('kendoDropDownList');

            ddlLob.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlFundingArrangement.dataSource.read();
            ddlEntity.dataSource.read();

        },

        onChange_LOB: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);
            var param = _AddNewDetailController.dropdown.param.All();
            param.EPAL_FUND_ARNGMNT_CD = "";
            $("#ddlFundingArrangement_AddNew").val('');
            param.EPAL_PRODUCT_CD = "";
            $("#ddlProduct_AddNew").val('');
            param.EPAL_PLAN_CD = "";
            $("#ddlPlan_AddNew").val('');

            _AddNewDetailController.dropdown.refreshDropDownList();

            //if ($("#ddlLOB_AddNew").val() != "") {
            //    $('#ddlEntity_AddNew').getKendoDropDownList().enable(true);
            //}
        },

        onChange_Entity: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            var param = _AddNewDetailController.dropdown.param.All();
            param.EPAL_FUND_ARNGMNT_CD = "";
            param.EPAL_PRODUCT_CD = "";
            param.EPAL_PLAN_CD = "";
            var statesArray = $('#ValidStatesList').val() != undefined ? $('#ValidStatesList').val().split(','): [];    

            // DevOps BUG 34168 - MFQ 9/19/2022
            if ($('#epal-page-view').val().toLowerCase() == 'adddetail' || $('#epal-page-view').val().toLowerCase() == 'editdetail') {                

                var isBusinessSegmentCnsIfp = $("#txtBusinessSegment").val().toLowerCase() == 'cns' || $("#txtBusinessSegment").val().toLowerCase() == 'ifp';
                var entityValue = $('#txtEntity').val();

                if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) > -1) {

                    hideStateAddToolbar();

                } else if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) < 0) {

                    showStateAddToolbar();

                } else if (!isBusinessSegmentCnsIfp) {

                    showStateAddToolbar();
                }
            } else if ($('#epal-page-view').val().toLowerCase() == 'addnew') {

                var isBusinessSegmentCnsIfp = $("#ddlLOB_AddNew").data('kendoDropDownList').value().toLowerCase() == 'cns' || $("#ddlLOB_AddNew").data('kendoDropDownList').value().toLowerCase() == 'ifp';
                var entityValue = $('#ddlEntity_AddNew').data('kendoDropDownList').value();

                if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) < 0) {

                    showStateAddToolbar();

                } else if (isBusinessSegmentCnsIfp &&
                    statesArray.indexOf(entityValue) > -1) {

                    hideStateAddToolbar();

                } else if (!isBusinessSegmentCnsIfp) {

                    showStateAddToolbar();

                }    
            }

            function showStateAddToolbar() {
                $("#grid_StateInfo .k-grid-toolbar").show();                
            }
            function hideStateAddToolbar() {
                $("#grid_StateInfo .k-grid-toolbar").hide();                
            }

            _AddNewDetailController.dropdown.refreshDropDownList();            
        },

        onChange_Plan: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            var param = _AddNewDetailController.dropdown.param.All();
            param.EPAL_FUND_ARNGMNT_CD = "";
            param.EPAL_PRODUCT_CD = "";

            _AddNewDetailController.dropdown.refreshDropDownList();
            //if ($("#ddlPlan_AddNew").val() != "") {
            //    $('#ddlProduct_AddNew').getKendoDropDownList().enable(true);
            //}
        },

        onChange_Product: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            var param = _AddNewDetailController.dropdown.param.All();
            param.EPAL_FUND_ARNGMNT_CD = "";
            _AddNewDetailController.dropdown.refreshDropDownList();

            //if ($("#ddlLOB_AddNew").val() != "") {
            //    $('#ddlEntity_AddNew').getKendoDropDownList().enable(true);
            //}
        },

        onChange_FundingArrangement: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);

            _AddNewDetailController.dropdown.refreshDropDownList();
            //if ($("#ddlProduct_AddNew").val() != "") {
            //    $('#ddlFundingArrangement_AddNew').getKendoDropDownList().enable(true);
            //}
        },

        onChange_txtProcedureCode_AddNew: function () {
            var pims_id = _AddNewDetailController.methods.create_pims_id()
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)
            $("#txtPIMSID").val(pims_id);
            $("#txtPIMSID_Display").val(pims_id_display);
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
                    EPAL_BUS_SEG_CD: $("#ddlLOB_AddNew").val()
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
                    EPAL_BUS_SEG_CD: $("#ddlLOB_AddNew").val()
                }
            },
            APP_FUNDING_ARRANGEMENT: function () {
                return {
                    COLUMN_NAME: "EPAL_FUND_ARNGMNT_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB_AddNew").val()
                }
            },
            APP_ENTITY: function () {                
                return {
                    COLUMN_NAME: "EPAL_ENTITY_CD",
                    EPAL_BUS_SEG_CD: $("#ddlLOB_AddNew").val()                     
                }
            },

            All_APP_LOB: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";

                var textValue = $("#ddlLOB_AddNew").data("kendoDropDownList").text();
                var filter = $('#ddlLOB_AddNew').data('kendoDropDownList').dataSource.filter();

                if (filter && filter.filters[0] && filter.filters[0].operator == "contains") {
                    textValue = filter.filters[0].value;
                }
                if (textValue !== '------') param.EPAL_BUS_SEG_CD = textValue;

                return param;
            },
            All_APP_ENTITY: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_ENTITY_CD";

                var textValue = $("#ddlEntity_AddNew").data("kendoDropDownList").text();
                var filter = $('#ddlEntity_AddNew').data('kendoDropDownList').dataSource.filter();

                if (filter && filter.filters[0] && filter.filters[0].operator == "contains") {
                    textValue = filter.filters[0].value;
                }
                if (textValue !== '------') param.EPAL_ENTITY_CD = textValue;
                return param;
            },
            All_APP_PLAN: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PLAN_CD";

                var textValue = $("#ddlPlan_AddNew").data("kendoDropDownList").text();
                var filter = $('#ddlPlan_AddNew').data('kendoDropDownList').dataSource.filter();

                if (filter && filter.filters[0] && filter.filters[0].operator == "contains") {
                    textValue = filter.filters[0].value;
                }
                if (textValue !== '------') param.EPAL_PLAN_CD = textValue;
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PRODUCT_CD";
                var textValue = $("#ddlProduct_AddNew").data("kendoDropDownList").text();
                var filter = $('#ddlProduct_AddNew').data('kendoDropDownList').dataSource.filter();

                if (filter && filter.filters[0] && filter.filters[0].operator == "contains") {
                    textValue = filter.filters[0].value;
                }
                if (textValue !== '------') param.EPAL_PRODUCT_CD = textValue;
                return param;
            },
            All_APP_FUNDING_ARRANGEMENT: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_FUND_ARNGMNT_CD";
                var textValue = $("#ddlFundingArrangement_AddNew").data("kendoDropDownList").text();
                var filter = $('#ddlFundingArrangement_AddNew').data('kendoDropDownList').dataSource.filter();

                if (filter && filter.filters[0] && filter.filters[0].operator == "contains") {
                    textValue = filter.filters[0].value;
                }
                if (textValue !== '------') param.EPAL_FUND_ARNGMNT_CD = textValue;
                return param;
            },
            All: function () {
                return {
                    EPAL_BUS_SEG_CD: $("#ddlLOB_AddNew").val(),
                    EPAL_ENTITY_CD: $("#ddlEntity_AddNew").val(),
                    EPAL_PLAN_CD: $("#ddlPlan_AddNew").val(),
                    EPAL_PRODUCT_CD: $("#ddlProduct_AddNew").val(),
                    EPAL_FUND_ARNGMNT_CD: $("#ddlFundingArrangement_AddNew").val()
                }
            }
        },
    },
    checkbox: {
        disableDrugName: function () {
            $('#txtDrugName_AddNew').attr('readonly', true);
            $('#txtDrugName_AddNew').val('NA');
        },

        enableDrugName: function () {
            $('#txtDrugName_AddNew').attr('readonly', false);
            $('#txtDrugName_AddNew').val('');
            $('#txtDrugName_AddNew').focus();
        },
    },
    methods: {
        redirect_addDetail: function (pims_id) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 5000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/AddDetail/" + encodeURIComponent(pims_id);
                }
            })
        },
        redirect_viewDetail: function (pims_id) {

            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 5000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                }
            })
        },
        redirect_editDetail: function (pims_id) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 5000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);
                }
            })
        },
        redirect_duplicateDetail: function (pims_id) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 5000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/DuplicateRecordDetail/" + encodeURIComponent(pims_id);
                }
            })
        },
        create_pims_id: function () {
            var pims_id = ''
            var EPAL_BUS_SEG_CD = $("#ddlLOB_AddNew").val();
            var EPAL_ENTITY_CD = $("#ddlEntity_AddNew").val();
            var EPAL_PLAN_CD = $("#ddlPlan_AddNew").val();
            var EPAL_PRODUCT_CD = $("#ddlProduct_AddNew").val();
            var EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement_AddNew").val();
            var PROC_CD = $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value();//$("#txtProcedureCode_AddNew").val();
            var DRUG_NM = $("#txtDrugName_AddNew").val();


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
        validateICHRAUserRole: function () {
            var hasRole = false
            var product = $("#ddlProduct_AddNew").val();
            var array = $('#txtRoleIds').val().split(",");
                    $.each(array, function (i) {
                        if (array[i] == "1") {
                            hasRole = true;
                        }
                        else {
                            if (array[i] == "27" && product == "ICHRA") {
                                hasRole = true;
                            }
                            if (array[i] == "28" && product == "ICHRA") {
                                hasRole = true;
                            }
                        }

                    });

            //console.log(array);
            //console.log("hasRole: " + hasRole);
            return hasRole;
        },

        validateUMRUserRole: function () {
            var hasRole = false
            var entity = $("#ddlEntity_AddNew").val();
            var array = $('#txtRoleIds').val().split(",");
            $.each(array, function (i) {
                if (array[i] == "1") {
                    hasRole = true;
                }
                else {
                    if (array[i] == "12" && entity == "UMR") {
                        hasRole = true;
                    }
                    if (array[i] == "13" && entity == "UMR") {
                        hasRole = true;
                    }
                    if (array[i] == "3" && entity !== "UMR") {
                        hasRole = true;
                    }
                    if (array[i] == "4" && entity !== "UMR") {
                        hasRole = true;
                    }
                }

            });

            //console.log(array);
            //console.log("hasRole: " + hasRole);
            return hasRole;
        },

        CheckValidProcedureCode: function () {
            var isActive = "";
            var token = $('meta[name="request-verification-token"]')[0].content;
            $.ajax({
                url: VIRTUAL_DIRECTORY + '/EPAL/Home/GetEPALProcStatus',
                dataType: 'JSON',
                data: { p_PROC_CD: $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value().toUpperCase() },
                type: 'POST',
                async: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (result) {
                    isActive = result.IPROC_STATUS
                },
                error: function () {
                    isActive = "Inactive";
                }
            })

          return isActive;

        },
        CheckValidNewPIMS_ID: function () {
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
                    _AddNewDetailController.message.showError();
                }
            })
            return IsExists

        },
        HandlePIMSValidation: function () {
            var isValidNewPIMS_ID = true;
            var PIMSHierarchyCodesExist = true;

            //User Story 129499 MFQ 5/21/2025
            if (!MIApp.EPAL.invalidDrugNamePattern($("#txtDrugName_AddNew").val())) {
                _AddNewDetailController.message.showWarning("Invalid text in Drug name, please enter a valid drug name!\nOnly alpha numeric along with ~ _ allowed!")
                isValidNewPIMS_ID = false;
            }

            // Check valid procedure code
            var result = _AddNewDetailController.methods.CheckValidProcedureCode();
           
            if (result == null || result == "Inactive") {
                isValidNewPIMS_ID = false;
                _AddNewDetailController.message.showWarning("Invalid Procedure Code! Please enter a valid procedure code.")
            }


            // Check valid hierarchy code combination
            PIMSHierarchyCodesExist = _AddNewDetailController.methods.checkPIMSHierarchyCodeCombinationExists();
            if (PIMSHierarchyCodesExist == false) {
                _AddNewDetailController.message.showWarning("Invalid PIMS ID hierarchy code combination. Please select another value!")
                isValidNewPIMS_ID = false;
            }

            // Check UMR Role Access
            HasUMRRole = _AddNewDetailController.methods.validateUMRUserRole();
            var entity = $("#ddlEntity_AddNew").val();
            if (HasUMRRole === false && entity=='UMR') {
                _AddNewDetailController.message.showWarning("You do not have access to update this UMR record type! Please select another entity value!")
                isValidNewPIMS_ID = false;
            }

            // Check ICHRA Role Access
            HasICHRARole = _AddNewDetailController.methods.validateICHRAUserRole();
            var product = $("#ddlProduct_AddNew").val();
            if (HasICHRARole === false && product=='ICHRA') {
                _AddNewDetailController.message.showWarning("You do not have access to update this ICHRA record type! Please select another product value!")
                isValidNewPIMS_ID = false;
            }

            if (isValidNewPIMS_ID) {
                var EPAL_BUS_SEG_CD = $("#ddlLOB_AddNew").val();
                var EPAL_ENTITY_CD = $("#ddlEntity_AddNew").val();
                var EPAL_PLAN_CD = $("#ddlPlan_AddNew").val();
                var EPAL_PRODUCT_CD = $("#ddlProduct_AddNew").val();
                var EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement_AddNew").val();
                var PROC_CD = $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value();

                // Check null or empty string
                if (
                    EPAL_BUS_SEG_CD?.trim().length > 0 &&
                    EPAL_ENTITY_CD?.trim().length > 0 &&
                    EPAL_PLAN_CD?.trim().length > 0 &&
                    EPAL_PRODUCT_CD?.trim().length > 0 &&
                    EPAL_FUND_ARNGMNT_CD?.trim().length > 0 &&
                    PROC_CD?.trim().length > 0
                ) {
                    var pimsExistResult = _AddNewDetailController.methods.CheckValidNewPIMS_ID();
                    console.log(pimsExistResult)
                    if (pimsExistResult != null && pimsExistResult == "Exists") {
                        isValidNewPIMS_ID = false;
                        //alert("PIMS ID Exist!");
                        $('#addNewModal').modal('hide');
                        $('#validate-add-new-record-modal').modal('show');
                    }
                    else {
                        _AddNewDetailController.methods.redirect_addDetail(MIApp.Sanitize.string($("#txtPIMSID").val())); // fix MFQ 7/17/2023
                    }
                }
                else {
                    isValidNewPIMS_ID = false;
                    _AddNewDetailController.message.showWarning("Please fill in required fields!")
                }


            }

            return isValidNewPIMS_ID;
        },
        checkPIMSHierarchyCodeCombinationExists: function () {
            var EPAL_BUS_SEG_CD = "";
            var EPAL_ENTITY_CD = "";
            var EPAL_PLAN_CD = "";
            var EPAL_PRODUCT_CD = "";
            var EPAL_FUND_ARNGMNT_CD = "";
            var isExists = true;

            EPAL_BUS_SEG_CD = $("#ddlLOB_AddNew").val();
            EPAL_ENTITY_CD = $("#ddlEntity_AddNew").val();
            EPAL_PLAN_CD = $("#ddlPlan_AddNew").val();
            EPAL_PRODUCT_CD = $("#ddlProduct_AddNew").val();
            EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement_AddNew").val();

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
                    _AddNewDetailController.message.showError();
                }
            });

            return isExists;
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
    _AddNewDetailController.init();
});