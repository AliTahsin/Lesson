import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { RequestStats } from '../types/gateway';

interface Props {
  stats: RequestStats;
}

export const RequestStatsCard: React.FC<Props> = ({ stats }) => {
  return (
    <View style={styles.card}>
      <Text style={styles.title}>İstatistikler</Text>

      <View style={styles.statsGrid}>
        <View style={styles.statItem}>
          <Icon name="swap-horizontal-outline" size={24} color="#007AFF" />
          <Text style={styles.statValue}>{stats.totalRequests.toLocaleString()}</Text>
          <Text style={styles.statLabel}>Toplam İstek</Text>
        </View>
        <View style={styles.statItem}>
          <Icon name="time-outline" size={24} color="#f59e0b" />
          <Text style={styles.statValue}>{stats.averageResponseTime}ms</Text>
          <Text style={styles.statLabel}>Ort. Yanıt Süresi</Text>
        </View>
        <View style={styles.statItem}>
          <Icon name="alert-circle-outline" size={24} color="#ef4444" />
          <Text style={styles.statValue}>{stats.errorRate}%</Text>
          <Text style={styles.statLabel}>Hata Oranı</Text>
        </View>
      </View>

      <View style={styles.chartContainer}>
        <Text style={styles.chartTitle}>İstek Dağılımı</Text>
        <View style={styles.barsContainer}>
          {stats.requestsByMinute.map((item, index) => (
            <View key={index} style={styles.barWrapper}>
              <View style={[styles.bar, { height: Math.min(100, item.count / 5) }]} />
              <Text style={styles.barLabel}>{item.minute}</Text>
            </View>
          ))}
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 16,
  },
  statsGrid: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 20,
  },
  statItem: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 8,
  },
  statLabel: {
    fontSize: 11,
    color: '#888',
    marginTop: 4,
  },
  chartContainer: {
    marginTop: 8,
  },
  chartTitle: {
    fontSize: 14,
    fontWeight: '500',
    color: '#666',
    marginBottom: 12,
  },
  barsContainer: {
    flexDirection: 'row',
    alignItems: 'flex-end',
    justifyContent: 'space-between',
    height: 100,
  },
  barWrapper: {
    alignItems: 'center',
    flex: 1,
  },
  bar: {
    width: 20,
    backgroundColor: '#007AFF',
    borderRadius: 4,
    marginBottom: 8,
  },
  barLabel: {
    fontSize: 10,
    color: '#888',
  },
});