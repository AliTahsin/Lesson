import React, { useState } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TextInput,
  Modal
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useGatewayContext } from '../context/GatewayContext';
import { ServiceCard } from '../components/ServiceCard';
import { Service } from '../types/gateway';

export const ServiceListScreen = ({ navigation }: any) => {
  const { services, loading, refreshing, refresh, registerService } = useGatewayContext();
  const [searchText, setSearchText] = useState('');
  const [filter, setFilter] = useState<'all' | 'healthy' | 'unhealthy'>('all');
  const [modalVisible, setModalVisible] = useState(false);
  const [newService, setNewService] = useState({
    name: '',
    url: '',
    port: ''
  });

  const filteredServices = services.filter(service => {
    // Search filter
    const matchesSearch = service.name.toLowerCase().includes(searchText.toLowerCase()) ||
                          service.url.toLowerCase().includes(searchText.toLowerCase());
    
    // Health filter
    const matchesHealth = filter === 'all' ? true :
                          filter === 'healthy' ? service.isHealthy :
                          !service.isHealthy;
    
    return matchesSearch && matchesHealth;
  });

  const handleServicePress = (service: Service) => {
    navigation.navigate('ServiceDetail', { service });
  };

  const handleRegisterService = async () => {
    if (!newService.name || !newService.url || !newService.port) {
      alert('Lütfen tüm alanları doldurun');
      return;
    }

    const success = await registerService({
      name: newService.name,
      url: newService.url,
      port: parseInt(newService.port),
      isHealthy: true,
      registeredAt: new Date().toISOString(),
      lastHeartbeat: new Date().toISOString()
    });

    if (success) {
      setModalVisible(false);
      setNewService({ name: '', url: '', port: '' });
    } else {
      alert('Servis kaydedilemedi');
    }
  };

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {/* Search Bar */}
      <View style={styles.searchContainer}>
        <Icon name="search" size={20} color="#666" style={styles.searchIcon} />
        <TextInput
          style={styles.searchInput}
          placeholder="Servis ara..."
          value={searchText}
          onChangeText={setSearchText}
        />
      </View>

      {/* Filter Buttons */}
      <View style={styles.filterContainer}>
        <TouchableOpacity
          style={[styles.filterButton, filter === 'all' && styles.filterButtonActive]}
          onPress={() => setFilter('all')}
        >
          <Text style={[styles.filterText, filter === 'all' && styles.filterTextActive]}>
            Tümü ({services.length})
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.filterButton, filter === 'healthy' && styles.filterButtonActive]}
          onPress={() => setFilter('healthy')}
        >
          <Text style={[styles.filterText, filter === 'healthy' && styles.filterTextActive]}>
            Sağlıklı ({services.filter(s => s.isHealthy).length})
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.filterButton, filter === 'unhealthy' && styles.filterButtonActive]}
          onPress={() => setFilter('unhealthy')}
        >
          <Text style={[styles.filterText, filter === 'unhealthy' && styles.filterTextActive]}>
            Sağlıksız ({services.filter(s => !s.isHealthy).length})
          </Text>
        </TouchableOpacity>
      </View>

      {/* Service List */}
      <FlatList
        data={filteredServices}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <ServiceCard service={item} onPress={handleServicePress} />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={refresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="server-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Servis bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      {/* Add Service FAB */}
      <TouchableOpacity
        style={styles.fab}
        onPress={() => setModalVisible(true)}
      >
        <Icon name="add" size={28} color="white" />
      </TouchableOpacity>

      {/* Register Service Modal */}
      <Modal visible={modalVisible} transparent animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Yeni Servis Ekle</Text>

            <Text style={styles.modalLabel}>Servis Adı</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="Örn: HotelManagement"
              value={newService.name}
              onChangeText={(text) => setNewService({ ...newService, name: text })}
            />

            <Text style={styles.modalLabel}>Servis URL</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="http://localhost:5000"
              value={newService.url}
              onChangeText={(text) => setNewService({ ...newService, url: text })}
            />

            <Text style={styles.modalLabel}>Port</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="5000"
              value={newService.port}
              onChangeText={(text) => setNewService({ ...newService, port: text })}
              keyboardType="numeric"
            />

            <View style={styles.modalButtons}>
              <TouchableOpacity
                style={[styles.modalButton, styles.cancelModalButton]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.cancelModalButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.modalButton, styles.submitModalButton]}
                onPress={handleRegisterService}
              >
                <Text style={styles.submitModalButtonText}>Ekle</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
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
  searchContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: 'white',
    margin: 16,
    paddingHorizontal: 12,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#ddd',
  },
  searchIcon: {
    marginRight: 8,
  },
  searchInput: {
    flex: 1,
    paddingVertical: 12,
    fontSize: 16,
  },
  filterContainer: {
    flexDirection: 'row',
    paddingHorizontal: 12,
    marginBottom: 8,
  },
  filterButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    borderRadius: 20,
    marginHorizontal: 4,
    backgroundColor: '#e0e0e0',
  },
  filterButtonActive: {
    backgroundColor: '#007AFF',
  },
  filterText: {
    fontSize: 12,
    color: '#666',
  },
  filterTextActive: {
    color: 'white',
  },
  listContent: {
    paddingBottom: 80,
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
  },
  fab: {
    position: 'absolute',
    bottom: 20,
    right: 20,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: '#007AFF',
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 5,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'flex-end',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
    padding: 20,
    maxHeight: '80%',
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 20,
  },
  modalLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
    marginTop: 12,
  },
  modalInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
  },
  modalButtons: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 20,
  },
  modalButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
  },
  cancelModalButton: {
    backgroundColor: '#f0f0f0',
  },
  submitModalButton: {
    backgroundColor: '#007AFF',
  },
  cancelModalButtonText: {
    color: '#666',
  },
  submitModalButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});