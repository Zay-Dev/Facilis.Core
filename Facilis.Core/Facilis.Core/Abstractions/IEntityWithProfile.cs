namespace Facilis.Core.Abstractions
{
    public interface IEntityWithProfile
    {
        object UncastedProfile { get; }
        string SerializedProfile { get; }

        void SetProfile(object profile);
    }

    public interface IEntityWithProfile<T> : IEntityWithProfile
    {
        T Profile { get; }

        void SetProfile(T profile);
    }
}