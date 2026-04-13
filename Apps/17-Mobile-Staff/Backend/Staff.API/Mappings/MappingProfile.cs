using AutoMapper;
using Staff.API.Models;
using Staff.API.DTOs;

namespace Staff.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<UpdateStaffDto, Staff>();
            
            CreateMap<StaffTask, TaskDto>();
            CreateMap<CreateTaskDto, StaffTask>();
            
            CreateMap<MaintenanceIssue, IssueDto>();
            CreateMap<CreateIssueDto, MaintenanceIssue>();
            
            CreateMap<CheckInOut, CheckDto>();
        }
    }
}