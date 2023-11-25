using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Tibres.Commands;

namespace Tibres
{
    internal class HandleInteractionFunction(IDiscordClient discordClient, ICommandRepository commandRepository)
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly ICommandRepository _commandRepository = commandRepository;

        [Function(Names.Functions.HandleInteraction)]
        public async Task RunAsync([QueueTrigger(Names.Queues.Interactions)] InteractionMessage message)
        {
            var interaction = await _discordClient.ParseHttpInteractionAsync(message, doApiCallOnCreation: _ => true);

            if (interaction is not RestSlashCommand slashCommand || !_commandRepository.TryGetCommand(slashCommand.Data.Name, out var command))
            {
                // TODO: Log warning

                await interaction.DeleteOriginalResponseAsync();

                return;
            }

            await command.HandleInteractionAsync(slashCommand);
        }
    }
}
