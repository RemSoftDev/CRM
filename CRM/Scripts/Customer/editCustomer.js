$(document).ready(function () {
//    $('#edit_button').on('click', function () {
//        var customerModel = {
//            title: $('#Title').val(),
//            firstName: $('#FirstName').val(),
//            lastName: $('#LastName').val(),
//            email: $('#Email').val(),
//            phones: [{ PhoneNumber: $('#PhoneNumber').val() }],
//            notes: getNotes()
//        };
//        var customerId = $('#customerId').val();
//        $.ajax({
//            url: "/Customer/Edit",
//            type: 'POST',
//            data: {
//                customer: customerModel,
//                id: customerId
//            },
//            success: function (response) {
//                window.location = '/';
//            },
//            error: function (error) {
//                alert(error);
//            }
//        });
//    });

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
        var customerId = $('#customerId').val();
        if (!message) {
            alert("Message can't be empty!");
        }
        else {
            $.ajax({
                url: "/Customer/SendMessage",
                type: 'POST',
                data: {
                    id: customerId,
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

//function getNotes() {
//    var notes = $('[name="note"]');
//    var notesValue = [];
//    if (notes.length > 0) {
//        for (var i = 0; i < notes.length; i++) {
//            if (notes[i].value != '') {
//                notesValue.push(notes[i].value);
//            }
//        }
//    }
//    return notesValue;
//}