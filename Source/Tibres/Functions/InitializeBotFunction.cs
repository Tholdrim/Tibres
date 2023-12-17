using Azure.Storage.Blobs;
using Discord;
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
            var guild = await _discordClient.GetMainGuildAsync();

            if (guild == null)
            {
                // TODO: Log information

                return;
            }

            var emotes = await guild.GetEmotesAsync();
            var user = await guild.GetCurrentUserAsync();

            Permissions.ManageExpressions.CheckIfGranted(user.GuildPermissions);

            foreach (var emoji in Enum.GetValues<Emoji>())
            {
                var emote = emotes.FindEmoji(emoji, creatorId: user.Id) ?? await UploadEmojiAsync(blobContainerClient, guild, emoji.ToName());

                _emojiRepository.UpdateEmoji(emoji, emote);
            }
        }

        private static async Task<IEmote> UploadEmojiAsync(BlobContainerClient blobContainerClient, IGuild guild, string name)
        {
            var blob = blobContainerClient.GetBlobClient($"{name}.png");

            using var blobStream = await blob.OpenReadAsync();

            return await guild.CreateEmoteAsync(name, new Image(blobStream));
        }
    }
}
