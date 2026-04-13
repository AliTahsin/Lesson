import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { SyncLog } from '../types/channel';

interface Props {
  log: SyncLog;
}

export const SyncStatusCard: React.FC<Props> = ({ log }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Success': return '#10b981';
      case 'Failed': return '#ef4444';
      case 'Pending': return '#f59e0b';
      default: return '#6b7280';
    }
  };

  const getSyncTypeIcon = (type: string) => {
    switch (type) {
      case 'Availability': return '📊';
      case 'Price': return '💰';
      case 'Booking': return '📅';
      default: return '🔄';
    }
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}`;
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <View style={styles.typeContainer}>
          <Text style={styles.typeIcon}>{getSyncTypeIcon(log.syncType)}</Text>
          <Text style={styles.typeText}>{log.syncType}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(log.status) }]}>
          <Text style={styles.statusText}>{log.status}</Text>
        </View>
      </View>
      
      <View style={styles.stats}>
        <View style={styles.stat}>
          <Text style={styles.statValue}>{log.recordsProcessed}</Text>
          <Text style={styles.statLabel}>İşlenen</Text>
        </View>
        <View style={styles.stat}>
          <Text style={[styles.statValue, { color: '#10b981' }]}>{log.recordsSuccess}</Text>
          <Text style={styles.statLabel}>Başarılı</Text>
        </View>
        <View style={styles.stat}>
          <Text style={[styles.statValue, { color: '#ef4444' }]}>{log.recordsFailed}</Text>
          <Text style={styles.statLabel}>Başarısız</Text>
        </View>
      </View>
      
      <View style={styles.footer}>
        <Text style={styles.time}>
          {formatDate(log.startTime)} {formatTime(log.startTime)}
        </Text>
        {log.errorMessage && (
          <Text style={styles.error} numberOfLines={1}>{log.errorMessage}</Text>
        )}
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    elevation: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  typeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
  },
  typeIcon: {
    fontSize: 16,
  },
  typeText: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 3,
    borderRadius: 6,
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
    paddingVertical: 8,
    borderTopWidth: 1,
    borderBottomWidth: 1,
    borderColor: '#f0f0f0',
  },
  stat: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  statLabel: {
    fontSize: 10,
    color: '#888',
    marginTop: 2,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  time: {
    fontSize: 11,
    color: '#888',
  },
  error: {
    fontSize: 11,
    color: '#ef4444',
    flex: 1,
    textAlign: 'right',
  },
});