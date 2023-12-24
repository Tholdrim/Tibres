using Discord;
using Discord.Rest;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal class NotificationsCommand(IHttpClientFactory httpClientFactory) : Command
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public override string Name => Names.Commands.Notifications;

        public override string Category => Categories.Administrative;

        public override async Task HandleInteractionAsync(RestSlashCommand command)
        {
            var user = await command.Guild.GetCurrentUserAsync();

            Permissions.ManageWebhooks.CheckIfGranted(user.GuildPermissions);

            var embedBuilder = CreateEmbedBuilder().WithTitle("Notifications");
            var option = command.Data.Options.FirstOrDefault();
            var webhook = await command.Guild.FindWebhookAsync(Names.Webhooks.Notifications, creatorId: user.Id);

            var commandTask = (option?.Name, webhook) switch
            {
                (Names.Options.Disable, _)    => DisableNotificationsAsync(embedBuilder, webhook),
                (Names.Options.Enable,  null) => EnableNotificationsAsync(embedBuilder, option, user.GetDisplayAvatarUrl()),
                (Names.Options.Enable,  { })  => UpdateChannelAsync(embedBuilder, option, webhook),
                _                             => throw new UnexpectedException()
            };

            await commandTask;
            await command.FollowupAsync(embed: embedBuilder.Build());
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

        private async Task EnableNotificationsAsync(EmbedBuilder embedBuilder, RestSlashCommandDataOption option, string avatarUrl)
        {
            var channel = option.GetChannel();

            embedBuilder.WithDescription($"Notifications have been enabled on the {channel.Mention} channel.");

            using var httpClient = _httpClientFactory.CreateClient();
            using var avatarStream = await httpClient.GetStreamAsync(avatarUrl);

            await channel.CreateWebhookAsync(Names.Webhooks.Notifications, avatarStream);
        }

        private static Task DisableNotificationsAsync(EmbedBuilder embedBuilder, RestWebhook? webhook)
        {
            if (webhook == null)
            {
                throw new MissingWebhookException();
            }

            embedBuilder.WithDescription("Notifications have been disabled on this server.");

            return webhook.DeleteAsync();
        }

        private static Task UpdateChannelAsync(EmbedBuilder embedBuilder, RestSlashCommandDataOption option, RestWebhook webhook)
        {
            var channel = option.GetChannel();

            if (webhook.ChannelId == channel.Id)
            {
                throw new ExistingWebhookException(channel);
            }

            embedBuilder.WithDescription($"The channel for notifications has been changed to {channel.Mention}.");

            return webhook.ModifyAsync(w => w.ChannelId = channel.Id);
        }
    }
}
