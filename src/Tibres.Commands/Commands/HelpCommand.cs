using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    internal class HelpCommand : ICommand
    {
        private readonly ICommandFactory _commandFactory;

        public HelpCommand(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public string Name => "help";

        public string Category => CommandCategories.Other;

        public CommandDescription Description { get; } = new(
            Chat: "Displays a brief description of the bot and information about available commands.",
            Help: "displays this message.");

        public Task HandleInteractionAsync(ISlashCommandInteraction slashCommand)
        {
            var embedBuilder = new EmbedBuilder()
               .WithTitle("Tibres")
               .WithDescription(
                    "It is an open-source Discord bot that makes it easier to track a player's advancement in the MMORPG Tibia. The program " +
                    "internally uses the [TibiaData API](https://tibiadata.com/) to retrieve and save the results of individual characters. " +
                    "These are then used, among other things, to generate monthly progress reports introducing an element of competition " +
                    "among server members and encouraging them to work harder.")
               .WithColor(Color.LightGrey);

            AddCommandCategoryFields(embedBuilder);
            AddContactField(embedBuilder);

            return slashCommand.FollowupAsync(embed: embedBuilder.Build());
        }

        private void AddCommandCategoryFields(EmbedBuilder embedBuilder)
        {
            var commands = _commandFactory.GetAllCommandMetadata();

            foreach (var commandCategory in commands.GroupBy(c => c.Category))
            {
                var embedFieldBuilder = new EmbedFieldBuilder()
                    .WithName($"{commandCategory.Key} commands")
                    .WithValue(string.Join('\n', commandCategory.Select(c => $"`/{c.Name}` {c.Description.Help}")));

                embedBuilder.AddField(embedFieldBuilder);
            }
        }

        private static void AddContactField(EmbedBuilder embedBuilder)
        {
            var contactField = new EmbedFieldBuilder()
                .WithName("Contact")
                .WithValue(
                    "More information is available on the [project's Github page](https://github.com/Tholdrim/Tibres). I also encourage you to " +
                    "contact me directly via Discord (<@133273246568153089>) for matters not addressed by the website.");

            embedBuilder.AddField(contactField);
        }
    }
}
