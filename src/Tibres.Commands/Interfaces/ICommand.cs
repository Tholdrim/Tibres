using Discord;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    public interface ICommand : ICommandMetadata
    {
        Task HandleInteractionAsync(ISlashCommandInteraction slashCommand);
    }
}
