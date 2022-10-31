var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataMatches').DataTable({
        "ajax": {
            "url": "/Matches/GetMatchId",
        },
        "columns": [
            { "data": "round.description", "width": "15%" },
            {
                "data": "homeTeam.imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="40"/>';
                }, "width": "10%"
            },
            { "data": "homeTeam.name", "width": "20%" },
            { "data": "awayTeam.name", "width": "20%" },
            {
                "data": "awayTeam.imageURL",
                "render": function (data, type, row, meta) {
                    return '<img src="' + data + '" alt="' + data + '"height="50" width="40"/>';
                }, "width": "10%"
            },
            { "data": "matchDate", "width": "20%" },
            { "data": "homeTeam.venue", "width": "20%" },
        ]
    });
    order: [[0, 'asc']]
}