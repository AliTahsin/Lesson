import axios from 'axios';
import { Restaurant, MenuItem, Table } from '../types/restaurant';

const API_BASE_URL = 'http://localhost:5008/api';

export const restaurantApi = {
  // Restaurant endpoints
  getRestaurantsByHotel: async (hotelId: number): Promise<Restaurant[]> => {
    const response = await axios.get(`${API_BASE_URL}/restaurants/hotel/${hotelId}`);
    return response.data;
  },

  getRestaurantById: async (id: number): Promise<Restaurant> => {
    const response = await axios.get(`${API_BASE_URL}/restaurants/${id}`);
    return response.data;
  },

  // Menu endpoints
  getMenusByRestaurant: async (restaurantId: number): Promise<any[]> => {
    const response = await axios.get(`${API_BASE_URL}/menus/restaurant/${restaurantId}`);
    return response.data;
  },

  getMenuItems: async (menuId: number): Promise<MenuItem[]> => {
    const response = await axios.get(`${API_BASE_URL}/menus/${menuId}`);
    return response.data.items;
  },

  // Table endpoints
  getTablesByRestaurant: async (restaurantId: number): Promise<Table[]> => {
    const response = await axios.get(`${API_BASE_URL}/restaurants/${restaurantId}/tables`);
    return response.data;
  },

  generateTableQrCode: async (tableId: number): Promise<string> => {
    const response = await axios.post(`${API_BASE_URL}/restaurants/tables/${tableId}/qrcode`);
    return response.data.qrUrl;
  }
};