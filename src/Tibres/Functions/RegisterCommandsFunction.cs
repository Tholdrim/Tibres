using Discord;
using Discord.Rest;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tibres
{
    internal class RegisterCommandsFunction
    {
        private readonly IBotConfiguration _configuration;

        public RegisterCommandsFunction(IBotConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function(Names.Functions.RegisterCommands)]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "registrations")] HttpRequestData request)
        {
            var discord = new DiscordRestClient();

            await discord.LoginAsync(TokenType.Bot, _configuration.Token);

            await discord.BulkOverwriteGlobalCommands(new[]
            {
                new SlashCommandBuilder()
                    .WithName("help")
                    .WithDescription("Displays a brief description of the bot and information about available commands.")
                    .Build()
            });

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
