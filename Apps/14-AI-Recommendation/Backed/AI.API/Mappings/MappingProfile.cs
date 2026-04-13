using AutoMapper;
using AI.API.Models;
using AI.API.DTOs;

namespace AI.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserInteraction, TrackInteractionDto>();
            CreateMap<Recommendation, RecommendationDto>();
        }
    }
}