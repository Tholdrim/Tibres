using Discord;
using Discord.Rest;
using System.Linq;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    public static class RestGuildExtensions
    {
        public static async Task<IEmote?> FindEmojiAsync(this RestGuild guild, Emoji emoji, ulong creatorId)
        {
            var emotes = await guild.GetEmotesAsync();

            return emotes.FindEmoji(emoji, creatorId);
        }

        public static async Task<RestWebhook?> FindWebhookAsync(this RestGuild guild, string name, ulong creatorId)
        {
            var webhooks = await guild.GetWebhooksAsync();

            return webhooks.FirstOrDefault(w => w.Name == name && w.Creator.Id == creatorId);
        }
    }
}
