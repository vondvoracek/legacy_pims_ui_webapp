
var UserAccessHistController = {
    add: function (data) {        
        MICore.Api.post($('#UserAccessHistAddUrl').val(), data, null, null);
    }
}