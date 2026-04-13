import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { useStaffAuthContext } from '../context/StaffAuthContext';

interface Props {
  children: React.ReactNode;
  allowedRoles: string | string[];
  fallback?: React.ReactNode;
}

export const RoleBasedRoute: React.FC<Props> = ({ children, allowedRoles, fallback }) => {
  const { hasRole, loading } = useStaffAuthContext();

  if (loading) {
    return (
      <View style={styles.center}>
        <Text>Yükleniyor...</Text>
      </View>
    );
  }

  if (!hasRole(allowedRoles)) {
    return fallback ? (
      <>{fallback}</>
    ) : (
      <View style={styles.center}>
        <Text style={styles.permissionText}>Bu sayfaya erişim yetkiniz yok</Text>
      </View>
    );
  }

  return <>{children}</>;
};

const styles = StyleSheet.create({
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  permissionText: {
    fontSize: 14,
    color: '#ef4444',
  },
});