using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Tibres
{
    internal interface IDiscordClient
    {
        Task<DiscordRestClient> GetInternalClientAsync();

        Task<RestInteraction> ParseHttpInteractionAsync(InteractionMessage message, Func<InteractionProperties, bool>? doApiCallOnCreation = null);
    }
}
