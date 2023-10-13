using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tibres
{
    internal class ReceiveInteractionFunction
    {
        private readonly IDiscordClient _discordClient;

        public ReceiveInteractionFunction(IDiscordClient discordClient)
        {
            _discordClient = discordClient;
        }

        [Function(Names.Functions.ReceiveInteraction)]
        public async Task<Output> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "interactions")] HttpRequestData request)
        {
            try
            {
                var message = await GetInteractionMessageAsync(request);
                var interaction = await _discordClient.ParseHttpInteractionAsync(message, doApiCallOnCreation: _ => false);

                return interaction switch
                {
                    RestSlashCommand slashCommand       => Output.Create(request, json: slashCommand.Defer(), message),
                    RestPingInteraction pingInteraction => Output.Create(request, json: pingInteraction.AcknowledgePing(), message: null),
                    _                                   => Output.Create(request, HttpStatusCode.NotImplemented)
                };
            }
            catch (Exception exception) when (exception is BadRequestException or BadSignatureException)
            {
                return Output.Create(request, HttpStatusCode.Unauthorized);
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

        public class Output
        {
            public required HttpResponseData Response { get; init; }

            [QueueOutput(Names.Queues.Interactions)]
            public InteractionMessage? Message { get; init; }

            public static Output Create(HttpRequestData request, HttpStatusCode statusCode) => new()
            {
                Response = request.CreateResponse(statusCode)
            };

            public static Output Create(HttpRequestData request, string json, InteractionMessage? message) => new()
            {
                Response = request.CreateJsonResponse(json),
                Message = message
            };
        }
    }
}
