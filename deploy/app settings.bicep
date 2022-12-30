param appServiceName string

@secure()
param existingAppSettings object

@secure()
param newAppSettings object

resource siteconfig 'Microsoft.Web/sites/config@2022-03-01' = {
  name: '${appServiceName}/appsettings'
  properties: union(existingAppSettings, newAppSettings)
}
