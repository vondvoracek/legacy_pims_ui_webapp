function redirectToHome() {
    //var base_url = window.location.origin;
    //window.location = VIRTUAL_DIRECTORY == '' || VIRTUAL_DIRECTORY == null ? base_url + '/' : base_url + '/' + VIRTUAL_DIRECTORY;
    //window.location = MIApp.Sanitize.decode($('#HomeUrl').val());// fix for https://github.optum.com/HCEMI-APP-DEV/pims-ui/security/code-scanning/72 MFQ 7/17/2023
    window.location = MIApp.Sanitize.decode(MIApp.Common.ApiEPRepository.getFromObject('home-url', BaseApiUrls));
}

$(document).ready(function () {
    $('#backToHome').click(function () {
        redirectToHome();
    });
});