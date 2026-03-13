// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var ENTER_KEY = 13;
var ESC_KEY = 27;

var TODAY_DATE = new Date();
var dd = String(TODAY_DATE.getDate()).padStart(2, '0');
var mm = String(TODAY_DATE.getMonth() + 1).padStart(2, '0'); //January is 0!
var yyyy = TODAY_DATE.getFullYear();
TODAY_DATE = mm + '/' + dd + '/' + yyyy;

/* MODAL*/
function setModalTitle(titleId, title) {
    $('#' + titleId).html(title);
}

function jQFormSerializeArrToJson(formSerializeArr) {
    var jsonObj = {};
    $.map(formSerializeArr, function (n, i) {
        jsonObj[n.name] = n.value;
    });

    return jsonObj;
}

function convertDateMMddyyyy(input_date) {    
    if (input_date == '' || input_date == '12/31/2999 00:00:00') {
        return null;
    }
    input_date = new Date(input_date);
    var dd = String(input_date.getDate()).padStart(2, '0');
    var mm = String(input_date.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = input_date.getFullYear();
    return mm + '/' + dd + '/' + yyyy;
}

//Date(000000000000) to MM/dd/yyyy HH:MM:SS
function convertIntToDate(intDate) {
    var _retDate = new Date(parseInt(intDate.substr(6)));

    var dd = _retDate.getDate();
    var mm = _retDate.getMonth() + 1; //January is 0!
    var yyyy = _retDate.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }
    return mm + '/' + dd + '/' + yyyy + ' ' + _retDate.getHours() + ':' + _retDate.getMinutes() + ':' + _retDate.getSeconds();
}

function redirectToHome() {
    var base_url = window.location.origin;
    //window.location = VIRTUAL_DIRECTORY == '' || VIRTUAL_DIRECTORY == null ? base_url + '/' : base_url + VIRTUAL_DIRECTORY;
    window.location = MIApp.Sanitize.decode(MIApp.Common.ApiEPRepository.getFromObject('home-url', BaseApiUrls)); // MIApp.Sanitize.decode($('#HomeUrl').val()); fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/80 MFQ 7/17/2023
}

function logOut() {
    window.location = MIApp.Sanitize.decode(MIApp.Common.ApiEPRepository.getFromObject('log-out-url', BaseApiUrls));// $('#LogOutUrl').val() fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/80 MFQ 7/17/2023
}

function windowPopup(popupId, url, overRideCtrlFunc, overRideRegFunc) {
    document.body.onclick = function (e) {
        if (e.ctrlKey) {
            if (overRideCtrlFunc == null || overRideCtrlFunc == undefined) {
                ctrlWinOpen(url, popupId);
            } else {
                overRideCtrlFunc(url, popupId);
            }
        } else {
            if (overRideRegFunc == null || overRideRegFunc == undefined) {
                winHrefOpen(url, popupId);
            } else {
                overRideRegFunc(url, popupId);
            }            
        }
    }

    function ctrlWinOpen(url, popupId) {
        setTimeout(
            op
            , 1000);

        function op() {
            var a = window.open(url, popupId);
            a.focus();
            document.body.onclick = undefined;
        }
    }

    function winHrefOpen(url, popupId) {
        window.location.href = url;
        document.body.onclick = undefined;
    }
}

