
var wnd;
var diagList_Isvalid;
var diagList_prgmgby_counter = 0;
var dialList_row_count = 0;
var diagList_prgmgby_Isvalid;

var DiagListController = {
    data: {
        init: function () {
            var _data = this;
            var validationText = '';

            _data.DIAG_LIST = [];
            _data.INCL_EXCL_CD = [];
            _data.PROG_MGD_BY = [];
            var diagListData = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            jQuery.each(diagListData, function (index, diag) {
                _data.DIAG_LIST.push(diag.LIST_NAME);
                _data.PROG_MGD_BY.push(diag.PROG_MGD_BY);
                if ($.trim(diag.INCL_EXCL_CD) == '') validationText = 'Please select Include/Exclude for the selected Diagnosis Code(s) section!\n';
                _data.INCL_EXCL_CD.push(diag.INCL_EXCL_CD);
            });

            _data.DIAG_LIST = _data.DIAG_LIST.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.join(',');
            _data.PROG_MGD_BY = _data.PROG_MGD_BY.join(',');
            return validationText;
        },
        DIAG_LIST: [],
        INCL_EXCL_CD: [],
        PROG_MGD_BY: []
    },
    init: function () {
        DiagListController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $("#grid_DiagList").on("click", ".diagnosis-list-delete", function () {
            DiagListController.grid.deleteRow(this);
        });

        //$("#btnTest").click(function () {
        //    DiagListController.grid.check_diagList_prgmgby();
        //});
    },
    grid: {
        refreshGrid: function () {
            $("#grid_DiagList").data("kendoGrid").dataSource.read();
            $("#grid_DiagList").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                /* PIMS_ID: "EnI-ALS-COM-ALL-ALL-J1572-NA"*/
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val()
            }
            return MIApp.Sanitize.encodeObject(paramObj);
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
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);

            ProgMgdByController.grid.verifyPMBbyIndicator();
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete", null, function () {
            //    $("#grid_DiagCodes").data("kendoGrid").removeRow(row);
            //    $("#grid_DiagCodes").data("kendoGrid").refresh();
            //});

            var progMgdBy = row.closest('tr').find('td:eq(3)').text()

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                //cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {

                    $("#grid_DiagList").data("kendoGrid").removeRow(row);
                    ProgMgdByController.grid.updateRow(progMgdBy, "diags", "No")
                    DetailController.dirtyform.dirtycheck();

                    if (diagList_prgmgby_counter > 0) {
                        diagList_prgmgby_counter = (diagList_prgmgby_counter + 1);
                    }
                    else if (diagList_prgmgby_counter == 0) {
                        diagList_prgmgby_counter = 0;
                    }

                    $("#grid_DiagList").data("kendoGrid").refresh();

                    ProgMgdByController.grid.verifyPMBbyIndicator();

                    //Bug 125191 MFQ 2/20/2025 If diag list has records then Disable ADD NEW RECORD of Diag Code otherwise Enable it
                    var diagListData = $("#grid_DiagList").data("kendoGrid").dataSource._data;
                    if (diagListData.length == 0)
                        $('#grid_DiagCodes .k-grid-add').attr('disabled', false);
                    else
                        $('#grid_DiagCodes .k-grid-add').attr('disabled', true);
                }
            });

        },
        onViewOnlyDataBound: function () {
            var diagData = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            if (diagData.length > 0) {
                if ($.trim($('#txtDxCodeRequirements').val()) == 'No' || $.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('Yes');
            } else {
                if ($.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('No');
            }
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("400px");
            gridContent.height("275px");

            var dataArray = []
            var diagListData = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            jQuery.each(diagListData, function (index, e) {
                if (jQuery.inArray(e.LIST_NAME + '-' + e.INCL_EXCL_CD_DESC, dataArray) === -1) {
                    dataArray.push(e.LIST_NAME + '-' + e.INCL_EXCL_CD_DESC)
                    diagList_Isvalid = true
                } else {
                    DiagListController.message.showWarning('Diagnosis list name already exists with selected include/exclude values. Please update include/exclude values!')
                    diagList_Isvalid = false
                }

                if (dataArray.length > 1) {
                    if (e.LIST_NAME.length > 0) {
                        if (e.PROG_MGD_BY === null || e.PROG_MGD_BY === '') {
                            if (diagList_prgmgby_counter <= 0) {
                                //DiagListController.message.showWarning('Diagnosis list name requires a program managed by!')
                                diagList_prgmgby_Isvalid = false
                            }
                        }
                    }
                }

                if ((e.PROG_MGD_BY != '' && e.PROG_MGD_BY != null && e.PROG_MGD_BY != 'null') && e.LIST_NAME != null) {
                    ProgMgdByController.grid.addRow(e.PROG_MGD_BY);
                }

                ProgMgdByController.grid.updateRow(e.PROG_MGD_BY, "diags", "Yes");

            });

            if (diagListData.length > 0) {
                if ($.trim($('#txtDxCodeRequirements').val()) == 'No' || $.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('Yes');                
            } else {
                if ($.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('No');
            }

            $("#grid_DiagList .k-grid-add").unbind("click").bind("click", function () {
                //Bug 125191 MFQ 2/20/2025                
                $('#grid_DiagCodes .k-grid-add').attr('disabled', true);
            });

            //Remove Clearing of PMB from State grid based on expired PMB since now system is based on current status of the current rather than current date
            //DiagListController.grid.clearExpiredPMB(ProgMgdByController.grid.getExpiredPMB());

        },
        updateRow: function (progMgdBy) {

            var diagListArray = []
            var diagListObj = []
            var data = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                diagListObj = {
                    "LIST_NAME": e.LIST_NAME,
                    "INCL_EXCL_CD": e.INCL_EXCL_CD,
                    "INCL_EXCL_CD_DESC": e.INCL_EXCL_CD_DESC,
                    "PROG_MGD_BY": e.PROG_MGD_BY
                }

                diagListArray.push(diagListObj);

            });

            //Find index of specific object using findIndex method.
            objIndex = diagListArray.findIndex((obj => obj.PROG_MGD_BY == progMgdBy));

            if (objIndex > -1) {
                //Update object's name property.
                diagListArray[objIndex].PROG_MGD_BY = "";


                //Empty grid
                $("#grid_DiagList").data("kendoGrid").dataSource.data([]);

                var grid = $("#grid_DiagList").data("kendoGrid");
                var datasource = grid.dataSource;

                //Append new grid data. 
                var i;
                for (i = 0; i < diagListArray.length; ++i) {
                    datasource.insert(diagListArray[i]);
                }
            }
        },
        row_count: function () {
            //var grid = $("#grid_DiagList").data("kendoGrid");
            //var dataSource = grid.dataSource;
            //var totalRecords = dataSource.total();

            var totalRecords = 0;
            var data = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                totalRecords += 1
            });

            return totalRecords;
        },
        check_diagList_prgmgby: function () {
            var row_count = 0;
            var prgmgby_count = 0;
            var data = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                row_count += 1

                var val = e.PROG_MGD_BY == null ? '' : e.PROG_MGD_BY;
                if (val.length > 0) {
                    prgmgby_count++;
                }
            });


            // When there's no record, then return validation true
            if (row_count == 0) {
                return true;
            }

            // When there's ONE record, then return validation true
            else if (row_count == 1) {
                return true;
            }

            // When there's more than one record and progMgdBy has at least one value then return validation true
            else if (row_count >= 1 && prgmgby_count >= 1) {
                return true;
            }

            return false;
        },
        clearExpiredPMB: function (arrPMB) {
            // if PMB is not expired then return
            if (arrPMB.length == 0) return;

            var data = $("#grid_DiagList").data("kendoGrid").dataSource.data();
            jQuery.each(data, function (index, diagListItem) {
                if (jQuery.inArray(diagListItem.PROG_MGD_BY, arrPMB) > -1) {
                    //diagListItem.set('PROG_MGD_BY', '');
                    diagListItem.PROG_MGD_BY = '';
                    $("#grid_DiagList").data("kendoGrid").refresh();
                }
            });
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
};

