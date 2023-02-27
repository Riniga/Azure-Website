
##Förutsättningar
func installeras med: npm i -g azure-functions-core-tools@3 --unsafe-perm true

##Utvecklingsmiljöer (för test och utveckling)
starta med: func start --csharp
Debug genom att "Attach to process" -> välj func bland processer

##Konfigurationer finnas i local.settings.json
{
    ### Avaktivera kryptering
    "IsEncrypted": false,
    
    ###Lägg till för att möjliggöra CORS
    "Host": 
    {
        "CORS": "*"
    },
    "Values": 
    {
      ###Åtkomst till Cosmos DB emulator med följande konfiguration
        "EndpointUrl": "https://localhost:8081",
        "PrimaryKey": "[Key from site above]",
        "DatabaseId": "CosmosDatabase",
        "ContainerId": "Menu",
        ### Åtkomst till Azure Storage
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "StorageConnectionString": "UseDevelopmentStorage=true"
        //"StorageConnectionString": "DefaultEndpointsProtocol=https;AccountName=azureexperiments;AccountKey=[Se connection string under Access Keys ];EndpointSuffix=core.windows.net"
    }
}

# Miljöer
##Utvecklingsmiljö

### Azureit
För att kunna köra exemplet fullt ut i lokal miljö krävs Microsoft Azurite Emulator
npm install -g azurite
starta azureite med : azurite

### CosmosDB
För att emulera CosmosDB behvös Azure Cosmos DB Emulator https://aka.ms/cosmosdb-emulator
Databasen måste skapas: https://localhost:8081/_explorer/index.html

### Azure Storage Explorer
Använd Microsoft Azure Storage Explorer för att navigera i lokal miljö
Du behöver justera offentlig åtkomstnivå för Lagringskonton->Emulator...->Blob Containers->images 


##Produktionsmiljö 
Jag använder en publish profile, högerklicka och publish för att produktionssätta.
Aktuell miljö: SkanskaResourceGroup -> AzureFunctionApp (Function App)


# Test
## Använd PostMan: Ange Parametrar -> Key/Value och json i body

