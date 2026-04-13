export interface Channel {
  id: number;
  name: string;
  code: string;
  type: string;
  typeName: string;
  commission: number;
  markup: number;
  isActive: boolean;
  logoUrl: string;
  description: string;
}

export interface ChannelConnection {
  id: number;
  channelId: number;
  channelName: string;
  hotelId: number;
  hotelName: string;
  connectionStatus: string;
  connectedAt: string;
  lastSyncAt: string | null;
  lastSyncStatus: string;
  totalBookings: number;
  totalRevenue: number;
  autoSync: boolean;
}

export interface ChannelBooking {
  id: number;
  channelBookingId: string;
  channelName: string;
  hotelName: string;
  roomNumber: string;
  guestName: string;
  guestEmail: string;
  checkInDate: string;
  checkOutDate: string;
  nightCount: number;
  guestCount: number;
  totalPrice: number;
  commission: number;
  netRevenue: number;
  status: string;
  bookingDate: string;
}

export interface SyncResponse {
  success: boolean;
  message: string;
  recordsProcessed: number;
  recordsSuccess: number;
  recordsFailed: number;
  errors: string[];
  syncTime: string;
}

export interface DashboardStats {
  totalChannels: number;
  activeChannels: number;
  totalConnections: number;
  activeConnections: number;
  totalRevenue: number;
  totalCommission: number;
  netRevenue: number;
  totalBookings: number;
  channelStats: {
    channelName: string;
    bookingCount: number;
    revenue: number;
    commission: number;
    netRevenue: number;
  }[];
  recentBookings: {
    date: string;
    bookingCount: number;
    revenue: number;
  }[];
}

export interface SyncLog {
  id: number;
  channelId: number;
  hotelId: number;
  syncType: string;
  startTime: string;
  endTime: string | null;
  status: string;
  recordsProcessed: number;
  recordsSuccess: number;
  recordsFailed: number;
  errorMessage: string | null;
}