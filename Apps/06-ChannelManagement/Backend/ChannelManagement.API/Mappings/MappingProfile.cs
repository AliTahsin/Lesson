using AutoMapper;
using ChannelManagement.API.Models;
using ChannelManagement.API.DTOs;

namespace ChannelManagement.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Channel, ChannelDto>();
            CreateMap<ChannelConnection, ChannelConnectionDto>();
        }
    }
}
