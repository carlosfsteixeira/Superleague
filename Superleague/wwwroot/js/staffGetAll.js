var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblStaff').DataTable({
        "ajax": {
            "url": "/Staffs/GetAll"
        },
        "columns": [
            {
                "data": "imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="30"/>';
                }, "width": "5%"
            },
            { "data": "name", "width": "15%" },
            { "data": "function.description", "width": "15%" },
            {
                "data": "team.imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="40"/>';
                }, "width": "5%"
            },
            { "data": "team.name", "width": "15%" },
            { "data": "country.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Staffs/Edit?id=${data}"
                        class="btn btn-info"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Staffs/Delete/${data}')
                        class="btn btn-danger text-white"> <i class="bi bi-trash-fill"></i> Del</a>
                    </div>
                     `
                },
                "width": "15%"
            }
        ]
    });
    order: [[0, 'asc']]
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "POST",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                }
            });
        }
    })
}