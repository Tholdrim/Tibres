using System;

namespace Tibres
{
    internal class UnknownGuildException : Exception
    {
        public UnknownGuildException(ulong guildId)
            : base($"Server with ID {guildId} does not exist or is unavailable.")
        {
        }
    }
}
