using AutoMapper;
using MICE.API.Models;
using MICE.API.DTOs;
using MICE.API.Repositories;

namespace MICE.API.Services
{
    public class MeetingRoomService : IMeetingRoomService
    {
        private readonly IMeetingRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MeetingRoomService> _logger;

        public MeetingRoomService(
            IMeetingRoomRepository roomRepository,
            IMapper mapper,
            ILogger<MeetingRoomService> logger)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MeetingRoomDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            return room != null ? _mapper.Map<MeetingRoomDto>(room) : null;
        }

        public async Task<List<MeetingRoomDto>> GetRoomsByHotelAsync(int hotelId)
        {
            var rooms = await _roomRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<MeetingRoomDto>>(rooms);
        }

        public async Task<List<MeetingRoomDto>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int capacity)
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync(startDate, endDate, capacity);
            return _mapper.Map<List<MeetingRoomDto>>(rooms);
        }

        public async Task<MeetingRoomDto> CreateRoomAsync(CreateMeetingRoomDto dto)
        {
            var room = _mapper.Map<MeetingRoom>(dto);
            room.IsActive = true;
            room.CreatedAt = DateTime.Now;
            
            await _roomRepository.AddAsync(room);
            return _mapper.Map<MeetingRoomDto>(room);
        }

        public async Task<MeetingRoomDto> UpdateRoomAsync(int id, UpdateMeetingRoomDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                throw new Exception("Meeting room not found");
            
            _mapper.Map(dto, room);
            room.UpdatedAt = DateTime.Now;
            
            await _roomRepository.UpdateAsync(room);
            return _mapper.Map<MeetingRoomDto>(room);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await _roomRepository.DeleteAsync(id);
        }

        public async Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            return await _roomRepository.CheckAvailabilityAsync(roomId, startDate, endDate);
        }
    }
}