using System.ComponentModel.DataAnnotations;

namespace Tibres.Discord
{
    internal class BotOptions
    {
        public const string SectionName = "Bot";

        [Required]
        public string PublicKey { get; private set; } = null!;

        [Required]
        public string Token { get; private set; } = null!;
    }
}
