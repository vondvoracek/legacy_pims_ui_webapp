
var DPOC_POSController = {
    loadData: function () {
        var pos_by_gdln_id = DetailController.grid.pos.data.filter((pos) => pos.POS_IQ_GDLN_ID == $('#txtIQ_GDLN_ID_GS').val());

        if (pos_by_gdln_id.length > 0) {
            var grid_pos = $("#grid_pos").data("kendoGrid");
            grid_pos.dataSource.data(pos_by_gdln_id);
        } 
    },
    addData: function () {
        var grid_posData = $("#grid_pos").data("kendoGrid").dataSource._data;
        DetailController.grid.pos.data = DetailController.grid.pos.data.filter(d => d.POS_IQ_GDLN_ID !== $('#txtIQ_GDLN_ID_GS').val());
        jQuery.each(grid_posData, function (index, pos) {
            DetailController.grid.pos.data.push({
                POS_IQ_GDLN_ID: $('#txtIQ_GDLN_ID_GS').val(),
                PLC_OF_SVC_CD: pos.PLC_OF_SVC_CD,
                PLC_OF_SVC_DESC: pos.PLC_OF_SVC_DESC,
                INCL_EXCL_CD: pos.INCL_EXCL_CD,
                INCL_EXCL_CD_DESC: pos.INCL_EXCL_CD_DESC
            });
        });
    },
    bind: function(){
        $("#grid_pos").on("click", ".pos-delete", function () {
            DPOC_POSController.grid.deleteRow(this);
        });
    },
    grid: {
        params: function () {

            return {
                p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                p_DPOC_PACKAGE: MIApp.Sanitize.string($('#txtDPOCPackage').val()),
                p_DPOC_RELEASE: MIApp.Sanitize.string($('#txtDPOCRelease').val()),
                p_IQ_GDLN_ID: MIApp.Sanitize.string($('#txtIQ_GDLN_ID_GS').val())
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
                    $("#grid_pos").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                    $("#grid_pos").data("kendoGrid").refresh();                    
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

$(document).ready(function () {
    DPOC_POSController.bind();
});