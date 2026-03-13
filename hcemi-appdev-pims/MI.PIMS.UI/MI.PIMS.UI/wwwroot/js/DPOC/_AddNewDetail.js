
var _AddNewDetailController = {
    forceHidden: false,
    init: function () {
        _AddNewDetailController.bind();
    },
    bind: function () {

        $("#chkDefault").click(function () {
            if ($('#chkDefault').is(':checked')) {
                MIApp.DPOC.disableDrugName();
                var pims_id = MIApp.DPOC.create_pims_id();
                $("#txtPIMSID").val(pims_id);
            }
            else {
                MIApp.DPOC.enableDrugName();
                var pims_id = MIApp.DPOC.create_pims_id();                
                $("#txtPIMSID").val(pims_id);
            }
        }); 

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

        $(".resetAddNew").click(function () {
            $('#addNewForm').trigger("reset");
        });

        $(".validate-add-new-record-modal-close").click(function () {
            $('#addNewModal').modal('show');
        })


        $('#btnValidate, #btnValidatePopup').click(function () {
           _AddNewDetailController.methods.HandlePIMSValidation();

        })
        $('.update-existing').click(function () {
            _AddNewDetailController.methods.redirect_Detail("Edit", MIApp.Sanitize.string($('#txtDPOCPackage_New').val()));
        });

        $('.duplicate').click(function () {
            _AddNewDetailController.methods.redirect_Detail("DuplicateRecord", MIApp.Sanitize.string($('#txtDPOCPackage_New').val()));
        });

        $('.view-existing').click(function () {
            _AddNewDetailController.methods.redirect_Detail("View", MIApp.Sanitize.string($('#txtDPOCPackage_New').val()));
        });

        $("#txtDrugName").on('change keyup', function () {
            var pims_id = MIApp.DPOC.create_pims_id();
            var pims_id_display = _AddNewDetailController.methods.pims_id_drugname_parse(pims_id)            
            $("#txtPIMSID").val(pims_id_display);
        });
    },        
    dropdown: {
        refreshDropDownList: function () {
            var ddlBusinessSegment = $("#ddlBusinessSegment").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan").data('kendoDropDownList');
            var ddlFundingArrangement = $("#ddlFundingArrangement").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity").data('kendoDropDownList');
            var ddlPackage = $("#txtDPOCPackage_New").data('kendoDropDownList'); 
            ddlBusinessSegment.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlFundingArrangement.dataSource.read();
            ddlEntity.dataSource.read();
            ddlPackage.dataSource.read();
        },

        onChange_LOB: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
            _AddNewDetailController.dropdown.refreshDropDownList();
        },

        onChange_Entity: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
            _AddNewDetailController.dropdown.refreshDropDownList();         
        },

        onChange_Plan: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
            _AddNewDetailController.dropdown.refreshDropDownList();
           
        },
        onChange_Product: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
            _AddNewDetailController.dropdown.refreshDropDownList();
        },

        onChange_FundingArrangement: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
            _AddNewDetailController.dropdown.refreshDropDownList();
        },

        onChange_txtProcedureCode: function (e) {
            $('#txtPIMSID').val(MIApp.DPOC.create_pims_id());
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
            All_APP_LOB: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";
                return param;
            },
            All_APP_ENTITY: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_ENTITY_CD";
                return param;
            },
            All_APP_PLAN: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PLAN_CD";
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "EPAL_PRODUCT_CD";
                return param;
            },
            All_APP_FUNDING_ARRANGEMENT: function () {
                var param = _AddNewDetailController.dropdown.param.All();
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
    },
    methods: {
        redirect_Detail: function (page, dpoc_package) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 50000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    // fix for https://github.com/uhc-actuarial/hcemi-appdev-pims/security/code-scanning/9 MFQ 8/22/2024
                    if (page == 'Add') {
                        window.location.href = MIApp.Common.ApiEPRepository.get(page.toLowerCase() + 'detail', 'DpocUrls')
                            .replace('__pims_id__', MIApp.Sanitize.encode($("#txtPIMSID").val())).replace('__p__', dpoc_package);
                    }
                    else {
                        window.location.href = MIApp.Common.ApiEPRepository.get(page.toLowerCase() + 'detail', 'DpocUrls')
                            .replace('__pims_id__', MIApp.Sanitize.encode($("#txtPIMSID").val())) + ',' + dpoc_package;
                    }
                }
            })
        },
        pims_id_drugname_parse: function (pims_id) {
            if (pims_id.indexOf(" ") > 0) {
                return pims_id = pims_id.substring(0, pims_id.indexOf(' '));
            }

            else {
                return pims_id;
            }
        },
        CheckValidProcedureCode: function () {
            var isActive = "";
            var token = $('meta[name="request-verification-token"]')[0].content;
            $.ajax({
                type: 'POST',
                url: VIRTUAL_DIRECTORY + '/EPAL/Home/GetEPALProcStatus',
                data: { p_PROC_CD: $("#txtProcedureCode").data("kendoMultiColumnComboBox").value().toUpperCase() },
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
        checkPIMSHierarchyCodeCombinationExists: function () {
            var EPAL_BUS_SEG_CD = "";
            var EPAL_ENTITY_CD = "";
            var EPAL_PLAN_CD = "";
            var EPAL_PRODUCT_CD = "";
            var EPAL_FUND_ARNGMNT_CD = "";
            var isExists = true;

            EPAL_BUS_SEG_CD = $("#ddlBusinessSegment").val();
            EPAL_ENTITY_CD = $("#ddlEntity").val();
            EPAL_PLAN_CD = $("#ddlPlan").val();
            EPAL_PRODUCT_CD = $("#ddlProduct").val();
            EPAL_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();

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
                    _AddNewDetailController.message.showError();
                }
            });

            return isExists;
        },
        HandlePIMSValidation: function () {
            var isValidNewPIMS_ID = true;
            var DPOC_BUS_SEG_CD = $("#ddlBusinessSegment").val();
            var DPOC_ENTITY_CD = $("#ddlEntity").val();
            var DPOC_PLAN_CD = $("#ddlPlan").val();
            var DPOC_PRODUCT_CD = $("#ddlProduct").val();
            var DPOC_FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
            var PROC_CD = $("#txtProcedureCode").data("kendoMultiColumnComboBox").value();
            var DPOC_PACKAGE = $('#txtDPOCPackage_New').val();
            //var DPOC_RELEASE = $('#txtDPOCRelease_New').val();
            var isValid_PROC_CD = _AddNewDetailController.methods.CheckValidProcedureCode();

            if (isValid_PROC_CD == "Inactive") {
                _AddNewDetailController.message.showWarning("Invalid procedure code, please enter a valid procedure code!")
                isValidNewPIMS_ID = false;
            }

            // Check valid hierarchy code combination
            PIMSHierarchyCodesExist = _AddNewDetailController.methods.checkPIMSHierarchyCodeCombinationExists();
            if (PIMSHierarchyCodesExist == false) {
                _AddNewDetailController.message.showWarning("Invalid DPOC ID hierarchy code combination. Please select another value!")
                isValidNewPIMS_ID = false;
            }

            if (isValidNewPIMS_ID) {
                // Check null or empty string
                if (
                    DPOC_BUS_SEG_CD?.trim().length > 0 &&
                    DPOC_ENTITY_CD?.trim().length > 0 &&
                    DPOC_PLAN_CD?.trim().length > 0 &&
                    DPOC_PRODUCT_CD?.trim().length > 0 &&
                    DPOC_FUND_ARNGMNT_CD?.trim().length > 0 &&
                    PROC_CD?.trim().length > 0 &&
                    DPOC_PACKAGE?.trim().length > 0
                    //DPOC_RELEASE?.trim().length > 0
                ) {
                    $.ajax({
                        type: "GET",
                        url: VIRTUAL_DIRECTORY + '/DPOC/Home/GetDPOCIDExistStatus?dpoc_hierarchy_key=' + MIApp.Sanitize.string($("#txtPIMSID").val()) + '&dpoc_package=' + MIApp.Sanitize.string($('#txtDPOCPackage_New').val()),
                        async: false,
                        headers: {
                            'Accept': 'application/json'
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        success: function (result) {
                            if (result == 'Exists') {
                                $('#validate-add-new-record-modal').modal('show');
                            }
                            else _AddNewDetailController.methods.redirect_Detail("Add", MIApp.Sanitize.string($('#txtDPOCPackage_New').val()));
                        },
                        error: function () {
                            _AddNewDetailController.message.showError();
                        }
                    })
                }
                else {
                    isValidNewPIMS_ID = false;
                    _AddNewDetailController.message.showWarning("Please fill in required fields!")
                }
            }
            return isValidNewPIMS_ID;
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