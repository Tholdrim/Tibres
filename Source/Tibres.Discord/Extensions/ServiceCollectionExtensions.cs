using Microsoft.Extensions.DependencyInjection;

namespace Tibres.Discord
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscord(this IServiceCollection services)
        {
            services.AddSingleton<IDiscordClient, DiscordClient>();
            services.AddSingleton<IEmojiRepository, EmojiRepository>();

            services.AddOptions<BotOptions>(BotOptions.SectionName);
            services.AddOptions<ServerOptions>(ServerOptions.SectionName);
        }

        private static void AddOptions<TOptions>(this IServiceCollection services, string sectionName)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(sectionName, options => options.BindNonPublicProperties = true)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
