
var SearchController = {
    init: function () {
        SearchController.bind();
    },
    bind: function () {

        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })


 
        $("#btnReset").click(function () {
            $('#searchForm').trigger("reset");            
            SearchController.filters.clearLocalStorage();
            $("#grid_Search").data("kendoGrid").dataSource.data([]);
            if ($("#txtIncludeHistorical").val() == 0) {
                $("txtIncludeHistorical").val('1');
                var switchInstance = $("#switch").data("kendoSwitch");
                switchInstance.check(true);
            }
            location.reload();
        });

        $("#btnSearch").click(function (e) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.PayCodeSearch")) {
                const logProperties = new LogInfo(e.target.id, MS_ID, "Pay Code Search Button click");
                appInsightsClient.trackEvent('Client.PayCodeSearch.Search', logProperties);
            }

            window.kendo.ui.progress($("#grid_Search"), true);
            SearchController.filters.setLocalStorage();
            SearchController.grid.refreshGrid();
        });

      
        SearchController.filters.getLocalStorage();

        SearchController.switch.defaultChecked();

    },
    multiSelect: {
        loadMultiSelect: function () {
            SearchController.multiSelect.setMultiSelect("KLPCS");
            SearchController.multiSelect.setMultiSelect("NDBPCS");
            SearchController.multiSelect.setMultiSelect("iCES");
            SearchController.multiSelect.setMultiSelect("LOB");
            SearchController.multiSelect.setMultiSelect("Entity");
            SearchController.multiSelect.setMultiSelect("Plan");
            SearchController.multiSelect.setMultiSelect("Product");
            //SearchController.multiSelect.setMultiSelectDS("AlternateCategory");
            //SearchController.multiSelect.setMultiSelectDS("AlternateSubCategory");
        },
        refreshMultiSelect: function () {
            $("#msProduct").data('kendoMultiSelect').dataSource.read();
            $("#msPlan").data('kendoMultiSelect').dataSource.read();
            $("#msProduct").data('kendoMultiSelect').dataSource.read();
            $("#msEntity").data('kendoMultiSelect').dataSource.read();
        },
        onChange_ClinicaliCESAction: function () {
            $("#txtClinicaliCESAction").val($("#msClinicaliCESAction").val());
        },
        onChange_ClinicaliCESName: function () {
            $("#txtClinicaliCESName").val($("#msClinicaliCESName").val());
        },
        onChange_PRODUCT: function () {
            $("#txtProduct").val($("#msProduct").val());
        },
        onChange_LOB: function () {
            $("#txtLOB").val($("#msLOB").val());
            SearchController.multiSelect.refreshMultiSelect();   
        },
        onChange_PLAN: function () {
            $("#txtPlan").val($("#msPlan").val());
        },
        onChange_ENTITY: function () {
            $("#txtLOB").val($("#msLOB").val());
            $("#txtEntity").val($("#msEntity").val());
            SearchController.multiSelect.refreshMultiSelect();   
        },
        onChange_KLPCS: function () {
            $("#txtKLPCS").val($("#msKLPCS").val());
        },
        onChange_NDBPCS: function () {
            $("#txtNDBPCS").val($("#msNDBPCS").val());
        },
        onChange_iCES: function () {
            $("#txtiCES").val($("#msiCES").val());
        },
        onChange_AlternateCategory: function () {
            $("#txtAlternateCategory").val($("#msAlternateCategory").val());
        },
        onChange_AlternateSubCategory: function () {
            $("#txtAlternateSubCategory").val($("#msAlternateSubCategory").val());
        },
        onFiltering_AlternateCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtAlternateCategoryFilterText').val(filterText);
            }
        },
        onFiltering_AlternateSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtAlternateSubCategoryFilterText').val(filterText);
            }
        },
        param: {

            APP_PRODUCT: function () {
                return {
                    COLUMN_NAME: "EPAL_PRODUCT_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val(),
                    EPAL_ENTITY_CD: $("#txtEntity").val()
                    //EPAL_BUS_SEG_CD: ""
                   
                }
            },
            APP_LOB: function () {
                return {
                    COLUMN_NAME: "EPAL_BUS_SEG_CD"
                    //EPAL_BUS_SEG_CD: $("#txtLOB").val()
                    //EPAL_BUS_SEG_CD: ""
                }
            },
            APP_PLAN: function () {
                return {
                    COLUMN_NAME: "EPAL_PLAN_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val(),
                    EPAL_ENTITY_CD: $("#txtEntity").val(),
                }
            },
            APP_ENTITY: function () {
                return {
                    COLUMN_NAME: "EPAL_ENTITY_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val(),
                    EPAL_ENTITY_CD: $("#txtEntity").val(),
                }
            },
            KLPCS: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_KL_PCS'
                }
            },
            NDBPCS: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_NDB_PCS'
                }
            },
            iCES: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_EDIT_ACTION'
                }
            },
            ClinicaliCESName: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_NAME'
                }
            },
            ClinicaliCESAction: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_ICES_EDIT_ACTION'
                }
            },
            AlternateCategory: function () {
                return {
                    text: $("#txtAlternateCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_CATEGORY",
                    P_PARENT_CATEGORY: ''
                }
            },
            AlternateSubCategory: function () {
                return {
                    text: $("#txtAlternateSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_SUB_CATEGORY",
                    P_PARENT_CATEGORY: $('#txtAlternateCategory').val()
                }
            },
            PayCodeStatus: function () {
                return {
                    p_VV_SET_NAME: 'PAYC_STATUS'
                }
            }
        },

        setMultiSelect: function (name) {
            var multiselect = $("#ms" + name).data("kendoMultiSelect");
            var values = $("#txt" + name).val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);   
        },
        setMultiSelectDS: function (name) {
            var multiselect = $("#ms" + name).data("kendoMultiSelect");
            var values = $("#txt" + name).val()
            var splitValues = values.split(",");
            var sc = []
            $.each(splitValues, function (i, item) {
                if ($.trim(item).length > 0)
                    sc.push({ "CATEGORY": item });
            });
            if (sc.length > 0) {
                multiselect.dataSource.data(sc);
                multiselect.value(splitValues);
            }
        }
    },
    filters: {
        setLocalStorage: function () {   
            var data = SearchController.grid.param()
            localStorage.setItem('payCode_Search_p_PAYC_HIERARCHY_KEY', data.p_PAYC_HIERARCHY_KEY);
            localStorage.setItem('payCode_Search_p_PAYC_STATUS', data.p_PAYC_STATUS);
            localStorage.setItem('payCode_Search_p_PAYC_NDB_PCS', data.p_PAYC_NDB_PCS);
            localStorage.setItem('payCode_Search_p_PAYC_KL_PCS', data.p_PAYC_KL_PCS);

            localStorage.setItem('payCode_Search_p_PROC_CD', data.p_PROC_CD);

            //DY - 09/06/2023 - Commented Alt Category Filters, per Lisa Cornish Request. 
            //localStorage.setItem('payCode_Search_p_ALTRNT_SVC_CAT', data.p_ALTRNT_SVC_CAT);
            //localStorage.setItem('payCode_Search_p_ALTRNT_SVC_SUBCAT', data.p_ALTRNT_SVC_SUBCAT);

            localStorage.setItem('payCode_Search_p_iCES', data.p_iCES);
            localStorage.setItem('payCode_Search_p_PRIOR_AUTH_STATUS', data.p_PRIOR_AUTH_STATUS);

            localStorage.setItem('payCode_Search_p_CURRENT_EFF_DT', data.p_CURRENT_EFF_DT);
            localStorage.setItem('payCode_Search_p_CURRENT_EXP_DT', data.p_CURRENT_EXP_DT);
            localStorage.setItem('payCode_Search_p_PAYC_VER_EFF_DT', data.p_PAYC_VER_EFF_DT);            
            localStorage.setItem('payCode_Search_p_PAYC_BUS_SEG_CD', data.p_PAYC_BUS_SEG_CD);
            localStorage.setItem('payCode_Search_p_PAYC_ENTITY_CD', data.p_PAYC_ENTITY_CD);
            localStorage.setItem('payCode_Search_p_PAYC_PRODUCT_CD', data.p_PAYC_PRODUCT_CD);
            localStorage.setItem('payCode_Search_p_PAYC_PLAN_CD', data.p_PAYC_PLAN_CD);

            localStorage.setItem('payCode_Search_p_ClinicaliCESAction', data.p_PAYC_ICES_EDIT_ACTION);
            localStorage.setItem('payCode_Search_p_ClinicaliCESName', data.p_PAYC_ICES_EDIT_NAME);

            localStorage.setItem('payCode_Search_p_INCLUDE_HISTORICAL', data.p_INCLUDE_HISTORICAL); 
        },
        getLocalStorage: function () {
            $("#txtPayCodeID").val(localStorage.getItem('payCode_Search_p_PAYC_HIERARCHY_KEY'));
            $("#ddlPayCodeStatus").val(localStorage.getItem('payCode_Search_p_PAYC_STATUS'));
            $("#txtNDBPCS").val(localStorage.getItem('payCode_Search_p_PAYC_NDB_PCS'));
            $("#txtKLPCS").val(localStorage.getItem('payCode_Search_p_PAYC_KL_PCS'));

            $("#txtProcedureCode").val(localStorage.getItem('payCode_Search_p_PROC_CD'));

            //DY - 09/06/2023 - Commented Alt Category Filters, per Lisa Cornish Request.
            //$("#txtAlternateCategory").val(localStorage.getItem('payCode_Search_p_ALTRNT_SVC_CAT'));
            //$("#txtAlternateSubCategory").val(localStorage.getItem('payCode_Search_p_ALTRNT_SVC_SUBCAT'));

            $("#txtiCES").val(localStorage.getItem('payCode_Search_p_iCES'));
            $("#ddlPriorAuthStatus").val(localStorage.getItem('payCode_Search_p_PRIOR_AUTH_STATUS'));

            $("#dpCurrentEffectiveDate").val(localStorage.getItem('payCode_Search_p_CURRENT_EFF_DT'));
            $("#dpCurrentExpirationDate").val(localStorage.getItem('payCode_Search_p_CURRENT_EXP_DT'));
            $("#dpPayCodeVerEffDate").val(localStorage.getItem('payCode_Search_p_PAYC_VER_EFF_DT'));
            $("#txtLOB").val(localStorage.getItem('payCode_Search_p_PAYC_BUS_SEG_CD'));
            $("#txtEntity").val(localStorage.getItem('payCode_Search_p_PAYC_ENTITY_CD'));
            $("#txtProduct").val(localStorage.getItem('payCode_Search_p_PAYC_PRODUCT_CD'));
            $("#txtPlan").val(localStorage.getItem('payCode_Search_p_PAYC_PLAN_CD'));   

            $("#txtClinicaliCESAction").val(localStorage.getItem('payCode_Search_p_ClinicaliCESAction'));   
            $("#txtClinicaliCESName").val(localStorage.getItem('payCode_Search_p_ClinicaliCESName'));   


            $("#txtIncludeHistorical").val(localStorage.getItem('payCode_Search_p_INCLUDE_HISTORICAL'));   
            setTimeout(function () {
                $(function () {
                    SearchController.multiSelect.loadMultiSelect();
                    //SearchController.grid.refreshGrid();

                    if ($("#txtIncludeHistorical").val() == 1) {
                        var switchInstance = $("#switch").data("kendoSwitch");
                        switchInstance.check(true);
                    }
                });
            }, 1000);

        },
        clearLocalStorage: function () {
            localStorage.setItem('payCode_Search_p_PAYC_HIERARCHY_KEY', '');
            localStorage.setItem('payCode_Search_p_PAYC_STATUS', '');
            localStorage.setItem('payCode_Search_p_PAYC_NDB_PCS', '');
            localStorage.setItem('payCode_Search_p_PAYC_KL_PCS', '');

            localStorage.setItem('payCode_Search_p_PROC_CD', '');

            //DY - 09/06/2023 - Commented Alt Category Filters, per Lisa Cornish Request.
            //localStorage.setItem('payCode_Search_p_ALTRNT_SVC_CAT', '');
            //localStorage.setItem('payCode_Search_p_ALTRNT_SVC_SUBCAT', '');


            localStorage.setItem('payCode_Search_p_iCES', '');
            localStorage.setItem('payCode_Search_p_PRIOR_AUTH_STATUS', '');

            localStorage.setItem('payCode_Search_p_CURRENT_EFF_DT', '');
            localStorage.setItem('payCode_Search_p_CURRENT_EXP_DT', '');
            localStorage.setItem('payCode_Search_p_PAYC_VER_EFF_DT', '');
            localStorage.setItem('payCode_Search_p_PAYC_BUS_SEG_CD', '');
            localStorage.setItem('payCode_Search_p_PAYC_ENTITY_CD', '');
            localStorage.setItem('payCode_Search_p_PAYC_PRODUCT_CD', '');
            localStorage.setItem('payCode_Search_p_PAYC_PLAN_CD', '');

            localStorage.setItem('payCode_Search_p_ClinicaliCESAction', '');
            localStorage.setItem('payCode_Search_p_ClinicaliCESName', '');


            localStorage.setItem('payCode_Search_p_INCLUDE_HISTORICAL', '');

            $("#txtPayCodeID").val('');
            $("#ddlPayCodeStatus").val('');
            $("#txtNDBPCS").val('');
            $("#txtKLPCS").val('');

            $("#txtProcedureCode").val('');

            //DY - 09/06/2023 - Commented Alt Category Filters, per Lisa Cornish Request.
            //$("#txtAlternateCategory").val('');
            //$("#txtAlternateSubCategory").val('');


            $("#txtiCES").val(localStorage.getItem('payCode_Search_p_iCES'));
            $("#ddlPriorAuthStatus").val('');

            $("#dpCurrentEffectiveDate").val('');
            $("#dpCurrentExpirationDate").val('');
            $("#dpPayCodeVerEffDate").val('');
            $("#txtLOB").val('');
            $("#txtEntity").val('');
            $("#txtProduct").val('');
            $("#txtPlan").val('');   
            $("#txtClinicaliCESAction").val();
            $("#txtClinicaliCESName").val();   
        }
    },
    switch: {
        defaultChecked: function () {
            $("#txtIncludeHistorical").val(1);
        },
        onChange: function (e) {
            if (e.checked == true) {
                $("#txtIncludeHistorical").val(1);
                $("#btnSearch").click();
            }
            else {
                $("#txtIncludeHistorical").val(0);
                $("#btnSearch").click();
            }
           
        }
    },
    grid: {
        refreshGrid: function () {
            $("#grid_Search").data("kendoGrid").dataSource.read();
            $("#grid_Search").data("kendoGrid").refresh();
        },
        param: function () {
            var paramObj = {
                p_PAYC_HIERARCHY_KEY: $.trim($("#txtPayCodeID").val()),
                p_PAYC_PRODUCT_CD: $.trim($("#txtProduct").val()),
                p_PROC_CD: $.trim($("#txtProcedureCode").val()),
                p_PAYC_PLAN_CD: $.trim($("#txtPlan").val()),
                p_PAYC_BUS_SEG_CD: $.trim($("#txtLOB").val()),
                p_PAYC_ENTITY_CD: $.trim($("#txtEntity").val()),
                p_PAYC_KL_PCS: $.trim($("#txtKLPCS").val()),
                p_PAYC_NDB_PCS: $.trim($("#txtNDBPCS").val()),
                p_CURRENT_EFF_DT: $.trim($("#dpCurrentEffectiveDate").val()),
                p_CURRENT_EXP_DT: $.trim($("#dpCurrentExpirationDate").val()),
                p_PAYC_VER_EFF_DT: $.trim($("#dpPayCodeVerEffDate").val()),
                p_INCLUDE_HISTORICAL: $("#txtIncludeHistorical").val(),
                p_PAYC_STATUS: $.trim($("#ddlPayCodeStatus").val()),
                p_PAYC_ICES_EDIT_NAME: $.trim($("#txtClinicaliCESName").val()),
                p_PAYC_ICES_EDIT_ACTION: $.trim($("#txtClinicaliCESAction").val()),

                //DY - 09/06/2023 - Commented Alt Category Filters, per Lisa Cornish Request.
                //p_ALTRNT_SVC_CAT: $.trim($("#txtAlternateCategory").val()),
                //p_ALTRNT_SVC_SUBCAT: $.trim($("#txtAlternateSubCategory").val()),

                p_iCES: $.trim($("#txtiCES").val()),
                p_PRIOR_AUTH_STATUS: $.trim($("#ddlPriorAuthStatus").val()),

            }
            return paramObj
        },
        onDataBound: function (e) {
            var appRoleIds = $("#txtAppRoleIds").val();
            var appRoleArray = []
            appRoleArray.push(appRoleIds.split(','))

            var gridRows = this.tbody.find("tr");
            gridRows.each(function (e)
            {
                var rowNum = $(this).find("#lblRowNum").text();
                // DY: 03/04/2024 - Only allow Super Admins or PayCode Admins to see the pencil icon. 
                if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("7", appRoleArray[0]) !== -1)) {
                    $(this).closest('tr').find('.showPencil').show();
                }
                else if ((jQuery.inArray("6", appRoleArray[0]) !== -1) && rowNum =='1')
                {
                    $(this).closest('tr').find('.showPencil').show();
                }
                else {
                    $(this).closest('tr').find('.showPencil').hide();
                }
            });
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
        excelExport: function (e) {
            e.workbook.sheets[0].rows[0].cells[1].value = 'Pay Code Status';
            e.workbook.sheets[0].rows[0].cells[2].value = 'Procedure Code';
            e.workbook.sheets[0].rows[0].cells[3].value = 'Procedure Code Description';
            e.workbook.sheets[0].rows[0].cells[6].value = 'iCES Edit/CES';
            e.workbook.sheets[0].rows[0].cells[7].value = 'iCES Edit Name'; 
            e.workbook.sheets[0].rows[0].cells[8].value = 'iCES Edit Action';
            e.workbook.sheets[0].rows[0].cells[9].value = 'Prior Auth Status';
            e.workbook.sheets[0].rows[0].cells[10].value = 'Pay Code Effective Date';
            e.workbook.sheets[0].rows[0].cells[11].value = 'Pay Code Expiration Date';
            e.workbook.sheets[0].rows[0].cells[12].value = 'Pay Code Ver Effective Date';            

            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];
                row.cells[12].type = "date";
                row.cells[12].format = 'MM/dd/yyyy hh:mm:ss';
                row.cells[12].value = kendo.toString(kendo.parseDate(row.cells[12].value), 'MM/dd/yyyy hh:mm:ss tt');
            };
        },
        onGridRequestEnd: function (e) {
            window.kendo.ui.progress($("#grid_Search"), false);
        }
    },
    methods: {
        redirect_Detail: function (page,id ) {

            var trg_url = VIRTUAL_DIRECTORY + "/PayCodes/Home/" + page + "Detail/" + id;
            windowPopup("_blank", trg_url, null, swalRedirect);

            function swalRedirect(url) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to detail page, please wait..',
                    timerProgressBar: true,
                    timer: 50000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = url;
                    }
                })
            }
        },
    },
    helper: {
        isValidDate: function(s) {
            var bits = s.split('/');
            var d = new Date(bits[2] + '/' + bits[0] + '/' + bits[1]);
            return !!(d && (d.getMonth() + 1) == bits[0] && d.getDate() == Number(bits[1]));
        }
    } 


}

$(document).ready(function () {
    SearchController.init();
});