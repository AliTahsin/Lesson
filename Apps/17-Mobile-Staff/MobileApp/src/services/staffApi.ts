import axios from 'axios';
import { storage } from '../utils/storage';
import {
  LoginRequest,
  LoginResponse,
  Staff,
  StaffTask,
  MaintenanceIssue,
  CheckInOut,
  DashboardStats,
  Notification,
  CreateTaskDto,
  CreateIssueDto,
  CheckInDto,
  CheckOutDto
} from '../types/staff';

const API_BASE_URL = 'http://localhost:5016/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
api.interceptors.request.use(
  async (config) => {
    const token = await storage.getAccessToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      await storage.clear();
      // Navigate to login screen
    }
    return Promise.reject(error);
  }
);

export const staffApi = {
  // Auth
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post('/auth/login', data);
    return response.data;
  },

  logout: async (): Promise<void> => {
    await api.post('/auth/logout');
    await storage.clear();
  },

  getProfile: async (): Promise<Staff> => {
    const response = await api.get('/auth/profile');
    return response.data;
  },

  updateProfile: async (data: any): Promise<Staff> => {
    const response = await api.put('/auth/profile', data);
    return response.data;
  },

  changePassword: async (data: any): Promise<void> => {
    await api.post('/auth/change-password', data);
  },

  // Tasks
  getMyTasks: async (): Promise<StaffTask[]> => {
    const response = await api.get('/tasks/my-tasks');
    return response.data;
  },

  getPendingTasks: async (): Promise<StaffTask[]> => {
    const response = await api.get('/tasks/pending');
    return response.data;
  },

  getTaskById: async (id: number): Promise<StaffTask> => {
    const response = await api.get(`/tasks/${id}`);
    return response.data;
  },

  startTask: async (id: number): Promise<StaffTask> => {
    const response = await api.post(`/tasks/${id}/start`);
    return response.data;
  },

  completeTask: async (id: number, notes: string, afterImages?: string[]): Promise<StaffTask> => {
    const response = await api.post(`/tasks/${id}/complete`, { notes, afterImages });
    return response.data;
  },

  createTask: async (data: CreateTaskDto): Promise<StaffTask> => {
    const response = await api.post('/tasks', data);
    return response.data;
  },

  assignTask: async (taskId: number, staffId: number): Promise<StaffTask> => {
    const response = await api.post(`/tasks/${taskId}/assign/${staffId}`);
    return response.data;
  },

  // Issues
  getMyIssues: async (): Promise<MaintenanceIssue[]> => {
    const response = await api.get('/issues/my-issues');
    return response.data;
  },

  getCriticalIssues: async (): Promise<MaintenanceIssue[]> => {
    const response = await api.get('/issues/critical');
    return response.data;
  },

  getIssueById: async (id: number): Promise<MaintenanceIssue> => {
    const response = await api.get(`/issues/${id}`);
    return response.data;
  },

  reportIssue: async (data: CreateIssueDto): Promise<MaintenanceIssue> => {
    const response = await api.post('/issues', data);
    return response.data;
  },

  assignIssue: async (issueId: number, staffId: number): Promise<MaintenanceIssue> => {
    const response = await api.post(`/issues/${issueId}/assign/${staffId}`);
    return response.data;
  },

  startIssue: async (id: number): Promise<MaintenanceIssue> => {
    const response = await api.post(`/issues/${id}/start`);
    return response.data;
  },

  resolveIssue: async (id: number, resolutionNotes: string, actualCost: number): Promise<MaintenanceIssue> => {
    const response = await api.post(`/issues/${id}/resolve`, { resolutionNotes, actualCost });
    return response.data;
  },

  // Check In/Out
  checkIn: async (data: CheckInDto): Promise<CheckInOut> => {
    const response = await api.post('/check/checkin', data);
    return response.data;
  },

  checkOut: async (data: CheckOutDto): Promise<CheckInOut> => {
    const response = await api.post('/check/checkout', data);
    return response.data;
  },

  getTodayCheckIns: async (): Promise<CheckInOut[]> => {
    const response = await api.get('/check/today-checkins');
    return response.data;
  },

  getTodayCheckOuts: async (): Promise<CheckInOut[]> => {
    const response = await api.get('/check/today-checkouts');
    return response.data;
  },

  // Dashboard
  getDashboardStats: async (): Promise<DashboardStats> => {
    const response = await api.get('/dashboard/stats');
    return response.data;
  },

  // Notifications
  getNotifications: async (): Promise<Notification[]> => {
    const response = await api.get('/notifications');
    return response.data;
  },

  markAsRead: async (id: number): Promise<void> => {
    await api.post(`/notifications/${id}/read`);
  },

  getUnreadCount: async (): Promise<number> => {
    const response = await api.get('/notifications/unread/count');
    return response.data.count;
  }
};