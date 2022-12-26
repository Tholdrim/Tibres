param applicationName string
param location string = resourceGroup().location
param tenantId string = subscription().tenantId

@secure()
param discordPublicKey string

@secure()
param discordToken string

var keyVaultSecretsUserRoleId = '4633458b-17de-408a-b874-0445c86b69e6'
var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value};EndpointSuffix=${environment().suffixes.storage}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: applicationName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    allowBlobPublicAccess: false
  }
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

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: applicationName
  location: location
  properties: {
    enableRbacAuthorization: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
  }

  resource discordPublicKeySecret 'secrets@2022-07-01' = {
    name: 'DiscordPublicKey'
    properties: {
      contentType: 'string'
      value: discordPublicKey
    }
  }

  resource discordTokenSecret 'secrets@2022-07-01' = {
    name: 'DiscordToken'
    properties: {
      contentType: 'string'
      value: discordToken
    }
  }
}

resource keyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVaultSecretsUserRoleId, functionApp.id, keyVault.id)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', keyVaultSecretsUserRoleId)
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

module appServiceSettings './app settings.bicep' = {
  name: '${deployment().name}-AppSettings'
  params: {
    appServiceName: functionApp.name
    existingAppSettings: list('${functionApp.id}/config/appsettings', functionApp.apiVersion).properties
    newAppSettings: {
      AzureWebJobsStorage: storageAccountConnectionString
      DiscordPublicKey: '@Microsoft.KeyVault(SecretUri=${keyVault::discordPublicKeySecret.properties.secretUri})'
      DiscordToken: '@Microsoft.KeyVault(SecretUri=${keyVault::discordTokenSecret.properties.secretUri})'
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionString
      WEBSITE_CONTENTSHARE: functionApp.name
    }
  }
}
