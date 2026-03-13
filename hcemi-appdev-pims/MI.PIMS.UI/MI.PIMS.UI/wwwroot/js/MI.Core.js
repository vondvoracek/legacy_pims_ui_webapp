/**
 * MI.Core
 * MFQ
 * (c) UHC E&I
 * Require: jQuery.version.js, PNotify.js and Kendoui.js, global variable: SERVICE_URL
 * Consists following modules:
 *      Api
 *      Controls
 *      Notification
 *      Valdiation
*/

VIRTUAL_DIRECTORY = VIRTUAL_DIRECTORY == '' || VIRTUAL_DIRECTORY == null ? '' : '/' + VIRTUAL_DIRECTORY;

var MICore = (function ($) {

    /**
     * End-Point call functions
     * */
    var Api = {
        call: function (url, ajaxType, data, callback, elemForLoader, async, disableButton, disableNgProgress, disableError) {
            //if (disableNgProgress != undefined && disableNgProgress != false) NProgress.start();
            ajaxType = ajaxType == null ? "GET" : ajaxType;
            //data = data == null ? null : JSON.stringify(data);
            async = async == null || async == undefined ? true : async;
            jQuery.ajaxSettings.traditional = true;

            var token = $('meta[name="request-verification-token"]')[0].content;

            $.ajax({
                url: url,
                type: ajaxType,
                data: data,
                //dataType: "json",
                //contentType: "application/json",
                headers: {
                    'Accept': 'application/json',
                    'RequestVerificationToken': token
                },
                cache: false,
                async: async,
                success: function (data) {
                    //if (disableNgProgress != undefined && nudisableNgProgress != false) NProgress.done();
                    if (disableButton != null && disableButton != undefined) $(disableButton).attr('disabled', false);
                    callback != null ? callback(data) : null;
                    if (elemForLoader != null && elemForLoader != undefined) kendo.ui.progress(elemForLoader, false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (elemForLoader != null && elemForLoader != undefined) kendo.ui.progress(elemForLoader, false);
                    if (disableButton != null && disableButton != undefined) $(disableButton).attr('disabled', false);
                    if (thrownError && disableError !== true) {
                        MICore.Notification.error(
                            'Error',
                            thrownError.toLowerCase() == 'internal server error' ? 'An error occured while performing operation, Please try again or contact system administrator if it persist!' : thrownError
                        );
                    }
                    //NProgress.done();
                }
            });
        },
        get: function (url, data, callback, elemForLoader, disableButton, disableError) {
            if (elemForLoader != null && elemForLoader != undefined) kendo.ui.progress(elemForLoader, true);
            Api.call(url, 'GET', data, callback, elemForLoader, null, disableButton); //SERVICE_URL
        },
        post: function (url, data, callback, elemForLoader, disableButton, disableError) {
            Api.call(url, 'POST', data, callback, elemForLoader, null, disableButton); //SERVICE_URL 
        },
        put: function (url, data, callback, elemForLoader, disableButton, disableError) {
            Api.call(url, 'PUT', data, callback, elemForLoader, null, disableButton); //SERVICE_URL
        },
        delete: function (url, data, callback, elemForLoader, disableButton, disableError) {
            Api.call(url, 'DELETE', data, callback, elemForLoader, null, disableButton); //SERVICE_URL
        },
        logError: function () {
            Api.call(UI_URL + url, 'POST', data, callback);
        },
        poll: function (url, data, callback) {
            var self = this;
            self.get(url, data, callback);
        }
    };

    var checkInputs = function (elements) {
        elements.each(function () {
            var element = $(this);
            var input = element.children("input");

            input.prop("checked", element.hasClass("k-state-selected"));
        });
    };

    /**
    /**
     * SweetAlert2
     * success, error, warning
     * http://sweetalert2.github.io
     * Added polyfill-3.14.1/promise-min.js for IE ES6 Support
     * */
    var Notification = {
        success: function (title, html, callback, position, allowOutsideClick) {
            Swal.fire({
                icon: 'success',
                title: title,
                html: html,                
                position: position,
                customClass: 'swal-size-sm',
                allowOutsideClick: allowOutsideClick == null || allowOutsideClick == undefined ? true : allowOutsideClick
            }).then(function (result) {
                if (result.value || result.dismiss == 'backdrop') {
                    if (callback && {}.toString.call(callback) === '[object Function]') {
                        callback();
                    }
                }
            });
        },
        error: function (title, text, callback) {
            Swal.fire({
                icon: 'error',
                title: title,
                text: text
            }).then(function (result) {
                if (result.value) {
                    if (callback && {}.toString.call(callback) === '[object Function]') {
                        callback();
                    }
                }
            });
        },
        warning: function (title, text, callback, html) {
            Swal.fire({
                icon: 'warning',
                title: title,
                text: text,
                html: html,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    if (callback && {}.toString.call(callback) === '[object Function]') {
                        callback('warning');
                    }
                }
            });
        },
        question: function (title, text, confirmButtonText, cancelButtonText, callback, cancelEvent) {
            confirmButtonText = confirmButtonText == null ? 'Yes' : confirmButtonText;
            cancelButtonText = cancelButtonText == null ? 'Cancel' : cancelButtonText;

            Swal.fire({
                icon: 'question',
                title: title,
                //text: text,
                html: text,
                showCancelButton: true,
                confirmButtonText: confirmButtonText,
                cancelButtonText: cancelButtonText,
                customClass: 'swal-size-sm'
            }).then(function (result) {
                if (result.value) {
                    callback();
                }
                else {
                    if (cancelEvent) {
                        cancelEvent();
                    }
                }
            });
        },
        successPageReload: function (title, text) {
            Swal.fire({
                title: title,
                html: text,
                icon: 'success',
                showCancelButton: false,
                confirmButtonText: 'OK',
                allowOutsideClick: false,
            }).then(function (result) {
                if (result.value) {
                    location.reload();
                }
            });
        },

        whileSaving: function (title, callback) {
            title = title == undefined || title == null ? 'Saving record...' : title;
            Swal.fire({
                title: title,
                text:"Please wait...",
                onBeforeOpen: function () {
                    Swal.showLoading();
                    setTimeout(function () {
                        if (callback && typeof callback == "function") callback();
                    }, 50);
                },
                allowOutsideClick: function () {
                    return !Swal.isLoading();
                }
            });
        },

        set: function (message, type, callback, allowOutsideClick) {
            if (type.toLowerCase() === 'success')
                Notification.success('Update!', message, callback, null, null, allowOutsideClick);
            else if (type.toLowerCase() === 'warning')
                Notification.warning(null, message, callback);
            else if (type.toLowerCase() === 'error')
                Notification.error(null, message, callback);
            else
                Notification.warning(null, message, callback);
        }
    };

    var Validation = {
        set: function (formSelector, customRule) {

            /* Bind Mutation Events */
            var elements = $(formSelector).find("[data-role=autocomplete],[data-role=combobox],[data-role=dropdownlist],[data-role=numerictextbox]");

            //correct mutation event detection
            var hasMutationEvents = ("MutationEvent" in window),
                MutationObserver = window.WebKitMutationObserver || window.MutationObserver;

            if (MutationObserver) {
                var observer = new MutationObserver(function (mutations) {
                    var idx = 0,
                        mutation,
                        length = mutations.length;

                    for (; idx < length; idx++) {
                        mutation = mutations[idx];
                        if (mutation.attributeName === "class") {
                            updateCssOnPropertyChange(mutation);
                        }
                    }
                }),
                    config = { attributes: true, childList: false, characterData: false };

                elements.each(function () {
                    observer.observe(this, config);
                });
            } else if (hasMutationEvents) {
                elements.bind("DOMAttrModified", updateCssOnPropertyChange);
            } else {
                elements.each(function () {
                    this.attachEvent("onpropertychange", updateCssOnPropertyChange);
                });
            }

            function updateCssOnPropertyChange(e) {
                var element = $(e.target || e.srcElement);

                element.siblings("span.k-dropdown-wrap")
                    .add(element.parent("span.k-numeric-wrap"))
                    .toggleClass("k-invalid", element.hasClass("k-invalid"));
            }

            //bind validator to the form
            var validator;

            /*
            if (customRule == undefined) customRule = {};
            customRule['errorTemplate'] = _errorTemplate;
            validator = $(formSelector).kendoValidator(customRule).data("kendoValidator");
            */

            ///*
            validator = customRule == undefined || customRule == null ?
                $(formSelector).kendoValidator().data("kendoValidator") :
                $(formSelector).kendoValidator(customRule).data("kendoValidator");
            //*/            

            return validator;
        }
    };

    var Utility = {
        stringToBinary: function (str) {
            var result = [];
            for (var i = 0; i < str.length; i++) {
                result.push(str.charCodeAt(i).toString(2));
            }
            return result;
        },
        binaryToString: function (data) {
            const extraByteMap = [1, 1, 1, 1, 2, 2, 3, 0];
            var count = data.length;
            var str = "";

            for (var index = 0; index < count;) {
                var ch = data[index++];
                if (ch & 0x80) {
                    var extra = extraByteMap[(ch >> 3) & 0x07];
                    if (!(ch & 0x40) || !extra || ((index + extra) > count))
                        return null;

                    ch = ch & (0x3F >> extra);
                    for (; extra > 0; extra -= 1) {
                        var chx = data[index++];
                        if ((chx & 0xC0) != 0x80)
                            return null;

                        ch = (ch << 6) | (chx & 0x3F);
                    }
                }

                str += String.fromCharCode(ch);
            }

            return str;
        }
    };

    return {
        Api: Api,
        Notification: Notification,        
        Validation: Validation.set,
        Utility: Utility
    };

})(jQuery);

/**
 * PopOvers
 * options for type are: success, info, warning, danger
 * eg: $('.testWidget').popover({ type: 'danger', text: 'Dangerous' });
 * */
(function ($) {
    $.fn.popover = function (options) {

        // default options.
        var settings = $.extend({
            type: 'success',
            text: 'Please add your message.'
        }, options);

        _template = function () {
            return '<div class="alert alert-' + settings.type + ' alert-dismissible fade in" role="alert">\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span>\
                                </button>\
                                '+ settings.text + '\
                           </div>'
        }

        $(this).html(_template);
        return this;

    };
}(jQuery));
