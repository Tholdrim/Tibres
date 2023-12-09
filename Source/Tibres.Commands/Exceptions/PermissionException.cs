using Tibres.Discord;

namespace Tibres.Commands
{
    internal class PermissionException(Permission permission)
        : FormattedException($"**{permission.Name}** permission is not granted to the bot.")
    {
    }
}
