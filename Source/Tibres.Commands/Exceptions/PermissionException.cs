using Tibres.Discord;

namespace Tibres.Commands
{
    internal class PermissionException(Permission permission)
        : FormattedException($"The **{permission.Name}** permission is not granted to the bot.")
    {
    }
}
