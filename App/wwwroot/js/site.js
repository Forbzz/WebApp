// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const AjaxBtnListener = function (a, placeholderElem, table, view) {
    var url = a.attr("href");
    placeholderElem.off('click');

    $.get(url).done(function (data) {
        placeholderElem.html(view.render(data));
        placeholderElem.find('.modal').modal('show');
        if (data.role) {
            $('select#modal-role').val(data.role);
        }
    });

    placeholderElem.on('click', 'button[type="submit"]', function (e) {
        e.preventDefault();
        var ajax_form = $("form#ajax-form");
        var data = ajax_form.serializeArray();
        var dataToSend = {}
        for (let i = 0; i < data.length; i++) {
            dataToSend[data[i].name] = data[i].value;
        }
        var actionUrl = ajax_form.attr("action");
        $.ajax({
            url: actionUrl,
            type: "post",
            data: dataToSend,
            success: function () {
                placeholderElem.find('.modal').modal('hide');
                if (table) {
                    table.ajax.reload();
                }
            },
            error: function (data) {
                var errorsData = JSON.parse(data.responseText);
                var errors = {};
                var ul = document.getElementById('errors');
                ul.innerHTML = "";
                ul.hidden = true;
                var validator = $("form#ajax-form").validate({
                    errorClass: "text-danger"
                });
                for (let i = 0; i < errorsData.length; i++) {
                    if (errorsData[i].property && errorsData[i].errors.length > 0) {
                        errors[errorsData[i].property] = errorsData[i].errors;
                    }
                    else {
                        ul.hidden = false;
                        for (let j = 0; j < errorsData[i].errors.length; j++) {
                            li = document.createElement('li');
                            li.innerHTML = errorsData[i].errors[j];
                            ul.appendChild(li);
                        }
                    }
                }
                validator.showErrors(errors);
            }
        });
    });
}

const createOptions = function (roles) {
    let select = document.createElement('select');
    select.classList.add('browser-default', 'custom-select');
    select.id = "modal-role";
    select.name = "Role";
    for (let i = 0; i < roles.length; i++) {
        let opt = document.createElement('option');
        opt.innerHTML = roles[i];
        select.appendChild(opt);
    }
    return select.outerHTML;
}