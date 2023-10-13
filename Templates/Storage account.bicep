param storageAccountName string

param location string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: toLower(storageAccountName)
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    allowBlobPublicAccess: false
  }
}
