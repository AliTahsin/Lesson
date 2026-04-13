using AutoMapper;
using Staff.API.Models;
using Staff.API.DTOs;
using Staff.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Staff.API.Services
{
    public class CheckService : ICheckService
    {
        private readonly ICheckRepository _checkRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<CheckService> _logger;

        public CheckService(
            ICheckRepository checkRepository,
            IStaffRepository staffRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<CheckService> logger)
        {
            _checkRepository = checkRepository;
            _staffRepository = staffRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<CheckDto> CheckInAsync(CheckInDto dto, int staffId)
        {
            var canCheckIn = await CanCheckInAsync(dto.ReservationId);
            if (!canCheckIn)
                throw new Exception("Cannot check in this reservation");

            var staff = await _staffRepository.GetByIdAsync(staffId);
            var digitalKey = GenerateDigitalKey();

            var check = new CheckInOut
            {
                ReservationId = dto.ReservationId,
                GuestId = dto.GuestId,
                GuestName = dto.GuestName,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                ProcessedByStaffId = staffId,
                ProcessedByStaffName = staff?.FullName,
                Type = "CheckIn",
                Notes = dto.Notes,
                DigitalKey = digitalKey
            };

            await _checkRepository.AddAsync(check);
            await _hubContext.Clients.Group($"hotel-{dto.HotelId}").SendAsync("GuestCheckedIn", check);

            return _mapper.Map<CheckDto>(check);
        }

        public async Task<CheckDto> CheckOutAsync(CheckOutDto dto, int staffId)
        {
            var canCheckOut = await CanCheckOutAsync(dto.ReservationId);
            if (!canCheckOut)
                throw new Exception("Cannot check out this reservation");

            var staff = await _staffRepository.GetByIdAsync(staffId);

            var check = new CheckInOut
            {
                ReservationId = dto.ReservationId,
                GuestId = dto.GuestId,
                GuestName = dto.GuestName,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                ProcessedByStaffId = staffId,
                ProcessedByStaffName = staff?.FullName,
                Type = "CheckOut",
                Notes = dto.Notes
            };

            await _checkRepository.AddAsync(check);
            await _hubContext.Clients.Group($"hotel-{dto.HotelId}").SendAsync("GuestCheckedOut", check);

            return _mapper.Map<CheckDto>(check);
        }

        public async Task<List<CheckDto>> GetTodayCheckInsAsync(int hotelId)
        {
            var checks = await _checkRepository.GetByDateAsync(DateTime.Today);
            return _mapper.Map<List<CheckDto>>(checks.Where(c => c.Type == "CheckIn"));
        }

        public async Task<List<CheckDto>> GetTodayCheckOutsAsync(int hotelId)
        {
            var checks = await _checkRepository.GetByDateAsync(DateTime.Today);
            return _mapper.Map<List<CheckDto>>(checks.Where(c => c.Type == "CheckOut"));
        }

        public async Task<bool> CanCheckInAsync(int reservationId)
        {
            return !(await _checkRepository.HasCheckedInAsync(reservationId));
        }

        public async Task<bool> CanCheckOutAsync(int reservationId)
        {
            return await _checkRepository.HasCheckedInAsync(reservationId) && 
                   !(await _checkRepository.HasCheckedOutAsync(reservationId));
        }

        private string GenerateDigitalKey()
        {
            return $"DK-{DateTime.Now.Ticks}-{new Random().Next(1000, 9999)}";
        }
    }
}