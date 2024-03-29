using Discord;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Tibres.Commands
{
    internal class CommandRepository : ICommandRepository
    {
        public CommandRepository(IServiceProvider services)
        {
            Commands = new Lazy<IDictionary<string, Command>>(
                () => services.GetRequiredService<IEnumerable<Command>>().ToDictionary(c => c.Name),
                LazyThreadSafetyMode.PublicationOnly);
        }

        private Lazy<IDictionary<string, Command>> Commands { get; }

        public IEnumerable<ICommandMetadata> GetAllCommandMetadata() => Commands.Value.Values;

        public SlashCommandProperties[] GetAllCommandProperties() => Commands.Value.Values.Select(c => c.GetCommandProperties()).ToArray();

        public bool TryGetCommand(string name, [NotNullWhen(true)] out ICommand? command)
        {
            var result = Commands.Value.TryGetValue(name, out var value);

            command = value;

            return result;
        }
    }
}
