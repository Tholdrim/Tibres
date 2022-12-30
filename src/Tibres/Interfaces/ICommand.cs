using Discord;
using System.Threading.Tasks;

namespace Tibres
{
    internal interface ICommand
    {
        string Name { get; }

        string Category { get; }

        CommandDescription Description { get; }

        Task HandleInteractionAsync(ISlashCommandInteraction slashCommand);
    }
}
