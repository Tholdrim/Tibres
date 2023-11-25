param keyVaultName string
param storageAccountName string

param deploymentOperatorPrincipalId string
param deploymentOperatorRoleDefinitionId string
param functionAppPrincipalId string

var roles = {
  keyVault: items({
    'Key Vault Secrets User': '4633458b-17de-408a-b874-0445c86b69e6'
  })
  storageAccount: items({
    'Storage Account Contributor': '17d1049b-9a84-46fb-8f53-869881c3d3ab'
    'Storage Blob Data Contributor': 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
    'Storage Queue Data Contributor': '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
  })
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource keyVaultRoleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for role in roles.keyVault: {
  name: guid(role.value, keyVault.id, functionAppPrincipalId)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', role.value)
    principalId: functionAppPrincipalId
    principalType: 'ServicePrincipal'
  }
}]

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource storageAccountRoleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for role in roles.storageAccount: {
  name: guid(role.value, storageAccount.id, functionAppPrincipalId)
  scope: storageAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', role.value)
    principalId: functionAppPrincipalId
    principalType: 'ServicePrincipal'
  }
}]

var conditionTemplate = '''
(
  (
    !(ActionMatches{{'Microsoft.Authorization/roleAssignments/write'}})
  )
  OR 
  (
    @Request[Microsoft.Authorization/roleAssignments:RoleDefinitionId] ForAnyOfAnyValues:GuidEquals {{ {0} }}
  )
)
'''

resource resourceGroupRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (!empty(deploymentOperatorPrincipalId)) {
  name: guid(deploymentOperatorRoleDefinitionId, resourceGroup().id, deploymentOperatorPrincipalId)
  properties: {
    roleDefinitionId: deploymentOperatorRoleDefinitionId
    principalId: deploymentOperatorPrincipalId
    principalType: 'ServicePrincipal'
    condition: format(conditionTemplate, join(concat(map(roles.keyVault, x => x.value), map(roles.storageAccount, x => x.value)), ', '))
    conditionVersion: '2.0'
  }
}
