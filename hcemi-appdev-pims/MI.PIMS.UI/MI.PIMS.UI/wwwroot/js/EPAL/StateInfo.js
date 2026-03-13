
var StateInfoController = {    
    init: function () {
        StateInfoController.bind();
    },
    bind: function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        $("#grid_StateInfo").on("click", ".state-info-delete", function () {
            StateInfoController.grid.deleteRow(this);
        });        

        // IF ADD NEW MODE 
        if (window.location.href.toLowerCase().indexOf('adddetail') > -1) {
            var grid = $("#grid_StateInfo").data("kendoGrid");
            grid.addRow();
        }
    },
    data: {
        init: function () {
            var _data = this;
            var stateInfoInclValidationText = '';

            _data.STATE_CD = [];
            _data.STATE_MANDATED_IND = [];
            _data.INCL_EXCL_CD = [];
            _data.ATS_EFF_DT = [];
            _data.ATS_EXP_DT = [];
            _data.ATS_ISSUE_GOV = [];
            _data.ATS_PROG_MGD_BY = [];

            var stateInfoData = $("#grid_StateInfo").data("kendoGrid").dataSource._data;
            jQuery.each(stateInfoData, function (index, stateInfo) {
                _data.STATE_CD.push(stateInfo.STATE_CD);
                _data.STATE_MANDATED_IND.push(stateInfo.STATE_MANDATED_IND);

                if ($.trim(stateInfo.INCL_EXCL_CD) == '') stateInfoInclValidationText += "Please select Include/Exclude for the selected state in State Info section!\n";
                _data.INCL_EXCL_CD.push(stateInfo.INCL_EXCL_CD);

                //Added 1/9/2023 MFQ US-44673
                if ($.trim(stateInfo.ATS_EFF_DT).length) {
                    _data.ATS_EFF_DT.push(kendo.toString(kendo.parseDate(stateInfo.ATS_EFF_DT), 'yyyy-MM-dd'));
                } else {
                    _data.ATS_EFF_DT.push(null);
                }

                if ($.trim(stateInfo.ATS_EXP_DT).length) {
                    _data.ATS_EXP_DT.push(kendo.toString(kendo.parseDate(stateInfo.ATS_EXP_DT), 'yyyy-MM-dd'));
                } else {
                    _data.ATS_EXP_DT.push(null);
                }

                //Removed as per BUG 85289
                //if ($.trim(stateInfo.ATS_EFF_DT) == "" || $.trim(stateInfo.ATS_EFF_DT) == null) {
                //    _data.ATS_EFF_DT.push(kendo.toString(kendo.parseDate('12/31/2999'), 'dd/MMM/yyyy'));
                //}

                //if ($.trim(stateInfo.ATS_EXP_DT) == "" || $.trim(stateInfo.ATS_EXP_DT) == null) {
                //    _data.ATS_EXP_DT.push(kendo.toString(kendo.parseDate('12/31/2999'), 'dd/MMM/yyyy'));
                //}

                _data.ATS_ISSUE_GOV.push(stateInfo.ATS_ISSUE_GOV);
                _data.ATS_PROG_MGD_BY.push(stateInfo.ATS_PROG_MGD_BY);
            });            

            _data.STATE_CD = _data.STATE_CD.join(',');
            _data.STATE_MANDATED_IND = _data.STATE_MANDATED_IND.join(',');
            _data.INCL_EXCL_CD = _data.INCL_EXCL_CD.filter(Boolean).join(',');
            _data.ATS_EFF_DT = _data.ATS_EFF_DT.join(',');
            _data.ATS_EXP_DT = _data.ATS_EXP_DT.join(',');
            _data.ATS_ISSUE_GOV = _data.ATS_ISSUE_GOV.join(',');
            _data.ATS_PROG_MGD_BY = _data.ATS_PROG_MGD_BY.join(',');

            return stateInfoInclValidationText;
        },
        STATE_CD: [],
        STATE_MANDATED_IND: [],
        INCL_EXCL_CD: [],
        ATS_EFF_DT: [],
        ATS_EXP_DT: [],
        ATS_ISSUE_GOV: [],
        ATS_PROG_MGD_BY: []
    },
    state_cd: {
        param: function () {
            return {
                COLUMN_NAME: "EPAL_ENTITY_CD",
                EPAL_BUS_SEG_CD: ''
            }
        }
    },
    grid: {
        clear: function () {
            $("#grid_StateInfo").data("kendoGrid").dataSource.data([]);
        },
        refreshGrid: function () {
            $("#grid_StateInfo").data("kendoGrid").dataSource.read();
            $("#grid_StateInfo").data("kendoGrid").refresh();
        },
        param: function () {            
            var ePALPageView = MIApp.Global.Page.ePALPageView();

            var epal_entity_cd = (ePALPageView == "adddetail" ? $('#txtEntity').val() : (ePALPageView == "duplicaterecorddetail" ? getDuplicateEpal_entity_cd() : $('#txtEntity').val()));

            var paramObj = {
                p_EPAL_HIERARCHY_KEY: ePALPageView == "duplicaterecorddetail" ? $('#btnGotoDetail').html().trim(): $("#txtPIMSID").val(),
                p_EPAL_VER_EFF_DT: ePALPageView == "duplicaterecorddetail" ? $('#txtOrigEPALVersionDt').val(): $("#txtEPALVersionDt").val(),
                p_EPAL_ENTITY_CD: epal_entity_cd,
                p_EPAL_BUS_SEG_CD: $('#txtOrigEPALBusSegCD').val() != undefined ? $('#txtOrigEPALBusSegCD').val() : null,
                EPALPageView: ePALPageView
            }
            return MIApp.Sanitize.encodeObject(paramObj);


            function getDuplicateEpal_entity_cd() {                
                
                var selectedEntityValue = $('#ddlEntity').data('kendoDropDownList').value();

                if (selectedEntityValue.length > 0) {
                    return selectedEntityValue;
                } else {
                    return $("#txtOrigEPALEntityCD").val();
                }
                
            }
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
            //MICore.Notification.question('Delete confirmation?', "Are you sure, you want to delete this entry!", "Delete",null, function () {
            //    $("#grid_StateInfo").data("kendoGrid").removeRow(row);
            //});

            var progMgdBy = row.closest('tr').find('td:eq(7)').text()

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
                    $("#grid_StateInfo").data("kendoGrid").removeRow(row);
                    ProgMgdByController.grid.updateRow(progMgdBy, "stateinfo", "No")
                    DetailController.dirtyform.dirtycheck();
                }
            });    
        },
        dataSource: {
            onChange: function (e) {
                if (e.action == "itemchange") {
                    DetailController.dirtyform.dirtycheck();
                }
            }
        },
        onDataBound: function (e) {
            // DevOps BUG 34168 - MFQ 9/19/2022
            var grid = e.sender;

            if ($('#epal-page-view').val().toLowerCase() == 'duplicaterecorddetail') {
                StateInfoController.methods.renderStateInfoOnDuplicateRecordDetail(grid);
            }

            if ($('#epal-page-view').val().toLowerCase() == 'adddetail' || $('#epal-page-view').val().toLowerCase() == 'editdetail') {
                StateInfoController.methods.renderStateInfoOnAddEditDetail(grid);
            }     

            //Remove Clearing of PMB from State grid based on expired PMB since now system is based on current status of the current rather than current date
            //StateInfoController.grid.clearExpiredPMB(ProgMgdByController.grid.getExpiredPMB());
        },
        onEdit: function (e) {             
            // USER STORY 34168 - MFQ
            if ($('#epal-page-view').val().toLowerCase() == 'duplicaterecorddetail') {
                if ($("#grid_StateInfo .k-grid-toolbar").is(":hidden")) {
                    if (e.container.find("[name]").first().attr("name") == "STATE_NAME_input") {
                        e.sender.closeCell();
                    }
                }                                                       
            }
            if ($('#epal-page-view').val().toLowerCase() == 'adddetail' || $('#epal-page-view').val().toLowerCase() == 'editdetail') {                                
                if ($("#grid_StateInfo .k-grid-toolbar").is(":hidden")) {
                    if (e.container.find("[name]").first().attr("name") == "STATE_NAME_input") {
                        e.sender.closeCell();
                    }
                }                 
            }
        },
        updateRow: function (progMgdBy) {
            var stateInfoArray = []
            var stateInfoObj = []
            var data = $("#grid_StateInfo").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                stateInfoObj = {
                    "STATE_NAME": e.STATE_NAME,
                    "STATE_CD": e.STATE_CD,
                    "STATE_MANDATED_IND": e.STATE_MANDATED_IND,
                    "INCL_EXCL_CD": e.INCL_EXCL_CD,
                    "INCL_EXCL_CD_DESC": e.INCL_EXCL_CD_DESC,
                    "ATS_EFF_DT": e.ATS_EFF_DT,
                    "ATS_EXP_DT": e.ATS_EXP_DT,
                    "ATS_ISSUE_GOV": e.ATS_ISSUE_GOV,
                    "ATS_ISSUE_GOV_DESC": e.ATS_ISSUE_GOV_DESC,
                    "ATS_PROG_MGD_BY": e.ATS_PROG_MGD_BY
                }

                stateInfoArray.push(stateInfoObj);

            });

            //Find index of specific object using findIndex method.
            objIndex = stateInfoArray.findIndex((obj => obj.ATS_PROG_MGD_BY == progMgdBy));

            if (objIndex > -1) {
                //Update object's name property.
                stateInfoArray[objIndex].ATS_PROG_MGD_BY = "";

                //Empty grid
                $("#grid_StateInfo").data("kendoGrid").dataSource.data([]);

                var grid = $("#grid_StateInfo").data("kendoGrid");
                var datasource = grid.dataSource;

                //Append new grid data. 
                var i;
                for (i = 0; i < stateInfoArray.length; ++i) {
                    datasource.insert(stateInfoArray[i]);
                }
            }
        },
        clearExpiredPMB: function (arrPMB) {

            // if PMB is not expired then return
            if (arrPMB.length == 0) return;

            var data = $("#grid_StateInfo").data("kendoGrid").dataSource.data();
            jQuery.each(data, function (index, stateInfo) {
                if (jQuery.inArray(stateInfo.ATS_PROG_MGD_BY, arrPMB) > -1) {
                    //stateInfo.set('ATS_PROG_MGD_BY','');
                    stateInfo.ATS_PROG_MGD_BY = '';
                    $("#grid_StateInfo").data("kendoGrid").refresh();
                }
            });            
        },
        updateStateDate: function (pmbExpDt, effDt) {
            var stateInfoArray = []
            var data = $("#grid_StateInfo").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                stateInfoObj = {
                    "STATE_CD": e.STATE_CD,
                    "STATE_NAME": e.STATE_NAME,
                    "STATE_MANDATED_IND": e.STATE_MANDATED_IND,
                    "INCL_EXCL_CD": e.INCL_EXCL_CD,
                    "INCL_EXCL_CD_DESC": e.INCL_EXCL_CD_DESC,
                    "ATS_EFF_DT": e.ATS_EFF_DT,
                    "ATS_EXP_DT": e.ATS_EFF_DT != '' && e.ATS_EFF_DT != null ? (kendo.toString(kendo.parseDate(e.ATS_EXP_DT), 'MM/dd/yyyy') == null || kendo.toString(kendo.parseDate(e.ATS_EXP_DT), 'MM/dd/yyyy') == '12/31/2999' ?
                        (pmbExpDt == null || pmbExpDt == '' ? kendo.toString(kendo.parseDate(e.ATS_EXP_DT), 'MM/dd/yyyy') : pmbExpDt) :
                        pmbExpDt) : null,
                    "ATS_ISSUE_GOV": e.ATS_ISSUE_GOV,
                    "ATS_ISSUE_GOV_DESC": e.ATS_ISSUE_GOV_DESC,
                    "ATS_PROG_MGD_BY": e.ATS_PROG_MGD_BY
                }

                stateInfoArray.push(stateInfoObj);
            });

            //Empty grid
            $("#grid_StateInfo").data("kendoGrid").dataSource.data([]);

            var grid = $("#grid_StateInfo").data("kendoGrid");
            var datasource = grid.dataSource;

            //Append new grid data. 
            var i;
            for (i = 0; i < stateInfoArray.length; ++i) {
                datasource.insert(stateInfoArray[i]);
            }
        }
    },
    validation: function (currentStatus, csDateRange) {
        var isValid = true;
        var errorMessage = "";
        var grid = $("#grid_StateInfo").data("kendoGrid");        

        if ($('#stateInfo-tab').hasClass('required')) {
            if (grid.dataSource._data.length == 0) {
                errorMessage = "Please add at least one state in State Info section!\n";
                isValid = false;
            }
        }

        csDateRange = csDateRange?.future != null ? csDateRange?.future : csDateRange;

        //BUG 125194 2/11/2025
        var validStateDateRangeText = '';

        jQuery.each(grid.dataSource._data, function (index, stateInfo) {
            if ($.trim(stateInfo.STATE_MANDATED_IND) == '') {
                errorMessage = "Please select state mandated for the selected state in State Info section!\n";
                isValid = false;
            }

            //stateInfo.ATS_EFF_DT != null && stateInfo.ATS_EFF_DT != '' && stateInfo.ATS_EXP_DT != null && stateInfo.ATS_EXP_DT != ''
            if (stateInfo.ATS_EFF_DT != null && stateInfo.ATS_EXP_DT != null) {
                var ats_eff_dt = kendo.parseDate(stateInfo.ATS_EFF_DT);
                var ats_exp_dt = kendo.parseDate(stateInfo.ATS_EXP_DT);

                if (ats_eff_dt > ats_exp_dt && ats_exp_dt != null) { // User Story 129434 MFQ 3/6/2025
                    errorMessage = "One of the state's effective date is greater than expiration date! Please verify and correct!\n";
                    isValid = false;
                }                
            }

            //BUG 125194 2/11/2025
            if (currentStatus != null) {
                if (validStateDateRangeText == '') {
                    var ats_eff_dt = kendo.parseDate(stateInfo.ATS_EFF_DT);
                    var ats_exp_dt = kendo.parseDate(stateInfo.ATS_EXP_DT);
                    if (ats_eff_dt != null && !(ats_eff_dt >= csDateRange.startDate && ats_exp_dt <= csDateRange.endDate)) {
                        validStateDateRangeText = "One of the state's date range is out of Current status date range! Please verify and correct!\n";
                        isValid = false;
                    }
                }
            }
            //#END BUG 125194 2/11/2025
        });  


        // If validation is failed then error message will be filled up in here. 
        var stateInclExlValidation = StateInfoController.data.init();

        if (stateInclExlValidation != '' || validStateDateRangeText != '') {
            errorMessage += stateInclExlValidation + validStateDateRangeText;
            isValid = false;
        }
        
        var result = {
            isValid: isValid,
            errorMessage: errorMessage
        };

        return result;
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
        focusStateInfoTab: function () {            
            $('.tab-pane,.nav-link').removeClass('active show');
            $('#stateInfo-tab').addClass('active');
            $('#stateInfo').addClass('active show');
        },
        renderStateInfoOnDuplicateRecordDetail: function (grid) {

            var statesArray = $('#ValidStatesList').val().split(',');
            var isBusinessSegmentCnsIfp = $("#ddlLOB").data('kendoDropDownList').value().toLowerCase() == 'cns' || $("#ddlLOB").data('kendoDropDownList').value().toLowerCase() == 'ifp';
            var entityValue = $('#ddlEntity').data('kendoDropDownList').value();

            if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) < 0) {

                StateInfoController.methods.showStateAddToolbar(grid);                

            } else if (isBusinessSegmentCnsIfp &&
                statesArray.indexOf(entityValue) > -1) {

                StateInfoController.methods.hideStateAddToolbar(grid);                

            } else if (!isBusinessSegmentCnsIfp) {

                StateInfoController.methods.showStateAddToolbar(grid);

            }            
        },
        renderStateInfoOnAddEditDetail: function (grid) {            
            var statesArray = $('#ValidStatesList').val().split(',');    
            var isBusinessSegmentCnsIfp = $("#txtBusinessSegment").val().toLowerCase() == 'cns' || $("#txtBusinessSegment").val().toLowerCase() == 'ifp';
            var entityValue = $('#txtEntity').val();

            if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) < 0) { //$('#txtEntity').val().toLowerCase() == 'cns'

                StateInfoController.methods.showStateAddToolbar(grid);

            } else if (isBusinessSegmentCnsIfp && statesArray.indexOf(entityValue) > -1) { //$('#txtEntity').val().toLowerCase() != 'cns'

                StateInfoController.methods.hideStateAddToolbar(grid);

            } else if (!isBusinessSegmentCnsIfp) {

                StateInfoController.methods.showStateAddToolbar(grid);

            }                
        },
        showStateAddToolbar: function(grid) {
            $("#grid_StateInfo .k-grid-toolbar").show();
            grid.showColumn(7); //BUG 50962 - FIX Show Delete button
        },
        hideStateAddToolbar: function (grid) {
            $("#grid_StateInfo .k-grid-toolbar").hide();
            grid.hideColumn(7); //BUG 50962 - FIX Hide Delete button
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
    StateInfoController.init();    
});