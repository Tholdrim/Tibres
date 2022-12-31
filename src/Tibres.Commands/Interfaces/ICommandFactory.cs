
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tibres.Commands
{
    public interface ICommandFactory
    {
        IEnumerable<ICommandMetadata> GetAllCommandMetadata();

        bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);
    }
}
