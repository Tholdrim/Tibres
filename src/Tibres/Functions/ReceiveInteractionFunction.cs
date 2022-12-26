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
    internal class ReceiveInteractionFunction
    {
        private readonly IBotConfiguration _configuration;

        public ReceiveInteractionFunction(IBotConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function(Names.Functions.ReceiveInteraction)]
        public async Task<Output> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "webhook")] HttpRequestData request)
        {
            try
            {
                var client = new DiscordRestClient();
                var message = await GetMessageAsync(request);
                var interaction = await client.ParseHttpInteractionAsync(message, _configuration.PublicKey, doApiCallOnCreation: _ => false);

                return interaction switch
                {
                    RestSlashCommand slashCommand       => await CreateOutputWithMessageAsync(request, message, json: slashCommand.Defer()),
                    RestPingInteraction pingInteraction => await PrepareResponseAsync(request, json: pingInteraction.AcknowledgePing()),
                    _                                   => request.CreateResponse(HttpStatusCode.NotImplemented)
                };
            }
            catch (Exception exception) when (exception is BadRequestException or BadSignatureException)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private static async Task<Output> CreateOutputWithMessageAsync(HttpRequestData request, InteractionMessage message, string json) => new()
        {
            Response = await PrepareResponseAsync(request, json),
            Message = message
        };

        private static async Task<InteractionMessage> GetMessageAsync(HttpRequestData request)
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

        internal class Output
        {
            public HttpResponseData? Response { get; init; }

            [QueueOutput(Names.Queues.Interactions)]
            public InteractionMessage? Message { get; init; }

            public static implicit operator Output(HttpResponseData response) => new()
            {
                Response = response
            };
        }
    }
}
