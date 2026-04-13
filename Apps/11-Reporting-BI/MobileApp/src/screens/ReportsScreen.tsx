import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import DatePicker from 'react-native-datepicker';
import { reportApi } from '../services/reportApi';

export const ReportsScreen = ({ navigation, route }: any) => {
  const { hotelId } = route.params;
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(false);

  const reportTypes = [
    { id: 'Revenue', name: 'Gelir Raporu', icon: '💰', color: '#10b981' },
    { id: 'Occupancy', name: 'Doluluk Raporu', icon: '🏨', color: '#3b82f6' },
    { id: 'Reservation', name: 'Rezervasyon Raporu', icon: '📅', color: '#8b5cf6' },
    { id: 'Customer', name: 'Müşteri Raporu', icon: '👥', color: '#f59e0b' },
    { id: 'Channel', name: 'Kanal Raporu', icon: '📡', color: '#ec4899' }
  ];

  const handleGenerateReport = async (reportType: string) => {
    if (!startDate || !endDate) {
      Alert.alert('Uyarı', 'Lütfen başlangıç ve bitiş tarihlerini seçin');
      return;
    }

    setLoading(true);
    try {
      const filePath = await reportApi.exportToExcel({
        hotelId,
        reportType,
        startDate,
        endDate,
        format: 'Excel'
      });
      
      await reportApi.downloadAndShare(filePath);
    } catch (error) {
      Alert.alert('Hata', 'Rapor oluşturulamadı');
    } finally {
      setLoading(false);
    }
  };

  const handleViewReport = (reportType: string) => {
    if (!startDate || !endDate) {
      Alert.alert('Uyarı', 'Lütfen başlangıç ve bitiş tarihlerini seçin');
      return;
    }

    navigation.navigate(`${reportType}Report`, {
      hotelId,
      startDate,
      endDate
    });
  };

  return (
    <ScrollView style={styles.container}>
      <View style={styles.dateCard}>
        <Text style={styles.sectionTitle}>Tarih Aralığı</Text>
        
        <View style={styles.dateField}>
          <Text style={styles.label}>Başlangıç Tarihi</Text>
          <DatePicker
            style={styles.datePicker}
            date={startDate}
            mode="date"
            placeholder="Seçiniz"
            format="YYYY-MM-DD"
            confirmBtnText="Tamam"
            cancelBtnText="İptal"
            onDateChange={(date) => setStartDate(date)}
          />
        </View>
        
        <View style={styles.dateField}>
          <Text style={styles.label}>Bitiş Tarihi</Text>
          <DatePicker
            style={styles.datePicker}
            date={endDate}
            mode="date"
            placeholder="Seçiniz"
            format="YYYY-MM-DD"
            confirmBtnText="Tamam"
            cancelBtnText="İptal"
            onDateChange={(date) => setEndDate(date)}
          />
        </View>
      </View>

      <Text style={styles.sectionTitle}>Rapor Tipleri</Text>
      
      {reportTypes.map((report) => (
        <View key={report.id} style={styles.reportCard}>
          <View style={styles.reportHeader}>
            <View style={[styles.reportIcon, { backgroundColor: report.color }]}>
              <Text style={styles.reportIconText}>{report.icon}</Text>
            </View>
            <View>
              <Text style={styles.reportName}>{report.name}</Text>
              <Text style={styles.reportDesc}>
                {report.id === 'Revenue' && 'Gelir, ADR, RevPAR analizi'}
                {report.id === 'Occupancy' && 'Doluluk oranları ve trendler'}
                {report.id === 'Reservation' && 'Rezervasyon, iptal, no-show analizi'}
                {report.id === 'Customer' && 'Müşteri segmentasyonu ve sadakat'}
                {report.id === 'Channel' && 'Kanal performans karşılaştırması'}
              </Text>
            </View>
          </View>
          <View style={styles.reportButtons}>
            <TouchableOpacity
              style={[styles.reportButton, styles.viewButton]}
              onPress={() => handleViewReport(report.id)}
            >
              <Text style={styles.viewButtonText}>Görüntüle</Text>
            </TouchableOpacity>
            <TouchableOpacity
              style={[styles.reportButton, styles.downloadButton]}
              onPress={() => handleGenerateReport(report.id)}
            >
              <Text style={styles.downloadButtonText}>Excel İndir</Text>
            </TouchableOpacity>
          </View>
        </View>
      ))}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  dateCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
    marginHorizontal: 16,
    marginTop: 8,
  },
  dateField: {
    marginBottom: 16,
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
  reportCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  reportHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 16,
  },
  reportIcon: {
    width: 50,
    height: 50,
    borderRadius: 25,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 12,
  },
  reportIconText: {
    fontSize: 24,
  },
  reportName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  reportDesc: {
    fontSize: 12,
    color: '#888',
    marginTop: 2,
  },
  reportButtons: {
    flexDirection: 'row',
    gap: 12,
  },
  reportButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 8,
    alignItems: 'center',
  },
  viewButton: {
    backgroundColor: '#f0f0f0',
  },
  downloadButton: {
    backgroundColor: '#007AFF',
  },
  viewButtonText: {
    color: '#666',
    fontWeight: 'bold',
  },
  downloadButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});