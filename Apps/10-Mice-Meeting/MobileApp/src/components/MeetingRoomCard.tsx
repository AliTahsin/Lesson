import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet, Image } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { MeetingRoom } from '../types/mice';

interface Props {
  room: MeetingRoom;
  onPress: (id: number) => void;
}

export const MeetingRoomCard: React.FC<Props> = ({ room, onPress }) => {
  const getCapacityIcon = () => {
    if (room.capacity >= 200) return '🏟️';
    if (room.capacity >= 50) return '🏛️';
    if (room.capacity >= 20) return '🏠';
    return '📏';
  };

  return (
    <TouchableOpacity onPress={() => onPress(room.id)} style={styles.card}>
      <Image
        source={{ uri: room.images?.[0] || `https://picsum.photos/400/200?random=${room.id}` }}
        style={styles.image}
      />
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.name}>{room.name}</Text>
          <Text style={styles.code}>{room.roomCode}</Text>
        </View>
        
        <View style={styles.capacityRow}>
          <Text style={styles.capacityIcon}>{getCapacityIcon()}</Text>
          <Text style={styles.capacity}>{room.capacity} kişi</Text>
          <Text style={styles.area}>📐 {room.area} m²</Text>
        </View>
        
        <View style={styles.amenities}>
          {room.hasProjector && <Text style={styles.amenity}>📽️ Projeksiyon</Text>}
          {room.hasSoundSystem && <Text style={styles.amenity}>🔊 Ses Sistemi</Text>}
          {room.hasWiFi && <Text style={styles.amenity}>📶 WiFi</Text>}
          {room.hasWhiteboard && <Text style={styles.amenity}>📝 Tahta</Text>}
        </View>
        
        <View style={styles.priceRow}>
          <View style={styles.priceItem}>
            <Text style={styles.priceLabel}>Yarım Gün</Text>
            <Text style={styles.priceValue}>€{room.halfDayPrice}</Text>
          </View>
          <View style={styles.priceItem}>
            <Text style={styles.priceLabel}>Tam Gün</Text>
            <Text style={styles.priceValue}>€{room.fullDayPrice}</Text>
          </View>
        </View>
        
        <Text style={styles.floor}>📍 {room.floor}</Text>
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
    elevation: 2,
  },
  image: {
    width: '100%',
    height: 150,
  },
  content: {
    padding: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  name: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  code: {
    fontSize: 12,
    color: '#888',
  },
  capacityRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 8,
  },
  capacityIcon: {
    fontSize: 14,
    marginRight: 4,
  },
  capacity: {
    fontSize: 14,
    color: '#666',
    marginRight: 12,
  },
  area: {
    fontSize: 14,
    color: '#666',
  },
  amenities: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    marginBottom: 12,
  },
  amenity: {
    fontSize: 12,
    color: '#666',
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
    marginRight: 6,
    marginBottom: 4,
  },
  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  priceItem: {
    alignItems: 'center',
  },
  priceLabel: {
    fontSize: 11,
    color: '#888',
  },
  priceValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  floor: {
    fontSize: 12,
    color: '#888',
  },
});