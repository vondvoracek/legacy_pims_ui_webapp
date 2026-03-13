var counter = 0;

var SearchController = {
    init: function () {
        SearchController.bind();
    },
    bind: function () {

        hideAdvancedSearch();

        $(function () {
            $('.noSubmit').on('click', function (event) { event.preventDefault(); });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })


        $("#btnReset").click(function () {            
            $('#searchForm').trigger("reset");
            SearchController.filters.clearLocalStorage();
            //Bug 124813 Resolution
            location.reload();
        });

        $("#btnSearch").click(function (e) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.EPALSearch")) {
                const logProperties = new LogInfo(e.target.id, MS_ID, "EPAL Search Button click");
                appInsightsClient.trackEvent('Client.EPALSearch.Search', logProperties);
            }

            window.kendo.ui.progress($("#grid_Search"), true);
            SearchController.filters.setLocalStorage();
            SearchController.grid.refreshGrid();
        });

        $("#btnAdvancedSearch").click(function () {
            toggleAdvancedSearch();
        });

        function toggleAdvancedSearch() {
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
        }

        function hideAdvancedSearch() {
            $("#divAdvancedSearch").hide();
            $("#icon-up").hide();
        }

        SearchController.filters.getLocalStorage();
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
        loadMultiSelect: function () {
            SearchController.multiSelect.msPRODUCT();
            SearchController.multiSelect.msLOB();
            SearchController.multiSelect.msPLAN();
            SearchController.multiSelect.msFUNDING_ARRANGEMENT();
            SearchController.multiSelect.msENTITY();       
            SearchController.multiSelect.msSTATUS();
            SearchController.multiSelect.msStandardCategory();
            SearchController.multiSelect.msStandardSubCategory();
            SearchController.multiSelect.msAlternateCategory();
            SearchController.multiSelect.msAlternateSubCategory();
        },
        refreshMultiSelect: function () {
            $("#msProduct").data('kendoMultiSelect').dataSource.read();
            $("#msPlan").data('kendoMultiSelect').dataSource.read();
            $("#msFundingArrangement").data('kendoMultiSelect').dataSource.read();
            $("#msProduct").data('kendoMultiSelect').dataSource.read();
            $("#msEntity").data('kendoMultiSelect').dataSource.read();
        },
        onChange_APP_PRODUCT: function () {
            $("#txtProduct").val('');
            $("#txtProduct").val($("#msProduct").val());

            SearchController.multiSelect.refreshMultiSelect();
        },
        onChange_LOB: function () {
            $("#txtLOB").val('');
            $("#txtLOB").val($("#msLOB").val());

            SearchController.multiSelect.refreshMultiSelect();   
        },
        onChange_PLAN: function () {
            $("#txtPlan").val('');
            $("#txtPlan").val($("#msPlan").val());

            SearchController.multiSelect.refreshMultiSelect();
        },
        onChange_FUNDING_ARRANGEMENT: function () {
            $("#txtFundingArrangement").val('');
            $("#txtFundingArrangement").val($("#msFundingArrangement").val());
        },
        onChange_ENTITY: function () {
            $("#txtEntity").val('');
            $("#txtEntity").val($("#msEntity").val());

            SearchController.multiSelect.refreshMultiSelect();
        },
        onChange_STATE: function () {
            $("#txtState").val('');
            $("#txtState").val($("#msState").val());
        },
        onChange_STATUS: function () {
            $("#txtEPALStatus").val('');
            $("#txtEPALStatus").val($("#msEPALStatus").val());
        },
        onChange_StandardCategory: function () {            
            $("#txtStandardCategory").val($("#msStandardCategory").val());
        },
        onChange_StandardSubCategory: function () {
            $("#txtStandardSubCategory").val($("#msStandardSubCategory").val());
        },
        onChange_AlternateCategory: function () {
            $("#txtAlternateCategory").val($("#msAlternateCategory").val());
        },
        onChange_AlternateSubCategory: function () {
            $("#txtAlternateSubCategory").val($("#msAlternateSubCategory").val());
        },
        onFiltering_StandardCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtStandardCategoryFilterText').val(filterText);
            }
        },
        onFiltering_StandardSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtStandardSubCategoryFilterText').val(filterText);
            }
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
                    EPAL_ENTITY_CD: $("#txtEntity").val(),
                    EPAL_PLAN_CD: $("#txtPlan").val(),
                }
            },
            APP_LOB: function () {
                return {
                    COLUMN_NAME: "BUS_SEG_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val()
                    //EPAL_BUS_SEG_CD: ""
                }
            },
            APP_PLAN: function () {
                return {
                    COLUMN_NAME: "EPAL_PLAN_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val(),
                    EPAL_ENTITY_CD: $("#txtEntity").val()
                }
            },
            APP_FUNDING_ARRANGEMENT: function () {
                return {
                    COLUMN_NAME: "EPAL_FUND_ARNGMNT_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val(),
                    EPAL_ENTITY_CD: $("#txtEntity").val(),
                    EPAL_PLAN_CD: $("#txtPlan").val(),
                    EPAL_PRODUCT_CD: $("#txtProduct").val(),
                }
            },
            APP_ENTITY: function () {
                return {
                    COLUMN_NAME: "EPAL_ENTITY_CD",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val()
                    //EPAL_BUS_SEG_CD: ""
                }
            },
            PROG_MGD_BY: function () {
                return {
                    p_VV_SET_NAME: "PROG_MGD_BY",
                    EPAL_BUS_SEG_CD: $("#txtLOB").val()
                }
            },
            StandardCategory: function () {
                return {
                    text: $("#txtStandardCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "SVCTYPE",
                    P_PARENT_CATEGORY: ''
                }
            },
            StandardSubCategory: function () {
                return {
                    text: $("#txtStandardSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "SVCSUBTYPE",
                    P_PARENT_CATEGORY: $("#txtStandardCategory").val()
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
            }
        },
        msPRODUCT: function () {
            var multiselect = $("#msProduct").data("kendoMultiSelect");
            var values = $("#txtProduct").val();
            if (values == '') return; //BUG 139192 MFQ 10/7/2025
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msLOB: function () {
            var multiselect = $("#msLOB").data("kendoMultiSelect");
            var values = $("#txtLOB").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msPLAN: function () {
            var multiselect = $("#msPlan").data("kendoMultiSelect");
            var values = $("#txtPlan").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msFUNDING_ARRANGEMENT: function () {
            var multiselect = $("#msFundingArrangement").data("kendoMultiSelect");
            var values = $("#txtFundingArrangement").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msENTITY: function () {
            var multiselect = $("#msEntity").data("kendoMultiSelect");
            var values = $("#txtEntity").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msSTATUS: function () {
            var multiselect = $("#msEPALStatus").data("kendoMultiSelect");
            var values = $("#txtEPALStatus").val()
            var splitValues = values.split(",");
            multiselect.value(splitValues);
        },
        msStandardCategory: function () {
            var multiselect = $("#msStandardCategory").data("kendoMultiSelect");
            var values = $("#txtStandardCategory").val();
            SearchController.multiSelect.setMultiSelectDS(multiselect, values);
        },
        msStandardSubCategory: function () {
            var multiselect = $("#msStandardSubCategory").data("kendoMultiSelect");
            var values = $("#txtStandardSubCategory").val();
            SearchController.multiSelect.setMultiSelectDS(multiselect, values);
        },
        msAlternateCategory: function () {
            var multiselect = $("#msAlternateCategory").data("kendoMultiSelect");
            var values = $("#txtAlternateCategory").val();
            SearchController.multiSelect.setMultiSelectDS(multiselect, values);
        },
        msAlternateSubCategory: function () {
            var multiselect = $("#msAlternateSubCategory").data("kendoMultiSelect");
            var values = $("#txtAlternateSubCategory").val();
            SearchController.multiSelect.setMultiSelectDS(multiselect, values);
        },
        setMultiSelectDS: function (multiselect, values) {
            var splitValues = values.split(",");
            var sc = []
            $.each(splitValues, function (i, item) {
                if($.trim(item).length > 0)
                    sc.push({ "CATEGORY": item });
            });
            if (sc.length > 0) {
                multiselect.dataSource.data(sc);
                multiselect.value(splitValues);
            }            
        }
    },
    dropdown: {
        param: {
            SUSP_IND: function () {
                return {
                    p_VV_SET_NAME: "SUSP_IND",
                    p_BUS_SEG_CD: null
                }
            },
            SUSP_TYPE: function () {
                return {
                    p_VV_SET_NAME: "SUSP_TYPE",
                    p_BUS_SEG_CD: null
                }
            }
        }
    },
    filters: {
        setLocalStorage: function () {
            var v_lob = $("#txtLOB").val();
            var v_product = $("#txtProduct").val();
            var v_plan = $("#txtPlan").val();
            var v_fundingarrangement = $("#txtFundingArrangement").val();
            var v_procedurecode = $("#txtProcedureCode").val();
            var v_pimsid = $("#txtPIMS_ID").val();
            var v_entity = $("#txtEntity").val();
            var v_drugname = $("#txtDrugName_Search").val();
            var v_epalstatus = $("#txtEPALStatus").val();
            var v_fromeffectivedate = $("#dpFromEffectiveDate").val();
            var v_endeffectivedate = $("#dpEndEffectiveDate").val();
            var v_fromexpirationdate = $("#dpFromExpirationDate").val();
            var v_endexpirationdate = $("#dpEndExpirationDate").val();
            var v_epalvereffdate = $("#dpEPALVerEffDate").val();
            var v_includehistorical = $("#txtIncludeHistorical").val();
            var v_standardcategory = $("#txtStandardCategory").val();
            var v_standardsubcategory = $("#txtStandardSubCategory").val();
            var v_alternatecategory = $("#txtAlternateCategory").val();
            var v_alternatesubcategory = $("#txtAlternateSubCategory").val();

            localStorage.setItem('ls_lob', v_lob);
            localStorage.setItem('ls_product', v_product);
            localStorage.setItem('ls_plan', v_plan);
            localStorage.setItem('ls_fundingarrangement', v_fundingarrangement);
            localStorage.setItem('ls_procedurecode', v_procedurecode);
            localStorage.setItem('ls_pimsid', v_pimsid);
            localStorage.setItem('ls_entity', v_entity);
            localStorage.setItem('ls_drugname', v_drugname);
            localStorage.setItem('ls_epalstatus', v_epalstatus);
            localStorage.setItem('ls_fromeffectivedate', v_fromeffectivedate);
            localStorage.setItem('ls_fromexpirationdate', v_fromexpirationdate);
            localStorage.setItem('ls_epalvereffdate', v_epalvereffdate);
            localStorage.setItem('ls_includehistorical', v_includehistorical);
            localStorage.setItem('ls_standardcategory', v_standardcategory);
            localStorage.setItem('ls_standardsubcategory', v_standardsubcategory);
            localStorage.setItem('ls_alternatecategory', v_alternatecategory);
            localStorage.setItem('ls_alternatesubcategory', v_alternatesubcategory);

        },
        getLocalStorage: function () {
            $('#txtLOB').val(localStorage.getItem('ls_lob'));
            $('#txtProduct').val(localStorage.getItem('ls_product'));
            $('#txtPlan').val(localStorage.getItem('ls_plan'));
            $('#txtFundingArrangement').val(localStorage.getItem('ls_fundingarrangement'));
            $('#txtProcedureCode').val(localStorage.getItem('ls_procedurecode'));
            $('#txtPIMS_ID').val(localStorage.getItem('ls_pimsid'));
            $('#txtEntity').val(localStorage.getItem('ls_entity'));
            $('#txtDrugName_Search').val(localStorage.getItem('ls_drugname'));
            $('#txtEPALStatus').val(localStorage.getItem('ls_epalstatus'));
            $('#dpFromEffectiveDate').val(localStorage.getItem('ls_fromeffectivedate'));
            $('#dpFromExpirationDate').val(localStorage.getItem('ls_fromexpirationdate'));
            $('#dpEPALVerEffDate').val(localStorage.getItem('ls_epalvereffdate'));
            $('#txtIncludeHistorical').val(localStorage.getItem('ls_includehistorical'));
            $('#txtStandardCategory').val(localStorage.getItem('ls_standardcategory'));
            $('#txtStandardSubCategory').val(localStorage.getItem('ls_standardsubcategory'));
            $('#txtAlternateCategory').val(localStorage.getItem('ls_alternatecategory'));
            $('#txtAlternateSubCategory').val(localStorage.getItem('ls_alternatesubcategory'));

            var v_lob = $("#txtLOB").val();
            var v_product = $("#txtProduct").val();
            var v_plan = $("#txtPlan").val();
            var v_fundingarrangement = $("#txtFundingArrangement").val();
            var v_procedurecode = $("#txtProcedureCode").val();
            var v_pimsid = $("#txtPIMS_ID").val();
            var v_entity = $("#txtEntity").val();
            var v_drugname = $("#txtDrugName_Search").val();
            var v_epalstatus = $("#txtEPALStatus").val();
            var v_fromeffectivedate = $("#dpFromEffectiveDate").val();
            var v_fromexpirationdate = $("#dpFromExpirationDate").val();
            var v_epalvereffdate = $("#dpEPALVerEffDate").val();
            var v_includehistorical = $("#txtIncludeHistorical").val();
            var v_standardcategory = $("#txtStandardCategory").val();
            var v_standardsubcategory = $("#txtStandardSubCategory").val();
            var v_alternatecategory = $("#txtAlternateCategory").val();
            var v_alternatesubcategory = $("#txtAlternateSubCategory").val();

            if ((v_lob != '') ||
                (v_product != '') ||
                (v_plan != '') ||
                (v_fundingarrangement != '') ||
                (v_procedurecode != '') ||
                (v_pimsid != '') ||
                (v_entity != '') ||
                (v_drugname != '') ||
                (v_epalstatus != '') ||
                (v_fromeffectivedate != '') ||
                (v_fromexpirationdate != '') ||
                (v_epalvereffdate != '') ||
                (v_includehistorical != '') ||
                (v_standardcategory != '') ||
                (v_standardsubcategory != '') ||
                (v_alternatecategory != '') ||
                (v_alternatesubcategory != '') 
                ) {

                setTimeout(function () {
                    $(function () {
                        SearchController.multiSelect.loadMultiSelect();
                       // SearchController.grid.refreshGrid();

                        if ($("#txtIncludeHistorical").val() == 1) {
                            var switchInstance = $("#switch").data("kendoSwitch");
                            switchInstance.check(true);
                        }   
                    });
                }, 1000);

            }

        },
        clearLocalStorage: function () {
            localStorage.clear();
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
            var OVERALL_EFF_DT_From = $.trim($("#dpFromEffectiveDate").val());
            var OVERALL_EXP_DT_From = $.trim($("#dpFromExpirationDate").val());

            if (SearchController.helper.isValidDate(OVERALL_EFF_DT_From) == true && SearchController.helper.isValidDate(OVERALL_EXP_DT_From) == false) {
                OVERALL_EXP_DT_From = '12/31/2999';
            }

            if (SearchController.helper.isValidDate(OVERALL_EXP_DT_From) == true && SearchController.helper.isValidDate(OVERALL_EFF_DT_From) == false) {
                OVERALL_EFF_DT_From = '01/01/1900'
            }

            var paramObj = {
                p_EPAL_HIERARCHY_KEY: MIApp.Sanitize.encodeProp($.trim($("#txtPIMS_ID").val())),
                p_EPAL_PRODUCT_CD: MIApp.Sanitize.encodeProp($.trim($("#txtProduct").val())),
                p_PROC_CD: MIApp.Sanitize.encodeProp($.trim($("#txtProcedureCode").val())),
                p_EPAL_PLAN_CD: MIApp.Sanitize.encodeProp($.trim($("#txtPlan").val())),
                p_EPAL_BUS_SEG_CD: MIApp.Sanitize.encodeProp($.trim($("#txtLOB").val())),
                p_EPAL_FUND_ARNGMNT_CD: MIApp.Sanitize.encodeProp($.trim($("#txtFundingArrangement").val())),
                p_EPAL_ENTITY_CD: MIApp.Sanitize.encodeProp($.trim($("#txtEntity").val())),
                p_DRUG_NM: MIApp.Sanitize.encodeProp($.trim($("#txtDrugName_Search").val())),
                p_OVERALL_EFF_DT_From: OVERALL_EFF_DT_From, //User Story 129226 MFQ
                p_OVERALL_EXP_DT_From: OVERALL_EXP_DT_From, //User Story 129226 MFQ
                p_EPAL_VER_EFF_DT: $.trim($("#dpEPALVerEffDate").val()), //User Story 129226 MFQ
                p_INCLUDE_HISTORICAL: $("#txtIncludeHistorical").val(), //User Story 129226 MFQ
                p_EPAL_STATUS: MIApp.Sanitize.encodeProp($.trim($("#txtEPALStatus").val())),
                p_STNDRD_SVC_CAT: MIApp.Sanitize.encodeProp($.trim($("#txtStandardCategory").val())),
                p_STNDRD_SVC_SUBCAT: MIApp.Sanitize.encodeProp($.trim($("#txtStandardSubCategory").val())),
                p_ALTRNT_SVC_CAT: MIApp.Sanitize.encodeProp($.trim($("#txtAlternateCategory").val())),
                p_ALTRNT_SVC_SUBCAT: MIApp.Sanitize.encodeProp($.trim($("#txtAlternateSubCategory").val())),
                p_SUSP_IND: MIApp.Sanitize.encodeProp($.trim($("#ddlSuspensionInd").val())),
                p_SUSP_TYPE: MIApp.Sanitize.encodeProp($.trim($("#ddlSuspensionType").val())),
                p_SUSP_EXP_DT: MIApp.Sanitize.encodeProp($.trim($("#dpSuspensionEffDt").val()))
            }            

            return paramObj;
        },

        onDataBound: function (e) {
            var appRoleIds = $("#txtAppRoleIds").val();
            var appRoleArray = []
            appRoleArray.push(appRoleIds.split(','))

            var gridRows = this.tbody.find("tr");
            gridRows.each(function (e) {
                var entity = $(this).find("#lblEntity").text();
                var bus_seg_cd = $(this).find("#lblbusSegCd").text();
                var is_Current = $(this).find("#lblIs_Current").text();
                var hierarchy = $(this).find("#lblHierarchy").text();
                var isDonorRecord = $(this).find("#lblIsDonorRecord").text();
                var product_cd = $(this).find("#lblEPAL_PRODUCT_CD").text();
                
                // DY: 01/28/2024 - Only allow Super Admins or EPAL Admins to see the pencil icon. 
                if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("4", appRoleArray[0]) !== -1)) {
                    $(this).closest('tr').find('.showPencil').show();
                }
                else { 

                    if (is_Current == 'Y' && hierarchy != 'Inactive') {

                            var UMREdit = false
                            if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("12", appRoleArray[0]) !== -1) || (jQuery.inArray("13", appRoleArray[0]) !== -1)) {
                                UMREdit = true
                            }

                            var EPALEdit = false
                            if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("3", appRoleArray[0]) !== -1) || (jQuery.inArray("4", appRoleArray[0]) !== -1)) {
                                EPALEdit = true
                            }

                            var CMPEdit = false
                            if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("19", appRoleArray[0]) !== -1) || (jQuery.inArray("20", appRoleArray[0]) !== -1)) {
                                CMPEdit = true
                            }

                            var DonorRecordsEdit = false
                            if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("23", appRoleArray[0]) !== -1) || (jQuery.inArray("24", appRoleArray[0]) !== -1)) {
                                DonorRecordsEdit = true
                            }

                            var ICHRAEdit = false
                            if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("27", appRoleArray[0]) !== -1) || (jQuery.inArray("28", appRoleArray[0]) !== -1)) {
                                ICHRAEdit = true
                            }

                            //==============
                            // UMR Records
                            //==============
                            if (entity === 'UMR' && (UMREdit === true)) {
                                $(this).closest('tr').find('.showPencil').show();
                            }

                            //==============
                            // ICHRA Records
                            //==============
                            else if (product_cd === 'ICHRA' && (ICHRAEdit === true)) {
                                $(this).closest('tr').find('.showPencil').show();
                            }

                            //==============
                            // Donor Records
                            //==============
                            else if (DonorRecordsEdit === true) {
                                $(this).closest('tr').find('.showPencil').show();
                            }

                            else if (entity !== 'UMR') {
                                //======================
                                // CMP_ or _CMP Records
                                //======================
                                if (bus_seg_cd.indexOf("CMP_") >= 0 || bus_seg_cd.indexOf("_CMP") >= 0) {
                                    if ((CMPEdit === true)) {
                                        $(this).closest('tr').find('.showPencil').show();
                                    }  
                                }
                                else {
                                    if ((EPALEdit === true)) {
                                        $(this).closest('tr').find('.showPencil').show();
                                    }                                   
                                }
                               
                            }
                            else {
                                $(this).closest('tr').find('.showPencil').hide();
                            }
                 
                    }
                    else {
                       
                        $(this).closest('tr').find('.showPencil').hide();
                    }

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
            //Search screen export column not mapping correctly -  BUG 68580
            e.workbook.sheets[0].rows[0].cells[1].value = 'Procedure Code';
            e.workbook.sheets[0].rows[0].cells[2].value = 'Business Segment';
            e.workbook.sheets[0].rows[0].cells[9].value = 'Alternate Category';
            e.workbook.sheets[0].rows[0].cells[10].value = 'Alternate Sub Category';
            e.workbook.sheets[0].rows[0].cells[11].value = 'EPAL Version Eff Dt';
            e.workbook.sheets[0].rows[0].cells[12].value = 'Overall Eff Dt';
            e.workbook.sheets[0].rows[0].cells[13].value = 'Overall Exp Dt';

            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];
                row.cells[11].type = "date";
                row.cells[11].format = 'MM/dd/yyyy hh:mm:ss';
                row.cells[11].value = kendo.toString(kendo.parseDate(row.cells[11].value), 'MM/dd/yyyy hh:mm:ss tt');
            };
        },
        onGridRequestEnd: function (e) {
            window.kendo.ui.progress($("#grid_Search"), false);
        }
    },
    methods: {

        redirect: function (pims_id) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.EPALSearch")) {
                const logProperties = new LogInfo("Search.redirect", MS_ID, pims_id);
                appInsightsClient.trackEvent('Client.EPALSearch.ViewDetail', logProperties);
            }

            var trg_url = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);

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
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                    }
                });
            }

            windowPopup("_blank", trg_url, null, swalRedirect);
        },
        redirect_edit: function (pims_id) {
            const appInsightsClient = new TelemetryLogger();
            if (appInsightsClient.isAppInsightsEnabled("Client.EPALSearch")) {
                const logProperties = new LogInfo("Search.redirect_edit", MS_ID, pims_id);
                appInsightsClient.trackEvent('Client.EPALSearch.EditDetail', logProperties);
            }

            var trg_url = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);

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