using Discord;
using Discord.Rest;
using System.Linq;
using System.Threading.Tasks;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal class NotificationsCommand : Command
    {
        public override string Name => Names.Commands.Notifications;

        public override string Category => Categories.Administrative;

        public override async Task HandleInteractionAsync(RestSlashCommand command)
        {
            var user = await command.Guild.GetCurrentUserAsync();

            Permissions.ManageWebhooks.CheckIfGranted(user.GuildPermissions);

            var webhooks = await command.Guild.GetWebhooksAsync();
            var webhook = webhooks.FirstOrDefault(w => w.Creator.Id == user.Id && w.Name == Names.Webhooks.Notifications);

            var subcommand = command.Data.Options.FirstOrDefault();
            var subtask = subcommand?.Name switch
            {
                Names.Options.Disable => DisableNotificationsAsync(command, webhook),
                Names.Options.Enable  => EnableNotificationsAsync(command, subcommand, webhook),
                _                     => throw new UnexpectedException()
            };

            await subtask;
        }

        protected override SlashCommandProperties BuildCommandProperties(SlashCommandBuilder slashCommandBuilder)
        {
            var channelOption = new SlashCommandOptionBuilder()
                .WithName(Names.Options.Channel)
                .WithDescription("Channel for sending notifications.")
                .WithType(ApplicationCommandOptionType.Channel)
                .WithRequired(true)
                .AddChannelType(ChannelType.Text);

            var disableOption = new SlashCommandOptionBuilder()
                .WithName(Names.Options.Disable)
                .WithDescription("Disables notifications on the server.")
                .WithType(ApplicationCommandOptionType.SubCommand);

            var enableOption = new SlashCommandOptionBuilder()
                .WithName(Names.Options.Enable)
                .WithDescription("Enables notifications on the specified channel.")
                .WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption(channelOption);

            return slashCommandBuilder
                .WithDefaultMemberPermissions(GuildPermission.Administrator)
                .WithDMPermission(false)
                .AddOption(disableOption)
                .AddOption(enableOption)
                .Build();
        }

        private async Task DisableNotificationsAsync(RestSlashCommand command, RestWebhook? webhook)
        {
            if (webhook == null)
            {
                throw new MissingWebhookException();
            }

            var embedBuilder = CreateEmbedBuilder()
                .WithTitle("Notifications")
                .WithDescription("Notifications have been disabled on this server.");

            await webhook.DeleteAsync();
            await command.FollowupAsync(embed: embedBuilder.Build());
        }

        private async Task EnableNotificationsAsync(RestSlashCommand command, RestSlashCommandDataOption option, RestWebhook? webhook)
        {
            var suboption = option.Options.FirstOrDefault();

            if (suboption?.Name != Names.Options.Channel || suboption.Value is not RestTextChannel channel)
            {
                throw new UnexpectedException();
            }

            if (webhook?.ChannelId == channel.Id)
            {
                throw new ExistingWebhookException(channel);
            }

            var embedBuilder = CreateEmbedBuilder()
                .WithTitle("Notifications")
                .WithDescription(webhook == null
                    ? $"Notifications have been enabled on the {channel.Mention} channel."
                    : $"The channel for notifications has been changed to {channel.Mention}.");

            await (webhook?.ModifyAsync(w => w.ChannelId = channel.Id) ?? channel.CreateWebhookAsync(Names.Webhooks.Notifications));
            await command.FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
