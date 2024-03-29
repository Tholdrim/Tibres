using Discord;
using Discord.Rest;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    internal class DiscordClient(IOptions<BotOptions> botOptions, IOptions<ServerOptions> serverOptions) : IDiscordClient
    {
        private readonly BotOptions _botOptions = botOptions.Value;
        private readonly ServerOptions _serverOptions = serverOptions.Value;

        public ulong? MainGuildId => _serverOptions.Id;

        private DiscordRestClient InternalClient { get; } = new();

        public async Task<RestGuild?> GetMainGuildAsync()
        {
            if (MainGuildId is not ulong guildId)
            {
                return null;
            }

            var client = await GetInternalClientAsync();

            return await client.GetGuildAsync(guildId);
        }

        public async Task<RestInteraction> ParseHttpInteractionAsync(
            InteractionMessage message,
            Func<InteractionProperties, bool>? doApiCallOnCreation)
        {
            var client = await GetInternalClientAsync();

            message.Deconstruct(out var body, out var signature, out var timestamp);

            return await client.ParseHttpInteractionAsync(_botOptions.PublicKey, signature, timestamp, body, doApiCallOnCreation);
        }

        public async Task RegisterSlashCommandsAsync(ApplicationCommandProperties[] commandProperties)
        {
            var client = await GetInternalClientAsync();

            await client.BulkOverwriteGlobalCommands(commandProperties);
        }

        private async Task<DiscordRestClient> GetInternalClientAsync()
        {
            if (InternalClient.LoginState != LoginState.LoggedIn)
            {
                // There is no need for a semaphore, as the library already uses one.
                await InternalClient.LoginAsync(TokenType.Bot, _botOptions.Token);
            }

            return InternalClient;
        }
    }
}
