$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var callHub = $.connection.phoneHub;

    // Открываем соединение
    $.connection.hub.start();

    callHub.client.onReciveCall = function (path) {
        window.location = path;
    }
});