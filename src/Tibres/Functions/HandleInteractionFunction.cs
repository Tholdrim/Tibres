using Discord;
using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Tibres.Commands;

namespace Tibres
{
    internal class HandleInteractionFunction
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IBotConfiguration _configuration;

        public HandleInteractionFunction(ICommandRepository commandRepository, IBotConfiguration configuration)
        {
            _commandRepository = commandRepository;
            _configuration = configuration;
        }

        [Function(Names.Functions.HandleInteraction)]
        public async Task RunAsync(
            [QueueTrigger(Names.Queues.Interactions)] InteractionMessage message)
        {
            var client = new DiscordRestClient();

            await client.LoginAsync(TokenType.Bot, _configuration.Token);

            var interaction = await client.ParseHttpInteractionAsync(message, _configuration.PublicKey, doApiCallOnCreation: _ => true);

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
