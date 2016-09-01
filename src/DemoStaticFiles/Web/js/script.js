$(document).ready(function() {
    $("#pressme").click(function () {
        $("#output").append("You pressed me!<br>");
    });
    $("#requestForm").submit(function() {
        $("#method").attr('disabled','disabled');
        $("#url").attr('disabled','disabled');
        $("#send").attr('disabled','disabled');
        $.ajax({
            url: $("#url").val(),
            method: $("#method").val()
        //     ,
        //     xhrFields: {
        //         withCredentials: true
        // }
        }).done(function(data, textStatus, jqXHR) {
            $("#requestOutput").html(data);
        }).fail(function(jqXHR, textStatus, errorThrown) {

        }).always(function(dataOrjqXHR, textStatus, jqXHROrErrorThrown ) { 
            $("#method").removeAttr('disabled');
            $("#url").removeAttr('disabled');
            $("#send").removeAttr('disabled');
        });

        return false;
    });
});