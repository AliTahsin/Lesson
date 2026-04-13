import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { gatewayApi } from '../services/gatewayApi';
import { HealthBadge } from '../components/HealthBadge';
import { Service, ServiceHealthCheck } from '../types/gateway';

export const ServiceDetailScreen = ({ route }: any) => {
  const { service } = route.params;
  const [healthCheck, setHealthCheck] = useState<ServiceHealthCheck | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadHealthCheck();
    const interval = setInterval(() => {
      loadHealthCheck();
    }, 10000); // Refresh every 10 seconds
    return () => clearInterval(interval);
  }, []);

  const loadHealthCheck = async () => {
    setLoading(true);
    try {
      const healthData = await gatewayApi.getServiceHealth();
      const serviceHealth = healthData.find(h => h.serviceName === service.name);
      setHealthCheck(serviceHealth || null);
    } catch (error) {
      console.error('Error loading health check:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDeregister = () => {
    Alert.alert(
      'Servisi Kaldır',
      `${service.name} servisini kayıttan kaldırmak istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Kaldır',
          onPress: async () => {
            try {
              await gatewayApi.deregisterService(service.name);
              Alert.alert('Başarılı', 'Servis kayıttan kaldırıldı');
            } catch (error) {
              Alert.alert('Hata', 'Servis kaldırılamadı');
            }
          }
        }
      ]
    );
  };

  const getResponseTimeColor = (ms: number) => {
    if (ms < 200) return '#10b981';
    if (ms < 500) return '#f59e0b';
    return '#ef4444';
  };

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
  };

  if (loading && !healthCheck) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.serviceName}>{service.name}</Text>
        <HealthBadge isHealthy={service.isHealthy} size="medium" />
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Servis Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>URL:</Text>
          <Text style={styles.infoValue}>{service.url}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Port:</Text>
          <Text style={styles.infoValue}>{service.port}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Kayıt Tarihi:</Text>
          <Text style={styles.infoValue}>{formatDateTime(service.registeredAt)}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Son Heartbeat:</Text>
          <Text style={styles.infoValue}>{formatDateTime(service.lastHeartbeat)}</Text>
        </View>
      </View>

      {healthCheck && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Sağlık Durumu</Text>
          <View style={styles.healthCard}>
            <View style={styles.healthRow}>
              <Text style={styles.healthLabel}>Durum:</Text>
              <HealthBadge isHealthy={healthCheck.isHealthy} size="small" />
            </View>
            <View style={styles.healthRow}>
              <Text style={styles.healthLabel}>Yanıt Süresi:</Text>
              <Text style={[styles.healthValue, { color: getResponseTimeColor(healthCheck.responseTime) }]}>
                {healthCheck.responseTime} ms
              </Text>
            </View>
            <View style={styles.healthRow}>
              <Text style={styles.healthLabel}>Son Kontrol:</Text>
              <Text style={styles.healthValue}>{formatDateTime(healthCheck.checkedAt)}</Text>
            </View>
            {healthCheck.errorMessage && (
              <View style={styles.errorContainer}>
                <Icon name="alert-circle" size={16} color="#ef4444" />
                <Text style={styles.errorText}>{healthCheck.errorMessage}</Text>
              </View>
            )}
          </View>
        </View>
      )}

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>İşlemler</Text>
        <TouchableOpacity style={styles.heartbeatButton} onPress={loadHealthCheck}>
          <Icon name="refresh" size={20} color="#007AFF" />
          <Text style={styles.heartbeatButtonText}>Sağlık Kontrolü Yap</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.deregisterButton} onPress={handleDeregister}>
          <Icon name="trash-outline" size={20} color="#ef4444" />
          <Text style={styles.deregisterButtonText}>Servisi Kaldır</Text>
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
  headerCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  serviceName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
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
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 100,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  healthCard: {
    backgroundColor: '#f8f9fa',
    borderRadius: 8,
    padding: 12,
  },
  healthRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 8,
  },
  healthLabel: {
    width: 100,
    fontSize: 13,
    color: '#666',
  },
  healthValue: {
    fontSize: 13,
    fontWeight: '500',
    color: '#333',
  },
  errorContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
    gap: 6,
  },
  errorText: {
    fontSize: 12,
    color: '#ef4444',
    flex: 1,
  },
  heartbeatButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
    paddingVertical: 12,
    marginBottom: 12,
  },
  heartbeatButtonText: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  deregisterButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    backgroundColor: '#fee2e2',
    borderRadius: 8,
    paddingVertical: 12,
  },
  deregisterButtonText: {
    color: '#ef4444',
    fontWeight: 'bold',
  },
});