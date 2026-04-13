import React, { createContext, useContext, ReactNode } from 'react';
import { useGateway } from '../hooks/useGateway';
import { Service, ServiceHealthCheck, GatewayInfo, RequestStats } from '../types/gateway';

interface GatewayContextType {
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

const GatewayContext = createContext<GatewayContextType | undefined>(undefined);

interface GatewayProviderProps {
  children: ReactNode;
  autoRefresh?: boolean;
}

export const GatewayProvider: React.FC<GatewayProviderProps> = ({ 
  children, 
  autoRefresh = true 
}) => {
  const gateway = useGateway(autoRefresh);

  return (
    <GatewayContext.Provider value={gateway}>
      {children}
    </GatewayContext.Provider>
  );
};

export const useGatewayContext = () => {
  const context = useContext(GatewayContext);
  if (context === undefined) {
    throw new Error('useGatewayContext must be used within a GatewayProvider');
  }
  return context;
};