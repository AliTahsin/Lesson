import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  Image,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { meetingApi } from '../services/meetingApi';
import { MeetingRoom } from '../types/mice';

export const MeetingRoomDetailScreen = ({ route, navigation }: any) => {
  const { roomId } = route.params;
  const [room, setRoom] = useState<MeetingRoom | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadRoom();
  }, []);

  const loadRoom = async () => {
    setLoading(true);
    try {
      const data = await meetingApi.getRoomById(roomId);
      setRoom(data);
    } catch (error) {
      console.error('Error loading room:', error);
      Alert.alert('Hata', 'Oda detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const getCapacityConfig = (capacity: number) => {
    if (capacity >= 200) return { icon: '🏟️', text: 'Büyük Salon' };
    if (capacity >= 50) return { icon: '🏛️', text: 'Konferans Salonu' };
    if (capacity >= 20) return { icon: '🏠', text: 'Toplantı Odası' };
    return { icon: '📏', text: 'Küçük Oda' };
  };

  const capacityConfig = room ? getCapacityConfig(room.capacity) : { icon: '', text: '' };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!room) {
    return (
      <View style={styles.center}>
        <Text>Oda bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <Image
        source={{ uri: room.images?.[0] || `https://picsum.photos/400/300?random=${room.id}` }}
        style={styles.mainImage}
      />
      
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.name}>{room.name}</Text>
          <Text style={styles.code}>{room.roomCode}</Text>
        </View>
        
        <View style={styles.capacityCard}>
          <Text style={styles.capacityIcon}>{capacityConfig.icon}</Text>
          <View>
            <Text style={styles.capacityTitle}>{capacityConfig.text}</Text>
            <Text style={styles.capacityValue}>{room.capacity} kişi kapasite</Text>
          </View>
        </View>
        
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Kapasiteler</Text>
          <View style={styles.capacitiesGrid}>
            <View style={styles.capacityItem}>
              <Text style={styles.capacityLabel}>Tiyatro</Text>
              <Text style={styles.capacityNumber}>{room.theaterCapacity}</Text>
            </View>
            <View style={styles.capacityItem}>
              <Text style={styles.capacityLabel}>Sınıf</Text>
              <Text style={styles.capacityNumber}>{room.classroomCapacity}</Text>
            </View>
            <View style={styles.capacityItem}>
              <Text style={styles.capacityLabel}>U Şekli</Text>
              <Text style={styles.capacityNumber}>{room.uShapeCapacity}</Text>
            </View>
            <View style={styles.capacityItem}>
              <Text style={styles.capacityLabel}>Toplantı</Text>
              <Text style={styles.capacityNumber}>{room.boardroomCapacity}</Text>
            </View>
          </View>
        </View>
        
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Fiyatlar</Text>
          <View style={styles.priceCard}>
            <View style={styles.priceRow}>
              <Text style={styles.priceLabel}>Yarım Gün (4 saat)</Text>
              <Text style={styles.priceValue}>€{room.halfDayPrice}</Text>
            </View>
            <View style={styles.priceRow}>
              <Text style={styles.priceLabel}>Tam Gün (8 saat)</Text>
              <Text style={styles.priceValue}>€{room.fullDayPrice}</Text>
            </View>
          </View>
        </View>
        
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Donanım ve Olanaklar</Text>
          <View style={styles.amenitiesGrid}>
            {room.hasProjector && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>📽️</Text>
                <Text style={styles.amenityName}>Projeksiyon</Text>
              </View>
            )}
            {room.hasSoundSystem && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>🔊</Text>
                <Text style={styles.amenityName}>Ses Sistemi</Text>
              </View>
            )}
            {room.hasWhiteboard && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>📝</Text>
                <Text style={styles.amenityName}>Beyaz Tahta</Text>
              </View>
            )}
            {room.hasFlipChart && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>📊</Text>
                <Text style={styles.amenityName}>Flip Chart</Text>
              </View>
            )}
            {room.hasWiFi && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>📶</Text>
                <Text style={styles.amenityName}>WiFi</Text>
              </View>
            )}
            {room.hasNaturalLight && (
              <View style={styles.amenityItem}>
                <Text style={styles.amenityIcon}>☀️</Text>
                <Text style={styles.amenityName}>Doğal Işık</Text>
              </View>
            )}
          </View>
        </View>
        
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Konum</Text>
          <Text style={styles.locationText}>{room.floor}</Text>
          <Text style={styles.areaText}>Alan: {room.area} m²</Text>
        </View>
        
        <TouchableOpacity
          style={styles.bookButton}
          onPress={() => navigation.navigate('CreateEvent', { roomId: room.id, roomName: room.name })}
        >
          <Text style={styles.bookButtonText}>Etkinlik Oluştur</Text>
        </TouchableOpacity>
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
  mainImage: {
    width: '100%',
    height: 220,
  },
  content: {
    padding: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
  },
  name: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#333',
  },
  code: {
    fontSize: 14,
    color: '#888',
  },
  capacityCard: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    padding: 16,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    marginBottom: 16,
  },
  capacityIcon: {
    fontSize: 32,
  },
  capacityTitle: {
    fontSize: 14,
    color: 'white',
    opacity: 0.9,
  },
  capacityValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginBottom: 12,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  capacitiesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  capacityItem: {
    width: '50%',
    marginBottom: 12,
  },
  capacityLabel: {
    fontSize: 12,
    color: '#888',
  },
  capacityNumber: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
  },
  priceCard: {
    gap: 12,
  },
  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  priceLabel: {
    fontSize: 14,
    color: '#666',
  },
  priceValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  amenitiesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 12,
  },
  amenityItem: {
    alignItems: 'center',
    width: '23%',
  },
  amenityIcon: {
    fontSize: 28,
    marginBottom: 4,
  },
  amenityName: {
    fontSize: 11,
    color: '#666',
    textAlign: 'center',
  },
  locationText: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  areaText: {
    fontSize: 14,
    color: '#666',
  },
  bookButton: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    paddingVertical: 14,
    alignItems: 'center',
    marginTop: 8,
    marginBottom: 20,
  },
  bookButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});