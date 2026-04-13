import React from 'react';
import { View, Text, Image, TouchableOpacity, StyleSheet } from 'react-native';
import { Hotel } from '../types/hotel';

interface Props {
  hotel: Hotel;
  onPress: (id: number) => void;
}

export const HotelCard: React.FC<Props> = ({ hotel, onPress }) => {
  const getStarDisplay = (rating: number) => {
    return '★'.repeat(rating) + '☆'.repeat(5 - rating);
  };

  return (
    <TouchableOpacity onPress={() => onPress(hotel.id)} style={styles.card}>
      <Image
        source={{ uri: `https://picsum.photos/400/200?random=${hotel.id}` }}
        style={styles.image}
      />
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.name}>{hotel.name}</Text>
          <Text style={styles.rating}>{getStarDisplay(hotel.starRating)}</Text>
        </View>
        <Text style={styles.brand}>{hotel.brandName || 'Hotel'}</Text>
        <Text style={styles.location}>📍 {hotel.city}, {hotel.country}</Text>
        <View style={styles.amenities}>
          {hotel.amenities?.slice(0, 3).map((amenity, index) => (
            <View key={index} style={styles.amenityBadge}>
              <Text style={styles.amenityText}>{amenity}</Text>
            </View>
          ))}
        </View>
        <View style={styles.statusContainer}>
          <View style={[styles.statusBadge, { backgroundColor: hotel.status === 'Active' ? '#16a34a' : '#f59e0b' }]}>
            <Text style={styles.statusText}>{hotel.status}</Text>
          </View>
          <Text style={styles.roomsCount}>🏨 {hotel.totalRooms} oda</Text>
        </View>
      </View>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    overflow: 'hidden',
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  image: {
    width: '100%',
    height: 150,
  },
  content: {
    padding: 12,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 4,
  },
  name: {
    fontSize: 18,
    fontWeight: 'bold',
    flex: 1,
  },
  rating: {
    fontSize: 14,
    color: '#f59e0b',
  },
  brand: {
    fontSize: 14,
    color: '#007AFF',
    marginBottom: 4,
  },
  location: {
    fontSize: 14,
    color: '#666',
    marginBottom: 8,
  },
  amenities: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    marginBottom: 8,
  },
  amenityBadge: {
    backgroundColor: '#f0f0f0',
    borderRadius: 12,
    paddingHorizontal: 8,
    paddingVertical: 4,
    marginRight: 6,
    marginBottom: 4,
  },
  amenityText: {
    fontSize: 11,
    color: '#666',
  },
  statusContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 4,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 11,
    fontWeight: 'bold',
  },
  roomsCount: {
    fontSize: 12,
    color: '#666',
  },
});