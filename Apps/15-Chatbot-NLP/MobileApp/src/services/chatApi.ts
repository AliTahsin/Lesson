import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { ChatMessage, Conversation, SendMessageRequest, StartConversationRequest, ChatStatistics } from '../types/chat';

const API_BASE_URL = 'http://localhost:5014/api';

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

export const chatApi = {
  // Start a new conversation
  startConversation: async (hotelId?: number): Promise<Conversation> => {
    const response = await api.post('/chat/start', null, { params: { hotelId } });
    return response.data;
  },

  // Get active conversation
  getActiveConversation: async (): Promise<Conversation | null> => {
    const response = await api.get('/chat/active');
    return response.data;
  },

  // Get conversation messages
  getConversationMessages: async (conversationId: number): Promise<ChatMessage[]> => {
    const response = await api.get(`/chat/${conversationId}/messages`);
    return response.data;
  },

  // Send message
  sendMessage: async (conversationId: number, message: string): Promise<ChatMessage> => {
    const response = await api.post(`/chat/${conversationId}/send`, { message });
    return response.data;
  },

  // End conversation
  endConversation: async (conversationId: number): Promise<void> => {
    await api.post(`/chat/${conversationId}/end`);
  },

  // Get chat statistics (admin only)
  getChatStatistics: async (startDate?: string, endDate?: string): Promise<ChatStatistics> => {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    const response = await api.get('/chat/statistics', { params });
    return response.data;
  },

  // Get user conversations (admin only)
  getUserConversations: async (userId: number): Promise<Conversation[]> => {
    const response = await api.get(`/conversation/user/${userId}`);
    return response.data;
  }
};