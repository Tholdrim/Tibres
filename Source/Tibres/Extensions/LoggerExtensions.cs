using Microsoft.Extensions.Logging;
using Tibres.Discord;

namespace Tibres
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(
            EventId = 300,
            EventName = "NoMainServer",
            Level = LogLevel.Warning,
            Message = "Main server is not specified or unavailable. Emoji upload has been omitted.")]
        public static partial void LogNoMainServer(this ILogger logger);

        [LoggerMessage(
            EventId = 400,
            EventName = "UnexpectedError",
            Level = LogLevel.Error,
            Message = "An unexpected error has occurred that may be an implementation error. Check the exception for details.")]
        public static partial void LogUnexpectedError(this ILogger logger, UnexpectedException exception);
    }
}
