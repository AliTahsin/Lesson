export interface HousekeepingTask {
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

export interface DashboardStats {
  totalTasksToday: number;
  pendingTasks: number;
  inProgressTasks: number;
  completedToday: number;
  completionRate: number;
  averageTaskTime: number;
  criticalIssues: number;
  openIssues: number;
  averageResolutionTime: number;
  tasksByType: Record<string, number>;
  issuesByCategory: Record<string, number>;
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