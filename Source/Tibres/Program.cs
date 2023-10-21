using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Tibres;
using Tibres.Commands;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
    .ConfigureServices(ConfigureServices)
    .Build();

host.Run();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddCommands();

    services.AddSingleton<IDiscordClient, DiscordClient>();

    services.AddOptions<BotOptions>(context.Configuration.GetSection(BotOptions.SectionName));
    services.AddOptions<ServerOptions>(context.Configuration.GetSection(ServerOptions.SectionName));

    services.Configure<JsonSerializerOptions>(options => options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
}
