import axios from 'axios';
import { Order, CreateOrderDto, DailyRevenue, PopularItems } from '../types/restaurant';

const API_BASE_URL = 'http://localhost:5008/api';

export const orderApi = {
  // Get order by ID
  getOrderById: async (id: number): Promise<Order> => {
    const response = await axios.get(`${API_BASE_URL}/orders/${id}`);
    return response.data;
  },

  // Get order by number
  getOrderByNumber: async (orderNumber: string): Promise<Order> => {
    const response = await axios.get(`${API_BASE_URL}/orders/number/${orderNumber}`);
    return response.data;
  },

  // Get orders by restaurant
  getOrdersByRestaurant: async (restaurantId: number): Promise<Order[]> => {
    const response = await axios.get(`${API_BASE_URL}/orders/restaurant/${restaurantId}`);
    return response.data;
  },

  // Get pending orders
  getPendingOrders: async (restaurantId: number): Promise<Order[]> => {
    const response = await axios.get(`${API_BASE_URL}/orders/restaurant/${restaurantId}/pending`);
    return response.data;
  },

  // Create order
  createOrder: async (data: CreateOrderDto): Promise<Order> => {
    const response = await axios.post(`${API_BASE_URL}/orders`, data);
    return response.data;
  },

  // Update order status
  updateOrderStatus: async (orderId: number, status: string): Promise<Order> => {
    const response = await axios.patch(`${API_BASE_URL}/orders/${orderId}/status`, null, {
      params: { status }
    });
    return response.data;
  },

  // Update order item status
  updateOrderItemStatus: async (orderItemId: number, status: string): Promise<Order> => {
    const response = await axios.patch(`${API_BASE_URL}/orders/items/${orderItemId}/status`, null, {
      params: { status }
    });
    return response.data;
  },

  // Cancel order
  cancelOrder: async (orderId: number, reason: string): Promise<Order> => {
    const response = await axios.post(`${API_BASE_URL}/orders/${orderId}/cancel`, null, {
      params: { reason }
    });
    return response.data;
  },

  // Get daily revenue
  getDailyRevenue: async (restaurantId: number, date: string): Promise<DailyRevenue> => {
    const response = await axios.get(`${API_BASE_URL}/orders/restaurant/${restaurantId}/revenue`, {
      params: { date }
    });
    return response.data;
  },

  // Get popular items
  getPopularItems: async (restaurantId: number, days: number = 7): Promise<PopularItems> => {
    const response = await axios.get(`${API_BASE_URL}/orders/restaurant/${restaurantId}/popular`, {
      params: { days }
    });
    return response.data;
  }
};