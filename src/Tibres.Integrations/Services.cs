using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Tibres.Integrations
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ITibiaDataClient, TibiaDataClient>(ConfigureHttpClient);
            // TODO: Move to the client level
            // .AddPolicyHandler(TransientHttpErrorRetryPolicy);
        }

        // TODO: Check if executed only once
        // TODO: Check if can be used on the whole client level
        private static IAsyncPolicy<HttpResponseMessage> TransientHttpErrorRetryPolicy =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.tibiadata.com/v3");
        }
    }
}
