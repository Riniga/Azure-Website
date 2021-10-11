function getFiles() {
    var pageUrl = "http://localhost:7071/api/ListAllBlobs";

    fetch(pageUrl, { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            data.forEach(function (file) {
                let image = document.createElement("img");
                image.src = file[1];
                image.alt = file[0];
                document.getElementById("filelist").append(image);
            });
            document.getElementById("result").innerHTML = "Resultat: Ok, det gick bra!";
        });
}

function upload(file) {
    fetch('http://localhost:7071/api/UploadBlobItem?name=' + file.name,
        {
            method: 'POST',
            headers:
            {
                "Content-Type": "undefined"
            },
            body: file
        })
        .then(success => console.log(success), error => console.log(error))
};


window.addEventListener('load', getFiles);
var input = document.getElementById('fileItem')
const onSelectFile = () => upload(input.files[0]);
input.addEventListener('change', onSelectFile, false);