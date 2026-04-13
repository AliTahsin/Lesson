import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Modal,
  ScrollView
} from 'react-native';
import { hotelApi } from '../services/hotelApi';
import { HotelCard } from '../components/HotelCard';
import { Hotel, Brand, Chain } from '../types/hotel';

export const HotelListScreen = ({ navigation }: any) => {
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [filteredHotels, setFilteredHotels] = useState<Hotel[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchText, setSearchText] = useState('');
  const [showFilters, setShowFilters] = useState(false);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [chains, setChains] = useState<Chain[]>([]);
  const [selectedBrand, setSelectedBrand] = useState<number | null>(null);
  const [selectedChain, setSelectedChain] = useState<number | null>(null);
  const [selectedCity, setSelectedCity] = useState<string | null>(null);
  const [cities, setCities] = useState<string[]>([]);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [hotels, searchText, selectedBrand, selectedChain, selectedCity]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [hotelsData, brandsData, chainsData] = await Promise.all([
        hotelApi.getAllHotels(),
        hotelApi.getAllBrands(),
        hotelApi.getAllChains()
      ]);
      setHotels(hotelsData);
      setFilteredHotels(hotelsData);
      setBrands(brandsData);
      setChains(chainsData);
      
      const uniqueCities = [...new Set(hotelsData.map(h => h.city))];
      setCities(uniqueCities);
    } catch (error) {
      console.error('Error loading data:', error);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...hotels];

    if (searchText) {
      filtered = filtered.filter(h =>
        h.name.toLowerCase().includes(searchText.toLowerCase()) ||
        h.city.toLowerCase().includes(searchText.toLowerCase())
      );
    }

    if (selectedBrand) {
      filtered = filtered.filter(h => h.brandId === selectedBrand);
    }

    if (selectedChain) {
      const brandIds = brands.filter(b => b.id === selectedChain).map(b => b.id);
      filtered = filtered.filter(h => brandIds.includes(h.brandId));
    }

    if (selectedCity) {
      filtered = filtered.filter(h => h.city === selectedCity);
    }

    setFilteredHotels(filtered);
  };

  const clearFilters = () => {
    setSearchText('');
    setSelectedBrand(null);
    setSelectedChain(null);
    setSelectedCity(null);
  };

  const handleHotelPress = (id: number) => {
    navigation.navigate('HotelDetail', { hotelId: id });
  };

  const FilterModal = () => (
    <Modal visible={showFilters} animationType="slide" transparent={true}>
      <View style={styles.modalContainer}>
        <View style={styles.modalContent}>
          <Text style={styles.modalTitle}>Filtreler</Text>
          
          <ScrollView>
            <Text style={styles.filterTitle}>Zincirler</Text>
            <ScrollView horizontal showsHorizontalScrollIndicator={false}>
              <TouchableOpacity
                style={[styles.filterChip, !selectedChain && styles.filterChipActive]}
                onPress={() => setSelectedChain(null)}
              >
                <Text style={[styles.filterChipText, !selectedChain && styles.filterChipTextActive]}>Tümü</Text>
              </TouchableOpacity>
              {chains.map(chain => (
                <TouchableOpacity
                  key={chain.id}
                  style={[styles.filterChip, selectedChain === chain.id && styles.filterChipActive]}
                  onPress={() => setSelectedChain(chain.id)}
                >
                  <Text style={[styles.filterChipText, selectedChain === chain.id && styles.filterChipTextActive]}>
                    {chain.name}
                  </Text>
                </TouchableOpacity>
              ))}
            </ScrollView>

            <Text style={styles.filterTitle}>Şehirler</Text>
            <ScrollView horizontal showsHorizontalScrollIndicator={false}>
              <TouchableOpacity
                style={[styles.filterChip, !selectedCity && styles.filterChipActive]}
                onPress={() => setSelectedCity(null)}
              >
                <Text style={[styles.filterChipText, !selectedCity && styles.filterChipTextActive]}>Tümü</Text>
              </TouchableOpacity>
              {cities.map(city => (
                <TouchableOpacity
                  key={city}
                  style={[styles.filterChip, selectedCity === city && styles.filterChipActive]}
                  onPress={() => setSelectedCity(city)}
                >
                  <Text style={[styles.filterChipText, selectedCity === city && styles.filterChipTextActive]}>
                    {city}
                  </Text>
                </TouchableOpacity>
              ))}
            </ScrollView>
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
          placeholder="Otel veya şehir ara..."
          value={searchText}
          onChangeText={setSearchText}
        />
        <TouchableOpacity style={styles.filterButton} onPress={() => setShowFilters(true)}>
          <Text style={styles.filterButtonText}>🔍 Filtre</Text>
        </TouchableOpacity>
      </View>

      {(selectedBrand || selectedChain || selectedCity) && (
        <View style={styles.activeFilters}>
          <Text style={styles.activeFiltersText}>Aktif Filtreler:</Text>
          {selectedChain && (
            <View style={styles.activeFilterBadge}>
              <Text style={styles.activeFilterText}>Zincir: {chains.find(c => c.id === selectedChain)?.name}</Text>
            </View>
          )}
          {selectedCity && (
            <View style={styles.activeFilterBadge}>
              <Text style={styles.activeFilterText}>Şehir: {selectedCity}</Text>
            </View>
          )}
        </View>
      )}

      <FlatList
        data={filteredHotels}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <HotelCard hotel={item} onPress={handleHotelPress} />
        )}
        contentContainerStyle={styles.listContent}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Otel bulunamadı</Text>
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
  activeFilters: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    padding: 12,
    backgroundColor: '#f0f0f0',
  },
  activeFiltersText: {
    fontSize: 12,
    color: '#666',
    marginRight: 8,
  },
  activeFilterBadge: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    paddingHorizontal: 8,
    paddingVertical: 4,
    marginRight: 8,
  },
  activeFilterText: {
    color: 'white',
    fontSize: 11,
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