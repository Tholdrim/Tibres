using System.Text.Json.Serialization;

namespace Tibres.Data
{
    public class HighScoreEntry
    {
        [JsonRequired]
        public int Rank { get; init; }

        [JsonRequired]
        public string Name { get; init; } = null!;

        [JsonRequired]
        public int Level { get; init; }

        [JsonRequired]
        public long Value { get; init; }

        [JsonRequired]
        public Vocation Vocation { get; init; }
    }
}
