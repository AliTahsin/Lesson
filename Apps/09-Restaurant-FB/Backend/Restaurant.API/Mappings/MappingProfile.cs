using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.DTOs;

namespace Restaurant.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Restaurant mappings
            CreateMap<Restaurant, RestaurantDto>();
            CreateMap<CreateRestaurantDto, Restaurant>();
            CreateMap<UpdateRestaurantDto, Restaurant>();
            
            // Table mappings
            CreateMap<Table, TableDto>();
            
            // Menu mappings
            CreateMap<Menu, MenuDto>();
            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemDto, MenuItem>();
            
            // Order mappings
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();
            
            // Reservation mappings
            CreateMap<TableReservation, ReservationDto>();
            CreateMap<CreateReservationDto, TableReservation>();
            
            // Stock mappings
            CreateMap<StockItem, StockItemDto>();
            CreateMap<CreateStockItemDto, StockItem>();
            CreateMap<UpdateStockItemDto, StockItem>();
        }
    }
}