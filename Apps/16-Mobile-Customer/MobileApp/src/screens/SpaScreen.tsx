import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { customerApi } from '../services/customerApi';
import { SpaServiceCard } from '../components/SpaServiceCard';
import { SpaService, SpaAppointment } from '../types/customer';

export const SpaScreen = ({ navigation }: any) => {
  const { t } = useLanguageContext();
  const [services, setServices] = useState<SpaService[]>([]);
  const [appointments, setAppointments] = useState<SpaAppointment[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'services' | 'appointments'>('services');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [servicesData, appointmentsData] = await Promise.all([
        customerApi.getSpaServices(),
        customerApi.getMyAppointments()
      ]);
      setServices(servicesData);
      setAppointments(appointmentsData);
    } catch (error) {
      console.error('Error loading spa data:', error);
      Alert.alert('Hata', 'Veriler yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleServicePress = (service: SpaService) => {
    navigation.navigate('SpaDetail', { service });
  };

  const handleCancelAppointment = async (appointmentId: number) => {
    Alert.alert(
      'Randevu İptali',
      'Bu randevuyu iptal etmek istediğinize emin misiniz?',
      [
        { text: 'Hayır', style: 'cancel' },
        {
          text: 'Evet',
          onPress: async () => {
            try {
              await customerApi.cancelAppointment(appointmentId);
              loadData();
              Alert.alert('Başarılı', 'Randevu iptal edildi');
            } catch (error) {
              Alert.alert('Hata', 'Randevu iptal edilemedi');
            }
          }
        }
      ]
    );
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Confirmed': return '#10b981';
      case 'Pending': return '#f59e0b';
      case 'Completed': return '#6b7280';
      case 'Cancelled': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Confirmed': return 'Onaylandı';
      case 'Pending': return 'Beklemede';
      case 'Completed': return 'Tamamlandı';
      case 'Cancelled': return 'İptal Edildi';
      default: return status;
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.tabBar}>
        <TouchableOpacity
          style={[styles.tab, activeTab === 'services' && styles.tabActive]}
          onPress={() => setActiveTab('services')}
        >
          <Text style={[styles.tabText, activeTab === 'services' && styles.tabTextActive]}>
            {t('spa_services')}
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.tab, activeTab === 'appointments' && styles.tabActive]}
          onPress={() => setActiveTab('appointments')}
        >
          <Text style={[styles.tabText, activeTab === 'appointments' && styles.tabTextActive]}>
            Randevularım
          </Text>
        </TouchableOpacity>
      </View>

      {activeTab === 'services' ? (
        <FlatList
          data={services}
          keyExtractor={(item) => item.id.toString()}
          renderItem={({ item }) => (
            <SpaServiceCard service={item} onPress={handleServicePress} />
          )}
          contentContainerStyle={styles.listContent}
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Icon name="flower-outline" size={64} color="#ccc" />
              <Text style={styles.emptyText}>Hizmet bulunamadı</Text>
            </View>
          }
        />
      ) : (
        <FlatList
          data={appointments}
          keyExtractor={(item) => item.id.toString()}
          renderItem={({ item }) => (
            <View style={styles.appointmentCard}>
              <View style={styles.appointmentHeader}>
                <Text style={styles.appointmentService}>{item.serviceName}</Text>
                <View style={[styles.appointmentStatus, { backgroundColor: getStatusColor(item.status) }]}>
                  <Text style={styles.appointmentStatusText}>{getStatusText(item.status)}</Text>
                </View>
              </View>
              <Text style={styles.appointmentDate}>
                {formatDate(item.appointmentDate)} - {item.appointmentTime}
              </Text>
              <Text style={styles.appointmentDuration}>
                ⏱️ {item.durationMinutes} dakika
              </Text>
              <Text style={styles.appointmentPrice}>€{item.price}</Text>
              {item.status === 'Pending' && (
                <TouchableOpacity
                  style={styles.cancelButton}
                  onPress={() => handleCancelAppointment(item.id)}
                >
                  <Text style={styles.cancelButtonText}>İptal Et</Text>
                </TouchableOpacity>
              )}
            </View>
          )}
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Icon name="calendar-outline" size={64} color="#ccc" />
              <Text style={styles.emptyText}>Henüz randevunuz bulunmuyor</Text>
              <TouchableOpacity
                style={styles.bookButton}
                onPress={() => setActiveTab('services')}
              >
                <Text style={styles.bookButtonText}>Hizmetleri Görüntüle</Text>
              </TouchableOpacity>
            </View>
          }
          contentContainerStyle={styles.listContent}
        />
      )}
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
  tabBar: {
    flexDirection: 'row',
    backgroundColor: 'white',
    paddingHorizontal: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  tab: {
    flex: 1,
    paddingVertical: 14,
    alignItems: 'center',
    borderBottomWidth: 2,
    borderBottomColor: 'transparent',
  },
  tabActive: {
    borderBottomColor: '#007AFF',
  },
  tabText: {
    fontSize: 14,
    color: '#666',
  },
  tabTextActive: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
    marginTop: 16,
  },
  bookButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 20,
    paddingVertical: 10,
    marginTop: 16,
  },
  bookButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  appointmentCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
    elevation: 2,
  },
  appointmentHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  appointmentService: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  appointmentStatus: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  appointmentStatusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  appointmentDate: {
    fontSize: 14,
    color: '#666',
    marginBottom: 8,
  },
  appointmentDuration: {
    fontSize: 12,
    color: '#888',
    marginBottom: 8,
  },
  appointmentPrice: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
    marginBottom: 12,
  },
  cancelButton: {
    backgroundColor: '#ef4444',
    borderRadius: 8,
    paddingVertical: 10,
    alignItems: 'center',
  },
  cancelButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
}); 