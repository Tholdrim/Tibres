using Discord;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    public interface ICommand
    {
        Task HandleInteractionAsync(ISlashCommandInteraction slashCommand);
    }
}
