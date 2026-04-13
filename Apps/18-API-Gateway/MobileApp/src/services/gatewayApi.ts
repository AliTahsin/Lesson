import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import {
  Service,
  ServiceHealthCheck,
  RouteInfo,
  GatewayInfo,
  RequestStats,
  RateLimitInfo
} from '../types/gateway';

const API_BASE_URL = 'http://localhost:8080/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
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

export const gatewayApi = {
  // Get all services
  getServices: async (): Promise<Service[]> => {
    const response = await api.get('/gateway/services');
    return response.data;
  },

  // Get service health
  getServiceHealth: async (): Promise<ServiceHealthCheck[]> => {
    const response = await api.get('/gateway/services/health');
    return response.data;
  },

  // Get gateway info
  getGatewayInfo: async (): Promise<GatewayInfo> => {
    const response = await api.get('/gateway/info');
    return response.data;
  },

  // Get routes
  getRoutes: async (): Promise<RouteInfo[]> => {
    const response = await api.get('/gateway/routes');
    return response.data;
  },

  // Register service
  registerService: async (service: Partial<Service>): Promise<void> => {
    await api.post('/gateway/services/register', service);
  },

  // Deregister service
  deregisterService: async (serviceName: string): Promise<void> => {
    await api.delete(`/gateway/services/${serviceName}`);
  },

  // Send heartbeat
  sendHeartbeat: async (serviceName: string): Promise<void> => {
    await api.post(`/gateway/services/${serviceName}/heartbeat`);
  },

  // Get request stats (mock)
  getRequestStats: async (): Promise<RequestStats> => {
    // Mock data for demonstration
    return {
      totalRequests: 12543,
      averageResponseTime: 245,
      errorRate: 2.3,
      requestsByMinute: Array.from({ length: 10 }, (_, i) => ({
        minute: `${i * 5}dk`,
        count: Math.floor(Math.random() * 500) + 100
      }))
    };
  },

  // Check rate limit status (mock)
  getRateLimitStatus: async (): Promise<RateLimitInfo> => {
    return {
      isLimited: false,
      remaining: 987,
      resetAt: new Date(Date.now() + 60000).toISOString(),
      limit: 1000
    };
  }
};