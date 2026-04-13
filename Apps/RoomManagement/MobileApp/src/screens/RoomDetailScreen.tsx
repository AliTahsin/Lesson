import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import { roomApi } from '../services/roomApi';
import { Room, RoomType } from '../types/room';

export const RoomDetailScreen = ({ route }: any) => {
  const { roomId } = route.params;
  const [room, setRoom] = useState<Room | null>(null);
  const [roomType, setRoomType] = useState<RoomType | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadRoomDetail();
  }, []);

  const loadRoomDetail = async () => {
    setLoading(true);
    try {
      const roomData = await roomApi.getRoomById(roomId);
      setRoom(roomData);
      
      const typeData = await roomApi.getRoomTypeById(roomData.roomTypeId);
      setRoomType(typeData);
    } catch (error) {
      console.error('Error loading room detail:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleStatusChange = async (newStatus: string) => {
    Alert.alert(
      'Durum Değiştir',
      `Oda durumunu "${newStatus}" olarak değiştirmek istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Değiştir',
          onPress: async () => {
            try {
              await roomApi.updateRoomStatus(roomId, newStatus);
              await loadRoomDetail();
              Alert.alert('Başarılı', 'Oda durumu güncellendi');
            } catch (error) {
              Alert.alert('Hata', 'Durum güncellenemedi');
            }
          }
        }
      ]
    );
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Available': return '#16a34a';
      case 'Occupied': return '#dc2626';
      case 'Maintenance': return '#f59e0b';
      case 'Cleaning': return '#3b82f6';
      default: return '#6b7280';
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!room || !roomType) {
    return (
      <View style={styles.center}>
        <Text>Oda bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.roomNumber}>Oda {room.roomNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(room.status) }]}>
          <Text style={styles.statusText}>{room.status}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>🏨 Otel Bilgisi</Text>
        <Text style={styles.hotelName}>{room.hotelName}</Text>
        <Text style={styles.roomTypeName}>{roomType.name}</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>📋 Oda Özellikleri</Text>
        <View style={styles.featureGrid}>
          <View style={styles.featureItem}>
            <Text style={styles.featureLabel}>Kat</Text>
            <Text style={styles.featureValue}>{room.floor}</Text>
          </View>
          <View style={styles.featureItem}>
            <Text style={styles.featureLabel}>Manzara</Text>
            <Text style={styles.featureValue}>{room.view}</Text>
          </View>
          <View style={styles.featureItem}>
            <Text style={styles.featureLabel}>Kapasite</Text>
            <Text style={styles.featureValue}>{room.capacity} kişi</Text>
          </View>
          <View style={styles.featureItem}>
            <Text style={styles.featureLabel}>Ekstra Yatak</Text>
            <Text style={styles.featureValue}>{room.extraBedCapacity}</Text>
          </View>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>💰 Fiyat Bilgisi</Text>
        <Text style={styles.price}>€{room.basePrice}</Text>
        <Text style={styles.perNight}>/ gece</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>✨ Olanaklar</Text>
        <View style={styles.amenitiesGrid}>
          {room.amenities?.map((amenity, index) => (
            <View key={index} style={styles.amenityItem}>
              <Text style={styles.amenityText}>✓ {amenity}</Text>
            </View>
          ))}
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>📝 Açıklama</Text>
        <Text style={styles.description}>{room.description}</Text>
      </View>

      <View style={styles.actionSection}>
        <Text style={styles.sectionTitle}>⚙️ Durum Değiştir</Text>
        <View style={styles.statusButtons}>
          <TouchableOpacity
            style={[styles.statusButton, room.status === 'Available' && styles.statusButtonActive]}
            onPress={() => handleStatusChange('Available')}
          >
            <Text style={styles.statusButtonText}>Müsait</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.statusButton, room.status === 'Occupied' && styles.statusButtonActive]}
            onPress={() => handleStatusChange('Occupied')}
          >
            <Text style={styles.statusButtonText}>Dolu</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.statusButton, room.status === 'Cleaning' && styles.statusButtonActive]}
            onPress={() => handleStatusChange('Cleaning')}
          >
            <Text style={styles.statusButtonText}>Temizlikte</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.statusButton, room.status === 'Maintenance' && styles.statusButtonActive]}
            onPress={() => handleStatusChange('Maintenance')}
          >
            <Text style={styles.statusButtonText}>Bakımda</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
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
  headerCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    margin: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  roomNumber: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginHorizontal: 16,
    marginBottom: 12,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  hotelName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
    marginBottom: 4,
  },
  roomTypeName: {
    fontSize: 14,
    color: '#666',
  },
  featureGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  featureItem: {
    width: '50%',
    marginBottom: 12,
  },
  featureLabel: {
    fontSize: 12,
    color: '#888',
  },
  featureValue: {
    fontSize: 16,
    fontWeight: '500',
    color: '#333',
    marginTop: 4,
  },
  price: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  perNight: {
    fontSize: 14,
    color: '#888',
    marginTop: 4,
  },
  amenitiesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  amenityItem: {
    width: '50%',
    marginBottom: 8,
  },
  amenityText: {
    fontSize: 14,
    color: '#555',
  },
  description: {
    fontSize: 14,
    color: '#666',
    lineHeight: 20,
  },
  actionSection: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginHorizontal: 16,
    marginBottom: 20,
  },
  statusButtons: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  statusButton: {
    paddingHorizontal: 16,
    paddingVertical: 10,
    borderRadius: 8,
    backgroundColor: '#f0f0f0',
    marginRight: 8,
    marginBottom: 8,
  },
  statusButtonActive: {
    backgroundColor: '#007AFF',
  },
  statusButtonText: {
    color: '#333',
  },
});