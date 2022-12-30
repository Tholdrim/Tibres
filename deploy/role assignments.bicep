param keyVaultName string
param storageAccountName string

param principalId string

var roles = {
  keyVault: {
    'Key Vault Secrets User': '4633458b-17de-408a-b874-0445c86b69e6'
  }
  storageAccount: {
    'Storage Account Contributor': '17d1049b-9a84-46fb-8f53-869881c3d3ab'
    'Storage Blob Data Contributor': 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
    'storage Queue Data Contributor': '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource keyVaultRoleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for role in items(roles.keyVault): {
  name: guid(role.key, keyVault.id, principalId)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', role.value)
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}]

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
}

resource storageAccountRoleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for role in items(roles.storageAccount): {
  name: guid(role.key, storageAccount.id, principalId)
  scope: storageAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', role.value)
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}]
