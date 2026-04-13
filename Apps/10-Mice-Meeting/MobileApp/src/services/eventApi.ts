import axios from 'axios';
import { Event, CreateEventData } from '../types/mice';

const API_BASE_URL = 'http://localhost:5009/api';

export const eventApi = {
  // Get all events by hotel
  getEventsByHotel: async (hotelId: number): Promise<Event[]> => {
    const response = await axios.get(`${API_BASE_URL}/events/hotel/${hotelId}`);
    return response.data;
  },

  // Get event by ID
  getEventById: async (id: number): Promise<Event> => {
    const response = await axios.get(`${API_BASE_URL}/events/${id}`);
    return response.data;
  },

  // Get event by number
  getEventByNumber: async (eventNumber: string): Promise<Event> => {
    const response = await axios.get(`${API_BASE_URL}/events/number/${eventNumber}`);
    return response.data;
  },

  // Get upcoming events
  getUpcomingEvents: async (hotelId: number, days: number = 7): Promise<Event[]> => {
    const response = await axios.get(`${API_BASE_URL}/events/hotel/${hotelId}/upcoming`, {
      params: { days }
    });
    return response.data;
  },

  // Create event
  createEvent: async (data: CreateEventData): Promise<Event> => {
    const response = await axios.post(`${API_BASE_URL}/events`, data);
    return response.data;
  },

  // Update event status
  updateEventStatus: async (id: number, status: string): Promise<Event> => {
    const response = await axios.patch(`${API_BASE_URL}/events/${id}/status`, null, {
      params: { status }
    });
    return response.data;
  },

  // Delete event
  deleteEvent: async (id: number): Promise<void> => {
    await axios.delete(`${API_BASE_URL}/events/${id}`);
  },

  // Download calendar file
  downloadCalendar: async (eventId: number): Promise<string> => {
    const response = await axios.get(`${API_BASE_URL}/events/${eventId}/calendar`, {
      responseType: 'blob'
    });
    return URL.createObjectURL(response.data);
  }
};