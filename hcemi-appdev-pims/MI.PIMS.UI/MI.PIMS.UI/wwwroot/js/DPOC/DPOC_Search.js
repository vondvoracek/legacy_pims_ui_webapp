var counter = 0;
var isIQReferenceBound = false;
var appliedStoredValues = {};

var DPOC_SearchController = {
    init: function () {
        DPOC_SearchController.bind();
    },
    bind: function () {

        DPOC_SearchController.filters.hideAdvancedSearch();

        DPOC_SearchController.filters.getLocalStorage();

        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        $("#searchForm").on("submit", function (e) {
            e.preventDefault();
            $("#btnSearch").click(); // Trigger the actual search logic
        });


        $("#btnReset").click(function () {
            $('#searchForm').trigger("reset");
            $("#txtDPOCID").val('')
            $("#msLOB").val('')
            $("#msEntity").val('')
            $("#txtPIMS_ID").val('')
            $("#txtProcedureCodeText").val('')
            
            var dropdown = $("#ddlIQGuidelineID").data("kendoDropDownList");
            dropdown.dataSource.filter({}); // Removes all filters

            setTimeout(function () {
                dropdown.dataSource.read();
            }, 1000);
          
            $("#ddlIQGuidelineID").val('')
            $("#ddlIQVersion").val('')
            $("#msIQCriteria").val('')
            $("#txtIQReference").val('')
            $("#dpDPOCStartDate").val('')
            $("#txtDPOCPackage").val('')
            $("#txtDPOCRelease").val('')
            $("#rdoDPOCEligible").val('')
            $("#rdoDPOCImplemented").val('')
            $("#txtAlternateCategory").val('')
            $("#txtAlternateSubcategory").val('')
            $("#dpDPOCVerEffDate").val('')
            $("#txtDTQName").val('')
            $("#txtDTQType").val('')
            $("#txtDTQReason").val('')
            $("#txtOutpatientRuleTypeOutcome").val('')
            //$("#txtOutpatientRuleType").val('')
            $("#txtOutpatientFacilityRuleTypeOutcome").val('')
            $("#txtOutpatientFacilityRuleType").val('')
            $("#txtInpatientRuleTypeOutcome").val('')
            //$("#txtInpatientRuleType").val('')
            $("#txtJurisdiction").val('')

            DPOC_SearchController.multiSelect.refreshMultiSelect();

            DPOC_SearchController.filters.clearLocalStorage();
            $("#grid_Search").data("kendoGrid").dataSource.data([]);

            if ($("#txtIncludeHistorical").val() == 1) {
                $("txtIncludeHistorical").val('');
                var switchInstance = $("#switch").data("kendoSwitch");
                switchInstance.check(false);
            }

            location.reload()
        });

        $("#btnSearch").click(function (e) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.DPOCSearch")) {
                const logProperties = new LogInfo(e.target.id, MS_ID, "DPOC Search Button click");
                appInsightsClient.trackEvent('Client.DPOCSearch.Search', logProperties);
            }

            kendo.ui.progress($("#grid_Search"), true);

            DPOC_SearchController.filters.setLocalStorage();
            DPOC_SearchController.grid.refreshGrid();

        });

        $("#btnAdvancedSearch").click(function () {
            DPOC_SearchController.filters.toggleAdvancedSearch();
        });        

        setTimeout(function () {
            $(".k-grid-excel").click(function () {
                kendo.ui.progress($("#grid_Search"), true);
            });                                   
        }, 1000)

        $("#txtProcedureCodeText").on("input", function () {
            var value = $(this).val().trim();
            var msAlternateCategory = $("#msAlternateCategory").data("kendoMultiSelect");
            var msAlternateSubCategory = $("#msAlternateSubCategory").data("kendoMultiSelect");
            if (value === '') {
                if (msAlternateCategory) {                    
                    msAlternateCategory.dataSource.data([]); // Clear the data source                    
                    msAlternateCategory.refresh();
                }
                if (msAlternateSubCategory) {
                    msAlternateSubCategory.dataSource.data([]); // Clear the data source                    
                    msAlternateSubCategory.refresh();
                }
            } else {
                $("#msAlternateCategory").data('kendoMultiSelect').dataSource.read();
                $("#msAlternateSubCategory").data('kendoMultiSelect').dataSource.read();
            }
        });
    },
    datePicker: {
        dpFromEffectiveDateChange: function (e) {
            var endDate = $("#dpFromExpirationDate").data("kendoDatePicker"),
                fromDate = this.value();

            if (fromDate) {
                fromDate = new Date(fromDate);
                fromDate.setDate(fromDate.getDate() + 1);
                endDate.min(fromDate);
            } else {
                endDate.min("01/01/1900");
            }
        },
        dpEndEffectiveDateChange: function (e) {
            var fromDate = $("#dpFromEffectiveDate").data("kendoDatePicker"),
                endDate = this.value();

            if (endDate) {
                endDate = new Date(endDate);
                endDate.setDate(endDate.getDate() - 1);
                fromDate.max(endDate);
            }
        },
        dpFromExpirationDateChange: function (e) {
            var endDate = $("#dpFromEffectiveDate").data("kendoDatePicker"),
                fromDate = this.value();

            if (fromDate) {
                fromDate = new Date(fromDate);
                fromDate.setDate(fromDate.getDate() - 1);
                endDate.max(fromDate);
            } else {
                endDate.max("01/01/2999");
            }
        },
        dpEndExpirationDateChange: function (e) {
            var fromDate = $("#dpFromExpirationDate").data("kendoDatePicker"),
                endDate = this.value();

            if (endDate) {
                endDate = new Date(endDate);
                endDate.setDate(endDate.getDate() - 1);
                fromDate.max(endDate);
            }
        }
    },
    multiSelect: {
        refreshMultiSelect: function () {
            //$("#msLOB").data('kendoMultiSelect').dataSource.read();
            $("#msEntity").data('kendoMultiSelect').dataSource.read();
            $("#msIQCriteria").data('kendoMultiSelect').dataSource.read();
            $("#msIQReference").data('kendoMultiSelect').dataSource.read();
            $("#msDPOCPackage").data('kendoMultiSelect').dataSource.read();
            $("#msDPOCRelease").data('kendoMultiSelect').dataSource.read();
            $("#msDPOCEligible").data('kendoDropDownList').dataSource.read();
            $("#msDPOCImplemented").data('kendoMultiSelect').dataSource.read();
            $("#msAlternateCategory").data('kendoMultiSelect').dataSource.read();
            $("#msAlternateSubCategory").data('kendoMultiSelect').dataSource.read();
            $("#msOutpatientRuleTypeOutcome").data('kendoMultiSelect').dataSource.read();
            $("#msOutpatientFacilityRuleTypeOutcome").data('kendoMultiSelect').dataSource.read();
            $("#msInpatientRuleTypeOutcome").data('kendoMultiSelect').dataSource.read();
            $("#msJurisdiction").data('kendoMultiSelect').dataSource.read();
        },
        onDatabound_IQ_REFERENCE: function () {
            if (!isIQReferenceBound) {
                var ls_dpoc_IQReference = localStorage.getItem('msIQReference');
                if (ls_dpoc_IQReference) {
                    $("#msIQReference").data("kendoMultiSelect").value(ls_dpoc_IQReference.split('|'));
                }
                isIQReferenceBound = true;
            }
        },
        param: {
            LOB: function () {
                return {
                    columnName: "epal_bus_seg_cd"
                }
            },
            ENTITY: function () {
                return {
                    epal_bus_seg_cd: $("#msLOB").data("kendoMultiSelect").value().join(',')
                }
            },
            PRODUCT: function () {
                return {
                    epal_bus_seg_cd: $("#msLOB").data("kendoMultiSelect").value().join(','),
                    epal_entity_cd: $("#msEntity").data("kendoMultiSelect").value().join(','),
                    epal_fund_arngmnt_cd: $("#msFundingArrangment")?.data("kendoMultiSelect")?.value().join(',')
                }
            },
            FUNDING_ARRANGEMENT: function () {
              
                return {
                    epal_bus_seg_cd: $("#msLOB").data("kendoMultiSelect").value().join(','),
                    epal_entity_cd: $("#msEntity").data("kendoMultiSelect").value().join(','),
                    epal_product_cd: $("#msProduct").data("kendoMultiSelect").value().join(',')
                }
            },
            IQ_CRITERIAs: function () {
                return {
                    p_VV_SET_NAME: "DPOC_IQ_CRITERIA",
                    p_BUS_SEG_CD: null
                }
            },

            IQ_REFERENCE: function () {
                return {
                    p_column_name: "iq_reference",
                }
            },

            IQ_CRITERIA: function () {
                return {
                    P_COLUMN_NAME: "IQ_CRITERIA",
                }
            },

            DPOC_PACKAGE: function () {
                return {
                    p_VV_SET_NAME: "DPOC_PACKAGE",
                    p_BUS_SEG_CD: $("#msLOB").data("kendoMultiSelect").value().join(',')
                }
            },

            DPOC_RELEASE: function () {
                return {
                    p_column_name: "DPOC_RELEASE",
                }
            },
            DPOC_IMPLEMENTED_IND: function () {
                return {
                    p_VV_SET_NAME: "DPOC_IMPLEMENTED_IND",
                    p_BUS_SEG_CD: null
                }
            },
            AlternateCategory: function () {
                return {
                    p_text: $("#txtAlternateCategoryFilterText").val(),
                    p_column_name: "altrnt_svc_cat", //EPAL_ALTRNT_SVC_CAT
                    p_proc_cds: $('#txtProcedureCodeText').val()
                }
            },

            AlternateSubCategory: function () {
                return {
                    p_text: $("#txtAlternateSubCategoryFilterText").val(),
                    p_column_name: "ALTRNT_SVC_SUBCAT", //EPAL_ALTRNT_SVC_SUBCAT
                    p_proc_cds: $('#txtProcedureCodeText').val(),
                    p_epal_altrnt_svc_cat: $('#txtAlternateCategory').val()
                }
            },

            DPOC_JURISDICTIONS: function () {
                return {
                    p_VV_SET_NAME: "DPOC_JURISDICTIONS",
                    p_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
        },
        onChange_LOB: function (e) {
            $("#msEntity").data('kendoMultiSelect').dataSource.read();

            $("#msDPOCPackage").data('kendoMultiSelect').dataSource.read();

            $("#msFundingArrangment").data('kendoMultiSelect').dataSource.read();

            $("#msProduct").data('kendoMultiSelect').dataSource.read(); 
        },
        onChange_ENTITY: function () {
            $("#msFundingArrangment").data('kendoMultiSelect').dataSource.read();

            $("#msProduct").data('kendoMultiSelect').dataSource.read(); 
        },
        onChange_PRODUCT: function () {
            $("#msFundingArrangment").data('kendoMultiSelect').dataSource.read();
        },
        onChange_FUNDING_ARRANGEMENT: function () {
            $("#msProduct").data('kendoMultiSelect').dataSource.read();
        },
        onChange_IQ_CRITERIA: function () {
            /*PLACEHOLDER*/
        },
        onChange_IQ_REFERENCE: function () {
            /*PLACEHOLDER*/
        },
        onChange_AlternateCategory: function () {
            /*PLACEHOLDER*/
        },
        onChange_AlternateSubCategory: function () {
            /*PLACEHOLDER*/
        },
        onChange_PROG_MGD_BY: function () {
            /*PLACEHOLDER*/
        },
        onChange_JURISDICTIONS: function () {
            /*PLACEHOLDER*/
        },
        onChange_DPOC_PACKAGE: function () {
            /*PLACEHOLDER*/
        },
        onChange_DPOC_RELEASE: function () {
            /*PLACEHOLDER*/
        },
        onChange_DPOC_DTQ_APPLIES: function () {
            /*PLACEHOLDER*/
        },
        onChange_DTQ_REASON: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE_DECISION: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE_OUTCOME_OUTPAT: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE_OUTCOME_OUTPAT_FCLTY: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE_OUTPAT_FCLTY: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE_OUTCOME_INPAT: function () {
            /*PLACEHOLDER*/
        },
        onChange_RULE_TYPE: function () {
            /*PLACEHOLDER*/
        },
        onChange_DPOCEligible: function () {
            /*PLACEHOLDER*/
        },
        onChange_DPOCImplemented: function () {
            /*PLACEHOLDER*/
        }
    },
    dropdown: {
        IQ_GDLN_IDValueMapper: function (options) {
            $.ajax({
                url: VIRTUAL_DIRECTORY + "/DPOC/GuidelineSummary/GetGetGuideLineIds_ValueMapper",
                xhrFields: {
                    withCredentials: true
                },
                data: convertValues(options.value),
                success: function (data) {
                    if (Array.isArray(data)) {
                        options.success(data);
                    } else {
                        options.success([]); // Prevent undefined access
                    }
                },
                error: function () {
                    options.success([]); // Prevent binding errors
                }

            });

            function convertValues(value) {
                var data = {};
                value = $.isArray(value) ? value : [value];
                for (var idx = 0; idx < value.length; idx++) {
                    data["values[" + idx + "]"] = value[idx];
                }
                return data;
            }
        },        

        onDatabound_IQGuidelineID: function () {

            var dropdown = $("#ddlIQGuidelineID").data("kendoDropDownList");
            var ls_dpoc_IQGuidelineID = $.trim(localStorage.getItem('ddlIQGuidelineID'));

            if (ls_dpoc_IQGuidelineID) {
                dropdown.select(function (dataItem) {
                    return dataItem.IQ_GDLN_ID == ls_dpoc_IQGuidelineID;
                });
            }
         

            /*
            setTimeout(function () {
           
                var ls_dpoc_IQGuidelineID = localStorage.getItem('ddlIQGuidelineID');
                var dropdown = $("#ddlIQGuidelineID").data("kendoDropDownList");

                if (ls_dpoc_IQGuidelineID) {
                    // Set the filter text

                    dropdown.search(ls_dpoc_IQGuidelineID); 
                    dropdown.one("dataBound", function () {

                        var filteredData = dropdown.dataSource.view();

                        var match = filteredData.find(function (item) {
                            return item.IQ_GDLN_ID === ls_dpoc_IQGuidelineID; // Replace with desired value
                        });


               
                        if (match) {
                            dropdown.value(match.IQ_GDLN_ID); 
                            dropdown.dataSource.transport.read = function () {
                                return false; // Prevent further server calls
                            };
                        }
                    });
                }
            }, 3000);*/
        },

        onDatabound_IQVersion: function () {
            var ls_dpoc_IQVersion = localStorage.getItem('ddlIQVersion');
            if (ls_dpoc_IQVersion) {
                $("#ddlIQVersion").data("kendoDropDownList").value(ls_dpoc_IQVersion);
            } 
        },
        IQ_GDLN_VERSIONValueMapper: function (options) {
            $.ajax({
                url: VIRTUAL_DIRECTORY + "/DPOC/GuidelineSummary/GetGuideLineVersions_ValueMapper",
                data: convertValues(options.value),
                async: true,
                headers: {
                    'Accept': 'application/json'
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (data) {
                    options.success(data);
                }
            });

            function convertValues(value) {
                var data = {};

                value = $.isArray(value) ? value : [value];

                for (var idx = 0; idx < value.length; idx++) {
                    data["values[" + idx + "]"] = value[idx];
                }

                return data;
            }
        },
        param: {
            PARAM_IQ_GDLN_ID: function (e) {
                return {
                    p_text: $("#ddlIQGuidelineID").val(),
                }
            },
            PARAM_IQ_GDLN_VERSION: function () {
                return {
                    p_text: $("#txtIQVersionFilterText").val()
                }
            },
            PARAM_DPOC_PACKAGE: function () {
                return {
                    P_COLUMN_NAME: "DPOC_PACKAGE",
                }
            },
            PARAM_DPOC_RELEASE: function () {
                return {
                    P_COLUMN_NAME: "DPOC_RELEASE",
                }
            },
            DPOC_ELIGIBLE_IND: function () {
                return {
                    p_VV_SET_NAME: "DPOC_ELIGIBLE_IND",
                    p_BUS_SEG_CD: ""
                }
            }
        }
    },
    filters: {
        setLocalStorage: function () {
            var fields = {
                '#txtDPOCID': 'txtDPOCID',
                '#msLOB': 'msLOB',
                '#msEntity': 'msEntity',
                '#txtProcedureCodeText': 'txtProcedureCodeText',
                '#ddlIQGuidelineID': 'ddlIQGuidelineID',
                '#ddlIQVersion': 'ddlIQVersion',
                '#msIQCriteria': 'msIQCriteria',                
                '#dpDPOCStartDate': 'dpDPOCStartDate',
                '#msDPOCPackage': 'msDPOCPackage',
                '#msDPOCRelease': 'msDPOCRelease',
                '#msDPOCEligible': 'msDPOCEligible',
                '#msDPOCImplemented': 'msDPOCImplemented',
                '#msAlternateSubCategory': 'msAlternateSubCategory',
                '#msAlternateCategory': 'msAlternateCategory',
                '#dpDPOCVerEffDate': 'dpDPOCVerEffDate',
                '#msOutpatientRuleTypeOutcome': 'msOutpatientRuleTypeOutcome',
                '#msOutpatientFacilityRuleTypeOutcome': 'msOutpatientFacilityRuleTypeOutcome',
                '#msInpatientRuleTypeOutcome': 'msInpatientRuleTypeOutcome',
                '#msJurisdiction': 'msJurisdiction',
                '#msProduct': 'msProduct',
                '#msFundingArrangment': 'msFundingArrangment'
            };

            for (var selector in fields) {
                if (fields.hasOwnProperty(selector)) {
                    var key = fields[selector];
                    var value = $(selector).val();
                    localStorage.setItem(key, value);
                }
            }

            var iqReference = $("#msIQReference").val();
            var joinedReference = iqReference ? iqReference.join('|') : '';
            localStorage.setItem('msIQReference', joinedReference);
        },
        getLocalStorage: function () {
            $("#txtDPOCID").val(localStorage.getItem('txtDPOCID'));
            $("#txtProcedureCodeText").val(localStorage.getItem('txtProcedureCodeText'));
            $("#dpDPOCStartDate").val(localStorage.getItem('dpDPOCStartDate'));                   
            $("#dpDPOCVerEffDate").val(localStorage.getItem('dpDPOCVerEffDate'));            

            if ($("#txtIncludeHistorical").val() == 1) {
                var switchInstance = $("#switch").data("kendoSwitch");
                switchInstance.check(true);
            }
        },

        clearLocalStorage: function () {
            localStorage.clear();
        },

        toggleAdvancedSearch: function () {
            counter += 1
            $("#divAdvancedSearch").toggle();
            if (counter % 2 == 0) {
                $("#icon-down").show();
                $("#icon-up").hide();
            }
            else {
                $("#icon-up").show();
                $("#icon-down").hide();
            }
        },

        hideAdvancedSearch: function () {
            $("#divAdvancedSearch").hide();
            $("#icon-up").hide();
        }
    },
    switch: {
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
                p_DPOC_HIERARCHY_KEY: $.trim($("#txtDPOCID").val()),   		                                        // 1.
                p_DPOC_BUS_SEG_CD: $.trim($("#msLOB").val()),	 		                                            // 2.
                p_DPOC_ENTITY_CD: $.trim($("#msEntity").val()),                                                    // 3.
                p_PROC_CD: $.trim($("#txtProcedureCodeText").val()),                                                    // 4.
                p_IQ_GDLN_ID: $.trim($("#ddlIQGuidelineID").val()),                                                 // 5.
                p_IQ_GDLN_VERSION: $.trim($("#ddlIQVersion").val()),                                                // 6.
                p_IQ_CRITERIA: $.trim($("#msIQCriteria").val()),                                                   // 7.
                p_IQ_REFERENCE: $.trim($("#msIQReference").data("kendoMultiSelect").value().join('|')),            // 8.
                p_DPOC_EFF_DT: $.trim($("#dpDPOCStartDate").val()),                                                 // 9.
                p_DPOC_PACKAGE: $.trim($("#msDPOCPackage").val()),                                                 // 10.
                p_DPOC_RELEASE: $.trim($("#msDPOCRelease").val()),                                                 // 11.
                p_DPOC_ELIGIBLE_IND: $.trim($("#msDPOCEligible").val()),                                           // 12.
                p_DPOC_IMPLEMENTED_IND: $.trim($("#msDPOCImplemented").val()),                                     // 14.
                p_EPAL_ALTRNT_SVC_CAT: $.trim($("#msAlternateCategory").val()),                                    // 15.
                p_EPAL_ALTRNT_SVC_SUBCAT: $.trim($("#msAlternateSubCategory").val()),                              // 16.
                p_DPOC_VER_EFF_DT: $.trim($("#dpDPOCVerEffDate").val()),                                            // 17.
                p_RULE_TYPE_OUTCOME_OUTPAT: $.trim($("#msOutpatientRuleTypeOutcome").val()),                       // 21.
                p_RULE_TYPE_OUTCOME_OUTPAT_FCLTY: $.trim($("#msOutpatientFacilityRuleTypeOutcome").val()),         // 23.
                p_RULE_TYPE_OUTCOME_INPAT: $.trim($("#msInpatientRuleTypeOutcome").val()),                         // 25.
                p_JRSDCTN_NM: $.trim($("#msJurisdiction").val()),                                                  // 27.
                p_INCLUDE_HISTORICAL: $.trim($("#txtIncludeHistorical").val()),                                    // 28.
                p_PRODUCT_CD: $.trim($("#msProduct").val()),
                p_FUND_ARNGMNT_CD: $.trim($("#msFundingArrangment").val())
            }
            return paramObj
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
            window.kendo.ui.progress($("#grid_Search"), false);
            e.workbook.sheets[0].rows[0].cells[1].value = 'Procedure Code';
            e.workbook.sheets[0].rows[0].cells[2].value = 'DPOC Start Date';
            e.workbook.sheets[0].rows[0].cells[6].value = 'DPOC Version Eff Dt'; // Bug 136301 MFQ 7/17/2025 change of column id
        },
        onGridRequestEnd: function (e) {
            window.kendo.ui.progress($("#grid_Search"), false);
        }
    },
    methods: {
        redirect: function (pims_id) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.DPOCSearch")) {
                const logProperties = new LogInfo("Search.redirect", MS_ID, pims_id);
                appInsightsClient.trackEvent('Client.DPOCSearch.ViewDetail', logProperties);
            }

            var trg_url = MIApp.Common.ApiEPRepository.get('viewdetail', 'DpocUrls')
                .replace('__pims_id__', MIApp.Sanitize.encode(pims_id));

            function swalRedirect() {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to record, please wait..',
                    timerProgressBar: true,
                    // timer: 5000,
                    timer: 180000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = trg_url;
                    }
                });
            }

            windowPopup("_blank", trg_url, null, swalRedirect);
        },
        redirect_edit: function (pims_id) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.DPOCSearch")) {
                const logProperties = new LogInfo("Search.redirect_edit", MS_ID, pims_id);
                appInsightsClient.trackEvent('Client.DPOCSearch.EditDetail', logProperties);
            }

            var trg_url = MIApp.Common.ApiEPRepository.get('editdetail', 'DpocUrls')
                .replace('__pims_id__', MIApp.Sanitize.encode(pims_id));

            function swalRedirect() {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to record, please wait..',
                    timerProgressBar: true,
                    // timer: 5000,
                    timer: 180000,
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = trg_url;
                    }
                })
            }

            windowPopup("_blank", trg_url, null, swalRedirect);
        },
    },
    helper: {
        isValidDate: function (s) {
            var bits = s.split('/');
            var d = new Date(bits[2] + '/' + bits[0] + '/' + bits[1]);
            return !!(d && (d.getMonth() + 1) == bits[0] && d.getDate() == Number(bits[1]));
        }
    }
}

