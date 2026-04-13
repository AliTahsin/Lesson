import axios from 'axios';
import * as Notifications from 'expo-notifications';
import * as Device from 'expo-device';
import { Notification, CreateNotificationRequest, PushSubscriptionRequest, UnreadCount } from '../types/notification';

const API_BASE_URL = 'http://localhost:5012/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add token
api.interceptors.request.use(
  async (config) => {
    const token = await AsyncStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export const notificationApi = {
  // Get all user notifications
  getUserNotifications: async (): Promise<Notification[]> => {
    const response = await api.get('/notifications');
    return response.data;
  },

  // Get unread notifications
  getUnreadNotifications: async (): Promise<Notification[]> => {
    const response = await api.get('/notifications/unread');
    return response.data;
  },

  // Get notification by ID
  getNotificationById: async (id: number): Promise<Notification> => {
    const response = await api.get(`/notifications/${id}`);
    return response.data;
  },

  // Get unread count
  getUnreadCount: async (): Promise<UnreadCount> => {
    const response = await api.get('/notifications/unread/count');
    return response.data;
  },

  // Mark as read
  markAsRead: async (id: number): Promise<void> => {
    await api.post(`/notifications/${id}/read`);
  },

  // Mark all as read
  markAllAsRead: async (): Promise<void> => {
    await api.post('/notifications/read-all');
  },

  // Delete notification
  deleteNotification: async (id: number): Promise<void> => {
    await api.delete(`/notifications/${id}`);
  },

  // Send notification (admin only)
  sendNotification: async (data: CreateNotificationRequest): Promise<Notification> => {
    const response = await api.post('/notifications', data);
    return response.data;
  },

  // Push subscription
  subscribeToPush: async (token: string): Promise<void> => {
    const subscription: PushSubscriptionRequest = {
      endpoint: token,
      p256dh: token,
      auth: token,
      deviceType: Device.osName || 'Unknown',
      deviceName: Device.modelName || 'Unknown'
    };
    await api.post('/push/subscribe', subscription);
  },

  // Unsubscribe from push
  unsubscribeFromPush: async (endpoint: string): Promise<void> => {
    await api.post('/push/unsubscribe', { endpoint });
  }
};

export default api;