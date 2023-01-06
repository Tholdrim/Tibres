using Microsoft.Extensions.Configuration;

namespace Tibres
{
    internal class BotOptions : IBotConfiguration
    {
        private const string SectionName = "Bot";

        public string? PublicKey { get; set; }

        public string? Token { get; set; }

        string IBotConfiguration.PublicKey => PublicKey ?? throw new ConfigurationException(SectionName, nameof(PublicKey));

        string IBotConfiguration.Token => Token ?? throw new ConfigurationException(SectionName, nameof(Token));

        internal static BotOptions Initialize(IConfiguration configuration)
        {
            var options = new BotOptions();

            configuration.GetSection(SectionName).Bind(options);

            return options;
        }
    }
}
