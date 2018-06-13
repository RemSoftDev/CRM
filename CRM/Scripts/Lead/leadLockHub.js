$(function () {
    // reference on proxy hub
    // and add queryString parameters
    $.connection.hub.qs = { 'action': 'lead' };
    var lockHub = $.connection.lockHub;

    // Open connection
    $.connection.hub.start().done(function () {

        var id = $('#leadId').val();
        lockHub.server.lock(id);

    }).fail(function () {
        alert("Could not Connect!");
    });

    //$('#send_message').on('click', function () {

    //    var id = $('#leadId').val();
    //    lockHub.server.UnLock(id); 
    //});
});

$(window).unload(function () {

    $.connection.hub.qs = { 'action': 'lead' };

    $.connection.hub.start();

    var lockHub = $.connection.lockHub;

    var id = $('#leadId').val();
    lockHub.server.UnLock(id);
});

//$(document).ready(function () {
//    // reference on proxy hub
//    var lockHub = $.connection.lockHub;

//    // Open connection
//    $.connection.hub.start().fail(function () {
//        alert("Could not Connect!");
//    });

//    lockHub.client.LockEdit = function (lockModel) {

//    };

//    lockHub.client.UnLockEdit = function (id) {

//    };
//})

