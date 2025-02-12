# Azure-Website
A full stack web project template for a static website utilizing the power of Microsoft Azure

# Azure Resources
I've set this up to work with Microsoft Azure you need to have an Azure Account.

## Tools
- Visual Studio Code Extencion: Azure Resource Manager (ARM) Tools
- Azure CLI: https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?source=recommendations&tabs=azure-cli
* Miniconda (https://docs.anaconda.com/miniconda/install/)
*

## Environment
I use Anaconda to create virtuela environments.
* conda update --all
* conda search python
* conda create -n azure python=3.13.2
* conda activate azure

## Modules
* Azure Cli(Not sure, I have it installed globaly): conda install azure-cli-core

## Azure Cli
- az --version
- az upgrade

## Azure Account
- Logion using dialog: az login --tenant <Tenant ID found in Azure Entra ID - Properties>
- Show my account details: az account show

## Resource Group
- List resource groups: az group list
- Create a new group: az group create --name AzureWebsite --location swedencentral

## App Service Plan 
- Create an app service plan in the environment: az appservice plan create -g AzureWebsite -n AzureWebsitePlan

## Storage Account
- az storage account create -n azurewebsitestorage -g AzureWebsite -l swedencentral --sku Standard_LRS
- To retrieve existing connectionstring: az storage account show-connection-string --name azurewebsitestorage --resource-group AzureWebsite

[//]: # sku {Premium_LRS, Premium_ZRS, Standard_GRS, Standard_GZRS, Standard_LRS, Standard_RAGRS, Standard_RAGZRS, Standard_ZRS}]

### Cosmos DB
- az cosmosdb create --name azurewebsitecosmosdb --resource-group AzureWebsite
- az cosmosdb keys list --name azurewebsitecosmosdb --resource-group AzureWebsite

### Storage account


## Deployment
- az deployment group create --resource-group AzureWebsite --template-file azuredeploy.json --parameters azuredeploy.parameters.json

## App Service Environment - Need to check this out a bit more for deployments and stuff...
- Create the network: az network vnet create -g AzureWebsite -n MyVirtualNetwork --address-prefixes 10.0.0.0/16 --subnet-name MyAseSubnet --subnet-prefixes 10.0.0.0/24
- Create an app service environment: az appservice ase create -n AzureWebsiteAppServiceEnvironment -g AzureWebsite --vnet-name MyVirtualNetwork --subnet MyAseSubnet
