import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import {
  Recommendation,
  TrackInteractionRequest,
  DemandPrediction,
  RevenuePrediction,
  OccupancyPrediction,
  SentimentResult,
  SentimentSummary,
  RecommendationMetrics
} from '../types/ai';

const API_BASE_URL = 'http://localhost:5013/api';

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

export const aiApi = {
  // Recommendation endpoints
  getPersonalizedRecommendations: async (count: number = 10): Promise<Recommendation[]> => {
    const response = await api.get('/recommendations/personalized', { params: { count } });
    return response.data;
  },

  getSimilarItems: async (itemId: number, itemType: string, count: number = 10): Promise<Recommendation[]> => {
    const response = await api.get(`/recommendations/similar/${itemId}`, { params: { itemType, count } });
    return response.data;
  },

  getPopularItems: async (itemType: string, count: number = 10): Promise<Recommendation[]> => {
    const response = await api.get('/recommendations/popular', { params: { itemType, count } });
    return response.data;
  },

  trackInteraction: async (data: TrackInteractionRequest): Promise<any> => {
    const response = await api.post('/recommendations/track', data);
    return response.data;
  },

  trackClick: async (recommendationId: number): Promise<any> => {
    const response = await api.post(`/recommendations/${recommendationId}/click`);
    return response.data;
  },

  trackBooking: async (recommendationId: number): Promise<any> => {
    const response = await api.post(`/recommendations/${recommendationId}/book`);
    return response.data;
  },

  getRecommendationMetrics: async (): Promise<RecommendationMetrics> => {
    const response = await api.get('/recommendations/metrics');
    return response.data;
  },

  // Prediction endpoints
  predictDemand: async (hotelId: number, date: string): Promise<DemandPrediction> => {
    const response = await api.get('/predictions/demand', { params: { hotelId, date } });
    return response.data;
  },

  predictRevenue: async (hotelId: number, startDate: string, endDate: string): Promise<RevenuePrediction> => {
    const response = await api.get('/predictions/revenue', { params: { hotelId, startDate, endDate } });
    return response.data;
  },

  predictOccupancy: async (hotelId: number, date: string): Promise<OccupancyPrediction> => {
    const response = await api.get('/predictions/occupancy', { params: { hotelId, date } });
    return response.data;
  },

  // Sentiment endpoints
  analyzeSentiment: async (text: string): Promise<SentimentResult> => {
    const response = await api.post('/sentiment/analyze', { text });
    return response.data;
  },

  getSentimentSummary: async (itemId: number, itemType: string): Promise<SentimentSummary[]> => {
    const response = await api.get('/sentiment/summary', { params: { itemId, itemType } });
    return response.data;
  }
};