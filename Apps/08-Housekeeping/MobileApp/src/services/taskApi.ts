import axios from 'axios';
import { HousekeepingTask, CreateTaskDto, DashboardStats } from '../types/housekeeping';

const API_BASE_URL = 'http://localhost:5007/api';

export const taskApi = {
  // Get all tasks
  getAllTasks: async (): Promise<HousekeepingTask[]> => {
    const response = await axios.get(`${API_BASE_URL}/tasks`);
    return response.data;
  },

  // Get task by ID
  getTaskById: async (id: number): Promise<HousekeepingTask> => {
    const response = await axios.get(`${API_BASE_URL}/tasks/${id}`);
    return response.data;
  },

  // Get tasks by hotel
  getTasksByHotel: async (hotelId: number): Promise<HousekeepingTask[]> => {
    const response = await axios.get(`${API_BASE_URL}/tasks/hotel/${hotelId}`);
    return response.data;
  },

  // Get tasks by staff
  getTasksByStaff: async (staffId: number): Promise<HousekeepingTask[]> => {
    const response = await axios.get(`${API_BASE_URL}/tasks/staff/${staffId}`);
    return response.data;
  },

  // Get pending tasks
  getPendingTasks: async (): Promise<HousekeepingTask[]> => {
    const response = await axios.get(`${API_BASE_URL}/tasks/pending`);
    return response.data;
  },

  // Create task
  createTask: async (data: CreateTaskDto): Promise<HousekeepingTask> => {
    const response = await axios.post(`${API_BASE_URL}/tasks`, data);
    return response.data;
  },

  // Assign task
  assignTask: async (taskId: number, staffId: number): Promise<HousekeepingTask> => {
    const response = await axios.post(`${API_BASE_URL}/tasks/${taskId}/assign/${staffId}`);
    return response.data;
  },

  // Start task
  startTask: async (taskId: number): Promise<HousekeepingTask> => {
    const response = await axios.post(`${API_BASE_URL}/tasks/${taskId}/start`);
    return response.data;
  },

  // Complete task
  completeTask: async (taskId: number, notes: string, afterImages?: string[]): Promise<HousekeepingTask> => {
    const response = await axios.post(`${API_BASE_URL}/tasks/${taskId}/complete`, { notes, afterImages });
    return response.data;
  },

  // Cancel task
  cancelTask: async (taskId: number, reason: string): Promise<HousekeepingTask> => {
    const response = await axios.post(`${API_BASE_URL}/tasks/${taskId}/cancel`, null, {
      params: { reason }
    });
    return response.data;
  },

  // Get dashboard stats
  getDashboardStats: async (hotelId: number): Promise<DashboardStats> => {
    const response = await axios.get(`${API_BASE_URL}/dashboard/stats/${hotelId}`);
    return response.data;
  }
};