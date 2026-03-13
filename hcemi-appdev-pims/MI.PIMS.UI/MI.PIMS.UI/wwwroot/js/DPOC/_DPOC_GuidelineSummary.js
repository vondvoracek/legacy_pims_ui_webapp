var counter = 0;
var isActiveRetGuidelineWidgetWindowOpen = false;
var MedAssCovSumTitle_New = "";
var DPOC_GuidelineSummaryController = {
    cacheGridData: function (gridId) {
        var grid = $("#" + gridId).data("kendoGrid");
        if (!grid) return;

        if (grid.dataSource.data().length > 0) {
            var key = DPOC_GuidelineSummaryController.getGridCacheKey();

            // Extract key parts for filtering
            var guidelineId = key.split('^^')[0];
            var release = key.split('^^')[1];
            var version = key.split('^^')[2];

            // Filter data based on key info
            var filteredData = grid.dataSource.data().filter(function (item) {
                return item.DTQ_IQ_GDLN_ID === guidelineId &&
                    item.DPOC_RELEASE === release &&
                    item.DPOC_VER_NUM === version;
            });

            var jsonData = filteredData.map(function (item) {
                return item.toJSON();
            });

            if (gridId === 'grid_config') {
                gridConfigDataCache[key] = jsonData;
            } else {
                gridDiagDataCache[key] = jsonData;
            }
        }
    },
    getGridCacheKey: function () {
        var guidelineId = $('#txtIQ_GDLN_ID_GS').val() || $('#txtIQ_GDLN_ID_GS_hdn').val();
        var release = $('#txtDPOCRelease_GS').data('release') || $('#txtDPOCRelease_GS_hdn').val();
        var version = $('#txtDPOC_VER_NUM_GS').data('version') || $('#txtDPOC_VER_NUM_hdn').val();

        return `${guidelineId}^^${release}^^${version}`;
    },
    tab: {
        select: function (e) {
            switch ($(e.item).find("> .k-link").text()) {
                case 'Guideline Configuration':
                    // Before switching tabs, cache current grid data
                    DPOC_GuidelineSummaryController.cacheGridData('grid_config');


                    gsWindow.setOptions({
                        height: "666px", // full browser width
                        width: "1830px"
                    });
                    gsWindow.center(); // optional: re-center vertically


                    $("#gsTabstrip .k-content").css({
                        height: "auto",
                        minHeight: "592px"
                    });

                    $("#windowLoader").css({
                        width: gsWindow.wrapper.width(),
                        height: gsWindow.wrapper.height(),
                    });

                    //DPOC_ConfigController.loadData(false);
                    break;
                case 'Diagnosis Code':
                    // Before switching tabs, cache current grid data
                    DPOC_GuidelineSummaryController.cacheGridData('grid_GSDiagList');

                    gsWindow.setOptions({
                        height: "666px", // full browser width
                        width: "1830px"
                    });
                    gsWindow.center(); // optional: re-center vertically

                    $("#gsTabstrip .k-content").css({
                        height: "auto",
                        minHeight: "592px"
                    });

                    $("#windowLoader").css({
                        width: gsWindow.wrapper.width(),
                        height: gsWindow.wrapper.height(),
                    });


                    //DiagCodesController.loadData(false);
                    break;
                default:

                    gsWindow.setOptions({
                        height: "940px", // full browser width
                        width: "1024px"
                    });
                    gsWindow.center(); // optional: re-center vertically

                    // Before switching tabs, cache current grid data
                    DPOC_GuidelineSummaryController.cacheGridData('grid_config');
                    // Before switching tabs, cache current grid data
                    DPOC_GuidelineSummaryController.cacheGridData('grid_GSDiagList');
                    break;
            }
        },
        loaded: function (e) {
            if (e.item.innerText == 'Guideline Configuration') {
                DPOC_ConfigController.loadData(false);
            }
            if (e.item.innerText == 'Diagnosis Code') {
                DiagCodesController.loadData(false);
            }
        },
        activate: function (e) {
            switch ($(e.item).find("> .k-link").text()) {
                case 'Diagnosis Code':
                    DiagCodesController.loadData(false);
                    break;
                case 'State':
                    StateInfoController.loadData(false);
                    break;
                default:
                    break;
            }
        }
    },
    bind: function () {

        DPOC_GuidelineSummaryController.grid.hideAvailableGuidelines();

        gsWindow.init();

        $("#btnAvailableGuidelines").click(function () {
            DPOC_GuidelineSummaryController.grid.toggleAvailableGuidelines();
        });

        $("#grid_guideline_summary_active").on("click", ".gdln-summary-active-delete", function () {
            DPOC_GuidelineSummaryController.grid.deleteRow(this);
        });
        $('.bs-dpoc-guideline-summary-widget-modal-save').on('click', function () {
            DPOC_GuidelineSummaryController.save();
        });
    },
    controls: {
        param: {
            // Bug 139250 MFQ 10/7/2025
            GSJurisdiction: function () {
                const businessSegment = duplicateDetail
                    ? $("#ddlBusinessSegment").val()
                    : $("#txtBusinessSegment").val();

                const entity = duplicateDetail
                    ? $("#ddlEntity").val()
                    : $("#txtEntity").val();

                const busSegCd = (businessSegment === "CnS")
                    ? `${businessSegment}-${entity}`
                    : "MnR";

                return {
                    p_VV_SET_NAME: "DPOC_JURISDICTIONS",
                    p_BUS_SEG_CD: busSegCd
                };
            },
            GSIQCriteria: function () {
                return {
                    p_VV_SET_NAME: "DPOC_IQ_CRITERIA",
                    p_BUS_SEG_CD: null
                }
            },
            GS_MedicalPolicy: function () {
                if (duplicateDetail) {
                    DPOC_BUS_SEG_CD = $("#ddlBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#ddlEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }
                else {
                    DPOC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#txtEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }

                return {
                    P_DPOC_BUS_SEG_CD: DPOC_BUS_SEG_CD,
                    P_DPOC_ENTITY_CD: DPOC_ENTITY_CD,
                    P_PROC_CD: PROC_CD
                }
            },
            GSSOSProvTINExl: function () {
                return {
                    p_VV_SET_NAME: "DPOC_SOS_PROVIDER_TIN_EXCL",
                    p_BUS_SEG_CD: null
                    /*
                    p_text: '',
                    p_column_name: 'DPOC_SOS_PROVIDER_TIN_EXCL',
                    p_parent_category: null,
                    p_parent_category_val: null*/
                }
            },
            KL_PLCY_NM: function () {
                if (duplicateDetail) {
                    DPOC_BUS_SEG_CD = $("#ddlBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#ddlEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }
                else {
                    DPOC_BUS_SEG_CD = $("#txtBusinessSegment").val();
                    DPOC_ENTITY_CD = $("#txtEntity").val();
                    PROC_CD = $("#txtProcedureCode").val();
                }

                return {
                    p_dpoc_bus_seg_cd: DPOC_BUS_SEG_CD,
                    p_dpoc_entity_cd: DPOC_ENTITY_CD,
                    p_proc_cd: PROC_CD,
                    p_plcy_type_cd: "p_plcy_type_cd"
                }
            }
        },
        /**
         * OnChange of Medicare Associated Coverage Summary Title
         */
        onChangeKL_PLCY_NM: function () {

            var MedAssCovSumTitle = $('#ddlMedAssCovSumTitle').data('kendoComboBox').text();
            var MedAssCovSumId = $('#ddlMedAssCovSumTitle').data('kendoComboBox').value();

            $('#txtMedAssCovSumID').val(MedAssCovSumId);

            if (MedAssCovSumTitle.length > 0) {
                if (MedAssCovSumId == MedAssCovSumTitle) {
                    $('#txtMedAssCovSumID').val('');
                    $('#txtMedAssCovSumID').prop('disabled', false);
                    $('#txtMedAssCovSumID').focus();
                    MedAssCovSumTitle_New = MedAssCovSumTitle;
                }
                else {
                    $('#txtMedAssCovSumID').prop('disabled', true);
                    MedAssCovSumTitle_New = MedAssCovSumTitle;
                }
            }

            else if (MedAssCovSumTitle.length == 0) {
                $('#txtMedAssCovSumID').prop('disabled', true);
                $('#txtMedAssCovSumID').val('');
            }
        },

        onGS_MedicalPolicyChange: function (e) {
            var $ddlGS_MedicalPolicyID = $("#ddlGS_MedicalPolicyID").data("kendoComboBox");
            var $ddlGS_MedicalPolicy = $("#ddlGS_MedicalPolicy").data("kendoComboBox");

            var selectedText = $ddlGS_MedicalPolicy.text();
            var dataItem = $ddlGS_MedicalPolicyID.dataSource.data().find(function (item) {
                return item.PLCY_NM === selectedText;
            });

            if (dataItem) {
                $ddlGS_MedicalPolicyID.value(dataItem.PLCY_POLICY_ID);
            }

            if ($ddlGS_MedicalPolicy.value() == '') {
                $ddlGS_MedicalPolicyID.enable(false);
                $ddlGS_MedicalPolicyID.value(null);
            } else {
                $ddlGS_MedicalPolicyID.enable(true);
            }
        },
        onGS_MedicalPolicyIDChange: function (e) {
            var $ddlGS_MedicalPolicyID = $("#ddlGS_MedicalPolicyID").data("kendoComboBox");
            var $ddlGS_MedicalPolicy = $("#ddlGS_MedicalPolicy").data("kendoComboBox");

            var selectedText = $ddlGS_MedicalPolicyID.text();
            var dataItem = $ddlGS_MedicalPolicy.dataSource.data().find(function (item) {
                return item.PLCY_POLICY_ID === selectedText;
            });

            if (dataItem) {
                $ddlGS_MedicalPolicy.value(dataItem.PLCY_POLICY_ID);
            }
        },
        onInPatOutcomeChange: function (e) {
            var $InPatTypeRule = $("#ddlGSInPatTypeRule").data("kendoDropDownList");
            $InPatTypeRule.value($("#ddlGSInPatOutcome").data("kendoDropDownList").text());
        },
        onInPatTypeRuleChange: function (e) {
            var $InPatOutcome = $("#ddlGSInPatOutcome").data("kendoDropDownList");
            $InPatOutcome.value($("#ddlGSInPatTypeRule").data("kendoDropDownList").text());
        },
        onOutPatOutcomeChange: function (e) {
            var $OutPatTypeRule = $("#ddlGSOutPatTypeRule").data("kendoDropDownList");
            $OutPatTypeRule.value($("#ddlGSOutPatOutcome").data("kendoDropDownList").text());
        },
        onOutPatTypeRuleChange: function (e) {
            var $OutPatOutcome = $("#ddlGSOutPatOutcome").data("kendoDropDownList");
            $OutPatOutcome.value($("#ddlGSOutPatTypeRule").data("kendoDropDownList").text());
        },
        onOutPatFacOutcomeChange: function (e) {
            var $OutPatFacTypeRule = $("#ddlGSOutPatFacTypeRule").data("kendoDropDownList");
            $OutPatFacTypeRule.value($("#ddlGSOutPatFacOutcome").data("kendoDropDownList").text());
        },
        onOutPatFacTypeRuleChange: function (e) {
            var $OutPatFacOutcome = $("#ddlGSOutPatFacOutcome").data("kendoDropDownList");
            $OutPatFacOutcome.value($("#ddlGSOutPatFacTypeRule").data("kendoDropDownList").text());
        }
    },
    grid: {
        guideline_summary_active: {
            data: {
                IQ_GDLN_ID: [],
                IQ_GDLN_VERSION: [],
                IQ_GDLN_NM: [],
                IQ_REFERENCE: [],
                IQ_GDLN_PRODUCT_NM: [],
                IQ_GDLN_PRODUCT_DESC: [],
                IQ_GDLN_REL_DT: [],
                IQ_GDLN_EXP_DT: [],
                IQ_GDLN_DESC: [],
                IQ_GDLN_RECOMMENDATION_DESC: [],
                IQ_GDLN_JRSDCTN: [],
                IQ_CRITERIA: [],
                RULE_OUTCOME_OUTPAT: [],
                RULE_OUTCOME_OUTPAT_RSN: [],
                RULE_OUTCOME_OUTPAT_FCLTY: [],
                RULE_OUTCOME_OUTPAT_FCLTY_RSN: [],
                RULE_OUTCOME_INPAT: [],
                RULE_OUTCOME_INPAT_RSN: [],
                RULE_COMMENTS: [],
                RULE_IMP_TYPE: [],
                RULE_IMP_WITH: [],
                RULE_EXCLUSIONS: [],
                GDLN_ASSOC_EFF_DT: [],
                GDLN_ASSOC_EXP_DT: [],
                KL_PLCY_ID: [], // Phase III - future use
                KL_PLCY_NAME: [],
                MDCR_COVG_SUM_ID: [],
                MDCR_COVG_SUM_TITLE: []
            },
            validation: {
                init: function () {
                    var _data = DPOC_GuidelineSummaryController.grid.guideline_summary_active.data;
                    var validationGSSaveText = ''

                    // reset all fields
                    Object.keys(_data).forEach(key => {
                        _data[key] = []
                    });

                    var grid_guideline_summary_activeData = $("#grid_guideline_summary_active").data("kendoGrid").dataSource.view();
                    jQuery.each(grid_guideline_summary_activeData, function (index, gdl) {

                        validationGSSaveText = DPOC_GuidelineSummaryController.validation(gdl.GDLN_ASSOC_EFF_DT, gdl.GDLN_ASSOC_EXP_DT, gdl.IQ_GDLN_STATUS, gdl.IQ_GDLN_VERSION, gdl.IQ_CRITERIA);

                    });

                    return validationGSSaveText;
                }
            }
        },
        onDataBinding: function () {
            kendo.ui.progress(this.wrapper, true);
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");

            //fill guideline summary data on page load in local memory
            if (DetailController.grid.guideline_summary.data.length == 0) {
                var data = e.sender.dataSource.data();

                DetailController.grid.guideline_summary.data = data.map(function (gl) {
                    return {
                        DPOC_HIERARCHY_KEY: gl.DPOC_HIERARCHY_KEY,
                        DPOC_VER_EFF_DT: gl.DPOC_VER_EFF_DT,
                        DPOC_PACKAGE: gl.DPOC_PACKAGE,
                        IQ_CRITERIA: gl.IQ_CRITERIA,
                        IQ_GDLN_ID: gl.IQ_GDLN_ID,
                        IQ_GDLN_STATUS: gl.IQ_GDLN_STATUS,
                        IQ_GDLN_NM: gl.IQ_GDLN_NM,
                        IQ_GDLN_PRODUCT_DESC: gl.IQ_GDLN_PRODUCT_DESC,
                        IQ_GDLN_PRODUCT_NM: gl.IQ_GDLN_PRODUCT_NM,
                        IQ_GDLN_DESC: gl.IQ_GDLN_DESC,
                        IQ_GDLN_RECOMMENDATION_DESC: gl.IQ_GDLN_RECOMMENDATION_DESC,
                        IQ_REFERENCE: gl.IQ_REFERENCE,
                        IQ_GDLN_REL_DT: gl.IQ_GDLN_REL_DT,
                        IQ_GDLN_EXP_DT: gl.IQ_GDLN_EXP_DT,
                        MDCR_COVG_SUM_ID: gl.MDCR_COVG_SUM_ID,
                        MDCR_COVG_SUM_TITLE: gl.MDCR_COVG_SUM_TITLE,
                        IQ_GDLN_RULES_SYS_SEQ: gl.IQ_GDLN_RULES_SYS_SEQ,
                        IQ_GDLN_VERSION: gl.IQ_GDLN_VERSION,
                        IQ_GDLN_JRSDCTN: gl.IQ_GDLN_JRSDCTN,
                        RULE_IMP_TYPE: gl.RULE_IMP_TYPE,
                        RULE_IMP_WITH: gl.RULE_IMP_WITH,
                        RULE_TYPE_OUTPAT: gl.RULE_TYPE_OUTPAT,
                        RULE_TYPE_OUTCOME_OUTPAT: gl.RULE_TYPE_OUTCOME_OUTPAT,
                        RULE_TYPE_RSN_OUTPAT: gl.RULE_TYPE_RSN_OUTPAT,
                        RULE_TYPE_OUTPAT_FCLTY: gl.RULE_TYPE_OUTPAT_FCLTY,
                        RULE_TYPE_OUTCOME_OUTPAT_FCLTY: gl.RULE_TYPE_OUTCOME_OUTPAT_FCLTY,
                        RULE_TYPE_RSN_OUTPAT_FCLTY: gl.RULE_TYPE_RSN_OUTPAT_FCLTY,
                        RULE_TYPE_INPAT: gl.RULE_TYPE_INPAT,
                        RULE_TYPE_OUTCOME_INPAT: gl.RULE_TYPE_OUTCOME_INPAT,
                        RULE_TYPE_RSN_INPAT: gl.RULE_TYPE_RSN_INPAT,
                        RULE_COMMENTS: gl.RULE_COMMENTS,
                        RULE_EXCLUSIONS: gl.RULE_EXCLUSIONS,
                        KL_PLCY_ID: gl.KL_PLCY_ID,
                        KL_PLCY_NAME: gl.KL_PLCY_NAME,
                        DTQ_APPLIES: gl.DTQ_APPLIES,
                        GDLN_ASSOC_EFF_DT: gl.GDLN_ASSOC_EFF_DT,
                        GDLN_ASSOC_EXP_DT: gl.GDLN_ASSOC_EXP_DT,
                        JRSDCTN_DPOC_RELEASE: gl.DPOC_RELEASE,
                        JRSDCTN_DPOC_VER_NUM: gl.DPOC_VER_NUM,
                        JRSDCTN_NM: gl.JRSDCTN_NM,
                        JRSDCTN_IQ_GDLN_ID: gl.IQ_GDLN_ID,
                        JRSDCTN_IND: gl.JRSDCTN_IND,
                        HOLDING_DTQ: gl.HOLDING_DTQ,
                        HOLDING_DTQ_VERSION: gl.HOLDING_DTQ_VERSION,
                        TGT_DTQ: gl.TGT_DTQ,
                        TGT_DTQ_VERSION: gl.TGT_DTQ_VERSION,
                        DTQ_RSN: gl.DTQ_RSN,
                        GDLN_AGE_MIN: gl.GDLN_AGE_MIN,
                        GDLN_AGE_MAX: gl.GDLN_AGE_MAX,
                        STATES_APPLY: gl.STATES_APPLY,
                        STATES_APPL: gl.STATES_APPL,
                        DX_APPLY: gl.DX_APPLY,
                        POS_APPLY: gl.POS_APPLY,
                        DTQ_APPLY: gl.DTQ_APPLY,
                        DPOC_SOS_PROVIDER_TIN_EXCL: gl.DPOC_SOS_PROVIDER_TIN_EXCL,
                        DPOC_RELEASE: gl.DPOC_RELEASE,
                        DPOC_VER_NUM: gl.DPOC_VER_NUM,
                        PKG_CONFIG_COMMENTS: gl.PKG_CONFIG_COMMENTS
                    };
                });
            }

            if (typeof GridLoadTracker !== "undefined" && typeof GridLoadTracker.markLoaded === "function") {
                GridLoadTracker.markLoaded("grid_guideline_summary_active");
            }

            //User Story 132102 Hover configuration
            //$(".gsDXHover, .gsConfigHover").off('hover', hoverPopup)
            $(".diag,.pos,.dtq,.hv-state").on('mouseenter', function (e) {

                //check if guideline widget is already open then return                
                if (isActiveRetGuidelineWidgetWindowOpen) {
                    return;
                }

                var grid = $("#grid_guideline_summary_active").data("kendoGrid");
                var dataItem = grid.dataItem(this.closest("tr"));

                $('#txtIQ_GDLN_ID_GS_hdn').val(dataItem.IQ_GDLN_ID);
                $('#txtDPOCRelease_GS_hdn').val(dataItem.DPOC_RELEASE);
                $('#txtDPOC_VER_NUM_hdn').val(dataItem.DPOC_VER_NUM);

                const $cell = $(this);
                const $spinner = $cell.find(".spinner-" + this.className);
                $spinner.show();

                if (this.className == 'pos' || this.className == 'dtq') {
                    gsHoverWindow.param.type = this.className;
                    setHoverWindowParams(gsHoverWindow, this, guidelineConfigurationHoverUrl, {
                        p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string(p_DPOC_HIERARCHY_KEY),
                        p_DPOC_VER_EFF_DT: MIApp.Sanitize.string(p_DPOC_VER_EFF_DT),
                        p_DPOC_PACKAGE: MIApp.Sanitize.string(p_DPOC_PACKAGE),
                        p_DPOC_RELEASE: MIApp.Sanitize.string(dataItem.DPOC_RELEASE),
                        p_DPOC_VER_NUM: MIApp.Sanitize.string(dataItem.DPOC_VER_NUM),
                        p_IQ_GDLN_ID: MIApp.Sanitize.string(dataItem.IQ_GDLN_ID)
                    }, $spinner);
                }
                else if (this.className == 'diag') {
                    gsHoverWindow.param.type = this.className;
                    setHoverWindowParams(gsHoverWindow, this, diagViewLoadInHoverWindowUrl, {
                        p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string(p_DPOC_HIERARCHY_KEY),
                        p_DPOC_VER_EFF_DT: MIApp.Sanitize.string(p_DPOC_VER_EFF_DT),
                        p_DPOC_PACKAGE: MIApp.Sanitize.string(p_DPOC_PACKAGE),
                        p_DPOC_RELEASE: MIApp.Sanitize.string(dataItem.DPOC_RELEASE),
                        p_DPOC_VER_NUM: MIApp.Sanitize.string(dataItem.DPOC_VER_NUM),
                        p_IQ_GDLN_ID: MIApp.Sanitize.string(dataItem.IQ_GDLN_ID)
                    }, $spinner);
                } else if (this.className.indexOf('hv-state') > -1) {
                    gsHoverWindow.param.type = this.className;
                    setHoverWindowParams(gsHoverWindow, this, statesViewLoadInHoverWindowUrl, {
                        p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string(p_DPOC_HIERARCHY_KEY),
                        p_DPOC_VER_EFF_DT: MIApp.Sanitize.string(p_DPOC_VER_EFF_DT),
                        p_DPOC_PACKAGE: MIApp.Sanitize.string(p_DPOC_PACKAGE),
                        p_DPOC_RELEASE: MIApp.Sanitize.string(dataItem.DPOC_RELEASE),
                        p_DPOC_VER_NUM: MIApp.Sanitize.string(dataItem.DPOC_VER_NUM),
                        p_IQ_GDLN_ID: MIApp.Sanitize.string(dataItem.IQ_GDLN_ID)
                    }, $spinner);
                }
            });

            $(".diag,.pos,.dtq,.hv-state, .k-window").on('mouseleave', function (e) {

                clearTimeout(hoverTimeout);

                // Delay to allow moving into the window without it closing immediately
                var hCtrlName = this.className;
                const hoveredElem = this;
                setTimeout(function () {
                    if (hCtrlName.indexOf('hv-state') > -1 || hCtrlName.indexOf('k-window') > -1) {
                        if (!$(hoveredElem).is(":hover") && !$(gsHoverWindow)[0].wrapper.is(":hover")) { //&& !$(gsStateWindow)[0].wrapper.is(":hover")
                            gsHoverWindow.close();
                            //gsHoverWindow.setOptions({ modal: true });
                        }
                    } else {
                        if (!$(gsHoverWindow)[0].wrapper.is(":hover")) { //&& !gsWindow.is(":hover")
                            gsHoverWindow.close();
                            //gsHoverWindow.setOptions({ modal: true });
                        }
                    }

                }, 300);
                const $cell = $(this);
                const $spinner = $cell.find(".spinner-" + this.className);
                $spinner.hide();
            });

            let hoverTimeout;

            function setHoverWindowParams(kWindow, e, url, _data, $spinner) {

                hoverTimeout = setTimeout(async () => {

                    var offset = $(e).offset();
                    var windowWidth = kWindow.wrapper.outerWidth();
                    var windowHeight = kWindow.wrapper.outerHeight();
                    var windowTop = ($(window).height() / 2) - (windowHeight / 2); // center vertically gsHoverWindow
                    kWindow.setOptions({ modal: false });
                    kWindow.refresh({
                        url: url,
                        data: _data
                    }).open();

                    kWindow.wrapper.css({
                        top: windowTop + $(window).scrollTop(), // vertical center with scroll support
                        left: offset.left - windowWidth - 10 // 10px to the left of the label
                    });

                    $spinner.hide();

                }, 500);
            }

            kendo.ui.progress(this.wrapper, false);
        },
        pendingOnDataBound: function (e) {
            //var grid = e.sender.wrapper;
            //var gridContent = e.sender.wrapper.find(".k-grid-content");
            //grid.height("414px");
            //gridContent.height("310px");
        },
        paramsPending: function () {
            if (duplicateDetail) {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#txtDPOCPackageOld").val()),
                    //p_DPOC_RELEASE: MIApp.Sanitize.string($("#txtDPOCReleaseOld").val())
                }
            }
            else {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#ddlDPOCPackage").val()),
                    //p_DPOC_RELEASE: MIApp.Sanitize.string($("#txtDPOCRelease").val())
                }
            }
        },
        paramsActive: function () {
            if (duplicateDetail) {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#txtDPOCPackageOld").val()),
                    //p_DPOC_RELEASE: MIApp.Sanitize.string($("#txtDPOCReleaseOld").val()),
                    p_IQ_GDLN_STATUS: 'Active',
                    P_IS_HISTORICAL: $("#isCurrentRecord").val() === 'Y' ? 0 : 1
                }
            } else {
                return {
                    p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.string($('#txtPIMSID').val()),
                    p_DPOC_VER_EFF_DT: MIApp.Sanitize.string($('#txtDPOCVersionDt').val()),
                    p_DPOC_PACKAGE: MIApp.Sanitize.string($("#ddlDPOCPackage").val()),
                    //p_DPOC_RELEASE: MIApp.Sanitize.string($("#txtDPOCRelease").val()),
                    p_IQ_GDLN_STATUS: 'Active',
                    P_IS_HISTORICAL: $("#isCurrentRecord").val() === 'Y' ? 0 : 1
                }
            }
        },
        edit_redirect: function (row, popupType, tab) {
            openGuidelinePopup(row, popupType, 'detail');
        },
        deleteRow: function (delete_button) {
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

                    var gridPendingGS = $("#grid_guideline_summary").data("kendoGrid");
                    var dataItem = gridPendingGS.dataItem(row);
                    if (dataItem != undefined) {
                        if (dataItem.PendingRowId != '') {
                            gridPendingGS.tbody.find("tr[data-uid=" + gridPendingGS.dataItem(row).PendingRowId + "]").show();
                            gridPendingGS.refresh();
                        }
                    }

                    $("#grid_guideline_summary_active").data("kendoGrid").removeRow(row);
                    $("#grid_guideline_summary_active").data("kendoGrid").refresh();

                    refreshGdlnDataAfterDelete();

                    DetailController.dirtyform.dirtycheck();
                }
            });
        },
        toggleAvailableGuidelines: function () {
            counter += 1
            $("#divAvailableGuidelines").toggle('highlight');
            highlight_guidelines();
            if (counter % 2 == 0) {
                $("#icon-down").show();
                $("#icon-up").hide();
            }
            else {
                $("#icon-up").show();
                $("#icon-down").hide();
            }

            function highlight_guidelines() {
                $('#divAvailableGuidelines').addClass("highlight-anim");
                setTimeout(function () {
                    $('#divAvailableGuidelines').removeClass('highlight-anim');
                }, 2000);
            }
        },

        hideAvailableGuidelines: function () {
            $("#divAvailableGuidelines").hide();
            $("#icon-up").hide();
        },
        show_diagTab: function (row, popupType, tab) {
            openGuidelinePopup(row, popupType, 'diag');
        },
        show_posTab: function (row, popupType, tab) {
            openGuidelinePopup(row, popupType, 'pos');
        },
        show_dtqTab: function (row, popupType, tab) {
            openGuidelinePopup(row, popupType, 'dtq');
        },
        show_stateTab: function (row, popupType, tab) {
            openGuidelinePopup(row, popupType, 'state');
        }
    },

    validation: function (dpGS_StartDate, dpGS_EndDate, IQ_GDLN_STATUS, GS_IQ_Version, GS_IQCriteria, DPOC_RELEASE, DPOC_VER_NUM) {
        var validationGSSaveText = '';
        var isGS_StartDateValid = dpGS_StartDate != "" && Date.parse(dpGS_StartDate)
        var isGS_EndDateValid = dpGS_EndDate != "" && Date.parse(dpGS_EndDate)

        if (IQ_GDLN_STATUS == 'Active' && DPOC_RELEASE == '') {
            validationGSSaveText = 'DPOC Release is mandatory on Active record!\n';
        }

        if (IQ_GDLN_STATUS == 'Active' && DPOC_VER_NUM == '') {
            validationGSSaveText = 'DPOC Version Number is mandatory on Active record!\n';
        }

        const currentRowId = $('#txtDataRowId_GS').val();
        const guidelineId = $('#txtIQ_GDLN_ID_GS').val();

        const grid = $("#grid_guideline_summary_active").data("kendoGrid");
        const data = grid.dataSource.view();

        // DevOps 135572 remove this condition

        const isDuplicateVerNum = data.some(item =>
            item.uid !== currentRowId &&
            item.IQ_GDLN_ID === guidelineId &&
            item.DPOC_RELEASE === DPOC_RELEASE &&
            item.DPOC_VER_NUM === DPOC_VER_NUM
        );

        if (isDuplicateVerNum) {
            validationGSSaveText = "Duplicate Entry. Guideline ID, DPOC Release and DPOC Release Version already exist in active guidelines!";
        }


        if (IQ_GDLN_STATUS == 'Active' && $.trim(dpGS_StartDate) == '') {
            validationGSSaveText += 'Guideline Start date is mandatory on Active record!\n';
        }

        if (isGS_StartDateValid && isGS_EndDateValid) {
            if (Date.parse(dpGS_StartDate) > Date.parse(dpGS_EndDate)) {
                validationGSSaveText += "Guideline Start Date cannot be greater than Guideline End Date (Guideline Summary tab)!\n";
            }
            else if (Date.parse(dpGS_StartDate) == Date.parse(dpGS_EndDate)) {
                validationGSSaveText += "Guideline End Date must be greater than Guideline Start Date (Guideline Summary tab)!\n";
            }
        }

        // Bug 136738 MFQ 7/24/2025
        //if (IQ_GDLN_STATUS == 'Active' && $.trim(GS_IQ_Version) == '') {
        //    validationGSSaveText += 'IQ Version is mandatory on Active record!\n';
        //}

        // Bug 136738 MFQ 7/24/2025
        //if (IQ_GDLN_STATUS == 'Active' && $.trim(GS_IQCriteria) == '') {
        //    validationGSSaveText += 'IQ Criteria is mandatory on Active record!\n';
        //}

        if (parseInt($('#txtGS_AgeMax').data("kendoDropDownList").value()) < parseInt($('#txtGS_AgeMin').data("kendoDropDownList").value())) {
            validationGSSaveText += 'Guideline Age Max should be greater than Guideline Age Min!\n';
        }

        return validationGSSaveText;
    },
    save: function () {
        // validation

        var validationGSSaveText = '';
        validationGSSaveText = DPOC_GuidelineSummaryController.validation(
            kendo.parseDate($('#dpGS_StartDate').val(), 'MM/dd/yyyy'),
            kendo.parseDate($('#dpGS_EndDate').val(), 'MM/dd/yyyy'),
            $('#txtIQ_GDLN_STATUS_GS').val(),
            $('#txtGS_IQ_Version').val(),
            $('#ddlGS_IQCriteria').data("kendoDropDownList").value(),
            $('#txtDPOCRelease_GS').val(),
            $('#txtDPOC_VER_NUM_GS').val(),
        );

        if (validationGSSaveText.length > 0) {
            validationGSSaveText = validationGSSaveText;
        }

        var posValidation = DetailController.grid.config.validation('grid_config');
        if ($.trim(posValidation) != '' && $.trim(posValidation).length > 0) {
            validationGSSaveText += posValidation;
        } else {
            DPOC_ConfigController.addData();
        }

        var diagCodesValidation = DetailController.grid.diags.validation('grid_GSDiagList');
        if ($.trim(diagCodesValidation) != '' && $.trim(diagCodesValidation).length > 0) {
            validationGSSaveText += diagCodesValidation;
        } else {
            DiagCodesController.addData();
        }

        if (validationGSSaveText.length > 0) {
            DetailController.message.showWarning("<div style='text-align: left;'><ul style='list-style-type: square;'><li> " + validationGSSaveText.replace(/\n$/, "").replace(/\n/g, "</li><li>") + "</li></ul></div>");
            return;
        }

        var popupType = $('#txtGDLNPopupType').val();

        const newActiveGuideline = buildNewActiveGuideline();
        updateGridWithGuideline(popupType, newActiveGuideline);

        function buildNewActiveGuideline() {
            const jurisdictionValues = $("#msGSJurisdiction").data("kendoMultiSelect").value();

            return {
                DPOC_RELEASE: getTextBoxValue('txtDPOCRelease_GS'),
                DPOC_VER_NUM: getTextBoxValue('txtDPOC_VER_NUM_GS'),
                IQ_REFERENCE: getTextBoxValue('txtGS_IQ_Reference'),
                IQ_GDLN_VERSION: getTextBoxValue('txtGS_IQ_Version'),
                IQ_CRITERIA: getDropDownSelectedValue('ddlGS_IQCriteria'),
                GDLN_ASSOC_EFF_DT: getDatePickerValue('dpGS_StartDate'),
                GDLN_ASSOC_EXP_DT: getDatePickerValue('dpGS_EndDate'),

                //User Story 137657
                MDCR_COVG_SUM_ID: getTextBoxValue('txtMedAssCovSumID'),
                MDCR_COVG_SUM_TITLE: MedAssCovSumTitle_New.length > 0 ? MedAssCovSumTitle_New : getComboBoxValue('ddlMedAssCovSumTitle'),

                KL_PLCY_NAME: getComboBoxValue('ddlGS_MedicalPolicy'),
                KL_PLCY_ID: getComboBoxValue('ddlGS_MedicalPolicyID'),

                IQ_GDLN_JRSDCTN: null,
                RULE_EXCLUSIONS: getTextBoxValue('txtGS_Exclusion'),
                PKG_CONFIG_COMMENTS: getTextBoxValue('txtGS_PackageConfigComments'),

                RULE_TYPE_INPAT: getDropDownSelectedValue('ddlGSInPatOutcome'),
                RULE_TYPE_OUTCOME_INPAT: getDropDownSelectedValue('ddlGSInPatTypeRule'),
                RULE_TYPE_RSN_INPAT: getDropDownSelectedValue('ddlGSInPatReason'),
                RULE_TYPE_OUTPAT: getDropDownSelectedValue('ddlGSOutPatOutcome'),
                RULE_TYPE_OUTCOME_OUTPAT: getDropDownSelectedValue('ddlGSOutPatTypeRule'),
                RULE_TYPE_RSN_OUTPAT: getDropDownSelectedValue('ddlGSOutPatReason'),
                RULE_TYPE_OUTPAT_FCLTY: getDropDownSelectedValue('ddlGSOutPatFacOutcome'),
                RULE_TYPE_OUTCOME_OUTPAT_FCLTY: getDropDownSelectedValue('ddlGSOutPatFacTypeRule'),
                RULE_TYPE_RSN_OUTPAT_FCLTY: getDropDownSelectedValue('ddlGSOutPatFacReason'),

                DPOC_HIERARCHY_KEY: getTextBoxValue('txtDPOC_HIERARCHY_KEY_GS'),
                DPOC_VER_EFF_DT: getTextBoxValue('txtDPOC_VER_EFF_DT_GS'),
                DPOC_PACKAGE: getTextBoxValue('txtDPOC_PACKAGE_GS'),
                IQ_GDLN_ID: getTextBoxValue('txtIQ_GDLN_ID_GS'),
                IQ_GDLN_STATUS: getTextBoxValue('txtIQ_GDLN_STATUS_GS'),
                IQ_GDLN_REL_DT: kendo.toString(kendo.parseDate(getTextBoxValue('txtIQ_GDLN_REL_DT_GS')), 'MM/dd/yyyy'),
                PendingRowId: getTextBoxValue('txtDataRowId_GS'),
                IQ_GDLN_PRODUCT_NM: getTextBoxValue('txtIQ_GDLN_PRODUCT_NM'),
                IQ_GDLN_PRODUCT_DESC: getTextBoxValue('txtIQ_GDLN_PRODUCT_DESC'),
                IQ_GDLN_NM: getTextBoxValue('txtIQ_GDLN_NM'),
                IQ_REFERENCE: getTextBoxValue('txtGS_IQ_Reference'),
                IQ_GDLN_DESC: getTextBoxValue('txtIQ_GDLN_DESC'),
                IQ_GDLN_RECOMMENDATION_DESC: getTextBoxValue('txtIQ_GDLN_RECOMMENDATION_DESC'),
                IQ_GDLN_EXP_DT: getTextBoxValue('txtIQ_GDLN_EXP_DT'),

                JRSDCTN_DPOC_RELEASE: getTextBoxValue('txtDPOCRelease_GS'),
                JRSDCTN_DPOC_VER_NUM: getTextBoxValue('txtDPOC_VER_NUM_GS'),
                JRSDCTN_IQ_GDLN_ID: getTextBoxValue('txtIQ_GDLN_ID_GS'), // Bug 138413 MFQ 9/23/2025
                JRSDCTN_NM: getMultiSelectValues('msGSJurisdiction'),
                JRSDCTN_IND: null,

                GDLN_AGE_MIN: getDropDownSelectedValue('txtGS_AgeMin'),
                GDLN_AGE_MAX: getDropDownSelectedValue('txtGS_AgeMax'),
                DX_APPLY: getGridDataLength('grid_GSDiagList'),
                STATES_APPLY: getTotalFromConfigByType('state', getTextBoxValue('txtIQ_GDLN_ID_GS'), getTextBoxValue('txtDPOC_VER_NUM_GS'), getTextBoxValue('txtDPOCRelease_GS')),
                POS_APPLY: getTotalFromConfigByType('pos', getTextBoxValue('txtIQ_GDLN_ID_GS'), getTextBoxValue('txtDPOC_VER_NUM_GS'), getTextBoxValue('txtDPOCRelease_GS')),
                DTQ_APPLY: getTotalFromConfigByType('dtq', getTextBoxValue('txtIQ_GDLN_ID_GS'), getTextBoxValue('txtDPOC_VER_NUM_GS'), getTextBoxValue('txtDPOCRelease_GS'))

                //DPOC_SOS_PROVIDER_TIN_EXCL: getDropDownSelectedValue('ddl_GSSOSProvTINExl')
            };
        }

        function updateGridWithGuideline(popupType, newActiveGuideline) {
            if (!popupType) return;

            const activeGrid = $("#grid_guideline_summary_active").data("kendoGrid");
            const pendingGrid = $("#grid_guideline_summary").data("kendoGrid");
            const rowId = $('#txtDataRowId_GS').val();

            if (popupType === 'Pending') {
                // Also add to summaryData
                const summaryData = DetailController.grid.guideline_summary.data;

                var newRecord = {
                    uid: rowId,
                    DX_APPLY: $('#grid_GSDiagList').data("kendoGrid").dataSource.data().length,
                    STATES_APPLY: getTotalFromConfigByType('state', newActiveGuideline.IQ_GDLN_ID, newActiveGuideline.DPOC_VER_NUM, newActiveGuideline.DPOC_RELEASE),
                    POS_APPLY: getTotalFromConfigByType('pos', newActiveGuideline.IQ_GDLN_ID, newActiveGuideline.DPOC_VER_NUM, newActiveGuideline.DPOC_RELEASE),
                    DTQ_APPLY: getTotalFromConfigByType('dtq', newActiveGuideline.IQ_GDLN_ID, newActiveGuideline.DPOC_VER_NUM, newActiveGuideline.DPOC_RELEASE)
                };

                for (var key in newActiveGuideline) {
                    if (newActiveGuideline.hasOwnProperty(key)) {
                        var value = newActiveGuideline[key];
                        newRecord[key] = (value !== undefined && value !== null) ? value : null;
                    }
                }


                summaryData.push(newRecord);

                activeGrid.dataSource.insert(0, newActiveGuideline);
                activeGrid.tbody.find("tr:first").addClass('highlight');

                // Hide the corresponding row in the pending grid
                pendingGrid.tbody.find(`tr[data-uid="${rowId}"]`).hide();

            } else {
                const row = activeGrid.tbody.find(`tr[data-uid='${rowId}']`);
                const dataItem = activeGrid.dataItem($(row).closest("tr"));

                Object.keys(newActiveGuideline).forEach(key => {
                    dataItem[key] = newActiveGuideline[key] ?? null;
                });

                const guidelineId = newActiveGuideline.IQ_GDLN_ID;
                const dpocVerNum = newActiveGuideline.DPOC_VER_NUM;
                const dpocRel = newActiveGuideline.DPOC_RELEASE;

                dataItem.DX_APPLY = $('#grid_GSDiagList').data("kendoGrid").dataSource.data().length;
                dataItem.STATES_APPLY = getTotalFromConfigByType('state', guidelineId, dpocVerNum, dpocRel);
                dataItem.POS_APPLY = getTotalFromConfigByType('pos', guidelineId, dpocVerNum, dpocRel);
                dataItem.DTQ_APPLY = getTotalFromConfigByType('dtq', guidelineId, dpocVerNum, dpocRel);

                const summaryData = DetailController.grid.guideline_summary.data;
                const matchingRecord = summaryData.find(item => item.uid === rowId);

                if (matchingRecord) {
                    Object.keys(newActiveGuideline).forEach(key => {
                        matchingRecord[key] = newActiveGuideline[key] ?? null;
                    });

                    matchingRecord.DX_APPLY = dataItem.DX_APPLY;
                    matchingRecord.STATES_APPLY = dataItem.STATES_APPLY;
                    matchingRecord.POS_APPLY = dataItem.POS_APPLY;
                    matchingRecord.DTQ_APPLY = dataItem.DTQ_APPLY;
                } else {
                    var newRecord = {
                        uid: rowId,
                        DX_APPLY: dataItem.DX_APPLY,
                        STATES_APPLY: dataItem.STATES_APPLY,
                        POS_APPLY: dataItem.POS_APPLY,
                        DTQ_APPLY: dataItem.DTQ_APPLY
                    };

                    for (var key in newActiveGuideline) {
                        if (newActiveGuideline.hasOwnProperty(key)) {
                            var value = newActiveGuideline[key];
                            newRecord[key] = (value !== undefined && value !== null) ? value : null;
                        }
                    }

                    summaryData.push(newRecord);
                }

                activeGrid.refresh();
            }

            gsWindow.close();
            gsHoverWindow.close();

            DetailController.dirtyform.dirtycheck();
        }

        function getTotalFromConfigByType(type, guidelineId, dpoc_ver_num, dpoc_release) {
            let total = 0;
            const configs = DetailController.grid.config.data.filter(c =>
                c.DTQ_IQ_GDLN_ID === guidelineId &&
                c.DPOC_RELEASE == dpoc_release &&
                c.DPOC_VER_NUM === dpoc_ver_num
            );

            configs.forEach(config => {
                if (!config || typeof config !== 'object') return;

                if (type === 'state') {
                    total += typeof config.STATES_APPL === 'string' && (config.STATES_APPL.trim() !== '' && config.STATES_APPL.trim() !== 'Add States') ? 1 : 0;
                } else if (type === 'pos') {
                    total += typeof config.POS_APPL === 'string' && (config.POS_APPL.trim() !== '' && config.STATES_APPL.trim() !== 'Add POS') ? 1 : 0;
                } else if (type === 'dtq') {
                    total +=
                        (typeof config.DTQ_NM === 'string' && (config.DTQ_NM.trim() !== '' && config.STATES_APPL.trim() !== 'Add DTQ') ? 1 : 0) +
                        (typeof config.TGT_DTQ === 'string' && (config.TGT_DTQ.trim() !== '' && config.STATES_APPL.trim() !== 'Add DTQ') ? 1 : 0) +
                        (typeof config.HOLDING_DTQ === 'string' && (config.HOLDING_DTQ.trim() !== '' && config.STATES_APPL.trim() !== 'Add DTQ') ? 1 : 0);
                }
            });


            return total;
        }

        // clear all popup fields now
        $('.gs-fields').val('');
    },
    guideline_summary_tab: {
        clear: function () {
            // clear all popup fields now
            $('.gs-fields').val('');
        }
    }
}

