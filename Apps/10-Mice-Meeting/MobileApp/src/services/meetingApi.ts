import axios from 'axios';
import { MeetingRoom } from '../types/mice';

const API_BASE_URL = 'http://localhost:5009/api';

export const meetingApi = {
  // Get all meeting rooms by hotel
  getRoomsByHotel: async (hotelId: number): Promise<MeetingRoom[]> => {
    const response = await axios.get(`${API_BASE_URL}/meetingrooms/hotel/${hotelId}`);
    return response.data;
  },

  // Get meeting room by ID
  getRoomById: async (id: number): Promise<MeetingRoom> => {
    const response = await axios.get(`${API_BASE_URL}/meetingrooms/${id}`);
    return response.data;
  },

  // Get available rooms
  getAvailableRooms: async (startDate: string, endDate: string, capacity: number): Promise<MeetingRoom[]> => {
    const response = await axios.get(`${API_BASE_URL}/meetingrooms/available`, {
      params: { startDate, endDate, capacity }
    });
    return response.data;
  },

  // Check room availability
  checkAvailability: async (roomId: number, startDate: string, endDate: string): Promise<boolean> => {
    const response = await axios.get(`${API_BASE_URL}/meetingrooms/${roomId}/availability`, {
      params: { startDate, endDate }
    });
    return response.data.isAvailable;
  }
};