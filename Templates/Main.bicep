param applicationInsightsName string
param appServicePlanName string
param functionAppName string
param keyVaultName string
param logsWorkspaceName string
param storageAccountName string

param principalId string = ''

@secure()
param botPublicKey string

@secure()
param botToken string

@secure()
param serverId string = ''

#disable-next-line no-loc-expr-outside-params
var location = resourceGroup().location

module applicationInsights 'Application Insights.bicep' = {
  name: '${deployment().name}-ApplicationInsights'
  params: {
    applicationInsightsName: applicationInsightsName
    logsWorkspaceName: logsWorkspaceName
    location: location
  }
}

module appSettings 'App settings.bicep' = {
  name: '${deployment().name}-AppSettings'
  params: {
    appServiceName: functionAppName
    appSettings: union({
      APPLICATIONINSIGHTS_CONNECTION_STRING: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secretUris.applicationInsightsConnectionString}/)'
      AzureWebJobsStorage__accountName: storageAccountName
      Bot__PublicKey: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secretUris.botPublicKey}/)'
      Bot__Token: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secretUris.botToken}/)'
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secretUris.storageAccountConnectionString}/)'
      WEBSITE_CONTENTSHARE: functionAppName
      WEBSITE_ENABLE_SYNC_UPDATE_SITE: 'true'
      WEBSITE_RUN_FROM_PACKAGE: '1'
      WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED: '1'
    }, empty(keyVault.outputs.secretUris.serverId) ? {} : {
      Server__Id: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secretUris.serverId}/)'
    })
  }
  dependsOn: [ roleAssignments ]
}

module functionApp 'Function App.bicep' = {
  name: '${deployment().name}-FunctionApp'
  params: {
    appServicePlanName: appServicePlanName
    functionAppName: functionAppName
    location: location
  }
}

module keyVault 'Key Vault.bicep' = {
  name: '${deployment().name}-KeyVault'
  params: {
    applicationInsightsName: applicationInsightsName
    keyVaultName: keyVaultName
    storageAccountName: storageAccountName
    location: location
    botPublicKey: botPublicKey
    botToken: botToken
    serverId: serverId
  }
  dependsOn: [ applicationInsights, storageAccount ]
}

module roleAssignments 'Role assignments.bicep' = {
  name: '${deployment().name}-RoleAssignments'
  params: {
    keyVaultName: keyVaultName
    storageAccountName: storageAccountName
    functionAppPrincipalId: functionApp.outputs.principalId
    deploymentOperatorRoleDefinitionId: empty(principalId) ? '' : roleDefinition.outputs.id
    deploymentOperatorPrincipalId: principalId
  }
  dependsOn: [ keyVault, storageAccount ]
}

module roleDefinition 'Role definition.bicep' = if (!empty(principalId)) {
  name: '${deployment().name}-RoleDefinition'
}

module storageAccount 'Storage account.bicep' = {
  name: '${deployment().name}-StorageAccount'
  params: {
    storageAccountName: storageAccountName
    location: location
  }
}
