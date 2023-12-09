using Tibres.Discord;

namespace Tibres
{
    internal class UnknownCommandException()
        : UnexpectedException("The specified command was not recognized.")
    {
    }
}
