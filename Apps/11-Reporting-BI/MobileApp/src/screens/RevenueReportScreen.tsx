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
import { RevenueReport, DailyRevenue } from '../types/reporting';
import { RevenueChart } from '../components/RevenueChart';

export const RevenueReportScreen = ({ route }: any) => {
  const { hotelId, startDate, endDate } = route.params;
  const [report, setReport] = useState<RevenueReport | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadReport();
  }, []);

  const loadReport = async () => {
    setLoading(true);
    try {
      const data = await reportApi.getRevenueReport(hotelId, startDate, endDate);
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
        reportType: 'Revenue',
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
          <Text style={styles.summaryLabel}>Toplam Gelir</Text>
          <Text style={styles.summaryValue}>€{report.totalRevenue.toLocaleString()}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Oda Geliri</Text>
          <Text style={styles.summaryValue}>€{report.totalRoomRevenue.toLocaleString()}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>F&B Geliri</Text>
          <Text style={styles.summaryValue}>€{report.totalFBRevenue.toLocaleString()}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>ADR (Ort. Günlük Fiyat)</Text>
          <Text style={styles.summaryValue}>€{report.averageDailyRate.toFixed(2)}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>RevPAR</Text>
          <Text style={styles.summaryValue}>€{report.revPAR.toFixed(2)}</Text>
        </View>
      </View>

      <RevenueChart data={report.dailyData} />

      <View style={styles.tableCard}>
        <Text style={styles.tableTitle}>Günlük Detay</Text>
        <View style={styles.tableHeader}>
          <Text style={[styles.tableCell, styles.dateCol]}>Tarih</Text>
          <Text style={[styles.tableCell, styles.revenueCol]}>Gelir</Text>
          <Text style={[styles.tableCell, styles.roomCol]}>Oda</Text>
          <Text style={[styles.tableCell, styles.fbCol]}>F&B</Text>
        </View>
        {report.dailyData.slice(0, 10).map((item: DailyRevenue, index: number) => (
          <View key={index} style={styles.tableRow}>
            <Text style={[styles.tableCell, styles.dateCol]}>
              {new Date(item.date).getDate()}/{new Date(item.date).getMonth() + 1}
            </Text>
            <Text style={[styles.tableCell, styles.revenueCol]}>€{item.revenue.toFixed(0)}</Text>
            <Text style={[styles.tableCell, styles.roomCol]}>€{item.roomRevenue.toFixed(0)}</Text>
            <Text style={[styles.tableCell, styles.fbCol]}>€{item.fbRevenue.toFixed(0)}</Text>
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
    backgroundColor: '#007AFF',
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
    fontSize: 12,
    color: '#555',
  },
  dateCol: {
    width: '25%',
  },
  revenueCol: {
    width: '25%',
    textAlign: 'right',
  },
  roomCol: {
    width: '25%',
    textAlign: 'right',
  },
  fbCol: {
    width: '25%',
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