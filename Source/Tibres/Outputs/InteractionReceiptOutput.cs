using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Tibres
{
    internal class InteractionReceiptOutput
    {
        public required HttpResponseData Response { get; init; }

        [QueueOutput(Names.Queues.Interactions)]
        public InteractionMessage? Message { get; init; }
    }
}
