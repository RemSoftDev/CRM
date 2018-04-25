$(document).ready(function () {
    //$('#convert_button').on('click', function () {
    //    var customerModel = {
    //        title: $('#title').val(),
    //        firstName: $('#firstName').val(),
    //        lastName: $('#lastName').val(),
    //        address: $('#address').val(),
    //    };
    //    $.ajax({
    //        url: "/Lead/ConvertLead",
    //        type: 'POST',
    //        data: { model : customerModel, id : $('#leadId').val()},
    //        success: function (response) {
    //            window.location = '/Customer';
    //        },
    //        error: function (error) {
    //            alert(error);
    //        }
    //    });
    //});

    $('#add_phone').on('click', function () {
        var phone = $('#phones .phone-type:last');
        if (wasPhoneFilled(phone)) {
            var index = phone.find('[data-type="newPhone"]').val() || -1;
            $(phone).after('<div class="col-md-10 col-md-offset-2 margin-bottom phone-type">' +
                '<input name="newPhones[' + ++index + '].Id" type="hidden" value= "' + index + '" data-type="newPhone" >' +
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

    $('#add_address').on('click', function () {
        var address = $('#addresses .address-type:last');
        if (wasAddressFilled(address)) {
            var index = address.find('[data-type="address"]').val() || -1;
            $(address).after('<div class="col-md-10 col-md-offset-2 margin-bottom address-type">' +
                '<input name="newAddress[' + ++index + '].Id" type="hidden" value="' + index + '" data-type="address">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].Line1" type="text" placeholder="Line 1" data-id="line1">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].Line2" type="text" placeholder="Line 2" data-id="line2">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].Town" type="text" placeholder="Town" data-id="town">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].County" type="text" placeholder="County" data-id="county">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].PostCode" type="text" placeholder="Post Code" data-id="postcode">' +
                '<input class="form-control margin-bottom" name="newAddress[' + index + '].Country" type="text" placeholder="Country" data-id="country">' +
                '<select class="form-control" name="newAddress[' + index + '].Type" data-id="type">' +
                '<option value="" disabled selected>Address Type</option>' +
                '<option selected="selected" value="">Address Type</option>' +
                '<option value="1">Billing Address</option>' +
                '<option value="2">Contact Address</option>' +
                '<option value="3">Emergency Contact Address</option>' +
                '</select>' +
                '</div>');
        }
    });
});

function wasAddressFilled(address) {
    var flag = true;
    if (!address.find('[data-id="type"]').val()) {
        flag = false;
    }
    return flag;
}

function wasPhoneFilled(phone) {
    var flag = true;
    if (!(phone.find('[data-id="phone_number"]').val() && phone.find('[data-id="phone_type"]').val())) {
        flag = false;
    }
    return flag;
}