using Discord;
using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Tibres
{
    internal class HandleInteractionFunction
    {
        private readonly IBotConfiguration _configuration;

        public HandleInteractionFunction(IBotConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function(Names.Functions.HandleInteraction)]
        public async Task RunAsync(
            [QueueTrigger(Names.Queues.Interactions)] InteractionMessage message)
        {
            var client = new DiscordRestClient();

            await client.LoginAsync(TokenType.Bot, _configuration.Token);

            var interaction = await client.ParseHttpInteractionAsync(message, _configuration.PublicKey, _ => true);

            if (interaction is not RestSlashCommand slashCommand || slashCommand.Data.Name != "help")
            {
                // TODO: Log warning

                await interaction.DeleteOriginalResponseAsync();

                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("Tibres")
                .WithColor(Color.LightGrey)
                .WithDescription("It is an open-source Discord bot that makes it easier to track a player's advancement in the MMORPG Tibia. The program internally uses the [TibiaData API](https://tibiadata.com/) to retrieve and save the results of individual characters. These are then used, among other things, to generate monthly progress reports introducing an element of competition among server members and encouraging them to work harder.")
                .WithFields(
                    new EmbedFieldBuilder()
                        .WithName("Other commands")
                        .WithValue("`/help` displays this message"),
                    new EmbedFieldBuilder()
                        .WithName("Contact")
                        .WithValue("More information is available on the [project's Github page](https://github.com/Tholdrim/Tibres). I also encourage you to contact me directly via Discord (<@133273246568153089>) for matters not addressed by the website."))
                .Build();

            await slashCommand.FollowupAsync(embed: embed);
        }
    }
}
