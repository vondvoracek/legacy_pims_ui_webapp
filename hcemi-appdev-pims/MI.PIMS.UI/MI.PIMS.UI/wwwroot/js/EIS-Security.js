var eisSecurity = {
    log: function (requestWithParameters, rowCount) {
        // feature-eissecuritylog 11/25/2020 MFQ
        $.ajax({
            type: "GET",
            url: VIRTUAL_DIRECTORY + "EISSecurity/LogRequestData?requestWithParameters=" + requestWithParameters + "&rowCount=" + rowCount,
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: {
                'Accept': 'application/json'
            },
            async: false,
            success: function (msg) {
                //alert(msg)
            },
            error: function (e) {
                //debugger;
            }
        });
    }
}