function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function configureFooter(action) {
    const footer = $('.bs-dpoc-guideline-summary-widget-modal-footer');
    action !== 'hover' ? footer.show() : footer.hide();
}

function showProgress(show) {
    window.kendo.ui.progress($("#bs-dpoc-guideline-summary-widget-modal"), show);
}

async function loadInitialTab(tabstrip) {

    //Hide popup update/activate button if record is Historical
    $('.bs-dpoc-guideline-summary-widget-modal-save')
        .toggle($("#isCurrentRecord").val() === 'Y');

    const gs_tab = $('#gsTabstrip .k-item:first');
    tabstrip.reload(gs_tab);
    await delay(1000);
}

async function loadGridData(popupType, row) {

    const grid = popupType === 'Pending'
        ? $("#grid_guideline_summary").data("kendoGrid")
        : $("#grid_guideline_summary_active").data("kendoGrid");

    $('.bs-dpoc-guideline-summary-widget-modal-save').text(popupType === 'Pending' ? 'Activate' : 'Update');

    const dataItem = grid.dataItem($(row).closest("tr"));

    const requiredFields = $('#lblGS_StartDate');
    if (dataItem.IQ_GDLN_STATUS === 'Active' || popupType === 'Pending') {
        requiredFields.addClass('required');
    } else {
        requiredFields.removeClass('required');
    }

    $('#txtGDLNPopupType').val(popupType);
    setGSDetailToForm(dataItem, popupType);
}


