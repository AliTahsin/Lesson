export interface Service {
  id: number;
  name: string;
  url: string;
  port: number;
  isHealthy: boolean;
  lastHeartbeat: string;
  registeredAt: string;
}

export interface ServiceHealthCheck {
  serviceName: string;
  isHealthy: boolean;
  responseTime: number;
  checkedAt: string;
  errorMessage?: string;
}

export interface RouteInfo {
  path: string;
  service: string;
}

export interface GatewayInfo {
  name: string;
  version: string;
  services: string[];
  timestamp: string;
}

export interface RequestStats {
  totalRequests: number;
  averageResponseTime: number;
  errorRate: number;
  requestsByMinute: { minute: string; count: number }[];
}

export interface RateLimitInfo {
  isLimited: boolean;
  remaining: number;
  resetAt: string;
  limit: number;
}