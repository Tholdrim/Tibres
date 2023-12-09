using Discord;
using System;

namespace Tibres.Commands
{
    internal record Permission(string Name) : IPermission
    {
        internal required string Description { get; init; }

        internal required Func<GuildPermissions, bool> Selector { get; init; }

        internal Func<ulong?, ulong, bool>? Filter { get; init; }

        internal bool IsOptional { get; init; } = false;

        void IPermission.CheckIfGranted(GuildPermissions permissions)
        {
            if (!Selector(permissions))
            {
                throw new PermissionException(this);
            }
        }
    };
}
