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
import Icon from 'react-native-vector-icons/Ionicons';
import { reportApi } from '../services/reportApi';
import { ChannelReport, ChannelPerformance } from '../types/reporting';
import { ChannelBarChart } from '../components/ChannelBarChart';

export const ChannelReportScreen = ({ route }: any) => {
  const { hotelId, startDate, endDate } = route.params;
  const [report, setReport] = useState<ChannelReport | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadReport();
  }, []);

  const loadReport = async () => {
    setLoading(true);
    try {
      const data = await reportApi.getChannelReport(hotelId, startDate, endDate);
      setReport(data);
    } catch (error) {
      console.error('Error loading report:', error);
      Alert.alert('Hata', 'Rapor yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleExport = async () => {
    try {
      const filePath = await reportApi.exportToExcel({
        hotelId,
        reportType: 'Channel',
        startDate,
        endDate,
        format: 'Excel'
      });
      await reportApi.downloadAndShare(filePath);
    } catch (error) {
      Alert.alert('Hata', 'Rapor indirilemedi');
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!report) {
    return (
      <View style={styles.center}>
        <Text>Rapor bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.hotelName}>{report.hotelName}</Text>
        <Text style={styles.period}>
          {new Date(report.startDate).toLocaleDateString('tr-TR')} - {new Date(report.endDate).toLocaleDateString('tr-TR')}
        </Text>
      </View>

      <View style={styles.summaryCard}>
        <Text style={styles.summaryTitle}>Özet</Text>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Toplam Rezervasyon</Text>
          <Text style={styles.summaryValue}>{report.totalBookings}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Toplam Gelir</Text>
          <Text style={styles.summaryValue}>€{report.totalRevenue.toLocaleString()}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Toplam Komisyon</Text>
          <Text style={styles.summaryValue}>€{report.totalCommission.toLocaleString()}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Net Gelir</Text>
          <Text style={styles.summaryValue}>€{report.netRevenue.toLocaleString()}</Text>
        </View>
      </View>

      <ChannelBarChart data={report.channelData} />

      <View style={styles.tableCard}>
        <Text style={styles.tableTitle}>Kanal Performansı</Text>
        <View style={styles.tableHeader}>
          <Text style={[styles.tableCell, styles.channelCol]}>Kanal</Text>
          <Text style={[styles.tableCell, styles.bookingsCol]}>Rez.</Text>
          <Text style={[styles.tableCell, styles.revenueCol]}>Gelir</Text>
          <Text style={[styles.tableCell, styles.commissionCol]}>Komisyon</Text>
          <Text style={[styles.tableCell, styles.netCol]}>Net</Text>
        </View>
        {report.channelData.map((channel: ChannelPerformance, index: number) => (
          <View key={index} style={styles.tableRow}>
            <Text style={[styles.tableCell, styles.channelCol]}>{channel.channelName}</Text>
            <Text style={[styles.tableCell, styles.bookingsCol]}>{channel.bookings}</Text>
            <Text style={[styles.tableCell, styles.revenueCol]}>€{channel.revenue.toLocaleString()}</Text>
            <Text style={[styles.tableCell, styles.commissionCol]}>€{channel.commission.toLocaleString()}</Text>
            <Text style={[styles.tableCell, styles.netCol]}>€{(channel.revenue - channel.commission).toLocaleString()}</Text>
          </View>
        ))}
      </View>

      <TouchableOpacity style={styles.exportButton} onPress={handleExport}>
        <Icon name="download" size={20} color="white" />
        <Text style={styles.exportButtonText}>Excel Olarak İndir</Text>
      </TouchableOpacity>
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
    backgroundColor: '#ec4899',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  hotelName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
  },
  period: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  summaryCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  summaryTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  summaryRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  summaryLabel: {
    fontSize: 14,
    color: '#666',
  },
  summaryValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  tableCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  tableTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  tableHeader: {
    flexDirection: 'row',
    paddingBottom: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  tableRow: {
    flexDirection: 'row',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
  },
  tableCell: {
    fontSize: 11,
    color: '#555',
  },
  channelCol: {
    width: '30%',
  },
  bookingsCol: {
    width: '15%',
    textAlign: 'right',
  },
  revenueCol: {
    width: '20%',
    textAlign: 'right',
  },
  commissionCol: {
    width: '15%',
    textAlign: 'right',
  },
  netCol: {
    width: '20%',
    textAlign: 'right',
  },
  exportButton: {
    backgroundColor: '#10b981',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  exportButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
});