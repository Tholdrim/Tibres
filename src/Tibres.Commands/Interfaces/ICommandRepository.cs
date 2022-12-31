using Discord;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tibres.Commands
{
    public interface ICommandRepository
    {
        IEnumerable<ICommandMetadata> GetAllCommandMetadata();

        IEnumerable<SlashCommandProperties> GetAllCommandProperties();

        bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);
    }
}
