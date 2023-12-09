using Discord;
using Discord.Rest;
using System.Linq;
using System.Threading.Tasks;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal class PermissionsCommand(IDiscordClient discordClient, IEmojiRepository emojiRepository) : Command
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly IEmojiRepository _emojiRepository = emojiRepository;

        public override string Name => Names.Commands.Permissions;

        public override string Category => Categories.Administrative;

        public override string Description => "Lists the essential and optional permissions used by the bot along with their grant status.";

        public override async Task HandleInteractionAsync(ISlashCommandInteraction slashCommand)
        {
            var guild = await _discordClient.GetGuildAsync(slashCommand.GetGuildId());
            var permissionGroups = GetGroupedPermissions(guild.Id);
            var bot = await guild.GetCurrentUserAsync();

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Permissions")
                .WithDescription("This bot uses the following permissions:")
                .WithColor(Color.Gold);

            foreach (var permissionGroup in permissionGroups.OrderBy(k => k.Key))
            {
                var suffix = embedBuilder.Fields.Count != permissionGroups.Count - 1 ? ";" : ".";
                var lines = await Task.WhenAll(permissionGroup.Select(p => GetPermissionLineAsync(p, bot)));

                var embedFieldBuilder = new EmbedFieldBuilder()
                    .WithName(permissionGroup.Key)
                    .WithValue(string.Join(";\n", lines) + suffix);

                embedBuilder.AddField(embedFieldBuilder);
            }

            await slashCommand.FollowupAsync(embed: embedBuilder.Build());
        }

        protected override SlashCommandProperties BuildCommandProperties(SlashCommandBuilder slashCommandBuilder)
        {
            return slashCommandBuilder
                .WithDefaultMemberPermissions(GuildPermission.Administrator)
                .WithDMPermission(false)
                .Build();
        }

        private ILookup<string, Permission> GetGroupedPermissions(ulong guildId)
        {
            return Permissions.GetAll()
                .Where(p => p.Filter?.Invoke(_discordClient.MainGuildId, guildId) ?? true)
                .ToLookup(p => p.IsOptional ? "Optional" : "Essential");
        }

        private async Task<string> GetPermissionLineAsync(Permission permission, RestGuildUser user)
        {
            var isGranted = permission.Selector(user.GuildPermissions);
            var emoji = await _emojiRepository.GetEmojiAsync(isGranted ? Emoji.Check : Emoji.Error);

            return $"- {emoji} **{permission.Name}** {permission.Description}";
        }
    };
}
