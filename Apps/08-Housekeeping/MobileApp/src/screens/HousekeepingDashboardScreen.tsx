import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  RefreshControl
} from 'react-native';
import { taskApi } from '../services/taskApi';
import { DashboardStats } from '../types/housekeeping';

export const HousekeepingDashboardScreen = ({ route }: any) => {
  const { hotelId } = route.params;
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    setLoading(true);
    try {
      const data = await taskApi.getDashboardStats(hotelId);
      setStats(data);
    } catch (error) {
      console.error('Error loading stats:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadStats();
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!stats) {
    return (
      <View style={styles.center}>
        <Text>İstatistikler yüklenemedi</Text>
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
      <View style={styles.statsGrid}>
        <View style={[styles.statCard, styles.primaryCard]}>
          <Text style={styles.statValue}>{stats.totalTasksToday}</Text>
          <Text style={styles.statLabel}>Bugünkü Görev</Text>
        </View>
        <View style={[styles.statCard, styles.warningCard]}>
          <Text style={styles.statValue}>{stats.pendingTasks}</Text>
          <Text style={styles.statLabel}>Bekleyen Görev</Text>
        </View>
        <View style={[styles.statCard, styles.infoCard]}>
          <Text style={styles.statValue}>{stats.inProgressTasks}</Text>
          <Text style={styles.statLabel}>Devam Eden</Text>
        </View>
        <View style={[styles.statCard, styles.successCard]}>
          <Text style={styles.statValue}>{stats.completedToday}</Text>
          <Text style={styles.statLabel}>Tamamlanan</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Görev İstatistikleri</Text>
        <View style={styles.progressContainer}>
          <View style={styles.progressLabel}>
            <Text>Tamamlanma Oranı</Text>
            <Text>{stats.completionRate.toFixed(1)}%</Text>
          </View>
          <View style={styles.progressBar}>
            <View style={[styles.progressFill, { width: `${stats.completionRate}%` }]} />
          </View>
        </View>
        <View style={styles.statRow}>
          <Text style={styles.statRowLabel}>Ortalama Görev Süresi:</Text>
          <Text style={styles.statRowValue}>{stats.averageTaskTime.toFixed(0)} dk</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Arıza İstatistikleri</Text>
        <View style={styles.statRow}>
          <Text style={styles.statRowLabel}>Kritik Arızalar:</Text>
          <Text style={[styles.statRowValue, styles.criticalValue]}>{stats.criticalIssues}</Text>
        </View>
        <View style={styles.statRow}>
          <Text style={styles.statRowLabel}>Açık Arızalar:</Text>
          <Text style={styles.statRowValue}>{stats.openIssues}</Text>
        </View>
        <View style={styles.statRow}>
          <Text style={styles.statRowLabel}>Ortalama Çözüm Süresi:</Text>
          <Text style={styles.statRowValue}>{stats.averageResolutionTime} saat</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Görev Türleri</Text>
        {Object.entries(stats.tasksByType).map(([type, count]) => (
          <View key={type} style={styles.statRow}>
            <Text style={styles.statRowLabel}>{type}:</Text>
            <Text style={styles.statRowValue}>{count}</Text>
          </View>
        ))}
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Arıza Kategorileri</Text>
        {Object.entries(stats.issuesByCategory).map(([category, count]) => (
          <View key={category} style={styles.statRow}>
            <Text style={styles.statRowLabel}>{category}:</Text>
            <Text style={styles.statRowValue}>{count}</Text>
          </View>
        ))}
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
  statsGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    padding: 12,
    gap: 12,
  },
  statCard: {
    flex: 1,
    minWidth: '45%',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
  },
  primaryCard: {
    backgroundColor: '#007AFF',
  },
  warningCard: {
    backgroundColor: '#f59e0b',
  },
  infoCard: {
    backgroundColor: '#8b5cf6',
  },
  successCard: {
    backgroundColor: '#10b981',
  },
  statValue: {
    fontSize: 28,
    fontWeight: 'bold',
    color: 'white',
  },
  statLabel: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  progressContainer: {
    marginBottom: 12,
  },
  progressLabel: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  progressBar: {
    height: 8,
    backgroundColor: '#e0e0e0',
    borderRadius: 4,
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#007AFF',
    borderRadius: 4,
  },
  statRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  statRowLabel: {
    fontSize: 14,
    color: '#666',
  },
  statRowValue: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  criticalValue: {
    color: '#ef4444',
    fontWeight: 'bold',
  },
});