import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TextInput,
  Modal,
  ScrollView
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import Icon from 'react-native-vector-icons/Ionicons';
import { loggingApi } from '../services/loggingApi';
import { LogCard } from '../components/LogCard';
import { LevelBadge } from '../components/LevelBadge';
import { LogEntry, LogSearchRequest } from '../types/logging';

export const LogListScreen = ({ route }: any) => {
  const initialFilter = route.params?.filter || null;
  const [logs, setLogs] = useState<LogEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [selectedLevel, setSelectedLevel] = useState<string | null>(initialFilter);
  const [selectedService, setSelectedService] = useState<string | null>(null);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  const levels = ['Information', 'Warning', 'Error', 'Debug'];
  const services = ['HotelManagement', 'RoomManagement', 'ReservationSystem', 'DynamicPricing', 'AuthRBAC', 'PaymentInvoice'];

  useEffect(() => {
    searchLogs();
  }, []);

  const searchLogs = async () => {
    setLoading(true);
    try {
      const request: LogSearchRequest = {};
      if (searchText) request.searchText = searchText;
      if (selectedLevel) request.level = selectedLevel;
      if (selectedService) request.service = selectedService;
      if (startDate) request.startDate = startDate;
      if (endDate) request.endDate = endDate;
      
      const data = await loggingApi.searchLogs(request);
      setLogs(data);
    } catch (error) {
      console.error('Error searching logs:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    searchLogs();
  };

  const clearFilters = () => {
    setSearchText('');
    setSelectedLevel(null);
    setSelectedService(null);
    setStartDate('');
    setEndDate('');
    setShowFilters(false);
    searchLogs();
  };

  const applyFilters = () => {
    setShowFilters(false);
    searchLogs();
  };

  const FilterModal = () => (
    <Modal visible={showFilters} animationType="slide" transparent={true}>
      <View style={styles.modalContainer}>
        <View style={styles.modalContent}>
          <Text style={styles.modalTitle}>Filtreler</Text>
          
          <ScrollView>
            <Text style={styles.filterTitle}>Seviye</Text>
            <View style={styles.levelContainer}>
              {levels.map((level) => (
                <TouchableOpacity
                  key={level}
                  style={[styles.levelButton, selectedLevel === level && styles.levelButtonActive]}
                  onPress={() => setSelectedLevel(selectedLevel === level ? null : level)}
                >
                  <LevelBadge level={level} size="small" />
                </TouchableOpacity>
              ))}
            </View>

            <Text style={styles.filterTitle}>Servis</Text>
            <View style={styles.serviceContainer}>
              {services.map((service) => (
                <TouchableOpacity
                  key={service}
                  style={[styles.serviceButton, selectedService === service && styles.serviceButtonActive]}
                  onPress={() => setSelectedService(selectedService === service ? null : service)}
                >
                  <Text style={[styles.serviceText, selectedService === service && styles.serviceTextActive]}>
                    {service}
                  </Text>
                </TouchableOpacity>
              ))}
            </View>

            <Text style={styles.filterTitle}>Tarih Aralığı</Text>
            <View style={styles.dateContainer}>
              <View style={styles.dateField}>
                <Text style={styles.dateLabel}>Başlangıç</Text>
                <DatePicker
                  style={styles.datePicker}
                  date={startDate}
                  mode="date"
                  placeholder="Seçiniz"
                  format="YYYY-MM-DD"
                  confirmBtnText="Tamam"
                  cancelBtnText="İptal"
                  onDateChange={(date) => setStartDate(date)}
                />
              </View>
              <View style={styles.dateField}>
                <Text style={styles.dateLabel}>Bitiş</Text>
                <DatePicker
                  style={styles.datePicker}
                  date={endDate}
                  mode="date"
                  placeholder="Seçiniz"
                  format="YYYY-MM-DD"
                  confirmBtnText="Tamam"
                  cancelBtnText="İptal"
                  onDateChange={(date) => setEndDate(date)}
                />
              </View>
            </View>
          </ScrollView>

          <View style={styles.modalButtons}>
            <TouchableOpacity style={styles.clearButton} onPress={clearFilters}>
              <Text style={styles.clearButtonText}>Temizle</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.applyButton} onPress={applyFilters}>
              <Text style={styles.applyButtonText}>Uygula</Text>
            </TouchableOpacity>
          </View>
        </View>
      </View>
    </Modal>
  );

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder="Log ara..."
          value={searchText}
          onChangeText={setSearchText}
          onSubmitEditing={searchLogs}
        />
        <TouchableOpacity style={styles.filterButton} onPress={() => setShowFilters(true)}>
          <Icon name="options-outline" size={24} color="#007AFF" />
        </TouchableOpacity>
      </View>

      {(selectedLevel || selectedService) && (
        <View style={styles.activeFilters}>
          {selectedLevel && (
            <View style={styles.activeFilter}>
              <LevelBadge level={selectedLevel} size="small" />
              <TouchableOpacity onPress={() => setSelectedLevel(null)}>
                <Icon name="close" size={14} color="#888" />
              </TouchableOpacity>
            </View>
          )}
          {selectedService && (
            <View style={styles.activeFilter}>
              <Text style={styles.activeFilterText}>{selectedService}</Text>
              <TouchableOpacity onPress={() => setSelectedService(null)}>
                <Icon name="close" size={14} color="#888" />
              </TouchableOpacity>
            </View>
          )}
        </View>
      )}

      <FlatList
        data={logs}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <LogCard log={item} onPress={(id) => navigation.navigate('LogDetail', { logId: id })} />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="document-text-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Log bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      <FilterModal />
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
    padding: 16,
    backgroundColor: 'white',
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  searchInput: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
    marginRight: 8,
  },
  filterButton: {
    paddingHorizontal: 12,
    justifyContent: 'center',
  },
  activeFilters: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    padding: 12,
    backgroundColor: 'white',
    gap: 8,
  },
  activeFilter: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 16,
    gap: 4,
  },
  activeFilterText: {
    fontSize: 12,
    color: '#333',
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
  filterTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 16,
    marginBottom: 12,
  },
  levelContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  levelButton: {
    padding: 4,
  },
  levelButtonActive: {
    opacity: 0.8,
  },
  serviceContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  serviceButton: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    backgroundColor: '#f0f0f0',
    borderRadius: 16,
  },
  serviceButtonActive: {
    backgroundColor: '#007AFF',
  },
  serviceText: {
    fontSize: 12,
    color: '#333',
  },
  serviceTextActive: {
    color: 'white',
  },
  dateContainer: {
    flexDirection: 'row',
    gap: 12,
  },
  dateField: {
    flex: 1,
  },
  dateLabel: {
    fontSize: 12,
    color: '#666',
    marginBottom: 4,
  },
  datePicker: {
    width: '100%',
  },
  modalButtons: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 20,
  },
  clearButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#ddd',
    alignItems: 'center',
  },
  clearButtonText: {
    color: '#666',
  },
  applyButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    backgroundColor: '#007AFF',
    alignItems: 'center',
  },
  applyButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});