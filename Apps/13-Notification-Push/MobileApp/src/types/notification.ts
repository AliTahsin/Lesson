export interface Notification {
  id: number;
  title: string;
  body: string;
  imageUrl?: string;
  type: string; // Info, Success, Warning, Error, Promo
  category: string; // Reservation, Payment, Housekeeping, Promo, System
  senderName?: string;
  isRead: boolean;
  readAt?: string;
  sentAt: string;
  createdAt: string;
  actionUrl?: string;
  relatedId?: number;
}

export interface CreateNotificationRequest {
  title: string;
  body: string;
  type: string;
  category: string;
  recipientId?: number;
  recipientType: string;
  hotelId?: number;
  actionUrl?: string;
  relatedId?: number;
  sendPush: boolean;
}

export interface PushSubscriptionRequest {
  endpoint: string;
  p256dh: string;
  auth: string;
  deviceType: string;
  deviceName: string;
}

export interface UnreadCount {
  count: number;
}