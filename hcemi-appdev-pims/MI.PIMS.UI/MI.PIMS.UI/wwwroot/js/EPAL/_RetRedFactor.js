
var _RetRedFactorController = {
    data: {
        FACTOR_TYPE: [],
        FACTOR_NM: [],
        FACTOR_EFF_DT: [],
        FACTOR_EXP_DT: [],
        FACTOR_NOTES: [],
        init: function () {
            var _data = this;
            var retfactorDataValidationText = '';
            var isValid = true;
            var errorMessage = "";


            _data.FACTOR_TYPE = [];
            _data.FACTOR_NM = [];
            _data.FACTOR_EFF_DT = [];
            _data.FACTOR_EXP_DT = [];
            _data.FACTOR_NOTES = [];

            /* Retention */
            var retFactorData = $("#grid_Retention").data("kendoGrid").dataSource._data;

            jQuery.each(retFactorData, function (index, factorData) {

                //Added 7/11/2023 MFQ USER STORY 63342
                if ($.trim(factorData.FACTOR_EFF_DT_RET).length) {
                    _data.FACTOR_EFF_DT.push(kendo.toString(kendo.parseDate(factorData.FACTOR_EFF_DT_RET), 'yyyy-MM-dd'));
                } else {
                    retfactorDataValidationText = 'Please enter Retention Factor Effective date in Retention/Reduction section!\n'
                    isValid = false;
                }

                if ($.trim(factorData.FACTOR_EXP_DT_RET).length) {
                    _data.FACTOR_EXP_DT.push(kendo.toString(kendo.parseDate(factorData.FACTOR_EXP_DT_RET), 'yyyy-MM-dd'));
                } else {
                    _data.FACTOR_EXP_DT.push(null);
                }

                if (factorData.FACTOR_EFF_DT_RET != null && factorData.FACTOR_EXP_DT_RET != null) {
                    var factor_eff_dt = kendo.parseDate(factorData.FACTOR_EFF_DT_RET);
                    var factor_exp_dt = kendo.parseDate(factorData.FACTOR_EXP_DT_RET);

                    if (factor_eff_dt > factor_exp_dt) {
                        retfactorDataValidationText += "One of the Retention Factor effective date is greater than expiration date! Please verify and correct in Retention/Reduction section!\n";
                        isValid = false;
                    }
                }

                _data.FACTOR_TYPE.push("Retention");
                _data.FACTOR_NM.push(factorData.FACTOR_NM_RET.replace(/,/g, ''));
                _data.FACTOR_NOTES.push(String(factorData.FACTOR_NOTES_RET).replace(/,/g, ''));
            });


            /* Reduction */
            var redFactorData = $("#grid_Reduction").data("kendoGrid").dataSource._data;
            jQuery.each(redFactorData, function (index, factorData) {

                //Added 7/11/2023 MFQ USER STORY 63342
                if ($.trim(factorData.FACTOR_EFF_DT_RED).length) {
                    _data.FACTOR_EFF_DT.push(kendo.toString(kendo.parseDate(factorData.FACTOR_EFF_DT_RED), 'yyyy-MM-dd'));
                } else {
                    retfactorDataValidationText = 'Please enter Reduction Factor Effective date in Retention/Reduction section!\n'
                }


                if ($.trim(factorData.FACTOR_EXP_DT_RED).length) {
                    _data.FACTOR_EXP_DT.push(kendo.toString(kendo.parseDate(factorData.FACTOR_EXP_DT_RED), 'yyyy-MM-dd'));
                } else {
                    _data.FACTOR_EXP_DT.push(null);
                }

                if (factorData.FACTOR_EFF_DT_RED != null && factorData.FACTOR_EXP_DT_RED != null) {
                    var factor_eff_dt = kendo.parseDate(factorData.FACTOR_EFF_DT_RED);
                    var factor_exp_dt = kendo.parseDate(factorData.FACTOR_EXP_DT_RED);

                    if (factor_eff_dt > factor_exp_dt) {
                        retfactorDataValidationText += "One of the Reduction Factor effective date is greater than expiration date! Please verify and correct in Retention/Reduction section!\n";
                        isValid = false;
                    }
                }

                _data.FACTOR_TYPE.push("Reduction");
                _data.FACTOR_NM.push(factorData.FACTOR_NM_RED.replace(/,/g, ''));
                _data.FACTOR_NOTES.push(String(factorData.FACTOR_NOTES_RED).replace(/,/g, ''));
            });

            if (retfactorDataValidationText != '') {
                errorMessage += retfactorDataValidationText;
                isValid = false;
            }

            var result = {
                isValid: isValid,
                errorMessage: errorMessage
            };

            _data.FACTOR_TYPE = _data.FACTOR_TYPE.join(',');
            _data.FACTOR_NM = _data.FACTOR_NM.join(',');
            _data.FACTOR_EFF_DT = _data.FACTOR_EFF_DT.join(',');
            _data.FACTOR_EXP_DT = _data.FACTOR_EXP_DT.join(',');
            _data.FACTOR_NOTES = _data.FACTOR_NOTES.join(',');

            return result;
        }
    },

    bind: function () {
        $("#grid_Retention").on("click", ".retention-factor-delete", function () {
            _RetRedFactorController.gridRetention.deleteRow(this);
        });
        $("#grid_Reduction").on("click", ".reduction-factor-delete", function () {
            _RetRedFactorController.gridReduction.deleteRow(this);
        });
    },
    gridRetention: {
        param: function () {
            return MIApp.Sanitize.encodeObject({
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? MIApp.Sanitize.encodeProp($("#hidden-EPAL_HIERARCHY_KEY").val()) : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? MIApp.Sanitize.encodeProp($("#txtOrigEPALVersionDt").val()) : $("#txtEPALVersionDt").val(),
                p_FACTOR_TYPE: 'Retention'
            })
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
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_Retention").data("kendoGrid").removeRow(row);                    
                    DetailController.dirtyform.dirtycheck();
                }
            });
        },
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        dataSource: {
            onChange: function (e) {
                if (e.action == "itemchange") {
                    DetailController.dirtyform.dirtycheck();
                }
            }
        },
        FACTOR_DESCEditable: function () {
            return false;
        }
    },
    gridReduction: {
        param: function () {
            return MIApp.Sanitize.encodeObject({
                p_EPAL_HIERARCHY_KEY: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#hidden-EPAL_HIERARCHY_KEY") != null) ? $("#hidden-EPAL_HIERARCHY_KEY").val() : $('#txtPIMSID').val(),
                p_EPAL_VER_EFF_DT: ($("#duplicate-record") != null && $("#duplicate-record").val() == "Y" && $("#txtOrigEPALVersionDt") != null) ? $("#txtOrigEPALVersionDt").val() : $("#txtEPALVersionDt").val(),
                p_FACTOR_TYPE: 'Reduction'
            });
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
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    $("#grid_Reduction").data("kendoGrid").removeRow(row);
                    DetailController.dirtyform.dirtycheck();
                }
            });
        },
        deleteRowSilently: function (e) {
            var row = e.sender.element.closest('tr');
            var grid = row.closest(".k-grid").data("kendoGrid")
            grid.removeRow(row);
        },
        dataSource: {
            onChange: function (e) {
                if (e.action == "itemchange") {
                    DetailController.dirtyform.dirtycheck();
                }
            }
        },
        FACTOR_DESCEditable: function () {
            return false;
        }
    }
}

$(document).ready(function () {
    _RetRedFactorController.bind();
});