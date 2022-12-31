using Discord;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    internal abstract class Command : ICommand
    {
        public abstract string Name { get; }

        public abstract string Category { get; }

        public abstract CommandDescription Description { get; }

        public abstract Task HandleInteractionAsync(ISlashCommandInteraction slashCommand);

        internal SlashCommandProperties GetCommandProperties()
        {
            var slashCommandBuilder = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description.Chat);

            slashCommandBuilder = ExtendCommandDefinition(slashCommandBuilder);

            return slashCommandBuilder.Build();
        }

        protected virtual SlashCommandBuilder ExtendCommandDefinition(SlashCommandBuilder slashCommandBuilder) => slashCommandBuilder;
    }
}
