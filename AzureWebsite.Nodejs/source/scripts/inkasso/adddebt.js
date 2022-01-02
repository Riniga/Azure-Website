
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
            });
        });
}


function CreateDebt() {
    var form_personId = document.getElementById("form_personId");
    var form_contract = document.getElementById("form_contract");
    var form_amount = document.getElementById("form_amount");
    var data = "{ personId: '" + form_personId.value + "', contractId: '" + form_contract.value + "', amount: '" + form_amount.value + "'  }";
    console.log(data);
    const response = fetch("http://localhost:7071/api/CreateDebt",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: data
        })
    window.location.href = "/inkasso/editperson.html?id=" + form_personId.value ;
}

function GetPersonId() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('personId');
}

$(document).ready(function ()
{
    var form_personId = document.getElementById("form_personId");
    form_personId.value = GetPersonId();
    GetContracts();
    $('#form_save').click(CreateDebt);
});


