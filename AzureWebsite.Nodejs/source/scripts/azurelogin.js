var ua = window.navigator.userAgent;
var msie = ua.indexOf('MSIE ');
var msie11 = ua.indexOf('Trident/');
var msedge = ua.indexOf('Edge/');
var isIE = msie > 0 || msie11 > 0;
var isEdge = msedge > 0;

var msalConfig = 
{
    auth: 
    {
        clientId: '0c306a20-e773-43fb-86ce-ed3b5ea94f59', //Application (client) ID
        authority: 'https://login.microsoftonline.com/25d62b6c-6385-4a82-9e84-4b9804656dbf', //Directory (tenant) ID
        redirectURI: 'http://localhost:3000/login.html'
    },
    cache: 
    {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: isIE || isEdge
    }
};

var graphConfig = 
{
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me",
    requestObj: {
        scopes: ["user.read"]
    }
};

var msalApplication = new msal.PublicClientApplication(msalConfig);
var userName = "";
var loginType = isIE ? "REDIRECT" : "POPUP";

msalApplication.handleRedirectPromise()
    .then(handleResponse)
    .catch(function (error) { console.log(error); });

function updateUserInterface() 
{
    var divWelcome = document.getElementById('WelcomeMessage');
    divWelcome.innerHTML = 'Welcome <strong>' + userName + '</strong> to Microsoft Graph API';
    var loginbutton = document.getElementById('azureloginbutton');
    loginbutton.innerHTML = 'Sign Out from Microsoft Account';
    loginbutton.setAttribute('onclick', 'signOut();');
}

function acquireTokenAndGetUser() 
{
    var request = graphConfig.requestObj;
    request.account = msalApplication.getAccountByUsername(userName);
    msalApplication.acquireTokenSilent(request)
        .then(function (tokenResponse) {
            getUserFromMSGraph(tokenResponse.accessToken, graphAPICallback);
        })
        .catch(function (error) {
            console.log("silent token acquisition fails.");
            if (error instanceof msal.InteractionRequiredAuthError) {
                if (loginType == "POPUP") {
                    msalApplication.acquireTokenPopup(request)
                    .then(function (tokenResponse) {
                        getUserFromMSGraph(tokenResponse.accessToken, graphAPICallback);
                    })
                    .catch(function (error) { console.error(error); }
                    );
                } 
                else {
                    msalApplication.acquireTokenRedirect(request);
                }
            } 
            else {
                console.error(error);
            }
        });
}

function getUserFromMSGraph(accessToken, callback) 
{
    var endpoint = graphConfig.graphMeEndpoint;
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200)
        callback(JSON.parse(this.responseText));
    }
    xmlHttp.open("GET", endpoint, true);
    xmlHttp.setRequestHeader('Authorization', 'Bearer ' + accessToken);
    xmlHttp.send();
}

function graphAPICallback(data) 
{
    document.getElementById("json").innerHTML = JSON.stringify(data, null, 2);
}

function handleResponse(loginResponse) {
    if (loginResponse != null) {
    userName = loginResponse.account.username;
    } 
    else
    {
        var currentAccounts = msalApplication.getAllAccounts();
        if (currentAccounts == null || currentAccounts.length == 0) 
        {
            return;
        } 
        else {
            userName = currentAccounts[0].username;
        }
    }
    updateUserInterface();
    acquireTokenAndGetUser();
}

function signIn() 
{
    if (loginType == "POPUP") 
    {
        msalApplication.loginPopup(graphConfig.requestObj)
        .then(handleResponse)
        .catch(function (error) { console.log(error); });
    } 
    else 
    {
        msalApplication.loginRedirect(graphConfig.requestObj);
    }
}

function signOut() 
{
    var logoutRequest = {
    account: msalApplication.getAccountByUsername(userName)};
    msalApplication.logout(logoutRequest);
}