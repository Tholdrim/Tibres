using Discord.Rest;
using System.Threading.Tasks;

namespace Tibres.Commands
{
    public interface ICommand
    {
        Task HandleInteractionAsync(RestSlashCommand command);
    }
}
