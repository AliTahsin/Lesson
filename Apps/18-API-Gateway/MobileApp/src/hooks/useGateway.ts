import { useState, useEffect, useCallback } from 'react';
import { gatewayApi } from '../services/gatewayApi';
import { Service, ServiceHealthCheck, GatewayInfo, RequestStats } from '../types/gateway';

interface UseGatewayReturn {
  services: Service[];
  gatewayInfo: GatewayInfo | null;
  stats: RequestStats | null;
  healthChecks: ServiceHealthCheck[];
  loading: boolean;
  refreshing: boolean;
  error: string | null;
  refresh: () => Promise<void>;
  getServiceHealth: (serviceName: string) => ServiceHealthCheck | undefined;
  getServiceByName: (serviceName: string) => Service | undefined;
  checkServiceHealth: (serviceName: string) => Promise<ServiceHealthCheck | null>;
  registerService: (service: Partial<Service>) => Promise<boolean>;
  deregisterService: (serviceName: string) => Promise<boolean>;
  sendHeartbeat: (serviceName: string) => Promise<boolean>;
}

export const useGateway = (autoRefresh: boolean = true): UseGatewayReturn => {
  const [services, setServices] = useState<Service[]>([]);
  const [gatewayInfo, setGatewayInfo] = useState<GatewayInfo | null>(null);
  const [stats, setStats] = useState<RequestStats | null>(null);
  const [healthChecks, setHealthChecks] = useState<ServiceHealthCheck[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadAllData = useCallback(async () => {
    try {
      const [servicesData, infoData, statsData, healthData] = await Promise.all([
        gatewayApi.getServices(),
        gatewayApi.getGatewayInfo(),
        gatewayApi.getRequestStats(),
        gatewayApi.getServiceHealth()
      ]);
      
      setServices(servicesData);
      setGatewayInfo(infoData);
      setStats(statsData);
      setHealthChecks(healthData);
      setError(null);
    } catch (err) {
      setError('Failed to load gateway data');
      console.error(err);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  const refresh = useCallback(async () => {
    setRefreshing(true);
    await loadAllData();
  }, [loadAllData]);

  const getServiceHealth = useCallback((serviceName: string) => {
    return healthChecks.find(h => h.serviceName === serviceName);
  }, [healthChecks]);

  const getServiceByName = useCallback((serviceName: string) => {
    return services.find(s => s.name === serviceName);
  }, [services]);

  const checkServiceHealth = useCallback(async (serviceName: string) => {
    try {
      const healthData = await gatewayApi.getServiceHealth();
      const serviceHealth = healthData.find(h => h.serviceName === serviceName);
      setHealthChecks(healthData);
      return serviceHealth || null;
    } catch (err) {
      console.error(err);
      return null;
    }
  }, []);

  const registerService = useCallback(async (service: Partial<Service>) => {
    try {
      await gatewayApi.registerService(service);
      await refresh();
      return true;
    } catch (err) {
      console.error(err);
      return false;
    }
  }, [refresh]);

  const deregisterService = useCallback(async (serviceName: string) => {
    try {
      await gatewayApi.deregisterService(serviceName);
      await refresh();
      return true;
    } catch (err) {
      console.error(err);
      return false;
    }
  }, [refresh]);

  const sendHeartbeat = useCallback(async (serviceName: string) => {
    try {
      await gatewayApi.sendHeartbeat(serviceName);
      await refresh();
      return true;
    } catch (err) {
      console.error(err);
      return false;
    }
  }, [refresh]);

  useEffect(() => {
    loadAllData();
    
    let interval: NodeJS.Timeout;
    if (autoRefresh) {
      interval = setInterval(() => {
        loadAllData();
      }, 30000); // Refresh every 30 seconds
    }
    
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [autoRefresh, loadAllData]);

  return {
    services,
    gatewayInfo,
    stats,
    healthChecks,
    loading,
    refreshing,
    error,
    refresh,
    getServiceHealth,
    getServiceByName,
    checkServiceHealth,
    registerService,
    deregisterService,
    sendHeartbeat
  };
};