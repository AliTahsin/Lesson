using BCrypt.Net;

namespace Auth.API.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Verify(password, hash);
        }
    }
}