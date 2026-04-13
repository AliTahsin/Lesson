using MobileCustomer.API.Models;

namespace MobileCustomer.API.Repositories
{
    public interface IDigitalKeyRepository
    {
        Task<DigitalKey> GetByIdAsync(int id);
        Task<DigitalKey> GetByReservationAsync(int reservationId);
        Task<DigitalKey> GetByKeyCodeAsync(string keyCode);
        Task<DigitalKey> CreateAsync(DigitalKey digitalKey);
        Task<DigitalKey> UpdateAsync(DigitalKey digitalKey);
        Task<bool> ValidateKeyAsync(string keyCode, int roomId);
        Task<bool> UseKeyAsync(int id);
        Task<List<DigitalKey>> GetActiveKeysByUserAsync(int userId);
    }
}