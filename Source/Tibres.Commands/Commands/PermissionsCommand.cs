using Discord;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal class PermissionsCommand(IDiscordClient discordClient, IEmojiRepository emojiRepository) : Command
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly IEmojiRepository _emojiRepository = emojiRepository;

        public override string Name => "permissions";

        public override string Category => Categories.Administrative;

        public override string Description => "Lists the essential and optional permissions used by the bot along with their grant status.";

        public override async Task HandleInteractionAsync(ISlashCommandInteraction slashCommand)
        {
            if (slashCommand.GuildId is not ulong guildId)
            {
                throw new NotSupportedException();
            }

            var guild = await _discordClient.GetGuildAsync(guildId) ?? throw new UnknownGuildException(guildId);
            var bot = await guild.GetCurrentUserAsync();

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Permissions")
                .WithDescription("This bot uses the following permissions:")
                .WithColor(Color.Gold);

            foreach (var permissionGroup in GetGroupedPermissions(guildId))
            {
                var lines = await Task.WhenAll(permissionGroup.Select(p => GetPermissionLineAsync(p, bot)));

                var embedFieldBuilder = new EmbedFieldBuilder()
                    .WithName(permissionGroup.Key)
                    .WithValue(string.Join("\n", lines));

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

        private IEnumerable<IGrouping<string, Permission>> GetGroupedPermissions(ulong guildId)
        {
            return Permissions.All
                .Where(p => p.Filter?.Invoke(_discordClient.MainGuildId, guildId) ?? true)
                .GroupBy(p => p.IsOptional ? "Optional" : "Essential")
                .OrderBy(g => g.Key);
        }

        private async Task<string> GetPermissionLineAsync(Permission permission, RestGuildUser user)
        {
            var isGranted = permission.Selector(user.GuildPermissions);
            var emoji = await _emojiRepository.GetEmojiAsync(isGranted ? Emoji.Check : Emoji.Error);

            return $"- {emoji} **{permission.Name}** {permission.Description}";
        }
    };
}
