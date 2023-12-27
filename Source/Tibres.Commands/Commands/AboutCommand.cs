using Discord;
using Discord.Rest;
using System.Linq;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    internal class AboutCommand(ICommandRepository commandRepository) : Command
    {
        private readonly ICommandRepository _commandRepository = commandRepository;

        public override string Name => Names.Commands.About;

        public override string Category => Categories.Other;

        public override string Description => "Displays a brief description of the bot and provides information about available commands.";

        public override Task HandleInteractionAsync(RestSlashCommand command)
        {
            var embedBuilder = CreateEmbedBuilder()
               .WithTitle("About")
               .WithDescription("**Tibres** is an open-source Discord bot designed to facilitate tracking a player's progress in the MMORPG " +
                                "Tibia. The program retrieves and saves individual character results, which are then used to generate monthly " +
                                "summaries introducing an element of friendly competition among server members and encouraging them to actively " +
                                "pursue improvement.")
               .WithFooter("If you find the bot valuable, please express your support by starring the GitHub repository.");

            AddCommandCategoryFields(embedBuilder);
            AddContactField(embedBuilder);

            return command.FollowupAsync(embed: embedBuilder.Build());
        }

        protected override SlashCommandProperties BuildCommandProperties(SlashCommandBuilder slashCommandBuilder)
        {
            return slashCommandBuilder
                .WithDefaultMemberPermissions(null)
                .WithDMPermission(true)
                .Build();
        }

        private void AddCommandCategoryFields(EmbedBuilder embedBuilder)
        {
            var commands = _commandRepository.GetAllCommandMetadata();

            foreach (var commandCategory in commands.GroupBy(c => c.Category).OrderBy(g => g.Key))
            {
                var embedFieldBuilder = new EmbedFieldBuilder()
                    .WithName($"{commandCategory.Key} commands")
                    .WithValue(string.Join(", ", commandCategory.Select(c => $"`/{c.Name}`")));

                embedBuilder.AddField(embedFieldBuilder);
            }
        }

        private static void AddContactField(EmbedBuilder embedBuilder)
        {
            var contactField = new EmbedFieldBuilder()
                .WithName("Contact")
                .WithValue("For more information, visit the [project's Github page](https://github.com/Tholdrim/Tibres). The author also " +
                           "encourages direct contact via Discord (<@133273246568153089>) for matters not addressed on the website.");

            embedBuilder.AddField(contactField);
        }
    }
}
