using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Tibres;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
    .ConfigureServices(ConfigureServices)
    .Build();

host.Run();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    Tibres.Commands.Services.ConfigureServices(services);

    services.AddSingleton<IDiscordClient, DiscordClient>();

    services.AddOptions<BotOptions>()
        .Bind(context.Configuration.GetSection(BotOptions.SectionName), options => options.BindNonPublicProperties = true)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    services.Configure<JsonSerializerOptions>(options => options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
}
