using Discord;
using System.Collections.Generic;
using System.Linq;

namespace Tibres.Discord
{
    public static class GuildEmoteExtensions
    {
        public static IEmote? FindEmoji(this IEnumerable<GuildEmote> emotes, Emoji emoji, ulong creatorId)
        {
            var name = emoji.ToName();

            return emotes.FirstOrDefault(e => e.Name == name && e.CreatorId == creatorId);
        }
    }
}
