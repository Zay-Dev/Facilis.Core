namespace Facilis.Core.Abstractions
{
    public interface IEntityWithMime
    {
        string MimeType { get; }
        string Name { get; }
    }
}