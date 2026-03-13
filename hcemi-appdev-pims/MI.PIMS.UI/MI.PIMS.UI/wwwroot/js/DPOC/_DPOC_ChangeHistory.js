
var ChangeHistoryController = {
    init: function () {
        ChangeHistoryController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    },
    grid: {
        refreshGrid: function () {
            $("#grid_ChangeHistory").data("kendoGrid").dataSource.read();
            $("#grid_ChangeHistory").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {                
                p_DPOC_HIERARCHY_KEY: duplicateDetail ? null : $("#txtPIMSID").val(),
                p_DPOC_PACKAGE: duplicateDetail ? null : $("#ddlDPOCPackage").val(),
            }
            return paramObj
        },
        gridError: function (e) {
            if (e.errors) {
                var message = "Errors:\n";
                $.each(e.errors, function (key, value) {
                    if ('errors' in value) {
                        $.each(value.errors, function () {
                            message += this + "\n";
                        });
                    }
                });
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: message,
                    customClass: 'swal-size-sm'
                });
            }
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("400px");
            gridContent.height("310px");
        }
    },
    validation: {
        controlValidator: function (controlName) {
            var validator = MICore.Validation(controlName);
            return validator.validate();
        },
    },
    methods: {
        redirect: function (CaseId) {
            Swal.fire({
                title: 'Redirecting..',
                text: 'Redirecting to case, please wait..',
                timerProgressBar: true,
                //timer: 5000,
                timer: 60000,
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "";
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
        showWarning: function (message) {
            Swal.fire({
                icon: 'warning',
                title: 'Invalid Date',
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
};


$(document).ready(function () {
    ChangeHistoryController.init();
});