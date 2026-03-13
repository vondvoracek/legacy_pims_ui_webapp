
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
                            window.location.href = MIApp.Common.ApiEPRepository.get('viewdetail', 'DpocUrls').replace('__pims_id__', MIApp.Sanitize.encode(pims_id));
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
                        window.location.href = MIApp.Common.ApiEPRepository.get('viewdetail', 'DpocUrls').replace('__pims_id__', MIApp.Sanitize.encode(pims_id));
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
                            window.location.href = MIApp.Common.ApiEPRepository.get('editdetail', 'DpocUrls').replace('__pims_id__', MIApp.Sanitize.encode(pims_id));
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
                        window.location.href = MIApp.Common.ApiEPRepository.get('editdetail', 'DpocUrls').replace('__pims_id__', MIApp.Sanitize.encode(pims_id));
                    }
                })
            }
        },
    },
    grid: {
        param: function () {
            var paramObj = {
                p_DPOC_HIERARCHY_KEY: MIApp.Sanitize.encodeProp($.trim($("#txtPIMSID").val())),
                p_DPOC_VER_EFF_DT: null,
                p_DPOC_PACKAGE: MIApp.Sanitize.encodeProp($('#txtDPOCPackage').val())
            }
            return paramObj;
        },
        onDataBound: function (e) {
            var grid = e.sender.wrapper;
            var gridContent = e.sender.wrapper.find(".k-grid-content");
            grid.height("400px");
            gridContent.height("310px");
        }
    }
}
