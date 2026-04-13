import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Image
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { customerApi } from '../services/customerApi';

export const HomeScreen = ({ navigation }: any) => {
  const { t } = useLanguageContext();
  const [loading, setLoading] = useState(true);
  const [upcomingReservation, setUpcomingReservation] = useState<any>(null);
  const [profile, setProfile] = useState<any>(null);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const profileData = await customerApi.getProfile();
      setProfile(profileData);
      // Mock upcoming reservation
      setUpcomingReservation({
        id: 1,
        hotelName: 'Marriott Istanbul',
        roomNumber: '101',
        checkInDate: new Date(2024, 5, 15),
        checkOutDate: new Date(2024, 5, 18),
      });
    } catch (error) {
      console.error('Error loading home data:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (date: Date) => {
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      {/* Welcome Section */}
      <View style={styles.welcomeCard}>
        <View>
          <Text style={styles.welcomeText}>{t('welcome')},</Text>
          <Text style={styles.userName}>{profile?.firstName || 'Guest'}!</Text>
        </View>
        <Image
          source={{ uri: profile?.profileImageUrl || 'https://randomuser.me/api/portraits/lego/1.jpg' }}
          style={styles.avatar}
        />
      </View>

      {/* Upcoming Stay */}
      {upcomingReservation && (
        <View style={styles.card}>
          <Text style={styles.cardTitle}>{t('upcoming_stay')}</Text>
          <Text style={styles.hotelName}>{upcomingReservation.hotelName}</Text>
          <View style={styles.reservationDetails}>
            <View style={styles.dateRow}>
              <Icon name="calendar-outline" size={16} color="#666" />
              <Text style={styles.dateText}>
                {formatDate(upcomingReservation.checkInDate)} - {formatDate(upcomingReservation.checkOutDate)}
              </Text>
            </View>
            <View style={styles.roomRow}>
              <Icon name="bed-outline" size={16} color="#666" />
              <Text style={styles.roomText}>{t('room_number')}: {upcomingReservation.roomNumber}</Text>
            </View>
          </View>
          <TouchableOpacity
            style={styles.viewButton}
            onPress={() => navigation.navigate('DigitalKey')}
          >
            <Text style={styles.viewButtonText}>{t('view_reservation')}</Text>
          </TouchableOpacity>
        </View>
      )}

      {/* Quick Actions */}
      <View style={styles.quickActions}>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => navigation.navigate('DigitalKey')}
        >
          <Icon name="key-outline" size={28} color="#007AFF" />
          <Text style={styles.actionText}>{t('digital_key')}</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => navigation.navigate('RoomService')}
        >
          <Icon name="restaurant-outline" size={28} color="#007AFF" />
          <Text style={styles.actionText}>{t('room_service')}</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => navigation.navigate('Spa')}
        >
          <Icon name="flower-outline" size={28} color="#007AFF" />
          <Text style={styles.actionText}>{t('spa')}</Text>
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
  welcomeCard: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: '#007AFF',
    margin: 16,
    padding: 20,
    borderRadius: 16,
  },
  welcomeText: {
    color: 'white',
    fontSize: 14,
    opacity: 0.9,
  },
  userName: {
    color: 'white',
    fontSize: 24,
    fontWeight: 'bold',
  },
  avatar: {
    width: 60,
    height: 60,
    borderRadius: 30,
    borderWidth: 2,
    borderColor: 'white',
  },
  card: {
    backgroundColor: 'white',
    borderRadius: 16,
    marginHorizontal: 16,
    marginBottom: 16,
    padding: 16,
    elevation: 2,
  },
  cardTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  hotelName: {
    fontSize: 18,
    fontWeight: '600',
    color: '#007AFF',
    marginBottom: 8,
  },
  reservationDetails: {
    marginBottom: 16,
  },
  dateRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
    marginBottom: 8,
  },
  dateText: {
    fontSize: 14,
    color: '#666',
  },
  roomRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  roomText: {
    fontSize: 14,
    color: '#666',
  },
  viewButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 10,
    alignItems: 'center',
  },
  viewButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  quickActions: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginHorizontal: 16,
    marginBottom: 20,
  },
  actionButton: {
    alignItems: 'center',
    backgroundColor: 'white',
    padding: 16,
    borderRadius: 12,
    width: '30%',
    elevation: 2,
  },
  actionText: {
    fontSize: 12,
    color: '#333',
    marginTop: 8,
  },
});