var ManageGuidelines = (function () {
    // Get search parameters
    function getSearchParams() {
        return {
            proc_cd: $("#ddlProcedureCodes").data("kendoMultiSelect").value().join(","), //$("#txtProcedureCode").val().trim(),
            iq_reference: $("#txtIQReference").val().trim(),
            iq_gdln_id: $("#txtIQGuidelineID").val().trim(),
            iq_gdln_version: $("#txtIQVersion").val().trim()
        };
    }    

    function getProcCdParams() {
        var multiselect = $("#ddlProcedureCodes").data("kendoMultiSelect");
        return {
            searchTerm: multiselect.input.val() // ✅ Send typed value
        };
    }

    // Validate if at least one field has a value
    function hasSearchCriteria(params) {
        return Object.values(params).some(val => val !== "");
    }

    // Validate procedure codes format (comma-separated, optional spaces)
    function validateProcedureCodes(proc_cd) {
        if (!proc_cd) return true; // No codes entered, skip validation
        var codes = proc_cd.split(',').map(c => c.trim()).filter(c => c !== "");
        var invalidCodes = codes.filter(c => !/^[a-zA-Z0-9]+$/.test(c)); // Allow letters and numbers
        return invalidCodes.length === 0;
    }

    // Show alert banner
    function showAlert(message) {
        var banner = $("#searchAlertBanner");
        banner.html("<strong>Instruction:</strong> " + message +
            '<button type="button" class="close" aria-label="Close" onclick="$(\'#searchAlertBanner\').addClass(\'d-none\');"><span aria-hidden="true">&times;</span></button>');
        banner.removeClass("d-none");
    }

    function hideAlert() {
        // Hide the validation banner
        $("#searchAlertBanner").addClass("d-none").html("");
    }

    function showAlertAddNew(message) {
        MICore.Notification.warning('Invalid entries', message);
    }

    // Perform search

    function search() {
        var params = getSearchParams();

        hideAlert();

        // 1) Proc code must be selected
        const hasProcCd = typeof params.proc_cd === 'string' && params.proc_cd.trim() !== '';
        if (!hasProcCd) {
            showAlert("Procedure code is mandatory.");
            return;
        }

        // 2) Must have at least one search criterion
        if (!hasSearchCriteria(params)) {
            showAlert("At least one field of search criteria must be populated.");
            return;
        }

        // 3) Validate raw procedure codes format early (if provided)
        if (params.proc_cd && !validateProcedureCodes(params.proc_cd)) {
            showAlert("Procedure codes must be alphanumeric and separated by commas.");
            return;
        }        

        // Proceed with grid read
        const gridEl = $("#grid_guidelines");
        const grid = gridEl.data("kendoGrid");

        kendo.ui.progress(gridEl, true);
        grid.dataSource.read(params);
        grid.one("dataBound", function () {
            kendo.ui.progress(gridEl, false);
        });
    }

    // Reset form and reload grid
    function resetForm() {
        $("#guidelines-search-form")[0].reset();
        var grid = $("#grid_guidelines").data("kendoGrid");
        grid.dataSource.data([]);

        // Hide the validation banner
        hideAlert();
    }

    // addNew module
    var addNew = (function () {

        // Validate button logic
        function validateAndSave() {
            var iq_reference = $("#txtIQReferenceAddNew").val().trim();
            var iq_gdln_id = $("#txtIQGuidelineIDAddNew").val().trim();
            var iq_version = $("#txtIQVersionAddNew").val().trim();

            if (!iq_reference || !iq_gdln_id || !iq_version) {
                showAlertAddNew("IQ Reference, Guideline ID, and Version must not be empty.");
                return;
            }

            var procCodes = addNew.getProcedureCodes(); // This should return comma-separated string
            if (!procCodes || procCodes.length === 0) {
                showAlertAddNew("Please add at least one procedure code.");
                return;
            }

            kendo.ui.progress($("#addGuidelineWindow"), true);

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                type: "POST",
                url: MIApp.Common.ApiEPRepository.get('admin-add-guidelines-url', 'ManageGuidelinesUrls'),
                async: true,
                cache: false,
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                data: JSON.stringify({
                    iq_gdln_id: MIApp.Sanitize.encodeProp(iq_gdln_id),
                    iq_gdln_proc_cd: MIApp.Sanitize.encodeProp(procCodes), // Comma-separated codes
                    iq_version: MIApp.Sanitize.encodeProp(iq_version),
                    iq_reference: MIApp.Sanitize.encodeProp(iq_reference)
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (insertRetDto) {
                    kendo.ui.progress($("#addGuidelineWindow"), false);

                    var successStatus = {
                        title: 'Save Status',
                        type: '',
                        message: 'Please see below the status of the Save progress.',
                        insRecs: '',
                        dupRecs: '',
                        errRecs: '',
                        callback: null
                    };

                    // Invalid/Erroneous Records
                    if (insertRetDto.invalidProcCodes && insertRetDto.invalidProcCodes.length > 0) {
                        successStatus.errRecs += insertRetDto.invalidProcCodes.join(', ');
                        successStatus.type = 'warning';
                    }

                    // Inserted Records
                    if (insertRetDto.insertedRecords && insertRetDto.insertedRecords.length > 0) {
                        successStatus.insRecs += insertRetDto.insertedRecords.join(', ');
                        successStatus.type = 'success';
                    }

                    // Duplicate Records
                    if (insertRetDto.duplicateProcCodes && insertRetDto.duplicateProcCodes.length > 0) {
                        successStatus.dupRecs += insertRetDto.duplicateProcCodes.join(', ');
                        successStatus.type = 'warning';
                    }

                    // HTML Template
                    var htmlTemplate =
                        "<div class='mb-3 font-weight-bold'>{{message}}</div> \
                        <table class='table table-bordered'> \
                            <thead> \
                                <tr> \
                                    <th scope='col'>Inserted Records</th> \
                                    <th scope='col'>Duplicate Records</th> \
                                    <th scope='col'>Invalid/Erroneous Records</th> \
                                 </tr> \
                            </thead> \
                            <tbody> \
                                {{$trs}} \
                            </tbody> \
                        </table>";

                    var rowTemplate =
                                        "<tr> \
                        <td>{{insTd}}</td> \
                        <td>{{dupTd}}</td> \
                        <td>{{errTd}}</td> \
                    </tr>";

                    rowTemplate = rowTemplate.replace('{{insTd}}', successStatus.insRecs || 'None');
                    rowTemplate = rowTemplate.replace('{{dupTd}}', successStatus.dupRecs || 'None');
                    rowTemplate = rowTemplate.replace('{{errTd}}', successStatus.errRecs || 'None');

                    htmlTemplate = htmlTemplate.replace('{{$trs}}', rowTemplate).replace('{{message}}', successStatus.message);

                    // Show Notification
                    if (successStatus.insRecs == '') {
                        MICore.Notification.warning(successStatus.title, null, function () {
                            addNew.clearAddGuidelinePopup();
                        }, htmlTemplate);
                    } else {
                        MICore.Notification.success(successStatus.title, htmlTemplate, function () {
                            addNew.clearAddGuidelinePopup();
                        });
                    }
                },
                error: function (xhr) {
                    kendo.ui.progress($("#addGuidelineWindow"), false);
                    showAlertAddNew("Error adding guidelines: " + xhr.responseText);
                }
            });
        }

        function addToList(proc_cds) {
            proc_cds = proc_cds.replace(/\r\n/g, ",")
                .replace(/\r/g, ",")
                .replace(/\n/g, ",")
                .replace(/ /g, ",");

            var $lsvProcCode_ubpc = $("#lsvProcedureCodes").data("kendoListView");
            var lsvProcCodeItems = $lsvProcCode_ubpc.dataSource;

            proc_cds.split(/[\s,]+/).forEach(function (pc) {
                if ($.trim(pc) !== '') {
                    var existingItem = $.grep(lsvProcCodeItems.view(), function (m) {
                        return m.PROC_CD.toUpperCase() === pc.toUpperCase();
                    });

                    if (existingItem.length === 0) {
                        // Insert new item
                        $lsvProcCode_ubpc.dataSource.insert(0, { PROC_CD: pc.toUpperCase() });
                    }
                }
            });
        }

        function getProcedureCodes() {
            var $lsvProcCode_ubpc = $("#lsvProcedureCodes").data("kendoListView");
            return $lsvProcCode_ubpc.dataSource.view().map(function (item) {
                return item.PROC_CD;
            }).join(',');
        }

        function clearAddGuidelinePopup() {
            var $popup = $('#addGuidelineWindow');

            // Clear text inputs
            $popup.find('input[type="text"]').val('');

            // Clear Kendo ListView (lsvProcedureCodes)
            var listView = $('#lsvProcedureCodes').data('kendoListView');
            if (listView) {
                listView.dataSource.data([]); // Remove all items
            }

            // Optionally hide any status messages
            $('#AddGuidelineStatus').addClass('d-none');
        }

        return {
            validateAndSave: validateAndSave,
            addToList: addToList,
            getProcedureCodes: getProcedureCodes,
            clearAddGuidelinePopup: clearAddGuidelinePopup
        };
    })();

    // Bind events
    function bindEvents() {

        MIApp.Common.ApiEPRepository.set('manage-guidelines-urls', 'ManageGuidelinesUrls');

        $("#btnSearch").on("click", function (e) {
            e.preventDefault();
            search();
        });

        $("#btnReset").on("click", function () {
            resetForm();
        });

        $("#addGuidelineWindow").kendoWindow({
            width: "800px",
            height: "auto",
            title: "Add New Guideline",
            visible: false,
            modal: true,
            resizable: false,
            actions: ["Close"]
        });

        $("#btnAddNew").on("click", function () {
            $("#addGuidelineWindow").data("kendoWindow").center().open();
        });

        $("#btnValidateGuideline").on("click", function () {
            addNew.validateAndSave();
        });

        $("#btnCloseAddNewGuideline").on("click", function () {
            $("#addGuidelineWindow").data("kendoWindow").close();
        });

        $('#txtProcedureCodeAddNew').bind("keypress", function (e) {
            if (e.keyCode === 13) {
                addNew.addToList($('#txtProcedureCodeAddNew').val());
                $('#txtProcedureCodeAddNew').val('');
            }
        });

        $("#formAddNew").on("submit", function (e) {
            e.preventDefault(); // Prevent any submit
        });

        $('#btnResetAddNewGuideline').on('click', function (e) {
            e.preventDefault(); // Prevent default reset behavior if needed

            // Clear all text inputs
            $('#addGuidelineWindow').find('input[type="text"]').val('');

            // Clear Kendo ListView
            var listView = $('#lsvProcedureCodes').data('kendoListView');
            if (listView) {
                listView.dataSource.data([]); // Remove all items
            }

            // Optionally hide any status messages
            $('#AddGuidelineStatus').addClass('d-none');
        });

    }

    function init() {
        bindEvents();
    }

    
    return {
        init: init,
        getSearchParams: getSearchParams,
        getProcCdParams: getProcCdParams,
        addNew: addNew        
    };
})();

$(document).ready(function () {
    ManageGuidelines.init();
});