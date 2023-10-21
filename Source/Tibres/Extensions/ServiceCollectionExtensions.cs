using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tibres
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddOptions<TOptions>(this IServiceCollection services, IConfigurationSection section)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .Bind(section, options => options.BindNonPublicProperties = true)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
