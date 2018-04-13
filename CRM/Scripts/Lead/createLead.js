$(document).ready(function () {
    $('#create_button').on('click', function () {
        var leadModel = {
            name: $('#name').val(),
            email: $('#email').val(),
            phone: $('#phone').val(),
            message: $('#message').val()
        };
        $.ajax({
            url: "/api/webform",
            type: 'POST',
            data: leadModel,
            success: function (response) {
                //window.location.reload();
                alert(response.message);
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});