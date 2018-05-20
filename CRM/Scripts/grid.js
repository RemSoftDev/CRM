$(document).ready(function () {

    $('#profiles').on('change', function () {
        LoadProfile($("option:selected", this).val());
    });

    $("#save_dialog").dialog({
        autoOpen: false,
        open: function (e) { },
        modal: true,
        show: "blind",
        hide: "blind",
        buttons: [
            {
                text: "Cancel",
                "class": 'btn btn-default',
                click: function () {
                    closeSaveDialog();
                }
            },
            {
                text: "Save",
                "class": 'btn btn-default',
                click: function () {
                    saveProfile($('#profile_name').val());
                }
            }
        ]
    });

    $("#edit_dialog").dialog({
        autoOpen: false,
        open: function (e) { },
        modal: true,
        show: "blind",
        hide: "blind",
        buttons: [
            {
                text: "Cancel",
                "class": 'btn btn-default',
                click: function () {
                    closeEditDialog();
                }
            },
            {
                text: "Save",
                "class": 'btn btn-default',
                click: function () {
                    editProfile();
                }
            }
        ]
    });

    $('#new_profile_button').on('click', function () {
        $("#save_dialog").dialog("open");
        var target = $(this);
        $("#save_dialog").dialog("widget");
        return false;
    });

    $('#edit_profile_button').on('click', function () {
        $("#edit_dialog").dialog("open");
        var target = $(this);
        $("#edit_dialog").dialog("widget");
        return false;
    });

    applyjQueryFunctions();
});

function closeSaveDialog() {
    $('#save_dialog span').hide();
    $('#profile_name').val('');
    $('#save_dialog').dialog('close')
}

function closeEditDialog() {
    $('#edit_dialog span').hide();
    $('#edit_dialog').dialog('close')
}

function saveProfile(profileName) {
    $.ajax({
        url: getPath() + "/CreateProfile",
        type: 'POST',
        data: {
            model: getSearchModel(),
            profileName: profileName
        },
        success: function (response) {
            if (response.success) {
                closeSaveDialog();

                $('#profiles').append(
                    '<option selected>' + profileName + '</option>'
                );
            }
            else {
                $('#save_dialog span').show();
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function editProfile() {
    $.ajax({
        url: getPath() + "/EditProfile",
        type: 'POST',
        data: {
            model: getSearchModel(),
            makeDefault: $('#profile_default').is(':checked')
        },
        success: function (response) {
            if (response.success) {
                closeEditDialog();

                $('#profile_default').prop('checked', false);
            }
            else {
                $('#edit_dialog span').show();
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function search(model) {
    $.ajax({
        url: getPath() + "/Search",
        type: 'POST',
        data: model,
        success: function (response) {
            drawPage(response);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function getSearchModel() {
    var columns = [];
    var profiles = [];
    var currentPage = $('.active > a')[0];
    currentPage = currentPage ? currentPage.text.trim() : 1;

    var orderField = $('thead td').not('.sort').find('span').text()
    var orderDirection = getOrderDirection(orderField) == '2' ? 'DESC' : 'ASC';

    $('#columns li').each(function (i) {
        columns.push({
            field: $(this).find('input').val(),
            showOnGrid: $(this).find('input').is(':checked'),
            orderDirection: getOrderDirection($(this).find('input').val()),
            order: i
        })
    });

    $('#profiles option').each(function (i) {
        profiles.push({
            profileName: $(this).val(),
            isDefault: $(this).is(':selected')
        })
    });
    return {
        searchValue: $('#search_value').val(),
        page: currentPage,
        itemsPerPage: $('#items_per_page  option:selected').val(),
        orderField: orderField,
        orderDirection: orderDirection,
        columns: columns,
        profiles: profiles,
        field: $('#search_field option:selected').val()
    };
}

function SetOrderByField(field) {
    var orderDirection = setOrderDirection(field);
    var searchModel = getSearchModel();

    search(searchModel);
}

function getOrderDirection(field) {
    if (!field) {
        return;
    }

    var el = $('#' + field);
    var elClass = el.attr("class");
    if (elClass == 'sort') {
        return 0;
    }
    if (elClass == 'sort-up') {
        return 1;
    }
    if (elClass == 'sort-down') {
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

function LoadProfile(profile) {
    $.ajax({
        url: getPath() + "/LoadProfile",
        type: 'POST',
        data: { profileName: profile },
        success: function (response) {
            drawPage(response);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function drawPage(response) {
    if (response.status != "error") {
        $('table').html($(response).filter('table').html());
        $('#pagination').html($(response).filter('#pagination').html());
        $('#search_row').html($(response).filter('#search_row').html());

        applyjQueryFunctions();
    }
}

function applyjQueryFunctions() {
    $('.sortable').sortable();

    $('#search_button').on('click', function () {
        search(getSearchModel());
    });

    $('.dropdown-menu').bind(
        'click',
        function (e) {
            e.stopPropagation()
        }
    );
}

function getPath() {
    var path = location.pathname;
    if (location.pathname == '/') {
        path += 'Lead'
    }

    return path;
}