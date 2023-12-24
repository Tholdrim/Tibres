using Discord.Rest;
using System.Linq;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal static class RestSlashCommandDataOptionExtensions
    {
        public static RestTextChannel GetChannel(this RestSlashCommandDataOption option)
        {
            var suboption = option.Options.FirstOrDefault();

            if (suboption?.Name != Names.Options.Channel || suboption.Value is not RestTextChannel channel)
            {
                throw new UnexpectedException();
            }

            return channel;
        }
    }
}
