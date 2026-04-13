import React, { createContext, useContext, ReactNode } from 'react';
import { useStaffAuth } from '../hooks/useStaffAuth';
import { Staff } from '../types/staff';

interface StaffAuthContextType {
  staff: Staff | null;
  role: string | null;
  hotelId: number | null;
  loading: boolean;
  isAuthenticated: boolean;
  login: (data: any) => Promise<any>;
  logout: () => Promise<void>;
  hasRole: (role: string | string[]) => boolean;
}

const StaffAuthContext = createContext<StaffAuthContextType | undefined>(undefined);

export const StaffAuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const auth = useStaffAuth();

  return (
    <StaffAuthContext.Provider value={auth}>
      {children}
    </StaffAuthContext.Provider>
  );
};

export const useStaffAuthContext = () => {
  const context = useContext(StaffAuthContext);
  if (context === undefined) {
    throw new Error('useStaffAuthContext must be used within a StaffAuthProvider');
  }
  return context;
};