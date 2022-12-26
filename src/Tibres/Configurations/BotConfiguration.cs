using Microsoft.Extensions.Configuration;

namespace Tibres
{
    internal class BotConfiguration : IBotConfiguration
    {
        public BotConfiguration(IConfiguration configuration)
        {
            PublicKey = GetValue<string>(configuration, SettingsKeys.DiscordPublicKey);
            Token = GetValue<string>(configuration, SettingsKeys.DiscordToken);
        }

        public string PublicKey { get; }

        public string Token { get; }

        private static T GetValue<T>(IConfiguration configuration, string key)
        {
            return configuration.GetValue<T?>(key) ?? throw new ConfigurationException(key);
        }
    }
}
