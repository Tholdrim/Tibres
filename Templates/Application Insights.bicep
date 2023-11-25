param applicationInsightsName string
param logsWorkspaceName string

param location string

resource logsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 90
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 90
    WorkspaceResourceId: logsWorkspace.id
  }
}
