using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Tibres
{
    internal static class DiscordRestClientExtensions
    {
        public static Task<RestInteraction> ParseHttpInteractionAsync(
            this DiscordRestClient client,
            InteractionMessage message,
            string publicKey,
            Func<InteractionProperties, bool>? doApiCallOnCreation = null)
        {
            var (body, signature, timestamp) = message;

            return client.ParseHttpInteractionAsync(publicKey, signature, timestamp, body, doApiCallOnCreation);
        }
    }
}
