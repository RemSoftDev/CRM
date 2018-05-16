$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var callHub = $.connection.phoneHub;

    callHub.client.onReciveCall = function (path) {
        console.log(path[0].Name);
        console.log(path[0].ConnectionId);

        //window.location = path;
    }

    // Открываем соединение
    $.connection.hub.start().done(function () {
        // вызов у хаба метод connect
        callHub.server.connect();
    });
});