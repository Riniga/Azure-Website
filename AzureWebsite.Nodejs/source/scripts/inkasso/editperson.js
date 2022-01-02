
function GetPerson(personId)
{
    form_id.value = personId;
    fetch("http://localhost:7071/api/GetPerson?personId=" + form_id.value , { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            var form_name = document.getElementById("form_name");
            var form_adddebtbutton = document.getElementById("form_adddebtbutton");
            form_name.value = data.name;

            form_adddebtbutton.onclick = function () { window.location.href = '/inkasso/adddebt.html?personId=' + personId;}

        });
}

function GetPersonDebts(personId) {
    $('#debtstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetPersonDebts?personId=' + personId, dataSrc: "" },
            columns: [
                { data: 'contract.name' }
            ],
            columnDefs: [{
                targets: 1,
                data: 'id',
                orderable: false,
                render: function (data, type, row, meta) {
                    return '<a href="/inkasso/viewdebt.html?debtId=' + data + '">Open</a>';
                }
            }]
        }
    );
}

function UpdatePerson() {
    var form_name = document.getElementById("form_name");
    var form_id = document.getElementById("form_id");
    var data = "{ personId: '" + form_id.value + "', personName: '" + form_name.value + "' }";

    console.log(data);

    const response = fetch("http://localhost:7071/api/UpdatePerson",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: data
        })
    location.reload();
}

function GetPersonId() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('id');
}

$(document).ready(function ()
{
    const personId = GetPersonId();
    if (personId)
    {
        GetPerson(personId);
        GetPersonDebts(personId)
    }
    $('#form_save').click(UpdatePerson);
});