function getDrivingStatusDateRangeIncludingToday(dateRanges, today) {
    var activeDatePair = null,
        sos_eff = new Date($("#dpSOSEffectiveDate").val()),
        sos_exp = $.trim($("#dpSOSExpirationDate").val()) == '' ? new Date('12/31/2999') : new Date($("#dpSOSExpirationDate").val());


    if (dateRanges == 'all' || dateRanges.length == 0) {       
        dateRanges = [];
        if ($.trim($("#dpPAEffectiveDate").val()) != '') {

            var pa_eff = new Date($("#dpPAEffectiveDate").val()),
                pa_exp = $.trim($("#dpPAExpirationDate").val()) == '' ? new Date('12/31/2999') : new Date($("#dpPAExpirationDate").val()),
                aa_eff = new Date($("#dpAAEffectiveDate").val()),
                aa_exp = $.trim($("#dpAAExpirationDate").val()) == '' ? new Date('12/31/2999') : new Date($("#dpAAExpirationDate").val());

            if (sos_eff != 'Invalid Date' && aa_eff == 'Invalid Date') {
                if (sos_eff >= pa_eff && sos_exp <= pa_exp) {
                    dateRanges.push({ future: null, name: drivingStatus.PASOS, startDate: new Date($('#dpPAEffectiveDate').val()), endDate: $.trim($('#dpPAExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpPAExpirationDate').val()) });
                }
            }
            else {
                if ((aa_eff >= pa_eff && aa_exp <= pa_exp)) {
                    if ($.trim($('#dpAAEffectiveDate').val()) != '') dateRanges.push({ future: null, name: drivingStatus.PAAA, startDate: new Date($('#dpAAEffectiveDate').val()), endDate: $.trim($('#dpAAExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpAAExpirationDate').val()) });
                } else if ($('#txtPriorAuth').val() != 'Yes') {
                    if ($.trim($('#dpAAEffectiveDate').val()) != '') dateRanges.push({ future: null, name: drivingStatus.AA, startDate: new Date($('#dpAAEffectiveDate').val()), endDate: $.trim($('#dpAAExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpAAExpirationDate').val()) });
                }
            }

            // If SOS with PA or AA with PA isn't assigned then assign PA
            if (dateRanges.length == 0) {
                dateRanges.push({ name: drivingStatus.PA, startDate: new Date($('#dpPAEffectiveDate').val()), endDate: $.trim($('#dpPAExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpPAExpirationDate').val()) });
            }
        } else {
            if ($.trim($('#dpAAEffectiveDate').val()) != '') dateRanges.push({ future: null, name: drivingStatus.AA, startDate: new Date($('#dpAAEffectiveDate').val()), endDate: $.trim($('#dpAAExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpAAExpirationDate').val()) });
        }

        // Adv and AdvSOS
        if ($.trim($('#dpAdvNotificationEffDate').val()) != '') {
            var adv_eff = new Date($("#dpAdvNotificationEffDate").val()),
                adv_exp = $.trim($("#dpAdvNotificationExpDate").val()) == '' ? new Date('12/31/2999') : new Date($("#dpAdvNotificationExpDate").val());

            if (sos_eff != 'Invalid Date' && adv_eff != 'Invalid Date') {
                if (sos_eff >= adv_eff && sos_exp <= adv_exp) {
                    dateRanges.push({ future: null, name: drivingStatus.AdvSOS, startDate: adv_eff, endDate: adv_exp });
                }
            }

            if (dateRanges.filter(o => o.name === drivingStatus.AdvSOS).length == 0) {
                dateRanges.push({ future: null, name: drivingStatus.Adv, startDate: adv_eff, endDate: adv_exp });
            }            
        }

        // Rest of the date pairs
        if ($.trim($('#dpPreDeterminationEffDate').val()) != '') dateRanges.push({ future: null, name: drivingStatus.Pred, startDate: new Date($('#dpPreDeterminationEffDate').val()), endDate: $.trim($('#dpPreDeterminationExpDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpPreDeterminationExpDate').val()) });        
        if ($.trim($('#dpDRALEffDate').val()) != '') dateRanges.push({ future: null, name: drivingStatus.DRAL, startDate: new Date($('#dpDRALEffDate').val()), endDate: $.trim($('#dpDRALExpDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpDRALExpDate').val()) });                
        if ($.trim($('#dpMSPEffectiveDate').val()) != '') dateRanges.push({ future: null,  name: drivingStatus.MSP, startDate: new Date($('#dpMSPEffectiveDate').val()), endDate: $.trim($('#dpMSPExpirationDate').val()) == '' ? new Date('12/31/2999') : new Date($('#dpMSPExpirationDate').val()) });
    }

    var dateRangesWithInToday = dateRanges;

    dateRangesWithInToday.forEach(function (range) {
        if (range.startDate <= today && range.endDate >= today) {
            activeDatePair = range;
            return range;
        }
    });

    var latestEndDate = new Date(0);

    function compareDates(a, b) {
        if (a.endDate < b.endDate || (a.endDate == b.endDate && a.startDate > b.startDate))
            return -1;
        if (a.endDate > b.endDate || (a.endDate == b.endDate && a.startDate < b.startDate))
            return 1;
        return 0;
    }

    dateRanges.sort(compareDates);

    //if there's not any date pair that has today's date then get the max of these dates
    //if (activeDatePair == null) {
        dateRanges.forEach(function (range) {
            if (range.endDate > latestEndDate) {
                latestEndDate = range.endDate;
                if (activeDatePair == null) {
                    activeDatePair = range; 
                } else {
                    if (range.startDate > Date.parse(TODAY_DATE)) { // Check if start date is in FUTURE then consider it as future record
                        activeDatePair.future = range
                    } else {
                        activeDatePair = range;
                    }
                }
            }
        });
    //}

    //USER STORY 130953 SUSPENSION FIELDS VALIDATION FOR OVERWRITE
    /*
    if ($('#txtSuspensionInd').val() == 'Yes') {
        activeDatePair = {};

        switch ($('#ddlSUSPType').val()) {
            case 'PA':
                activeDatePair = { future: null, name: drivingStatus.PAS, startDate: null, endDate: null };
                break;
            case 'PRE-D':
                activeDatePair = { future: null, name: drivingStatus.PredS, startDate: null, endDate: null };
                break;
            case 'DRAL':
                activeDatePair = { future: null, name: drivingStatus.DRALS, startDate: null, endDate: null };
                break;
            case 'AN':
                activeDatePair = { future: null, name: drivingStatus.AdvS, startDate: null, endDate: null };
                break;
            case 'AA':
                activeDatePair = { future: null, name: drivingStatus.AAS, startDate: null, endDate: null };
                break;
            case 'SOS':
                activeDatePair = { future: null, name: drivingStatus.SOSS, startDate: null, endDate: null };
                break;
            case 'MSP':
                activeDatePair = { future: null, name: drivingStatus.MSPS, startDate: null, endDate: null };
                break;
            default:
                break;
        }        
    } 
    */

    return activeDatePair;
}

