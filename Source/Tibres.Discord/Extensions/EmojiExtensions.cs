namespace Tibres.Discord
{
    public static class EmojiExtensions
    {
        public static string ToName(this Emoji emoji) => emoji.ToString().ToLower();
    }
}
