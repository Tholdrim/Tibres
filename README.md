<p align="center">
  <a href="https://github.com/Tholdrim/Tibres"><img src="Resources/Logo.png" /></a>
</p>
<p align="center">
  <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0"><img src="https://img.shields.io/badge/.NET-8.0-blue" /></a>
  <a href="https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview"><img src="https://img.shields.io/badge/Azure%20Functions-v4-orange?logo=azurefunctions" /></a>
  <a href="https://github.com/Tholdrim/Tibres/actions/workflows/Provisioning.yml"><img src="https://img.shields.io/github/actions/workflow/status/Tholdrim/Tibres/Provisioning.yml?logo=github&label=Provision%20an%20infrastructure" /></a>
  <a href="https://github.com/Tholdrim/Tibres/actions/workflows/Deployment.yml"><img src="https://img.shields.io/github/actions/workflow/status/Tholdrim/Tibres/Deployment.yml?logo=github&label=Deploy%20an%20application" /></a>
  <a href="LICENSE.txt"><img src="https://img.shields.io/github/license/Tholdrim/Tibres?label=License" /></a>
</p>

# Tibres

Tibres is a Discord bot that makes it easier to track a player's advancement in the MMORPG [Tibia](https://www.tibia.com).

## Credits

The bot's avatar was created using the [Discord Avatar Maker](https://discord-avatar-maker.app) tool, and the icons that appear in the bot's messages were created by [Fathema Khanom - Flaticon](https://www.flaticon.com/packs/user-interface-2550).

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FTholdrim%2FTibres%2Fworkflow-test%2FTemplates%2FCompiled%2FMain.json)

// TODO: VNET
//  - https://github.com/Azure/Azure-Functions/issues/646
// TODO: Health check
//  - https://learn.microsoft.com/en-us/previous-versions/azure/azure-monitor/app/monitor-web-app-availability
//  - https://learn.microsoft.com/en-us/azure/azure-monitor/app/availability-overview
// TODO: Template definition
//  - https://learn.microsoft.com/en-us/azure/azure-resource-manager/managed-applications/publish-bicep-definition?tabs=azure-powershell
// TODO: AOT compilation
//  - https://andrewlock.net/exploring-the-dotnet-8-preview-the-minimal-api-aot-template/
// TODO: RegisterCommands schedule based
// https://tibres.azurewebsites.net/api/webhook