async function loadAdditionalTabs(tabstrip) {

    const pos_tab = $('#gsTabstrip .k-item:nth-child(2)');
    const dc_tab = $('#gsTabstrip .k-item:nth-child(3)');

    tabstrip.reload(pos_tab);
    //await delay(1000); // Allow time for tab content to load
    tabstrip.select(pos_tab);
    DPOC_ConfigController.loadData(); // Load data into the grid    

    await delay(500); // Allow time for tab content to load
    resizeConfigGridInWindow();

    tabstrip.reload(dc_tab);
    tabstrip.select(dc_tab);
    DiagCodesController.loadData(); // Load data into the grid    
    await delay(500); // Allow time for tab content to load
    resizeDiagGridInWindow();
    //const $grid_config = $("#grid_config").data("kendoGrid");
    //if ($grid_config) $grid_config.resize();

    //const $grid_GSDiagList = $("#grid_GSDiagList").data("kendoGrid");
    //if ($grid_GSDiagList) $grid_GSDiagList.resize();
}


function showWindowLoader() {
    const $window = $("#bs-dpoc-guideline-summary-widget-modal").data("kendoWindow");

    if ($window) {
        const windowElement = $window.wrapper;

        $("#windowLoader").css({
            position: "absolute",
            top: 0,
            left: 0,
            width: gsWindow.wrapper.width(),
            height: gsWindow.wrapper.height(),
            //width: windowElement.width(),
            //height: windowElement.height(),
            background: "aliceblue", // rgba(0, 0, 0, 0.6)", // darker background
            zIndex: 9999,
            display: "flex",
            alignItems: "center",
            justifyContent: "center"
        }).appendTo(windowElement).show();
    }
}
function hideWindowLoader() {
    $("#windowLoader").fadeOut();
}

