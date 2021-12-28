$(document).ready(function () {

    $('#form_save').click(function () {
        var form_name = document.getElementById("form_name");
        var form_id = document.getElementById("form_id");
        var data = "{ id: '" + form_id.value + "', name: '" + form_name.value + "' }";

        console.log(data);

        const response = fetch("http://localhost:7071/api/SavePerson",
            {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: data
            })
    });

    // Fill with person debts
    $('#debtstable_1').DataTable(
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
                    return '<a href="/inkasso/person.html?id=' + data + '">Open</a>';
                }
            }]
        }
    );
});

function GetPersonFromAzureFunction() {
    const queryString = window.location.search;
    
    const urlParams = new URLSearchParams(queryString);
    var form_id = document.getElementById("form_id");
    form_id.value = urlParams.get('id');
    if (form_id.value == "") return;
    fetch("http://localhost:7071/api/GetPerson?id=" + form_id.value , { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            var form_name = document.getElementById("form_name");
            form_name.value = data.name;
        });
}

GetPersonFromAzureFunction();