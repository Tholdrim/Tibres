var roleName = 'Tibres Deployment Operator'

resource roleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' = {
  name: guid(roleName)
  properties: {
    roleName: roleName
    description: 'Can perform Tibres deployments based on Azure Resource Manager templates.'
    type: 'customRole'
    assignableScopes: [
        resourceGroup().id
    ]
    permissions: [
      {
        actions: [
          'Microsoft.Authorization/roleAssignments/write'
          'Microsoft.Insights/components/annotations/write'
          'Microsoft.Insights/components/read'
          'Microsoft.Insights/components/write'
          'Microsoft.KeyVault/vaults/read'
          'Microsoft.KeyVault/vaults/secrets/read'
          'Microsoft.KeyVault/vaults/secrets/write'
          'Microsoft.KeyVault/vaults/write'
          'Microsoft.OperationalInsights/workspaces/read'
          'Microsoft.OperationalInsights/workspaces/write'
          'Microsoft.Resources/deployments/operationStatuses/read'
          'Microsoft.Resources/deployments/read'
          'Microsoft.Resources/deployments/validate/action'
          'Microsoft.Resources/deployments/write'
          'Microsoft.Resources/subscriptions/resourceGroups/read'
          'Microsoft.Storage/storageAccounts/blobServices/containers/read'
          'Microsoft.Storage/storageAccounts/blobServices/containers/write'
          'Microsoft.Storage/storageAccounts/blobServices/write'
          'Microsoft.Storage/storageAccounts/listKeys/action'
          'Microsoft.Storage/storageAccounts/write'
          'Microsoft.Web/serverfarms/write'
          'Microsoft.Web/sites/basicPublishingCredentialsPolicies/read'
          'Microsoft.Web/sites/config/list/action'
          'Microsoft.Web/sites/config/write'
          'Microsoft.Web/sites/publishxml/action'
          'Microsoft.Web/sites/read'
          'Microsoft.Web/sites/write'
        ]
        dataActions: [
          'Microsoft.Storage/storageAccounts/blobServices/containers/blobs/read'
          'Microsoft.Storage/storageAccounts/blobServices/containers/blobs/write'
        ]
      }
    ]
  }
}

output id string = roleDefinition.id
