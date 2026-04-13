using AutoMapper;
using Notification.API.Models;
using Notification.API.DTOs;

namespace Notification.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<PushSubscription, SubscriptionDto>();
        }
    }
}