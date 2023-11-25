using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tibres.Commands;

namespace Tibres
{
    internal class RegisterCommandsFunction(IDiscordClient discordClient, ICommandRepository commandRepository)
    {
        private readonly IDiscordClient _discordClient = discordClient;
        private readonly ICommandRepository _commandRepository = commandRepository;

        [Function(Names.Functions.RegisterCommands)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "registrations")] HttpRequestData request)
        {
            var client = await _discordClient.GetInternalClientAsync();
            var commandProperties = _commandRepository.GetAllCommandProperties();

            await client.BulkOverwriteGlobalCommands(commandProperties);

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
