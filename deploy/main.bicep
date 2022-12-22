param applicationName string
param location string = resourceGroup().location

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: applicationName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: applicationName
  location: location
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: applicationName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlan.id
    siteConfig: {
      use32BitWorkerProcess: false
    }
  }
}

var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value};EndpointSuffix=${environment().suffixes.storage}'

module appServiceSettings './app settings.bicep' = {
  name: '${deployment().name}-AppSettings'
  params: {
    appServiceName: functionApp.name
    existingAppSettings: list('${functionApp.id}/config/appsettings', functionApp.apiVersion).properties
    newAppSettings: {
      AzureWebJobsStorage: storageAccountConnectionString
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionString
      WEBSITE_CONTENTSHARE: functionApp.name
    }
  }
}
