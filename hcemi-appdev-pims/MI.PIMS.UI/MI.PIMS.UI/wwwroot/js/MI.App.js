
var MIApp = MIApp || {};

$(function () {
    'use strict';

    MIApp.Common = {
        UI: {
            getGridRow: function (gridId, itemId) {
                var kGrid = $("#" + gridId).data("kendoGrid");
                var dataItem = $("#" + gridId).data("kendoGrid").dataSource.get(itemId);
                var row = $("#" + gridId).data("kendoGrid").tbody.find("tr[data-uid='" + dataItem.uid + "']");
                return row;
            },
            logouturl: function (url) {
                return url;
            },
            loadModalBodyByUrl: function (modalClass, url, fnc) {
                url = url === null || url === undefined ? $('.' + modalClass).data('url') : url;

                $('.' + modalClass).modal('show');
                $.get(url, function (data) {
                    $('.' + modalClass + ' .modal-body').html(data);
                    if (fnc != undefined && fnc != null) {
                        setTimeout(function () {
                            fnc();
                        }, 1000)
                    }
                });
            },
            checkFileUpload: function (e, fileAllowed, fileMaxSize, callback) {
                var val = e.target.value;
                if (val !== "") {
                    var regex = new RegExp("(.*?)\\.(" + fileAllowed + ")$"); // fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/71 MFQ 7/19/2023
                    if (!(regex.test(val))) {
                        e.target.value = '';
                        MICore.Notification.warning('', '', '', '<div>Please select a file with the type from this list:</div><ul class="valid-files"><li>' + fileAllowed.split("|").join("</li><li>") + '</li></ul>');
                        return;
                    }
                    var size = Math.ceil(e.target.files[0].size / 1024);
                    if (size > fileMaxSize) {
                        e.preventDefault();
                        e.target.value = '';
                        MICore.Notification.warning(null, 'The maximum ' + fileMaxSize + 'KB size. Please select an smaller file.');
                        return;
                    }

                    if (callback !== null) callback();
                }
            },
            sendEmail: function (subject, bodyToSend) {
                var a = document.createElement("a");
                a.href = "mailto:?subject=" + subject + "&body=" + bodyToSend;
                a.id = "emailLink"; // + uuidv4();
                a.style.visibility = "hidden";
                document.body.appendChild(a);
                document.getElementById(a.id).click();
                document.getElementById(a.id).remove();
            }
        },
        Enum: {
            screenType: null,
            screenTypeValues: {
                search: 'search',
                addNewEPAL: 'addNewEPAL',
                editEPAL: 'editEPAL',
                admin: 'admin'
            }
        },
        Helper: {           
            //Check is string null or empty
            isStringNullOrEmpty: function (val) {
                switch (val) {
                    case "":
                    case 0:
                    case "0":
                    case null:
                    case false:
                    case undefined:
                    case typeof this === 'undefined':
                        return true;
                    default: return false;
                }
            },

            //Check is string null or whitespace
            isStringNullOrWhiteSpace: function (val) {
                return this.isStringNullOrEmpty(val) || val.replace(/\s/g, "") === '';
            },

            //If string is null or empty then return Null or else original value
            nullIfStringNullOrEmpty: function (val) {
                if (this.isStringNullOrEmpty(val)) {
                    return null;
                }
                return val;
            },
            multipleDateRangeOverlaps: function (timeEntries) {
                let i = 0, j = 0;
                //let timeIntervals = timeEntries.filter(entry => entry.from != null && entry.to != null && entry.from.length === 8 && entry.to.length === 8);
                let timeIntervals = timeEntries.filter(entry => entry.from != null && entry.to != null);
                //let timeIntervals = timeEntries;

                //if (timeIntervals != null && timeIntervals.length > 1)
                for (i = 0; i < timeIntervals.length - 1; i += 1) {
                    for (j = i + 1; j < timeIntervals.length; j += 1) {
                        if (
                            dateRangeOverlaps(
                                timeIntervals[i].from.getTime(), timeIntervals[i].to.getTime(),
                                timeIntervals[j].from.getTime(), timeIntervals[j].to.getTime()
                            )
                        ) return true;
                    }
                }
                return false;

                function dateRangeOverlaps(a_start, a_end, b_start, b_end) {
                    if (a_start <= b_start && b_start <= a_end) return true; // b starts in a
                    if (a_start <= b_end && b_end <= a_end) return true; // b ends in a
                    if (b_start <= a_start && a_end <= b_end) return true; // a in b
                    return false;
                }
            }
        },
        /******************************
         * MIApp.ApiEPRepository
         * API End Points Repository
         * It helps to stores all API end points for each of the view, that's been sent from Controller to view.
         * Need to use MIApp.ApiEPRepository.set at the load of the view along with ctrlId and listId
         * ctrlId = the control that has PageApiInfos list imported from controller
         * listId = provided any unique name that you wanna use to get the api link from 
         * ************************************ 
         * getFromObject = If you have called up on the links in an JS Object on view then call this function to get the respective link based on Type. In this call you don't need to MIApp.ApiEPRepository.set
         * ************************************ 
         * MFQ 7/7/2023 - Needed to create it to avoid vulnerabilities.
        ******************************/
        ApiEPRepository: {
            set: function (ctrlId, listId) {
                this[listId] = JSON.parse($('#' + ctrlId).val());
            },
            get: function (type, listId) {
                var self = this;
                var api = self[listId].find(function (item) { return item.Type === type; });
                return MIApp.Sanitize.decode(api.Url);
            },
            getFromObject: function (type, obj) {
                var api = obj.find(function (item) { return item.Type === type; });
                return MIApp.Sanitize.decode(api.Url);
            }
        }
    };

    MIApp.Sanitize = {
        /*!
         * Sanitize an HTML string
         * (c) 2021 Chris Ferdinandi, MIT License, https://gomakethings.com
         * https://gomakethings.com/how-to-sanitize-html-strings-with-vanilla-js-to-reduce-your-risk-of-xss-attacks/
         * @param  {String}          str   The HTML string to sanitize
         * @param  {Boolean}         nodes If true, returns HTML nodes instead of a string
         * @return {String|NodeList}       The sanitized string or nodes
         */

        cleanHTML: function (str, nodes) {
            /**
             * Convert the string to an HTML document
             * @return {Node} An HTML document
             */
            function stringToHTML() {
                let parser = new DOMParser();
                let doc = parser.parseFromString(str, 'text/html');
                return doc.body || document.createElement('body');
            }

            // Convert the string to HTML
            let html = stringToHTML();
        },

        /**
         * Remove <script> elements
         * @param  {Node} html The HTML
         */
        removeScripts: function removeScripts(html) {
            let scripts = html.querySelectorAll('script');
            for (let script of scripts) {
                script.remove();
            }
        },

        /**
         * Remove dangerous stuff from the HTML document's nodes
         * @param  {Node} html The HTML document
         */
        clean: function (html) {
            let nodes = html.children;
            for (let node of nodes) {
                removeAttributes(node);
                clean(node);
            }
        },

        /**
         * Remove potentially dangerous attributes from an element
         * @param  {Node} elem The element
         */
        removeAttributes: function (elem) {
            // Loop through each attribute
            // If it's dangerous, remove it
            let atts = elem.attributes;
            for (let { name, value } of atts) {
                if (!isPossiblyDangerous(name, value)) continue;
                elem.removeAttribute(name);
            }
        },

        /**
         * Check if the attribute is potentially dangerous
         * @param  {String}  name  The attribute name
         * @param  {String}  value The attribute value
         * @return {Boolean}       If true, the attribute is potentially dangerous
         */
        isPossiblyDangerous: function (name, value) {
            if (name.startsWith('on')) return true;
        },

        /**
         * https://stackoverflow.com/questions/23187013/is-there-a-better-way-to-sanitize-input-with-javascript
         * @param {any} str
         */
        specialCharacters: function (str) {
            str = str.replace(/[^a-z0-9áéíóúñü \.,_-]/gim, "");
            return str.trim();
        },

        string: function (str) {

            let elem = document.createElement("div");
            let cleanHTML = DOMPurify.sanitize(str);
            elem.innerHTML = cleanHTML;
            return elem.innerHTML;
            //const div = document.createElement('div')
            //div.textContent = str

            //return div.innerHTML
        },
        encode: function (str) {
            return encodeURIComponent(str);
        },
        decode: function (str) {
            return decodeURIComponent(str);
        },
        escapeRegExp: function (text) {
            return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
        },
        toBase64: function (text) {
            return btoa(text);
        },
        decodeBase64: function (text) {
            return atob(text);
        },
        encodeProp: function (plProp) {
            if (plProp == null) return null;
            let encodedUriPayload = encodeURIComponent(plProp);
            return btoa(encodedUriPayload);
        },
        encodeObject: function (obj) {
            const sanitizedObj = {};
            for (const key in obj) {
                if (obj.hasOwnProperty(key)) {
                    //Bug 128801 Resolution
                    sanitizedObj[key] = typeof obj[key] === 'string' && !kendo.parseInt(obj[key]) && !kendo.parseDate(obj[key]) ? this.encodeProp(obj[key]) : obj[key];
                }
            }
            return sanitizedObj;
        }
    }

    MIApp.Global = {
        Page: {
            ePALPageView: function () {
                var thisUrl = window.location.href.toLowerCase();
                var ePALPageView =
                    (thisUrl.indexOf('adddetail') > -1 ? "adddetail" :
                        thisUrl.indexOf('editdetail') > -1 ? "editdetail" :
                            thisUrl.indexOf('duplicaterecorddetail') > -1 ? "duplicaterecorddetail" :
                                thisUrl.indexOf('viewdetail') > -1 ? "viewdetail" : ""
                    );

                return ePALPageView;
            }
        }
    };

    MIApp.DPOC = {
        create_pims_id: function () {
            var pims_id = ''
            var BUS_SEG_CD = $("#ddlBusinessSegment").val();
            var ENTITY_CD = $("#ddlEntity").val();
            var PLAN_CD = $("#ddlPlan").val();
            var PRODUCT_CD = $("#ddlProduct").val();
            var FUND_ARNGMNT_CD = $("#ddlFundingArrangement").val();
            var PROC_CD = $("#txtProcedureCode").val();
            var DRUG_NM = $("#txtDrugName").val();


            if (BUS_SEG_CD != '') {
                BUS_SEG_CD += "-"
            }
            if (ENTITY_CD != '') {
                ENTITY_CD += "-"
            }
            if (PLAN_CD != '') {
                PLAN_CD += "-"
            }
            if (PRODUCT_CD != '') {
                PRODUCT_CD += "-"
            }
            if (FUND_ARNGMNT_CD != '') {
                FUND_ARNGMNT_CD += "-"
            }
            if (PROC_CD != '') {
                PROC_CD += "-"
            }

            pims_id = BUS_SEG_CD + ENTITY_CD + PLAN_CD + PRODUCT_CD + FUND_ARNGMNT_CD + PROC_CD.toUpperCase() + DRUG_NM.toUpperCase().trim();
            return pims_id;
        },
        disableDrugName: function () {
            $('#txtDrugName').attr('readonly', true);
            $('#txtDrugName').val('NA');

            if ($('#txtDrugName').val() == 'NA') {
                $('#lblDrugReviewAtLaunch').removeClass('required')
            }
        },
        enableDrugName: function () {
            $('#txtDrugName').attr('readonly', false);
            $('#txtDrugName').val('');
            if ($('#txtDrugName').val() == '') {
                $('#lblDrugReviewAtLaunch').addClass('required')
            }
            $('#txtDrugName').focus();
        }
    }

    MIApp.EPAL = {
        validateDrugNameOnTyping: function (event) {
            const char = String.fromCharCode(event.which);
            const pattern = /^[a-zA-Z0-9~_]$/;
            if (!pattern.test(char)) {
                event.preventDefault();
            }
        },
        invalidDrugNamePattern: function (drugName) {
            const invalidPattern = /[^a-zA-Z0-9~_]/;
            if (invalidPattern.test(drugName)) {
                return false;
            }
            return true;
        }
    }

});


