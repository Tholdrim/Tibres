param applicationInsightsName string
param keyVaultName string
param storageAccountName string

param location string

@secure()
param botPublicKey string

@secure()
param botToken string

@secure()
param serverId string

var deployServerIdSecret = !empty(serverId)

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    enableRbacAuthorization: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
  }
  
  resource applicationInsightsConnectionStringSecret 'secrets@2022-07-01' = {
    name: 'ApplicationInsightsConnectionString'
    properties: {
      contentType: 'string'
      value: applicationInsights.properties.ConnectionString
    }
  }

  resource botPublicKeySecret 'secrets@2022-07-01' = {
    name: 'BotPublicKey'
    properties: {
      contentType: 'string'
      value: botPublicKey
    }
  }

  resource botTokenSecret 'secrets@2022-07-01' = {
    name: 'BotToken'
    properties: {
      contentType: 'string'
      value: botToken
    }
  }

  resource serverIdSecret 'secrets@2022-07-01' = if (deployServerIdSecret) {
    name: 'ServerId'
    properties: {
      contentType: 'ulong'
      value: serverId
    }
  }

  // TODO: Temporarily until a managed identity alternative to WEBSITE_CONTENTAZUREFILECONNECTIONSTRING is created
  resource storageAccountConnectionStringSecret 'secrets@2022-07-01' = {
    name: 'StorageAccountConnectionString'
    properties: {
      contentType: 'string'
      value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
    }
  }
}

output secretUris object = {
  applicationInsightsConnectionString: keyVault::applicationInsightsConnectionStringSecret.properties.secretUri
  botPublicKey: keyVault::botPublicKeySecret.properties.secretUri
  botToken: keyVault::botTokenSecret.properties.secretUri
  serverId: deployServerIdSecret ? keyVault::serverIdSecret.properties.secretUri : ''
  storageAccountConnectionString: keyVault::storageAccountConnectionStringSecret.properties.secretUri
}
