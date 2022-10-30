var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblStaff').DataTable({
        "ajax": {
            "url":"/Staffs/GetAll"
        },
        "columns": [
            {
                "data": "imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="30"/>';
                }, "width": "1%"
            },
            { "data": "name", "width": "15%" },
            { "data": "function.description", "width": "15%" },
            {
                "data": "team.imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="40"/>';
                }, "width": "1%"
            },
            { "data": "team.name", "width": "15%" },
            { "data": "country.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                        <a href="/Staffs/Edit?id=${data}"
                        class="btn btn-secondary btn-sm"> Edit </a>
                        </div>
                     `
                },
                "width": "5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                        <a href="/Staffs/Delete?id=${data}"
                        class="btn btn-danger btn-sm">Delete</a>
                        </div>
                     `
                },
                "width": "5%"
            },
        ]
    });
    order: [[1, 'asc']]
};