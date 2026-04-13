import axios from 'axios';
import { Room, RoomType, RoomInventory, RoomStats } from '../types/room';

const API_BASE_URL = 'http://localhost:5001/api';

export const roomApi = {
  // Room endpoints
  getAllRooms: async (): Promise<Room[]> => {
    const response = await axios.get(`${API_BASE_URL}/rooms`);
    return response.data;
  },

  getRoomById: async (id: number): Promise<Room> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/${id}`);
    return response.data;
  },

  getRoomsByHotel: async (hotelId: number): Promise<Room[]> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/hotel/${hotelId}`);
    return response.data;
  },

  getRoomsByType: async (roomTypeId: number): Promise<Room[]> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/type/${roomTypeId}`);
    return response.data;
  },

  getAvailableRooms: async (hotelId: number, checkIn: string, checkOut: string): Promise<Room[]> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/available`, {
      params: { hotelId, checkIn, checkOut }
    });
    return response.data;
  },

  searchRooms: async (params: {
    hotelId?: number;
    roomTypeId?: number;
    capacity?: number;
    view?: string;
    minPrice?: number;
    maxPrice?: number;
  }): Promise<Room[]> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/search`, { params });
    return response.data;
  },

  getRoomStats: async (hotelId: number): Promise<RoomStats> => {
    const response = await axios.get(`${API_BASE_URL}/rooms/stats/${hotelId}`);
    return response.data;
  },

  updateRoomStatus: async (id: number, status: string): Promise<any> => {
    const response = await axios.patch(`${API_BASE_URL}/rooms/${id}/status`, null, {
      params: { status }
    });
    return response.data;
  },

  // RoomType endpoints
  getAllRoomTypes: async (): Promise<RoomType[]> => {
    const response = await axios.get(`${API_BASE_URL}/roomtypes`);
    return response.data;
  },

  getRoomTypeById: async (id: number): Promise<RoomType> => {
    const response = await axios.get(`${API_BASE_URL}/roomtypes/${id}`);
    return response.data;
  }
};