async function finalizeTabSelection(tabstrip, tab) {

    if (tab === 'detail') {
        const detail_tab = $('#gsTabstrip .k-item:nth-child(1)');
        tabstrip.select(detail_tab);
    } else if (['pos', 'dtq', 'state'].includes(tab)) {
        const config_tab = $('#gsTabstrip .k-item:nth-child(2)');
        tabstrip.select(config_tab);
        //DPOC_ConfigController.loadData(false);                
        $('.dpoc-guideline-state-footer').show();
    } else if (tab === 'diag') {
        const dc_tab = $('#gsTabstrip .k-item:nth-child(3)');
        tabstrip.select(dc_tab);
        //DiagCodesController.loadData(false);
    }
}

async function openGuidelinePopup(row, popupType, tab, action) {
    configureFooter(action);

    gsWindow.center().open();
    //gsWindow.center();
    //gsWindow.maximize(); //.maximize()

    const tabstrip = $("#gsTabstrip").data("kendoTabStrip");

    // Show loader
    await delay(500);
    showWindowLoader();

    await loadInitialTab(tabstrip);
    await loadGridData(popupType, row);
    await delay(1000);
    await loadAdditionalTabs(tabstrip);

    await delay(1000);
    await finalizeTabSelection(tabstrip, tab);

    // Hide loader
    hideWindowLoader();

    // set focus on DPOC Release field
    $("#txtDPOCRelease_GS").focus();
}

