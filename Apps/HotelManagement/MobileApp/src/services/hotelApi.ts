import axios from 'axios';
import { Hotel, Brand, Chain, HotelStats } from '../types/hotel';

const API_BASE_URL = 'http://localhost:5000/api';

export const hotelApi = {
  // Hotel endpoints
  getAllHotels: async (): Promise<Hotel[]> => {
    const response = await axios.get(`${API_BASE_URL}/hotels`);
    return response.data;
  },

  getHotelById: async (id: number): Promise<Hotel> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/${id}`);
    return response.data;
  },

  getHotelsByBrand: async (brandId: number): Promise<Hotel[]> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/brand/${brandId}`);
    return response.data;
  },

  getHotelsByChain: async (chainId: number): Promise<Hotel[]> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/chain/${chainId}`);
    return response.data;
  },

  getHotelsByCity: async (city: string): Promise<Hotel[]> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/city/${city}`);
    return response.data;
  },

  searchHotels: async (keyword: string): Promise<Hotel[]> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/search`, { params: { keyword } });
    return response.data;
  },

  getStatistics: async (): Promise<HotelStats> => {
    const response = await axios.get(`${API_BASE_URL}/hotels/stats`);
    return response.data;
  },

  // Brand endpoints
  getAllBrands: async (): Promise<Brand[]> => {
    const response = await axios.get(`${API_BASE_URL}/brands`);
    return response.data;
  },

  getBrandById: async (id: number): Promise<Brand> => {
    const response = await axios.get(`${API_BASE_URL}/brands/${id}`);
    return response.data;
  },

  getBrandsByChain: async (chainId: number): Promise<Brand[]> => {
    const response = await axios.get(`${API_BASE_URL}/brands/chain/${chainId}`);
    return response.data;
  },

  // Chain endpoints
  getAllChains: async (): Promise<Chain[]> => {
    const response = await axios.get(`${API_BASE_URL}/chains`);
    return response.data;
  },

  getChainById: async (id: number): Promise<Chain> => {
    const response = await axios.get(`${API_BASE_URL}/chains/${id}`);
    return response.data;
  }
};