
var _editUserController = {
    init: function () {
        _editUserController.bind();

    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        $("#btnSaveEdit").click(function () {
            _editUserController.methods.updateAppRole();
        });
    },
    grid: {
        refreshGrid: function () {
            $("#grid_RoleUserAssign").data("kendoGrid").dataSource.read();
            $("#grid_RoleUserAssign").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                MS_ID: $.trim($("#txtMSID_Edit").val()),
            }
            return paramObj
        },
    },
    methods: {
        getSelected: function () {
            var ischecked = $("#grid_RoleUserAssign tr").find(":checked");
            var strSelectedId = "";
            $.each(ischecked, function () {
                if ($(this).attr("value") != 0) {
                    strSelectedId += $(this).attr("value") + ",";
                }
            })

            return strSelectedId;
        },

        deleteUser: function (msid) {
            var obj = {
                "p_MS_ID": msid,
            }
            var token = $('meta[name="request-verification-token"]')[0].content;
            $.ajax({
                type: "POST",
                url: VIRTUAL_DIRECTORY + "/Admin/Home/DeleteUser",
                data: obj,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                async: false,
                xhrFields: {
                    withCredentials: true
                },
                success: function () {
                    //refresh datagrid
                    Swal.fire({
                        title: 'Success!',
                        text: 'PIMS user has been deleted!',
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        AdminController.grid.refreshGrid();
                    })
                },
                error: function () {
                    _editUserController.message.showError();
                }
            })
        },

        updateAppRole: function () {
            var obj = {
                "p_MS_ID": $("#txtMSID_Edit").val(),
                "p_APP_ROLEIDS": _editUserController.methods.getSelected()
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: $('#UpdateRoleUrl').val(), //VIRTUAL_DIRECTORY + "/Admin/Home/UpdateRole",
                data: obj,
                async: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (e) {
                    //close modal
                    $("#editUserModal").modal('hide');

                    //refresh datagrid
                    Swal.fire({
                        title: 'Success!',
                        text: 'PIMS User has been updated!',
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonText: 'OK',
                        customClass: 'swal-size-sm'
                    }).then((result) => {
                        if (obj.p_MS_ID == MS_ID)
                            window.location.href = window.location.href;
                        else
                            AdminController.grid.refreshGrid();
                    })
                },
                error: function () {
                    _editUserController.message.showError();
                }
            })
        },
        toggleActiveStatus: function () {

            var userActiveStatus = $('.pims-user').length ? "In-Active" : "Active";

            MICore.Notification.question('Toggle PIMS User Status?', 'Are you sure, you want to mark this user PIMS status as ' + userActiveStatus, null, null, function () {
                var obj = {
                    "p_MS_ID": $("#txtMSID_Edit").val(),
                    "p_PIMS_USER": $('.pims-user').length ? "0" : "1"
                }

                var token = $('meta[name="request-verification-token"]')[0].content;

                $.ajax({
                    type: "POST",
                    url: $('#TogglePIMSUserStatusUrl').val(), //VIRTUAL_DIRECTORY + "/Admin/Home/ToggleActiveStatus",
                    data: obj,
                    async: false,
                    headers: {
                        'Accept': 'application/json',
                        'RequestVerificationToken': token
                    },
                    xhrFields: {
                        withCredentials: true
                    },
                    success: function (e) {
                        //close modal
                        $("#editUserModal").modal('hide');
                        if (e.StatusType == 'Success')                        
                            MICore.Notification.success("Success", 'PIMS User Status has been updated!', AdminController.grid.refreshGrid);
                        else
                            MICore.Notification.error("Error", e.Message, AdminController.grid.refreshGrid);
                        //Swal.fire({
                        //    title: 'Success!',
                        //    text: 'PIMS User Status has been updated!',
                        //    icon: 'success',
                        //    showCancelButton: false,
                        //    confirmButtonText: 'OK',
                        //    customClass: 'swal-size-sm'
                        //}).then((result) => {
                        //    AdminController.grid.refreshGrid();
                        //});
                    },
                    error: function () {
                        _editUserController.message.showError();
                    }
                })
            }, null);            
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
        },

        showDeleteUser: function (ele) {
            var data = $("#grid_UserInfo").data("kendoGrid").dataItem($(ele).closest("tr"))
            var lname = data.LNAME
            var fname = data.FNAME
            var msid = data.MS_ID

            Swal.fire({
                icon: 'question',
                title: 'Delete PIMS User?',
                html: 'Are you sure you want to delete PIMS user: <br /> <strong>' + lname + ', ' + fname + '  (' + msid + ')</strong> from the application?',
                //html: 'Are you sure you want to delete PIMS user from the application?',
                customClass: 'swal-size-sm',
                showCancelButton: true,
                showConfirmButton: true,
                allowOutsideClick: false,
                confirmButtonText: 'Ok',
            }).then((result) => {
                if (result.isConfirmed) {
                    _editUserController.methods.deleteUser(msid)
                }
            });

        }
    }
};

$(document).ready(function () {
    _editUserController.init();
});