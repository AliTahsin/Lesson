import axios from 'axios';
import { TableReservation, Table } from '../types/restaurant';

const API_BASE_URL = 'http://localhost:5008/api';

export const reservationApi = {
  // Get reservations by restaurant
  getReservationsByRestaurant: async (restaurantId: number): Promise<TableReservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/restaurant/${restaurantId}`);
    return response.data;
  },

  // Get reservations by date
  getReservationsByDate: async (restaurantId: number, date: string): Promise<TableReservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/restaurant/${restaurantId}/date`, {
      params: { date }
    });
    return response.data;
  },

  // Create reservation
  createReservation: async (data: any): Promise<TableReservation> => {
    const response = await axios.post(`${API_BASE_URL}/reservations`, data);
    return response.data;
  },

  // Confirm reservation
  confirmReservation: async (id: number): Promise<TableReservation> => {
    const response = await axios.post(`${API_BASE_URL}/reservations/${id}/confirm`);
    return response.data;
  },

  // Cancel reservation
  cancelReservation: async (id: number, reason: string): Promise<TableReservation> => {
    const response = await axios.post(`${API_BASE_URL}/reservations/${id}/cancel`, null, {
      params: { reason }
    });
    return response.data;
  },

  // Get available tables
  getAvailableTables: async (restaurantId: number, date: string, time: string, guestCount: number): Promise<Table[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/available-tables`, {
      params: { restaurantId, date, time, guestCount }
    });
    return response.data;
  }
};