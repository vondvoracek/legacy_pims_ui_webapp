var GSSDTQController = {
    data: [],
    loadData: function () {
        var dtq_by_gdln_id = DetailController.grid.dtqs.data.filter((dtq) => dtq.DTQ_IQ_GDLN_ID == $('#txtIQ_GDLN_ID_GS').val());
        var grid_dtq = $("#grid_dtq").data("kendoGrid");            

        if (dtq_by_gdln_id.length > 0) {            
            grid_dtq.dataSource.data(dtq_by_gdln_id);
            grid_dtq.refresh();
        } /*else {
            //GSSDTQController.grid.refresh();
            grid_dtq.dataSource = $("#grid_dtqHidden").data("kendoGrid").dataSource;
        }*/
    },
    addData: function () {
        var grid_dtqData = $("#grid_dtq").data("kendoGrid").dataSource._data;
        DetailController.grid.dtqs.data = DetailController.grid.dtqs.data.filter(function (d) { d.DTQ_IQ_GDLN_ID !== $('#txtIQ_GDLN_ID_GS').val(); });
        jQuery.each(grid_dtqData, function (index, dtq) {
            DetailController.grid.dtqs.data.push({
                DTQ_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(), 
                DTQ_POS_APPL: dtq.DTQ_POS_APPL,
                DTQ_INCL_EXCL_CD: dtq.DTQ_INCL_EXCL_CD,
                DTQ_NM: dtq.DTQ_NM,
                DTQ_TYPE: dtq.DTQ_TYPE,
                DTQ_TYPE_DESC: dtq.DTQ_TYPE_DESC,
                DTQ_RSN: dtq.DTQ_RSN,                
                TGT_DTQ: dtq.TGT_DTQ,
                TGT_DTQ_VERSION: dtq.TGT_DTQ_VERSION,
                HOLDING_DTQ: dtq.HOLDING_DTQ,
                HOLDING_DTQ_VERSION: dtq.HOLDING_DTQ_VERSION
            });
        });
    },
    init: function () {
        $('#bs-dpoc-dtq-widget-modal').on('shown.bs.modal', function () {
            $(document).off('focusin.bs.modal');
        });

        $('#bs-dpoc-dtq-widget-modal').on('hidden.bs.modal', function () {
            $('#dtq-update-row-id').val('');            
        });

        //DTQ Update button
        $('.bs-dpoc-dtq-widget-modal-save').click(function () {
            GSSDTQController.methods.update();
        });

        $("#grid_dtq").on("click", ".dtq-delete", function () {
            GSSDTQController.grid.deleteRow(this, 'grid_dtq');
        });
        $("#grid_dtq_tgt").on("click", ".dtq-tgt-delete", function () {
            GSSDTQController.grid.deleteRow(this, 'grid_dtq_tgt');
        });
        $("#grid_dtq_holding").on("click", ".dtq-holding-delete", function () {
            GSSDTQController.grid.deleteRow(this, 'grid_dtq_holding');
        });
    },
    grid: {
        params: function () {            
            return {
                p_DPOC_HIERARCHY_KEY: $('#txtPIMSID').val(),
                p_DPOC_VER_EFF_DT: $('#txtDPOCVersionDt').val()
            }            
        },
        refresh: function () {
            $("#grid_dtq").data("kendoGrid").dataSource.read();
            $("#grid_dtq").data("kendoGrid").refresh();
        },        
        dtq: {            
            validation: {
                init: function () {
                    var validationTYPEText = '';
                    var validationRSNText = '';
                    var validationMEDText = '';
                    var validationTGTText = '';
                    var validationHOLDINGText = '';

                    var grid_dtqData = $("#grid_dtq").data("kendoGrid").dataSource._data;
                    var currentPOSadded = GSSDTQController.methods.getPOS();
                    var dtqPosMatched = true;

                    jQuery.each(grid_dtqData, function (index, dtq) {

                        if (dtq.DTQ_POS_APPL != '' && dtq.DTQ_POS_APPL != null) {
                            if (currentPOSadded.filter(p => p.PLC_OF_SVC_CD == dtq.DTQ_POS_APPL).length < 1) {                                
                                dtqPosMatched = false;
                            }
                        }

                        //if ($.trim(dtq.DTQ_NM) != '') {
                        //    if ($.trim(dtq.DTQ_TYPE) == '') {
                        //        validationTYPEText = 'Please select DTQ Type in one of the record(s) in DTQ section!\n';
                        //    }
                        //    if ($.trim(dtq.DTQ_RSN) == '') {
                        //        validationRSNText = 'Please select DTQ Reason in one of the record(s) in DTQ section!\n';
                        //    }
                        //}    
                        if ($.trim(dtq.TGT_DTQ) != '') {
                            if ($.trim(dtq.TGT_DTQ_VERSION) == '') {
                                validationTGTText = 'Please select Target Version in one of the record(s) in DTQ Target section!\n';
                            }
                        }
                        if ($.trim(dtq.HOLDING_DTQ) != '') {
                            if ($.trim(dtq.HOLDING_DTQ_VERSION) == '') {
                                validationHOLDINGText = 'Please select Holding Version in one of the record(s) in DTQ Holding section!\n';
                            }
                        }
                    });
                    if (!dtqPosMatched) {
                        validationTYPEText += 'Guideline Data cannot Save – The Place of Service (POS) selected for the DTQ is not listed on the POS screen. Either select another POS for the DTQ or add the additional POS to the POS Screen!\n';
                    }

                    return validationTYPEText + validationRSNText /* + validationMEDText */ + validationTGTText + validationHOLDINGText;
                }
            },
            onEdit: function (arg) {
                //arg.container.find("input[name='MED_PLCY_REF_CODE']").attr('maxlength', '4');
                arg.container.find("input[name='DTQ_NM']").attr('maxlength', '100');
                arg.container.find("input[name='TGT_DTQ']").attr('maxlength', '100');
                arg.container.find("input[name='TGT_DTQ_VERSION']").attr('maxlength', '22');
                arg.container.find("input[name='HOLDING_DTQ']").attr('maxlength', '100');
                arg.container.find("input[name='HOLDING_DTQ_VERSION']").attr('maxlength', '22');
            }
        },
        deleteRow: function (delete_button, grid_name) {
            var row = $(delete_button).closest("tr");

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
                    $("#" + grid_name).data("kendoGrid").removeRow(row);
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
    multiSelect: {
        param: {
            DPOC_DTQ_TYPE: function () {
                return {
                    p_VV_SET_NAME: "DPOC_DTQ_TYPE",
                    p_BUS_SEG_CD: "All"
                }
            },
            DPOC_DTQ_RSN: function () {
                return {
                    p_VV_SET_NAME: "DPOC_DTQ_RSN",
                    p_BUS_SEG_CD: "All"
                }
            }
        }
    },
    methods: {
        update: function () {

            if ($('#dtq-update-row-id').val().length == 0) return;

            var row = $("#grid_dtq").data("kendoGrid").tbody.find("tr[data-uid='" + $('#dtq-update-row-id').val() + "']");
            var dataItem = $("#grid_dtq").data("kendoGrid").dataItem($(row).closest("tr")); 

            dataItem.DTQ_NM = $('#DPOCDTQName').val();
            dataItem.DTQ_TYPE = $('#ddlDTQType').data("kendoMultiSelect").value();
            dataItem.DTQ_RSN = $('#ddlDTQReason').data("kendoMultiSelect").value();
            dataItem.REF_CD = $('#REF_CD').val();
            dataItem.TGT_DTQ = $('#TargetDTQ').val();
            dataItem.TGT_DTQ_VERSION = $('#TargetDTQVersion').val();
            dataItem.HOLDING_DTQ = $('#HoldingDTQ').val();
            dataItem.HOLDING_DTQ_VERSION = $('#HoldingDTQVersion').val();

            $("#grid_dtq").data("kendoGrid").refresh();
            $('#bs-dpoc-dtq-widget-modal').modal('hide');
        },
        editDTQ: function (row) {
            var grid = $("#grid_dtq").data("kendoGrid");
            var dataItem = grid.dataItem($(row).closest("tr")); 

            if (dataItem != null && dataItem != undefined) {

                $('#dtq-update-row-id').val(dataItem.uid);

                $('#ddlDTQType').data("kendoMultiSelect").value(dataItem.DTQ_TYPE);
                $('#ddlDTQReason').data("kendoMultiSelect").value(dataItem.DTQ_RSN);
                $('#REF_CD').val(dataItem.REF_CD);
                $('#TargetDTQ').val(dataItem.TGT_DTQ);
                $('#TargetDTQVersion').val(dataItem.TGT_DTQ_VERSION);
                $('#HoldingDTQ').val(dataItem.HOLDING_DTQ);
                $('#HoldingDTQVersion').val(dataItem.HOLDING_DTQ_VERSION);

                $('#bs-dpoc-dtq-widget-modal').modal('show');
            }
        },
        getPOS: function () {
            var posAdded = [];
            var grid_config_data = $("#grid_config").data("kendoGrid").dataSource._data;
            jQuery.each(grid_config_data, function (index, config) {
                posAdded.push(config.PLC_OF_SVC_CD);
            });
            return posAdded;
        },
    },
    validation: function () {

        var dtq_validation = GSSDTQController.grid.dtq.validation.init();

        //var dtq_tgt_validation = GSSDTQController.grid.dtq_tgt.validation.init();

        //var dtq_holding_validation = GSSDTQController.grid.dtq_holding.validation.init();

        return dtq_validation; // + ' ' + dtq_tgt_validation + ' ' + dtq_holding_validation;
    }
}

$(document).ready(function () {
    GSSDTQController.init();
});