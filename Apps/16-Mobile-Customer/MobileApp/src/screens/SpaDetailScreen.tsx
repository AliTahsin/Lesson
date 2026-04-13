import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  TextInput
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { customerApi } from '../services/customerApi';
import { SpaService } from '../types/customer';

export const SpaDetailScreen = ({ route, navigation }: any) => {
  const { service } = route.params;
  const { t } = useLanguageContext();
  const [appointmentDate, setAppointmentDate] = useState('');
  const [appointmentTime, setAppointmentTime] = useState('');
  const [availableTimes, setAvailableTimes] = useState<string[]>([]);
  const [specialRequests, setSpecialRequests] = useState('');
  const [loading, setLoading] = useState(false);
  const [booking, setBooking] = useState(false);

  const loadAvailableTimes = async (date: string) => {
    if (!date) return;
    setLoading(true);
    try {
      const times = await customerApi.getAvailableTimes(date, service.id);
      setAvailableTimes(times);
    } catch (error) {
      console.error('Error loading available times:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDateChange = (date: string) => {
    setAppointmentDate(date);
    setAppointmentTime('');
    loadAvailableTimes(date);
  };

  const handleBooking = async () => {
    if (!appointmentDate || !appointmentTime) {
      Alert.alert('Uyarı', 'Lütfen tarih ve saat seçin');
      return;
    }

    setBooking(true);
    try {
      await customerApi.createAppointment({
        serviceId: service.id,
        appointmentDate,
        appointmentTime,
        specialRequests
      });
      Alert.alert(
        'Başarılı',
        'Randevunuz oluşturuldu. Onay için size bildirim gönderilecektir.',
        [{ text: 'Tamam', onPress: () => navigation.goBack() }]
      );
    } catch (error) {
      Alert.alert('Hata', 'Randevu oluşturulamadı');
    } finally {
      setBooking(false);
    }
  };

  const timeSlots = availableTimes.map(time => ({
    time,
    selected: appointmentTime === time
  }));

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.serviceName}>{service.name}</Text>
        <Text style={styles.serviceCategory}>{service.category}</Text>
        <Text style={styles.serviceDescription}>{service.description}</Text>
        <View style={styles.serviceDetails}>
          <View style={styles.detailItem}>
            <Icon name="time-outline" size={20} color="#666" />
            <Text style={styles.detailText}>{service.durationMinutes} {t('minutes')}</Text>
          </View>
          <View style={styles.detailItem}>
            <Icon name="cash-outline" size={20} color="#666" />
            <Text style={styles.priceText}>€{service.price}</Text>
          </View>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>{t('select_date')}</Text>
        <DatePicker
          style={styles.datePicker}
          date={appointmentDate}
          mode="date"
          placeholder="Tarih seçin"
          format="YYYY-MM-DD"
          minDate={new Date().toISOString().split('T')[0]}
          confirmBtnText="Tamam"
          cancelBtnText="İptal"
          onDateChange={handleDateChange}
        />
      </View>

      {loading && (
        <View style={styles.loader}>
          <ActivityIndicator size="small" />
          <Text style={styles.loaderText}>Müsait saatler yükleniyor...</Text>
        </View>
      )}

      {availableTimes.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>{t('select_time')}</Text>
          <View style={styles.timeGrid}>
            {timeSlots.map((slot, index) => (
              <TouchableOpacity
                key={index}
                style={[styles.timeButton, slot.selected && styles.timeButtonSelected]}
                onPress={() => setAppointmentTime(slot.time)}
              >
                <Text style={[styles.timeText, slot.selected && styles.timeTextSelected]}>
                  {slot.time}
                </Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>
      )}

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Özel İstekler</Text>
        <TextInput
          style={styles.textArea}
          placeholder="Özel istekleriniz varsa belirtin..."
          value={specialRequests}
          onChangeText={setSpecialRequests}
          multiline
          numberOfLines={3}
        />
      </View>

      <TouchableOpacity
        style={[styles.bookButton, booking && styles.disabledButton]}
        onPress={handleBooking}
        disabled={booking}
      >
        <Text style={styles.bookButtonText}>
          {booking ? 'Randevu Oluşturuluyor...' : t('book_appointment')}
        </Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  headerCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    margin: 16,
    padding: 20,
    elevation: 2,
  },
  serviceName: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 4,
  },
  serviceCategory: {
    fontSize: 14,
    color: '#007AFF',
    marginBottom: 12,
  },
  serviceDescription: {
    fontSize: 14,
    color: '#666',
    lineHeight: 20,
    marginBottom: 16,
  },
  serviceDetails: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingTop: 16,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  detailItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
  },
  detailText: {
    fontSize: 14,
    color: '#666',
  },
  priceText: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  datePicker: {
    width: '100%',
  },
  loader: {
    alignItems: 'center',
    paddingVertical: 20,
  },
  loaderText: {
    marginTop: 8,
    color: '#666',
  },
  timeGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 10,
  },
  timeButton: {
    paddingHorizontal: 16,
    paddingVertical: 10,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  timeButtonSelected: {
    backgroundColor: '#007AFF',
  },
  timeText: {
    fontSize: 14,
    color: '#333',
  },
  timeTextSelected: {
    color: 'white',
  },
  textArea: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    minHeight: 80,
    textAlignVertical: 'top',
  },
  bookButton: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 16,
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  bookButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
}); 