
function CreatePayment() {
    var form_debtId = document.getElementById("form_debtId");
    var form_amount = document.getElementById("form_amount");
    var data = "{ debtId: '" + form_debtId.value + "', amount: '" + form_amount.value + "'  }";
    const response = fetch("http://localhost:7071/api/CreatePaymentTransaction",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: data
        })
    window.location.href = "/inkasso/viewdebt.html?debtId=" + form_debtId.value ;
}

function GetDebtId() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('debtId');
}

$(document).ready(function ()
{
    var form_debtId = document.getElementById("form_debtId");
    form_debtId.value = GetDebtId();
    
    $('#form_save').click(CreatePayment);
});


