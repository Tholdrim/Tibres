param appServicePlanName string
param functionAppName string
param keyVaultName string
param storageAccountName string

param location string
param keyVaultSecretUris object

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
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
      use32BitWorkerProcess: false
    }
  }

  resource metadata 'config@2022-03-01' = {
    name: 'metadata'
    properties: {
      CURRENT_STACK: 'dotnet-isolated'
    }
  }
}

module roleAssignments './role assignments.bicep' = {
  name: '${deployment().name}-RoleAssignments'
  params: {
    keyVaultName: keyVaultName
    storageAccountName: storageAccountName
    principalId: functionApp.identity.principalId
  }
}

module functionAppSettings './app settings.bicep' = {
  name: '${deployment().name}-AppSettings'
  params: {
    appServiceName: functionApp.name
    existingAppSettings: list('${functionApp.id}/config/appsettings', functionApp.apiVersion).properties
    newAppSettings: {
      AzureWebJobsStorage__accountName: storageAccountName
      DiscordPublicKey: '@Microsoft.KeyVault(SecretUri=${keyVaultSecretUris.discordPublicKey}/)'
      DiscordToken: '@Microsoft.KeyVault(SecretUri=${keyVaultSecretUris.discordToken}/)'
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: '@Microsoft.KeyVault(SecretUri=${keyVaultSecretUris.storageAccountConnectionString}/)'
      WEBSITE_CONTENTSHARE: functionApp.name
    }
  }
  dependsOn: [ roleAssignments ]
}
