
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

        $(".resetAddNew").click(function () {
            $('#addNewForm').trigger("reset");
            _AddNewDetailController.dropdown.refreshDropDownList();
        });

        $('#btnValidate, #btnValidatePopup').click(function () {
            _AddNewDetailController.methods.HandlePIMSValidation();
        })
        $('.update-existing').click(function () {
            _AddNewDetailController.methods.redirect_Detail("Edit");
        });

        $('.duplicate').click(function () {
             _AddNewDetailController.methods.redirect_Detail("DuplicateRecord");
        });

        $('.view-existing').click(function () {
            _AddNewDetailController.methods.redirect_Detail("View");
        });
    },
    dropdown: {
        refreshDropDownList: function () {
            var ddlLob = $("#ddlLOB_AddNew").data('kendoDropDownList');
            var ddlProduct = $("#ddlProduct_AddNew").data('kendoDropDownList');
            var ddlPlan = $("#ddlPlan_AddNew").data('kendoDropDownList');
            var ddlEntity = $("#ddlEntity_AddNew").data('kendoDropDownList');
            ddlLob.dataSource.read();
            ddlProduct.dataSource.read();
            ddlPlan.dataSource.read();
            ddlEntity.dataSource.read();
        },

        onChange_LOB: function () {
            _AddNewDetailController.methods.create_pims_id()      
            _AddNewDetailController.dropdown.refreshDropDownList();

        },

        onChange_Entity: function () {
            _AddNewDetailController.methods.create_pims_id()      
            _AddNewDetailController.dropdown.refreshDropDownList();            
        },

        onChange_Plan: function () {
            _AddNewDetailController.methods.create_pims_id()      
            _AddNewDetailController.dropdown.refreshDropDownList();
        },

        onChange_Product: function () {
            _AddNewDetailController.methods.create_pims_id()            
            _AddNewDetailController.dropdown.refreshDropDownList();
        },
        
        onChange_txtProcedureCode_AddNew: function () {
            _AddNewDetailController.methods.create_pims_id()      
        },

        param: {
            All_APP_LOB: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "BUS_SEG_CD";
                return param;
            },
            All_APP_ENTITY: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_ENTITY_CD";
                return param;
            },
            All_APP_PLAN: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_PLAN_CD";
                return param;
            },
            All_APP_PRODUCT: function () {
                var param = _AddNewDetailController.dropdown.param.All();
                param.COLUMN_NAME = "PAYC_PRODUCT_CD";
                return param;
            },
            All: function () {
                return {
                    PAYC_BUS_SEG_CD: $("#ddlLOB_AddNew").val(),
                    PAYC_ENTITY_CD: $("#ddlEntity_AddNew").val(),
                    PAYC_PLAN_CD: $("#ddlPlan_AddNew").val(),
                    PAYC_PRODUCT_CD: $("#ddlProduct_AddNew").val()
                }
            }
        },
    },
    methods: {
        redirect_Detail: function (page) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to detail page, please wait..',
                timerProgressBar: true,
                timer: 50000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    // fix for https://github.com/uhc-actuarial/hcemi-appdev-pims/security/code-scanning/16 MFQ 8/22/2024
                    /*  window.location.href = MIApp.Sanitize.encode(VIRTUAL_DIRECTORY + "/PayCodes/Home/" +page+"Detail/" + $("#txtPIMSID").val());*/
                    window.location.href = encodeURI(VIRTUAL_DIRECTORY + "/PayCodes/Home/" + page + "Detail/" + $("#txtPIMSID").val());
                }
            })
        },
        create_pims_id: function () {
            var pims_id = '-'
            var PAYC_BUS_SEG_CD = $("#ddlLOB_AddNew").val();
            var PAYC_ENTITY_CD = $("#ddlEntity_AddNew").val();
            var PAYC_PLAN_CD = $("#ddlPlan_AddNew").val();
            var PAYC_PRODUCT_CD = $("#ddlProduct_AddNew").val();
            var PAYC_FUNDING = "FI-";
            var PROC_CD = $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value();//$("#txtProcedureCode_AddNew").val();
            
            if (PAYC_BUS_SEG_CD != '') {
                PAYC_BUS_SEG_CD += "-"
            }
            if (PAYC_ENTITY_CD != '') {
                PAYC_ENTITY_CD += "-"
            }
            if (PAYC_PLAN_CD != '') {
                PAYC_PLAN_CD += "-"
            }
            if (PAYC_PRODUCT_CD != '') {
                PAYC_PRODUCT_CD += "-"
            }
            if (PROC_CD != '') {
                PROC_CD += "-"
            }

            pims_id = PAYC_BUS_SEG_CD + PAYC_ENTITY_CD + PAYC_PLAN_CD + PAYC_PRODUCT_CD + PAYC_FUNDING +  PROC_CD.toUpperCase();
            $("#txtPIMSID").val(pims_id.substr(0, pims_id.length-1));
            return pims_id;
        },
        HandlePIMSValidation: function () {
            var isValidNewPIMS_ID = true;
            var PAYC_BUS_SEG_CD = $("#ddlLOB_AddNew").val();
            var PAYC_ENTITY_CD = $("#ddlEntity_AddNew").val();
            var PAYC_PLAN_CD = $("#ddlPlan_AddNew").val();
            var PAYC_PRODUCT_CD = $("#ddlProduct_AddNew").val();
            var PROC_CD = $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value();

            var isValid_PROC_CD = _AddNewDetailController.methods.CheckValidProcedureCode();

            if (isValid_PROC_CD == "Inactive") {
                _AddNewDetailController.message.showWarning("Invalid procedure code, please enter a valid procedure code!")
                isValidNewPIMS_ID = false;
            }
            else if (isValid_PROC_CD == "Active") {
                // Check null or empty string
                if (
                    PAYC_BUS_SEG_CD?.trim().length > 0 &&
                    PAYC_ENTITY_CD?.trim().length > 0 &&
                    PAYC_PLAN_CD?.trim().length > 0 &&
                    PAYC_PRODUCT_CD?.trim().length > 0 &&
                    PROC_CD?.trim().length > 0
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
                            if (result != null && result.PAYC_HIERARCHY_KEY != null) {
                                $('#validate-add-new-record-modal').modal('show');
                            }
                            else _AddNewDetailController.methods.redirect_Detail("Add");
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
        },
        CheckValidProcedureCode: function () {
            var isActive = "";
            var token = $('meta[name="request-verification-token"]')[0].content;
            $.ajax({
                type: 'POST',
                url: VIRTUAL_DIRECTORY + '/EPAL/Home/GetEPALProcStatus',
                data: { p_PROC_CD: $("#txtProcedureCode_AddNew").data("kendoMultiColumnComboBox").value().toUpperCase() },
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