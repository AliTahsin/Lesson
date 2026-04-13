import axios from 'axios';
import { Channel, ChannelConnection, ChannelBooking, DashboardStats, SyncLog } from '../types/channel';

const API_BASE_URL = 'http://localhost:5005/api';

export const channelApi = {
  // Channels
  getAllChannels: async (): Promise<Channel[]> => {
    const response = await axios.get(`${API_BASE_URL}/channels`);
    return response.data;
  },

  getChannelById: async (id: number): Promise<Channel> => {
    const response = await axios.get(`${API_BASE_URL}/channels/${id}`);
    return response.data;
  },

  // Connections
  getHotelConnections: async (hotelId: number): Promise<ChannelConnection[]> => {
    const response = await axios.get(`${API_BASE_URL}/channelconnections/hotel/${hotelId}`);
    return response.data;
  },

  connectChannel: async (data: { channelId: number; hotelId: number; autoSync: boolean }): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/channelconnections`, data);
    return response.data;
  },

  disconnectChannel: async (connectionId: number): Promise<any> => {
    const response = await axios.delete(`${API_BASE_URL}/channelconnections/${connectionId}`);
    return response.data;
  },

  // Sync
  syncAvailability: async (data: { channelId: number; hotelId: number; roomIds?: number[] }): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/sync/availability`, data);
    return response.data;
  },

  syncPrices: async (data: { channelId: number; hotelId: number; roomIds?: number[]; priceMultiplier?: number }): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/sync/prices`, data);
    return response.data;
  },

  // Bookings
  getBookings: async (channelId?: number, hotelId?: number): Promise<ChannelBooking[]> => {
    const params = new URLSearchParams();
    if (channelId) params.append('channelId', channelId.toString());
    if (hotelId) params.append('hotelId', hotelId.toString());
    const response = await axios.get(`${API_BASE_URL}/sync/bookings`, { params });
    return response.data;
  },

  // Dashboard
  getDashboardStats: async (): Promise<DashboardStats> => {
    const response = await axios.get(`${API_BASE_URL}/sync/dashboard`);
    return response.data;
  },

  // Sync Logs
  getSyncLogs: async (channelId: number, hotelId?: number): Promise<SyncLog[]> => {
    const params = new URLSearchParams();
    if (hotelId) params.append('hotelId', hotelId.toString());
    const response = await axios.get(`${API_BASE_URL}/sync/logs/${channelId}`, { params });
    return response.data;
  }
};