function setGSDetailToForm(dataItem, popupType) {
    // Grid related data
    $("#txtDataRowId_GS").val(dataItem.uid);

    // Basic fields
    $("#txtDPOCRelease_GS").val(dataItem.DPOC_RELEASE).data('release', dataItem.DPOC_RELEASE);
    $("#txtDPOC_VER_NUM_GS").val(dataItem.DPOC_VER_NUM).data('version', dataItem.DPOC_VER_NUM);;
    $('#txtIQ_GDLN_NM').val(dataItem.IQ_GDLN_NM);
    $('#txtGS_IQ_Reference').val(dataItem.IQ_REFERENCE);
    $('#txtIQ_GDLN_DESC').val(dataItem.IQ_GDLN_DESC);
    $('#txtIQ_GDLN_PRODUCT_NM').val(dataItem.IQ_GDLN_PRODUCT_NM);
    $('#txtIQ_GDLN_PRODUCT_DESC').val(dataItem.IQ_GDLN_PRODUCT_DESC);
    $('#txtIQ_GDLN_RECOMMENDATION_DESC').val(dataItem.IQ_GDLN_RECOMMENDATION_DESC);
    $('#txtIQ_GDLN_ID_GS').val(dataItem.IQ_GDLN_ID);
    $('#txtIQ_GDLN_REL_DT_GS').val(dataItem.IQ_GDLN_REL_DT);
    $('#txtGS_Release').val(dataItem.DPOC_RELEASE);
    $('#txtGS_IQ_Version').val(dataItem.IQ_GDLN_VERSION);
    $('#txtGS_PackageConfigComments').val(dataItem.PKG_CONFIG_COMMENTS);

    // Dropdowns
    $('#ddlGS_IQCriteria').data("kendoDropDownList").value(dataItem.IQ_CRITERIA ?? "");
    $("#dpGS_StartDate").data("kendoDatePicker").value(dataItem.GDLN_ASSOC_EFF_DT);
    $("#dpGS_EndDate").data("kendoDatePicker").value(dataItem.GDLN_ASSOC_EXP_DT);

    //User Story 137657 MFQ 8/25/2025
    $("#ddlMedAssCovSumTitle").data("kendoComboBox").value(dataItem.MDCR_COVG_SUM_TITLE); //MDCR_COVG_SUM_TITLE
    $("#txtMedAssCovSumID").val(dataItem.MDCR_COVG_SUM_ID);

    // Medical Policy
    const ddlPolicy = $('#ddlGS_MedicalPolicy').data("kendoComboBox");
    const data = ddlPolicy.dataSource.data().toJSON(); // Convert to plain array
    const index = data.findIndex(d => d.PLCY_NM === dataItem.KL_PLCY_NAME); // Try to select the item by matching the name

    if (index !== -1) {
        ddlPolicy.select(index);
    } else {
        ddlPolicy.value(dataItem.KL_PLCY_NAME); // If not found, set the custom value
    }

    const ddlPolicyID = $('#ddlGS_MedicalPolicyID').data("kendoComboBox");
    const data2 = ddlPolicyID.dataSource.data().toJSON(); // Convert to plain array
    const index2 = data2.findIndex(d => d.PLCY_POLICY_ID === dataItem.KL_PLCY_ID); // Try to find the item by matching the policy ID

    if (index2 !== -1) {
        ddlPolicyID.select(index2);
    } else {
        ddlPolicyID.value(dataItem.KL_PLCY_ID); // If not found, set the custom value
    }

    // Jurisdiction
    $('#msGSJurisdiction').data("kendoMultiSelect").value(dataItem.JRSDCTN_NM?.split(',').map(j => j.trim()));

    // Comments
    $('#txtGS_Exclusion').val(dataItem.RULE_EXCLUSIONS);
    $('#txtGS_RuleComments').val(dataItem.RULE_COMMENTS);

    //SOS Provider TIN Exclusion
    $('#ddl_GSSOSProvTINExl').val(dataItem.DPOC_SOS_PROVIDER_TIN_EXCL);

    // Inpatient/Outpatient
    setInpatientFields(dataItem);
    setOutpatientFields(dataItem);
    setOutpatientFacilityFields(dataItem);

    // Hidden fields
    $('#txtIQ_GDLN_STATUS_GS').val(popupType === 'Pending' ? 'Active' : dataItem.IQ_GDLN_STATUS);
    $('#txtDataRowId_GS').val(dataItem.uid);

    // Age
    $('#txtGS_AgeMin').data("kendoDropDownList").value(dataItem.GDLN_AGE_MIN ?? 0);
    $('#txtGS_AgeMax').data("kendoDropDownList").value(dataItem.GDLN_AGE_MAX ?? 0);

    const summaryData = DetailController.grid.guideline_summary.data;
    // Update calculated fields
    const guidelineId = dataItem.IQ_GDLN_ID;
    const dpocVerNum = dataItem.DPOC_VER_NUM;
    const dpocRelease = dataItem.DPOC_RELEASE;

    const matchingRecord = summaryData.find(item =>
        item.DPOC_RELEASE === dpocRelease &&
        item.DPOC_VER_NUM === dpocVerNum &&
        item.IQ_GDLN_ID === guidelineId
    );

    if (matchingRecord)
        matchingRecord.uid = dataItem?.uid;
}

