using Discord;
using Discord.Rest;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    internal abstract class Command : ICommand, ICommandMetadata
    {
        public abstract string Name { get; }

        public abstract string Category { get; }

        public abstract string Description { get; }

        public abstract Task HandleInteractionAsync(RestSlashCommand command);

        internal SlashCommandProperties GetCommandProperties()
        {
            var slashCommandBuilder = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description);

            return BuildCommandProperties(slashCommandBuilder);
        }

        protected abstract SlashCommandProperties BuildCommandProperties(SlashCommandBuilder slashCommandBuilder);
    }
}
