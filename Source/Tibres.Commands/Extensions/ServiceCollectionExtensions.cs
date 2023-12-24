using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Tibres.Commands
{
    public static class ServiceCollectionExtensions
    {
        private static IAsyncPolicy<HttpResponseMessage> RetryPolicy => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i - 1)));

        public static void AddCommands(this IServiceCollection services)
        {
            services.AddHttpClient<NotificationsCommand>().AddPolicyHandler(RetryPolicy);

            services.AddSingleton<Command, AboutCommand>();
            services.AddSingleton<Command, NotificationsCommand>();
            services.AddSingleton<Command, PermissionsCommand>();

            services.AddSingleton<ICommandRepository, CommandRepository>();
        }
    }
}
