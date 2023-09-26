using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tibres.Data
{
    public class HighScores
    {
        [JsonRequired]
        public Vocation Vocation { get; init; }

        [JsonRequired]
        public string World { get; init; } = null!;

        [JsonRequired]
        public IDictionary<HighscoreCategory, IReadOnlyCollection<HighScoreEntry>> Entries { get; init; } = null!;
    }
}
