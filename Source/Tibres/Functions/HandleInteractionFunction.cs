using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Tibres.Commands;
using Tibres.Discord;

namespace Tibres
{
    internal class HandleInteractionFunction(IDiscordClient discordClient, ICommandRepository commandRepository, IEmojiRepository emojiRepository)
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly ICommandRepository _commandRepository = commandRepository;
        private readonly IEmojiRepository _emojiRepository = emojiRepository;

        [Function(Names.Functions.HandleInteraction)]
        public async Task RunAsync([QueueTrigger(Names.Queues.Interactions)] InteractionMessage message)
        {
            var interaction = await _discordClient.ParseHttpInteractionAsync(message, doApiCallOnCreation: _ => true);

            if (interaction is not RestSlashCommand slashCommand || !_commandRepository.TryGetCommand(slashCommand.Data.Name, out var command))
            {
                // TODO: Log warning

                var errorEmoji = await _emojiRepository.GetEmojiAsync(Emoji.Error);

                await interaction.FollowupAsync(errorEmoji + "I do not know how to handle this command.");

                return;
            }

            await command.HandleInteractionAsync(slashCommand);
        }
    }
}
