$(document).ready(function () {
    GetNewEmails();
    //setInterval(function () { GetNewEmails(); }, 60 * 1000);
});

function GetNewEmails() {
    $.ajax({
        url: "/Email/GetEmails",
        type: 'POST',
        data: {
            id: $('#Id').val(),
            type: $('#type').val()
        },
        success: function (response) {
            if (response.mailings) {
                var lastMail = $('[data-type="email"]:last')
                lastMail.after(GetMailingsHtml(response.mailings));
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function GetMailingsHtml(emails) {
    var html = '';
    for (var i = 0; i < emails.length; i++) {
        html += '<div class="mail-box col-sm-offset-8" data-type="email">';
        html += emails[i].Body;
        html += '</div>';
    }
    return html;
}