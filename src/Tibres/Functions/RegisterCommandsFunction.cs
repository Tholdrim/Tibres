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
        private readonly ICommandFactory _commandFactory;
        private readonly IBotConfiguration _configuration;

        public RegisterCommandsFunction(ICommandFactory commandFactory, IBotConfiguration configuration)
        {
            _commandFactory = commandFactory;
            _configuration = configuration;
        }

        [Function(Names.Functions.RegisterCommands)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "registrations")] HttpRequestData request)
        {
            var discord = new DiscordRestClient();

            await discord.LoginAsync(TokenType.Bot, _configuration.Token);
            await discord.BulkOverwriteGlobalCommands(_commandFactory.GetAllCommandMetadata().Select(BuildCommand).ToArray());

            return request.CreateResponse(HttpStatusCode.OK);
        }

        private static SlashCommandProperties BuildCommand(ICommandMetadata command) => new SlashCommandBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description.Chat)
            .Build();
    }
}
