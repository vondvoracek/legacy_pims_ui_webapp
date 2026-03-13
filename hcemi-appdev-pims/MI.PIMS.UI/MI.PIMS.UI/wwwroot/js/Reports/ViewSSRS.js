var _reportPath;

var reportsController = {
    init: function () {
        //reportsController.bind();
    },
    //bind: function () {
    //    var clientHeight = document.documentElement.clientHeight;
    //    var header = $("#navbarHeader").outerHeight(true);
    //    var breadcrumbHeader = $("#breadcrumbHeader").outerHeight(true);
    //    var navReportTab = $("#navReportTab").outerHeight(true);
    //    var divReportDDL = $("#divReportDDL").outerHeight(true);
    //    var footer = $('#footer').outerHeight(true);
    //    var reportHeight = clientHeight - header - breadcrumbHeader - navReportTab - divReportDDL - footer - 15;
    //    $('#reportFrame').height(reportHeight + 'px');
    //},
    dropdown: {
        onChangeSelection: function (e) {
            var dataItem = e.sender.dataItem();
            _reportPath = dataItem.Report_Path;
            reportsController.report.loading(dataItem.Report_Name, dataItem.Report_Path);
        }
        //,onDataBound: function (e) {
        //    $ddlReport = $("#ddlReport").data("kendoDropDownList");
        //    if ($ddlReport.select() === 0) { //check whether any item is selected 
        //        $ddlReport.select(1);
        //    }

        //    //var dataItem = e.sender.dataItem();
        //    //reportsController.report.loading(dataItem.Report_Name, dataItem.Report_Path);
        //}
    },
    report: {
        loading: function (_name, _path) {

            // feature-eissecuritylog
            eisSecurity.log("PageAccess:SUCCESS with /Reports/Home?ReportName=" + _name + "&rowCount=1", 1);
            MICore.Api.post("/Reports/Home/AddUserAccess", { 'ReportName': _name });

            var _ssrsServer = _path.split('|')[0];
            var _ssrsReport = _path.split('|')[1];

            //var url = 'http://' + _ssrsServer + "/Reports/report/UHC%20Employer%20Individual%20and%20Clinical%20Services/HCE-MI/SCANS_Exchange/" + _ssrsReport.replace(" ", "+");            
            var url = 'http://' + _ssrsServer + "/ReportServer/Pages/ReportViewer.aspx?%2fUHC%20Employer%20Individual%20and%20Clinical%20Services%2fHCE-MI%2fSCANS_Exchange%2f" + _ssrsReport.replace(" ", "+");//+ "&rs:Command=Render&rs:ClearSession=true";
            window.open(url, 'mywindow', 'scrollbars=yes,resizable=yes,toolbar=yes');


            //Swal.fire({
            //    title: 'Loading..',
            //    text: 'Loading selected report, please wait....',
            //    timerProgressBar: true,
            //    allowOutsideClick: false,
            //    willOpen: () => {
            //        Swal.showLoading();

            //        // feature-eissecuritylog
            //        eisSecurity.log("PageAccess:SUCCESS with /Reports/Home?ReportName=" + _name + "&rowCount=1", 1);

            //        MICore.Api.post("/Reports/Home/AddUserAccess", { 'ReportName': _name }, function (retVal) {
            //            var _ssrsServer = _path.split('|')[0];
            //            var _ssrsReport = _path.split('|')[1];
            //            //var ssrsURL = 'http://' + _ssrsServer + "/ReportServer/Pages/ReportViewer.aspx?%2fUHC%20Employer%20Individual%20and%20Clinical%20Services%2fHCE-MI%2fSCANS_Exchange%2f" + _ssrsReport.replace(" ", "+") + "&rs:Command=Render" + "&rs:ClearSession=true" + "&rc:Zoom=100"; // + "&rc:Parameters=Collapsed"
            //            //$("#reportFrame").attr('src', ssrsURL);

            //            var url = 'http://' + _ssrsServer + "/ReportServer/Pages/ReportViewer.aspx?%2fUHC%20Employer%20Individual%20and%20Clinical%20Services%2fHCE-MI%2fSCANS_Exchange%2f" + _ssrsReport.replace(" ", "+") + "&rs:Command=Render" + "&rs:ClearSession=true" + "&rc:Zoom=100";
            //            window.open(url, "_blank");
            //            Swal.close();
            //        });

            //    }
            //})
        },
        viewReport: function () {
            if (_reportPath == undefined) {
                MICore.Notification.warning(null, "Please select a report to view.");
                return;
            }
            var reportName = $("#ddlReport").data("kendoDropDownList").text();
            reportsController.report.loading(reportName, _reportPath);
        }
    }
};

$(document).ready(function () {
    reportsController.init();
});

