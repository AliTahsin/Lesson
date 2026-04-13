import axios from 'axios';
import { Attendee, AttendeeStats } from '../types/mice';

const API_BASE_URL = 'http://localhost:5009/api';

export const attendeeApi = {
  // Get attendees by event
  getAttendeesByEvent: async (eventId: number): Promise<Attendee[]> => {
    const response = await axios.get(`${API_BASE_URL}/attendees/event/${eventId}`);
    return response.data;
  },

  // Get attendee by ID
  getAttendeeById: async (id: number): Promise<Attendee> => {
    const response = await axios.get(`${API_BASE_URL}/attendees/${id}`);
    return response.data;
  },

  // Register attendee
  registerAttendee: async (data: any): Promise<Attendee> => {
    const response = await axios.post(`${API_BASE_URL}/attendees`, data);
    return response.data;
  },

  // Check-in attendee
  checkInAttendee: async (attendeeId: number): Promise<Attendee> => {
    const response = await axios.post(`${API_BASE_URL}/attendees/${attendeeId}/checkin`);
    return response.data;
  },

  // Check-in by QR code
  checkInByQrCode: async (qrCode: string): Promise<Attendee> => {
    const response = await axios.post(`${API_BASE_URL}/attendees/checkin/qrcode`, null, {
      params: { qrCode }
    });
    return response.data;
  },

  // Delete attendee
  deleteAttendee: async (id: number): Promise<void> => {
    await axios.delete(`${API_BASE_URL}/attendees/${id}`);
  },

  // Get attendee statistics
  getAttendeeStats: async (eventId: number): Promise<AttendeeStats> => {
    const response = await axios.get(`${API_BASE_URL}/attendees/event/${eventId}/statistics`);
    return response.data;
  }
};