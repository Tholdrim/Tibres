using Azure.Storage.Blobs;
using Discord;
using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Tibres.Commands;
using Tibres.Discord;

namespace Tibres
{
    internal class InitializeBotFunction(IDiscordClient discordClient, ICommandRepository commandRepository, IEmojiRepository emojiRepository)
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly ICommandRepository _commandRepository = commandRepository;
        private readonly IEmojiRepository _emojiRepository = emojiRepository;

        [Function(Names.Functions.InitializeBot)]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public async Task RunAsync(
            [TimerTrigger("%" + Names.AppSettings.InitializationFrequency + "%")] TimerInfo timerInfo,
            [BlobInput(Names.BlobContainers.Emojis)] BlobContainerClient blobContainerClient)
        {
            await RegisterCommandsAsync();
            await UploadEmojisAsync(blobContainerClient);
        }

        private Task RegisterCommandsAsync()
        {
            var commandProperties = _commandRepository.GetAllCommandProperties();

            return _discordClient.RegisterSlashCommandsAsync(commandProperties);
        }

        private async Task UploadEmojisAsync(BlobContainerClient blobContainerClient)
        {
            if (_discordClient.MainGuildId is not ulong guildId)
            {
                // TODO: Log information

                return;
            }

            var guild = await _discordClient.GetGuildAsync(guildId) ?? throw new UnknownGuildException(guildId);
            var bot = await guild.GetCurrentUserAsync();

            if (!bot.GuildPermissions.ManageEmojisAndStickers)
            {
                // TODO: Log warning

                return;
            }

            var emotes = await guild.GetEmotesAsync();

            foreach (var emoji in Enum.GetValues<Emoji>())
            {
                var emote = emotes.FindBotEmoji(emoji, bot.Id) ?? await UploadEmojiAsync(blobContainerClient, guild, emoji.ToName());

                _emojiRepository.UpdateEmoji(emoji, emote);
            }
        }

        private static async Task<GuildEmote> UploadEmojiAsync(BlobContainerClient blobContainerClient, RestGuild guild, string name)
        {
            var blob = blobContainerClient.GetBlobClient($"{name}.png");

            using var blobStream = await blob.OpenReadAsync();

            return await guild.CreateEmoteAsync(name, new Image(blobStream));
        }
    }
}
