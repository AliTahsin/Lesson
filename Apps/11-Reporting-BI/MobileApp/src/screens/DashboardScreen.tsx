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
import Icon from 'react-native-vector-icons/Ionicons';
import { reportApi } from '../services/reportApi';
import { KpiCard } from '../components/KpiCard';
import { RevenueChart } from '../components/RevenueChart';
import { OccupancyChart } from '../components/OccupancyChart';
import { KPI, RevenueReport, OccupancyReport } from '../types/reporting';

export const DashboardScreen = ({ route }: any) => {
  const { hotelId } = route.params;
  const [kpis, setKpis] = useState<KPI[]>([]);
  const [revenueReport, setRevenueReport] = useState<RevenueReport | null>(null);
  const [occupancyReport, setOccupancyReport] = useState<OccupancyReport | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [selectedPeriod, setSelectedPeriod] = useState<'week' | 'month' | 'quarter'>('month');

  const getDateRange = () => {
    const endDate = new Date();
    let startDate = new Date();
    
    switch (selectedPeriod) {
      case 'week':
        startDate.setDate(endDate.getDate() - 7);
        break;
      case 'month':
        startDate.setMonth(endDate.getMonth() - 1);
        break;
      case 'quarter':
        startDate.setMonth(endDate.getMonth() - 3);
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
      const [kpisData, revenueData, occupancyData] = await Promise.all([
        reportApi.getKPIs(hotelId),
        reportApi.getRevenueReport(hotelId, startDate, endDate),
        reportApi.getOccupancyReport(hotelId, startDate, endDate)
      ]);
      setKpis(kpisData);
      setRevenueReport(revenueData);
      setOccupancyReport(occupancyData);
    } catch (error) {
      console.error('Error loading dashboard:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadDashboard();
  }, [selectedPeriod]);

  const onRefresh = () => {
    setRefreshing(true);
    loadDashboard();
  };

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
          style={[styles.periodButton, selectedPeriod === 'week' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('week')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'week' && styles.periodTextActive]}>
            Haftalık
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.periodButton, selectedPeriod === 'month' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('month')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'month' && styles.periodTextActive]}>
            Aylık
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.periodButton, selectedPeriod === 'quarter' && styles.periodButtonActive]}
          onPress={() => setSelectedPeriod('quarter')}
        >
          <Text style={[styles.periodText, selectedPeriod === 'quarter' && styles.periodTextActive]}>
            3 Aylık
          </Text>
        </TouchableOpacity>
      </View>

      <View style={styles.kpiGrid}>
        {kpis.map((kpi) => (
          <KpiCard key={kpi.id} kpi={kpi} />
        ))}
      </View>

      {revenueReport && revenueReport.dailyData.length > 0 && (
        <RevenueChart data={revenueReport.dailyData} />
      )}

      {occupancyReport && occupancyReport.occupancyData.length > 0 && (
        <OccupancyChart data={occupancyReport.occupancyData} />
      )}

      <View style={styles.quickLinks}>
        <TouchableOpacity
          style={styles.quickLink}
          onPress={() => {}}
        >
          <Icon name="bar-chart" size={24} color="#007AFF" />
          <Text style={styles.quickLinkText}>Detaylı Raporlar</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.quickLink}
          onPress={() => {}}
        >
          <Icon name="download" size={24} color="#007AFF" />
          <Text style={styles.quickLinkText}>Rapor İndir</Text>
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
    justifyContent: 'space-around',
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  periodButton: {
    paddingHorizontal: 20,
    paddingVertical: 8,
    borderRadius: 20,
  },
  periodButtonActive: {
    backgroundColor: '#007AFF',
  },
  periodText: {
    color: '#666',
    fontWeight: '500',
  },
  periodTextActive: {
    color: 'white',
  },
  kpiGrid: {
    paddingVertical: 8,
  },
  quickLinks: {
    flexDirection: 'row',
    margin: 16,
    gap: 12,
  },
  quickLink: {
    flex: 1,
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
    flexDirection: 'row',
    justifyContent: 'center',
    gap: 8,
    elevation: 2,
  },
  quickLinkText: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
});