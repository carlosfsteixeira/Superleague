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
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="40"/>';
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
            { "data": "country.name", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Staffs/Edit?id=${data}"
                        class="btn text-primary"> <i class="fa-solid fa-pen-to-square"></i> </a>
                        <a onClick=Delete('/Staffs/Delete/${data}')
                        class="btn text-danger"> <i class="fa-solid fa-trash-can"></i> </a>
                    </div>
                     `
                },
                "width": "5%"
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