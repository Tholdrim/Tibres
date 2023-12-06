using Discord;
using System;

namespace Tibres.Commands
{
    internal record Permission(string Name)
    {
        public required string Description { get; init; }

        public required Func<GuildPermissions, bool> Selector { get; init; }

        public Func<ulong?, ulong, bool>? Filter { get; init; }

        public bool IsOptional { get; init; } = false;
    };
}
