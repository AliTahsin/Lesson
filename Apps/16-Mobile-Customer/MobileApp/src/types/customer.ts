export interface CustomerProfile {
  id: number;
  userId: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth?: string;
  country?: string;
  city?: string;
  address?: string;
  language: string;
  biometricEnabled: boolean;
  pushEnabled: boolean;
  emailEnabled: boolean;
  smsEnabled: boolean;
  profileImageUrl?: string;
  lastLoginAt?: string;
}

export interface DigitalKey {
  id: number;
  reservationId: number;
  roomId: number;
  roomNumber: string;
  keyCode: string;
  qrCode: string;
  validFrom: string;
  validUntil: string;
  isActive: boolean;
  isUsed: boolean;
  usedAt?: string;
}

export interface MenuItem {
  id: number;
  name: string;
  description: string;
  category: string;
  price: number;
  currency: string;
  imageUrl: string;
  isAvailable: boolean;
  preparationTimeMinutes: number;
}

export interface RoomServiceOrder {
  id: number;
  orderNumber: string;
  status: string;
  items: OrderItem[];
  subTotal: number;
  taxAmount: number;
  totalAmount: number;
  currency: string;
  specialInstructions?: string;
  orderTime: string;
  estimatedDeliveryTime?: string;
  deliveredAt?: string;
}

export interface OrderItem {
  itemName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  specialInstructions?: string;
}

export interface SpaService {
  id: number;
  name: string;
  description: string;
  durationMinutes: number;
  price: number;
  category: string;
  imageUrl?: string;
}

export interface SpaAppointment {
  id: number;
  appointmentNumber: string;
  serviceName: string;
  serviceType: string;
  durationMinutes: number;
  price: number;
  appointmentDate: string;
  appointmentTime: string;
  specialRequests?: string;
  status: string;
  createdAt: string;
}

export interface Language {
  code: string;
  name: string;
  nativeName: string;
  isRTL: boolean;
}

export interface UpdateProfileRequest {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  country?: string;
  city?: string;
  address?: string;
  profileImageUrl?: string;
}

export interface NotificationSettings {
  pushEnabled: boolean;
  emailEnabled: boolean;
  smsEnabled: boolean;
}