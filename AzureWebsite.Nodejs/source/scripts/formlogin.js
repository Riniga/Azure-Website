function signInOrOut()
{
    const currentUser = localStorage.getItem('currentUser');
    if (currentUser) signOut();
    else signIn();
}


function signIn()
{
    var username = document.getElementById('formloginusername').value ;
    var password = document.getElementById('formloginpassword').value ;
    var data = "{ username: '" + username + "', password: '" + password + "'  }";

    fetch("http://localhost:7071/api/Login",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/text',
            },
            body: data
        })
        .then(response => response.json())
        .then(data =>
        {
            console.log(data);
            localStorage.setItem('currentUser', JSON.stringify(data) );
            CheckLoggedInUser();
        });
}

function signOut() 
{
    const currentUser = localStorage.getItem('currentUser');

    fetch("http://localhost:7071/api/Logout",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/text',
            },
            body: currentUser
        })
        .then(response => response.json())
        .then(data =>
        {
            localStorage.removeItem('currentUser');
            CheckLoggedInUser();
        });
}

function createUser()
{
    var username = document.getElementById('formcreateuserusername').value ;
    var name = document.getElementById('formcreatename').value ;
    var password = document.getElementById('formcreateuserpassword').value ;

    var data = "{ username: '" + username + "', password: '" + password + "', name: '" + name + "'  }";

    fetch("http://localhost:7071/api/CreateUser",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/text',
            },
            body: data
        })
        .then(response => response.json())
        .then(data =>
        {
            console.log(data);
        });
}
function CheckLoggedInUser()
{
    const currentUser = localStorage.getItem('currentUser');

    if (currentUser) document.getElementById('formloginbutton').innerText = "Log Out";
    else document.getElementById('formloginbutton').innerText = "Logga in";
    document.getElementById("json").innerHTML = currentUser;
}

CheckLoggedInUser();

var button = document.getElementById('formloginbutton');
    button.addEventListener("click", signInOrOut);

var button = document.getElementById('formcreateuserbutton');
    button.addEventListener("click", createUser);
