import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Props {
  role: string;
  size?: 'small' | 'medium';
}

export const RoleBadge: React.FC<Props> = ({ role, size = 'medium' }) => {
  const getRoleColor = (role: string) => {
    switch (role.toLowerCase()) {
      case 'super admin':
        return '#ef4444';
      case 'hotel manager':
        return '#f59e0b';
      case 'front desk staff':
        return '#3b82f6';
      case 'housekeeping staff':
        return '#10b981';
      case 'guest':
        return '#6b7280';
      default:
        return '#8b5cf6';
    }
  };

  const getRoleIcon = (role: string) => {
    switch (role.toLowerCase()) {
      case 'super admin':
        return '👑';
      case 'hotel manager':
        return '🏨';
      case 'front desk staff':
        return '📋';
      case 'housekeeping staff':
        return '🧹';
      case 'guest':
        return '👤';
      default:
        return '⭐';
    }
  };

  const fontSize = size === 'small' ? 10 : 12;
  const padding = size === 'small' ? 4 : 6;

  return (
    <View style={[styles.badge, { backgroundColor: getRoleColor(role), paddingHorizontal: padding * 2, paddingVertical: padding }]}>
      <Text style={styles.icon}>{getRoleIcon(role)}</Text>
      <Text style={[styles.text, { fontSize }]}>{role}</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 12,
    gap: 4,
  },
  icon: {
    fontSize: 12,
  },
  text: {
    color: 'white',
    fontWeight: 'bold',
  },
});