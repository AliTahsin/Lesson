import axios from 'axios';
import { 
  PriceRequest, 
  PriceResponse, 
  DynamicPrice, 
  DemandFactor, 
  PriceRule, 
  Season,
  PricingStats 
} from '../types/pricing';

const API_BASE_URL = 'http://localhost:5003/api';

export const pricingApi = {
  // Calculate price
  calculatePrice: async (request: PriceRequest): Promise<PriceResponse> => {
    const response = await axios.post(`${API_BASE_URL}/pricing/calculate`, request);
    return response.data;
  },

  // Get all dynamic prices
  getAllPrices: async (): Promise<DynamicPrice[]> => {
    const response = await axios.get(`${API_BASE_URL}/pricing/prices`);
    return response.data;
  },

  // Get prices by room
  getPricesByRoom: async (roomId: number, startDate: string, endDate: string): Promise<DynamicPrice[]> => {
    const response = await axios.get(`${API_BASE_URL}/pricing/prices/room/${roomId}`, {
      params: { startDate, endDate }
    });
    return response.data;
  },

  // Get demand factors
  getDemandFactors: async (hotelId: number, startDate: string, endDate: string): Promise<DemandFactor[]> => {
    const response = await axios.get(`${API_BASE_URL}/pricing/demand/${hotelId}`, {
      params: { startDate, endDate }
    });
    return response.data;
  },

  // Get pricing statistics
  getStatistics: async (hotelId: number, startDate: string, endDate: string): Promise<PricingStats> => {
    const response = await axios.get(`${API_BASE_URL}/pricing/stats/${hotelId}`, {
      params: { startDate, endDate }
    });
    return response.data;
  },

  // Update price manually
  updatePrice: async (roomId: number, date: string, price: number): Promise<any> => {
    const response = await axios.put(`${API_BASE_URL}/pricing/price`, null, {
      params: { roomId, date, price }
    });
    return response.data;
  },

  // Get all price rules
  getAllPriceRules: async (): Promise<PriceRule[]> => {
    const response = await axios.get(`${API_BASE_URL}/pricerules`);
    return response.data;
  },

  // Get all seasons
  getAllSeasons: async (): Promise<Season[]> => {
    const response = await axios.get(`${API_BASE_URL}/pricerules/seasons`);
    return response.data;
  }
};