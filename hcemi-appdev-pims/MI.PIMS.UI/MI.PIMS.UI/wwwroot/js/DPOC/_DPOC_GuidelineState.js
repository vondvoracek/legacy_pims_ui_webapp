
var StateInfoController = {        
    loadData: function (isHoverWindow, stateCds, inclExclCd, inclExclCdDesc) {    
        var states_by_gdln_id = [];
        if (isHoverWindow == true) { // if called for hover window then ignore sys_seq number
            var filteredStates = DetailController.grid.states.data.filter((state) =>
                state.ATS_IQ_GDLN_ID == $('#txtIQ_GDLN_ID_GS_hdn').val() &&
                state.DPOC_RELEASE == $('#txtDPOCRelease_GS_hdn').val() &&
                state.DPOC_VER_NUM == $('#txtDPOC_VER_NUM_hdn').val()
            );

            $.each(filteredStates, function (index, state) {
                var stateCodes = state.STATE_CD ? state.STATE_CD.split('|') : [];

                $.each(stateCodes, function (i, stateCd) {
                    var stateName = getStateName(stateCd); // Assumes you have this function defined

                    var newState = {
                        ATS_IQ_GDLN_ID: state.ATS_IQ_GDLN_ID,
                        GDLN_DTQ_SYS_SEQ: state.GDLN_DTQ_SYS_SEQ,
                        STATE_CD: stateCd,
                        STATE_NAME: stateName,
                        ATS_INCL_EXCL_CD: state.ATS_INCL_EXCL_CD,
                        ATS_INCL_EXCL_CD_DESC: state.ATS_INCL_EXCL_CD_DESC,
                        DPOC_RELEASE: state.DPOC_RELEASE,
                        DPOC_VER_NUM: state.DPOC_VER_NUM
                    };

                    states_by_gdln_id.push(newState);
                });
            });

        } else { //THIS SECTION WILL BE USING ONLY TO LOAD STATE POPUP THROUGH CONFIGURATION            
            if (stateCds != undefined && stateCds != '') {
                var stateCdArray = null;
                if (stateCds.indexOf('|') > -1) {
                    stateCdArray = stateCds.split('|');
                } else {
                    stateCdArray = stateCds.split(',');
                }
                $.each(stateCdArray, function (index, stateCd) {
                    var obj = {
                        ATS_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                        GDLN_DTQ_SYS_SEQ: $('#txtGDLN_DTQ_SYS_SEQ').val(),
                        STATE_CD: stateCd,
                        STATE_NAME: getStateName(stateCd),
                        ATS_INCL_EXCL_CD: inclExclCd,
                        ATS_INCL_EXCL_CD_DESC: inclExclCdDesc,
                        DPOC_RELEASE: $('#txtDPOCRelease_GS').val(),
                        DPOC_VER_NUM: $('#txtDPOC_VER_NUM_GS').val()
                    };

                    states_by_gdln_id.push(obj);
                });
            }
        }                    

        if (states_by_gdln_id.length > 0) {
            var grid = $("#" + (isHoverWindow == true ? 'grid_GSStateInfo_Hover' : 'grid_GSStateInfo')).data("kendoGrid");                        
            if (grid)
                grid.dataSource.data(states_by_gdln_id);
        }
    },
    addData: function (inclExcl) {        
        var grid_gssStateData = $("#grid_GSStateInfo").data("kendoGrid").dataSource._data;

        const iqGuidelineId = $('#txtIQ_GDLN_ID_GS').val();
        const dtqSysSeq = parseInt($('#txtGDLN_DTQ_SYS_SEQ').val());
        const dpocVerNum = $('#txtDPOC_VER_NUM_GS').val();
        const dpocRelease = $('#txtDPOCRelease_GS').val();

        // Check if a matching record exists
        DetailController.grid.states.data = $.grep(DetailController.grid.states.data, function (d) {
            return !(
                d.ATS_IQ_GDLN_ID === iqGuidelineId &&
                d.GDLN_DTQ_SYS_SEQ === dtqSysSeq &&
                d.DPOC_VER_NUM === dpocVerNum &&
                d.DPOC_RELEASE === dpocRelease
            );
        });

        jQuery.each(grid_gssStateData, function (index, state) {
            DetailController.grid.states.data.push({
                ATS_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                GDLN_DTQ_SYS_SEQ: $('#txtGDLN_DTQ_SYS_SEQ').val(),
                STATE_CD: state.STATE_CD,
                STATE_NAME: state.STATE_NAME,
                ATS_INCL_EXCL_CD: inclExcl != null && state.STATE_CD != null ? inclExcl : state.ATS_INCL_EXCL_CD,
                ATS_INCL_EXCL_CD_DESC: inclExcl != null && state.STATE_CD != null ? inclExcl : state.ATS_INCL_EXCL_CD_DESC,
                DPOC_RELEASE: $('#txtDPOCRelease_GS').val(),
                DPOC_VER_NUM: $('#txtDPOC_VER_NUM_GS').val(),
            });
        });
        /*
        DetailController.grid.states.data = DetailController.grid.states.data.filter(d =>
            d.ATS_IQ_GDLN_ID != $('#txtIQ_GDLN_ID_GS').val() &&
            d.GDLN_DTQ_SYS_SEQ != $('#txtGDLN_DTQ_SYS_SEQ').val() &&
            d.DPOC_VER_NUM != $('#txtDPOC_VER_NUM_GS').val() &&
            d.DPOC_RELEASE != $('#txtDPOCRelease_GS').val() 
            //state.dpoc_ver_num != $('#txtDPOC_VER_NUM_GS').val()
        );
        jQuery.each(grid_gssStateData, function (index, state) {
            DetailController.grid.states.data.push({
                ATS_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                GDLN_DTQ_SYS_SEQ: $('#txtGDLN_DTQ_SYS_SEQ').val(),
                STATE_CD: state.STATE_CD,
                STATE_NAME: state.STATE_NAME,
                ATS_INCL_EXCL_CD: inclExcl != null && state.STATE_CD != null ? inclExcl : state.ATS_INCL_EXCL_CD,
                ATS_INCL_EXCL_CD_DESC: inclExcl != null && state.STATE_CD != null ? inclExcl : state.ATS_INCL_EXCL_CD_DESC,
                DPOC_RELEASE: $('#txtDPOCRelease_GS').val(),
                DPOC_VER_NUM: $('#txtDPOC_VER_NUM_GS').val(),
            });
        });*/

    },    
    bind: function () {
        $("#grid_GSStateInfo").on("click", ".gsstate-info-delete", function () {
            StateInfoController.grid.deleteRow(this);
        });                
    },
    update: function () {
        var guidelineStatesValidation = DetailController.grid.states.validation('grid_GSStateInfo', true); //'grid_GSStateInfoHidden'
        if (guidelineStatesValidation.length > 0) {
            DetailController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + guidelineStatesValidation.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>");
            return;
        } else {
            if (guidelineStatesValidation == '') {
                //StateInfoController.addData($('#ddlStateInclExcl').val());  
                //debugger;
                DPOC_ConfigController.updateStatesOnConfig($('#txtGDLN_DTQ_SYS_SEQ').val(), getSelectedStates(), $('#ddlStateInclExcl').data('kendoDropDownList').value(), $('#ddlStateInclExcl').data('kendoDropDownList').text(), $('#txtGDLN_ATS_RowNumber').val());
                $('#ddlStateInclExcl').data('kendoDropDownList').value(null);
                gsStateWindow.close();
            }
        }

        function getSelectedStates() {
            var data = $("#grid_GSStateInfo").data("kendoGrid").dataSource.view(); // Get the current view of the data source
            var state_names = data.map(function (item) {
                return item['STATE_CD'];
            });
            return state_names.join(",");
        }
    },
    grid: {        
        refresh: function () {
            $("#grid_GSStateInfo").data("kendoGrid").dataSource.read();
            $("#grid_GSStateInfo").data("kendoGrid").refresh();
        },
        empty: function () {
            $("#grid_GSStateInfo").data("kendoGrid").dataSource.data([]);
        },
        param: function () {
            var paramObj = {
                p_DPOC_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtPIMSID") != null) ? $("#txtPIMSID").val() : $('#txtDPOCID').val(),
                p_DPOC_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtDPOCVersionDt") != null) ? $("#txtDPOCVersionDt").val() : $("#txtDPOCVersionDt").val(),                
                p_DPOC_PACKAGE: MIApp.Sanitize.string($("#ddlDPOCPackage").val()),
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease_GS').val() == '' || $('#txtDPOCRelease_GS').val() == undefined ? $('#txtDPOCRelease_GS_hdn').val() : $('#txtDPOCRelease_GS').val()), 
                p_IQ_GDLN_ID: MIApp.Sanitize.string($('#txtIQ_GDLN_ID_GS').val() == '' || $('#txtIQ_GDLN_ID_GS').val() == undefined ? $('#txtIQ_GDLN_ID_GS_hdn').val() : $('#txtIQ_GDLN_ID_GS').val()),
                p_GDLN_DTQ_SYS_SEQ: MIApp.Sanitize.string($("#txtGDLN_DTQ_SYS_SEQ").val())
            }
            return paramObj;
        },
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
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
                    $("#grid_GSStateInfo").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                    $("#grid_GSStateInfo").data("kendoGrid").refresh();
                }
            });
        }, 
        onDataBound: function (e) {
            var dataS = $("#grid_GSStateInfo").data("kendoGrid").dataSource.data();
            //debugger;
            if (dataS.length && dataS[0].STATE_CD !== '' && dataS[0].ATS_INCL_EXCL_CD !== '')
                $('#ddlStateInclExcl').data("kendoDropDownList").value(dataS[0].ATS_INCL_EXCL_CD?.toUpperCase());
        }
    },
    openStatePopup: function (e) {
        //debugger;
        var grid = $("#grid_config").data("kendoGrid");
        var dataItem = grid.dataItem($(e).closest("tr"));

        if (dataItem.GDLN_DTQ_SYS_SEQ != null && dataItem.GDLN_DTQ_SYS_SEQ != 0) {
            //$(e).closest("tr").attr('id', dataItem.GDLN_DTQ_SYS_SEQ);
            $('#txtGDLN_DTQ_SYS_SEQ').val(dataItem.GDLN_DTQ_SYS_SEQ);
        } else {
            $('#txtGDLN_DTQ_SYS_SEQ').val($(e).closest("tr").attr('id'));
        }
        
        $('#txtGDLN_ATS_RowNumber').val(dataItem.RowNumber);

        //data: { isHoverWindow: false, stateCds: dataItem.STATES_APPL, inclExclCd: dataItem.STATES_INCL_EXCL_CD, inclExclCdDesc: dataItem.STATES_INCL_EXCL_DESC }
        gsStateWindow.setOptions({
            stateParam: { isHoverWindow: false, stateCds: dataItem.STATES_APPL, inclExclCd: dataItem.STATES_INCL_EXCL_CD, inclExclCdDesc: dataItem.STATES_INCL_EXCL_DESC }
        });

        gsStateWindow.refresh({
            url: statesViewLoadWindowUrl            
        }).open();

        /*
        setTimeout(function () {
            if (DetailController.grid.states.data.length > 0) {
                //stateCds, stateNames, inclExclCd, inclExclCdDesc
                StateInfoController.loadData(false, dataItem.STATES_APPL, dataItem.STATES_INCL_EXCL_CD, dataItem.STATES_INCL_EXCL_DESC);
            } else {
                //after assigning sys_seq now get data into the state grid
                StateInfoController.grid.refresh();
            }
        }, 200)
        */
      
    }
}

var gsStatesHoverController = gsStatesHoverController || {};
$(function () {
    'use strict';

    gsStatesHoverController = {
        params: function () {
            return {
                p_DPOC_HIERARCHY_KEY: p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT: p_DPOC_VER_EFF_DT,
                p_DPOC_PACKAGE: p_DPOC_PACKAGE,
                p_DPOC_RELEASE: $('#txtDPOCRelease_GS').val() == '' ? MIApp.Sanitize.string($('#txtDPOCRelease_GS_hdn').val()) : MIApp.Sanitize.string($('#txtDPOCRelease_GS').val()),
                p_DPOC_VER_NUM: $('#txtDPOC_VER_NUM_GS').val() == '' ? MIApp.Sanitize.string($('#txtDPOC_VER_NUM_hdn').val()) : MIApp.Sanitize.string($('#txtDPOC_VER_NUM_GS').val()),
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
            $("#grid_GSStateInfo_Hover").data("kendoGrid").dataSource.read();
            $("#grid_GSStateInfo_Hover").data("kendoGrid").refresh();
        },
    }

});

$(document).ready(function () {
    StateInfoController.bind();
});
