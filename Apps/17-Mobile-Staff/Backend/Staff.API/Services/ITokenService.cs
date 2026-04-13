using Staff.API.Models;

namespace Staff.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(Staff staff);
        bool ValidateToken(string token);
    }
}