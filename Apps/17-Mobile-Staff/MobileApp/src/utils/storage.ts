import AsyncStorage from '@react-native-async-storage/async-storage';

const STORAGE_KEYS = {
  ACCESS_TOKEN: 'staff_access_token',
  STAFF: 'staff_user',
  ROLE: 'staff_role',
  HOTEL_ID: 'staff_hotel_id'
};

export const storage = {
  async setAccessToken(token: string): Promise<void> {
    await AsyncStorage.setItem(STORAGE_KEYS.ACCESS_TOKEN, token);
  },

  async getAccessToken(): Promise<string | null> {
    return await AsyncStorage.getItem(STORAGE_KEYS.ACCESS_TOKEN);
  },

  async setStaff(staff: any): Promise<void> {
    await AsyncStorage.setItem(STORAGE_KEYS.STAFF, JSON.stringify(staff));
  },

  async getStaff(): Promise<any | null> {
    const data = await AsyncStorage.getItem(STORAGE_KEYS.STAFF);
    return data ? JSON.parse(data) : null;
  },

  async setRole(role: string): Promise<void> {
    await AsyncStorage.setItem(STORAGE_KEYS.ROLE, role);
  },

  async getRole(): Promise<string | null> {
    return await AsyncStorage.getItem(STORAGE_KEYS.ROLE);
  },

  async setHotelId(hotelId: number): Promise<void> {
    await AsyncStorage.setItem(STORAGE_KEYS.HOTEL_ID, hotelId.toString());
  },

  async getHotelId(): Promise<number | null> {
    const data = await AsyncStorage.getItem(STORAGE_KEYS.HOTEL_ID);
    return data ? parseInt(data) : null;
  },

  async clear(): Promise<void> {
    await AsyncStorage.multiRemove([
      STORAGE_KEYS.ACCESS_TOKEN,
      STORAGE_KEYS.STAFF,
      STORAGE_KEYS.ROLE,
      STORAGE_KEYS.HOTEL_ID
    ]);
  }
};