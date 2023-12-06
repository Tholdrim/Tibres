using Discord;
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
                if (interaction is RestSlashCommand slashCommand && _commandRepository.TryGetCommand(slashCommand.Data.Name, out var command))
                {
                    await command.HandleInteractionAsync(slashCommand);

                    return;
                }

                // TODO: Log warning

                await ShowErrorMessageAsync(interaction, "The specified command was not recognized.");
            }
            catch (Exception exception)
            {
                await ShowErrorMessageAsync(interaction, exception.Message);
            }
        }

        private static Task<RestFollowupMessage> ShowErrorMessageAsync(RestInteraction interaction, string message)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription(message)
                .WithFooter("If the problem persists, please report it.")
                .WithColor(Color.Red);

            return interaction.FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
