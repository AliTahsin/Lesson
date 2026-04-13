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
  Modal
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import Icon from 'react-native-vector-icons/Ionicons';
import { meetingApi } from '../services/meetingApi';
import { MeetingRoomCard } from '../components/MeetingRoomCard';
import { MeetingRoom } from '../types/mice';

export const MeetingRoomListScreen = ({ navigation, route }: any) => {
  const { hotelId } = route.params;
  const [rooms, setRooms] = useState<MeetingRoom[]>([]);
  const [filteredRooms, setFilteredRooms] = useState<MeetingRoom[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [capacity, setCapacity] = useState('');
  const [searchText, setSearchText] = useState('');

  useEffect(() => {
    loadRooms();
  }, []);

  const loadRooms = async () => {
    setLoading(true);
    try {
      const data = await meetingApi.getRoomsByHotel(hotelId);
      setRooms(data);
      setFilteredRooms(data);
    } catch (error) {
      console.error('Error loading rooms:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const searchRooms = async () => {
    if (startDate && endDate && capacity) {
      setLoading(true);
      try {
        const availableRooms = await meetingApi.getAvailableRooms(startDate, endDate, parseInt(capacity));
        setFilteredRooms(availableRooms);
      } catch (error) {
        console.error('Error searching rooms:', error);
      } finally {
        setLoading(false);
      }
    } else {
      applyLocalFilter();
    }
    setShowFilters(false);
  };

  const applyLocalFilter = () => {
    let filtered = [...rooms];
    if (searchText) {
      filtered = filtered.filter(r => 
        r.name.toLowerCase().includes(searchText.toLowerCase()) ||
        r.roomCode.toLowerCase().includes(searchText.toLowerCase())
      );
    }
    setFilteredRooms(filtered);
  };

  useEffect(() => {
    applyLocalFilter();
  }, [searchText]);

  const clearFilters = () => {
    setStartDate('');
    setEndDate('');
    setCapacity('');
    setSearchText('');
    setFilteredRooms(rooms);
    setShowFilters(false);
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadRooms();
  };

  const handleRoomPress = (roomId: number) => {
    navigation.navigate('MeetingRoomDetail', { roomId });
  };

  const FilterModal = () => (
    <Modal visible={showFilters} animationType="slide" transparent={true}>
      <View style={styles.modalContainer}>
        <View style={styles.modalContent}>
          <Text style={styles.modalTitle}>Müsaitlik Sorgula</Text>
          
          <View style={styles.dateField}>
            <Text style={styles.label}>Başlangıç Tarihi</Text>
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
            <Text style={styles.label}>Bitiş Tarihi</Text>
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
          
          <View style={styles.dateField}>
            <Text style={styles.label}>Katılımcı Sayısı</Text>
            <TextInput
              style={styles.capacityInput}
              placeholder="Minimum kapasite"
              value={capacity}
              onChangeText={setCapacity}
              keyboardType="numeric"
            />
          </View>
          
          <View style={styles.modalButtons}>
            <TouchableOpacity style={styles.clearButton} onPress={clearFilters}>
              <Text style={styles.clearButtonText}>Temizle</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.searchModalButton} onPress={searchRooms}>
              <Text style={styles.searchModalButtonText}>Ara</Text>
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
          placeholder="Oda ara..."
          value={searchText}
          onChangeText={setSearchText}
        />
        <TouchableOpacity style={styles.filterButton} onPress={() => setShowFilters(true)}>
          <Icon name="options-outline" size={24} color="#007AFF" />
        </TouchableOpacity>
      </View>

      <FlatList
        data={filteredRooms}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <MeetingRoomCard room={item} onPress={handleRoomPress} />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Toplantı odası bulunamadı</Text>
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
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50,
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
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 20,
  },
  dateField: {
    marginBottom: 16,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
  },
  datePicker: {
    width: '100%',
  },
  capacityInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
  },
  modalButtons: {
    flexDirection: 'row',
    marginTop: 20,
    gap: 12,
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
  searchModalButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    backgroundColor: '#007AFF',
    alignItems: 'center',
  },
  searchModalButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});