using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Tibres.Commands;
using Tibres.Discord;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddApplicationInsightsTelemetryWorkerService();

    services.AddCommands();
    services.AddDiscord();

    services.ConfigureFunctionsApplicationInsights();

    services.Configure<ApplicationInsightsServiceOptions>(ConfigureApplicationInsightsServiceOptions);
    services.Configure<JsonSerializerOptions>(ConfigureJsonSerializerOptions);
    services.Configure<WorkerOptions>(ConfigureWorkerOptions);
}

static void ConfigureApplicationInsightsServiceOptions(ApplicationInsightsServiceOptions options)
{
    options.EnableHeartbeat = false;
}

static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
}

static void ConfigureWorkerOptions(WorkerOptions options)
{
    options.EnableUserCodeException = true;
}
