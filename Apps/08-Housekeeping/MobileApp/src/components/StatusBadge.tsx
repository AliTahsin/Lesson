import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Props {
  status: string;
  type?: 'task' | 'issue';
}

export const StatusBadge: React.FC<Props> = ({ status, type = 'task' }) => {
  const getStatusConfig = () => {
    switch (status) {
      case 'Pending':
        return { color: '#f59e0b', text: 'Beklemede', icon: '⏳' };
      case 'Assigned':
        return { color: '#3b82f6', text: 'Atandı', icon: '👤' };
      case 'InProgress':
        return { color: '#8b5cf6', text: 'Devam Ediyor', icon: '🔄' };
      case 'Completed':
        return { color: '#10b981', text: 'Tamamlandı', icon: '✅' };
      case 'Cancelled':
        return { color: '#ef4444', text: 'İptal Edildi', icon: '❌' };
      case 'Reported':
        return { color: '#f59e0b', text: 'Bildirildi', icon: '📢' };
      case 'Resolved':
        return { color: '#10b981', text: 'Çözüldü', icon: '🔧' };
      case 'Closed':
        return { color: '#6b7280', text: 'Kapandı', icon: '📁' };
      default:
        return { color: '#6b7280', text: status, icon: '📌' };
    }
  };

  const config = getStatusConfig();

  return (
    <View style={[styles.badge, { backgroundColor: config.color }]}>
      <Text style={styles.icon}>{config.icon}</Text>
      <Text style={styles.text}>{config.text}</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 12,
    gap: 4,
  },
  icon: {
    fontSize: 12,
    color: 'white',
  },
  text: {
    fontSize: 11,
    fontWeight: 'bold',
    color: 'white',
  },
});