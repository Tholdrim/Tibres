using System;

namespace Tibres.Commands
{
    internal class MissingWebhookException()
        : Exception("Notifications are already disabled on this server.")
    {
    }
}
