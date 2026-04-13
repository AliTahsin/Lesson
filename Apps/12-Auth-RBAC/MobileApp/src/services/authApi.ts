import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import * as Keychain from 'react-native-keychain';
import {
  LoginRequest,
  LoginResponse,
  TwoFactorVerifyRequest,
  RegisterRequest,
  ChangePasswordRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  UpdateUserRequest,
  User,
  Role,
  Permission,
  TokenPair
} from '../types/auth';

const API_BASE_URL = 'http://localhost:5011/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add token
api.interceptors.request.use(
  async (config) => {
    const token = await AsyncStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor to handle token refresh
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshToken = await AsyncStorage.getItem('refreshToken');
        if (refreshToken) {
          const response = await axios.post(`${API_BASE_URL}/auth/refresh`, { refreshToken });
          const { accessToken, refreshToken: newRefreshToken } = response.data;
          await AsyncStorage.setItem('accessToken', accessToken);
          await AsyncStorage.setItem('refreshToken', newRefreshToken);
          originalRequest.headers.Authorization = `Bearer ${accessToken}`;
          return api(originalRequest);
        }
      } catch (refreshError) {
        await AsyncStorage.removeItem('accessToken');
        await AsyncStorage.removeItem('refreshToken');
        await AsyncStorage.removeItem('user');
      }
    }
    return Promise.reject(error);
  }
);

export const authApi = {
  // Auth endpoints
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post('/auth/login', data);
    return response.data;
  },

  loginWith2FA: async (data: TwoFactorVerifyRequest): Promise<LoginResponse> => {
    const response = await api.post('/auth/login/2fa', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<{ message: string; user: User }> => {
    const response = await api.post('/auth/register', data);
    return response.data;
  },

  refreshToken: async (refreshToken: string): Promise<TokenPair> => {
    const response = await api.post('/auth/refresh', { refreshToken });
    return response.data;
  },

  logout: async (refreshToken: string): Promise<void> => {
    await api.post('/auth/logout', { refreshToken });
  },

  changePassword: async (data: ChangePasswordRequest): Promise<void> => {
    await api.post('/auth/change-password', data);
  },

  forgotPassword: async (data: ForgotPasswordRequest): Promise<void> => {
    await api.post('/auth/forgot-password', data);
  },

  resetPassword: async (data: ResetPasswordRequest): Promise<void> => {
    await api.post('/auth/reset-password', data);
  },

  enableTwoFactor: async (enabled: boolean, method?: string): Promise<void> => {
    await api.post('/auth/2fa/enable', { enabled, method });
  },

  sendTwoFactorCode: async (): Promise<void> => {
    await api.post('/auth/2fa/send-code');
  },

  verifyTwoFactorCode: async (code: string): Promise<void> => {
    await api.post('/auth/2fa/verify', { code });
  },

  // User endpoints
  getUsers: async (): Promise<User[]> => {
    const response = await api.get('/users');
    return response.data;
  },

  getUserById: async (id: number): Promise<User> => {
    const response = await api.get(`/users/${id}`);
    return response.data;
  },

  getUsersByHotel: async (hotelId: number): Promise<User[]> => {
    const response = await api.get(`/users/hotel/${hotelId}`);
    return response.data;
  },

  updateUser: async (id: number, data: UpdateUserRequest): Promise<void> => {
    await api.put(`/users/${id}`, data);
  },

  updateUserRoles: async (id: number, roleIds: number[]): Promise<void> => {
    await api.put(`/users/${id}/roles`, { roleIds });
  },

  activateUser: async (id: number): Promise<void> => {
    await api.post(`/users/${id}/activate`);
  },

  deactivateUser: async (id: number): Promise<void> => {
    await api.post(`/users/${id}/deactivate`);
  },

  deleteUser: async (id: number): Promise<void> => {
    await api.delete(`/users/${id}`);
  },

  // Role endpoints
  getRoles: async (): Promise<Role[]> => {
    const response = await api.get('/roles');
    return response.data;
  },

  getRoleById: async (id: number): Promise<Role> => {
    const response = await api.get(`/roles/${id}`);
    return response.data;
  },

  createRole: async (data: Partial<Role>): Promise<Role> => {
    const response = await api.post('/roles', data);
    return response.data;
  },

  updateRole: async (id: number, data: Partial<Role>): Promise<void> => {
    await api.put(`/roles/${id}`, data);
  },

  deleteRole: async (id: number): Promise<void> => {
    await api.delete(`/roles/${id}`);
  },

  addPermissionToRole: async (roleId: number, permissionId: number): Promise<void> => {
    await api.post(`/roles/${roleId}/permissions/${permissionId}`);
  },

  removePermissionFromRole: async (roleId: number, permissionId: number): Promise<void> => {
    await api.delete(`/roles/${roleId}/permissions/${permissionId}`);
  },

  // Permission endpoints
  getPermissions: async (): Promise<Permission[]> => {
    const response = await api.get('/permissions');
    return response.data;
  },

  getPermissionsByCategory: async (category: string): Promise<Permission[]> => {
    const response = await api.get(`/permissions/category/${category}`);
    return response.data;
  },

  createPermission: async (data: Partial<Permission>): Promise<Permission> => {
    const response = await api.post('/permissions', data);
    return response.data;
  },

  updatePermission: async (id: number, data: Partial<Permission>): Promise<void> => {
    await api.put(`/permissions/${id}`, data);
  },

  deletePermission: async (id: number): Promise<void> => {
    await api.delete(`/permissions/${id}`);
  },
};

export default api;