using AutoMapper;
using MICE.API.Models;
using MICE.API.DTOs;

namespace MICE.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Meeting Room mappings
            CreateMap<MeetingRoom, MeetingRoomDto>();
            CreateMap<CreateMeetingRoomDto, MeetingRoom>();
            CreateMap<UpdateMeetingRoomDto, MeetingRoom>();
            
            // Event mappings
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.MeetingRoomName, opt => opt.Ignore());
            CreateMap<CreateEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();
            CreateMap<EventSchedule, EventScheduleDto>();
            CreateMap<CreateEventScheduleDto, EventSchedule>();
            
            // Attendee mappings
            CreateMap<EventAttendee, AttendeeDto>();
            CreateMap<CreateAttendeeDto, EventAttendee>();
            CreateMap<UpdateAttendeeDto, EventAttendee>();
            
            // Equipment mappings
            CreateMap<Equipment, EquipmentDto>();
            CreateMap<CreateEquipmentDto, Equipment>();
            CreateMap<UpdateEquipmentDto, Equipment>();
        }
    }
}