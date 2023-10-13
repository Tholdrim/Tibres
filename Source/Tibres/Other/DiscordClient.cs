using Discord;
using Discord.Rest;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Tibres
{
    internal class DiscordClient : IDiscordClient
    {
        private readonly BotOptions _options;

        public DiscordClient(IOptions<BotOptions> options)
        {
            _options = options.Value;
        }

        private DiscordRestClient InternalClient { get; } = new();

        public async Task<DiscordRestClient> GetInternalClientAsync()
        {
            if (InternalClient.LoginState != LoginState.LoggedIn)
            {
                // There is no need for a semaphore, as the library already uses one.
                await InternalClient.LoginAsync(TokenType.Bot, _options.Token);
            }

            return InternalClient;
        }

        public async Task<RestInteraction> ParseHttpInteractionAsync(
            InteractionMessage message,
            Func<InteractionProperties, bool>? doApiCallOnCreation = null)
        {
            var client = await GetInternalClientAsync();

            message.Deconstruct(out var body, out var signature, out var timestamp);

            return await client.ParseHttpInteractionAsync(_options.PublicKey, signature, timestamp, body, doApiCallOnCreation);
        }
    }
}
