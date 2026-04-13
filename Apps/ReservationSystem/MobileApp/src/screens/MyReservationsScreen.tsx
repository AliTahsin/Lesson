import React, { useState } from 'react';
import {
  View,
  Text,
  FlatList,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert
} from 'react-native';
import { reservationApi } from '../services/reservationApi';
import { ReservationCard } from '../components/ReservationCard';
import { Reservation } from '../types/reservation';

export const MyReservationsScreen = ({ navigation }: any) => {
  const [email, setEmail] = useState('');
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState(false);
  const [searched, setSearched] = useState(false);

  const loadReservations = async () => {
    if (!email) {
      Alert.alert('Uyarı', 'Lütfen e-posta adresinizi girin');
      return;
    }

    if (!email.includes('@')) {
      Alert.alert('Hata', 'Geçerli bir e-posta adresi girin');
      return;
    }

    setLoading(true);
    try {
      const data = await reservationApi.getReservationsByGuestEmail(email);
      setReservations(data);
      setSearched(true);
    } catch (error) {
      Alert.alert('Hata', 'Rezervasyonlar yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleCancelReservation = (id: number) => {
    Alert.alert(
      'Rezervasyon İptali',
      'Bu rezervasyonu iptal etmek istediğinize emin misiniz?',
      [
        { text: 'Hayır', style: 'cancel' },
        {
          text: 'Evet',
          onPress: async () => {
            try {
              await reservationApi.cancelReservation(id, 'Kullanıcı iptali');
              Alert.alert('Başarılı', 'Rezervasyon iptal edildi');
              loadReservations();
            } catch (error: any) {
              Alert.alert('Hata', error.response?.data?.error || 'İptal edilemedi');
            }
          }
        }
      ]
    );
  };

  const handleReservationPress = (id: number) => {
    navigation.navigate('ReservationDetail', { reservationId: id });
  };

  return (
    <View style={styles.container}>
      <View style={styles.searchContainer}>
        <TextInput
          style={styles.emailInput}
          placeholder="E-posta adresiniz"
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />
        <TouchableOpacity style={styles.searchButton} onPress={loadReservations}>
          <Text style={styles.searchButtonText}>Rezervasyonlarım</Text>
        </TouchableOpacity>
      </View>

      {loading ? (
        <ActivityIndicator size="large" style={styles.loader} />
      ) : searched && reservations.length === 0 ? (
        <View style={styles.emptyContainer}>
          <Text style={styles.emptyText}>Rezervasyon bulunamadı</Text>
          <Text style={styles.emptySubtext}>Bu e-posta ile yapılmış rezervasyon yok</Text>
        </View>
      ) : (
        <FlatList
          data={reservations}
          keyExtractor={(item) => item.id.toString()}
          renderItem={({ item }) => (
            <ReservationCard
              reservation={item}
              onPress={handleReservationPress}
              onCancel={handleCancelReservation}
            />
          )}
          contentContainerStyle={styles.listContent}
        />
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  searchContainer: {
    backgroundColor: 'white',
    padding: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  emailInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
    marginBottom: 12,
  },
  searchButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
  },
  searchButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  listContent: {
    paddingVertical: 8,
  },
  loader: {
    flex: 1,
    justifyContent: 'center',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#666',
  },
  emptySubtext: {
    fontSize: 14,
    color: '#888',
    marginTop: 8,
  },
});