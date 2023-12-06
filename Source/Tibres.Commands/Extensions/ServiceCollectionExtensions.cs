using Microsoft.Extensions.DependencyInjection;

namespace Tibres.Commands
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommands(this IServiceCollection services)
        {
            services.AddSingleton<Command, AboutCommand>();
            services.AddSingleton<Command, PermissionsCommand>();

            services.AddSingleton<ICommandRepository, CommandRepository>();
        }
    }
}
