import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  ScrollView
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { useStaffAuthContext } from '../context/StaffAuthContext';

export const CheckOutScreen = ({ navigation }: any) => {
  const { hotelId, staff } = useStaffAuthContext();
  const [reservationId, setReservationId] = useState('');
  const [guestId, setGuestId] = useState('');
  const [guestName, setGuestName] = useState('');
  const [roomId, setRoomId] = useState('');
  const [roomNumber, setRoomNumber] = useState('');
  const [notes, setNotes] = useState('');
  const [loading, setLoading] = useState(false);

  const handleCheckOut = async () => {
    if (!reservationId || !guestId || !guestName || !roomId || !roomNumber) {
      Alert.alert('Hata', 'Lütfen tüm zorunlu alanları doldurun');
      return;
    }

    setLoading(true);
    try {
      await staffApi.checkOut({
        reservationId: parseInt(reservationId),
        guestId: parseInt(guestId),
        guestName,
        roomId: parseInt(roomId),
        roomNumber,
        hotelId: hotelId || 0,
        notes
      });

      Alert.alert(
        'Başarılı',
        'Check-out işlemi tamamlandı',
        [{ text: 'Tamam', onPress: () => navigation.goBack() }]
      );
    } catch (error: any) {
      Alert.alert('Hata', error.response?.data?.error || 'Check-out işlemi başarısız');
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView style={styles.container}>
      <View style={styles.card}>
        <Text style={styles.title}>Check-out İşlemi</Text>

        <Text style={styles.label}>Rezervasyon No *</Text>
        <TextInput
          style={styles.input}
          placeholder="Rezervasyon numarası"
          value={reservationId}
          onChangeText={setReservationId}
          keyboardType="numeric"
        />

        <Text style={styles.label}>Misafir ID *</Text>
        <TextInput
          style={styles.input}
          placeholder="Misafir ID"
          value={guestId}
          onChangeText={setGuestId}
          keyboardType="numeric"
        />

        <Text style={styles.label}>Misafir Adı Soyadı *</Text>
        <TextInput
          style={styles.input}
          placeholder="Misafir adı soyadı"
          value={guestName}
          onChangeText={setGuestName}
        />

        <Text style={styles.label}>Oda ID *</Text>
        <TextInput
          style={styles.input}
          placeholder="Oda ID"
          value={roomId}
          onChangeText={setRoomId}
          keyboardType="numeric"
        />

        <Text style={styles.label}>Oda No *</Text>
        <TextInput
          style={styles.input}
          placeholder="Oda numarası"
          value={roomNumber}
          onChangeText={setRoomNumber}
        />

        <Text style={styles.label}>Notlar</Text>
        <TextInput
          style={[styles.input, styles.textArea]}
          placeholder="Varsa notlar..."
          value={notes}
          onChangeText={setNotes}
          multiline
          numberOfLines={3}
        />

        <TouchableOpacity
          style={[styles.button, loading && styles.disabledButton]}
          onPress={handleCheckOut}
          disabled={loading}
        >
          {loading ? (
            <ActivityIndicator color="white" />
          ) : (
            <Text style={styles.buttonText}>Check-out Yap</Text>
          )}
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
  card: {
    backgroundColor: 'white',
    borderRadius: 16,
    margin: 16,
    padding: 20,
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 20,
    textAlign: 'center',
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
    marginTop: 12,
  },
  input: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
  },
  textArea: {
    minHeight: 80,
    textAlignVertical: 'top',
  },
  button: {
    backgroundColor: '#f59e0b',
    borderRadius: 8,
    paddingVertical: 14,
    alignItems: 'center',
    marginTop: 20,
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  buttonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});