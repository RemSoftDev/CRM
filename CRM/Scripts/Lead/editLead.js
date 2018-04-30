$(document).ready(function () {
//    $('#edit_button').on('click', function () {
//        var leadModel = {
//            name: $('#Name').val(),
//            email: $('#Email').val(),
//            phones: [{ phoneNumber: $('#PhoneNumber').val() }],
//            notes: getNotes()
//            //message: $('#message').val()
//        };
//        var leadId = $('#leadId').val();
//        $.ajax({
//            url: "/Lead/Edit",
//            type: 'POST',
//            data: {
//                lead: leadModel,
//                id: leadId
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

    $('#add_phone').on('click', function () {
        var phone = $('#phones .phone-type:last');
        if (wasPhoneFilled(phone)) {
            var index = phone.find('[data-type="newPhone"]').val() || -1;
            $(phone).after('<div class="col-md-10 col-md-offset-2 margin-bottom phone-type">' +
                '<input type="hidden" value= "' + ++index + '" data-type="newPhone" >' +
                '<input class="form-control margin-bottom" name="newPhones[' + index + '].PhoneNumber" type="text" data-id="phone_number" placeholder="Phone Number"> ' +
                '<select class="form-control" name="newPhones[' + index + '].Type" data-id="phone_type">' +
                '<option value="" disabled selected>Phone Type</option>' +
                '<option value="1">Home Phone</option>' +
                '<option value="2">Work Phone</option>' +
                '<option value="3">Mobile Phone</option>' +
                '<option value="4">Emergency Contact Phone</option>' +
                '<option value="5">Fax</option>' +
                '</select><span class="field-validation-valid text-danger" data-valmsg-for="NewCustomer.Phones[' + index + '].PhoneNumber" data-valmsg-replace="true"></span>' +
                '</div>');
        }
    });

    $('#send_message').on('click', function () {
        var message = $('#message').val();
        var leadId = $('#Id').val();
        if (!message) {
            alert("Message can't be empty!");
        }
        else {
            $.ajax({
                url: "/Email/SendMessage",
                type: 'POST',
                data: {
                    id: leadId,
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

function wasPhoneFilled(phone) {
    var flag = true;
    if (!(phone.find('[data-id="phone_number"]').val() && phone.find('[data-id="phone_type"]').val())) {
        flag = false;
    }
    return flag;
}

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