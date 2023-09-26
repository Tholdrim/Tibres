using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tibres.Integrations
{
    public static class JsonHelper
    {
        private static JsonSerializerOptions Options { get; } = ConfigureJsonOptions(new(JsonSerializerDefaults.Web));

        public static JsonSerializerOptions ConfigureJsonOptions(JsonSerializerOptions options)
        {
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            return options;
        }

        public static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<T>(stream, Options);

                return result ?? throw new Exception();// UnexpectedResponseException(response);
            }
            catch (JsonException exception)
            {
                throw new Exception("", exception);//new UnexpectedResponseException(response, exception);
            }
        }
    }
}
