$(function () {
    var callHub = $.connection.phoneHub;
    var isConnected = false;

    // Открываем соединение
    $.connection.hub.start().done(function () {
        isConnected = true;
    }).fail(function () {
        alert("Could not Connect!");
    });

    $.connection.hub.disconnected(function () {
        isConnected = false;
        setTimeout(function () {
            $.connection.hub.start().done(function () {
                isConnected = true;
            });
        }, 5000); // Restart connection after 5 seconds.
    });

    $("#btnLogin").click(function () {
        var userId = $("#dropUserList").val();
        var phoneNum = $("#phoneNum").val();

        if (userId == null) {
            alert("You need to select user!");
        }
        else if (phoneNum == null || phoneNum == "" || phoneNum == " ") {
            alert("You need to enter phone number!");
        } else {
            if (isConnected) {
                callHub.server.call(userId, phoneNum);
            }
        };
    });

    callHub.client.onCall = function (errMesage) {
        alert(errMesage);
    };
});