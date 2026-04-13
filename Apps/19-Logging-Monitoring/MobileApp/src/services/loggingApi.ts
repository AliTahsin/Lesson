import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import {
  LogEntry,
  LogSearchRequest,
  LogStatistics,
  MetricsSummary,
  ServiceMetrics,
  Trace,
  TraceStatistics,
  Alert
} from '../types/logging';

const API_BASE_URL = 'http://localhost:5017/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

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

export const loggingApi = {
  // Logs
  searchLogs: async (request: LogSearchRequest): Promise<LogEntry[]> => {
    const response = await api.post('/logs/search', request);
    return response.data;
  },

  getLogById: async (id: string): Promise<LogEntry> => {
    const response = await api.get(`/logs/${id}`);
    return response.data;
  },

  getLogStatistics: async (startDate: string, endDate: string): Promise<LogStatistics> => {
    const response = await api.get('/logs/statistics', { params: { startDate, endDate } });
    return response.data;
  },

  getErrorLogs: async (startDate: string, endDate: string): Promise<LogEntry[]> => {
    const response = await api.get('/logs/errors', { params: { startDate, endDate } });
    return response.data;
  },

  getLogsByService: async (service: string, startDate: string, endDate: string): Promise<LogEntry[]> => {
    const response = await api.get(`/logs/service/${service}`, { params: { startDate, endDate } });
    return response.data;
  },

  getLogCountByLevel: async (startDate: string, endDate: string): Promise<Record<string, number>> => {
    const response = await api.get('/logs/levels', { params: { startDate, endDate } });
    return response.data;
  },

  // Metrics
  getMetricsSummary: async (): Promise<MetricsSummary> => {
    const response = await api.get('/metrics/summary');
    return response.data;
  },

  getServiceMetrics: async (service: string): Promise<ServiceMetrics> => {
    const response = await api.get(`/metrics/service/${service}`);
    return response.data;
  },

  getAllServicesMetrics: async (): Promise<ServiceMetrics[]> => {
    const response = await api.get('/metrics/services');
    return response.data;
  },

  getEndpointMetrics: async (): Promise<Record<string, number>> => {
    const response = await api.get('/metrics/endpoints');
    return response.data;
  },

  // Traces
  getTraceById: async (traceId: string): Promise<Trace> => {
    const response = await api.get(`/traces/${traceId}`);
    return response.data;
  },

  getTracesByService: async (service: string, startDate: string, endDate: string): Promise<Trace[]> => {
    const response = await api.get(`/traces/service/${service}`, { params: { startDate, endDate } });
    return response.data;
  },

  getSlowTraces: async (minDurationMs: number = 1000, startDate?: string, endDate?: string): Promise<Trace[]> => {
    const params: any = { minDurationMs };
    if (startDate) params.startDate = startDate;
    if (endDate) params.endDate = endDate;
    const response = await api.get('/traces/slow', { params });
    return response.data;
  },

  getTraceStatistics: async (startDate: string, endDate: string): Promise<TraceStatistics> => {
    const response = await api.get('/traces/statistics', { params: { startDate, endDate } });
    return response.data;
  }
};