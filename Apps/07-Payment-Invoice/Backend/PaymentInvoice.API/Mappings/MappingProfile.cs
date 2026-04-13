using AutoMapper;
using PaymentInvoice.API.Models;
using PaymentInvoice.API.DTOs;

namespace PaymentInvoice.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Payment mappings
            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.IsSuccess, opt => opt.MapFrom(src => src.Status == "Success"))
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            // Invoice mappings
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceItem, InvoiceItemDto>();
            
            // Refund mappings
            CreateMap<Refund, RefundDto>();
        }
    }
}