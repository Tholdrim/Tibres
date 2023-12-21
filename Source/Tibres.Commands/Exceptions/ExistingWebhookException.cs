using Discord.Rest;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal class ExistingWebhookException(RestTextChannel channel)
        : FormattedException($"Notifications are already enabled on the {channel.Mention} channel.")
    {
    }
}
