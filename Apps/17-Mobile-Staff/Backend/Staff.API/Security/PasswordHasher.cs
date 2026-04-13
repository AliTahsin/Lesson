using BCrypt.Net;

namespace Staff.API.Security
{
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