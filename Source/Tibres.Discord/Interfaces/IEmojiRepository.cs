using Discord;
using System.Threading.Tasks;

namespace Tibres.Discord
{
    public interface IEmojiRepository
    {
        Task<string> GetEmojiAsync(Emoji emoji);

        void UpdateEmoji(Emoji emoji, IEmote emote);
    }
}
