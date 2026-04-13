import { useState, useEffect } from 'react';
import { staffApi } from '../services/staffApi';
import { storage } from '../utils/storage';
import { signalRService } from '../services/signalRService';
import { Staff, LoginRequest } from '../types/staff';

export const useStaffAuth = () => {
  const [staff, setStaff] = useState<Staff | null>(null);
  const [role, setRole] = useState<string | null>(null);
  const [hotelId, setHotelId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    loadStoredData();
  }, []);

  const loadStoredData = async () => {
    try {
      const storedStaff = await storage.getStaff();
      const storedRole = await storage.getRole();
      const storedHotelId = await storage.getHotelId();
      const token = await storage.getAccessToken();

      if (storedStaff && token) {
        setStaff(storedStaff);
        setRole(storedRole);
        setHotelId(storedHotelId);
        setIsAuthenticated(true);
        
        if (storedHotelId && storedStaff?.id) {
          await signalRService.connect(storedHotelId, storedStaff.id);
        }
      }
    } catch (error) {
      console.error('Error loading stored data:', error);
    } finally {
      setLoading(false);
    }
  };

  const login = async (data: LoginRequest) => {
    const response = await staffApi.login(data);
    
    await storage.setAccessToken(response.accessToken);
    await storage.setStaff(response.staff);
    await storage.setRole(response.role);
    await storage.setHotelId(response.staff.hotelId);
    
    setStaff(response.staff);
    setRole(response.role);
    setHotelId(response.staff.hotelId);
    setIsAuthenticated(true);
    
    await signalRService.connect(response.staff.hotelId, response.staff.id);
    
    return response;
  };

  const logout = async () => {
    await staffApi.logout();
    await storage.clear();
    signalRService.disconnect();
    setStaff(null);
    setRole(null);
    setHotelId(null);
    setIsAuthenticated(false);
  };

  const hasRole = (requiredRole: string | string[]): boolean => {
    if (!role) return false;
    if (Array.isArray(requiredRole)) {
      return requiredRole.includes(role);
    }
    return role === requiredRole || role === 'Admin';
  };

  return {
    staff,
    role,
    hotelId,
    loading,
    isAuthenticated,
    login,
    logout,
    hasRole
  };
};