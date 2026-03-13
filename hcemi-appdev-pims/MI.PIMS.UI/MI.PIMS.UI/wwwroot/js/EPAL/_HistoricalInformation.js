

var _HistoricalInformationController = {
    methods: {
        redirect: function (pims_id) {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to leave?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Redirecting..',
                        text: 'Redirecting to record, please wait..',
                        timerProgressBar: true,
                        //timer: 5000,
                        timer: 60000, 
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();
                            window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                        }
                    })
                }
            }
            else if (!isDirty) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to record, please wait..',
                    timerProgressBar: true,
                    //timer: 5000,
                    timer: 60000, 
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/ViewDetail/" + encodeURIComponent(pims_id);
                    }
                })
            }


        },
        redirect_edit: function (pims_id) {
            if (isDirty) {
                if (confirm("Changes you made may not be saved. \n\nAre you sure you want to leave?") == true) {
                    $(window).unbind('beforeunload');
                    Swal.fire({
                        title: 'Redirecting..',
                        text: 'Redirecting to record, please wait..',
                        timerProgressBar: true,
                        //timer: 5000,
                        timer: 60000, 
                        allowOutsideClick: false,
                        willOpen: () => {
                            Swal.showLoading();
                            window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);
                        }
                    })
                }
            }
            else if (!isDirty) {
                Swal.fire({
                    title: 'Redirecting..',
                    text: 'Redirecting to record, please wait..',
                    timerProgressBar: true,
                    //timer: 5000,
                    timer: 60000, 
                    allowOutsideClick: false,
                    willOpen: () => {
                        Swal.showLoading();
                        window.location.href = VIRTUAL_DIRECTORY + "/EPAL/Home/EditDetail/" + encodeURIComponent(pims_id);
                    }
                })
            }
        },
    },
    grid: {
        param: function () {            
            var paramObj = {
                p_EPAL_HIERARCHY_KEY: $.trim($("#txtPIMSID").val())
            }
            return MIApp.Sanitize.encodeObject(paramObj);
        },

        onDataBound: function (e) {
            var appRoleIds = $("#txtAppRoleIds").val();
            var appRoleArray = []
            appRoleArray.push(appRoleIds.split(','))

            var gridRows = this.tbody.find("tr");
            gridRows.each(function (e) {
                var entity = $(this).find("#lblEntity").text();
                var is_Current = $(this).find("#lblIs_Current").text();
                var hierarchy = $(this).find("#lblHierarchy").text();

                // DY: 01/28/2024 - Only allow Super Admins or EPAL Admins to see the pencil icon. 
                if ((jQuery.inArray("1", appRoleArray[0]) !== -1) || (jQuery.inArray("4", appRoleArray[0]) !== -1)) {

                // DY: 12/18/2023 - Commented out other roles to only allow super admins to see the pencil icon.
                //if ((jQuery.inArray("1", appRoleArray[0]) !== -1)) {
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

                        //==============
                        // UMR Records
                        //==============
                        if (entity === 'UMR' && (UMREdit === true)) {
                            $(this).closest('tr').find('.showPencil').show();
                        }
                        else if (entity !== 'UMR' && (EPALEdit === true)) {
                            $(this).closest('tr').find('.showPencil').show();
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
    }
}
