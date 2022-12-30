using Discord;
using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Tibres
{
    internal class HandleInteractionFunction
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IBotConfiguration _configuration;

        public HandleInteractionFunction(ICommandFactory commandFactory, IBotConfiguration configuration)
        {
            _commandFactory = commandFactory;
            _configuration = configuration;
        }

        [Function(Names.Functions.HandleInteraction)]
        public async Task RunAsync(
            [QueueTrigger(Names.Queues.Interactions)] InteractionMessage message)
        {
            var client = new DiscordRestClient();

            await client.LoginAsync(TokenType.Bot, _configuration.Token);

            var interaction = await client.ParseHttpInteractionAsync(message, _configuration.PublicKey, _ => true);

            if (interaction is not RestSlashCommand slashCommand || !_commandFactory.TryGetCommand(slashCommand.Data.Name, out var command))
            {
                // TODO: Log warning

                await interaction.DeleteOriginalResponseAsync();

                return;
            }

            await command.HandleInteractionAsync(slashCommand);
        }
    }
}
