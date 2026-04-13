import * as LocalAuthentication from 'expo-local-authentication';
import { Platform } from 'react-native';

export interface BiometricResult {
  success: boolean;
  error?: string;
  type?: string;
}

export const biometricService = {
  async isAvailable(): Promise<boolean> {
    try {
      const isCompatible = await LocalAuthentication.hasHardwareAsync();
      const isEnrolled = await LocalAuthentication.isEnrolledAsync();
      return isCompatible && isEnrolled;
    } catch (error) {
      console.error('Biometric availability error:', error);
      return false;
    }
  },

  async getBiometricType(): Promise<string> {
    const types = await LocalAuthentication.supportedAuthenticationTypesAsync();
    if (types.includes(LocalAuthentication.AuthenticationType.FACIAL_RECOGNITION)) {
      return Platform.OS === 'ios' ? 'Face ID' : 'Yüz Tanıma';
    }
    if (types.includes(LocalAuthentication.AuthenticationType.FINGERPRINT)) {
      return Platform.OS === 'ios' ? 'Touch ID' : 'Parmak İzi';
    }
    return 'Biyometrik';
  },

  async authenticate(): Promise<BiometricResult> {
    try {
      const isAvailable = await this.isAvailable();
      if (!isAvailable) {
        return { success: false, error: 'Biyometrik kimlik doğrulama mevcut değil' };
      }

      const result = await LocalAuthentication.authenticateAsync({
        promptMessage: 'Kimlik doğrulama',
        cancelLabel: 'İptal',
        disableDeviceFallback: false,
      });

      if (result.success) {
        return { success: true, type: await this.getBiometricType() };
      } else {
        return { success: false, error: result.error || 'Doğrulama başarısız' };
      }
    } catch (error) {
      return { success: false, error: 'Doğrulama sırasında bir hata oluştu' };
    }
  },
};