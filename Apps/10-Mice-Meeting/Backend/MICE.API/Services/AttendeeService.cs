using AutoMapper;
using MICE.API.Models;
using MICE.API.DTOs;
using MICE.API.Repositories;

namespace MICE.API.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendeeService> _logger;

        public AttendeeService(
            IAttendeeRepository attendeeRepository,
            IEventRepository eventRepository,
            IMapper mapper,
            ILogger<AttendeeService> logger)
        {
            _attendeeRepository = attendeeRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AttendeeDto> GetAttendeeByIdAsync(int id)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            return attendee != null ? _mapper.Map<AttendeeDto>(attendee) : null;
        }

        public async Task<List<AttendeeDto>> GetAttendeesByEventAsync(int eventId)
        {
            var attendees = await _attendeeRepository.GetByEventAsync(eventId);
            return _mapper.Map<List<AttendeeDto>>(attendees);
        }

        public async Task<AttendeeDto> RegisterAttendeeAsync(CreateAttendeeDto dto)
        {
            var attendee = _mapper.Map<EventAttendee>(dto);
            attendee.Status = "Registered";
            attendee.RegisteredAt = DateTime.Now;
            
            await _attendeeRepository.AddAsync(attendee);
            
            // Update event expected attendees count
            var eventItem = await _eventRepository.GetByIdAsync(dto.EventId);
            if (eventItem != null)
            {
                eventItem.ExpectedAttendees++;
                await _eventRepository.UpdateAsync(eventItem);
            }
            
            return _mapper.Map<AttendeeDto>(attendee);
        }

        public async Task<AttendeeDto> UpdateAttendeeAsync(int id, UpdateAttendeeDto dto)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            if (attendee == null)
                throw new Exception("Attendee not found");
            
            _mapper.Map(dto, attendee);
            await _attendeeRepository.UpdateAsync(attendee);
            
            return _mapper.Map<AttendeeDto>(attendee);
        }

        public async Task<AttendeeDto> CheckInAsync(int attendeeId)
        {
            var attendee = await _attendeeRepository.CheckInAsync(attendeeId);
            
            // Update event actual attendees count
            var eventItem = await _eventRepository.GetByIdAsync(attendee.EventId);
            if (eventItem != null)
            {
                eventItem.ActualAttendees = await _attendeeRepository.GetAttendeeCountAsync(eventItem.Id);
                await _eventRepository.UpdateAsync(eventItem);
            }
            
            return _mapper.Map<AttendeeDto>(attendee);
        }

        public async Task<AttendeeDto> CheckInByQrCodeAsync(string qrCode)
        {
            var attendee = await _attendeeRepository.GetByQrCodeAsync(qrCode);
            if (attendee == null)
                throw new Exception("Invalid QR code");
            
            if (attendee.HasCheckedIn)
                throw new Exception("Attendee already checked in");
            
            return await CheckInAsync(attendee.Id);
        }

        public async Task<bool> DeleteAttendeeAsync(int id)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            if (attendee != null)
            {
                // Update event expected attendees count
                var eventItem = await _eventRepository.GetByIdAsync(attendee.EventId);
                if (eventItem != null)
                {
                    eventItem.ExpectedAttendees--;
                    await _eventRepository.UpdateAsync(eventItem);
                }
            }
            return await _attendeeRepository.DeleteAsync(id);
        }

        public async Task<AttendeeStatisticsDto> GetAttendeeStatisticsAsync(int eventId)
        {
            var attendees = await _attendeeRepository.GetByEventAsync(eventId);
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            
            return new AttendeeStatisticsDto
            {
                EventId = eventId,
                EventName = eventItem?.Name,
                TotalRegistered = attendees.Count,
                CheckedIn = attendees.Count(a => a.HasCheckedIn),
                CheckedInRate = attendees.Any() ? (decimal)attendees.Count(a => a.HasCheckedIn) / attendees.Count * 100 : 0,
                NoShow = attendees.Count(a => !a.HasCheckedIn && a.Status != "Cancelled"),
                ByCompany = attendees.GroupBy(a => a.Company)
                    .Where(g => !string.IsNullOrEmpty(g.Key))
                    .Select(g => new CompanyStatDto
                    {
                        Company = g.Key,
                        Count = g.Count(),
                        CheckedIn = g.Count(a => a.HasCheckedIn)
                    }).ToList()
            };
        }
    }
}