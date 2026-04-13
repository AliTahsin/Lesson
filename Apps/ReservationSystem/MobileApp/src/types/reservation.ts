export interface Guest {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  totalStays: number;
  totalSpent: number;
}

export interface Reservation {
  id: number;
  reservationNumber: string;
  guestName: string;
  guestEmail: string;
  hotelName: string;
  roomNumber: string;
  roomType: string;
  checkInDate: string;
  checkOutDate: string;
  nightCount: number;
  guestCount: number;
  totalPrice: number;
  status: string;
  paymentStatus: string;
  specialRequests: string;
  createdAt: string;
  canCancel: boolean;
  canModify: boolean;
}

export interface CreateReservationRequest {
  guestFirstName: string;
  guestLastName: string;
  guestEmail: string;
  guestPhone: string;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  guestCount: number;
  childCount: number;
  specialRequests: string;
  paymentMethod: string;
  source: string;
}

export interface CheckInRequest {
  reservationNumber: string;
  guestEmail: string;
  passportNumber: string;
  additionalGuestNames?: string[];
}

export interface CheckOutRequest {
  reservationNumber: string;
  guestEmail: string;
  extraCharges?: number;
  extraChargesDescription?: string;
}

export interface CheckResponse {
  success: boolean;
  message: string;
  reservation: Reservation;
  digitalKey?: string;
  totalExtraCharges?: number;
}

export interface ReservationStats {
  totalReservations: number;
  totalRevenue: number;
  averageBookingValue: number;
  confirmedReservations: number;
  checkedInToday: number;
  checkedOutToday: number;
  cancelledReservations: number;
  upcomingArrivals: number;
  occupancyRate: number;
}