function setInpatientFields(data) {
    $('#ddlGSInPatOutcome').data("kendoDropDownList").value(data.RULE_TYPE_INPAT);
    $('#ddlGSInPatTypeRule').data("kendoDropDownList").value(data.RULE_TYPE_OUTCOME_INPAT);
    $('#ddlGSInPatReason').data("kendoDropDownList").value(data.RULE_TYPE_RSN_INPAT);
}

function setOutpatientFields(data) {
    $('#ddlGSOutPatOutcome').data("kendoDropDownList").value(data.RULE_TYPE_OUTPAT);
    $('#ddlGSOutPatTypeRule').data("kendoDropDownList").value(data.RULE_TYPE_OUTCOME_OUTPAT);
    $('#ddlGSOutPatReason').data("kendoDropDownList").value(data.RULE_TYPE_RSN_OUTPAT);
}

function setOutpatientFacilityFields(data) {
    $('#ddlGSOutPatFacOutcome').data("kendoDropDownList").value(data.RULE_TYPE_OUTPAT_FCLTY);
    $('#ddlGSOutPatFacTypeRule').data("kendoDropDownList").value(data.RULE_TYPE_OUTCOME_OUTPAT_FCLTY);
    $('#ddlGSOutPatFacReason').data("kendoDropDownList").value(data.RULE_TYPE_RSN_OUTPAT_FCLTY);
}

