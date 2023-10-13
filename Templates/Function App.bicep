param appServicePlanName string
param functionAppName string

param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlan.id
    siteConfig: {
      ftpsState: 'Disabled'
      http20Enabled: true
      netFrameworkVersion: 'v7.0'
      functionAppScaleLimit: 1
      use32BitWorkerProcess: false
    }
  }

  resource metadata 'config@2022-09-01' = {
    name: 'metadata'
    properties: {
      CURRENT_STACK: 'dotnet-isolated'
    }
  }
}

output id string = functionApp.id
output principalId string = functionApp.identity.principalId
