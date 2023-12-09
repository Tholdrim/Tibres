namespace Tibres.Discord
{
    public class UnexpectedException(string message = "An unexpected exception occurred.")
        : FormattedException(message)
    {
    }
}
