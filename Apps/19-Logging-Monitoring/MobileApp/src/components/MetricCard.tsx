import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { ServiceMetrics } from '../types/logging';

interface Props {
  metric: ServiceMetrics;
}

export const MetricCard: React.FC<Props> = ({ metric }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Healthy': return '#10b981';
      case 'Degraded': return '#f59e0b';
      case 'Down': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Healthy': return 'checkmark-circle';
      case 'Degraded': return 'alert-circle';
      case 'Down': return 'close-circle';
      default: return 'help-circle';
    }
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    
    if (diffMins < 1) return 'Şimdi';
    if (diffMins < 60) return `${diffMins} dakika önce`;
    return `${Math.floor(diffMins / 60)} saat önce`;
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <Text style={styles.serviceName}>{metric.serviceName}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(metric.status) }]}>
          <Icon name={getStatusIcon(metric.status)} size={12} color="white" />
          <Text style={styles.statusText}>{metric.status}</Text>
        </View>
      </View>

      <View style={styles.stats}>
        <View style={styles.statItem}>
          <Text style={styles.statValue}>{metric.totalRequests.toLocaleString()}</Text>
          <Text style={styles.statLabel}>Toplam İstek</Text>
        </View>
        <View style={styles.statItem}>
          <Text style={[styles.statValue, { color: '#ef4444' }]}>{metric.errorCount}</Text>
          <Text style={styles.statLabel}>Hata</Text>
        </View>
        <View style={styles.statItem}>
          <Text style={styles.statValue}>{metric.averageResponseTime}ms</Text>
          <Text style={styles.statLabel}>Ort. Yanıt</Text>
        </View>
      </View>

      <Text style={styles.lastActivity}>
        Son Aktivite: {formatTime(metric.lastActivity)}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    padding: 16,
    elevation: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  serviceName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
    gap: 4,
  },
  statusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  stats: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 12,
  },
  statItem: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  statLabel: {
    fontSize: 11,
    color: '#888',
    marginTop: 4,
  },
  lastActivity: {
    fontSize: 11,
    color: '#888',
    textAlign: 'center',
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
});