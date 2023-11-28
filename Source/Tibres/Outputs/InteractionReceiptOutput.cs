using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Tibres.Discord;

namespace Tibres
{
    internal class InteractionReceiptOutput
    {
        public required HttpResponseData Response { get; init; }

        [QueueOutput(Names.Queues.Interactions)]
        public InteractionMessage? Message { get; init; }
    }
}