function getDropDownValue(id, defaultText = 'Type or Click for Dropdown') {
    const ddl = $(`#${id}`).data("kendoDropDownList");
    return ddl && ddl.text() !== defaultText ? ddl.text() : '';
}

function getComboBoxValue(id, defaultText = 'Type or Click for Dropdown') {
    const ddl = $(`#${id}`).data("kendoComboBox");
    return ddl && ddl.text() !== defaultText ? ddl.text() : '';
}

function getDropDownSelectedValue(id) {
    const ddl = $(`#${id}`).data("kendoDropDownList");
    return ddl ? ddl.value() : '';
}

function getDropDownSelectedText(id) {
    const ddl = $(`#${id}`).data("kendoDropDownList");
    return ddl ? (ddl.text() == 'Type or Click for Dropdown' ? '' : ddl.text()) : '';
}

function getDatePickerValue(id) {
    const dp = $(`#${id}`).data("kendoDatePicker");
    return dp ? kendo.parseDate(dp.value()) : null;
}

function getMultiSelectValues(id) {
    const ms = $(`#${id}`).data("kendoMultiSelect");
    const values = ms ? ms.value() : [];
    return values.length === 0 ? null : values.join(',');
}

function getTextBoxValue(id) {
    return $(`#${id}`).val();
}

function getGridDataLength(id) {
    const grid = $(`#${id}`).data("kendoGrid");

    if (grid.dataSource.data().length == 0) {
        var diagCodes_by_gdln_id = DetailController.grid.diags.data.filter((dtq) =>
            dtq.DIAG_IQ_GDLN_ID == ($('#txtIQ_GDLN_ID_GS').val() == '' ? $('#txtIQ_GDLN_ID_GS_hdn').val() : $('#txtIQ_GDLN_ID_GS').val()) &&
            dtq.DPOC_RELEASE == ($('#txtDPOCRelease_GS').val() == '' ? $('#txtDPOCRelease_GS_hdn').val() : $('#txtDPOCRelease_GS').val()) &&
            dtq.DPOC_VER_NUM == ($('#txtDPOC_VER_NUM_GS').val() == '' ? $('#txtDPOC_VER_NUM_hdn').val() : $('#txtDPOC_VER_NUM_GS').val())
        );
        return diagCodes_by_gdln_id;
    } else {
        return grid ? grid.dataSource.data().length : 0;
    }
}

function isEmpty(value) {
    return value === null || value === undefined || value.toString().trim() === "";
}
