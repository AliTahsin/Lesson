using AutoMapper;
using Logging.API.Models;
using Logging.API.DTOs;

namespace Logging.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LogEntry, LogDto>();
            CreateMap<Trace, TraceDto>();
            CreateMap<Metric, MetricDto>();
        }
    }
}