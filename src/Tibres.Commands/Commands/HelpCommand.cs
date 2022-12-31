using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    internal class HelpCommand : Command
    {
        private readonly ICommandRepository _commandRepository;

        public HelpCommand(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public override string Name => "help";

        public override string Category => CommandCategories.Other;

        public override CommandDescription Description { get; } = new(
            Chat: "Displays a brief description of the bot and information about available commands.",
            Help: "displays this message.");

        public override Task HandleInteractionAsync(ISlashCommandInteraction slashCommand)
        {
            var embedBuilder = new EmbedBuilder()
               .WithTitle("Tibres")
               .WithDescription(
                    "It is an open-source Discord bot that makes it easier to track a player's advancement in the MMORPG Tibia. The program " +
                    "internally uses the [TibiaData API](https://tibiadata.com/) to retrieve and save the results of individual characters. " +
                    "These are then used, among other things, to generate monthly progress reports introducing an element of competition " +
                    "among server members and encouraging them to work harder.")
               .WithFooter("If you enjoy the bot, please show your support by giving the GitHub repository a star.")
               .WithColor(Color.LightGrey);

            AddCommandCategoryFields(embedBuilder);
            AddContactField(embedBuilder);

            return slashCommand.FollowupAsync(embed: embedBuilder.Build());
        }

        protected override SlashCommandBuilder ExtendCommandDefinition(SlashCommandBuilder slashCommandBuilder)
        {
            return slashCommandBuilder
                .WithDefaultMemberPermissions(null)
                .WithDMPermission(true);
        }

        private void AddCommandCategoryFields(EmbedBuilder embedBuilder)
        {
            var commands = _commandRepository.GetAllCommandMetadata();

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
