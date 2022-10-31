function signIn()
{
    var username = document.getElementById('login_form_email').value ;
    var password = document.getElementById('login_form_password').value ;
    var data = "{ username: '" + username + "', password: '" + password + "'  }";
    document.getElementById("ajaxLoader").style.visibility = "visible";

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
            localStorage.setItem('currentUser', JSON.stringify(data) );
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        })
        .catch((error) => {
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        });
}

function signOut() 
{
    const currentUser = localStorage.getItem('currentUser');
    document.getElementById("ajaxLoader").style.visibility = "visible";
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
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        })
        .catch((error) => {
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        });
}

function createUser()
{
    var username = document.getElementById('register_form_email').value ;
    var name = document.getElementById('register_form_name').value ;
    var password = document.getElementById('register_form_password').value ;

    var data = "{ username: '" + username + "', password: '" + password + "', name: '" + name + "'  }";
    document.getElementById("ajaxLoader").style.visibility = "visible";
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
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        })
        .catch((error) => {
            refresh();
            document.getElementById("ajaxLoader").style.visibility = "hidden";
        });
}


function toogleVisibleForm(form)
{
    hideAll();
    document.getElementById(form).style.display = "block"; 
}


function hideAll()
{
    document.getElementById('login_form').style.display = "none";
    document.getElementById('forgotpassword_form').style.display = "none";
    document.getElementById('logout_form').style.display = "none";
    document.getElementById('register_form').style.display = "none";
}

function refresh()
{
    currentUser = localStorage.getItem('currentUser');
    

    if (currentUser)
    {
        toogleVisibleForm('logout_form');
        document.getElementById('loginLink').innerHTML  = "Logga ut " + JSON.parse(currentUser).name;
    }
    else
    {
        toogleVisibleForm('login_form');
        document.getElementById('loginLink').innerHTML  = "Logga in";
    }
    $('#loginModal').modal('hide');
}


var button = document.getElementById('login_form_button');
    button.addEventListener("click", signIn);


var button = document.getElementById('register_form_button');
    button.addEventListener("click", createUser);

    var button = document.getElementById('logout_form_button');
    button.addEventListener("click", signOut);



refresh();