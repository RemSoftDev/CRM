$(document).ready(function () {
    $('#search_button').on('click', function () {
        //var searchModel = {
        //    field: $('#search_field option:selected').val(),
        //    searchValue: $('#search_value').val(),
        //    orderField: $('#order_field option:selected').val(),
        //    orderDirection: $('#order_direction option:selected').val()
        //};
        $.ajax({
            url: "/Lead/Search",
            type: 'POST',
            data: getSearchModel(),
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

function getSearchModel() {
    var columns = [];
    $('#columns li').each(function (i) {
        columns.push({
            key: $(this).find('input').val(),
            value: $(this).find('input').is(':checked')
        })
    });
    return {
        searchValue: $('#search_value').val(),
        page : '1',
        itemsPerPage: $('#items_per_page  option:selected').val(),
        orderField : '',
        orderDirection: '',
        columns : columns
    };
}