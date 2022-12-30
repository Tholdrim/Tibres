param keyVaultName string
param storageAccountName string

param location string
param tenantId string = subscription().tenantId

@secure()
param discordPublicKey string

@secure()
param discordToken string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
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

  resource storageAccountConnectionStringSecret 'secrets@2022-07-01' = {
    name: 'StorageAccountConnectionString'
    properties: {
      contentType: 'string'
      value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
    }
  }
}

output name string = keyVault.name
output secretUris object = {
  discordPublicKey: keyVault::discordPublicKeySecret.properties.secretUri
  discordToken: keyVault::discordTokenSecret.properties.secretUri
  storageAccountConnectionString: keyVault::storageAccountConnectionStringSecret.properties.secretUri
}
