using Discord;
using Discord.Rest;
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
        private readonly ICommandRepository _commandRepository;
        private readonly IBotConfiguration _configuration;

        public RegisterCommandsFunction(ICommandRepository commandRepository, IBotConfiguration configuration)
        {
            _commandRepository = commandRepository;
            _configuration = configuration;
        }

        [Function(Names.Functions.RegisterCommands)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "registrations")] HttpRequestData request)
        {
            var discord = new DiscordRestClient();

            await discord.LoginAsync(TokenType.Bot, _configuration.Token);
            await discord.BulkOverwriteGlobalCommands(_commandRepository.GetAllCommandProperties().ToArray());

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
