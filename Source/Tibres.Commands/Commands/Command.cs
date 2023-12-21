using Discord;
using Discord.Rest;
using System.Threading.Tasks;
using Tibres.Discord;

namespace Tibres.Commands
{
    internal abstract class Command : ICommand, ICommandMetadata
    {
        public abstract string Name { get; }

        public abstract string Category { get; }

        public virtual string Description => "Empty";

        public abstract Task HandleInteractionAsync(RestSlashCommand command);

        internal SlashCommandProperties GetCommandProperties()
        {
            var slashCommandBuilder = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description);

            return BuildCommandProperties(slashCommandBuilder);
        }

        protected abstract SlashCommandProperties BuildCommandProperties(SlashCommandBuilder slashCommandBuilder);

        protected EmbedBuilder CreateEmbedBuilder()
        {
            var color = Category switch
            {
                Categories.Administrative => Color.DarkGreen,
                Categories.Other          => Color.LightGrey,
                _                         => throw new UnexpectedException()
            };

            return new EmbedBuilder().WithColor(color);
        }
    }
}
