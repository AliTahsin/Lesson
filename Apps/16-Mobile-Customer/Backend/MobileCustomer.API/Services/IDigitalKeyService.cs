using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Services
{
    public interface IDigitalKeyService
    {
        Task<DigitalKeyDto> GenerateKeyAsync(int reservationId, int userId);
        Task<DigitalKeyDto> GetKeyByReservationAsync(int reservationId);
        Task<bool> ValidateKeyAsync(string keyCode, int roomId);
        Task<bool> UseKeyAsync(string keyCode);
        Task<List<DigitalKeyDto>> GetActiveKeysAsync(int userId);
        Task<string> GenerateQrCodeAsync(int reservationId);
    }
}