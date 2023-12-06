namespace Tibres
{
    internal static class Names
    {
        public static class AppSettings
        {
            public const string InitializationFrequency = nameof(InitializationFrequency);
        }

        public static class BlobContainers
        {
            public const string Emojis = "emojis";
        }

        public static class Functions
        {
            public const string HandleInteraction = nameof(HandleInteraction);

            public const string InitializeBot = nameof(InitializeBot);

            public const string ReceiveInteraction = nameof(ReceiveInteraction);
        }

        public static class Queues
        {
            public const string Interactions = "interactions";
        }
    }
}
