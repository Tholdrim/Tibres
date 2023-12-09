namespace Tibres.Discord
{
    public class UnknownGuildException(ulong guildId)
        : FormattedException($"Server with ID **{guildId}** does not exist or is unavailable.")
    {
    }
}
