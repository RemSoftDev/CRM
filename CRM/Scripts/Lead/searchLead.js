$(document).ready(function () {
    $('#search_button').on('click', function () {
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
                $('#pagination').html($(response).filter('#pagination')[0].outerHTML);
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function getSearchModel() {
    var columns = [];
    var currentPage = $('.active > a')[0];
    currentPage = currentPage ? currentPage.text.trim() : 1;

    $('#columns li').each(function (i) {
        columns.push({
            field: $(this).find('input').val(),
            showOnGrid: $(this).find('input').is(':checked'),
            orderDirection: getOrderDirection($(this).find('input').val())
        })
    });
    return {
        searchValue: $('#search_value').val(),
        page: currentPage,
        itemsPerPage: $('#items_per_page  option:selected').val(),
        orderField : '',
        orderDirection: '',
        columns: columns,
        field: $('#search_field option:selected').val()
    };
}

function SetOrderByField(field) {
    var orderDirection = setOrderDirection(field);
    var searchModel = getSearchModel();

    searchModel.orderField = field;
    searchModel.orderDirection = orderDirection;
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

    var el = $('#' + field);
    $('thead td').not('#' + field).each(function () {
        $(this).attr('class', 'sort')
    });

    if (currentDirection == 0) {
        el.attr("class", 'sort-up')
        return 'ASC';
    }
    if (currentDirection == 1) {
        el.attr("class", 'sort-down')
        return 'DESC';
    }
    if (currentDirection == 2) {
        el.attr("class", 'sort-up')
        return 'ASC';
    }
}

function LoadPage(page) {
    var searchModel = getSearchModel();
    searchModel.page = page;
    search(searchModel);
}