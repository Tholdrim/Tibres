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

                result = Cache.GetOrAdd(emoji, _ => emoji.ToMarkdown(emote));
            }
            
            return result;
        }

        public void UpdateEmoji(Emoji emoji, IEmote emote)
        {
            Cache[emoji] = emoji.ToMarkdown(emote);
        }

        private async Task<IEmote?> GetEmoteAsync(Emoji emoji)
        {
            var guild = await _discordClient.GetMainGuildAsync();

            if (guild == null)
            {
                return null;
            }

            var user = await guild.GetCurrentUserAsync();

            return await guild.FindEmojiAsync(emoji, creatorId: user.Id);
        }
    }
}
