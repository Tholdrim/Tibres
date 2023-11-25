using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tibres;
using Tibres.Commands;

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

    services.AddSingleton<IDiscordClient, DiscordClient>();

    services.AddOptions<BotOptions>(context.Configuration.GetSection(BotOptions.SectionName));
    services.AddOptions<ServerOptions>(context.Configuration.GetSection(ServerOptions.SectionName));

    services.ConfigureFunctionsApplicationInsights();

    services.Configure<ApplicationInsightsServiceOptions>(ConfigureApplicationInsightsServiceOptions);
    services.Configure<JsonSerializerOptions>(ConfigureJsonSerializerOptions);
    services.Configure<LoggerFilterOptions>(ConfigureLoggerFilterOptions);
}

static void ConfigureApplicationInsightsServiceOptions(ApplicationInsightsServiceOptions options)
{
    options.EnableHeartbeat = false;
}

static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
}

static void ConfigureLoggerFilterOptions(LoggerFilterOptions options)
{
    options.Rules.Clear();
    options.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning);
}
