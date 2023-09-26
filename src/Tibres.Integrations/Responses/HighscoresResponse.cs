using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tibres.Integrations
{
    /// <remarks>
    /// The class names were taken directly from the <see href="https://docs.tibiadata.com/swagger.json">TibiaData Swagger file</see>.
    /// </remarks>
    internal class HighscoresResponse
    {
        [JsonPropertyName("highscores")]
        public Highscores? Result { get; init; }

        [JsonPropertyName("error")]
        public string? Error { get; init; }

        internal class Highscore
        {
            [JsonRequired]
            [JsonPropertyName("rank")]
            public int Rank { get; init; }

            [JsonRequired]
            [JsonPropertyName("name")]
            public string Name { get; init; } = null!;

            [JsonRequired]
            [JsonPropertyName("level")]
            public int Level { get; init; }

            [JsonRequired]
            [JsonPropertyName("value")]
            public long Value { get; init; }

            [JsonRequired]
            [JsonPropertyName("vocation")]
            public string Vocation { get; init; } = null!;
        }

        internal class HighscorePage
        {
            [JsonRequired]
            [JsonPropertyName("current_page")]
            public int CurrentPage { get; init; }

            [JsonRequired]
            [JsonPropertyName("total_pages")]
            public int TotalPages { get; init; }

            [JsonRequired]
            [JsonPropertyName("total_records")]
            public int TotalRecords { get; init; }
        }

        internal class Highscores
        {
            [JsonRequired]
            [JsonPropertyName("highscore_list")]
            public IEnumerable<Highscore> List { get; init; } = null!;

            [JsonRequired]
            [JsonPropertyName("highscore_page")]
            public HighscorePage Page { get; init; } = null!;
        }
    }
}
