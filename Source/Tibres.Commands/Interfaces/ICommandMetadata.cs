namespace Tibres.Commands
{
    public interface ICommandMetadata
    {
        string Name { get; }

        string Category { get; }

        string Description { get; }

        string Summary { get; }
    }
}
