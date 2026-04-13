export interface LogEntry {
  id: string;
  timestamp: string;
  level: string;
  message: string;
  exception?: string;
  sourceContext?: string;
  requestPath?: string;
  requestMethod?: string;
  statusCode?: number;
  durationMs?: number;
  userId?: string;
  correlationId?: string;
  ipAddress?: string;
}

export interface LogSearchRequest {
  level?: string;
  service?: string;
  correlationId?: string;
  startDate?: string;
  endDate?: string;
  searchText?: string;
}

export interface LogStatistics {
  totalLogs: number;
  errorCount: number;
  warningCount: number;
  infoCount: number;
  debugCount: number;
  averageResponseTime: number;
  startDate: string;
  endDate: string;
}

export interface MetricsSummary {
  totalRequests: number;
  errorRate: number;
  averageResponseTime: number;
  requestsPerMinute: number;
  timestamp: string;
}

export interface ServiceMetrics {
  serviceName: string;
  totalRequests: number;
  errorCount: number;
  averageResponseTime: number;
  lastActivity: string;
  status: string;
}

export interface Trace {
  traceId: string;
  spanId: string;
  parentSpanId?: string;
  operationName: string;
  startTime: string;
  durationMs: number;
  tags: Record<string, string>;
  service: string;
  endpoint: string;
}

export interface TraceStatistics {
  totalTraces: number;
  averageDurationMs: number;
  maxDurationMs: number;
  minDurationMs: number;
  slowTracesCount: number;
  startDate: string;
  endDate: string;
}

export interface Alert {
  id: string;
  name: string;
  severity: string;
  message: string;
  condition: string;
  currentValue: number;
  threshold: number;
  service: string;
  triggeredAt: string;
  resolvedAt?: string;
  isResolved: boolean;
}