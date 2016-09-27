$(document).ready(function() {
    $("#pressme").click(function () {
        $("#output").append("You pressed me!<br>");
    });
    var getJQXHRAsNiceMessage = function(jqXHR) {
        var statusCode = jqXHR.statusCode();
        var responseHeaders = jqXHR.getAllResponseHeaders().replace(/(?:\r\n|\r|\n)/g, "<br />");
        var responseText = jqXHR.responseText.replace(/(?:\r\n|\r|\n)/g, "<br />");
        
        var niceMessage = "Status: " + statusCode.status + " " + statusCode.statusText + "<br />" +
            "Message:" + "<br />" +
            responseHeaders + "<br />" +
            responseText + "<br />";

        return niceMessage;
    };

    $("#requestForm").submit(function() {
        $("#method").attr('disabled','disabled');
        $("#url").attr('disabled','disabled');
        $("#send").attr('disabled', 'disabled');

        var method = $("#method").val();
        var extraHeaders = undefined;
        if (method === "PUT")
            extraHeaders = { 'x-my-custom-header': 'some value' }
        $.ajax({
            url: $("#url").val(),
            method: method,
            headers: extraHeaders
        }).done(function(data, textStatus, jqXHR) {
            $("#requestOutput").html(
                "Success:" + "<br />" +
                getJQXHRAsNiceMessage(jqXHR)
             );
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#requestOutput").html(
                "Error:" + "<br />" +
                getJQXHRAsNiceMessage(jqXHR)
             );            
        }).always(function(dataOrjqXHR, textStatus, jqXHROrErrorThrown ) { 
            $("#method").removeAttr('disabled');
            $("#url").removeAttr('disabled');
            $("#send").removeAttr('disabled');
        });

        return false;
    });
});