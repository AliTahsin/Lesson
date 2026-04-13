import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TouchableOpacity
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { AlertCard } from '../components/AlertCard';
import { Alert } from '../types/logging';

// Mock alerts data - in real app, fetch from API
const getMockAlerts = (): Alert[] => {
  return [
    {
      id: '1',
      name: 'High Error Rate',
      severity: 'Critical',
      message: 'Error rate exceeded 5% threshold',
      condition: 'error_rate > 5',
      currentValue: 7.2,
      threshold: 5,
      service: 'API Gateway',
      triggeredAt: new Date(Date.now() - 5 * 60000).toISOString(),
      isResolved: false
    },
    {
      id: '2',
      name: 'Slow Response Time',
      severity: 'Warning',
      message: 'Response time exceeded 1s',
      condition: 'response_time > 1000',
      currentValue: 1250,
      threshold: 1000,
      service: 'ReservationSystem',
      triggeredAt: new Date(Date.now() - 15 * 60000).toISOString(),
      isResolved: false
    },
    {
      id: '3',
      name: 'Service Down',
      severity: 'Critical',
      message: 'Service is not responding',
      condition: 'up == 0',
      currentValue: 0,
      threshold: 1,
      service: 'DynamicPricing',
      triggeredAt: new Date(Date.now() - 60 * 60000).toISOString(),
      resolvedAt: new Date(Date.now() - 30 * 60000).toISOString(),
      isResolved: true
    },
    {
      id: '4',
      name: 'High Memory Usage',
      severity: 'Warning',
      message: 'Memory usage exceeded 85%',
      condition: 'memory_usage > 85',
      currentValue: 88,
      threshold: 85,
      service: 'HotelManagement',
      triggeredAt: new Date(Date.now() - 120 * 60000).toISOString(),
      isResolved: false
    },
    {
      id: '5',
      name: 'Database Connection Pool Exhausted',
      severity: 'Critical',
      message: 'Connection pool reached maximum capacity',
      condition: 'db_connections > 100',
      currentValue: 110,
      threshold: 100,
      service: 'AuthRBAC',
      triggeredAt: new Date(Date.now() - 180 * 60000).toISOString(),
      resolvedAt: new Date(Date.now() - 90 * 60000).toISOString(),
      isResolved: true
    }
  ];
};

export const AlertScreen = () => {
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<'all' | 'active' | 'resolved'>('active');

  useEffect(() => {
    loadAlerts();
  }, []);

  const loadAlerts = async () => {
    setLoading(true);
    try {
      // In real app, fetch from API
      // const data = await loggingApi.getAlerts();
      const data = getMockAlerts();
      setAlerts(data);
    } catch (error) {
      console.error('Error loading alerts:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadAlerts();
  };

  const filteredAlerts = alerts.filter(alert => {
    if (filter === 'active') return !alert.isResolved;
    if (filter === 'resolved') return alert.isResolved;
    return true;
  });

  const activeCount = alerts.filter(a => !a.isResolved).length;
  const criticalCount = alerts.filter(a => a.severity === 'Critical' && !a.isResolved).length;

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {/* Summary Cards */}
      <View style={styles.summaryContainer}>
        <View style={[styles.summaryCard, styles.activeCard]}>
          <Text style={styles.summaryCount}>{activeCount}</Text>
          <Text style={styles.summaryLabel}>Aktif Alert</Text>
        </View>
        <View style={[styles.summaryCard, styles.criticalCard]}>
          <Text style={styles.summaryCount}>{criticalCount}</Text>
          <Text style={styles.summaryLabel}>Kritik</Text>
        </View>
        <View style={[styles.summaryCard, styles.totalCard]}>
          <Text style={styles.summaryCount}>{alerts.length}</Text>
          <Text style={styles.summaryLabel}>Toplam</Text>
        </View>
      </View>

      {/* Filter Tabs */}
      <View style={styles.filterTabs}>
        <TouchableOpacity
          style={[styles.filterTab, filter === 'all' && styles.filterTabActive]}
          onPress={() => setFilter('all')}
        >
          <Text style={[styles.filterText, filter === 'all' && styles.filterTextActive]}>
            Tümü
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.filterTab, filter === 'active' && styles.filterTabActive]}
          onPress={() => setFilter('active')}
        >
          <Text style={[styles.filterText, filter === 'active' && styles.filterTextActive]}>
            Aktif
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.filterTab, filter === 'resolved' && styles.filterTabActive]}
          onPress={() => setFilter('resolved')}
        >
          <Text style={[styles.filterText, filter === 'resolved' && styles.filterTextActive]}>
            Çözülen
          </Text>
        </TouchableOpacity>
      </View>

      <FlatList
        data={filteredAlerts}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <AlertCard alert={item} />}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="checkmark-circle-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Alert bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  summaryContainer: {
    flexDirection: 'row',
    padding: 16,
    gap: 12,
  },
  summaryCard: {
    flex: 1,
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
  },
  activeCard: {
    backgroundColor: '#ef4444',
  },
  criticalCard: {
    backgroundColor: '#f59e0b',
  },
  totalCard: {
    backgroundColor: '#3b82f6',
  },
  summaryCount: {
    fontSize: 28,
    fontWeight: 'bold',
    color: 'white',
  },
  summaryLabel: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  filterTabs: {
    flexDirection: 'row',
    backgroundColor: 'white',
    paddingHorizontal: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  filterTab: {
    flex: 1,
    paddingVertical: 12,
    alignItems: 'center',
    borderBottomWidth: 2,
    borderBottomColor: 'transparent',
  },
  filterTabActive: {
    borderBottomColor: '#007AFF',
  },
  filterText: {
    fontSize: 14,
    color: '#666',
  },
  filterTextActive: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
    marginTop: 16,
  },
});