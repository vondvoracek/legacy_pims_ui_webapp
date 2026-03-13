
var AdminController = {
    init: function () {
        AdminController.bind();
    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $("#btnSearch").click(function () {
            AdminController.grid.refreshGrid();
        });

        $("#btnReset").click(function () {
            $('#searchForm').trigger("reset");
            $("#grid_UserInfo").data("kendoGrid").dataSource.data([]);
        });
    },
    grid: {
        refreshGrid: function () {
            $("#grid_UserInfo").data("kendoGrid").dataSource.read();
            $("#grid_UserInfo").data("kendoGrid").refresh();
        },
        param: function () {
        /*    var dropdownlist1 = $("#ddlAppRole").data("kendoDropDownList");*/
            var dropdownlist2 = $("#ddlActive").data("kendoDropDownList");
            var dropdownlist3 = $("#ddlPIMSUser").data("kendoDropDownList");

            var paramObj = {
                MS_ID: $.trim($("#txtMS_ID").val()),
                Fname: $.trim($("#txtFname").val()),
                Lname: $.trim($("#txtLname").val()),
   /*             App_Role_ID: dropdownlist1.value(),*/
                Active: dropdownlist2.value(),
                PIMS_user: dropdownlist3.value(),
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
        editAccess: function (ele) {
          
            var data = $("#grid_UserInfo").data("kendoGrid").dataItem($(ele).closest("tr"));
            $("#txtMSID_Edit").val(data.MS_ID);
            $("#txtFirstName_Edit").val(data.FNAME);
            $("#txtLastName_Edit").val(data.LNAME);
            $("#txtDepartment_Edit").val(data.DEPARTMENT_NAME);
            $("#txtManager_Edit").val(data.SUP_NAME);

            _editUserController.grid.refreshGrid();
           // var $ddlAppRoleEdit = $("#ddlAppRoleEdit").data("kendoDropDownList");
/*            $ddlAppRoleEdit.value(data.APP_ROLEID);*/
            if (data.PIMS_USER == 1) {
                $('.div-pims-user').html('<i style="color:teal;vertical-align:middle;font-size:20px;cursor:pointer;" class="fas fa-check-circle 3x pims-user"></i>');
             //   $ddlAppRoleEdit.enable(true);
                $('#btnSaveEdit').prop('disabled', false);
            } else {
               // $ddlAppRoleEdit.enable(false);
                $('#btnSaveEdit').prop('disabled', true);
                $('.div-pims-user').html('<i style="color:red;vertical-align:middle;font-size:20px;cursor:pointer;" class="fas fa-times-circle 3x in-pims-user"></i>');
            }       

            $('#editUserModal').modal('show');
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
    AdminController.init();
});