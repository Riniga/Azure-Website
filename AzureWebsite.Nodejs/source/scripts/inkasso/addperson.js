
function GetContracts()
{
    fetch("http://localhost:7071/api/GetContracts", { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            var form_contract = document.getElementById("form_contract");
            $.each(data, function (index, value) {
                var option = document.createElement("option");
                option.text = value.name;
                option.value = value.id;
                form_contract.add(option);
                console.log(value);
            });
        });
}


function CreatePerson() {
    var form_name = document.getElementById("form_name");
    var form_contract = document.getElementById("form_contract");
    var form_amount = document.getElementById("form_amount");
    var data = "{ personName: '" + form_name.value + "', contractId: '" + form_contract.value + "', amount: '" + form_amount.value + "'  }";
    const response = fetch("http://localhost:7071/api/CreatePerson",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: data
        })
    window.location.href = "/inkasso/persons.html";
}

function GetPersonId() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('id');
}

$(document).ready(function ()
{
    GetContracts();
    $('#form_save').click(CreatePerson);
});


