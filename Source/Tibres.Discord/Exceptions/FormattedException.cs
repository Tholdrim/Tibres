using System;

namespace Tibres.Discord
{
    public abstract class FormattedException(string formattedMessage)
        : Exception(RemoveMarkdown(formattedMessage))
    {
        public string FormattedMessage { get; } = formattedMessage;

        private static string RemoveMarkdown(string markdown) => markdown.Replace("*", null);
    }
}
