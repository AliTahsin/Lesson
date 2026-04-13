using AutoMapper;
using Notification.API.Models;
using Notification.API.DTOs;
using Notification.API.Repositories;

namespace Notification.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushService _pushService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IPushService pushService,
            IMapper mapper,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _pushService = pushService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetByUserAsync(userId);
            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetUnreadByUserAsync(userId);
            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            return notification != null ? _mapper.Map<NotificationDto>(notification) : null;
        }

        public async Task<NotificationDto> SendNotificationAsync(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                Category = dto.Category,
                SenderId = dto.SenderId,
                SenderName = dto.SenderName,
                RecipientId = dto.RecipientId,
                RecipientType = dto.RecipientType,
                HotelId = dto.HotelId,
                ActionUrl = dto.ActionUrl,
                RelatedId = dto.RelatedId,
                SentAt = DateTime.Now,
                IsSent = true
            };

            var created = await _notificationRepository.AddAsync(notification);

            // Send push notification if recipient has subscription
            if (dto.RecipientId.HasValue && dto.SendPush)
            {
                await _pushService.SendToUserAsync(dto.RecipientId.Value, dto.Title, dto.Body, new
                {
                    notificationId = created.Id,
                    type = dto.Type,
                    actionUrl = dto.ActionUrl,
                    relatedId = dto.RelatedId
                });
                created.IsPushSent = true;
                await _notificationRepository.UpdateAsync(created);
            }

            return _mapper.Map<NotificationDto>(created);
        }

        public async Task<bool> MarkAsReadAsync(int id, int userId)
        {
            return await _notificationRepository.MarkAsReadAsync(id, userId);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            return await _notificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task<bool> DeleteNotificationAsync(int id, int userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification != null && (notification.RecipientId == userId || notification.RecipientType == "All"))
            {
                return await _notificationRepository.DeleteAsync(id);
            }
            return false;
        }

        public async Task<NotificationDto> SendReservationNotificationAsync(int userId, int reservationId, string status)
        {
            string title, body, type;
            
            switch (status)
            {
                case "Confirmed":
                    title = "Rezervasyon Onaylandı";
                    body = $"Rezervasyonunuz #{reservationId} onaylandı.";
                    type = "Success";
                    break;
                case "Cancelled":
                    title = "Rezervasyon İptal Edildi";
                    body = $"Rezervasyonunuz #{reservationId} iptal edildi.";
                    type = "Error";
                    break;
                case "Modified":
                    title = "Rezervasyon Güncellendi";
                    body = $"Rezervasyonunuz #{reservationId} güncellendi.";
                    type = "Info";
                    break;
                default:
                    title = "Rezervasyon Durumu";
                    body = $"Rezervasyonunuz #{reservationId} durumu: {status}";
                    type = "Info";
                    break;
            }

            return await SendNotificationAsync(new CreateNotificationDto
            {
                Title = title,
                Body = body,
                Type = type,
                Category = "Reservation",
                RecipientId = userId,
                RecipientType = "User",
                ActionUrl = $"/reservations/{reservationId}",
                RelatedId = reservationId,
                SendPush = true
            });
        }

        public async Task<NotificationDto> SendPaymentNotificationAsync(int userId, int paymentId, string status)
        {
            string title, body, type;
            
            switch (status)
            {
                case "Success":
                    title = "Ödeme Başarılı";
                    body = $"Ödemeniz #{paymentId} başarıyla tamamlandı.";
                    type = "Success";
                    break;
                case "Failed":
                    title = "Ödeme Başarısız";
                    body = $"Ödemeniz #{paymentId} başarısız oldu. Lütfen tekrar deneyin.";
                    type = "Error";
                    break;
                case "Refunded":
                    title = "İade İşlemi";
                    body = $"Ödemeniz #{paymentId} iade edildi.";
                    type = "Warning";
                    break;
                default:
                    title = "Ödeme Durumu";
                    body = $"Ödemeniz #{paymentId} durumu: {status}";
                    type = "Info";
                    break;
            }

            return await SendNotificationAsync(new CreateNotificationDto
            {
                Title = title,
                Body = body,
                Type = type,
                Category = "Payment",
                RecipientId = userId,
                RecipientType = "User",
                ActionUrl = $"/payments/{paymentId}",
                RelatedId = paymentId,
                SendPush = true
            });
        }

        public async Task<NotificationDto> SendPromoNotificationAsync(int hotelId, string title, string message)
        {
            return await SendNotificationAsync(new CreateNotificationDto
            {
                Title = title,
                Body = message,
                Type = "Promo",
                Category = "Promo",
                HotelId = hotelId,
                RecipientType = "Hotel",
                ActionUrl = "/promotions",
                SendPush = true
            });
        }
    }
}