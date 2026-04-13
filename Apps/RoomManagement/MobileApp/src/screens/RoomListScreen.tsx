import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  TextInput,
  ScrollView,
  Modal
} from 'react-native';
import { roomApi } from '../services/roomApi';
import { RoomCard } from '../components/RoomCard';
import { Room, RoomType } from '../types/room';

export const RoomListScreen = ({ navigation }: any) => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [filteredRooms, setFilteredRooms] = useState<Room[]>([]);
  const [loading, setLoading] = useState(true);
  const [roomTypes, setRoomTypes] = useState<RoomType[]>([]);
  const [selectedHotel, setSelectedHotel] = useState<number | null>(null);
  const [selectedRoomType, setSelectedRoomType] = useState<number | null>(null);
  const [searchText, setSearchText] = useState('');
  const [showFilters, setShowFilters] = useState(false);
  const [minPrice, setMinPrice] = useState('');
  const [maxPrice, setMaxPrice] = useState('');
  const [hotels, setHotels] = useState<{ id: number; name: string }[]>([]);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [rooms, selectedHotel, selectedRoomType, searchText, minPrice, maxPrice]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [roomsData, typesData] = await Promise.all([
        roomApi.getAllRooms(),
        roomApi.getAllRoomTypes()
      ]);
      setRooms(roomsData);
      setFilteredRooms(roomsData);
      setRoomTypes(typesData);
      
      const uniqueHotels = Array.from(
        new Map(roomsData.map(r => [r.hotelId, { id: r.hotelId, name: r.hotelName }])).values()
      );
      setHotels(uniqueHotels);
    } catch (error) {
      console.error('Error loading rooms:', error);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...rooms];

    if (selectedHotel) {
      filtered = filtered.filter(r => r.hotelId === selectedHotel);
    }

    if (selectedRoomType) {
      filtered = filtered.filter(r => r.roomTypeId === selectedRoomType);
    }

    if (searchText) {
      filtered = filtered.filter(r =>
        r.roomNumber.toLowerCase().includes(searchText.toLowerCase()) ||
        r.hotelName.toLowerCase().includes(searchText.toLowerCase())
      );
    }

    if (minPrice) {
      filtered = filtered.filter(r => r.basePrice >= parseFloat(minPrice));
    }

    if (maxPrice) {
      filtered = filtered.filter(r => r.basePrice <= parseFloat(maxPrice));
    }

    setFilteredRooms(filtered);
  };

  const clearFilters = () => {
    setSelectedHotel(null);
    setSelectedRoomType(null);
    setSearchText('');
    setMinPrice('');
    setMaxPrice('');
  };

  const handleRoomPress = (id: number) => {
    navigation.navigate('RoomDetail', { roomId: id });
  };

  const FilterModal = () => (
    <Modal visible={showFilters} animationType="slide" transparent={true}>
      <View style={styles.modalContainer}>
        <View style={styles.modalContent}>
          <Text style={styles.modalTitle}>Filtreler</Text>
          
          <ScrollView>
            <Text style={styles.filterTitle}>Oteller</Text>
            <ScrollView horizontal showsHorizontalScrollIndicator={false}>
              <TouchableOpacity
                style={[styles.filterChip, !selectedHotel && styles.filterChipActive]}
                onPress={() => setSelectedHotel(null)}
              >
                <Text style={[styles.filterChipText, !selectedHotel && styles.filterChipTextActive]}>Tümü</Text>
              </TouchableOpacity>
              {hotels.map(hotel => (
                <TouchableOpacity
                  key={hotel.id}
                  style={[styles.filterChip, selectedHotel === hotel.id && styles.filterChipActive]}
                  onPress={() => setSelectedHotel(hotel.id)}
                >
                  <Text style={[styles.filterChipText, selectedHotel === hotel.id && styles.filterChipTextActive]}>
                    {hotel.name}
                  </Text>
                </TouchableOpacity>
              ))}
            </ScrollView>

            <Text style={styles.filterTitle}>Oda Tipleri</Text>
            <ScrollView horizontal showsHorizontalScrollIndicator={false}>
              <TouchableOpacity
                style={[styles.filterChip, !selectedRoomType && styles.filterChipActive]}
                onPress={() => setSelectedRoomType(null)}
              >
                <Text style={[styles.filterChipText, !selectedRoomType && styles.filterChipTextActive]}>Tümü</Text>
              </TouchableOpacity>
              {roomTypes.map(type => (
                <TouchableOpacity
                  key={type.id}
                  style={[styles.filterChip, selectedRoomType === type.id && styles.filterChipActive]}
                  onPress={() => setSelectedRoomType(type.id)}
                >
                  <Text style={[styles.filterChipText, selectedRoomType === type.id && styles.filterChipTextActive]}>
                    {type.name}
                  </Text>
                </TouchableOpacity>
              ))}
            </ScrollView>

            <Text style={styles.filterTitle}>Fiyat Aralığı</Text>
            <View style={styles.priceContainer}>
              <TextInput
                style={styles.priceInput}
                placeholder="Min €"
                value={minPrice}
                onChangeText={setMinPrice}
                keyboardType="numeric"
              />
              <Text style={styles.priceSeparator}>-</Text>
              <TextInput
                style={styles.priceInput}
                placeholder="Max €"
                value={maxPrice}
                onChangeText={setMaxPrice}
                keyboardType="numeric"
              />
            </View>
          </ScrollView>

          <View style={styles.modalButtons}>
            <TouchableOpacity style={styles.clearButton} onPress={clearFilters}>
              <Text style={styles.clearButtonText}>Temizle</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.applyButton} onPress={() => setShowFilters(false)}>
              <Text style={styles.applyButtonText}>Uygula</Text>
            </TouchableOpacity>
          </View>
        </View>
      </View>
    </Modal>
  );

  if (loading) {
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
          placeholder="Oda no veya otel ara..."
          value={searchText}
          onChangeText={setSearchText}
        />
        <TouchableOpacity style={styles.filterButton} onPress={() => setShowFilters(true)}>
          <Text style={styles.filterButtonText}>🔍 Filtre</Text>
        </TouchableOpacity>
      </View>

      <FlatList
        data={filteredRooms}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <RoomCard room={item} onPress={handleRoomPress} />
        )}
        contentContainerStyle={styles.listContent}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Oda bulunamadı</Text>
          </View>
        }
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
    marginRight: 8,
    fontSize: 16,
  },
  filterButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 16,
    justifyContent: 'center',
  },
  filterButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  listContent: {
    paddingVertical: 8,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50,
  },
  emptyText: {
    fontSize: 16,
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
    marginBottom: 20,
  },
  filterTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    marginTop: 16,
    marginBottom: 8,
  },
  filterChip: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 20,
    backgroundColor: '#f0f0f0',
    marginRight: 8,
  },
  filterChipActive: {
    backgroundColor: '#007AFF',
  },
  filterChipText: {
    color: '#333',
  },
  filterChipTextActive: {
    color: 'white',
  },
  priceContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 8,
  },
  priceInput: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 8,
  },
  priceSeparator: {
    marginHorizontal: 8,
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