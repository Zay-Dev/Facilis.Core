namespace Facilis.Core.Abstractions
{
    public interface IEntityWithKeyValuePair<T>
    {
        string Key { get; set; }
        T Value { get; set; }
    }

    public interface IEntityWithKeyValuePair : IEntityWithKeyValuePair<string>
    {
    }
}