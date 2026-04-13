export interface DailyRevenue {
  date: string;
  revenue: number;
  roomRevenue: number;
  fbRevenue: number;
  otherRevenue: number;
}

export interface RevenueReport {
  hotelId: number;
  hotelName: string;
  startDate: string;
  endDate: string;
  totalRevenue: number;
  totalRoomRevenue: number;
  totalFBRevenue: number;
  totalOtherRevenue: number;
  averageDailyRate: number;
  revPAR: number;
  dailyData: DailyRevenue[];
  generatedAt: string;
}

export interface DailyOccupancy {
  date: string;
  occupancyRate: number;
  availableRooms: number;
  soldRooms: number;
  averagePrice: number;
}

export interface OccupancyReport {
  hotelId: number;
  hotelName: string;
  startDate: string;
  endDate: string;
  averageOccupancyRate: number;
  totalAvailableRooms: number;
  totalSoldRooms: number;
  occupancyData: DailyOccupancy[];
  generatedAt: string;
}

export interface DailyReservation {
  date: string;
  totalReservations: number;
  confirmedReservations: number;
  cancelledReservations: number;
  noShowReservations: number;
  averageLeadDays: number;
}

export interface ReservationReport {
  hotelId: number;
  hotelName: string;
  startDate: string;
  endDate: string;
  totalReservations: number;
  confirmedReservations: number;
  cancelledReservations: number;
  noShowReservations: number;
  cancellationRate: number;
  noShowRate: number;
  reservationData: DailyReservation[];
  generatedAt: string;
}

export interface TopCustomer {
  customerId: number;
  customerName: string;
  totalStays: number;
  totalSpent: number;
  averageSpentPerStay: number;
}

export interface CustomerReport {
  hotelId: number;
  hotelName: string;
  startDate: string;
  endDate: string;
  totalCustomers: number;
  newCustomers: number;
  repeatCustomers: number;
  customerSatisfactionScore: number;
  topCustomers: TopCustomer[];
  generatedAt: string;
}

export interface ChannelPerformance {
  channelName: string;
  bookings: number;
  revenue: number;
  commission: number;
  netRevenue: number;
}

export interface ChannelReport {
  hotelId: number;
  hotelName: string;
  startDate: string;
  endDate: string;
  totalBookings: number;
  totalRevenue: number;
  totalCommission: number;
  netRevenue: number;
  channelData: ChannelPerformance[];
  generatedAt: string;
}

export interface KPI {
  id: number;
  name: string;
  code: string;
  description: string;
  category: string;
  hotelId: number;
  currentValue: number;
  previousValue: number;
  targetValue: number;
  changePercent: number;
  trend: string;
  status: string;
  lastUpdated: string;
}

export interface ReportRequest {
  hotelId: number;
  reportType: string;
  startDate: string;
  endDate: string;
  format: string;
}

export interface ReportDto {
  id: number;
  name: string;
  description: string;
  reportType: string;
  format: string;
  hotelId: number;
  startDate: string;
  endDate: string;
  generatedAt: string;
  fileSize: number;
  status: string;
  generatedByUserName: string;
}

export interface ScheduledReport {
  id: number;
  name: string;
  reportId: number;
  frequency: string;
  dayOfWeek?: string;
  dayOfMonth?: number;
  timeOfDay: string;
  recipientEmails: string;
  format: string;
  isActive: boolean;
  lastRunAt: string;
  nextRunAt: string;
  lastRunStatus: string;
}