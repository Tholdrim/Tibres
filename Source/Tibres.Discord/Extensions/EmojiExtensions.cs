using Discord;

namespace Tibres.Discord
{
    public static class EmojiExtensions
    {
        public static string ToName(this Emoji emoji) => emoji.ToString().ToLower();

        public static string ToMarkdown(this Emoji emoji, GuildEmote? emote) => emote?.ToString() ?? $":{emoji.ToName()}:";
    }
}
