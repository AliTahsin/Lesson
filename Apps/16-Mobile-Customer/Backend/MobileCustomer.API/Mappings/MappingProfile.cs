using AutoMapper;
using MobileCustomer.API.Models;
using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Profile mappings
            CreateMap<CustomerProfile, ProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<UpdateProfileDto, CustomerProfile>();
            
            // Digital Key mappings
            CreateMap<DigitalKey, DigitalKeyDto>();
            
            // Room Service mappings
            CreateMap<CreateOrderItemDto, OrderItem>();
            CreateMap<CreateRoomServiceOrderDto, RoomServiceOrder>();
            CreateMap<RoomServiceOrder, RoomServiceOrderDto>();
            
            // Spa mappings
            CreateMap<CreateSpaAppointmentDto, SpaAppointment>();
            CreateMap<SpaAppointment, SpaAppointmentDto>();
        }
    }
}