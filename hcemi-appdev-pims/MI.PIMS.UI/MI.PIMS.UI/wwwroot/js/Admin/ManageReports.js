
var ManageReportsController = {
    init: function () {
        ManageReportsController.bind();
    },
    bind: function () {
        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });


        $("#btnSaveEdit").click(function () {
            ManageReportsController.methods.update();
        });

    },
    grid: {
        refreshGrid: function () {
            $("#grid_ReportLinks").data("kendoGrid").dataSource.read();
            $("#grid_ReportLinks").data("kendoGrid").refresh();
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

    modal: {

    },
    methods: {
        edit: function (ele) {
            var data = $("#grid_ReportLinks").data("kendoGrid").dataItem($(ele).closest("tr"));
            $("#txtID").val(data.PAGE_ID);
            $("#txtModuleName").val(data.MODULE_NAME);
            $("#txtReportURL").val(data.URLPATH);
            $('#editModal').modal('show');

        },
        update: function () {
            var obj = {
                "URLPATH": $("#txtReportURL").val(),
                "PAGE_ID": $("#txtID").val()
            }

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: $('#UpdateReportsUrl').val(), 
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
                    } else if (data.StatusID == -1) {

                    } else {
                        Swal.fire({
                            title: 'Success!',
                            text: 'PIMS Reports URL has been updated!',
                            icon: 'success',
                            showCancelButton: false,
                            confirmButtonText: 'OK',
                            customClass: 'swal-size-sm'
                        }).then((result) => {
                            ManageReportsController.grid.refreshGrid();
                            $('#editModal').modal('hide');
                        })
                    }
                },
                error: function (e) {
                    ManageReportsController.message.showError();
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
    ManageReportsController.init();
});