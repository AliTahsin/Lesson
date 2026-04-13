import axios from 'axios';
import { 
  Reservation, 
  CreateReservationRequest, 
  CheckInRequest, 
  CheckOutRequest,
  CheckResponse,
  ReservationStats 
} from '../types/reservation';

const API_BASE_URL = 'http://localhost:5002/api';

export const reservationApi = {
  // Get all reservations
  getAllReservations: async (): Promise<Reservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations`);
    return response.data;
  },

  // Get reservation by ID
  getReservationById: async (id: number): Promise<Reservation> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/${id}`);
    return response.data;
  },

  // Get reservation by number
  getReservationByNumber: async (reservationNumber: string): Promise<Reservation> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/number/${reservationNumber}`);
    return response.data;
  },

  // Get reservations by guest email
  getReservationsByGuestEmail: async (email: string): Promise<Reservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/guest/${email}`);
    return response.data;
  },

  // Get today's arrivals
  getTodayArrivals: async (): Promise<Reservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/arrivals/today`);
    return response.data;
  },

  // Get today's departures
  getTodayDepartures: async (): Promise<Reservation[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/departures/today`);
    return response.data;
  },

  // Get statistics
  getStatistics: async (startDate?: string, endDate?: string): Promise<ReservationStats> => {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    const response = await axios.get(`${API_BASE_URL}/reservations/stats`, { params });
    return response.data;
  },

  // Get reservation history
  getReservationHistory: async (reservationId: number): Promise<any[]> => {
    const response = await axios.get(`${API_BASE_URL}/reservations/history/${reservationId}`);
    return response.data;
  },

  // Create reservation
  createReservation: async (request: CreateReservationRequest): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/reservations`, request);
    return response.data;
  },

  // Update reservation
  updateReservation: async (id: number, data: any): Promise<any> => {
    const response = await axios.put(`${API_BASE_URL}/reservations/${id}`, data);
    return response.data;
  },

  // Cancel reservation
  cancelReservation: async (id: number, reason: string): Promise<any> => {
    const response = await axios.delete(`${API_BASE_URL}/reservations/${id}`, {
      params: { reason }
    });
    return response.data;
  },

  // Check-in
  checkIn: async (request: CheckInRequest): Promise<CheckResponse> => {
    const response = await axios.post(`${API_BASE_URL}/check/checkin`, request);
    return response.data;
  },

  // Check-out
  checkOut: async (request: CheckOutRequest): Promise<CheckResponse> => {
    const response = await axios.post(`${API_BASE_URL}/check/checkout`, request);
    return response.data;
  },

  // Check availability
  checkAvailability: async (roomId: number, checkIn: string, checkOut: string): Promise<any> => {
    const response = await axios.get(`${API_BASE_URL}/check/availability`, {
      params: { roomId, checkIn, checkOut }
    });
    return response.data;
  }
};