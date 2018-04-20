$(document).ready(function () {
    $('#convert_button').on('click', function () {
        var customerModel = {
            title: $('#title').val(),
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(),
            address: $('#address').val(),
        };
        $.ajax({
            url: "/Lead/ConvertLead",
            type: 'POST',
            data: { model : customerModel, id : $('#leadId').val()},
            success: function (response) {
                window.location = '/Customer';
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});