$(document).ready(function () {

    $('#add_note').on('click', function () {
        var notes = $('#notes > textarea');
        if (notes.length == 0) {
            $('#notes button').before('<textarea class="form-control margin-bottom" name="note"></textarea>');
        }
        else {
            var lastNote = $('#notes textarea:last');
            if (lastNote.val() != '') {
                $(lastNote).after('<textarea class="form-control margin-bottom" name="note"></textarea>');
            }          
        }
    });

    $('#send_message').on('click', function () {
        var message = $('#message').val();
        var customerId = $('#Id').val();
        if (!message) {
            alert("Message can't be empty!");
        }
        else {
            $.ajax({
                url: "/Email/SendMessage",
                type: 'POST',
                data: {
                    id: customerId,
                    message: message
                },
                success: function (response) {
                    $('#message').val('');
                    alert("Message was sent successfully!");
                },
                error: function (error) {
                    alert(error);
                }
            });
        }
    });
});