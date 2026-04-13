using AutoMapper;
using Housekeeping.API.Models;
using Housekeeping.API.DTOs;

namespace Housekeeping.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HousekeepingTask, TaskDto>();
            CreateMap<CreateTaskDto, HousekeepingTask>();
            CreateMap<MaintenanceIssue, IssueDto>();
            CreateMap<CreateIssueDto, MaintenanceIssue>();
        }
    }
}