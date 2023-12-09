using Discord;

namespace Tibres.Commands
{
    public interface IPermission
    {
        string Name { get; }

        void CheckIfGranted(GuildPermissions permissions);
    }
}