function updateMultiSelectFromInput(multiselectId, inputId) {
    var multiselect = $(multiselectId).data("kendoMultiSelect");
    var values = $(inputId).val();

    if (!multiselect) {
        console.warn("MultiSelect widget not found for selector:", multiselectId);
        return;
    }

    if (values) {
        var splitValues = values.split(",").map(function (item) {
            return item.trim();
        });
        multiselect.value(splitValues);
    } else {
        multiselect.value([]);
    }
}

$(document).ready(function () {
    DPOC_SearchController.init();

    setTimeout(function () {
        var ddl = $("#ddlIQGuidelineID").data("kendoDropDownList");
        ddl.bind("filtering", function (e) {
            currentFilterText = e.filter ? e.filter.value : "";
        });

    }, 300)
});

function onDatabound_DropDown(e) {
    var name = e.sender.element.attr("name");
    var storedValue = localStorage.getItem(name);
    if (storedValue) {
        e.sender.value(storedValue);
    }
}

function onDatabound_MultiSelect(e) {
    var name = e.sender.element.attr("name");

    // Only apply stored value once per widget
    if (!appliedStoredValues[name]) {
        var storedValue = localStorage.getItem(name);
        if (storedValue && storedValue !== 'null') {
            e.sender.value(storedValue.split(','));
        }
        appliedStoredValues[name] = true;
    }
}

var currentFilterText = "";

function filterData() {
    return {
        text: currentFilterText
    };
}


function IQ_GDLN_IDValueMapper(options) {
    $.ajax({
        type: "GET",
        url: "/DPOC/GuidelineSummary/MapGuidelineIDs", // create this endpoint
        data: {
            values: options.value // this is an array of selected values
        },
        success: function (data) {
            options.success(data); // return the matching data items
        },
        error: function () {
            options.error();
        }
    });
}