function resizeConfigGridInWindow() {
    var windowElement = $("#bs-dpoc-guideline-summary-widget-modal");
    var gridElement = $("#grid_config");
    var kendoWindow = windowElement.data("kendoWindow");

    if (kendoWindow && gridElement.length) {
        var windowContent = windowElement.find(".k-window-content");
        var headerHeight = windowElement.find(".k-window-titlebar").outerHeight() || 0;
        var footerHeight = $('.bs-dpoc-guideline-summary-widget-modal-footer').outerHeight() || 0;
        var padding = 20; // Optional extra spacing

        // Get the total height of the popup window
        var totalWindowHeight = windowElement.height();

        // Calculate the maximum height available for the grid
        var maxAvailableHeight = totalWindowHeight - headerHeight - footerHeight - padding;

        // Ensure minimum height to avoid collapsing
        var newGridHeight = Math.max(maxAvailableHeight, 200);

        gridElement.height(newGridHeight);

        var grid = gridElement.data("kendoGrid");
        if (grid) {
            grid.resize();
        }
    }
}

function resizeDiagGridInWindow() {
    var windowElement = $("#bs-dpoc-guideline-summary-widget-modal");
    var gridElement = $("#grid_GSDiagList");
    var kendoWindow = windowElement.data("kendoWindow");

    if (kendoWindow && gridElement.length) {
        var windowContent = windowElement.find(".k-window-content");
        var headerHeight = windowElement.find(".k-window-titlebar").outerHeight() || 0;
        var footerHeight = $('.bs-dpoc-guideline-summary-widget-modal-footer').outerHeight() || 0;
        var padding = 20; // Optional extra spacing

        // Get the total height of the popup window
        var totalWindowHeight = windowElement.height();

        // Calculate the maximum height available for the grid
        var maxAvailableHeight = totalWindowHeight - headerHeight - footerHeight - padding;

        // Ensure minimum height to avoid collapsing
        var newGridHeight = Math.max(maxAvailableHeight, 200);

        gridElement.height(newGridHeight);

        var grid = gridElement.data("kendoGrid");
        if (grid) {
            grid.resize();
        }
    }
}

function resizeWindowToGrid(windowId, gridId) {
    var grid = $("#" + gridId).data("kendoGrid");
    var kendoWindow = $("#" + windowId).data("kendoWindow");

    if (grid && kendoWindow) {
        // Get the grid's dimensions
        var gridWidth = $("#" + gridId).outerWidth();
        var gridHeight = $("#" + gridId).outerHeight();

        // Resize the window to match the grid
        kendoWindow.setOptions({
            width: gridWidth,
            height: gridHeight
        });

        // Optional: center the window after resizing
        kendoWindow.center();
    }
}


var gsWindow = gsWindow || {};
$(function () {
    'use strict';

    gsWindow = $("#bs-dpoc-guideline-summary-widget-modal").kendoWindow({
        actions: ["Minimize", "Maximize", "Close"],
        draggable: true,
        center: true,
        modal: true,
        visible: false,
        resizable: true,
        title: 'Guideline Summary',
        width: '1024px !important',
        //height: '794px !important',
        open: function () {
            isActiveRetGuidelineWidgetWindowOpen = true;
        },
        close: function () {
            isActiveRetGuidelineWidgetWindowOpen = false;
            setTimeout(function () {
                var tabToActivate = $("#tab-gl-summary");
                $('#gsTabstrip').kendoTabStrip().data('kendoTabStrip').activateTab(tabToActivate);
            }, 200);

            // Bug 138721 MFQ 9/25/2025
            $('#txtIQ_GDLN_ID_GS').val('');
            $('#txtDPOCRelease_GS').val('');
            $('#txtDPOC_VER_NUM_GS').val('');

            $('#grid_config').data("kendoGrid")?.dataSource.data([]);
            gridConfigDataCache = {};

            $('#grid_GSDiagList').data("kendoGrid")?.dataSource.data([]);
            gridDiagDataCache = {};
        },
        resize: function () {
            //resizeConfigGridInWindow()
            //resizeDiagGridInWindow();
        }

    }).data("kendoWindow");

    gsWindow.init = function () {
        $('.bs-dpoc-guideline-summary-widget-modal-close').on('click', function () {
            gsWindow.close();
        });
    }

    var prevPos = null;

    // Capture the position right before maximize
    gsWindow.bind("maximize", function () {
        // Kendo Window DOM wrapper
        var $w = this.wrapper;
        prevPos = {
            top: parseInt($w.css("top"), 10),
            left: parseInt($w.css("left"), 10)
        };
    });

    // On restore, put it back where it was
    gsWindow.bind("restore", function () {
        if (prevPos && Number.isFinite(prevPos.top) && Number.isFinite(prevPos.left)) {
            this.setOptions({ top: prevPos.top, left: prevPos.left });
        } else {
            this.center();
            this.setOptions({ top: 0 });
        }
    });

});

