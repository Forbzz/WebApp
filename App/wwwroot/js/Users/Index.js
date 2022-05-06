let returnUrl;

$(document).ready(() => {
    var table = $('#users-table').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        autoWidth: false,
        "language": {
            "sProcessing": "Подождите...",
            "sLengthMenu": "Показать _MENU_ записей",
            "sZeroRecords": "Записи отсутствуют.",
            "sInfo": "Записи с _START_ до _END_ из _TOTAL_ записей",
            "sInfoEmpty": "Записи с 0 до 0 из 0 записей",
            "sInfoFiltered": "(отфильтровано из _MAX_ записей)",
            "sInfoPostFix": "",
            "sSearch": "Поиск:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Первая",
                "sPrevious": "Предыдущая",
                "sNext": "Следующая",
                "sLast": "Последняя"
            },
            "oAria": {
                "sSortAscending": ": активировать для сортировки столбца по возрастанию",
                "sSortDescending": ": активировать для сортировки столбцов по убыванию"
            }
        },
        "ajax": {
            "url": "/Users/LoadUsers",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "email", "name": "email" },
            { "data": "name1", "name": "name1" },
            { "data": "name2", "name": "name2" },
            { "data": "name3", "name": "name3" },
            { "data": "role", "name": "role", sortable: false },
            {
                "render": (data, type, full, meta) => {
                    let group = document.createElement('div');
                    group.classList.add('btn-group');

                    var infoBtn = document.createElement('a');
                    infoBtn.classList.add('btn', 'btn-outline-primary', 'mx-1');
                    infoBtn.innerHTML = `<i class="fa fa-info"></i>`;
                    infoBtn.href = '/Users/Details/' + full.id;

                    var updateBtn = document.createElement('a');
                    updateBtn.classList.add('btn', 'btn-primary', 'mx-1', 'usr-update');
                    updateBtn.innerHTML = `<i class="fa fa-pencil"></i>`;
                    updateBtn.href = '/Users/Edit/' + full.id + '?returnUrl=' + returnUrl;

                    var removeBtn = document.createElement('a');
                    removeBtn.classList.add('btn', 'btn-outline-danger', 'mx-1', 'usr-del');
                    removeBtn.innerHTML = `<i class="fa fa-trash"></i>`;
                    removeBtn.href = '/Users/Remove/' + full.id;

                    group.appendChild(infoBtn);
                    group.appendChild(updateBtn);
                    group.appendChild(removeBtn);

                    return group.outerHTML;
                },
                sortable: false,
            }
        ]
    });

    

    $("#backBtn").on('click', (e) => {
        e.preventDefault();
        window.history.go(-1);
        return false;
    });
    $("#users-table").on('click', 'a.usr-del', function (e) {
        e.preventDefault();
        href = $(this).attr("href");
        bootbox.confirm("Удалить пользователя. Вы уверены?", function (result) {
            if (result) {
                $.get(href).done(function (data) {
                    if (data.status == "success") {
                        table.ajax.reload();
                        console.log("Check")
                    }
                    else {
                        bootbox.alert(data.error);
                    }
                });
            }
        });
    })

});