var DiagCodesController = {
    data: {
        init: function () {
            var _data = this;
            var validationText = '';

            _data.DIAG_CD = [];
            _data.INCL_EXCL_CD = [];
            _data.PROG_MGD_BY = [];

            var diagCodesData = $("#grid_DiagCodes").data("kendoGrid").dataSource._data;
            jQuery.each(diagCodesData, function (index, diag) {
                _data.DIAG_CD.push(diag.DIAG_CD);
                _data.PROG_MGD_BY.push(diag.PROG_MGD_BY);
                if ($.trim(diag.INCL_EXCL_CD) == '') validationText = 'Please select Include/Exclude for the selected Diagnosis Code(s) section!\n';
                _data.INCL_EXCL_CD.push(diag.INCL_EXCL_CD);
            });

            _data.DIAG_CD = _data.DIAG_CD.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.join(',');
            _data.PROG_MGD_BY = _data.PROG_MGD_BY.join(',');
            return validationText;
        },
        DIAG_CD: [],
        INCL_EXCL_CD: [],
        PROG_MGD_BY: []
    },
    init: function () {
        DiagCodesController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $("#grid_DiagCodes").on("click", ".diagnosis-codes-delete", function () {
            DiagCodesController.grid.deleteRow(this);
        });

        $("#grid_RevCodes").on("click", ".revenue-codes-delete", function () {
            RevCodesController.grid.deleteRow(this);
        });

        $("#grid_AllocatedPlaces").on("click", ".allocated-places-codes-delete", function () {
            AllocatedPlacesController.grid.deleteRow(this);
        });

        $("#grid_Modifiers").on("click", ".modifiers-codes-delete", function () {
            ModifiersController.grid.deleteRow(this);
        });



    },
    grid: {
        refreshGrid: function () {
            $("#grid_DiagCodes").data("kendoGrid").dataSource.read();
            $("#grid_DiagCodes").data("kendoGrid").refresh();
        },
        isNotEditable: function (e) {
            if (e.LIST_NAME != '')
                return false;
            else {
                return true;
            }
        },
        param: function () {
            var paramObj = {
                /* PIMS_ID: "EnI-ALS-COM-ALL-ALL-J1572-NA"*/
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val()
            }
            return MIApp.Sanitize.encodeObject(paramObj);
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
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete", null, function () {
            //    $("#grid_DiagCodes").data("kendoGrid").removeRow(row);
            //    $("#grid_DiagCodes").data("kendoGrid").refresh();
            //});
            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                //cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_DiagCodes").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                    $("#grid_DiagCodes").data("kendoGrid").refresh();

                    //Bug 125191 MFQ 2/20/2025 If diag list has records then Disable ADD NEW RECORD of Diag Code otherwise Enable it
                    var diagCodesData = $("#grid_DiagCodes").data("kendoGrid").dataSource._data;
                    if (diagCodesData.length == 0)
                        $('#grid_DiagList .k-grid-add').attr('disabled', false);
                    else
                        $('#grid_DiagList .k-grid-add').attr('disabled', true);
                }
            });

        },
        onViewOnlyDataBound: function () {
            var diagData = $("#grid_DiagCodes").data("kendoGrid").dataSource._data;
            if (diagData.length > 0) {
                if ($.trim($('#txtDxCodeRequirements').val()) == 'No' || $.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('Yes');
            } else {
                if ($.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('No');
            }
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("400px");
            gridContent.height("275px");

            var diagCodesData = $("#grid_DiagCodes").data("kendoGrid").dataSource._data;
            if (diagCodesData.length > 0) {
                if ($.trim($('#txtDxCodeRequirements').val()) == 'No' || $.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('Yes');
            } else {
                if ($.trim($('#txtDxCodeRequirements').val()) == '')
                    $('#txtDxCodeRequirements').val('No');
            }

            $("#grid_DiagCodes .k-grid-add").unbind("click").bind("click", function () {
                //Bug 125191 MFQ 2/20/2025
                $('#grid_DiagList .k-grid-add').attr('disabled', true);
            });
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

        redirect_diagcodes: function () {
            alert('testing...');
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

var RevCodesController = {
    data: {
        init: function () {
            var _data = this;
            var validationText = '';

            _data.REV_CD = [];
            _data.INCL_EXCL_CD = [];

            var revData = $("#grid_RevCodes").data("kendoGrid").dataSource._data;
            jQuery.each(revData, function (index, rev) {
                _data.REV_CD.push(rev.REV_CD);

                if ($.trim(rev.INCL_EXCL_CD) == '') validationText = 'Please select Include/Exclude for the selected Revenue Code(s) section!\n';
                _data.INCL_EXCL_CD.push(rev.INCL_EXCL_CD);
            });

            _data.REV_CD = _data.REV_CD.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.join(',');

            return validationText;
        },
        REV_CD: [],
        INCL_EXCL_CD: []
    },
    init: function () {
        RevCodesController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        //var gridElement = $("#grid_RevCodes");
        //var dataArea = gridElement.find(".k-grid-content");
        //gridElement.height("305px");
        //dataArea.height("225px");
    },
    grid: {
        refreshGrid: function () {
            $("#grid_RevCodes").data("kendoGrid").dataSource.read();
            $("#grid_RevCodes").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                //PIMS_ID: "EnI-ALS-COM-ALL-ALL-J1572-NA"
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val()
            }
            return MIApp.Sanitize.encodeObject(paramObj);
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
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete", null, function () {
            //    $('#grid_RevCodes').data("kendoGrid").removeRow(row);
            //});

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                //cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_RevCodes").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                }
            });

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

var AllocatedPlacesController = {
    data: {
        init: function () {
            var _data = this;
            var validationText = '';

            _data.ALLWD_PLC_OF_SVC = [];
            _data.INCL_EXCL_CD = [];

            var allwdPlaceData = $("#grid_AllocatedPlaces").data("kendoGrid").dataSource._data;
            jQuery.each(allwdPlaceData, function (index, allwdPlace) {
                _data.ALLWD_PLC_OF_SVC.push(allwdPlace.CODE);

                if ($.trim(allwdPlace.INCL_EXCL_CD) == '') validationText = 'Please select Include/Exclude for the selected POS Code(s) section!\n';
                _data.INCL_EXCL_CD.push(allwdPlace.INCL_EXCL_CD);
            });

            _data.ALLWD_PLC_OF_SVC = _data.ALLWD_PLC_OF_SVC.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.join(',');

            return validationText;
        },
        ALLWD_PLC_OF_SVC: [],
        INCL_EXCL_CD: []
    },
    init: function () {
        AllocatedPlacesController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        //var gridElement = $("#grid_AllocatedPlaces");
        //var dataArea = gridElement.find(".k-grid-content");
        //gridElement.height("305px");
        //dataArea.height("225px");
    },
    grid: {
        refreshGrid: function () {
            $("#grid_AllocatedPlaces").data("kendoGrid").dataSource.read();
            $("#grid_AllocatedPlaces").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                //PIMS_ID: "EnI-ALS-COM-ALL-ALL-J1572-NA"
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val()
            }
            return MIApp.Sanitize.encodeObject(paramObj);
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
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete", null, function () {
            //    $("#grid_AllocatedPlaces").data("kendoGrid").removeRow(row);
            //});

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                //cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_AllocatedPlaces").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                }
            });

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

var ModifiersController = {
    data: {
        init: function () {
            var _data = this;
            var validationText = '';
            _data.MODIFIER = [];
            _data.INCL_EXCL_CD = [];

            var modifiersData = $("#grid_Modifiers").data("kendoGrid").dataSource._data;
            jQuery.each(modifiersData, function (index, modifier) {
                _data.MODIFIER.push(modifier.MODIFIER);

                if ($.trim(modifier.INCL_EXCL_CD) == '') validationText = 'Please select Include/Exclude for the selected Modifier Code(s) section!\n';
                _data.INCL_EXCL_CD.push(modifier.INCL_EXCL_CD);
            });

            _data.MODIFIER = _data.MODIFIER.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.join(',');

            return validationText;
        },
        MODIFIER: [],
        INCL_EXCL_CD: []
    },
    init: function () {
        ModifiersController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });
    },
    grid: {
        refreshGrid: function () {
            $("#grid_Modifiers").data("kendoGrid").dataSource.read();
            $("#grid_Modifiers").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                //PIMS_ID: "EnI-ALS-COM-ALL-ALL-J1572-NA"
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val()                
            }
            paramObj = MIApp.Sanitize.encodeObject(paramObj);
            paramObj.__RequestVerificationToken = $('meta[name="request-verification-token"]')[0].content
            return paramObj;
            //__RequestVerificationToken: $('input[name="request-verification-token"]').val()
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
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete", null, function () {
            //    $("#grid_Modifiers").data("kendoGrid").removeRow(row);
            //});

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                //cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_Modifiers").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                }
            });
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
    DiagListController.init();
    DiagCodesController.init();
    RevCodesController.init();
    AllocatedPlacesController.init();
    ModifiersController.init();

    wnd = $("#modalWindow").kendoWindow({
        title: "Delete confirmation",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");
});

function autoFitGridColumn(e) {
    for (var i = 0; i < e.columns.length; i++) {
        e.autoFitColumn(i);
    }
}

