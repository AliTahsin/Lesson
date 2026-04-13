using AutoMapper;
using Auth.API.Models;
using Auth.API.DTOs;

namespace Auth.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());
            
            CreateMap<RegisterDto, User>();
            CreateMap<UpdateUserDto, User>();
            
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());
            
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();
            
            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>();
        }
    }
}