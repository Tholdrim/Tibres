namespace Tibres.Discord
{
    public class UnexpectedException(string message = "An unexpected exception has occurred.")
        : FormattedException(message)
    {
    }
}
