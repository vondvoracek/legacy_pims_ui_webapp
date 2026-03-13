var gridConfigDataCache = {};

var DPOC_ConfigController = {
    loadData: function (isHover) {        

        var gridId = isHover === true ? 'grid_config_hover' : 'grid_config';
        var grid_config = $("#" + gridId).data("kendoGrid");

        var key = DPOC_GuidelineSummaryController.getGridCacheKey();


        DetailController.grid.config.data.forEach(item => {
            if (typeof item.STATES_APPL === "string" && item.STATES_APPL.length) {
                // Replace pipes with commas and normalize whitespace
                item.STATES_APPL = item.STATES_APPL.replace(/\|/g, ",").replace(/\s*,\s*/g, ",").trim();

                // Optional: remove leading/trailing commas
                item.STATES_APPL = item.STATES_APPL.replace(/^,|,$/g, "");
            }
        });

        var config_by_gdln_id = DetailController.grid.config.data.filter((config) =>
            config.DTQ_IQ_GDLN_ID == ($('#txtIQ_GDLN_ID_GS').val() || $('#txtIQ_GDLN_ID_GS_hdn').val()) &&
            config.DPOC_RELEASE == ($('#txtDPOCRelease_GS').val() || $('#txtDPOCRelease_GS_hdn').val()) &&
            config.DPOC_VER_NUM == ($('#txtDPOC_VER_NUM_GS').val() || $('#txtDPOC_VER_NUM_hdn').val())
        );

        if (grid_config !== undefined) {
            if (gridConfigDataCache[key]) {
                grid_config.dataSource.data(gridConfigDataCache[key]);
            } else if (config_by_gdln_id.length > 0) {
                grid_config.dataSource.data(config_by_gdln_id);
            }
        }

        //resizeConfigGridInWindow();
        DPOC_ConfigController.bind();
    },

    addData: function () {
        const grid_configData = $("#grid_config").data("kendoGrid").dataSource.data();
        const guidelineId = $('#txtIQ_GDLN_ID_GS').val();
        const dpocVerNum = $('#txtDPOC_VER_NUM_GS').val();
        const dpocVerNumData = $('#txtDPOC_VER_NUM_GS').data('version');
        const dpocRelease = $('#txtDPOCRelease_GS').val();
        const dpocReleaseData = $('#txtDPOCRelease_GS').data('release');

        // Remove entries only if textbox values differ from data attributes
        if (dpocVerNum !== dpocVerNumData || dpocRelease !== dpocReleaseData) {
            DetailController.grid.config.data = DetailController.grid.config.data.filter(d =>
                !(d.DTQ_IQ_GDLN_ID === guidelineId &&
                    d.DPOC_VER_NUM === dpocVerNumData &&
                    d.DPOC_RELEASE === dpocReleaseData)
            );
        } else {
            // Remove entries that are no longer in the grid
            DetailController.grid.config.data = DetailController.grid.config.data.filter(d => {
                if (d.DTQ_IQ_GDLN_ID === guidelineId &&
                    d.DPOC_VER_NUM === dpocVerNum &&
                    d.DPOC_RELEASE === dpocRelease) {
                    // Check if this entry exists in grid_configData
                    const existsInGrid = grid_configData.some(dtq =>
                        (d.GDLN_DTQ_SYS_SEQ && d.GDLN_DTQ_SYS_SEQ === dtq.GDLN_DTQ_SYS_SEQ) ||
                        (d.tempId && d.tempId === dtq.tempId)
                    );
                    return existsInGrid; // Keep only if still in grid
                }
                return true; // Keep entries from other guidelines/versions/releases
            });
        }

        grid_configData.forEach(function (dtq) {
            //BUG 140320 MQF 11/12/2025
            // Assign a temporary unique ID if sequence is 0 or missing
            if (!dtq.GDLN_DTQ_SYS_SEQ || dtq.GDLN_DTQ_SYS_SEQ === 0) {
                dtq.tempId = dtq.tempId || Date.now() + Math.random();
            }

            // Find existing entry using either real sequence or tempId
            const existingIndex = DetailController.grid.config.data.findIndex(d =>
                d.DTQ_IQ_GDLN_ID === guidelineId &&
                d.DPOC_VER_NUM === dpocVerNum &&
                d.DPOC_RELEASE === dpocRelease &&
                ((d.GDLN_DTQ_SYS_SEQ && d.GDLN_DTQ_SYS_SEQ === dtq.GDLN_DTQ_SYS_SEQ) ||
                    (d.tempId && d.tempId === dtq.tempId))
            );
            //END BUG 140320 MQF 11/12/2025

            const newEntry = {
                RowNumber: dtq.RowNumber,
                GDLN_DTQ_SYS_SEQ: dtq.GDLN_DTQ_SYS_SEQ,
                DTQ_IQ_GDLN_ID: guidelineId,
                POS_APPL: dtq.POS_APPL,
                POS_INCL_EXCL_CD: dtq.POS_INCL_EXCL_CD,
                POS_INCL_EXCL_DESC: dtq.POS_INCL_EXCL_DESC,
                STATES_APPL: dtq.STATES_APPL,
                STATES_INCL_EXCL_CD: dtq.STATES_INCL_EXCL_CD,
                STATES_INCL_EXCL_DESC: dtq.STATES_INCL_EXCL_DESC,
                DTQ_NM: dtq.DTQ_NM,
                TGT_DTQ: dtq.TGT_DTQ,
                TGT_DTQ_VERSION: dtq.TGT_DTQ_VERSION,
                HOLDING_DTQ: dtq.HOLDING_DTQ,
                HOLDING_DTQ_VERSION: dtq.HOLDING_DTQ_VERSION,
                DPOC_RELEASE: dpocRelease,
                DPOC_VER_NUM: dpocVerNum,
                DPOC_SOS_PROVIDER_TIN_EXCL: dtq.DPOC_SOS_PROVIDER_TIN_EXCL,
                DPOC_ADDTNL_RQRMNTS: dtq.DPOC_ADDTNL_RQRMNTS,
                PKG_CONFIG_COMMENTS: dtq.PKG_CONFIG_COMMENTS,
            };

            if (existingIndex !== -1) {
                // Update existing entry
                DetailController.grid.config.data[existingIndex] = newEntry;
            } else {
                // Add new entry
                DetailController.grid.config.data.push(newEntry);
            }
        });

        DPOC_ConfigController.updateStateDataFromConfig();
    },
    updateStatesOnConfig: function (GDLN_DTQ_SYS_SEQ, STATES, ATS_INCL_EXCL_CDs, ATS_INCL_EXCL_CD_DESC, RowNumber) {              
        var eConfigArr = [];
        var data = $("#grid_config").data("kendoGrid").dataSource.data();
        //debugger;
        jQuery.each(data, function (index, e) {
            eConfigArr.push({
                GDLN_DTQ_SYS_SEQ: e.GDLN_DTQ_SYS_SEQ == 0 ? GDLN_DTQ_SYS_SEQ : e.GDLN_DTQ_SYS_SEQ,
                DTQ_IQ_GDLN_ID: e.DTQ_IQ_GDLN_ID,
                POS_APPL: e.POS_APPL,
                POS_INCL_EXCL_CD: e.POS_INCL_EXCL_CD,
                POS_INCL_EXCL_DESC: e.POS_INCL_EXCL_DESC,
                STATES_APPL: e.STATES_APPL, //STATES == '' ? e.STATES_APPL : STATES,
                STATES_INCL_EXCL_CD: e.STATES_INCL_EXCL_CD, //STATES == '' ? e.STATES_INCL_EXCL_CD : ATS_INCL_EXCL_CDs,
                STATES_INCL_EXCL_DESC: e.STATES_INCL_EXCL_DESC, //STATES == '' ? e.STATES_INCL_EXCL_DESC : ATS_INCL_EXCL_CD_DESC,
                DTQ_NM: e.DTQ_NM,
                TGT_DTQ: e.TGT_DTQ,
                TGT_DTQ_VERSION: e.TGT_DTQ_VERSION,
                HOLDING_DTQ: e.HOLDING_DTQ,
                HOLDING_DTQ_VERSION: e.HOLDING_DTQ_VERSION,
                RowNumber: e.RowNumber,
                DPOC_VER_NUM: e.DPOC_VER_NUM,
                DPOC_RELEASE: e.DPOC_RELEASE,
                DPOC_SOS_PROVIDER_TIN_EXCL: e.DPOC_SOS_PROVIDER_TIN_EXCL, // User Story 138385 MFQ 9/22/2025
                DPOC_ADDTNL_RQRMNTS: e.DPOC_ADDTNL_RQRMNTS, // User Story 138385 MFQ 9/22/2025
                PKG_CONFIG_COMMENTS: e.PKG_CONFIG_COMMENTS, // User Story 138385 MFQ 9/22/2025
            });
        });

        var grid = $("#grid_config").data("kendoGrid");
        var dataItems = grid.dataSource.data(); // Use .view() if paging is enabled

        // Find the item to update
        var item = dataItems.find(item => item.RowNumber == RowNumber);

        if (item) {
            item.set("STATES_APPL", STATES);
            item.set("STATES_INCL_EXCL_CD", ATS_INCL_EXCL_CDs);
            item.set("STATES_INCL_EXCL_DESC", STATES === '' ? '' : ATS_INCL_EXCL_CD_DESC);
        }

        //if all other fields are also empty along with STATES, then DELETE the configuration ROW as well
        //E58 from UAT sheet MFQ 8/7/2025 -- Request by Kerri Stockton        
        if (
            isEmpty(item.POS_APPL) &&
            isEmpty(item.POS_INCL_EXCL_CD) &&
            isEmpty(item.STATES_APPL) &&
            isEmpty(item.DTQ_NM) &&
            isEmpty(item.TGT_DTQ) &&
            isEmpty(item.TGT_DTQ_VERSION) &&
            isEmpty(item.HOLDING_DTQ) &&
            isEmpty(item.HOLDING_DTQ_VERSION)
        ) {
            grid.dataSource.remove(item);
        }

        grid.refresh();
    },
    updateStateDataFromConfig: function () {

        const iqGuidelineId = $('#txtIQ_GDLN_ID_GS').val();
        const dtqSysSeq = parseInt($('#txtGDLN_DTQ_SYS_SEQ').val());
        const dpocVerNum = $('#txtDPOC_VER_NUM_GS').val();
        const dpocRelease = $('#txtDPOCRelease_GS').val();

        DetailController.grid.states.data = $.grep(DetailController.grid.states.data, function (d) {
            return !(
                d.ATS_IQ_GDLN_ID === iqGuidelineId &&
                //d.GDLN_DTQ_SYS_SEQ === dtqSysSeq &&
                d.DPOC_VER_NUM === dpocVerNum &&
                d.DPOC_RELEASE === dpocRelease
            );
        });

        // get state data from config opened popup
        var grid_configData = $("#grid_config").data("kendoGrid").dataSource.data();

        jQuery.each(grid_configData, function (index, config) {

            var statesValue = config.STATES_APPL || ""; // fallback to empty string if null
            var statesArray = statesValue.split(',').map(s => s.trim()).filter(s => s !== "");
            var stateInclExcl = config.STATES_INCL_EXCL_CD;
            var stateInclExclDesc = config.STATES_INCL_EXCL_DESC;

            statesArray.forEach(function (state) {

                DetailController.grid.states.data.push({
                    ATS_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                    GDLN_DTQ_SYS_SEQ: config.GDLN_DTQ_SYS_SEQ,
                    STATE_CD: state,
                    STATE_NAME: state,
                    ATS_INCL_EXCL_CD: stateInclExcl,
                    ATS_INCL_EXCL_CD_DESC: stateInclExclDesc,
                    DPOC_RELEASE: $('#txtDPOCRelease_GS').val(),
                    DPOC_VER_NUM: $('#txtDPOC_VER_NUM_GS').val(),
                });
            });
        });
    },
    bind: function () {
        $("#grid_config").on("click", ".config-delete", function () {
            DPOC_ConfigController.grid.deleteRow(this);
        });
    },
    grid: {
        refresh: function () {
            $("#grid_config").data("kendoGrid").dataSource.read();
            $("#grid_config").data("kendoGrid").refresh();
        },
        params: function () {
            return {
                p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                p_DPOC_PACKAGE: MIApp.Sanitize.string($('#ddlDPOCPackage').val()),
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease_GS').val()),
                p_IQ_GDLN_ID: MIApp.Sanitize.string($('#txtIQ_GDLN_ID_GS').val() == '' || $('#txtIQ_GDLN_ID_GS').val() == undefined ? $('#txtIQ_GDLN_ID_GS_hdn').val() : $('#txtIQ_GDLN_ID_GS').val())
            }
        },
        onDataBound: function () {            
            // Access the grid data source
            var dataSource = this.dataSource;
            var totalConfigs = dataSource.view().length;
            //debugger;
            // Iterate over the data items
            $.each(dataSource.view(), function (index, item) {
                // Get the row element using the item's unique identifier (uid)
                var row = $("#grid_config").find("[data-uid='" + item.uid + "']");

                // Assign the desired ID to the row
                if (Number.isInteger(item.GDLN_DTQ_SYS_SEQ) && item.GDLN_DTQ_SYS_SEQ > 0)
                    row.attr("id", item.GDLN_DTQ_SYS_SEQ);
                else
                    row.attr("id", "row_" + (totalConfigs - index));                               
            });

            /*
            var grid = $("#grid_config").data("kendoGrid");
            if (grid != undefined) {
                var columnIndex = grid.wrapper.find("th[data-field='STATES_INCL_EXCL_DESC']").index();
                grid.tbody.find("tr").each(function () {
                    $(this).find("td").eq(columnIndex).addClass("k-state-disabled");
                });
            }*/

            //debugger;
            var grid = this;
            var data = grid.dataSource.view();

            for (var i = 0; i < data.length; i++) {
                data[i].RowNumber = i + 1;
            }
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");

            Swal.fire({
                icon: 'question',
                title: 'Delete confirmation?',
                html: 'Are you sure, you want to delete this entry!',
                showCancelButton: true,
                confirmButtonText: 'Delete',
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_config").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                    $("#grid_config").data("kendoGrid").refresh();
                }
            });
        },
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        }
    }
}

var gsConfigHoverController = gsConfigHoverController || {};
$(function () {
    'use strict';

    gsConfigHoverController = {
        params: function () {
            return {
                p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string(p_DPOC_HIERARCHY_KEY),
                p_DPOC_VER_EFF_DT: MIApp.Sanitize.string(p_DPOC_VER_EFF_DT),
                p_DPOC_PACKAGE: MIApp.Sanitize.string(p_DPOC_PACKAGE),
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease_GS').val()),
                p_IQ_GDLN_ID: MIApp.Sanitize.string($('#txtIQ_GDLN_ID_GS').val() == '' || $('#txtIQ_GDLN_ID_GS').val() == undefined ? $('#txtIQ_GDLN_ID_GS_hdn').val() : $('#txtIQ_GDLN_ID_GS').val())
            }
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("414px");
            gridContent.height("310px");
        },
        refresh: function () {
            $("#grid_config_hover").data("kendoGrid").dataSource.read();
            $("#grid_config_hover").data("kendoGrid").refresh();
        },
    }

});


$(document).ready(function () {
    DPOC_ConfigController.bind();
});

function nonEditable() {
    return false;
}

function nonEditablePreventDetault(e) {
    
}