var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblRounds').DataTable({
        "ajax": {
            "url": "/Rounds/GetAll"
        },
        "columns": [
            { "data": "description", "width": "25%" },
            { "data": "complete", "width": "70%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a onClick=Delete('/Rounds/Delete/${data}')
                        class="btn text-danger"> <i class="fa-solid fa-trash-can"></i></a>
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