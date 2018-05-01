$(document).ready(function () {
    $('#create_button').on('click', function () {
        var leadModel = {
            name: $('#name').val(),
            email: $('#email').val(),
            phones: [{ phoneNumber: $('#phone').val() }],
            message: $('#message').val()
        };
        $.ajax({
            url: "/api/webform",
            type: 'POST',
            data: leadModel,
            success: function (response) {
                if (response.status != "error") {
                    window.location = '/';
                }               
                alert(response.message);
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});