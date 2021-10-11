
##Förutsättningar
func installeras med: npm i -g azure-functions-core-tools@3 --unsafe-perm true

##Utvecklingsmiljöer (för test och utveckling)
starta med: func start --csharp
Debug genom att "Attach to process" -> välj func bland processer

##Konfigurationer finnas i local.settings.json
###Lägg till för att möjliggöra CORS
  "Host": {
    "CORS": "*"
  }

###Åtkomst till Cosmos DB emulator med följande konfiguration...
  "Values": {
    "EndpointUrl": "https://localhost:8081",
    "PrimaryKey": "SomeKey",
    "DatabaseId": "TheIDForDatabase",
    "ContainerId": "TheIDForContainer"
  }
  
### Åtkomst till Azure Storage
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "StorageConnectionString": "UseDevelopmentStorage=true"
    //"StorageConnectionString": "DefaultEndpointsProtocol=https;AccountName=azureexperiments;AccountKey=[Se connection string under Access Keys ];EndpointSuffix=core.windows.net"
  }


  {
  "IsEncrypted": false,
  
}

# Miljöer
##Utvecklingsmiljö
För att kunna köra exemplet fullt ut i lokal miljö krävs Microsoft Azurite Emulator
npm install -g azurite
starta azureite med : azurite

Använd Microsoft Azure Storage Explorer för att navigera i lokal miljö
Du behöver justera offentlig åtkomstnivå för Lagringskonton->Emulator...->Blob Containers->images 




##Produktionsmiljö 
Jag använder en publish profile, högerklicka och publish för att produktionssätta.
Aktuell miljö: SkanskaResourceGroup -> AzureFunctionApp (Function App)



