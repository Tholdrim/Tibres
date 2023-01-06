param mainIdentifier string

param location string = resourceGroup().location
param tenantId string = subscription().tenantId

@secure()
param botPublicKey string

@secure()
param botToken string

module functionApp './function app.bicep' = {
  name: '${deployment().name}-FunctionApp'
  params: {
    appServicePlanName: mainIdentifier
    functionAppName: mainIdentifier
    keyVaultName: keyVault.outputs.name
    storageAccountName: storageAccount.outputs.name
    location: location
    keyVaultSecretUris: keyVault.outputs.secretUris
  }
}

module keyVault './key vault.bicep' = {
  name: '${deployment().name}-KeyVault'
  params: {
    keyVaultName: mainIdentifier
    storageAccountName: storageAccount.outputs.name
    location: location
    tenantId: tenantId
    botPublicKey: botPublicKey
    botToken: botToken
  }
}

module storageAccount './storage account.bicep' = {
  name: '${deployment().name}-StorageAccount'
  params: {
    storageAccountName: mainIdentifier
    location: location
  }
}
