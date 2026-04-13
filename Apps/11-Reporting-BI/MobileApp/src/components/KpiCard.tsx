import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { KPI } from '../types/reporting';

interface Props {
  kpi: KPI;
}

export const KpiCard: React.FC<Props> = ({ kpi }) => {
  const getTrendIcon = (trend: string) => {
    switch (trend) {
      case 'Up': return 'arrow-up';
      case 'Down': return 'arrow-down';
      default: return 'remove';
    }
  };

  const getTrendColor = (trend: string, changePercent: number) => {
    if (trend === 'Up') return changePercent > 0 ? '#10b981' : '#ef4444';
    if (trend === 'Down') return changePercent < 0 ? '#10b981' : '#ef4444';
    return '#f59e0b';
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'OnTrack': return '#10b981';
      case 'Warning': return '#f59e0b';
      case 'Critical': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const formatValue = (code: string, value: number) => {
    if (code === 'OCCUPANCY' || code === 'CSAT') return `${value.toFixed(1)}%`;
    if (code === 'REVPAR' || code === 'ADR') return `€${value.toFixed(2)}`;
    return value.toFixed(0);
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <Text style={styles.name}>{kpi.name}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(kpi.status) }]}>
          <Text style={styles.statusText}>{kpi.status}</Text>
        </View>
      </View>
      
      <Text style={styles.value}>{formatValue(kpi.code, kpi.currentValue)}</Text>
      
      <View style={styles.footer}>
        <View style={styles.targetContainer}>
          <Text style={styles.targetLabel}>Hedef:</Text>
          <Text style={styles.targetValue}>{formatValue(kpi.code, kpi.targetValue)}</Text>
        </View>
        
        <View style={styles.changeContainer}>
          <Icon 
            name={getTrendIcon(kpi.trend)} 
            size={16} 
            color={getTrendColor(kpi.trend, kpi.changePercent)} 
          />
          <Text style={[styles.changeText, { color: getTrendColor(kpi.trend, kpi.changePercent) }]}>
            {Math.abs(kpi.changePercent).toFixed(1)}%
          </Text>
        </View>
      </View>
      
      <Text style={styles.lastUpdated}>
        Son güncelleme: {new Date(kpi.lastUpdated).toLocaleDateString('tr-TR')}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginHorizontal: 16,
    marginVertical: 8,
    elevation: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  name: {
    fontSize: 14,
    fontWeight: '600',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  value: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#007AFF',
    marginBottom: 12,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  targetContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  targetLabel: {
    fontSize: 12,
    color: '#888',
    marginRight: 4,
  },
  targetValue: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  changeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  changeText: {
    fontSize: 14,
    fontWeight: '500',
    marginLeft: 4,
  },
  lastUpdated: {
    fontSize: 10,
    color: '#aaa',
    marginTop: 8,
  },
});