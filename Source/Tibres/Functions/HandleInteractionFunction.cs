using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Threading.Tasks;
using Tibres.Commands;
using Tibres.Discord;

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

            try
            {
                if (interaction is not RestSlashCommand slashCommand || !_commandRepository.TryGetCommand(slashCommand.Data.Name, out var command))
                {
                    throw new UnknownCommandException();
                }

                await command.HandleInteractionAsync(slashCommand);
            }
            catch (Exception exception)
            {
                var formattedException = exception as FormattedException;

                if (exception is UnexpectedException)
                {
                    // TODO: Log error
                }

                await interaction.ShowErrorMessageAsync(formattedException?.FormattedMessage ?? exception.Message);
            }
        }
    }
}
