var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblFunctions').DataTable({
        "ajax": {
            "url": "/Functions/GetAll"
        },
        "columns": [
            { "data": "description", "width": "95%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn btn-sm btn-group" role="group">
                        <a href="/Functions/Edit?id=${data}"
                        class="btn text-primary"> <i class="fa-solid fa-pen-to-square"></i> </a>
                        <a onClick=Delete('/Functions/Delete/${data}')
                        class="btn text-danger"> <i class="fa-solid fa-trash-can"></i> </a>
                    </div>
                     `
                },
                "width": "5%"
            }
        ]
    });
    order: [[0, 'asc']]
};

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