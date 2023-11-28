using Discord;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    internal class EmojiRepository(IDiscordClient discordClient) : IEmojiRepository
    {
        private readonly IDiscordClient _discordClient = discordClient;
 
        private ConcurrentDictionary<Emoji, string> Cache { get; } = new();

        public async Task<string> GetEmojiAsync(Emoji emoji)
        {
            if (!Cache.TryGetValue(emoji, out var result))
            {
                var emote = await GetEmoteAsync(emoji);

                result = Cache.GetOrAdd(emoji, _ => emote.ToMarkdown());
            }
            
            return result;
        }

        public void UpdateEmoji(Emoji emoji, GuildEmote emote)
        {
            Cache[emoji] = emote.ToMarkdown();
        }

        private async Task<GuildEmote?> GetEmoteAsync(Emoji emoji)
        {
            var guild = await _discordClient.GetMainGuildAsync();

            if (guild == null)
            {
                return null;
            }

            var bot = await guild.GetCurrentUserAsync();
            var emotes = await guild.GetEmotesAsync();

            return emotes.FindBotEmoji(emoji, bot.Id);
        }
    }
}
