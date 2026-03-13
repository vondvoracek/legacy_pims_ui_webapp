var gridDiagDataCache = {};

var DiagCodesController = {    
    loadData: function (isHover) {        
        var gridId = isHover === true ? 'grid_GSDiagList_hover' : 'grid_GSDiagList';
        var grid = $("#" + gridId).data("kendoGrid");

        var key = DPOC_GuidelineSummaryController.getGridCacheKey();

        var diagCodes_by_gdln_id = DetailController.grid.diags.data.filter((dtq) =>
            dtq.DIAG_IQ_GDLN_ID == ($('#txtIQ_GDLN_ID_GS').val() || $('#txtIQ_GDLN_ID_GS_hdn').val()) &&
            dtq.DPOC_RELEASE == ($('#txtDPOCRelease_GS').val() || $('#txtDPOCRelease_GS_hdn').val()) &&
            dtq.DPOC_VER_NUM == ($('#txtDPOC_VER_NUM_GS').val() || $('#txtDPOC_VER_NUM_hdn').val())
        );

        if (grid !== undefined) {
            if (gridDiagDataCache[key] && gridDiagDataCache[key].length > 0) {
                grid.dataSource.data(gridDiagDataCache[key]);
            } else if (diagCodes_by_gdln_id.length > 0) {
                grid.dataSource.data(diagCodes_by_gdln_id);
                gridDiagDataCache[key] = diagCodes_by_gdln_id;
            } else {
                grid.dataSource.data([]); // fallback to empty
            }
        }

        //resizeDiagGridInWindow();
        DiagCodesController.bind();
    },
    addData: function () {
        const grid_gsDiagListData = $("#grid_GSDiagList").data("kendoGrid").dataSource.data();
        const guidelineId = $('#txtIQ_GDLN_ID_GS').val();
        const dpocVerNum = $('#txtDPOC_VER_NUM_GS').val();
        const dpocVerNumData = $('#txtDPOC_VER_NUM_GS').data('version');
        const dpocRelease = $('#txtDPOCRelease_GS').val();
        const dpocReleaseData = $('#txtDPOCRelease_GS').data('release');

        // Remove entries only if textbox values differ from data attributes
        if (dpocVerNum !== dpocVerNumData || dpocRelease !== dpocReleaseData) {
            DetailController.grid.diags.data = DetailController.grid.diags.data.filter(d =>
                !(d.DIAG_IQ_GDLN_ID === guidelineId &&
                    d.DPOC_VER_NUM === dpocVerNumData &&
                    d.DPOC_RELEASE === dpocReleaseData)
            );
        } else {
            // Remove entries that are no longer in the grid
            DetailController.grid.diags.data = DetailController.grid.diags.data.filter(d => {
                if (d.DIAG_IQ_GDLN_ID === guidelineId &&
                    d.DPOC_VER_NUM === dpocVerNum &&
                    d.DPOC_RELEASE === dpocRelease) {
                    // Check if this entry exists in grid_configData
                    const existsInGrid = grid_gsDiagListData.some(dtq =>
                        (d.GDLN_DTQ_SYS_SEQ && d.GDLN_DTQ_SYS_SEQ === dtq.GDLN_DTQ_SYS_SEQ) ||
                        (d.tempId && d.tempId === dtq.tempId)
                    );
                    return existsInGrid; // Keep only if still in grid
                }
                return true; // Keep entries from other guidelines/versions/releases
            });
        }

        grid_gsDiagListData.forEach(function (diag) {
            const existingIndex = DetailController.grid.diags.data.findIndex(d =>
                d.DIAG_IQ_GDLN_ID === guidelineId &&
                d.DPOC_VER_NUM === dpocVerNum &&
                d.DPOC_RELEASE === dpocRelease &&
                d.DIAG_CD === diag.DIAG_CD
            );

            const newEntry = {
                DIAG_IQ_GDLN_ID: guidelineId,
                DIAG_CD: diag.DIAG_CD,
                DIAG_INCL_EXCL_CD: diag.DIAG_INCL_EXCL_CD,
                DIAG_INCL_EXCL_CD_DESC: diag.DIAG_INCL_EXCL_CD_DESC,
                LIST_NAME: diag.LIST_NAME,
                DPOC_RELEASE: dpocRelease,
                DPOC_VER_NUM: dpocVerNum,
                LIST_NAME_CODE: !diag.LIST_NAME ? null : diag.LIST_NAME.replace(/\s+/g, "_").replace(/-/g, "_")
            };

            if (existingIndex !== -1) {
                // Update existing entry
                DetailController.grid.diags.data[existingIndex] = newEntry;
            } else {
                // Add new entry
                DetailController.grid.diags.data.push(newEntry);
            }
        });
    },
    bind: function () {
        $("#grid_GSDiagList").on("click", ".diagnosis-codes-delete", function () {
            DiagCodesController.grid.deleteRow(this);
        });
    },
    grid: {
        refresh: function () {
            $("#grid_GSDiagList").data("kendoGrid").dataSource.read();
            $("#grid_GSDiagList").data("kendoGrid").refresh();
        },
        isDiagCDNotEditable: function (e) {
            var grid_gsDiagListData = $("#grid_GSDiagList").data("kendoGrid").dataSource._data;
            var editable = true;
            jQuery.each(grid_gsDiagListData, function (index, diagCd) {
                if (diagCd.LIST_NAME != '' && diagCd.LIST_NAME != null) {
                    editable = false;
                    return;
                }
            });
            return editable;
        },
        isDiagListNotEditable: function (e) {
            var grid_gsDiagListData = $("#grid_GSDiagList").data("kendoGrid").dataSource._data;
            var editable = true;
            jQuery.each(grid_gsDiagListData, function (index, diag) {
                if (diag.DIAG_CD != '' && diag.DIAG_CD != null) {
                    editable = false;
                    return;
                }
            });
            return editable;
        },
        param: function (e) {            
            var paramObj = {
                p_DPOC_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtPIMSID") != null) ? $("#txtPIMSID").val() : $('#txtDPOCID').val(),
                p_DPOC_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtDPOCVersionDt") != null) ? $("#txtDPOCVersionDt").val() : $("#txtDPOCVersionDt").val(),
                p_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                p_DPOC_PACKAGE: MIApp.Sanitize.string($("#ddlDPOCPackage").val()),
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease_GS').val()),
                p_DPOC_VER_NUM: MIApp.Sanitize.string($('#txtDPOC_VER_NUM_GS').val())
            }
            return paramObj;
        },
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            //var grid = row.closest(".k-grid").data("kendoGrid")
            $("#grid_GSDiagList").data("kendoGrid").removeRow(row);
        },
        deleteRow: function (deleteButton) {
            var row = $(deleteButton).closest("tr");
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
                    $("#grid_GSDiagList").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                    $("#grid_GSDiagList").data("kendoGrid").refresh();
                }
            });

        },
        onDataBound: function (e) {
            
            $("#grid_GSDiagList > .k-grid-toolbar > .k-grid-add").off('click').on("click", function (e) {      
                e.preventDefault();
                e.stopPropagation();

                $grid = $("#grid_GSDiagList").data("kendoGrid");                
                
                var dData = $grid.dataSource.data();
                if (dData.length >= 1 && !checkIfEmptyRowExist()) {
                    if (dData[0].LIST_NAME == null || dData[0].LIST_NAME == '') {
                        $grid.addRow();
                        isAddingDiagRow = true;
                    } else {
                        $grid.cancelRow();
                    }
                } else if (checkIfEmptyRowExist()) {
                    $grid.cancelRow();
                }
                else {
                    $grid.addRow();
                }
            });

            var grid = this;
            grid.tbody.find("tr").each(function () {
                var dataItem = grid.dataItem(this);

                // Check if it's a new row (no ID or DIAG_CD is empty/null)
                var isNew = dataItem.isNew && dataItem.isNew();
                var hasDiagCD = dataItem.DIAG_CD && dataItem.DIAG_CD.trim() !== "" && dataItem.DIAG_CD != undefined;

                // If it's a new row or has DIAG_CD, hide the hierarchy arrow
                if (hasDiagCD || dataItem.LIST_NAME_CODE?.length == 0) {
                    $(this).find(".k-hierarchy-cell .k-icon").hide();
                } else {
                    $(this).find(".k-hierarchy-cell .k-icon").show();
                }
            });


            //if (this.dataSource.data().length > 0) {
            //    var _d = this.dataSource.data();
            //    if (_d[0].LIST_NAME_CODE == null || _d[0].LIST_NAME_CODE == '') {
            //        $.each($(this.element.find('tr.k-master-row')) , function (index, row) {
            //            $(row).find('.k-hierarchy-cell a').css({ opacity: 0.0, cursor: 'default' }).click(function (e) {
            //                e.preventDefault(); return false;
            //            });
            //        });
            //    }                
            //}

            function checkIfEmptyRowExist() {
                grid = $("#grid_GSDiagList").data("kendoGrid");  
                var emptryGrid = false;
                
                jQuery.each(grid.dataSource.data(), function (index, diagRec) {
                    if ((diagRec.LIST_NAME == '' || diagRec.LIST_NAME == null) &&
                        (diagRec.DIAG_CD == '' || diagRec.DIAG_CD == null)) {
                        emptryGrid = true;
                        }
                });
                return emptryGrid;
            }
        },
        onDetailInit: function (e) {
            //var detailRow = e.detailRow;

            //// Render the child grid inside the detail row
            //detailRow.find("[id^='grid_GSDiagCodes_']").each(function () {
            //    var grid = $(this).data("kendoGrid");
            //    if (grid) {
            //        grid.dataSource.read(); // Trigger the read
            //    }
            //});
        }
    }
}

var gsDiagHoverController = gsDiagHoverController || {};
$(function () {
    'use strict';

    gsDiagHoverController = {
        params: function () {
            return {
                p_DPOC_HIERARCHY_KEY: p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT: p_DPOC_VER_EFF_DT,
                p_DPOC_PACKAGE: p_DPOC_PACKAGE,
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease_GS').val()),
                p_IQ_GDLN_ID: MIApp.Sanitize.string($('#hv-iq-gdln-id').val())
            }
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("414px");
            gridContent.height("310px");
        },
        refresh: function () {
            $("#grid_GSDiagList_hover").data("kendoGrid").dataSource.read();
            $("#grid_GSDiagList_hover").data("kendoGrid").refresh();
        },
    }

});

$(document).ready(function () {
    DiagCodesController.bind();
});