var gsHoverWindow = gsHoverWindow || {};
$(function () {
    'use strict';

    gsHoverWindow = $("#dpoc-guideline-summary-hover-window").kendoWindow({
        width: "1024px",
        title: "Guideline",
        visible: false,
        resizable: false,
        refresh: function () {
            if (gsHoverWindow.param.type == 'pos' || gsHoverWindow.param.type == 'dtq')
                typeof DPOC_ConfigController === "object" && DPOC_ConfigController.loadData(true);
            else if (gsHoverWindow.param.type == 'diag')
                typeof DiagCodesController === "object" && DiagCodesController.loadData(true);
            else if (gsHoverWindow.param.type == 'hv-state') {
                typeof StateInfoController === "object" && StateInfoController.loadData(true);
            }
        }
    }).data("kendoWindow");

    gsHoverWindow.param = function () {
        return {
            type: null
        }
    }
})

$(function () {
    'use strict';

    const $windowElement = $("#dpoc-guideline-config-state-widget-window");

    const stateWindow = $windowElement.kendoWindow({
        draggable: true,
        size: 'medium',
        modal: true,
        visible: false,
        title: 'States by Configurations',
        close: function () {
            $('#txtGDLN_DTQ_SYS_SEQ').val('');

            if (window.StateInfoController?.grid?.empty) {
                StateInfoController.grid.empty();
            }
        },
        refresh: function (e) {
            if (window.StateInfoController?.loadData) {
                var params = e.sender.options.stateParam;
                StateInfoController.loadData(params.isHoverWindow, params.stateCds, params.inclExclCd, params.inclExclCdDesc);
            }
        },
        position: {
            top: 200,
            left: "30%"
        }
    }).data("kendoWindow");

    // Optional: expose globally if needed
    window.gsStateWindow = stateWindow;
});



$(document).ready(function () {
    DPOC_GuidelineSummaryController.bind();

    $("#windowLoader").hide();
});


function refreshGdlnDataAfterDelete() {
    var grid = $("#grid_guideline_summary_active").data("kendoGrid");

    //fill guideline summary data on page load in local memory
    var data = grid.dataSource.data();

    DetailController.grid.guideline_summary.data = data.map(function (gl) {
        return {
            DPOC_HIERARCHY_KEY: gl.DPOC_HIERARCHY_KEY,
            DPOC_VER_EFF_DT: gl.DPOC_VER_EFF_DT,
            DPOC_PACKAGE: gl.DPOC_PACKAGE,
            IQ_CRITERIA: gl.IQ_CRITERIA,
            IQ_GDLN_ID: gl.IQ_GDLN_ID,
            IQ_GDLN_STATUS: gl.IQ_GDLN_STATUS,
            IQ_GDLN_NM: gl.IQ_GDLN_NM,
            IQ_GDLN_PRODUCT_DESC: gl.IQ_GDLN_PRODUCT_DESC,
            IQ_GDLN_PRODUCT_NM: gl.IQ_GDLN_PRODUCT_NM,
            IQ_GDLN_DESC: gl.IQ_GDLN_DESC,
            IQ_GDLN_RECOMMENDATION_DESC: gl.IQ_GDLN_RECOMMENDATION_DESC,
            IQ_REFERENCE: gl.IQ_REFERENCE,
            IQ_GDLN_REL_DT: gl.IQ_GDLN_REL_DT,
            IQ_GDLN_EXP_DT: gl.IQ_GDLN_EXP_DT,
            IQ_GDLN_RULES_SYS_SEQ: gl.IQ_GDLN_RULES_SYS_SEQ,
            IQ_GDLN_VERSION: gl.IQ_GDLN_VERSION,
            IQ_GDLN_JRSDCTN: gl.IQ_GDLN_JRSDCTN,
            RULE_IMP_TYPE: gl.RULE_IMP_TYPE,
            RULE_IMP_WITH: gl.RULE_IMP_WITH,
            RULE_TYPE_OUTPAT: gl.RULE_TYPE_OUTPAT,
            RULE_TYPE_OUTCOME_OUTPAT: gl.RULE_TYPE_OUTCOME_OUTPAT,
            RULE_TYPE_RSN_OUTPAT: gl.RULE_TYPE_RSN_OUTPAT,
            RULE_TYPE_OUTPAT_FCLTY: gl.RULE_TYPE_OUTPAT_FCLTY,
            RULE_TYPE_OUTCOME_OUTPAT_FCLTY: gl.RULE_TYPE_OUTCOME_OUTPAT_FCLTY,
            RULE_TYPE_RSN_OUTPAT_FCLTY: gl.RULE_TYPE_RSN_OUTPAT_FCLTY,
            RULE_TYPE_INPAT: gl.RULE_TYPE_INPAT,
            RULE_TYPE_OUTCOME_INPAT: gl.RULE_TYPE_OUTCOME_INPAT,
            RULE_TYPE_RSN_INPAT: gl.RULE_TYPE_RSN_INPAT,
            RULE_COMMENTS: gl.RULE_COMMENTS,
            RULE_EXCLUSIONS: gl.RULE_EXCLUSIONS,
            KL_PLCY_ID: gl.KL_PLCY_ID,
            KL_PLCY_NAME: gl.KL_PLCY_NAME,
            DTQ_APPLIES: gl.DTQ_APPLIES,
            GDLN_ASSOC_EFF_DT: gl.GDLN_ASSOC_EFF_DT,
            GDLN_ASSOC_EXP_DT: gl.GDLN_ASSOC_EXP_DT,
            JRSDCTN_DPOC_RELEASE: gl.DPOC_RELEASE,
            JRSDCTN_DPOC_VER_NUM: gl.DPOC_VER_NUM,
            JRSDCTN_NM: gl.JRSDCTN_NM,
            JRSDCTN_IQ_GDLN_ID: gl.IQ_GDLN_ID,
            JRSDCTN_IND: gl.JRSDCTN_IND,
            HOLDING_DTQ: gl.HOLDING_DTQ,
            HOLDING_DTQ_VERSION: gl.HOLDING_DTQ_VERSION,
            TGT_DTQ: gl.TGT_DTQ,
            TGT_DTQ_VERSION: gl.TGT_DTQ_VERSION,
            DTQ_RSN: gl.DTQ_RSN,
            GDLN_AGE_MIN: gl.GDLN_AGE_MIN,
            GDLN_AGE_MAX: gl.GDLN_AGE_MAX,
            STATES_APPLY: gl.STATES_APPLY,
            STATES_APPL: gl.STATES_APPL,
            DX_APPLY: gl.DX_APPLY,
            POS_APPLY: gl.POS_APPLY,
            DTQ_APPLY: gl.DTQ_APPLY,
            DPOC_SOS_PROVIDER_TIN_EXCL: gl.DPOC_SOS_PROVIDER_TIN_EXCL,
            DPOC_RELEASE: gl.DPOC_RELEASE,
            DPOC_VER_NUM: gl.DPOC_VER_NUM,
            PKG_CONFIG_COMMENTS: gl.PKG_CONFIG_COMMENTS
        };
    });

}

function normalizeGdlnAgeValues(ageMin, ageMax) {
    // Convert empty strings or undefined to null
    ageMin = ageMin === "" || ageMin == null ? null : parseInt(ageMin);
    ageMax = ageMax === "" || ageMax == null ? null : parseInt(ageMax);

    // Apply logic
    if (ageMin === null && ageMax === null) {
        ageMin = 0;
        ageMax = 0;
    } else if (ageMin === null && ageMax !== null) {
        ageMin = 0;
    } else if (ageMin !== null && ageMax === null) {
        ageMax = 125;
    }

    return { ageMin, ageMax };
}

function normalizeGdlnAgeMin(ageMin, ageMax) {
    ageMin = ageMin === "" || ageMin == null ? null : parseInt(ageMin);
    ageMax = ageMax === "" || ageMax == null ? null : parseInt(ageMax);

    if (ageMin === null && ageMax === null) {
        ageMin = 0;
    } else if (ageMin === null && ageMax !== null) {
        ageMin = 0;
    }

    return ageMin;
}

function normalizeGdlnAgeMax(ageMin, ageMax) {
    ageMin = ageMin === "" || ageMin == null ? null : parseInt(ageMin);
    ageMax = ageMax === "" || ageMax == null ? null : parseInt(ageMax);

    if (ageMin === null && ageMax === null) {
        ageMax = 0;
    } else if (ageMin !== null && ageMax === null) {
        ageMax = 125;
    }

    return ageMax;
}