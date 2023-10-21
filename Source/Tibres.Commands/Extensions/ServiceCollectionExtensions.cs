using Microsoft.Extensions.DependencyInjection;

namespace Tibres.Commands
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommands(this IServiceCollection services)
        {
            services.AddSingleton<Command, HelpCommand>();
            services.AddSingleton<ICommandRepository, CommandRepository>();
        }
    }
}
