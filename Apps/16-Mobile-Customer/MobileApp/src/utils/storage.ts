import AsyncStorage from '@react-native-async-storage/async-storage';

const STORAGE_KEYS = {
  ACCESS_TOKEN: 'accessToken',
  REFRESH_TOKEN: 'refreshToken',
  USER: 'user',
  LANGUAGE: 'language',
  BIOMETRIC_ENABLED: 'biometricEnabled',
};

export const storage = {
  async setItem(key: string, value: any): Promise<void> {
    try {
      await AsyncStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error('Error saving to storage:', error);
    }
  },

  async getItem<T>(key: string): Promise<T | null> {
    try {
      const value = await AsyncStorage.getItem(key);
      return value ? JSON.parse(value) : null;
    } catch (error) {
      console.error('Error reading from storage:', error);
      return null;
    }
  },

  async removeItem(key: string): Promise<void> {
    try {
      await AsyncStorage.removeItem(key);
    } catch (error) {
      console.error('Error removing from storage:', error);
    }
  },

  async clear(): Promise<void> {
    try {
      await AsyncStorage.clear();
    } catch (error) {
      console.error('Error clearing storage:', error);
    }
  },

  // Auth related
  async setTokens(accessToken: string, refreshToken: string): Promise<void> {
    await this.setItem(STORAGE_KEYS.ACCESS_TOKEN, accessToken);
    await this.setItem(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
  },

  async getAccessToken(): Promise<string | null> {
    return await this.getItem<string>(STORAGE_KEYS.ACCESS_TOKEN);
  },

  async getRefreshToken(): Promise<string | null> {
    return await this.getItem<string>(STORAGE_KEYS.REFRESH_TOKEN);
  },

  async clearTokens(): Promise<void> {
    await this.removeItem(STORAGE_KEYS.ACCESS_TOKEN);
    await this.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    await this.removeItem(STORAGE_KEYS.USER);
  },

  // Language
  async setLanguage(language: string): Promise<void> {
    await this.setItem(STORAGE_KEYS.LANGUAGE, language);
  },

  async getLanguage(): Promise<string | null> {
    return await this.getItem<string>(STORAGE_KEYS.LANGUAGE);
  },

  // Biometric
  async setBiometricEnabled(enabled: boolean): Promise<void> {
    await this.setItem(STORAGE_KEYS.BIOMETRIC_ENABLED, enabled);
  },

  async isBiometricEnabled(): Promise<boolean> {
    return await this.getItem<boolean>(STORAGE_KEYS.BIOMETRIC_ENABLED) || false;
  },

  // User
  async setUser(user: any): Promise<void> {
    await this.setItem(STORAGE_KEYS.USER, user);
  },

  async getUser(): Promise<any | null> {
    return await this.getItem(STORAGE_KEYS.USER);
  },
};