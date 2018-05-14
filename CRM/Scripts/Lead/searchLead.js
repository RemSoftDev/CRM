$(document).ready(function () {
    $('#search_button').on('click', function () {
        //var searchModel = {
        //    field: $('#search_field option:selected').val(),
        //    searchValue: $('#search_value').val(),
        //    orderField: $('#order_field option:selected').val(),
        //    orderDirection: $('#order_direction option:selected').val()
        //};
        search(getSearchModel());
    });


    $('.sortable').sortable();
});

function search(model) {
    $.ajax({
        url: "/Lead/Search",
        type: 'POST',
        data: model,
        success: function (response) {
            if (response.status != "error") {
                $('table').html($(response).filter('table')[0].outerHTML);
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function getSearchModel() {
    var columns = [];
    $('#columns li').each(function (i) {
        columns.push({
            field: $(this).find('input').val(),
            showOnGrid: $(this).find('input').is(':checked'),
            orderDirection: getOrderDirection($(this).find('input').val())
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

function SetOrderByField(field) {
    var searchModel = getSearchModel();
    searchModel.orderField = field;

    searchModel.orderDirection = setOrderDirection(field);
    search(searchModel);
}

function getOrderDirection(field) {
    var el = $('#' + field);
    var elClass = el.attr("class");
    if (elClass == 'sort') {
        return 0;
    }
    if (elClass == 'sort-up') {
        return 1;
    }
    if (elClass == 'sort-down'){
        return 2;
    }
}

function setOrderDirection(field) {
    var currentDirection = getOrderDirection(field);
    if (currentDirection == 0) {
        return 'ASC';
    }
    if (currentDirection == 1) {
        return 'DESC';
    }
    if (currentDirection == 2) {
        return 'ASC';
    }
}