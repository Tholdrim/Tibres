using Discord;

namespace Tibres.Discord
{
    public static class EmojiExtensions
    {
        public static string ToName(this Emoji emoji) => emoji.ToString().ToLower();

        internal static string ToMarkdown(this Emoji emoji, GuildEmote? emote) => emote?.ToString() ?? $":{emoji.ToName()}:";
    }
}
