using AutoMapper;
using ReservationSystem.API.Models;
using ReservationSystem.API.DTOs;

namespace ReservationSystem.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Reservation, ReservationResponseDto>();
            CreateMap<Guest, GuestDto>();
        }
    }
}
