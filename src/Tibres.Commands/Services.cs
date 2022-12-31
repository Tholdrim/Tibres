using Microsoft.Extensions.DependencyInjection;

namespace Tibres.Commands
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Command, HelpCommand>();
            services.AddSingleton<ICommandRepository, CommandRepository>();
        }
    }
}
