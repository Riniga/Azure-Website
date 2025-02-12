# Setup environment 
## Förutsättningar
func installeras med: npm i -g azure-functions-core-tools@3 --unsafe-perm true

## Paket
dotnet add package Microsoft.Azure.Functions.Extensions

## Utvecklingsmiljöer (för test och utveckling)
starta med: func start --csharp
Debug genom att "Attach to process" -> välj func bland processer

## Configuration
Create file: local.settings.json
{
    ### Avaktivera kryptering
    "IsEncrypted": false,
    
    ### Lägg till för att möjliggöra CORS
    "Host": 
    {
        "CORS": "*"
    },
    "Values": 
    {
        "EndpointUrl": "https://azurewebsitecosmosdb.documents.azure.com:443",
        "PrimaryKey": "[Key]",
        "DatabaseId": "CosmosDatabase",
        "ContainerId": "Menu",

        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        //"StorageConnectionString": "UseDevelopmentStorage=true"
        "StorageConnectionString": "DefaultEndpointsProtocol=https;AccountName=azurewebsitestorage;AccountKey=[Key];EndpointSuffix=core.windows.net"
    }
}

# Test
Use Postman: Ange Parametrar -> Key/Value och json i body

## Produktionsmiljö 
Jag använder en publish profile, högerklicka och publish för att produktionssätta.
Aktuell miljö: SkanskaResourceGroup -> AzureFunctionApp (Function App)

# Offline
If you ever need to run this code offline, then you need to have 

### Deployment: Azure functions
az functionapp create -g AzureWebsite  -p AzureWebsitePlan -n AzureWebsiteApi -s azurewebsitestorage

## Azureit - Microsoft Azurite Emulator
npm install -g azurite
starta azureite med : azurite

## CosmosDB
För att emulera CosmosDB behvös Azure Cosmos DB Emulator https://aka.ms/cosmosdb-emulator
Databasen måste skapas: https://localhost:8081/_explorer/index.html
Update edpoint in local.settings

### Azure Storage Explorer
Använd Microsoft Azure Storage Explorer för att navigera i lokal miljö
Du behöver justera offentlig åtkomstnivå för Lagringskonton->Emulator...->Blob Containers->images 



