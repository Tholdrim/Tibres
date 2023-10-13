param appServiceName string

@secure()
param appSettings object

resource siteconfig 'Microsoft.Web/sites/config@2022-09-01' = {
  name: '${appServiceName}/appsettings'
  properties: appSettings
}
