
var _activeDirectorySearchController = {
    init: function () {
        _activeDirectorySearchController.bind();

    },
    bind: function () {


        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
        $("#btnAzureAD").click(function () {
            _activeDirectorySearchController.grid.refreshGrid();
        });
        $("#btnAddUserSearch_ADSearch").click(function () {
            _activeDirectorySearchController.grid.refreshGrid();
        });

        $("#btnAddUserReset_ADSearch").click(function () {
            $('#AD_searchForm').trigger("reset");
            $("#grid_ADSearch").data("kendoGrid").dataSource.data([]);
        });
    },

    grid: {
        refreshGrid: function () {
            $("#grid_ADSearch").data("kendoGrid").dataSource.read();
            $("#grid_ADSearch").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                MS_ID: $.trim($("#txtMS_ID_ADSearch").val()),
                Fname: $.trim($("#txtFname_ADSearch").val()),
                Lname: $.trim($("#txtLname_ADSearch").val())
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
                allowOutsideClick: false,
                willOpen: () => {
                    Swal.showLoading();
                    window.location.href = VIRTUAL_DIRECTORY + "";
                }
            })
        },


        addUserInfo: function (ele) {
            var data = $("#grid_ADSearch").data("kendoGrid").dataItem($(ele).closest("tr"));
            var obj = {
                "P_MS_ID": data.MS_ID,
                "P_SUP_MSID": data.Supervisor_MS_ID,
                "P_SUP_NAME": data.Supervisor_Name,
                "P_LNAME": data.Last_Name,
                "P_FNAME": data.First_Name,
                "P_MI": data.MI,
                "P_EMAIL": data.Email,
                "P_PHONE": data.Phone,
                "P_FAX": data.Fax,
                "P_DIV_CODE": data.Division_Code,
                "P_DIVISION_NAME": data.Division_Name,
                "P_DEPARTMENT_NAME": data.Department_Name,
                "P_ACTIVE": "1",
                "P_LST_UPDT_BY": "",
                "P_MANUALUPDT": "1",
                "P_AUTOSAVESET": null,
                "P_DISPLAYNAME": data.Display_Name,
                "P_APP_ROLEID": "2",
                "P_PIMS_USER": "1"
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/Admin/Home/AddUserInfo",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (data) {
                    if (data.StatusID == 0) {
                        MICore.Notification.warning('Warning', data.Message);
                    } else if (data.StatusID == 1) {
                        //close modal
                        $("#activeDirectorySearchModal").modal('hide');
                        $('#AD_searchForm').trigger("reset");
                        $("#grid_ADSearch").data("kendoGrid").dataSource.data([]);

                        //refresh datagrid
                        Swal.fire({
                            title: 'Success!',
                            text: 'PIMS User has been added!',
                            icon: 'success',
                            showCancelButton: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            AdminController.grid.refreshGrid();
                        })
                    } else {
                        MICore.Notification.error('Error', data.Message)
                    }
                },
                error: function () {
                    _activeDirectorySearchController.message.showError();
                }
            })


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
    _activeDirectorySearchController.init();
});