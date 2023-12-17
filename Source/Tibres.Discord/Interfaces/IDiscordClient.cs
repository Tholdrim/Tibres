using Discord;
using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    public interface IDiscordClient
    {
        ulong? MainGuildId { get; }

        Task<IGuild?> GetMainGuildAsync();

        Task<IDiscordInteraction> ParseHttpInteractionAsync(InteractionMessage message, Func<InteractionProperties, bool>? doApiCallOnCreation);

        Task RegisterSlashCommandsAsync(ApplicationCommandProperties[] commandProperties);
    }
}
