param keyVaultName string
param storageAccountName string

param location string
param tenantId string

@secure()
param botPublicKey string

@secure()
param botToken string

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
  botPublicKey: keyVault::botPublicKeySecret.properties.secretUri
  botToken: keyVault::botTokenSecret.properties.secretUri
  storageAccountConnectionString: keyVault::storageAccountConnectionStringSecret.properties.secretUri
}
