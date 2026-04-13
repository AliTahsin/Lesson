import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  Image,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  Linking,
  TouchableOpacity
} from 'react-native';
import { hotelApi } from '../services/hotelApi';
import { Hotel } from '../types/hotel';

export const HotelDetailScreen = ({ route }: any) => {
  const { hotelId } = route.params;
  const [hotel, setHotel] = useState<Hotel | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadHotelDetail();
  }, []);

  const loadHotelDetail = async () => {
    setLoading(true);
    try {
      const data = await hotelApi.getHotelById(hotelId);
      setHotel(data);
    } catch (error) {
      console.error('Error loading hotel detail:', error);
    } finally {
      setLoading(false);
    }
  };

  const getStarDisplay = (rating: number) => {
    return '★'.repeat(rating) + '☆'.repeat(5 - rating);
  };

  const openPhone = () => {
    if (hotel?.phone) {
      Linking.openURL(`tel:${hotel.phone}`);
    }
  };

  const openEmail = () => {
    if (hotel?.email) {
      Linking.openURL(`mailto:${hotel.email}`);
    }
  };

  const openWebsite = () => {
    if (hotel?.website) {
      Linking.openURL(`https://${hotel.website}`);
    }
  };

  const openMaps = () => {
    if (hotel?.location) {
      Linking.openURL(`https://maps.google.com/?q=${hotel.location.latitude},${hotel.location.longitude}`);
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!hotel) {
    return (
      <View style={styles.center}>
        <Text>Otel bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <Image
        source={{ uri: `https://picsum.photos/400/300?random=${hotel.id}` }}
        style={styles.mainImage}
      />
      
      <View style={styles.content}>
        <Text style={styles.name}>{hotel.name}</Text>
        <Text style={styles.rating}>{getStarDisplay(hotel.starRating)}</Text>
        <Text style={styles.brand}>{hotel.brandName}</Text>
        
        <View style={styles.infoSection}>
          <Text style={styles.sectionTitle}>📍 Konum</Text>
          <Text style={styles.address}>{hotel.address}</Text>
          <Text style={styles.city}>{hotel.city}, {hotel.country}</Text>
          <TouchableOpacity onPress={openMaps} style={styles.mapButton}>
            <Text style={styles.mapButtonText}>Haritada Göster</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.infoSection}>
          <Text style={styles.sectionTitle}>📞 İletişim</Text>
          <TouchableOpacity onPress={openPhone}>
            <Text style={styles.contactText}>📞 {hotel.phone}</Text>
          </TouchableOpacity>
          <TouchableOpacity onPress={openEmail}>
            <Text style={styles.contactText}>✉️ {hotel.email}</Text>
          </TouchableOpacity>
          <TouchableOpacity onPress={openWebsite}>
            <Text style={styles.contactText}>🌐 {hotel.website}</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.infoSection}>
          <Text style={styles.sectionTitle}>📝 Açıklama</Text>
          <Text style={styles.description}>{hotel.description}</Text>
        </View>

        <View style={styles.infoSection}>
          <Text style={styles.sectionTitle}>🏨 Otel Bilgileri</Text>
          <View style={styles.statsRow}>
            <View style={styles.stat}>
              <Text style={styles.statValue}>{hotel.totalRooms}</Text>
              <Text style={styles.statLabel}>Toplam Oda</Text>
            </View>
            <View style={styles.stat}>
              <Text style={styles.statValue}>{hotel.status}</Text>
              <Text style={styles.statLabel}>Durum</Text>
            </View>
          </View>
        </View>

        <View style={styles.infoSection}>
          <Text style={styles.sectionTitle}>✨ Olanaklar</Text>
          <View style={styles.amenitiesGrid}>
            {hotel.amenities?.map((amenity, index) => (
              <View key={index} style={styles.amenityItem}>
                <Text style={styles.amenityItemText}>{amenity}</Text>
              </View>
            ))}
          </View>
        </View>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: 'white',
  },
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  mainImage: {
    width: '100%',
    height: 250,
  },
  content: {
    padding: 16,
  },
  name: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 4,
  },
  rating: {
    fontSize: 16,
    color: '#f59e0b',
    marginBottom: 4,
  },
  brand: {
    fontSize: 16,
    color: '#007AFF',
    marginBottom: 16,
  },
  infoSection: {
    marginBottom: 20,
    paddingBottom: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 12,
    color: '#333',
  },
  address: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  city: {
    fontSize: 14,
    color: '#666',
    marginBottom: 8,
  },
  mapButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 8,
    paddingHorizontal: 16,
    alignSelf: 'flex-start',
    marginTop: 8,
  },
  mapButtonText: {
    color: 'white',
    fontSize: 14,
  },
  contactText: {
    fontSize: 14,
    color: '#007AFF',
    marginBottom: 8,
  },
  description: {
    fontSize: 14,
    color: '#555',
    lineHeight: 20,
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  stat: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  statLabel: {
    fontSize: 12,
    color: '#666',
    marginTop: 4,
  },
  amenitiesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  amenityItem: {
    backgroundColor: '#f0f0f0',
    borderRadius: 16,
    paddingHorizontal: 12,
    paddingVertical: 6,
    marginRight: 8,
    marginBottom: 8,
  },
  amenityItemText: {
    fontSize: 12,
    color: '#666',
  },
});