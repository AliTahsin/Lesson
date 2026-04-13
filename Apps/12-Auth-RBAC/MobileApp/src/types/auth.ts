export interface User {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  username: string;
  isActive: boolean;
  isEmailVerified: boolean;
  isPhoneVerified: boolean;
  twoFactorEnabled: boolean;
  twoFactorMethod: string;
  lastLoginAt?: string;
  hotelId: number;
  department: string;
  position: string;
  profileImageUrl?: string;
  roles: string[];
  permissions: string[];
}

export interface LoginRequest {
  emailOrUsername: string;
  password: string;
  deviceId?: string;
  deviceName?: string;
  ipAddress?: string;
  userAgent?: string;
}

export interface LoginResponse {
  requiresTwoFactor: boolean;
  userId?: number;
  accessToken?: string;
  refreshToken?: string;
  user?: User;
  message: string;
}

export interface TwoFactorVerifyRequest {
  userId: number;
  code: string;
  deviceId?: string;
  deviceName?: string;
  ipAddress?: string;
  userAgent?: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
  username?: string;
  hotelId: number;
  department?: string;
  position?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface UpdateUserRequest {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  department?: string;
  position?: string;
  profileImageUrl?: string;
}

export interface Role {
  id: number;
  name: string;
  description: string;
  level: string;
  permissions: string[];
  isDefault: boolean;
  isActive: boolean;
}

export interface Permission {
  id: number;
  name: string;
  code: string;
  category: string;
  description: string;
}

export interface TokenPair {
  accessToken: string;
  refreshToken: string;
}