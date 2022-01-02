$(document).ready(function () {
    $('#personstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetPersons', dataSrc: "" },
            columns: [
                { data: 'name' },
                { data: 'id' }
            ],
            columnDefs: [{
                targets: 1,
                data: 'id',
                orderable: false,
                render: function (data, type, row, meta) {
                    return '<a href="/inkasso/editperson.html?id=' + data + '">Open</a>';
                }
            }]
        }
    );
});
