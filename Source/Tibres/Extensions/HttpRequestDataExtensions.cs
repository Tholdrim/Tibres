using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Tibres
{
    internal static class HttpRequestDataExtensions
    {
        public static HttpResponseData CreateJsonResponse(this HttpRequestData request, string json)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(json);

            return response;
        }
    }
}
