function getFiles()
{
    var pageUrl = "https://azureexperiments.blob.core.windows.net/rickarddemo";
    $.ajax({
        type: "GET", url: pageUrl, data: {
            restype: "directory",
            comp: "list"
        },
        dataType: "xml",
        success: function (response) {
            $(response).find("Blob").each(function () {
                var name = $(this).find('Name').text();
                console.log(name);
                var url = $(this).find('Url').text();
                $("#filelist").append("<img src='" + url + "' alt='" + name + "' width='100px'/>");
            });
            $("div#result").html("Resultat: Ok, det gick bra!");
        },
        error: function (response) {
            //debugger;
            console.log(response);
            $("div#result").html("Resultat: Dao, det gick inte så bra!");
        }
    });
}

window.addEventListener('load', getFiles);

