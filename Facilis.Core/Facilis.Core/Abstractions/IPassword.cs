namespace Facilis.Core.Abstractions
{
    public interface IPassword
    {
        string HashingMethod { get; }
        string HashedPassword { get; }
        string PasswordSalt { get; }
        int PasswordIterated { get; }
    }

    public class Password : IPassword
    {
        public string HashingMethod { get; set; }
        public string HashedPassword { get; set; }
        public string PasswordSalt { get; set; }
        public int PasswordIterated { get; set; }
    }
}