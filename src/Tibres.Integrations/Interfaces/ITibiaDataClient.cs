using System.Collections.Generic;
using System.Threading.Tasks;
using Tibres.Data;

namespace Tibres.Integrations
{
    public interface ITibiaDataClient
    {
        Task<IReadOnlyCollection<HighScoreEntry>> GetHighscoresAsync(string world, HighscoreCategory category, Vocation vocation = Vocation.All);
    }
}
