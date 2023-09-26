using Polly;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tibres.Data;

namespace Tibres.Integrations
{
    internal class TibiaDataClient : ITibiaDataClient
    {
        private readonly HttpClient _httpClient;

        public TibiaDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private static AsyncPolicyWrap Lol => Policy.WrapAsync<HttpResponseMessage>(T2, T3, T4);

        public async Task<IReadOnlyCollection<HighScoreEntry>> GetHighscoresAsync(string world, HighscoreCategory category, Vocation vocation)
        {
            var response = await GetHighscoresPageAsync(world, category, vocation, 1);

            var totalPages = response.Page.TotalPages;
            var totalRecords = response.Page.TotalRecords;

            for (var page = 2; page < totalPages; ++page)
            {
                var nextResponse = await GetHighscoresPageAsync(world, category, vocation, page);
            }
        }

        private async Task<HighscoresResponse.Highscores> GetHighscoresPageAsync(string world, HighscoreCategory category, Vocation vocation, int page)
        {
            var uri = new Uri($"highscores/{world}/{category}/{vocation}/{page}", UriKind.Relative);

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await _httpClient.SendAsync(request);

            var model = await JsonHelper.DeserializeAsync<HighscoresResponse>(response);

            if (!string.IsNullOrWhiteSpace(model.Error) || model.Result == null)
            {
                // TODO: Log error

                throw new Exception();
            }

            return model.Result;
        }
    }
}
