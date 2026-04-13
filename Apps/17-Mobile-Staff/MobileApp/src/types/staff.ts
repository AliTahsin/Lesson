export interface Staff {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  role: string;
  department: string;
  position: string;
  hotelId: number;
  isActive: boolean;
  lastLoginAt?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  staff: Staff;
  role: string;
}

export interface StaffTask {
  id: number;
  taskNumber: string;
  hotelId: number;
  roomId: number;
  roomNumber: string;
  taskType: string;
  priority: string;
  description: string;
  assignedToStaffId?: number;
  assignedToStaffName?: string;
  createdAt: string;
  scheduledDate?: string;
  startedAt?: string;
  completedAt?: string;
  status: string;
  notes?: string;
  estimatedMinutes: number;
  actualMinutes: number;
}

export interface MaintenanceIssue {
  id: number;
  issueNumber: string;
  hotelId: number;
  roomId: number;
  roomNumber: string;
  category: string;
  description: string;
  priority: string;
  reportedByName: string;
  reportedAt: string;
  assignedToStaffName?: string;
  assignedAt?: string;
  resolvedAt?: string;
  status: string;
  resolutionNotes?: string;
  estimatedCost: number;
  actualCost: number;
}

export interface CheckInOut {
  id: number;
  reservationId: number;
  guestId: number;
  guestName: string;
  roomId: number;
  roomNumber: string;
  processedByStaffName: string;
  type: string;
  processedAt: string;
  notes?: string;
  digitalKey?: string;
}

export interface DashboardStats {
  totalTasksToday: number;
  pendingTasks: number;
  inProgressTasks: number;
  completedToday: number;
  completionRate: number;
  criticalIssues: number;
  openIssues: number;
  todayCheckIns: number;
  todayCheckOuts: number;
  tasksByType: Record<string, number>;
  issuesByCategory: Record<string, number>;
}

export interface Notification {
  id: number;
  staffId: number;
  title: string;
  body: string;
  type: string;
  relatedId?: number;
  isRead: boolean;
  createdAt: string;
  readAt?: string;
}

export interface CreateTaskDto {
  hotelId: number;
  roomId: number;
  roomNumber: string;
  taskType: string;
  priority: string;
  description: string;
  scheduledDate?: string;
  estimatedMinutes?: number;
  notes?: string;
}

export interface CreateIssueDto {
  hotelId: number;
  roomId: number;
  roomNumber: string;
  category: string;
  description: string;
  priority: string;
  reportedByStaffId?: number;
  reportedByName: string;
  images?: string[];
}

export interface CheckInDto {
  reservationId: number;
  guestId: number;
  guestName: string;
  roomId: number;
  roomNumber: string;
  hotelId: number;
  notes?: string;
}

export interface CheckOutDto {
  reservationId: number;
  guestId: number;
  guestName: string;
  roomId: number;
  roomNumber: string;
  hotelId: number;
  notes?: string;
}