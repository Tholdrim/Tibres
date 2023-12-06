using System;

namespace Tibres.Discord
{
    public class UnknownGuildException(ulong guildId)
        : Exception($"Server with ID {guildId} does not exist or is unavailable.")
    {
    }
}
