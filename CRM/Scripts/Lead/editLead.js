$(document).ready(function () {
    $('#edit_button').on('click', function () {
        var leadModel = {
            name: $('#Name').val(),
            email: $('#Email').val(),
            phone: $('#Phone').val(),
            notes: getNotes()
            //message: $('#message').val()
        };
        var oldPhone = $('#oldPhone').val();
        $.ajax({
            url: "/Lead/Edit",
            type: 'POST',
            data: {
                lead: leadModel,
                oldPhone: oldPhone
            },
            success: function (response) {
                window.location = '/';
            },
            error: function (error) {
                alert(error);
            }
        });
    });

    $('#add_note').on('click', function () {
        var notes = $('#notes textarea');
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
        var oldPhone = $('#oldPhone').val();
        if (!message) {
            alert("Message can't be empty!");
        }
        else {
            $.ajax({
                url: "/Lead/SendMessage",
                type: 'POST',
                data: {
                    oldPhone : oldPhone,
                    message: message
                },
                success: function (response) {
                    alert("Message was sent successfully!");
                },
                error: function (error) {
                    alert(error);
                }
            });
        }
    });
});

function getNotes() {
    var notes = $('[name="note"]');
    var notesValue = [];
    if (notes.length > 0) {
        for (var i = 0; i < notes.length; i++) {
            if (notes[i].value != '') {
                notesValue.push(notes[i].value);
            }
        }
    }
    return notesValue;
}