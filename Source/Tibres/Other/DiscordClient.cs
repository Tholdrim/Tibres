using Discord;
using Discord.Rest;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Tibres
{
    internal class DiscordClient : IDiscordClient
    {
        private readonly BotOptions _botOptions;
        private readonly ServerOptions _serverOptions;

        public DiscordClient(IOptions<BotOptions> botOptions, IOptions<ServerOptions> serverOptions)
        {
            _botOptions = botOptions.Value;
            _serverOptions = serverOptions.Value;
        }

        public ulong? ServerId => _serverOptions.Id;

        private DiscordRestClient InternalClient { get; } = new();

        public async Task<DiscordRestClient> GetInternalClientAsync()
        {
            if (InternalClient.LoginState != LoginState.LoggedIn)
            {
                // There is no need for a semaphore, as the library already uses one.
                await InternalClient.LoginAsync(TokenType.Bot, _botOptions.Token);
            }

            return InternalClient;
        }

        public async Task<RestInteraction> ParseHttpInteractionAsync(
            InteractionMessage message,
            Func<InteractionProperties, bool>? doApiCallOnCreation = null)
        {
            var client = await GetInternalClientAsync();

            message.Deconstruct(out var body, out var signature, out var timestamp);

            return await client.ParseHttpInteractionAsync(_botOptions.PublicKey, signature, timestamp, body, doApiCallOnCreation);
        }
    }
}
