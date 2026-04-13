import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  Switch
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import { pricingApi } from '../services/pricingApi';
import { PriceBreakdownCard } from '../components/PriceBreakdownCard';
import { PriceResponse } from '../types/pricing';

export const PriceCalculatorScreen = () => {
  const [roomId, setRoomId] = useState('1');
  const [checkInDate, setCheckInDate] = useState('');
  const [checkOutDate, setCheckOutDate] = useState('');
  const [guestCount, setGuestCount] = useState('2');
  const [promoCode, setPromoCode] = useState('');
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<PriceResponse | null>(null);
  const [showBreakdown, setShowBreakdown] = useState(true);

  const rooms = [
    { id: 1, name: 'Standard Oda - Marriott Istanbul', price: 150 },
    { id: 2, name: 'Deluxe Oda - Marriott Istanbul', price: 250 },
    { id: 3, name: 'Suite - Marriott Istanbul', price: 450 },
    { id: 4, name: 'Standard Oda - Hilton Izmir', price: 120 },
    { id: 5, name: 'Deluxe Oda - Hilton Izmir', price: 200 },
    { id: 6, name: 'Standard Oda - Sofitel Bodrum', price: 180 },
    { id: 7, name: 'Suite - Sofitel Bodrum', price: 500 },
  ];

  const calculatePrice = async () => {
    if (!checkInDate || !checkOutDate) {
      Alert.alert('Hata', 'Lütfen giriş ve çıkış tarihlerini seçin');
      return;
    }

    if (new Date(checkInDate) >= new Date(checkOutDate)) {
      Alert.alert('Hata', 'Çıkış tarihi giriş tarihinden sonra olmalı');
      return;
    }

    setLoading(true);
    try {
      const response = await pricingApi.calculatePrice({
        roomId: parseInt(roomId),
        checkInDate,
        checkOutDate,
        guestCount: parseInt(guestCount),
        promoCode: promoCode || undefined
      });
      setResult(response);
    } catch (error) {
      Alert.alert('Hata', 'Fiyat hesaplanırken bir sorun oluştu');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const getSelectedRoom = () => {
    return rooms.find(r => r.id === parseInt(roomId));
  };

  const selectedRoom = getSelectedRoom();

  return (
    <ScrollView style={styles.container}>
      <View style={styles.formCard}>
        <Text style={styles.sectionTitle}>Oda Seçimi</Text>
        
        <View style={styles.roomSelector}>
          {rooms.map(room => (
            <TouchableOpacity
              key={room.id}
              style={[
                styles.roomButton,
                parseInt(roomId) === room.id && styles.roomButtonActive
              ]}
              onPress={() => setRoomId(room.id.toString())}
            >
              <Text style={[
                styles.roomButtonText,
                parseInt(roomId) === room.id && styles.roomButtonTextActive
              ]}>
                {room.name.split(' - ')[0]}
              </Text>
              <Text style={styles.roomPrice}>€{room.price}</Text>
            </TouchableOpacity>
          ))}
        </View>

        <Text style={styles.selectedRoomName}>{selectedRoom?.name}</Text>

        <Text style={styles.sectionTitle}>Tarihler</Text>
        
        <View style={styles.dateContainer}>
          <View style={styles.dateField}>
            <Text style={styles.label}>Giriş Tarihi</Text>
            <DatePicker
              style={styles.datePicker}
              date={checkInDate}
              mode="date"
              placeholder="Seçiniz"
              format="YYYY-MM-DD"
              confirmBtnText="Tamam"
              cancelBtnText="İptal"
              onDateChange={(date) => setCheckInDate(date)}
            />
          </View>
          
          <View style={styles.dateField}>
            <Text style={styles.label}>Çıkış Tarihi</Text>
            <DatePicker
              style={styles.datePicker}
              date={checkOutDate}
              mode="date"
              placeholder="Seçiniz"
              format="YYYY-MM-DD"
              confirmBtnText="Tamam"
              cancelBtnText="İptal"
              onDateChange={(date) => setCheckOutDate(date)}
            />
          </View>
        </View>

        <View style={styles.guestContainer}>
          <Text style={styles.label}>Misafir Sayısı</Text>
          <TextInput
            style={styles.guestInput}
            value={guestCount}
            onChangeText={setGuestCount}
            keyboardType="numeric"
            maxLength={2}
          />
        </View>

        <View style={styles.promoContainer}>
          <Text style={styles.label}>Promosyon Kodu</Text>
          <TextInput
            style={styles.promoInput}
            placeholder="Varsa giriniz"
            value={promoCode}
            onChangeText={setPromoCode}
            autoCapitalize="characters"
          />
        </View>

        <TouchableOpacity style={styles.calculateButton} onPress={calculatePrice}>
          <Text style={styles.calculateButtonText}>Fiyat Hesapla</Text>
        </TouchableOpacity>
      </View>

      {loading && (
        <ActivityIndicator size="large" style={styles.loader} />
      )}

      {result && (
        <>
          <View style={styles.summaryCard}>
            <Text style={styles.summaryTitle}>Fiyat Özeti</Text>
            
            <View style={styles.summaryRow}>
              <Text style={styles.summaryLabel}>Konaklama Süresi</Text>
              <Text style={styles.summaryValue}>{result.nightCount} gece</Text>
            </View>
            
            <View style={styles.summaryRow}>
              <Text style={styles.summaryLabel}>Ortalama Gecelik</Text>
              <Text style={styles.summaryValue}>€{result.averagePricePerNight.toFixed(2)}</Text>
            </View>

            {result.longStayDiscount > 0 && (
              <View style={styles.summaryRow}>
                <Text style={styles.summaryLabel}>Uzun Konaklama İndirimi</Text>
                <Text style={styles.discountValue}>-%{(result.longStayDiscount * 100).toFixed(0)}</Text>
              </View>
            )}

            <View style={styles.totalRow}>
              <Text style={styles.totalLabel}>Toplam Fiyat</Text>
              <Text style={styles.totalValue}>€{result.totalPrice.toFixed(2)}</Text>
            </View>
          </View>

          <View style={styles.breakdownHeader}>
            <Text style={styles.breakdownTitle}>Günlük Fiyat Detayı</Text>
            <View style={styles.switchContainer}>
              <Text style={styles.switchLabel}>Detaylı Gösterim</Text>
              <Switch
                value={showBreakdown}
                onValueChange={setShowBreakdown}
                trackColor={{ false: '#ddd', true: '#007AFF' }}
              />
            </View>
          </View>

          {result.dailyPrices.map((daily, index) => (
            <PriceBreakdownCard
              key={index}
              date={daily.date}
              basePrice={daily.basePrice}
              finalPrice={daily.finalPrice}
              breakdowns={showBreakdown ? daily.breakdowns : []}
              demandScore={daily.demandScore}
              occupancyRate={daily.occupancyRate}
            />
          ))}
        </>
      )}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  formCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    margin: 16,
    elevation: 2,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
    marginTop: 8,
  },
  roomSelector: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    marginBottom: 12,
  },
  roomButton: {
    width: '30%',
    margin: '1.5%',
    padding: 10,
    borderRadius: 8,
    backgroundColor: '#f0f0f0',
    alignItems: 'center',
  },
  roomButtonActive: {
    backgroundColor: '#007AFF',
  },
  roomButtonText: {
    fontSize: 12,
    color: '#333',
    marginBottom: 4,
  },
  roomButtonTextActive: {
    color: 'white',
  },
  roomPrice: {
    fontSize: 11,
    color: '#666',
  },
  selectedRoomName: {
    fontSize: 14,
    color: '#007AFF',
    textAlign: 'center',
    marginBottom: 16,
    paddingVertical: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  dateContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 16,
  },
  dateField: {
    flex: 1,
    marginHorizontal: 4,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
  },
  datePicker: {
    width: '100%',
  },
  guestContainer: {
    marginBottom: 16,
  },
  guestInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 8,
    fontSize: 16,
    width: 80,
  },
  promoContainer: {
    marginBottom: 20,
  },
  promoInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
  },
  calculateButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 14,
    alignItems: 'center',
  },
  calculateButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  loader: {
    marginVertical: 20,
  },
  summaryCard: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    padding: 16,
    margin: 16,
  },
  summaryTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
    marginBottom: 12,
  },
  summaryRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  summaryLabel: {
    fontSize: 14,
    color: 'white',
    opacity: 0.9,
  },
  summaryValue: {
    fontSize: 14,
    color: 'white',
    fontWeight: '500',
  },
  discountValue: {
    fontSize: 14,
    color: '#fbbf24',
    fontWeight: 'bold',
  },
  totalRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 12,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: 'rgba(255,255,255,0.3)',
  },
  totalLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    color: 'white',
  },
  totalValue: {
    fontSize: 24,
    fontWeight: 'bold',
    color: 'white',
  },
  breakdownHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginHorizontal: 16,
    marginTop: 8,
    marginBottom: 8,
  },
  breakdownTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  switchContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  switchLabel: {
    fontSize: 12,
    color: '#666',
    marginRight: 8,
  },
});