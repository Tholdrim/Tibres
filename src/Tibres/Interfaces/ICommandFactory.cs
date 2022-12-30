
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tibres
{
    internal interface ICommandFactory
    {
        IEnumerable<ICommand> GetAllCommands();

        bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);
    }
}
