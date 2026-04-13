import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TouchableOpacity
} from 'react-native';
import { VictoryPie, VictoryBar, VictoryChart, VictoryAxis } from 'victory-native';
import { loggingApi } from '../services/loggingApi';
import { useStaffAuthContext } from '../../../17-Mobile-Staff/MobileApp/src/context/StaffAuthContext';
import { MetricCard } from '../components/MetricCard';
import { LogStatistics, MetricsSummary, ServiceMetrics } from '../types/logging';

export const LogDashboardScreen = ({ navigation }: any) => {
  const { hasRole } = useStaffAuthContext();
  const [logStats, setLogStats] = useState<LogStatistics | null>(null);
  const [metricsSummary, setMetricsSummary] = useState<MetricsSummary | null>(null);
  const [serviceMetrics, setServiceMetrics] = useState<ServiceMetrics[]>([]);
  const [levelCounts, setLevelCounts] = useState<Record<string, number>>({});
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [selectedPeriod, setSelectedPeriod] = useState<'day' | 'week' | 'month'>('day');

  useEffect(() => {
    loadDashboard();
  }, [selectedPeriod]);

  const getDateRange = () => {
    const endDate = new Date();
    let startDate = new Date();
    
    switch (selectedPeriod) {
      case 'day':
        startDate.setDate(endDate.getDate() - 1);
        break;
      case 'week':
        startDate.setDate(endDate.getDate() - 7);
        break;
      case 'month':
        startDate.setMonth(endDate.getMonth() - 1);
        break;
    }
    
    return {
      startDate: startDate.toISOString().split('T')[0],
      endDate: endDate.toISOString().split('T')[0]
    };
  };

  const loadDashboard = async () => {
    const { startDate, endDate } = getDateRange();
    
    setLoading(true);
    try {
      const [stats, metrics, services, levels] = await Promise.all([
        loggingApi.getLogStatistics(startDate, endDate),
        loggingApi.getMetricsSummary(),
        loggingApi.getAllServicesMetrics(),
        loggingApi.getLogCountByLevel(startDate, endDate)
      ]);
      setLogStats(stats);
      setMetricsSummary(metrics);
      setServiceMetrics(services);
      setLevelCounts(levels);
    } catch (error) {
      console.error('Error loading dashboard:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadDashboard();
  };

  const pieData = Object.entries(levelCounts).map(([level, count]) => ({
    x: level,
    y: count,
    label: `${level}: ${count}`
  }));

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView
      style={styles.container}
      refreshControl={
        <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
      }
    >
      <View style={styles.periodSelector}>
        <TouchableOpacity
          style={[styles.periodButton, selectedPeriod === 'day' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('day')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'day' && styles.periodTextActive]}>Son 1 Gün</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.periodButton, selectedPeriod === 'week' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('week')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'week' && styles.periodTextActive]}>Son 7 Gün</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.periodButton, selectedPeriod === 'month' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('month')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'month' && styles.periodTextActive]}>Son 30 Gün</Text>
        </TouchableOpacity>
      </View>

      {/* Metrics Summary Cards */}
      {metricsSummary && (
        <View style={styles.summaryGrid}>
          <View style={styles.summaryCard}>
            <Text style={styles.summaryValue}>{metricsSummary.totalRequests.toLocaleString()}</Text>
            <Text style={styles.summaryLabel}>Toplam İstek</Text>
          </View>
          <View style={[styles.summaryCard, styles.errorCard]}>
            <Text style={styles.summaryValue}>{metricsSummary.errorRate.toFixed(1)}%</Text>
            <Text style={styles.summaryLabel}>Hata Oranı</Text>
          </View>
          <View style={styles.summaryCard}>
            <Text style={styles.summaryValue}>{metricsSummary.averageResponseTime}ms</Text>
            <Text style={styles.summaryLabel}>Ort. Yanıt</Text>
          </View>
          <View style={styles.summaryCard}>
            <Text style={styles.summaryValue}>{metricsSummary.requestsPerMinute.toFixed(1)}</Text>
            <Text style={styles.summaryLabel}>İstek/dk</Text>
          </View>
        </View>
      )}

      {/* Log Level Distribution Chart */}
      {pieData.length > 0 && (
        <View style={styles.chartCard}>
          <Text style={styles.chartTitle}>Log Dağılımı</Text>
          <VictoryPie
            data={pieData}
            width={350}
            height={250}
            colorScale={['#10b981', '#f59e0b', '#ef4444', '#6b7280']}
            labels={({ datum }) => `${datum.x}: ${datum.y}`}
            labelRadius={({ innerRadius }) => innerRadius + 50}
            style={{
              labels: { fontSize: 10, fill: '#333' }
            }}
          />
        </View>
      )}

      {/* Service Metrics */}
      <Text style={styles.sectionTitle}>Servis Metrikleri</Text>
      {serviceMetrics.map((metric) => (
        <MetricCard key={metric.serviceName} metric={metric} />
      ))}

      {/* Quick Actions */}
      <View style={styles.quickActions}>
        <TouchableOpacity
          style={styles.quickButton}
          onPress={() => navigation.navigate('LogList')}
        >
          <Text style={styles.quickButtonText}>Tüm Loglar</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.quickButton, styles.errorButton]}
          onPress={() => navigation.navigate('LogList', { filter: 'Error' })}
        >
          <Text style={styles.quickButtonText}>Hata Logları</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.quickButton}
          onPress={() => navigation.navigate('Traces')}
        >
          <Text style={styles.quickButtonText}>Trace'ler</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
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
  periodSelector: {
    flexDirection: 'row',
    backgroundColor: 'white',
    padding: 12,
    marginBottom: 12,
    justifyContent: 'space-around',
  },
  periodButton: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 20,
  },
  periodButtonActive: {
    backgroundColor: '#007AFF',
  },
  periodText: {
    color: '#666',
    fontSize: 12,
  },
  periodTextActive: {
    color: 'white',
  },
  summaryGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    paddingHorizontal: 8,
    gap: 8,
  },
  summaryCard: {
    flex: 1,
    minWidth: '45%',
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
    elevation: 1,
  },
  errorCard: {
    backgroundColor: '#fee2e2',
  },
  summaryValue: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#333',
  },
  summaryLabel: {
    fontSize: 12,
    color: '#888',
    marginTop: 4,
  },
  chartCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    alignItems: 'center',
  },
  chartTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginHorizontal: 16,
    marginTop: 8,
    marginBottom: 8,
  },
  quickActions: {
    flexDirection: 'row',
    margin: 16,
    gap: 12,
  },
  quickButton: {
    flex: 1,
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
  },
  errorButton: {
    backgroundColor: '#ef4444',
  },
  quickButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});