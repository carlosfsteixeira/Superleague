var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsers').DataTable({
        "ajax": {
            "url": "/Account/GetListUsers"
        },
        "columns": [
            { "data": "firstName", "width": "15%" },
            { "data": "lastName", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "role", "width": "15%" },
            { "data": "teamId", "width": "15%" },
        ]
    });
    order: [[1, 'asc']]
};