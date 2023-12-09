using Discord;
using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    public interface IDiscordClient
    {
        ulong? MainGuildId { get; }

        Task<RestGuild> GetGuildAsync(ulong guildId);

        Task<RestGuild?> GetMainGuildAsync();

        Task<RestInteraction> ParseHttpInteractionAsync(InteractionMessage message, Func<InteractionProperties, bool>? doApiCallOnCreation = null);

        Task RegisterSlashCommandsAsync(SlashCommandProperties[] commandProperties);
    }
}
