import axios from 'axios';
import { 
  Customer, 
  LoyaltyInfo, 
  LoyaltyTransaction, 
  MembershipLevel,
  Preference,
  LoyaltyStats 
} from '../types/loyalty';

const API_BASE_URL = 'http://localhost:5004/api';

export const loyaltyApi = {
  // Customer endpoints
  getAllCustomers: async (): Promise<Customer[]> => {
    const response = await axios.get(`${API_BASE_URL}/customers`);
    return response.data;
  },

  getCustomerById: async (id: number): Promise<Customer> => {
    const response = await axios.get(`${API_BASE_URL}/customers/${id}`);
    return response.data;
  },

  getCustomerByEmail: async (email: string): Promise<Customer> => {
    const response = await axios.get(`${API_BASE_URL}/customers/email/${email}`);
    return response.data;
  },

  createCustomer: async (data: any): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/customers`, data);
    return response.data;
  },

  updateCustomer: async (id: number, data: Partial<Customer>): Promise<any> => {
    const response = await axios.put(`${API_BASE_URL}/customers/${id}`, data);
    return response.data;
  },

  // Loyalty endpoints
  getLoyaltyInfo: async (customerId: number): Promise<LoyaltyInfo> => {
    const response = await axios.get(`${API_BASE_URL}/loyalty/info/${customerId}`);
    return response.data;
  },

  getTransactionHistory: async (customerId: number): Promise<LoyaltyTransaction[]> => {
    const response = await axios.get(`${API_BASE_URL}/loyalty/history/${customerId}`);
    return response.data;
  },

  getMembershipLevels: async (): Promise<MembershipLevel[]> => {
    const response = await axios.get(`${API_BASE_URL}/loyalty/levels`);
    return response.data;
  },

  addPoints: async (customerId: number, points: number, source: string, description: string): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/loyalty/add-points`, {
      customerId, points, source, description
    });
    return response.data;
  },

  redeemPoints: async (customerId: number, points: number, description: string): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/loyalty/redeem-points`, {
      customerId, points, description
    });
    return response.data;
  },

  getLoyaltyStats: async (): Promise<LoyaltyStats> => {
    const response = await axios.get(`${API_BASE_URL}/loyalty/statistics`);
    return response.data;
  },

  // Preferences endpoints
  getPreferences: async (customerId: number): Promise<Preference[]> => {
    const response = await axios.get(`${API_BASE_URL}/preferences/${customerId}`);
    return response.data;
  },

  addPreference: async (customerId: number, data: Partial<Preference>): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/preferences/${customerId}`, data);
    return response.data;
  },

  deletePreference: async (preferenceId: number): Promise<any> => {
    const response = await axios.delete(`${API_BASE_URL}/preferences/${preferenceId}`);
    return response.data;
  }
};