$(document).ready(function () {
    $('#search_button').on('click', function () {
        var searchModel = {
            field: $('#search_field option:selected').val(),
            searchValue: $('#search_value').val(),
            orderField: $('#order_field option:selected').val(),
            orderDirection: $('#order_direction option:selected').val()
        };
        $.ajax({
            url: "/Lead/Search",
            type: 'POST',
            data: searchModel,
            success: function (response) {
                if (response.status != "error") {
                    $('table').html(response);
                }               
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});