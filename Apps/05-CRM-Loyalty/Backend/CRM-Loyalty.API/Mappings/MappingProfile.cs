using AutoMapper;
using CRM_Loyalty.API.Models;
using CRM_Loyalty.API.DTOs;

namespace CRM_Loyalty.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<LoyaltyTransaction, LoyaltyTransactionDto>();
        }
    }
}
