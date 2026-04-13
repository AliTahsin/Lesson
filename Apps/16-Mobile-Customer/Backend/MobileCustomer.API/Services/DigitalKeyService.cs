using AutoMapper;
using MobileCustomer.API.Models;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Repositories;
using System.Text;

namespace MobileCustomer.API.Services
{
    public class DigitalKeyService : IDigitalKeyService
    {
        private readonly IDigitalKeyRepository _keyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DigitalKeyService> _logger;

        public DigitalKeyService(
            IDigitalKeyRepository keyRepository,
            IMapper mapper,
            ILogger<DigitalKeyService> logger)
        {
            _keyRepository = keyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DigitalKeyDto> GenerateKeyAsync(int reservationId, int userId)
        {
            var existingKey = await _keyRepository.GetByReservationAsync(reservationId);
            if (existingKey != null)
            {
                return _mapper.Map<DigitalKeyDto>(existingKey);
            }

            var keyCode = GenerateUniqueKeyCode();
            var qrCode = GenerateQrCode(keyCode);
            
            var digitalKey = new DigitalKey
            {
                ReservationId = reservationId,
                UserId = userId,
                RoomId = GetRoomIdFromReservation(reservationId),
                RoomNumber = GetRoomNumberFromReservation(reservationId),
                KeyCode = keyCode,
                QrCode = qrCode,
                ValidFrom = DateTime.Now,
                ValidUntil = DateTime.Now.AddDays(GetReservationDaysLeft(reservationId)),
                IsActive = true,
                IsUsed = false,
                CreatedAt = DateTime.Now
            };

            await _keyRepository.CreateAsync(digitalKey);
            return _mapper.Map<DigitalKeyDto>(digitalKey);
        }

        public async Task<DigitalKeyDto> GetKeyByReservationAsync(int reservationId)
        {
            var key = await _keyRepository.GetByReservationAsync(reservationId);
            return key != null ? _mapper.Map<DigitalKeyDto>(key) : null;
        }

        public async Task<bool> ValidateKeyAsync(string keyCode, int roomId)
        {
            return await _keyRepository.ValidateKeyAsync(keyCode, roomId);
        }

        public async Task<bool> UseKeyAsync(string keyCode)
        {
            var key = await _keyRepository.GetByKeyCodeAsync(keyCode);
            if (key == null) return false;
            
            return await _keyRepository.UseKeyAsync(key.Id);
        }

        public async Task<List<DigitalKeyDto>> GetActiveKeysAsync(int userId)
        {
            var keys = await _keyRepository.GetActiveKeysByUserAsync(userId);
            return _mapper.Map<List<DigitalKeyDto>>(keys);
        }

        public async Task<string> GenerateQrCodeAsync(int reservationId)
        {
            var key = await _keyRepository.GetByReservationAsync(reservationId);
            if (key == null) return null;
            
            return key.QrCode;
        }

        private string GenerateUniqueKeyCode()
        {
            return $"DK-{DateTime.Now.Ticks}-{new Random().Next(1000, 9999)}";
        }

        private string GenerateQrCode(string keyCode)
        {
            // Simulate QR code generation
            return $"https://api.hotel.com/digital-key/{Convert.ToBase64String(Encoding.UTF8.GetBytes(keyCode))}";
        }

        private int GetRoomIdFromReservation(int reservationId)
        {
            // Mock: return room ID based on reservation
            return (reservationId % 10) + 1;
        }

        private string GetRoomNumberFromReservation(int reservationId)
        {
            return $"{(reservationId % 5) + 1}0{(reservationId % 10)}";
        }

        private int GetReservationDaysLeft(int reservationId)
        {
            // Mock: return days left
            var random = new Random();
            return random.Next(1, 7);
        }
    }
}