namespace Facilis.Core.Abstractions
{
    public interface IEntityWithScope
    {
        string Scope { get; set; }
        string ScopedId { get; set; }
    }
}