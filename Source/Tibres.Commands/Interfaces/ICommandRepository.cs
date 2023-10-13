using Discord;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tibres.Commands
{
    public interface ICommandRepository
    {
        IEnumerable<ICommandMetadata> GetAllCommandMetadata();

        SlashCommandProperties[] GetAllCommandProperties();

        bool TryGetCommand(string name, [NotNullWhen(true)] out ICommand? command);
    }
}
