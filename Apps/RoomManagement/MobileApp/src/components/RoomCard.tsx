import React from 'react';
import { View, Text, Image, TouchableOpacity, StyleSheet } from 'react-native';
import { Room } from '../types/room';

interface Props {
  room: Room;
  onPress: (id: number) => void;
  showStatus?: boolean;
}

export const RoomCard: React.FC<Props> = ({ room, onPress, showStatus = true }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Available': return '#16a34a';
      case 'Occupied': return '#dc2626';
      case 'Maintenance': return '#f59e0b';
      case 'Cleaning': return '#3b82f6';
      default: return '#6b7280';
    }
  };

  const getViewIcon = (view: string) => {
    switch (view) {
      case 'Sea': return '🌊';
      case 'City': return '🏙️';
      case 'Mountain': return '⛰️';
      case 'Garden': return '🌿';
      default: return '👁️';
    }
  };

  return (
    <TouchableOpacity onPress={() => onPress(room.id)} style={styles.card}>
      <View style={styles.header}>
        <View>
          <Text style={styles.roomNumber}>Oda {room.roomNumber}</Text>
          <Text style={styles.hotelName}>{room.hotelName}</Text>
        </View>
        {showStatus && (
          <View style={[styles.statusBadge, { backgroundColor: getStatusColor(room.status) }]}>
            <Text style={styles.statusText}>{room.status}</Text>
          </View>
        )}
      </View>

      <View style={styles.details}>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>📐 Kat</Text>
          <Text style={styles.detailValue}>{room.floor}</Text>
        </View>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>{getViewIcon(room.view)} Manzara</Text>
          <Text style={styles.detailValue}>{room.view}</Text>
        </View>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>👥 Kapasite</Text>
          <Text style={styles.detailValue}>{room.capacity} kişi</Text>
        </View>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>💰 Fiyat</Text>
          <Text style={styles.priceValue}>€{room.basePrice}</Text>
        </View>
      </View>

      <View style={styles.amenities}>
        {room.amenities?.slice(0, 3).map((amenity, index) => (
          <View key={index} style={styles.amenityBadge}>
            <Text style={styles.amenityText}>{amenity}</Text>
          </View>
        ))}
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
    padding: 16,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  roomNumber: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  hotelName: {
    fontSize: 14,
    color: '#666',
    marginTop: 2,
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
  details: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    borderTopWidth: 1,
    borderBottomWidth: 1,
    borderColor: '#f0f0f0',
    paddingVertical: 12,
    marginBottom: 12,
  },
  detailItem: {
    width: '50%',
    marginBottom: 8,
  },
  detailLabel: {
    fontSize: 12,
    color: '#888',
  },
  detailValue: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
    marginTop: 2,
  },
  priceValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  amenities: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  amenityBadge: {
    backgroundColor: '#f0f0f0',
    borderRadius: 12,
    paddingHorizontal: 8,
    paddingVertical: 4,
    marginRight: 8,
    marginBottom: 4,
  },
  amenityText: {
    fontSize: 11,
    color: '#666',
  },
});