function getEPALStatusName(currentStatus) {
    var ardfullName = '';
    switch (currentStatus) {
        case drivingStatus.PA:
            ardfullName = 'Prior Auth';
            break;
        case drivingStatus.PASOS:
            ardfullName = 'Prior Auth with SOS';
            break;
        case drivingStatus.PAAA:
            ardfullName = 'Prior Auth with Auto Approval';
            break;
        case drivingStatus.Pred:
            ardfullName = 'Pre-Determination';
            break;
        case drivingStatus.Adv:
            ardfullName = 'Advanced Notification';
            break;
        case drivingStatus.AdvSOS:
            ardfullName = 'Advanced Notification with SOS';
            break;
        case drivingStatus.DRAL:
            ardfullName = 'Drug Review At Launch';
            break;
        case drivingStatus.AA:
            ardfullName = 'Auto Approval';
            break;
        case drivingStatus.MSP:
            ardfullName = 'Medicare Special Processing';
            break;
        case drivingStatus.PAS:
            ardfullName = 'Prior Auth Suspension';
            break;
        case drivingStatus.PredS:
            ardfullName = 'Pre-Determination Suspension';
            break;
        case drivingStatus.DRALS:
            ardfullName = 'Drug Review At Launch Suspension';
            break;
        case drivingStatus.AdvS:
            ardfullName = 'Advanced Notification Suspension';
            break;
        case drivingStatus.AAS:
            ardfullName = 'Auto Approval Suspension';
            break;
        case drivingStatus.SOSS:
            ardfullName = 'Site of Service Suspension';
            break;
        case drivingStatus.MSPS:
            ardfullName = 'Medical Special Processing Suspension';
            break;
        default:
            break;
    }
    return ardfullName;
}