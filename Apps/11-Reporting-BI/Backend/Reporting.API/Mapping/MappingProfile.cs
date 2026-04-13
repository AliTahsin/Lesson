using AutoMapper;
using Reporting.API.Models;
using Reporting.API.DTOs;

namespace Reporting.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Report, ReportDto>();
            CreateMap<Dashboard, DashboardDto>();
            CreateMap<KPI, KPIDto>();
        }
    }
}