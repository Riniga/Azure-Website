
function GetDebt(debtId)
{
    fetch("http://localhost:7071/api/GetDebt?debtId=" + debtId, { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            var form_contract = document.getElementById("form_contract");
            form_contract.value = data.contract.name;
            form_addpaymentbutton.onclick = function () { window.location.href = '/inkasso/addpayment.html?debtId=' + debtId;}
        });
}

function GetDebtTransactions(debtId) {
    $('#debtstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetTransactionsForDebt?debtId=' + debtId, dataSrc: "" },
            columns: [
                { data: 'timeStamp' },
                { data: 'transactionType' },
                { data: 'amount' }
            ],
            columnDefs: [{
                targets: 1,
                data: 'transactionType',
                orderable: false,
                render: function (data, type, row, meta) {
                    return GetTransactionType(data);
                }
            }]
        }
    );
}

function GetTransactionType(transactionTypeInt)
{
    switch (transactionTypeInt) {
        case 0: return "SetBalance";
        case 1: return "Fee";
        case 2: return "Payment";
        case 3: return "Interest";
        default: return "Unknown";

    }
}

function GetDebtId() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('debtId');
}

$(document).ready(function ()
{
    const debtId = GetDebtId();
    if (debtId )
    {
        GetDebt(debtId);
        GetDebtTransactions(debtId)
    }
});


