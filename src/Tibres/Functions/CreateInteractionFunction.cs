using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Tibres
{
    internal class CreateInteractionFunction
    {
        private readonly IBotConfiguration _configuration;

        public CreateInteractionFunction(IBotConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function(Names.Functions.CreateInteraction)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "interactions")] HttpRequestData request)
        {
            try
            {
                var publicKey = _configuration.PublicKey;
                var (body, signature, timestamp) = await GetInteractionMessageAsync(request);

                var client = new DiscordRestClient();
                var interaction = await client.ParseHttpInteractionAsync(publicKey, signature, timestamp, body, doApiCallOnCreation: _ => false);

                return interaction switch
                {
                    RestPingInteraction pingInteraction => await PrepareResponseAsync(request, pingInteraction.AcknowledgePing()),
                    _                                   => request.CreateResponse(HttpStatusCode.NotImplemented)
                };
            }
            catch (Exception exception) when (exception is BadRequestException or BadSignatureException)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private static async Task<InteractionMessage> GetInteractionMessageAsync(HttpRequestData request)
        {
            var signature = GetRequiredHeaderValue("x-signature-ed25519");
            var timestamp = GetRequiredHeaderValue("x-signature-timestamp");

            var body = await request.ReadAsStringAsync() ?? throw new BadRequestException();

            return new InteractionMessage(body, signature, timestamp);

            string GetRequiredHeaderValue(string name)
            {
                if (!request.Headers.TryGetValues(name, out var values) || values.Count() != 1)
                {
                    throw new BadRequestException();
                }

                return values.Single();
            }
        }

        private static async Task<HttpResponseData> PrepareResponseAsync(HttpRequestData request, string json)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);

            await response.WriteStringAsync(json);

            return response;
        }
    }
}
