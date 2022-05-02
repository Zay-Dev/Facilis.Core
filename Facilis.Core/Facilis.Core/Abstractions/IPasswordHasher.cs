namespace Facilis.Core.Abstractions
{
    public interface IPasswordHasher
    {
        IPassword Hash(string value);

        bool Verify(IPassword hashed, string value);
    }
}