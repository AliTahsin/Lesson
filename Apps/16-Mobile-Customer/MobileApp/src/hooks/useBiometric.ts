import { useState, useEffect } from 'react';
import { biometricService, BiometricResult } from '../services/biometricService';
import { storage } from '../utils/storage';
import { customerApi } from '../services/customerApi';

export const useBiometric = () => {
  const [isAvailable, setIsAvailable] = useState(false);
  const [isEnabled, setIsEnabled] = useState(false);
  const [biometricType, setBiometricType] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    checkBiometricStatus();
  }, []);

  const checkBiometricStatus = async () => {
    setLoading(true);
    try {
      const available = await biometricService.isAvailable();
      setIsAvailable(available);
      
      if (available) {
        const type = await biometricService.getBiometricType();
        setBiometricType(type);
        
        const enabled = await storage.isBiometricEnabled();
        setIsEnabled(enabled);
      }
    } catch (error) {
      console.error('Error checking biometric status:', error);
    } finally {
      setLoading(false);
    }
  };

  const authenticate = async (): Promise<BiometricResult> => {
    if (!isEnabled) {
      return { success: false, error: 'Biyometrik giriş aktif değil' };
    }
    return await biometricService.authenticate();
  };

  const enableBiometric = async (): Promise<boolean> => {
    try {
      const authResult = await biometricService.authenticate();
      if (authResult.success) {
        await storage.setBiometricEnabled(true);
        await customerApi.updateBiometric(true, 'biometric-key');
        setIsEnabled(true);
        return true;
      }
      return false;
    } catch (error) {
      console.error('Error enabling biometric:', error);
      return false;
    }
  };

  const disableBiometric = async (): Promise<boolean> => {
    try {
      await storage.setBiometricEnabled(false);
      await customerApi.updateBiometric(false);
      setIsEnabled(false);
      return true;
    } catch (error) {
      console.error('Error disabling biometric:', error);
      return false;
    }
  };

  return {
    isAvailable,
    isEnabled,
    biometricType,
    loading,
    authenticate,
    enableBiometric,
    disableBiometric,
  };
};