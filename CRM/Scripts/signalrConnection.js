$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var callHub = $.connection.phoneHub;

    // Open connection
    $.connection.hub.start().fail(function () {
        alert("Could not Connect!");
    });

    // Function for redirect
    callHub.client.onReciveCall = function (path) {
        window.location = path;
    };

    // Script for reconnection
    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 3000); // Restart connection after 5 seconds.
    });

});