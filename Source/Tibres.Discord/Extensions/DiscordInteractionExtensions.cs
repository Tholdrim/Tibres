using Discord;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    public static class DiscordInteractionExtensions
    {
        public static ulong GetGuildId(this IDiscordInteraction interaction)
        {
            if (!interaction.GuildId.HasValue)
            {
                throw new UnexpectedException();
            }

            return interaction.GuildId.Value;
        }

        public static Task<IUserMessage> ShowErrorMessageAsync(this IDiscordInteraction interaction, string message)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription(message)
                .WithFooter("If the problem persists, please report it.")
                .WithColor(Color.Red);

            return interaction.FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
