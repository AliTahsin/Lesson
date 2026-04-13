import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { gatewayApi } from '../services/gatewayApi';
import { ServiceCard } from '../components/ServiceCard';
import { RequestStatsCard } from '../components/RequestStatsCard';
import { Service, GatewayInfo, RequestStats } from '../types/gateway';

export const GatewayStatusScreen = ({ navigation }: any) => {
  const [services, setServices] = useState<Service[]>([]);
  const [gatewayInfo, setGatewayInfo] = useState<GatewayInfo | null>(null);
  const [stats, setStats] = useState<RequestStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  useEffect(() => {
    loadData();
    const interval = setInterval(() => {
      loadServices();
    }, 30000); // Refresh every 30 seconds
    return () => clearInterval(interval);
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      await Promise.all([
        loadServices(),
        loadGatewayInfo(),
        loadStats()
      ]);
    } catch (error) {
      console.error('Error loading gateway data:', error);
      Alert.alert('Hata', 'Gateway verileri yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const loadServices = async () => {
    try {
      const servicesData = await gatewayApi.getServices();
      setServices(servicesData);
    } catch (error) {
      console.error('Error loading services:', error);
    }
  };

  const loadGatewayInfo = async () => {
    try {
      const info = await gatewayApi.getGatewayInfo();
      setGatewayInfo(info);
    } catch (error) {
      console.error('Error loading gateway info:', error);
    }
  };

  const loadStats = async () => {
    try {
      const statsData = await gatewayApi.getRequestStats();
      setStats(statsData);
    } catch (error) {
      console.error('Error loading stats:', error);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadData();
  };

  const handleServicePress = (service: Service) => {
    navigation.navigate('ServiceDetail', { service });
  };

  const healthyCount = services.filter(s => s.isHealthy).length;
  const totalCount = services.length;
  const healthPercentage = totalCount > 0 ? (healthyCount / totalCount) * 100 : 0;

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
      {/* Gateway Header */}
      {gatewayInfo && (
        <View style={styles.headerCard}>
          <Text style={styles.gatewayName}>{gatewayInfo.name}</Text>
          <Text style={styles.gatewayVersion}>v{gatewayInfo.version}</Text>
          <View style={styles.healthSummary}>
            <View style={styles.healthCircle}>
              <Text style={styles.healthPercent}>{healthPercentage.toFixed(0)}%</Text>
            </View>
            <Text style={styles.healthText}>
              {healthyCount}/{totalCount} Servis Aktif
            </Text>
          </View>
          <Text style={styles.timestamp}>
            Son Güncelleme: {new Date(gatewayInfo.timestamp).toLocaleTimeString()}
          </Text>
        </View>
      )}

      {/* Request Stats */}
      {stats && <RequestStatsCard stats={stats} />}

      {/* Services List */}
      <Text style={styles.sectionTitle}>Mikroservisler</Text>
      {services.map((service) => (
        <ServiceCard
          key={service.id}
          service={service}
          onPress={handleServicePress}
        />
      ))}
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
  headerCard: {
    backgroundColor: '#007AFF',
    borderRadius: 16,
    margin: 16,
    padding: 20,
    alignItems: 'center',
  },
  gatewayName: {
    fontSize: 22,
    fontWeight: 'bold',
    color: 'white',
  },
  gatewayVersion: {
    fontSize: 14,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  healthSummary: {
    alignItems: 'center',
    marginVertical: 16,
  },
  healthCircle: {
    width: 80,
    height: 80,
    borderRadius: 40,
    backgroundColor: 'white',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 8,
  },
  healthPercent: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  healthText: {
    fontSize: 14,
    color: 'white',
  },
  timestamp: {
    fontSize: 11,
    color: 'white',
    opacity: 0.7,
    marginTop: 8,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginHorizontal: 16,
    marginTop: 8,
    marginBottom: 8,
  },
});