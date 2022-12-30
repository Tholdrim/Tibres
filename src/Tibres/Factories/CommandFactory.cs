using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Tibres
{
    internal class CommandFactory : ICommandFactory
    {
        public CommandFactory(IServiceProvider services)
        {
            Commands = new Lazy<IDictionary<string, ICommand>>(
                () => services.GetRequiredService<IEnumerable<ICommand>>().ToDictionary(c => c.Name),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private Lazy<IDictionary<string, ICommand>> Commands { get; }

        public IEnumerable<ICommand> GetAllCommands() => Commands.Value.Values;

        public bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command) => Commands.Value.TryGetValue(name, out command);
    }
}
