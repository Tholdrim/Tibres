using Discord;
using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Tibres
{
    internal class DiscordClient : IDiscordClient
    {
        private readonly IBotConfiguration _configuration;

        public DiscordClient(IBotConfiguration configuration)
        {
            _configuration = configuration;
        }

        private DiscordRestClient InternalClient { get; } = new DiscordRestClient();

        public async Task<DiscordRestClient> GetInternalClientAsync()
        {
            if (InternalClient.LoginState != LoginState.LoggedIn)
            {
                await InternalClient.LoginAsync(TokenType.Bot, _configuration.Token);
            }

            return InternalClient;
        }

        public async Task<RestInteraction> ParseHttpInteractionAsync(
            InteractionMessage message,
            Func<InteractionProperties, bool>? doApiCallOnCreation = null)
        {
            var client = await GetInternalClientAsync();

            message.Deconstruct(out var body, out var signature, out var timestamp);

            return await client.ParseHttpInteractionAsync(_configuration.PublicKey, signature, timestamp, body, doApiCallOnCreation);
        }
    }
}
