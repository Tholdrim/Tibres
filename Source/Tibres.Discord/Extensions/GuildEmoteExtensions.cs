using Discord;
using System.Collections.Generic;
using System.Linq;

namespace Tibres.Discord
{
    public static class GuildEmoteExtensions
    {
        public static GuildEmote? FindBotEmoji(this IEnumerable<GuildEmote> emotes, Emoji emoji, ulong creatorId)
        {
            var name = emoji.ToName();

            return emotes.FirstOrDefault(e => e.CreatorId == creatorId && e.Name == name);
        }

        public static string ToMarkdown(this GuildEmote? emote) => emote != null ? $" {emote} " : string.Empty;
    }
}
