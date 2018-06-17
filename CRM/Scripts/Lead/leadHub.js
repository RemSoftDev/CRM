$(function () {
    // reference on proxy hub
    var phoneHub = $.connection.phoneHub;

    //Open connection
    $.connection.hub.start().done(function () {
        phoneHub.server.addToGroup('lead');

        var id = $('#Id').val();
        if (id != null) {
            phoneHub.server.lock(id, 'lead');
        }

    }).fail(function () {
        alert("Could not Connect!");
    });

    // Function for redirect
    phoneHub.client.onReciveCall = function (path) {
        window.location = path;
    };

    phoneHub.client.LockEdit = function (lockModel) {
        $('#edit_' + lockModel.Id).hide();
        $('#convert_' + lockModel.Id).hide();
        $('#img_' + lockModel.Id).attr('title', 'Locked by: ' + lockModel.LockName).show();
    };

    phoneHub.client.LockList = function (lockModel) {
        for (var i = 0; i < lockModel.length; i++) {
            $('#edit_' + lockModel[i].Id).hide();
            $('#convert_' + lockModel[i].Id).hide();
            $('#img_' + lockModel[i].Id).attr('title', 'Locked by: ' + lockModel[i].LockName).show();
        }
    };

    phoneHub.client.UnLockEdit = function (id) {
        $('#edit_' + id).show();
        $('#convert_' + id).show();
        $('#img_' + id).hide();
    };
});

$(window).bind('beforeunload', function () {
    var phoneHub = $.connection.phoneHub;

    $.connection.hub.start().done(function () {
        var id = $('#Id').val();
        if (id != null) {
            phoneHub.server.unLock(id, 'lead');
        }

    }).fail(function () {
        alert("Could not Connect!");
    });
});