using AutoMapper;
using MICE.API.Models;
using MICE.API.DTOs;
using MICE.API.Repositories;
using System.Text;

namespace MICE.API.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMeetingRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(
            IEventRepository eventRepository,
            IMeetingRoomRepository roomRepository,
            IMapper mapper,
            ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            return eventItem != null ? _mapper.Map<EventDto>(eventItem) : null;
        }

        public async Task<EventDto> GetEventByNumberAsync(string eventNumber)
        {
            var eventItem = await _eventRepository.GetByEventNumberAsync(eventNumber);
            return eventItem != null ? _mapper.Map<EventDto>(eventItem) : null;
        }

        public async Task<List<EventDto>> GetEventsByHotelAsync(int hotelId)
        {
            var events = await _eventRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetUpcomingEventsAsync(int hotelId, int days = 7)
        {
            var events = await _eventRepository.GetUpcomingEventsAsync(hotelId, days);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
        {
            // Check room availability
            var isAvailable = await _roomRepository.CheckAvailabilityAsync(
                dto.MeetingRoomId, dto.StartDate, dto.EndDate);
            
            if (!isAvailable)
                throw new Exception("Meeting room is not available for the selected dates");

            var eventItem = _mapper.Map<Event>(dto);
            eventItem.Status = "Planned";
            eventItem.CreatedAt = DateTime.Now;
            
            await _eventRepository.AddAsync(eventItem);
            return _mapper.Map<EventDto>(eventItem);
        }

        public async Task<EventDto> UpdateEventAsync(int id, UpdateEventDto dto)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
                throw new Exception("Event not found");
            
            _mapper.Map(dto, eventItem);
            eventItem.UpdatedAt = DateTime.Now;
            
            await _eventRepository.UpdateAsync(eventItem);
            return _mapper.Map<EventDto>(eventItem);
        }

        public async Task<EventDto> UpdateEventStatusAsync(int id, string status)
        {
            var eventItem = await _eventRepository.UpdateStatusAsync(id, status);
            return _mapper.Map<EventDto>(eventItem);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventRepository.DeleteAsync(id);
        }

        public async Task<byte[]> GenerateCalendarFileAsync(int eventId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            if (eventItem == null)
                throw new Exception("Event not found");

            var room = await _roomRepository.GetByIdAsync(eventItem.MeetingRoomId);
            
            var iCalContent = GenerateICalendar(eventItem, room);
            return Encoding.UTF8.GetBytes(iCalContent);
        }

        private string GenerateICalendar(Event eventItem, MeetingRoom room)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Hotel Management System//MICE//EN");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{eventItem.EventNumber}@hotel.com");
            sb.AppendLine($"DTSTAMP:{DateTime.Now:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{eventItem.StartDate:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTEND:{eventItem.EndDate:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"SUMMARY:{EscapeText(eventItem.Name)}");
            sb.AppendLine($"DESCRIPTION:{EscapeText(eventItem.Description)}");
            sb.AppendLine($"LOCATION:{EscapeText(room?.Name ?? "Meeting Room")}");
            sb.AppendLine($"ORGANIZER:MAILTO:{eventItem.CustomerEmail}");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");
            return sb.ToString();
        }

        private string EscapeText(string text)
        {
            return text?.Replace(",", "\\,").Replace(";", "\\;").Replace("\n", "\\n") ?? "";
        }
    }
}