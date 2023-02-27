# Setup the Azure Environment 
This will create the resource group and create  all the resources for this Azure Website Projects

## Tools
- Visual Studio Code Extencion: Azure Resource Manager (ARM) Tools
- Azure CLI: https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?source=recommendations&tabs=azure-cli


## Deployemnt commands
- az login --tenant <Tenant ID found in Azure Active Directory - Properties>]  // 25d62b6c-6385-4a82-9e84-4b9804656dbf
- az group create --name AzureWebsite --location swedencentral
- az deployment group create --resource-group AzureWebsite --template-file azuredeploy.json --parameters azuredeploy.parameters.json

## Removment commnds
- az group delete --name AzureWebsite


## TODO:
- Setup Resource for Azur Functions
- Enable Static website for storage
- Create Cosmos database
- Deploy function
- Deploy website



