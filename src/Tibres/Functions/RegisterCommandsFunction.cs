using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tibres.Commands;

namespace Tibres
{
    internal class RegisterCommandsFunction
    {
        private readonly IDiscordClient _discordClient;
        private readonly ICommandRepository _commandRepository;

        public RegisterCommandsFunction(IDiscordClient discordClient, ICommandRepository commandRepository)
        {
            _discordClient = discordClient;
            _commandRepository = commandRepository;
        }

        [Function(Names.Functions.RegisterCommands)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "registrations")] HttpRequestData request)
        {
            var client = await _discordClient.GetInternalClientAsync();

            await client.BulkOverwriteGlobalCommands(_commandRepository.GetAllCommandProperties().ToArray());

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
