namespace Tibres.Commands
{
    public interface ICommandMetadata
    {
        string Name { get; }

        string Category { get; }

        CommandDescription Description { get; }
    }
}
