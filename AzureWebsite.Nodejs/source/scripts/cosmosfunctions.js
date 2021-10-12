function CreateMenu()
{

}

$("#createnewmenu").click(function () {
    

    fetch(MainMenuUpdateApiUrl, { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            console.log("Oh yeh");
        });

});