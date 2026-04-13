import axios from 'axios';
import { storage } from '../utils/storage';
import {
  CustomerProfile,
  UpdateProfileRequest,
  NotificationSettings,
  DigitalKey,
  MenuItem,
  RoomServiceOrder,
  SpaService,
  SpaAppointment,
  Language
} from '../types/customer';

const API_BASE_URL = 'http://localhost:5015/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
api.interceptors.request.use(
  async (config) => {
    const token = await storage.getAccessToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshToken = await storage.getRefreshToken();
        const response = await axios.post(`${API_BASE_URL}/auth/refresh`, { refreshToken });
        const { accessToken, refreshToken: newRefreshToken } = response.data;
        await storage.setTokens(accessToken, newRefreshToken);
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        await storage.clearTokens();
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);

export const customerApi = {
  // Profile endpoints
  getProfile: async (): Promise<CustomerProfile> => {
    const response = await api.get('/profile');
    return response.data;
  },

  updateProfile: async (data: UpdateProfileRequest): Promise<CustomerProfile> => {
    const response = await api.put('/profile', data);
    return response.data;
  },

  updateBiometric: async (enabled: boolean, biometricKey?: string): Promise<void> => {
    await api.put('/profile/biometric', { enabled, biometricKey });
  },

  updateLanguage: async (language: string): Promise<void> => {
    await api.put('/profile/language', { language });
  },

  updateNotificationSettings: async (settings: NotificationSettings): Promise<void> => {
    await api.put('/profile/notifications', settings);
  },

  // Digital Key endpoints
  getDigitalKeys: async (): Promise<DigitalKey[]> => {
    const response = await api.get('/digitallkey/active');
    return response.data;
  },

  generateDigitalKey: async (reservationId: number): Promise<DigitalKey> => {
    const response = await api.post(`/digitallkey/generate/${reservationId}`);
    return response.data;
  },

  getDigitalKeyByReservation: async (reservationId: number): Promise<DigitalKey> => {
    const response = await api.get(`/digitallkey/reservation/${reservationId}`);
    return response.data;
  },

  validateDigitalKey: async (keyCode: string, roomId: number): Promise<boolean> => {
    const response = await api.post('/digitallkey/validate', { keyCode, roomId });
    return response.data.isValid;
  },

  // Room Service endpoints
  getMenu: async (): Promise<MenuItem[]> => {
    const response = await api.get('/roomservice/menu');
    return response.data;
  },

  getMenuByCategory: async (category: string): Promise<MenuItem[]> => {
    const response = await api.get(`/roomservice/menu/${category}`);
    return response.data;
  },

  getCategories: async (): Promise<string[]> => {
    const response = await api.get('/roomservice/categories');
    return response.data;
  },

  createOrder: async (orderData: any): Promise<RoomServiceOrder> => {
    const response = await api.post('/roomservice/order', orderData);
    return response.data;
  },

  getMyOrders: async (): Promise<RoomServiceOrder[]> => {
    const response = await api.get('/roomservice/orders');
    return response.data;
  },

  getOrderById: async (orderId: number): Promise<RoomServiceOrder> => {
    const response = await api.get(`/roomservice/orders/${orderId}`);
    return response.data;
  },

  cancelOrder: async (orderId: number): Promise<void> => {
    await api.post(`/roomservice/orders/${orderId}/cancel`);
  },

  trackOrder: async (orderId: number): Promise<any> => {
    const response = await api.get(`/roomservice/orders/track/${orderId}`);
    return response.data;
  },

  // Spa endpoints
  getSpaServices: async (): Promise<SpaService[]> => {
    const response = await api.get('/spa/services');
    return response.data;
  },

  getSpaServiceById: async (id: number): Promise<SpaService> => {
    const response = await api.get(`/spa/services/${id}`);
    return response.data;
  },

  getAvailableTimes: async (date: string, serviceId: number): Promise<string[]> => {
    const response = await api.get('/spa/available-times', { params: { date, serviceId } });
    return response.data;
  },

  createAppointment: async (data: any): Promise<SpaAppointment> => {
    const response = await api.post('/spa/appointments', data);
    return response.data;
  },

  getMyAppointments: async (): Promise<SpaAppointment[]> => {
    const response = await api.get('/spa/appointments');
    return response.data;
  },

  cancelAppointment: async (appointmentId: number): Promise<void> => {
    await api.post(`/spa/appointments/${appointmentId}/cancel`);
  },

  // Language endpoints
  getSupportedLanguages: async (): Promise<Language[]> => {
    const response = await api.get('/language/supported');
    return response.data;
  },

  getCurrentLanguage: async (): Promise<string> => {
    const response = await api.get('/language/current');
    return response.data.language;
  },

  setLanguage: async (languageCode: string): Promise<void> => {
    await api.put('/language/current', { languageCode });
  },

  getTranslations: async (languageCode: string): Promise<Record<string, string>> => {
    const response = await api.get(`/language/translations/${languageCode}`);
    return response.data;
  },
};