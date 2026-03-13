//US-44673

var ProgMgdByController = {
    bind: function () {
        $("#grid_ProgMgdBy").on("click", ".progmgdby-delete", function () {
            ProgMgdByController.grid.deleteRow(this);
        });
    },
    data: {
        init: function () {
            var _data = this;

            _data.PROG_MGD_BY = [];
            _data.DELEGATED_UM = [];
            _data.PMB_EFF_DT = [];
            _data.PMB_EXP_DT = [];
            _data.PMB_BASED_ON_DX_IND = [];
            _data.PMB_BASED_ON_ST_APP_IND = [];
            _data.PMB_BASED_ON_AGE_MIN = [];
            _data.PMB_BASED_ON_AGE_MAX = [];

            var progMgdByData = $("#grid_ProgMgdBy").data("kendoGrid").dataSource._data;
            jQuery.each(progMgdByData, function (index, progMgdBy) {

                //Added 1/9/2023 MFQ US-44673
                if ($.trim(progMgdBy.PMB_EFF_DT).length) {
                    _data.PMB_EFF_DT.push(kendo.toString(kendo.parseDate(progMgdBy.PMB_EFF_DT), 'yyyy-MM-dd HH:mm:ss'));
                } else {
                    _data.PMB_EFF_DT.push(null);
                }

                if ($.trim(progMgdBy.PMB_EXP_DT).length) {
                    _data.PMB_EXP_DT.push(kendo.toString(kendo.parseDate(progMgdBy.PMB_EXP_DT), 'yyyy-MM-dd HH:mm:ss'));
                } else {
                    _data.PMB_EXP_DT.push(null);
                }

                _data.PROG_MGD_BY.push(progMgdBy.PROG_MGD_BY);
                _data.DELEGATED_UM.push(progMgdBy.DELEGATED_UM);
                _data.PMB_BASED_ON_DX_IND.push(progMgdBy.PMB_BASED_ON_DX_IND);
                _data.PMB_BASED_ON_ST_APP_IND.push(progMgdBy.PMB_BASED_ON_ST_APP_IND);
                _data.PMB_BASED_ON_AGE_MIN.push(progMgdBy.PMB_BASED_ON_AGE_MIN);
                _data.PMB_BASED_ON_AGE_MAX.push(progMgdBy.PMB_BASED_ON_AGE_MAX);
            });

            _data.PROG_MGD_BY = _data.PROG_MGD_BY.join(',');
            _data.DELEGATED_UM = _data.DELEGATED_UM.join(',');
            //BUG 140238 11/10/2025 MFQ
            _data.PMB_EFF_DT = _data.PMB_EFF_DT.map(function (date) {
                return kendo.toString(kendo.parseDate(date), "yyyy-MM-dd")
            }).join(',');
            _data.PMB_EXP_DT = _data.PMB_EXP_DT.map(function (date) {
                return kendo.toString(kendo.parseDate(date), "yyyy-MM-dd")
            }).join(',');
            //END BUG 140238
            _data.PMB_BASED_ON_DX_IND = _data.PMB_BASED_ON_DX_IND.join(',');
            _data.PMB_BASED_ON_ST_APP_IND = _data.PMB_BASED_ON_ST_APP_IND.join(',');
            _data.PMB_BASED_ON_AGE_MIN = _data.PMB_BASED_ON_AGE_MIN.join(',');
            _data.PMB_BASED_ON_AGE_MAX = _data.PMB_BASED_ON_AGE_MAX.join(',');
        },
        PROG_MGD_BY: [],
        DELEGATED_UM: [],
        PMB_EFF_DT: [],
        PMB_EXP_DT: [],
        PMB_BASED_ON_DX_IND: [],
        PMB_BASED_ON_ST_APP_IND: [],
        PMB_BASED_ON_AGE_MIN: [],
        PMB_BASED_ON_AGE_MAX: []
    },
    grid: {
        params: function () {
            var ePALPageView = MIApp.Global.Page.ePALPageView();

            var paramObj = {
                p_EPAL_HIERARCHY_KEY: ePALPageView == "duplicaterecorddetail" ? $('#btnGotoDetail').html().trim() : $("#txtPIMSID").val(),
                p_EPAL_VER_EFF_DT: ePALPageView == "duplicaterecorddetail" ? $('#txtOrigEPALVersionDt').val() : $("#txtEPALVersionDt").val(),
                p_EPAL_BUS_SEG_CD: ($('#txtOrigEPALBusSegCD').val() != undefined ? $('#txtOrigEPALBusSegCD').val() : null),
                EPALPageView: ePALPageView
            }
            return MIApp.Sanitize.encodeObject(paramObj);
        },
        dataSource: {
            onChange: function (e) {
                /*if (e.action == "itemchange") {
                    DetailController.dirtyform.dirtycheck();

                    if (e.field == "PMB_EXP_DT") {
                        var pmb_exp_dt = kendo.parseDate(e.items.length ? e.items[0].PMB_EXP_DT : null);
                        var today_dt = kendo.parseDate(new Date());

                        if (pmb_exp_dt <= today_dt) {
                            clearExpiredPMB();
                        }
                    }
                }*/
            }
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");

            var progMgdBy = row.closest('tr').find('td:eq(1)').text()

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                //text: text,
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_ProgMgdBy").data("kendoGrid").removeRow(row);
                    DiagListController.grid.updateRow(progMgdBy);
                    StateInfoController.grid.updateRow(progMgdBy);
                    DetailController.dirtyform.dirtycheck();
                }
            });
        },
        DELEGATED_UMEditable: function (dataItem) {
            return false; // Bug 123635 MFQ 3-24-2025
            /*
            if (dataItem.PROG_MGD_BY != null && dataItem.PROG_MGD_BY != '') {
                return false;
            } else {
                return true;
            }*/
        },
        PROG_MGD_BYEditable: function (dataItem) {
            if (dataItem.DELEGATED_UM != null && dataItem.DELEGATED_UM != '') {
                return false;
            } else {
                return true;
            }
        },
        INTERNAL_EXTERNALEditable: function () {
            return false;
        },
        PMB_BASED_ON_DX_INDEditable: function () {
            return false;
        },
        PMB_BASED_ON_ST_APP_INDEditable: function () {
            return false;
        },
        getExpiredPMB: function () {
            var arrExpiredProgMgdBys = [];
            var grid = $("#grid_ProgMgdBy").data("kendoGrid");
            if (grid.dataSource._data.length > 0) {
                jQuery.each(grid.dataSource._data, function (index, progMgdBy) {
                    var pmb_exp_dt = kendo.parseDate(progMgdBy.PMB_EXP_DT);
                    var today_dt = kendo.parseDate(new Date());

                    if (pmb_exp_dt != null) {
                        if (pmb_exp_dt <= today_dt && !MIApp.Common.Helper.isStringNullOrEmpty(progMgdBy.PROG_MGD_BY)) {
                            arrExpiredProgMgdBys.push(progMgdBy.PROG_MGD_BY);
                        }
                    }
                });
            }
            return arrExpiredProgMgdBys;
        },
        onDataBound: function () {
            //$("#grid_ProgMgdBy .k-grid-content").attr("style", "width: auto;height: auto");

            //clearExpiredPMB();
        },
        addRow: function (prog_mgd_by) {
            //var todays_date = new Date();
            var grid = $("#grid_ProgMgdBy").data("kendoGrid");
            var datasource = grid.dataSource;

            if (!ProgMgdByController.isDuplicatePROG_MGD_BY(grid, prog_mgd_by) && prog_mgd_by != "") {
                //diagList_prgmgby_counter += 1
                datasource.insert({
                    DELEGATED_UM: null,
                    PROG_MGD_BY: prog_mgd_by,
                    INT_EXT_CD: null,
                    PMB_EFF_DT: null,
                    PMB_EXP_DT: null,
                    PMB_BASED_ON_DX_IND: "Yes",
                    PMB_BASED_ON_ST_APP_IND: "No",
                    PMB_BASED_ON_AGE_MIN: 0,
                    PMB_BASED_ON_AGE_MAX: 0
                });
            }
        },
        updateRow: function (progMgdBy, caller, yesNo) {
            var progMgdByArray = [];
            var progMgdByObj = [];
            var data = $("#grid_ProgMgdBy").data("kendoGrid").dataSource._data;
            jQuery.each(data, function (index, e) {
                progMgdByObj = {
                    "DELEGATED_UM": e.DELEGATED_UM,
                    "PROG_MGD_BY": e.PROG_MGD_BY,
                    "INT_EXT_CD": e.INT_EXT_CD,
                    "PMB_EFF_DT": e.PMB_EFF_DT,
                    "PMB_EXP_DT": e.PMB_EXP_DT,
                    "PMB_BASED_ON_DX_IND": e.PMB_BASED_ON_DX_IND,
                    "PMB_BASED_ON_ST_APP_IND": e.PMB_BASED_ON_ST_APP_IND,
                    "PMB_BASED_ON_AGE_MIN": e.PMB_BASED_ON_AGE_MIN,
                    "PMB_BASED_ON_AGE_MAX": e.PMB_BASED_ON_AGE_MAX,
                }

                progMgdByArray.push(progMgdByObj);

            });

            //Find index of specific object using findIndex method.
            objIndex = progMgdByArray.findIndex((obj => obj.PROG_MGD_BY == progMgdBy));


            if (objIndex > -1) {
                if (caller === 'stateinfo' && (progMgdBy != "" && progMgdBy != "null" && progMgdBy != null)) {
                    //Update object's name property.
                    if (yesNo == "Yes") {
                        progMgdByArray[objIndex].PMB_BASED_ON_ST_APP_IND = "Yes";
                    }
                    else if (yesNo == "No") {
                        progMgdByArray[objIndex].PMB_BASED_ON_ST_APP_IND = "No";
                    }
                }

                if (caller === 'diags' && (progMgdBy != "" && progMgdBy != "null" && progMgdBy != null)) {
                    //Update object's name property.
                    if (yesNo == "Yes") {
                        progMgdByArray[objIndex].PMB_BASED_ON_DX_IND = "Yes";
                    }
                    else if (yesNo == "No") {
                        progMgdByArray[objIndex].PMB_BASED_ON_DX_IND = "No";
                    }
                }

                if (caller === 'progMgdBy') {
                    //Update object's name property.
                    progMgdByArray[objIndex].PMB_BASED_ON_ST_APP_IND = "No"
                    progMgdByArray[objIndex].PMB_BASED_ON_DX_IND = "No"
                }


                //Empty grid
                $("#grid_ProgMgdBy").data("kendoGrid").dataSource.data([]);

                var grid = $("#grid_ProgMgdBy").data("kendoGrid");
                var datasource = grid.dataSource;


                //Append new grid data. 
                var i;
                for (i = 0; i < progMgdByArray.length; ++i) {
                    datasource.insert(progMgdByArray[i]);
                }

            }


        },
        updatePMBExpDate: function (pmbExpDt, priorAuthInd) { //, effDt

            var todaysdate = new Date();
            var todays_date2 = kendo.toString(kendo.parseDate(todaysdate), 'MM/dd/yyyy')

            var progMgdByArray = []
            var progMgdByObj = []
            var data = $("#grid_ProgMgdBy").data("kendoGrid").dataSource._data;

            if (priorAuthInd == "No") {
                jQuery.each(data, function (index, e) {
                    progMgdByObj = {
                        "DELEGATED_UM": e.DELEGATED_UM,
                        "PROG_MGD_BY": e.PROG_MGD_BY,
                        "INT_EXT_CD": e.INT_EXT_CD,
                        "PMB_EFF_DT": e.PMB_EFF_DT,
                        "PMB_EXP_DT":
                            kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == null || kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == '12/31/2999' ?
                                (pmbExpDt == null || pmbExpDt == '' ? kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') : pmbExpDt) :
                                kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy'),
                        "PMB_BASED_ON_DX_IND": e.PMB_BASED_ON_DX_IND,
                        "PMB_BASED_ON_ST_APP_IND": e.PMB_BASED_ON_ST_APP_IND,
                        "PMB_BASED_ON_AGE_MIN": e.PMB_BASED_ON_AGE_MIN,
                        "PMB_BASED_ON_AGE_MAX": e.PMB_BASED_ON_AGE_MAX,
                    }
                    progMgdByArray.push(progMgdByObj);
                });
            }

            else if (priorAuthInd == "Yes" && (new Date(pmbExpDt) > new Date(todays_date2))) { // MFQ 12/17/2025 PRE0001115 Disappearing PMB Issue and not able to term codes.
                jQuery.each(data, function (index, e) {
                    progMgdByObj = {
                        "DELEGATED_UM": e.DELEGATED_UM,
                        "PROG_MGD_BY": e.PROG_MGD_BY,
                        "INT_EXT_CD": e.INT_EXT_CD,
                        "PMB_EFF_DT": e.PMB_EFF_DT,
                        "PMB_EXP_DT":
                            kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == null || kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == '12/31/2999' ?
                                (pmbExpDt == null || pmbExpDt == '' ? kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') : pmbExpDt) :
                                kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy'),
                        "PMB_BASED_ON_DX_IND": e.PMB_BASED_ON_DX_IND,
                        "PMB_BASED_ON_ST_APP_IND": e.PMB_BASED_ON_ST_APP_IND,
                        "PMB_BASED_ON_AGE_MIN": e.PMB_BASED_ON_AGE_MIN,
                        "PMB_BASED_ON_AGE_MAX": e.PMB_BASED_ON_AGE_MAX,
                    }
                    progMgdByArray.push(progMgdByObj);
                });
            }

            else if (priorAuthInd == "Yes" && (pmbExpDt == "")) {
                jQuery.each(data, function (index, e) {
                    progMgdByObj = {
                        "DELEGATED_UM": e.DELEGATED_UM,
                        "PROG_MGD_BY": e.PROG_MGD_BY,
                        "INT_EXT_CD": e.INT_EXT_CD,
                        "PMB_EFF_DT": e.PMB_EFF_DT,
                        "PMB_EXP_DT":
                            kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == null || kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') == '12/31/2999' ?
                                (pmbExpDt == null || pmbExpDt == '' ? kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy') : pmbExpDt) :
                                kendo.toString(kendo.parseDate(e.PMB_EXP_DT), 'MM/dd/yyyy'),
                        "PMB_BASED_ON_DX_IND": e.PMB_BASED_ON_DX_IND,
                        "PMB_BASED_ON_ST_APP_IND": e.PMB_BASED_ON_ST_APP_IND,
                        "PMB_BASED_ON_AGE_MIN": e.PMB_BASED_ON_AGE_MIN,
                        "PMB_BASED_ON_AGE_MAX": e.PMB_BASED_ON_AGE_MAX,
                    }
                    progMgdByArray.push(progMgdByObj);
                });
            }

            //Find index of specific object using findIndex method.
            objIndex = progMgdByArray.findIndex((obj => kendo.toString(kendo.parseDate(obj.PMB_EXP_DT) == null || kendo.toString(kendo.parseDate(obj.PMB_EXP_DT) == '12/31/2999'))));

            //Empty grid
            $("#grid_ProgMgdBy").data("kendoGrid").dataSource.data([]);

            var grid = $("#grid_ProgMgdBy").data("kendoGrid");
            var datasource = grid.dataSource;

            //Append new grid data. 
            var i;
            for (i = 0; i < progMgdByArray.length; ++i) {
                datasource.insert(progMgdByArray[i]);
            }
        },
        row_count: function () {
            var grid = $("#grid_ProgMgdBy").data("kendoGrid");
            var dataSource = grid.dataSource;
            var totalRecords = dataSource.total();

            return totalRecords;
        },
        verifyPMBbyIndicator: function () {
            var pmbData = $("#grid_ProgMgdBy").data("kendoGrid").dataSource._data;
            var dxListData = $("#grid_DiagList").data("kendoGrid").dataSource._data;
            var dxCodesData = $("#grid_DiagCodes").data("kendoGrid").dataSource._data;
            var statesData = $("#grid_StateInfo").data("kendoGrid").dataSource._data;

            jQuery.each(pmbData, function (index, pmbItem) {
                var isDXInd = false;

                // Check if DX List has matching PMB
                jQuery.each(dxListData, function (index, dxListItem) {
                    if (dxListItem.PROG_MGD_BY == pmbItem.PROG_MGD_BY) {
                        isDXInd = true;
                    }
                });

                // Check if DX Codes has matching PMB
                if (dxListData.length == 0) {
                    jQuery.each(dxCodesData, function (index, dxCodesItem) {
                        if (dxCodesItem.PROG_MGD_BY == pmbItem.PROG_MGD_BY) {
                            isDXInd = true;
                        }
                    });
                }

                if (!isDXInd) pmbItem.PMB_BASED_ON_DX_IND = 'No';

                // Check if States has matching PMB
                var isStatesInd = false;
                jQuery.each(statesData, function (index, statesItem) {
                    if (statesItem.ATS_PROG_MGD_BY == pmbItem.PROG_MGD_BY) {
                        isStatesInd = true;
                    }
                });

                if (!isStatesInd) pmbItem.PMB_BASED_ON_ST_APP_IND = 'No';
            });

            var progMgdByArray = [];
            jQuery.each(pmbData, function (index, e) {
                progMgdByObj = {
                    "DELEGATED_UM": e.DELEGATED_UM,
                    "PROG_MGD_BY": e.PROG_MGD_BY,
                    "INT_EXT_CD": e.INT_EXT_CD,
                    "PMB_EFF_DT": e.PMB_EFF_DT,
                    "PMB_EXP_DT": e.PMB_EXP_DT,
                    "PMB_BASED_ON_DX_IND": e.PMB_BASED_ON_DX_IND,
                    "PMB_BASED_ON_ST_APP_IND": e.PMB_BASED_ON_ST_APP_IND,
                    "PMB_BASED_ON_AGE_MIN": e.PMB_BASED_ON_AGE_MIN,
                    "PMB_BASED_ON_AGE_MAX": e.PMB_BASED_ON_AGE_MAX,
                }

                progMgdByArray.push(progMgdByObj);
            });

            $("#grid_ProgMgdBy").data("kendoGrid").dataSource.data([]);

            var _grid_ProgMgdBy = $("#grid_ProgMgdBy").data("kendoGrid");
            var datasource = _grid_ProgMgdBy.dataSource;

            //Append new grid data. 
            var i;
            for (i = 0; i < progMgdByArray.length; ++i) {
                datasource.insert(progMgdByArray[i]);
            }
        }
    },
    validation: function () {
        var isValid = true;
        var errorMessage = "";
        var grid = $("#grid_ProgMgdBy").data("kendoGrid");
        var expdate_count = 0;
        var row_count = 0;
        var max_pmb_dt = null;
        var pmb_pa = 0;
        var pmb_predet = 0;
        var pmb_adv = 0;
        var pmb_dral = 0;
        var pmb_all = { ind: true, value: 0 };
        var paEff = $.trim($('#dpPAEffectiveDate').val());
        var paExp = $.trim($('#dpPAExpirationDate').val());
        var preDetEff = $.trim($('#dpPreDeterminationEffDate').val());
        var preDetExp = $.trim($('#dpPreDeterminationExpDate').val());
        var advEff = $.trim($('#dpAdvNotificationEffDate').val());
        var advExp = $.trim($('#dpAdvNotificationExpDate').val());
        var dralEff = $.trim($('#dpDRALEffDate').val());
        var dralExp = $.trim($('#dpDRALExpDate').val());
        var dupPMB_PROG_MGD_BY = []; // stores duplicate Program Managed By 
        var outOfDrivingRecordPMB = false;
        // If validation is failed then error message will be filled up in here. 
        ProgMgdByController.data.init();

        var total_pmb = grid.dataSource._data.length;
        var allProg_Mgd_By = grid.dataSource._data;

        var dateRanges = [];
        if ($.trim(paEff) != '') dateRanges.push({ name: 'PA', startDate: new Date(paEff), endDate: ($.trim(paExp) == '' ? new Date('12/31/2999') : new Date(paExp)) });
        if ($.trim(preDetEff) != '') dateRanges.push({ name: 'Pred', startDate: new Date(preDetEff), endDate: ($.trim(preDetExp) == '' ? new Date('12/31/2999') : new Date(preDetExp)) });
        if ($.trim(advEff) != '') dateRanges.push({ name: 'Adv', startDate: new Date(advEff), endDate: ($.trim(advExp) == '' ? new Date('12/31/2999') : new Date(advExp)) });
        if ($.trim(dralEff) != '') dateRanges.push({ name: 'DRAL', startDate: new Date(dralEff), endDate: ($.trim(dralExp) == '' ? new Date('12/31/2999') : new Date(dralExp)) });

        var activeDateRange = getDrivingStatusDateRangeIncludingToday(dateRanges, null);

        pmb_all.ind = false;

        // Loop through all PMBs
        jQuery.each(allProg_Mgd_By, function (index, progMgdBy) {
            row_count++;
            validatePMBRecord(progMgdBy, activeDateRange);
        });
        // END Loop through all PMBs

        /**
         * START Validate duplicate Program Managed By date ranges
         * */
        if (allProg_Mgd_By.length >= 2) {

            var isPmbProgMgdByDatesDuplicate = false;

            const dupProg_Mgd_By = allProg_Mgd_By.filter(function (prog_mgd_by) {
                return prog_mgd_by.PMB_BASED_ON_DX_IND == 'No' && prog_mgd_by.PMB_BASED_ON_ST_APP_IND == 'No';
            });

            for (i = 0; i < allProg_Mgd_By.length - 1; i += 1) {
                if (dupProg_Mgd_By.length >= 2) {
                    //if (allProg_Mgd_By[i].PMB_EFF_DT != null) {
                    for (j = i + 1; j < dupProg_Mgd_By.length; j += 1) {
                        if (dupProg_Mgd_By[j].PMB_EFF_DT != null) {
                            isPmbProgMgdByDatesDuplicate =
                                MIApp.Common.Helper.multipleDateRangeOverlaps([
                                    { from: dupProg_Mgd_By[i].PMB_EFF_DT, to: (dupProg_Mgd_By[i].PMB_EXP_DT == null ? kendo.parseDate('12/31/2999') : (dupProg_Mgd_By[i].PMB_EXP_DT == null ? kendo.parseDate('12/31/2999') : kendo.parseDate(dupProg_Mgd_By[i].PMB_EXP_DT))) },
                                    { from: dupProg_Mgd_By[j].PMB_EFF_DT, to: (dupProg_Mgd_By[j].PMB_EXP_DT == null ? kendo.parseDate('12/31/2999') : (dupProg_Mgd_By[j].PMB_EXP_DT == null ? kendo.parseDate('12/31/2999') : kendo.parseDate(dupProg_Mgd_By[j].PMB_EXP_DT))) }
                                ]);

                            if (isPmbProgMgdByDatesDuplicate) break;
                        }
                    }
                    //}
                }
            }

            if (isPmbProgMgdByDatesDuplicate) {
                errorMessage += "One or more of the same Program Managed by date overlaps! Please verify dates and correct in Program Managed by section!\n";
                isValid = false;
            }
        }
        /**
         * END Validate duplicate Program Managed By date ranges
         * */
        validatePMBPeriodRequirements();

        //BUG Bug 140368 MFQ 11/13/2025
        if (activeDateRange != null) {
            if (activeDateRange.name == 'PA' || activeDateRange.name == 'Pred' || activeDateRange.name == 'Adv')
                validatePMBCoverage(allProg_Mgd_By, activeDateRange);
        }

        if (outOfDrivingRecordPMB) {
            errorMessage += "Record cannot save - Program Managed By effective dates precede the effective date of the record – Please remove the PMB entry with dates that precede the effective date of the record\n";
            isValid = false;
        }

        function isEmpty(value) {
            return value == null || $.trim(value) === '';
        }

        function addError(message) {
            errorMessage += message + "\n";
            isValid = false;
        }

        function validatePMBRecord(progMgdBy, activeDateRange) {
            const effDate = kendo.parseDate(progMgdBy.PMB_EFF_DT);
            const expDate = kendo.parseDate(progMgdBy.PMB_EXP_DT) || kendo.parseDate("12/31/2999");

            if (!effDate) {
                addError("PMB Effective date is required in Program Managed by section");
            }

            if (!isEmpty(progMgdBy.PMB_PROG_MGD_BY) && isEmpty(progMgdBy.INT_EXT_CD)) {
                addError("Please select Internal/External in one of the record of Program Managed by list!");
            }

            if (effDate && expDate && effDate > expDate) {
                addError("One of the Program Managed by effective date is greater than expiration date! Please verify and correct in Program Managed by section!");
            }

            if (isEmpty(progMgdBy.PMB_BASED_ON_DX_IND)) {
                addError("One of the Program Managed by DX Indicator value is not selected! Please verify and correct in Program Managed by section!");
            }

            if (isEmpty(progMgdBy.PMB_BASED_ON_ST_APP_IND)) {
                addError("One of the Program Managed by State Indicator value is not selected! Please verify and correct in Program Managed by section!");
            }

            if (progMgdBy.PMB_BASED_ON_AGE_MIN > progMgdBy.PMB_BASED_ON_AGE_MAX) {
                addError("One of the Program Managed by Min age is greater than Max age! Please verify and correct in Program Managed by section!");
            }

            const expectedStart = activeDateRange?.startDate;
            const expectedEnd = activeDateRange?.future == null ? activeDateRange?.endDate : activeDateRange?.future?.endDate;

            if (effDate >= expectedStart && expDate <= expectedEnd && activeDateRange != null) {
                if (isEmpty(progMgdBy.PROG_MGD_BY)) {
                    addError(`Program Managed By is required in Program Managed by section since ${activeDateRange.name} is set to YES!`);
                }

                switch (activeDateRange.name) {
                    case 'PA': pmb_pa++; break;
                    case 'Pred': pmb_predet++; break;
                    case 'Adv': pmb_adv++; break;
                    //case 'DRAL': pmb_dral++; break;
                    default:
                        pmb_all.value++;
                        pmb_all.ind = true;
                        break;
                }
            } else {
                outOfDrivingRecordPMB = true;
            }
        }

        function validatePMBPeriodRequirements() {
            if (total_pmb <= 0) return;

            const noEffectiveDatesEntered =
                grid.dataSource._data.length > 0 &&
                isEmpty($('#dpPAEffectiveDate').val()) &&
                isEmpty($('#dpPreDeterminationEffDate').val()) &&
                isEmpty($("#dpAdvNotificationEffDate").val()) &&
                isEmpty($("#dpDRALEffDate").val());

            if (noEffectiveDatesEntered) {
                addError("Either the PA Effective Date / Pre Deteremination Effective Date / Advanced Notification / Drug Review At Launch must be entered when Program Managed By is populated.");
                return;
            }

            switch (activeDateRange.name) {
                case 'PA':
                    if (pmb_pa === 0) {
                        addError("There must be an active Program Managed By value for the Prior Auth period!");
                    }
                    break;
                case 'Pred':
                    if (pmb_predet === 0) {
                        addError("There must be an active Program Managed By value for the Pre-Determination period!");
                    }
                    break;
                case 'Adv':
                    if (pmb_adv === 0) {
                        addError("There must be an active Program Managed By value for the Advanced Notification period!");
                    }
                    break;
                //case 'DRAL':
                //    if (pmb_dral === 0) {
                //        addError("There must be an active Program Managed By value for the Drug Review At Launch period!");
                //    }
                //    break;
                default:
                    break;
            }
        }

        function validatePMBCoverage(allProg_Mgd_By, activeDateRange) {
            if (!activeDateRange || allProg_Mgd_By.length === 0) return;

            const expectedStart = activeDateRange.startDate;
            const expectedEnd = activeDateRange.future == null ? activeDateRange.endDate : activeDateRange.future.endDate;

            const pgmPeriods = allProg_Mgd_By.map(p => {
                const start = kendo.parseDate(p.PMB_EFF_DT);
                const end = kendo.parseDate(p.PMB_EXP_DT) || kendo.parseDate("12/31/2999");
                return start && end ? { start, end } : null;
            }).filter(p => p !== null);

            if (pgmPeriods.length === 0) {
                addError("Program Managed By is required in Program Managed by section since " + activeDateRange.name + " is set to YES!");
                return;
            }

            // Sort by start date
            pgmPeriods.sort((a, b) => a.start - b.start);

            let current = expectedStart;
            let coverageValid = true;

            for (let i = 0; i < pgmPeriods.length; i++) {
                const period = pgmPeriods[i];

                // Check for exact day-to-day continuity
                if (period.start.getTime() !== current.getTime()) {
                    coverageValid = false; // Gap detected
                    break;
                }

                // Move current to the day after this period's end
                current = new Date(period.end);
                current.setDate(current.getDate() + 1);

                // If we've covered the full range, exit early
                if (current > expectedEnd) break;
            }

            // Final check: did we reach or pass the expected end date?
            if (current <= expectedEnd || !coverageValid) {
                addError("There must be a program assigned for the entire timespan of the current status of the record");
            }
        }


        var result = {
            isValid: isValid,
            errorMessage: errorMessage
        };

        return result;
    },

    isDuplicatePROG_MGD_BY: function (grid, value) {
        var data = grid.dataSource._data;
        var duplicate = false;
        jQuery.each(data, function (index, rowData) {
            if (rowData.PROG_MGD_BY == value) {
                return duplicate = true;
            }
        });

        return duplicate;
    }
}


function clearExpiredPMB() {
    var arrExpiredPMB = ProgMgdByController.grid.getExpiredPMB();
    if (arrExpiredPMB.length > 0) {
        DiagListController.grid.clearExpiredPMB(arrExpiredPMB);
        StateInfoController.grid.clearExpiredPMB(arrExpiredPMB);
    }
}

var pmb_pa = 0;
var pmb_predet = 0;
var pmb_adv = 0;
var pmb_dral = 0;
var total_pmb = 0;

$(document).ready(function () {
    ProgMgdByController.bind();
});
