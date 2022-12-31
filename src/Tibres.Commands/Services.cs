using Microsoft.Extensions.DependencyInjection;

namespace Tibres.Commands
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICommand, HelpCommand>();
            services.AddSingleton<ICommandFactory, CommandFactory>();
        }
    }
}
